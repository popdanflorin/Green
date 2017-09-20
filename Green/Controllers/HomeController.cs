using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mail;
using System.Web.Mvc;

namespace Green.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = ".";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public void SendEmail(string name, string phone, string email, string message)
        {
            try
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                mail.From = new MailAddress(email, name);
                mail.To.Add("raluca.brasoveanu@tecknoworks.com");
                mail.To.Add("ioanabianca.puscasu@tecknoworks.com");
                // mail.To.Add("adriana.moisil@tecknoworks.com");
                // mail.To.Add("florin.pop@tecknoworks.com");
                mail.IsBodyHtml = true;
                mail.Subject = "El Bacalao Q&A";
                mail.Body = message;
                mail.Priority = System.Net.Mail.MailPriority.High;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                //smtp.UseDefaultCredentials = true;
                smtp.Credentials = new System.Net.NetworkCredential("info.elbacalao", "Tecknoworker");
                smtp.EnableSsl = true;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}