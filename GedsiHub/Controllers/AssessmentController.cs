using Microsoft.AspNetCore.Mvc;
using GedsiHub.Models;
using GedsiHub.Models.Quiz;
using GedsiHub.Services.Interfaces;
using GedsiHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Security.Claims;
using GedsiHub.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GedsiHub.Data;
using System.IO;

namespace GedsiHub.Controllers
{
    [Authorize]
    public class AssessmentController : Controller
    {
        private readonly IExam<Exam> _examService;
        private readonly IQuestion<Question> _questionService;
        private readonly IChoice<Choice> _choiceService;
        private readonly IResult<QuizResult> _resultService;
        private readonly IAnswerService _answerService;
        private readonly ILogger<AssessmentController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly CertificateService _certificateService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ExcelParserService _excelParserService;

        public AssessmentController(
            IExam<Exam> examService,
            IQuestion<Question> questionService,
            IChoice<Choice> choiceService,
            IResult<QuizResult> resultService,
            ILogger<AssessmentController> logger,
            IAnswerService answerService,
            ApplicationDbContext context,
            CertificateService certificateService,
            UserManager<ApplicationUser> userManager,
            ExcelParserService excelParserService)

        {
            _examService = examService;
            _questionService = questionService;
            _choiceService = choiceService;
            _resultService = resultService;
            _logger = logger;
            _answerService = answerService;
            _context = context;
            _certificateService = certificateService;
            _userManager = userManager;
            _excelParserService = excelParserService;
        }

        // GET: Assessment/CreateOrEdit
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOrEdit(int moduleId, int? examId = null)
        {
            var model = new QuizViewModel { ModuleId = moduleId };
            if (examId.HasValue)
            {
                var exam = await _examService.GetExam(examId.Value);
                if (exam == null) return NotFound();

                model.ExamID = exam.ExamID;
                model.Name = exam.Name;
                model.NumberOfQuestions = exam.NumberOfQuestions;
                model.ShuffleQuestions = exam.ShuffleQuestions;
                model.H5PEmbedCode = exam.H5PEmbedCode; // Load H5P embed code

                var questions = await _questionService.GetQuestionsByExamId(exam.ExamID);
                model.Questions = questions.Select(q => new QuestionViewModel
                {
                    QuestionID = q.QuestionID,
                    DisplayText = q.DisplayText,
                    QuestionType = q.QuestionType,  // Enum directly
                    Choices = _choiceService.GetChoicesByQuestion(q.QuestionID).Result.Select(c => new ChoiceViewModel
                    {
                        ChoiceID = c.ChoiceID,
                        DisplayText = c.DisplayText,
                        IsCorrect = _answerService.GetAnswerByQuestionAndChoice(q.QuestionID, c.ChoiceID).Result?.IsCorrect ?? false
                    }).ToList()
                }).ToList();
            }
            return View(model);
        }

