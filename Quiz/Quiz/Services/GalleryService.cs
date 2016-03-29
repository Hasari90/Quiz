using Quiz.Models;
using Quiz.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Quiz.Services
{
    public class GalleryService
    {
        private IList<Gallery> _galleryList;
        private const string _xmlPath = "~/App_Data/Galleries.xml";

        public GalleryService()
        {
            _galleryList = new List<Gallery>();
            LoadData();
        }

        private void LoadData()
        {
            XElement root = XElement.Load(HttpContext.Current.Server.MapPath(_xmlPath));

            _galleryList = root.Elements("gallery").Select(p => new Gallery()
            {
                Id = int.Parse(p.Attribute("id").Value),
                Name = p.Attribute("name").Value,
                Images = p.Elements("image").Select(i => string.Format("/Images{0}", i.Attribute("src").Value)).ToList()
            }).ToList();
        }

        public QuestionViewModel GetQuestion()
        {
            var question = new QuestionViewModel();
            var random = new Random(Guid.NewGuid().GetHashCode());
            // wybranie losowego pytania
            var gallery = _galleryList[random.Next(0, _galleryList.Count - 1)];
            // wybranie losowego obrazka z galerii
            question.ImagePath = gallery.Images.ElementAtOrDefault(random.Next(0, gallery.Images.Count - 1));
            // dodanie poprawnej odpowiedzi do listy odpowiedzi
            question.Answers.Add(new QuestionViewModel.Answer
            {
                AnswerId = gallery.Id,
                Name = gallery.Name
            });
            // liczba brakujących odpowiedzi do pytania
            var answerToTake = random.Next(3, 6) - 1;
            var indexOfCorrectAnswer = _galleryList.IndexOf(gallery);
            // losowe wybranie niepoprawnych odpowiedzi
            var randomFailAnswers = Enumerable.Range(0, _galleryList.Count)
                .Where(n => n != indexOfCorrectAnswer)
                .OrderBy(n => Guid.NewGuid())
                .Take(answerToTake)
                .OrderBy(n => n)
                .Select(n => new QuestionViewModel.Answer
                {
                    AnswerId = _galleryList[n].Id,
                    Name = _galleryList[n].Name
                }).ToList();
            // dodanie niepoprawnych odpowiedzi do pytania
            question.Answers.AddRange(randomFailAnswers);
            // losowe posortowanie odpowiedzi
            question.Answers = question.Answers.OrderBy(n => Guid.NewGuid()).ToList();
            // zapis id galerii w sesji - posłuży to do sprawdzenia poprawności odpowiedzi, gdy użytkownik
            // prześle odpowiedź
            HttpContext.Current.Session["GalleryId"] = gallery.Id;

            return question;
        }

        public CorrectAnswer CheckAnswer(int answerId, int galleryId)
        {
            var name = _galleryList.First(g => g.Id == galleryId).Name;
            var isCorrect = answerId == galleryId ? true : false;
            var answer = new CorrectAnswer
            {
                NameOfCorrectAnswer = name,
                IsCorrect = isCorrect
            };
            return answer;
        }

        public class CorrectAnswer
        {
            public string NameOfCorrectAnswer { get; set; }
            public bool IsCorrect { get; set; }
        }
    }
}