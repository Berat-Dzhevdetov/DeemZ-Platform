namespace DeemZ.Services.FileService
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Aspose.Cells;
    using DeemZ.Data.Models;
    using DeemZ.Services.ExamServices;

    public class ExcelService : IExcelService
    {
        private const int maxSheetNameLength = 31;
        private Workbook wb;
        private List<Worksheet> sheets;
        private readonly IExamService examService;


        public ExcelService(IExamService examService)
        {
            wb = new();
            sheets = new();
            this.examService = examService;
        }

        public byte[] ReturnAsBytes()
        {
            var memoryStream = new MemoryStream();

            wb.Save(memoryStream, SaveFormat.Xlsx);

            memoryStream.Position = 0;

            return memoryStream.ToArray();
        }

        public byte[] ExportExam(string examId)
        {
            var exam = examService.GetExamById<Exam>(examId);

            var sheetIndex = 0;

            CreateSheet(sheetIndex);
            AutoFit(sheetIndex);

            var cellForHeading = GetCell(0, 2, sheetIndex);
            var cellStyle = cellForHeading.GetStyle();

            cellStyle = CenteredAndBold(cellStyle);

            cellForHeading.SetStyle(cellStyle);

            SetValueToCell(cellForHeading, $"{exam.Name} Preview");

            var headerNames = new string[]
            {
                "Question",
                "Right Answer",
                "Answer 2",
                "Answer 3",
                "Answer 4",
                "Points"
            };

            for (int i = 0; i < headerNames.Length; i++)
            {
                var currentCel = GetCell(1, i, sheetIndex);

                var style = currentCel.GetStyle();

                style = CenteredAndBold(style);

                currentCel.SetStyle(style);

                SetValueToCell(currentCel, headerNames[i]);
            }

            var index = 0;

            var questions = exam.Questions.ToList();

            for (int row = 2; index < questions.Count; row++, index++)
            {
                SetValueToCell(row, 0, questions[index].Text, sheetIndex);
                SetValueToCell(row, 1, questions[index].Answers.FirstOrDefault(x => x.IsCorrect).Text, sheetIndex);

                var wrongAnswers = questions[index].Answers.Where(x => !x.IsCorrect).ToList();

                var currentWrongAnswerIndex = 0;

                for (int i = 2; currentWrongAnswerIndex < wrongAnswers.Count; i++, currentWrongAnswerIndex++)
                {
                    SetValueToCell(row, i, wrongAnswers[currentWrongAnswerIndex].Text, sheetIndex);
                }

                SetValueToCell(row, 5, questions[index].Points, sheetIndex);
            }

            var bytes = ReturnAsBytes();

            return bytes;
        }

        //creates new sheet with given name and index
        //the index should be saved so you can open the page
        //again later
        private void CreateSheet(int index, string name = null)
        {
            var sheet = wb.Worksheets[index];

            sheet.Name = name == null || name.Length >= maxSheetNameLength ? $"Sheet{index}" : name;

            sheets.Add(sheet);
        }

        private void SetValueToCell<T>(int row, int column, T value, int sheetIndex)
        {
            var cell = GetCell(row, column, sheetIndex);

            cell.PutValue(value);
        }

        private void SetValueToCell<T>(Cell cell, T value)
        {
            cell.PutValue(value);
        }

        private Cell GetCell(int row, int column, int sheetIndex) => sheets[sheetIndex].Cells[row, column];

        private Style CenteredAndBold(Style style)
        {
            style = Centered(style);
            style = Bold(style);
            return style;
        }

        private Style Centered(Style style, bool verical = true, bool horizontal = true)
        {
            if (verical) style.VerticalAlignment = TextAlignmentType.Center;
            else style.VerticalAlignment = TextAlignmentType.Left;

            if (horizontal) style.HorizontalAlignment = TextAlignmentType.Center;
            else style.HorizontalAlignment = TextAlignmentType.Left;

            return style;
        }

        private Style Bold(Style style, bool isBold = true)
        {
            style.Font.IsBold = isBold;
            return style;
        }

        private void AutoFit(int sheetIndex)
        {
            sheets[sheetIndex].AutoFitColumns();
            sheets[sheetIndex].AutoFitRows();
        }
    }
}