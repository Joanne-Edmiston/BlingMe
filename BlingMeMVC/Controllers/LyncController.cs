using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Outlook;
using Microsoft.Lync.Model;
using LyncConvo = Microsoft.Lync.Model.Conversation;
using Microsoft.Lync.Model.Extensibility;

namespace BlingMeMVC.Controllers
{
    using System.Web.Mvc;

    using BlingMeMVC;

    using Microsoft.Ajax.Utilities;

    public class LyncController : Controller
    {
        //
        // GET: /Lync/

        public void NewMeeting(string emails)
        {
            var emailStrings = emails.Split(';');
            var lyncItems = new MailItems()
            {
                Recipients = (from e in emailStrings where !e.IsNullOrWhiteSpace() select e).ToList()
            };
            LyncCode.SendAppointment(lyncItems);
        }

        public void Start(string email)
        {
            var lyncItems = new MailItems()
            {
                Recipients = new List<string>() { email }
            };
            LyncCode.StartLyncConvo(lyncItems);
        }

        public void StartMulti(string emails)
        {
            var emailStrings = emails.Split(';');
            var lyncItems = new MailItems()
            {
                Recipients = (from e in emailStrings where !e.IsNullOrWhiteSpace() select e).ToList()
            };
            LyncCode.StartLyncConvo(lyncItems);
        }

    }
}
