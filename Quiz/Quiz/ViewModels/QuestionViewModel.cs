using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.ViewModels
{
    public class QuestionViewModel
    {
        public string ImagePath { get; set; }
        public List<Answer> Answers { get; set; }
        public string Result { get; set; }
        public bool IsCorrect { get; set; }

        public class Answer
        {
            public int AnswerId { get; set; }
            public string Name { get; set; }
        }

        public QuestionViewModel()
        {
            Answers = new List<Answer>();
        }
    }
}