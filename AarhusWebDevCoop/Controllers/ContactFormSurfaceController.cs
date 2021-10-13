using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web.Mvc;
using AarhusWebDevCoop.ViewModels;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace AarhusWebDevCoop.Controllers
{
    // ContactFormSurface
    public class ContactFormSurfaceController : SurfaceController
    {
        //GET: Default
        public ActionResult Index()
        {
            return PartialView("ContactForm", new ContactForm());
        }

        //POST: Default
        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
            //Prevent attempting to send email if model state is invalid
            if (!ModelState.IsValid) {
                return CurrentUmbracoPage();
            }

            #region Setup for sending contact messages by email (SMTP)
            /*
            MailAddress from = new MailAddress("admin@aarhuswebdevcoop.dk", "Admin");
            MailAddress to = new MailAddress(model.Email);
            MailMessage message = new MailMessage(from, to);

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp-relay-.sendinblue.com";
                smtp.Port = 587;
                //I'm not putting my email, nor my password on github, sorry. ¯\_(ツ)_/¯
                smtp.Credentials = new NetworkCredential("USERNAME", "PASSWORD");

                //smtp.Send(message);
            }
            */
            #endregion Setup for sending contact messages by email (SMTP)

            #region Save contact message as a ContactMessage document.
            GuidUdi currentPageUdi = new GuidUdi(CurrentPage.ContentType.ItemType.ToString(), CurrentPage.Key);

            IContent msg = Services.ContentService.CreateContent(model.Subject, currentPageUdi, "contactMessage");
            msg.SetValue("messageName", model.Name);
            msg.SetValue("messageEmail", model.Email);
            msg.SetValue("messageSubject", model.Subject);
            msg.SetValue("messageContent", model.Message);

            Services.ContentService.Save(msg);
            #endregion Save contact message as a ContactMessage document.

            TempData["messageSent"] = true;
            model = new ContactForm();
            return RedirectToCurrentUmbracoPage();
        }
    }
}