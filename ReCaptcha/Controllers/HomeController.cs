using Newtonsoft.Json.Linq;
using ReCaptcha.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ReCaptcha.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ContactUs(ContactUs contact)
        {
            if (contact is null) return Json(new { success = false,error="Invalid Data" }, JsonRequestBehavior.AllowGet);

            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(err => err.Errors.Select(e => e.ErrorMessage)).ToList();

                return Json(new { success = false, error = error }, JsonRequestBehavior.AllowGet);
            }

            var recaptchaUrl = "https://www.google.com/recaptcha/api/siteverify";
            var secretKey = "6LdDS10pAAAAAO2nknXDIV0AdttE3LbLDG3-HWq9";

            var client = new HttpClient();
            var response = await client.PostAsync(recaptchaUrl, new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("secret",secretKey),
                new KeyValuePair<string, string>("response",contact.recapcha)

            }));
            var responseString = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseString);
            if (json["success"].Value<bool>() && json["score"].Value<double>() >= 0.5 && json["action"].Value<string>()== "contact_us")
            {
                //add functionality for contact persistence

                return Json(new {success=true}, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ThankYou() => View();

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}