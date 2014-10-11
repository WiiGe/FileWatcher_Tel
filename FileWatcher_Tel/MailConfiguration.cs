using System;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Collections;
using System.Xml;

namespace EMailSender
{
    public class MailConfiguration
    {
        //SMTP Server Setting Here
        //-----------------------------------------------------------------------------------------------------------------
        public string SMTP_IP
        {
            get 
            {
                return _SMTP_IP;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    //Valid IP Address String
                    if (MailConfiguration.IPValidator(value.Trim()))
                    {
                        _SMTP_IP = value.Trim();
                    }
                }
                else 
                {
                    _SMTP_IP = default(string);
                }
            }
        }
        private string _SMTP_IP;

        public string SMTP_DomainName
        { 
            get
            {
                return _SmtpDomainName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (DomainNameValidator(value.Trim()))
                    {
                        _SmtpDomainName = value.Trim();
                    }
                }
                else 
                {
                    _SmtpDomainName = default(string);
                }
            }
        }
        private string _SmtpDomainName;

        public static string SMTP_UserName;
        public string SMTP_PassWord;
        public bool SMTP_SSL_Enable;
       

        //-----------------------------------------------------------------------------------------------------------------
        
        //Email Sender Name
        public string FromName="Windows电报监视服务";
        //Enable HTML E-Mail
        public bool HtmlMail = true; 

        /// <summary> 
        /// Get Recipient Table Of E-Mail 
        /// </summary>
        public static void GetRecipient(string args)
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(args);
            //Select The Configuration Node
            XmlNodeList xmlNodeList_Mail = xmlDoc.ChildNodes[1].ChildNodes;
            //Select The MialBox Node
            foreach (XmlElement xmlelemt in xmlNodeList_Mail) 
            {
                if (xmlelemt.Name == "MailBox")
                {
                    xmlNodeList_Mail = xmlelemt.ChildNodes;
                }
            }

            foreach (XmlElement element in xmlNodeList_Mail)
            {
                XmlAttributeCollection arrt = element.Attributes;

                if (!string.IsNullOrEmpty(arrt[0].Value))
                {
                    _Recipient.Add(arrt[0].Value, arrt[1].Value);
                }
                else
                {
                    _Recipient.Add("Dear Administrator#" + DateTime.UtcNow.ToString(), arrt[1].Value);
                }

                //for (int i = 0; i <= element.Attributes.Count - 1; i++)
                //{
                //    if (element.Attributes[i].Value.Trim() != "")
                //    {
                //         _Recipient.Add(element.Attributes[i].InnerXml, element.Attributes[i].NextSibling.InnerXml);
                //    }
                //    else
                //    {
               //         _Recipient.Add("Dear Administrator", element.Attributes[i].Value.Trim());
                //    }
                //}

            }
            MailReciverCounter++;
            
        }
        
        //Hold The EMail Reciver Table
        public static Hashtable _Recipient = new Hashtable();
        private static int MailReciverCounter;
        //-----------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Indicate How Long The Time Should be modified,or trigger the Mail/SMS alarm
        /// </summary>
        public static TimeSpan timerAlterSpan = default(TimeSpan);

        //-----------------------------------------------------------------------------------------------------------------
        //Some Validation Function
        protected static bool EMailValidator(string args)
        {
            return Regex.IsMatch(args, @"^([w-.]+)@(([[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.)|(([w-]+.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(]?)$");
        }

        protected static bool IPValidator(string args)
        {
            return Regex.IsMatch(args, @"^(d{1,2}|1dd|2[0-4]d|25[0-5]).(d{1,2}|1dd|2[0-4]d|25[0-5]).(d{1,2}|1dd|2[0-4]d|25[0-5]).(d{1,2}|1dd|2[0-4]d|25[0-5])$");
        }

        protected static bool DomainNameValidator(string args)
        {
            return Regex.IsMatch(args, @"^([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$");
        }
       
        
        //-----------------------------------------------------------------------------------------------------------------
        //Configuration Class Constractor
        #region Constractor
        public MailConfiguration(string args)
        {
            //Read Config File via XMLReader or something else 
            ExeConfigurationFileMap mapedExeConfiguration = new ExeConfigurationFileMap();
            mapedExeConfiguration.ExeConfigFilename = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + args;
            //KeyValueConfigurationCollection configs = ConfigurationManager.OpenMappedExeConfiguration(mapedExeConfiguration, ConfigurationUserLevel.None).AppSettings.Settings;
            KeyValueConfigurationCollection configs = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings;
            
            //iterates through the KeyValueConfigurationCollection to Find Necessary Setting Value
            int SettingSectionCounter= default(int);


            //------------------------------------------------------------------------------------------------------------------
            //foreach()
            //{
            //    string tempER = ConfigurationManager.AppSettings["SMTP-IP"];  
            //    SMTP_IP = string.IsNullOrEmpty(tempER) ? "空字符串" : tempER; 
            //
            //------------------------------------------------------------------------------------------------------------------
            foreach (KeyValueConfigurationElement keyValueElement in configs)
            {
                switch (keyValueElement.Key)
                {
                    case "SMTP-IP":
                        if (string.IsNullOrEmpty(keyValueElement.Value))
                        {
                            SMTP_IP = default(string);
                        }
                        else
                        {
                            SMTP_IP = keyValueElement.Value;
                        }
                        break;
                    case "SMTP-DN":
                        if (string.IsNullOrEmpty(keyValueElement.Value))
                        {
                            SMTP_DomainName = default(string);
                        }
                        else
                        {
                            SMTP_DomainName = keyValueElement.Value;
                        }
                        break;
                    case "SMTP-LoginAccount":
                        if (string.IsNullOrEmpty(keyValueElement.Value))
                        {
                            SMTP_UserName = default(string);
                        }
                        else
                        {
                            SMTP_UserName = keyValueElement.Value;
                        }
                        break;
                    case "SMTP-LoginPassWord":
                        if (string.IsNullOrEmpty(keyValueElement.Value))
                        {
                            SMTP_PassWord = default(string);
                        }
                        else
                        {
                            SMTP_PassWord = keyValueElement.Value;
                        }
                        break;
                    case "SMTP-EnableSSL":
                        if (string.IsNullOrEmpty(keyValueElement.Value))
                        {
                            SMTP_SSL_Enable = default(bool);
                        }
                        else
                        {
                            SMTP_SSL_Enable = Boolean.Parse(keyValueElement.Value);
                        }
                        break;
                    case "AlarmTimeSpan":
                        if (string.IsNullOrEmpty(keyValueElement.Value))
                        {
                            timerAlterSpan = default(TimeSpan);
                        }
                        else
                        {
                            timerAlterSpan = TimeSpan.Parse(keyValueElement.Value);//set The Alarting TimeSpan 
                        }
                        break;
                    default:
                        break; 
                }
                SettingSectionCounter++;
            }
            GetRecipient(@"conf.xml");
        }

        #endregion

    }
}