        // POST: Assessment/CreateOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOrEdit(QuizViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogError("Validation error: {Error}", error.ErrorMessage);
                    }
                }
                return View(model);
            }

            try
            {
                Exam exam;

                // Check if this is an update or a new exam creation
                if (model.ExamID.HasValue)
                {
                    // Editing an existing exam
                    exam = await _examService.GetExam(model.ExamID.Value);
                    if (exam == null) return NotFound();

                    // Update the properties of the existing exam
                    exam.Name = model.Name;
                    exam.ModifiedOn = DateTime.Now;
                    exam.ModifiedBy = User.Identity.Name;
                    exam.NumberOfQuestions = model.NumberOfQuestions; // Correctly set number of questions for editing
                    exam.ShuffleQuestions = model.ShuffleQuestions;
                    exam.H5PEmbedCode = model.H5PEmbedCode; // Save H5P embed code

                    await _examService.UpdateExam(exam);
                }
                else
                {
                    // Creating a new exam
                    exam = new Exam
                    {
                        Name = model.Name,
                        ModuleId = model.ModuleId,
                        CreatedOn = DateTime.Now,
                        CreatedBy = User.Identity.Name,
                        ModifiedBy = User.Identity.Name,
                        NumberOfQuestions = model.NumberOfQuestions, // FIX: Set NumberOfQuestions during creation
                        ShuffleQuestions = model.ShuffleQuestions,
                        H5PEmbedCode = model.H5PEmbedCode // Save H5P embed code
                    };
                    await _examService.AddExam(exam);
                }

                // Loop through questions and choices
                foreach (var qvm in model.Questions)
                {
                    Question question;

                    if (qvm.QuestionID.HasValue)
                    {
                        // Update the existing question
                        question = await _questionService.GetQuestion(qvm.QuestionID.Value);
                        if (question == null) continue;

                        question.DisplayText = qvm.DisplayText;
                        question.QuestionType = (QuestionTypeEnum)qvm.QuestionType;
                        question.ModifiedOn = DateTime.Now;
                        await _questionService.UpdateQuestion(question);
                    }
                    else
                    {
                        // Create a new question
                        question = new Question
                        {
                            ExamID = exam.ExamID,
                            DisplayText = qvm.DisplayText,
                            QuestionType = (QuestionTypeEnum)qvm.QuestionType,
                            CreatedOn = DateTime.Now,
                            CreatedBy = User.Identity.Name,
                            ModifiedBy = User.Identity.Name
                        };
                        await _questionService.AddQuestion(question);
                    }

                    // Handle choices
                    for (int i = 0; i < qvm.Choices.Count; i++)
                    {
                        var choiceVm = qvm.Choices[i];
                        Choice choice;

                        if (choiceVm.ChoiceID.HasValue)
                        {
                            // Update the existing choice
                            choice = await _choiceService.GetChoice(choiceVm.ChoiceID.Value);
                            if (choice == null) continue;

                            choice.DisplayText = choiceVm.DisplayText;
                            choice.ModifiedOn = DateTime.Now;
                            await _choiceService.UpdateChoice(choice);
                        }
                        else
                        {
                            // Create a new choice
                            choice = new Choice
                            {
                                QuestionID = question.QuestionID,
                                DisplayText = choiceVm.DisplayText,
                                CreatedOn = DateTime.Now,
                                CreatedBy = User.Identity.Name,
                                ModifiedBy = User.Identity.Name
                            };
                            await _choiceService.AddChoice(choice);
                        }

                        // Handle correct answer marking
                        var answer = await _answerService.GetAnswerByQuestionAndChoice(question.QuestionID, choice.ChoiceID);
                        if (i == qvm.CorrectChoice)
                        {
                            if (answer == null)
                            {
                                // Create new correct answer entry if it doesn’t exist
                                answer = new Answer
                                {
                                    QuestionID = question.QuestionID,
                                    ChoiceID = choice.ChoiceID,
                                    DisplayText = choiceVm.DisplayText,
                                    IsCorrect = true,
                                    CreatedOn = DateTime.Now,
                                    CreatedBy = User.Identity.Name,
                                    ModifiedBy = User.Identity.Name
                                };
                                await _answerService.AddAnswer(answer);
                            }
                            else
                            {
                                // Update existing answer as correct
                                answer.IsCorrect = true;
                                answer.ModifiedOn = DateTime.Now;
                                await _answerService.UpdateAnswer(answer);
                            }
                        }
                        else if (answer != null)
                        {
                            // Unmark as correct if it’s not the chosen answer
                            answer.IsCorrect = false;
                            await _answerService.UpdateAnswer(answer);
                        }
                    }
                }

                return RedirectToAction("Details", "Module", new { id = model.ModuleId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the quiz.");
                ModelState.AddModelError(string.Empty, "An error occurred. Please try again.");
                return View(model);
            }
        }

        public async Task<IActionResult> SetCorrectAnswer(int questionId, int choiceId)
        {
            var answer = await _answerService.GetAnswerByQuestionAndChoice(questionId, choiceId);
            if (answer == null)
            {
                answer = new Answer
                {
                    QuestionID = questionId,
                    ChoiceID = choiceId,
                    IsCorrect = true,
                    CreatedBy = User.Identity.Name,
                    CreatedOn = DateTime.Now
                };
                await _answerService.AddAnswer(answer);
            }
            else
            {
                answer.IsCorrect = true;
                answer.ModifiedBy = User.Identity.Name;
                answer.ModifiedOn = DateTime.Now;
                await _answerService.UpdateAnswer(answer);
            }

            return Ok();
        }

        // GET: Assessment/Take/5
        public async Task<IActionResult> Take(int id)
        {
            var exam = await _examService.GetExam(id);
            if (exam == null)
            {
                return NotFound();
            }

            // Fetch all questions for the exam
            var allQuestions = await _questionService.GetQuestionsByExamId(exam.ExamID);

            // Shuffle and limit questions if needed
            var selectedQuestions = allQuestions.ToList();
            if (exam.ShuffleQuestions)
            {
                selectedQuestions = selectedQuestions.OrderBy(_ => Guid.NewGuid()).ToList();
            }

            // Take only the specified number of questions
            selectedQuestions = selectedQuestions.Take(exam.NumberOfQuestions).ToList();

            // Convert to view model and shuffle choices if needed
            var qna = new QnA
            {
                ExamID = exam.ExamID,
                Exam = exam.Name,
                H5PEmbedCode = exam.H5PEmbedCode, // Pass the embed code
                questions = selectedQuestions.Select(q => new QuestionDetails
                {
                    QuestionID = q.QuestionID,
                    QuestionText = q.DisplayText,
                    QuestionType = (int)q.QuestionType,
                    options = _choiceService.GetChoicesByQuestion(q.QuestionID).Result
                              .Select(c => new OptionDetails
                              {
                                  OptionID = c.ChoiceID,
                                  Option = c.DisplayText
                              }).ToList()
                }).ToList()
            };


            return View(qna);
        }


        // POST: Assessment/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(List<UserResponse> responses)
        {
            if (responses == null || !responses.Any())
            {
                ModelState.AddModelError("", "No responses received.");
                return View("Error");
            }

            var results = new List<QuizResult>();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int correctAnswers = 0;

            // Retrieve the ExamID from the first response
            int examId = responses.First().ExamID;

            // Fetch the exam to get the associated ModuleId
            var exam = await _examService.GetExam(examId);
            if (exam == null)
            {
                ModelState.AddModelError("", "Exam not found.");
                return View("Error");
            }

            foreach (var response in responses)
            {
                // Fetch the correct answer for the question
                var correctAnswer = await _answerService.GetCorrectAnswerByQuestionId(response.QuestionID);

                // Determine if the selected option is correct
                var isCorrect = correctAnswer != null && correctAnswer.ChoiceID == response.SelectedOption;

                if (isCorrect)
                {
                    correctAnswers++;
                }

                var result = new QuizResult
                {
                    SessionID = HttpContext.Session.Id,
                    UserId = userId,
                    ExamID = response.ExamID,
                    QuestionID = response.QuestionID,
                    AnswerID = correctAnswer != null ? correctAnswer.Sl_No : (int?)null, // Assign the correct AnswerID
                    SelectedOptionID = response.SelectedOption,
                    IsCorrect = isCorrect,
                    CreatedOn = DateTime.Now,
                    CreatedBy = User.Identity.Name,
                    ModifiedBy = User.Identity.Name
                };

                results.Add(result);
            }


            await _resultService.AddResult(results);

            // Calculate the user's score
            var totalQuestions = results.Count;
            var score = totalQuestions > 0 ? ((double)correctAnswers / totalQuestions) * 100 : 0;
            bool passed = score >= 60;

            if (passed)
            {
                // Now passing the correct ModuleId from the exam object
                await MarkProgressAndGenerateCertificate(userId, exam.ModuleId);
                return RedirectToAction("Result", new { sessionId = HttpContext.Session.Id, passed });
            }
            else
            {
                return RedirectToAction("Details", "Module", new { id = exam.ModuleId }); // Redirect using the correct ModuleId
            }
        }

        private async Task MarkProgressAndGenerateCertificate(string userId, int moduleId)
        {
            // Update UserProgress as complete
            var userProgress = await _context.UserProgresses
                .FirstOrDefaultAsync(up => up.UserId == userId && up.ModuleId == moduleId);

            if (userProgress == null)
            {
                userProgress = new UserProgress
                {
                    UserId = userId,
                    ModuleId = moduleId,
                    ProgressPercentage = 100,
                    IsCompleted = true,
                    StreakCount = 1, // Initialize as needed
                    CompletedLessonIds = "" // Initialize as needed
                };
                _context.UserProgresses.Add(userProgress);
            }
            else
            {
                userProgress.IsCompleted = true;
                userProgress.ProgressPercentage = 100;
            }

            await _context.SaveChangesAsync();

            // Generate and store certificate
            _logger.LogInformation($"Generating certificate for UserID: {userId}, ModuleID: {moduleId}");
            var pdfBytes = await _certificateService.GenerateAndStoreCertificateAsync(userId, moduleId);

            // Optionally, send an email with the certificate to the user
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                await _certificateService.SendCertificateEmail(user.Email, "Your Certificate of Completion", pdfBytes, "certificate.pdf");
            }
        }

        // GET: Assessment/Result
        public async Task<IActionResult> Result(string sessionId, bool passed)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var results = _resultService.Search(r => r.SessionID == sessionId && r.UserId == userId);

            var totalQuestions = results.Count();
            var correctAnswers = results.Count(r => r.IsCorrect);

            // Retrieve the ExamID from the first result (assuming the results contain this info)
            var firstResult = results.FirstOrDefault();
            if (firstResult == null)
            {
                return NotFound("No results found for this session."); // Handle case where there are no results
            }

            int examId = firstResult.ExamID; // Use ExamID from the first result

            // Retrieve the exam to get the associated ModuleId
            var exam = await _examService.GetExam(examId); // Make sure to await the method
            if (exam == null)
            {
                ModelState.AddModelError("", "Exam not found.");
                return View("Error"); // Handle the error appropriately
            }

            // Get the ModuleId from the exam
            int moduleId = exam.ModuleId; // Use the correct property to get ModuleId

            var model = new QuizResultViewModel
            {
                TotalQuestions = totalQuestions,
                CorrectAnswers = correctAnswers,
                Score = totalQuestions > 0 ? ((double)correctAnswers / totalQuestions) * 100 : 0,
                Passed = passed,  // Pass/fail status
                ModuleId = moduleId // Set the correct ModuleId here
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadQuizExcel(int moduleId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["FileError"] = "Please upload a valid Excel file.";
                return RedirectToAction("CreateOrEdit", new { moduleId });
            }

            try
            {
                var examsData = await _excelParserService.ParseExcelToQuizAsync(file);

                if (examsData == null || !examsData.Any())
                {
                    TempData["ParseError"] = "The uploaded file format is invalid or contains no data.";
                    return RedirectToAction("CreateOrEdit", new { moduleId });
                }

                foreach (var exam in examsData)
                {
                    await _examService.AddExam(exam);
                }

                TempData["Success"] = "Quiz uploaded successfully.";
                return RedirectToAction("Details", "Module", new { id = moduleId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing uploaded file.");
                TempData["Error"] = "An error occurred while processing the file. Please try again.";
                return RedirectToAction("CreateOrEdit", new { moduleId });
            }
        }



        // GET: Assessment/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var exam = await _examService.GetExam(id);
            if (exam == null)
            {
                return NotFound();
            }

            var model = new QuizViewModel
            {
                ExamID = exam.ExamID,
                Name = exam.Name,
                ModuleId = exam.ModuleId
            };

            return View(model);
        }

        // POST: Assessment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exam = await _examService.GetExam(id);
            if (exam == null)
            {
                return NotFound();
            }

            await _examService.DeleteExam(exam);

            // Optionally, delete related questions and choices

            return RedirectToAction("Details", "Module", new { id = exam.ModuleId });
        }
    }
}
