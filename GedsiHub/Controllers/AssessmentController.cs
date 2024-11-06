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

        public AssessmentController(
            IExam<Exam> examService,
            IQuestion<Question> questionService,
            IChoice<Choice> choiceService,
            IResult<QuizResult> resultService,
            ILogger<AssessmentController> logger,
            IAnswerService answerService,
            ApplicationDbContext context,
            CertificateService certificateService,
            UserManager<ApplicationUser> userManager)

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

                var questions = await _questionService.GetQuestionsByExamId(exam.ExamID);
                model.Questions = questions.Select(q => new QuestionViewModel
                {
                    QuestionID = q.QuestionID,
                    DisplayText = q.DisplayText,
                    QuestionType = q.QuestionType,
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
                if (model.ExamID.HasValue)
                {
                    exam = await _examService.GetExam(model.ExamID.Value);
                    if (exam == null) return NotFound();

                    exam.Name = model.Name;
                    exam.ModifiedOn = DateTime.Now;
                    exam.ModifiedBy = User.Identity.Name;
                    exam.NumberOfQuestions = model.NumberOfQuestions;
                    exam.ShuffleQuestions = model.ShuffleQuestions;

                    await _examService.UpdateExam(exam);
                }
                else
                {
                    exam = new Exam
                    {
                        Name = model.Name,
                        ModuleId = model.ModuleId,
                        CreatedOn = DateTime.Now,
                        CreatedBy = User.Identity.Name,
                        ModifiedBy = User.Identity.Name
                    };
                    await _examService.AddExam(exam);
                }

                foreach (var qvm in model.Questions)
                {
                    Question question;
                    if (qvm.QuestionID.HasValue)
                    {
                        question = await _questionService.GetQuestion(qvm.QuestionID.Value);
                        if (question == null) continue;

                        question.DisplayText = qvm.DisplayText;
                        question.QuestionType = qvm.QuestionType;
                        question.ModifiedOn = DateTime.Now;
                        await _questionService.UpdateQuestion(question);
                    }
                    else
                    {
                        question = new Question
                        {
                            ExamID = exam.ExamID,
                            DisplayText = qvm.DisplayText,
                            QuestionType = qvm.QuestionType,
                            CreatedOn = DateTime.Now,
                            CreatedBy = User.Identity.Name,
                            ModifiedBy = User.Identity.Name
                        };
                        await _questionService.AddQuestion(question);
                    }

                    for (int i = 0; i < qvm.Choices.Count; i++)
                    {
                        var choiceVm = qvm.Choices[i];
                        Choice choice;

                        if (choiceVm.ChoiceID.HasValue)
                        {
                            choice = await _choiceService.GetChoice(choiceVm.ChoiceID.Value);
                            if (choice == null) continue;

                            choice.DisplayText = choiceVm.DisplayText;
                            choice.ModifiedOn = DateTime.Now;
                            await _choiceService.UpdateChoice(choice);
                        }
                        else
                        {
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

                        var answer = await _answerService.GetAnswerByQuestionAndChoice(question.QuestionID, choice.ChoiceID);
                        if (i == qvm.CorrectChoice)
                        {
                            if (answer == null)
                            {
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
                                answer.IsCorrect = true;
                                answer.ModifiedOn = DateTime.Now;
                                await _answerService.UpdateAnswer(answer);
                            }
                        }
                        else if (answer != null)
                        {
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
                questions = selectedQuestions.Select(q => new QuestionDetails
                {
                    QuestionID = q.QuestionID,
                    QuestionText = q.DisplayText,
                    QuestionType = q.QuestionType,
                    options = _choiceService.GetChoicesByQuestion(q.QuestionID).Result
                              .OrderBy(_ => Guid.NewGuid())  // Shuffle choices
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
                var correctAnswer = await _answerService.GetAnswerByQuestionAndChoice(response.QuestionID, response.SelectedOption);
                var isCorrect = correctAnswer != null && correctAnswer.IsCorrect;

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
                    AnswerID = response.AnswerID,
                    SelectedOptionID = response.SelectedOption,
                    IsCorrent = isCorrect,
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

            if (userProgress != null)
            {
                userProgress.IsCompleted = true;
                userProgress.ProgressPercentage = 100; // Mark as fully complete
                await _context.SaveChangesAsync();
            }

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
            var correctAnswers = results.Count(r => r.IsCorrent);

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
