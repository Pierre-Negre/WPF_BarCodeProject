using System.IO;
using System.Xml;

namespace WPF_Front.Common
{
    internal static class XmlLogic
    {
        private static readonly string FolderPath = Environment.GetEnvironmentVariable("LocalAppData") + @"\PNSOFTWARES\CBP\";
        private static readonly string FileName = "config.xml";

        public const string PrinterNameXmlNode = "PrinterName";
        public const string PrinterModelXmlNode = "PrinterModel";
        public const string DbNameXmlNode = "DBName";
        public const string DbServerXmlNode = "DBServer";
        public const string DbUserXmlNode = "DBUser";
        public const string DbPasswordXmlNode = "DBPassword";

        /// <summary>
        /// Get or set value for XML node, if newValue parameter is null, read. Otherwise write.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="xmlNodeName"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string SyncXmlValue(string xmlNodeName, string? newValue = null)
        {
            XmlDocument doc = new();
            doc.PreserveWhitespace = true;
            try
            {
                doc.Load(FolderPath + FileName);
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException || e is DirectoryNotFoundException)
                    InitiateXmlDocument(doc);
            }

            var XMLparameters = doc["parameters"];
            if (XMLparameters == null) return string.Empty;

            var XMLnode = XMLparameters[xmlNodeName];
            if (XMLnode == null)
            {
                switch (xmlNodeName)
                {
                    case PrinterNameXmlNode:
                        newValue = Properties.Settings.Default.PrinterName;
                        break;
                    case DbNameXmlNode:
                        newValue = Properties.Settings.Default.DBName;
                        break;
                    case DbServerXmlNode:
                        newValue = Properties.Settings.Default.DBServer;
                        break;
                    case DbUserXmlNode:
                        newValue = Properties.Settings.Default.DBUser;
                        break;
                    case DbPasswordXmlNode:
                        newValue = Properties.Settings.Default.DBPassword;
                        break;
                }
                if (newValue == null) return string.Empty;

                var newNode = doc.CreateElement(xmlNodeName);
                newNode.SetAttribute("value", newValue);
                XMLparameters.AppendChild(newNode);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(newValue)) return XMLnode.GetAttribute("value");
                else XMLnode.SetAttribute("value", newValue);
            }
            doc.Save(FolderPath + FileName);

            return newValue;
        }

        /// <summary>
        /// Creates the xml parameters file.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private static bool InitiateXmlDocument(XmlDocument doc)
        {
            try
            {
                Directory.CreateDirectory(FolderPath);

                doc.LoadXml("<?xml version=\"1.0\"?> \n" +
                    "<parameters> \n" +
                    "  <PrinterName value=\"" + Properties.Settings.Default.PrinterName + "\" /> \n" +
                    "  <PrinterModel value=\"Zebra\" /> \n" +
                    "  <DBServer value=\"" + Properties.Settings.Default.DBServer + "\" /> \n" +
                    "  <DBName value=\"" + Properties.Settings.Default.DBName + "\" /> \n" +
                    "  <DBUser value=\"" + Properties.Settings.Default.DBUser + "\" /> \n" +
                    "  <DBPassword value=\"" + Properties.Settings.Default.DBPassword + "\" /> \n" +
                    "</parameters>"
                    );
                doc.Save(FolderPath + FileName);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
