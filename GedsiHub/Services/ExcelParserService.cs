using OfficeOpenXml;
using GedsiHub.Models.Quiz;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GedsiHub.Services
{
    public class ExcelParserService
    {
        public async Task<List<Exam>> ParseExcelToQuizAsync(IFormFile file)
        {
            var exams = new List<Exam>();
            var questions = new List<Question>();
            var choices = new List<Choice>();
            var answers = new List<Answer>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var examSheet = package.Workbook.Worksheets["Exams"];
                    var questionSheet = package.Workbook.Worksheets["Questions"];
                    var choiceSheet = package.Workbook.Worksheets["Choices"];

                    if (examSheet == null || questionSheet == null || choiceSheet == null)
                        throw new Exception("Invalid Excel format. Required sheets are missing.");

                    // Parse Exams
                    for (int row = 2; row <= examSheet.Dimension.End.Row; row++)
                    {
                        if (string.IsNullOrWhiteSpace(examSheet.Cells[row, 1].Text))
                            continue; // Skip empty rows

                        var exam = new Exam
                        {
                            Name = examSheet.Cells[row, 1].Text.Trim(),
                            NumberOfQuestions = TryParseInt(examSheet.Cells[row, 2].Text, defaultValue: 0),
                            ShuffleQuestions = TryParseBool(examSheet.Cells[row, 3].Text, defaultValue: false),
                            CreatedOn = DateTime.Now,
                            CreatedBy = "Admin",
                            ModifiedBy = "Admin"
                        };

                        exams.Add(exam);
                    }

                    // Parse Questions
                    for (int row = 2; row <= questionSheet.Dimension.End.Row; row++)
                    {
                        if (string.IsNullOrWhiteSpace(questionSheet.Cells[row, 1].Text))
                            continue; // Skip empty rows

                        var question = new Question
                        {
                            ExamID = TryParseInt(questionSheet.Cells[row, 1].Text, defaultValue: 0),
                            DisplayText = questionSheet.Cells[row, 2].Text.Trim(),
                            QuestionType = (QuestionTypeEnum)TryParseInt(questionSheet.Cells[row, 3].Text, defaultValue: 0),
                            CreatedOn = DateTime.Now,
                            CreatedBy = "Admin",
                            ModifiedBy = "Admin"
                        };

                        questions.Add(question);
                    }

                    // Parse Choices and Answers
                    for (int row = 2; row <= choiceSheet.Dimension.End.Row; row++)
                    {
                        if (string.IsNullOrWhiteSpace(choiceSheet.Cells[row, 1].Text))
                            continue; // Skip empty rows

                        var questionId = TryParseInt(choiceSheet.Cells[row, 1].Text, defaultValue: 0);
                        var isCorrect = TryParseBool(choiceSheet.Cells[row, 3].Text, defaultValue: false);

                        var choice = new Choice
                        {
                            QuestionID = questionId,
                            DisplayText = choiceSheet.Cells[row, 2].Text.Trim(),
                            CreatedOn = DateTime.Now,
                            CreatedBy = "Admin",
                            ModifiedBy = "Admin"
                        };

                        choices.Add(choice);

                        if (isCorrect)
                        {
                            var answer = new Answer
                            {
                                QuestionID = questionId,
                                DisplayText = choice.DisplayText,
                                IsCorrect = true,
                                CreatedOn = DateTime.Now,
                                CreatedBy = "Admin",
                                ModifiedBy = "Admin"
                            };
                            answers.Add(answer);
                        }
                    }
                }
            }

            return exams;
        }

        // Helper methods for parsing with default values
        private int TryParseInt(string input, int defaultValue)
        {
            return int.TryParse(input, out int result) ? result : defaultValue;
        }

        private bool TryParseBool(string input, bool defaultValue)
        {
            return bool.TryParse(input, out bool result) ? result : defaultValue;
        }
    }
}
