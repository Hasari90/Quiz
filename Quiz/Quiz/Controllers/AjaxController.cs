using Quiz.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quiz.Controllers
{
    public class AjaxController : Controller
    {
        private GalleryService _galleryService = new GalleryService();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetQuestion()
        {
            var question = _galleryService.GetQuestion();
            return Json(question, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckAnswer(int answerId = -1)
        {
            if (Session["GalleryId"] != null && Session["GalleryId"] is int)
            {
                var galleryId = (int)Session["GalleryId"];
                var answer = _galleryService.CheckAnswer(answerId, galleryId);
                var question = _galleryService.GetQuestion();
                question.Result = answer.NameOfCorrectAnswer;
                question.IsCorrect = answer.IsCorrect;
                return Json(question, JsonRequestBehavior.DenyGet);
            }
            else
                return Json(_galleryService.GetQuestion(), JsonRequestBehavior.DenyGet);
        }
    }
}