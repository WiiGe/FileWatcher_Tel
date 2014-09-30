using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Xml;

namespace EMailSender
{
    class MailConfiguration
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
                //Valid IP Address String
                if (MailConfiguration.IPValidator(value.Trim()))
                {
                    _SMTP_IP = value.Trim();
                }
            }
        }
        private string _SMTP_IP;

        public string SMTP_DomainName
        { 
            get
            {
                return _SMTP_DomainName;
            }
            set
            {
                if(DomainNameValidator(value.Trim()))
                {
                    _SMTP_DomainName = value.Trim();
                }
            }
        }
        private string _SMTP_DomainName;

        public string SMTP_UserName;
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

                if (arrt[0].Value != "")
                {
                    _Recipient.Add(arrt[0].Value, arrt[1].Value);
                }
                else
                {
                    _Recipient.Add("Dear Administrator", arrt[1].Value);
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
                        if (keyValueElement.Value == null || keyValueElement.Value == "")
                        {
                            SMTP_IP = default(string);
                        }
                        else
                        {
                            SMTP_IP = keyValueElement.Value;
                        }
                        break;
                    case "SMTP-DN":
                        if (keyValueElement.Value == null || keyValueElement.Value == "")
                        {
                            SMTP_DomainName = default(string);
                        }
                        else
                        {
                            SMTP_DomainName = keyValueElement.Value;
                        }
                        break;
                    case "SMTP-LoginAccount":
                        if (keyValueElement.Value == null || keyValueElement.Value == "")
                        {
                            SMTP_UserName = default(string);
                        }
                        else
                        {
                            SMTP_UserName = keyValueElement.Value;
                        }
                        break;
                    case "SMTP-LoginPassWord":
                        if (keyValueElement.Value == null || keyValueElement.Value == "")
                        {
                            SMTP_PassWord = default(string);
                        }
                        else
                        {
                            SMTP_PassWord = keyValueElement.Value;
                        }
                        break;
                    case "SMTP-EnableSSL":
                        if (keyValueElement.Value == null || keyValueElement.Value == "")
                        {
                            SMTP_SSL_Enable = default(bool);
                        }
                        else
                        {
                            SMTP_SSL_Enable = Boolean.Parse(keyValueElement.Value);
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
