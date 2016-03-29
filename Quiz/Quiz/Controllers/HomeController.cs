using Quiz.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quiz.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private GalleryService _galleryService = new GalleryService();

        [HttpGet]
        public ActionResult Index()
        {
            var question = _galleryService.GetQuestion();
            return View(question);
        }

        [HttpPost]
        public ActionResult Index(int answerId = -1)
        {
            if (Session["GalleryId"] != null && Session["GalleryId"] is int)
            {
                var galleryId = (int)Session["GalleryId"];
                var answer = _galleryService.CheckAnswer(answerId, galleryId);
                var question = _galleryService.GetQuestion();
                question.Result = answer.NameOfCorrectAnswer;
                question.IsCorrect = answer.IsCorrect;
                return View(question);
            }
            else
                return View(_galleryService.GetQuestion());
        }
    }
}