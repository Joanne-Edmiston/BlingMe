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

namespace BlingMeMVC
{
    public class LyncCode
    {
        static void Main(string[] args)
        {
            MailItems mailItems = new MailItems()
            {
                Recipients = new List<string>()
                        {
                            "Tomlinson",
                            "Hannah Williams"
                        }
            };

            MailItems lyncItems = new MailItems()
            {
                Recipients = new List<string>()
                        {
                            "Lauren.Gore@iress.co.uk"//,
                            //"James.Ashwin@iress.co.uk",
                            //"Joanne.Edmiston@iress.co.uk"
                        }
            };

            RuleValues ruleValues = new RuleValues()
            {
                RuleMembers = new List<string>()
                        {
                            "DL-Poulton"
                        },
                DisplayName = "DL-Poulton",
                FolderName = "Poulton List"
            };

            //SendAppointment(appointmentValues);

            StartLyncConvo(lyncItems);

            //CreateRule(ruleValues);

            //DeleteRule(ruleValues);

            Console.ReadLine();
        }

        public static void SendAppointment(MailItems mailItems)
        {
            Application outlookApp = new Application();

            AppointmentItem newAppointment = outlookApp.CreateItem(OlItemType.olAppointmentItem);

            foreach (var recipient in mailItems.Recipients)
            {
                newAppointment.Recipients.Add(recipient);
            }

            newAppointment.MeetingStatus = OlMeetingStatus.olMeeting;

            newAppointment.Display(true);
        }

        public static void CreateRule(RuleValues ruleValues)
        {
            Application outlookApp = new Application();

            Rules rules = outlookApp.Session.DefaultStore.GetRules();

            Folders folders = outlookApp.Session.GetDefaultFolder(OlDefaultFolders.olFolderInbox).Folders;

            Folder ruleFolder;

            try
            {
                ruleFolder = folders[ruleValues.FolderName] as Folder;
            }
            catch
            {
                ruleFolder = folders.Add(ruleValues.FolderName, Type.Missing) as Folder;
            }

            Rule newRule = rules.Create(ruleValues.DisplayName, OlRuleType.olRuleReceive);

            foreach (var member in ruleValues.RuleMembers)
            {
                newRule.Conditions.SentTo.Recipients.Add(member);
            }

            newRule.Conditions.SentTo.Enabled = true;
            newRule.Actions.MoveToFolder.Folder = ruleFolder;
            newRule.Actions.MoveToFolder.Enabled = true;

            rules.Save(true);

            newRule.Execute(true);
        }

        public static void StartLyncConvo(MailItems lyncItems)
        {
            // Create the major API automation object.

            Automation automation = LyncClient.GetAutomation();

            // Create a generic Dictionary object to contain conversation setting objects.
            Dictionary<AutomationModalitySettings, object> modalitySettings = new Dictionary<AutomationModalitySettings, object>();
            AutomationModalities chosenMode = AutomationModalities.InstantMessage;

            // Use these two lines if want to automatically have initial message as Hi or something
            //string firstIMText = "hi";
            //modalitySettings.Add(AutomationModalitySettings.FirstInstantMessage, firstIMText);

            // Set value to true if want to autmatically send the initial message of Hi
            modalitySettings.Add(AutomationModalitySettings.SendFirstInstantMessageImmediately, false);

            // Start the conversation.
            IAsyncResult ar = automation.BeginStartConversation(
                chosenMode
                , lyncItems.Recipients
                , modalitySettings
                , null
                , null);

            //Block UI thread until conversation is started
            automation.EndStartConversation(ar);
        }

        public static void StartLyncMeeting()
        {
            Automation automation = LyncClient.GetAutomation();
            LyncConvo.Conversation conversation;
            ConversationWindow conversationWindow;

        }

        /// <summary>
        /// Doesn't quite work
        /// </summary>
        /// <param name="ruleValues"></param>
        public static void DeleteRule(RuleValues ruleValues)
        {
            Application outlookApp = new Application();

            Rules rules = outlookApp.Session.DefaultStore.GetRules();

            Folders folders = outlookApp.Session.GetDefaultFolder(OlDefaultFolders.olFolderInbox).Folders;

            rules.Remove(ruleValues.DisplayName);

            Folder ruleFolder = folders[ruleValues.FolderName] as Folder;

            Folder destinationFolder = outlookApp.Session.GetDefaultFolder(OlDefaultFolders.olFolderInbox) as Folder;

            Items items = ruleFolder.Items;

            MailItem moveMail = null;

            foreach (object eMail in items)
            {
                moveMail = eMail as MailItem;
                if (moveMail != null)
                {
                    moveMail.Move(destinationFolder);
                }
            }


            rules.Save();
        }

    }

    public class MailItems
    {
        public List<string> Recipients { get; set; }
    }

    public class RuleValues
    {
        public List<string> RuleMembers { get; set; }

        public string FolderName { get; set; }

        public string DisplayName { get; set; }
    }
}

