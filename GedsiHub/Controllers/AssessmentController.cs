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

        public AssessmentController(
            IExam<Exam> examService,
            IQuestion<Question> questionService,
            IChoice<Choice> choiceService,
            IResult<QuizResult> resultService,
            ILogger<AssessmentController> logger,
            IAnswerService answerService)

        {
            _examService = examService;
            _questionService = questionService;
            _choiceService = choiceService;
            _resultService = resultService;
            _logger = logger;
            _answerService = answerService;
        }

        // GET: Assessment/CreateOrEdit
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOrEdit(int moduleId, int? examId = null)
        {
            // Initialize a new model for the view
            var model = new QuizViewModel
            {
                ModuleId = moduleId
            };

            if (examId.HasValue)
            {
                // If examId is provided, fetch the existing exam and populate the model
                var exam = await _examService.GetExam(examId.Value);
                if (exam == null)
                {
                    return NotFound();
                }

                model.ExamID = exam.ExamID;
                model.Name = exam.Name;
                model.FullMarks = exam.FullMarks;
                model.Duration = exam.Duration;

                // Populate questions and choices for edit view
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

            // Return the populated model to the view (for either create or edit)
            return View(model);
        }


        // POST: Assessment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOrEdit(QuizViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Exam exam;

                    // Determine if this is a new quiz or an update
                    if (model.ExamID.HasValue)
                    {
                        // Fetch the existing exam for editing
                        exam = await _examService.GetExam(model.ExamID.Value);
                        if (exam == null) return NotFound();

                        // Update existing exam properties
                        exam.Name = model.Name;
                        exam.FullMarks = model.FullMarks;
                        exam.Duration = model.Duration;
                        exam.ModifiedOn = DateTime.Now;
                        exam.ModifiedBy = User.Identity.Name;
                        await _examService.UpdateExam(exam);
                    }
                    else
                    {
                        // Create a new exam
                        exam = new Exam
                        {
                            Name = model.Name,
                            FullMarks = model.FullMarks,
                            Duration = model.Duration,
                            ModuleId = model.ModuleId,
                            CreatedOn = DateTime.Now,
                            CreatedBy = User.Identity.Name,
                            ModifiedBy = User.Identity.Name
                        };
                        await _examService.AddExam(exam);
                    }

                    // Loop through questions and choices
                    foreach (var qvm in model.Questions)
                    {
                        Question question;

                        if (qvm.QuestionID.HasValue)
                        {
                            // Fetch and update the existing question
                            question = await _questionService.GetQuestion(qvm.QuestionID.Value);
                            if (question == null) continue; // Skip if not found

                            question.DisplayText = qvm.DisplayText;
                            question.QuestionType = qvm.QuestionType;
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
                                QuestionType = qvm.QuestionType,
                                CreatedOn = DateTime.Now,
                                CreatedBy = User.Identity.Name,
                                ModifiedBy = User.Identity.Name
                            };
                            await _questionService.AddQuestion(question);
                        }

                        // Handle Choices
                        for (int i = 0; i < qvm.Choices.Count; i++)
                        {
                            var choiceVm = qvm.Choices[i];
                            Choice choice;

                            if (choiceVm.ChoiceID.HasValue)
                            {
                                // Fetch and update the existing choice
                                choice = await _choiceService.GetChoice(choiceVm.ChoiceID.Value);
                                if (choice == null) continue; // Skip if not found

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

                            if (i == qvm.CorrectChoice) // If this choice is marked as correct in the form
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
                }
            }
            return View(model);
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

            var qna = await _questionService.GetQuestionList(exam.ExamID);

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

            foreach (var response in responses)
            {
                // Find the correct answer for this question
                var correctAnswer = await _answerService.GetAnswerByQuestionAndChoice(response.QuestionID, response.SelectedOption);

                // Determine if the selected option is correct by checking against `IsCorrect` in `Answer`
                var isCorrect = correctAnswer != null && correctAnswer.IsCorrect;

                var result = new QuizResult
                {
                    SessionID = HttpContext.Session.Id,
                    UserId = userId,
                    ExamID = response.ExamID,
                    QuestionID = response.QuestionID,
                    AnswerID = response.AnswerID,
                    SelectedOptionID = response.SelectedOption,
                    IsCorrent = isCorrect,  // Updated logic for correctness
                    CreatedOn = DateTime.Now,
                    CreatedBy = User.Identity.Name,
                    ModifiedBy = User.Identity.Name
                };

                results.Add(result);
            }

            await _resultService.AddResult(results);

            return RedirectToAction("Result", new { sessionId = HttpContext.Session.Id });
        }

        // GET: Assessment/Result
        public IActionResult Result(string sessionId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var results = _resultService.Search(r => r.SessionID == sessionId && r.UserId == userId);

            var totalQuestions = results.Count();
            var correctAnswers = results.Count(r => r.IsCorrent);

            var model = new QuizResultViewModel
            {
                TotalQuestions = totalQuestions,
                CorrectAnswers = correctAnswers,
                Score = totalQuestions > 0 ? ((double)correctAnswers / totalQuestions) * 100 : 0
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
