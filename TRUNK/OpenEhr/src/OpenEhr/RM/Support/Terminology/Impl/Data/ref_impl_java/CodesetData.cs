using System;
using System.Collections.Generic;
using OpenEhr.DesignByContract;
using System.Xml.XPath;
using OpenEhr.RM.DataTypes.Text;
using OpenEhr.Utilities;
using OpenEhr.Utilities.PathHelper;

namespace OpenEhr.RM.Support.Terminology.Impl.Data.ref_impl_java
{
    internal sealed class CodesetData
    {
        private readonly Lazy<XPathDocument> termDocument ;

        public CodesetData()
        {
            termDocument = new Lazy<XPathDocument>(delegate()
            {
                return TerminologyDocument.Value;
            });
        }
        public CodesetData(string xmlFilePath)
        {
            Check.Require(!string.IsNullOrEmpty(xmlFilePath), "xmlFilePath must not be null or empty.");
            termDocument = new Lazy<XPathDocument>(delegate()
            {
                return new XPathDocument(PathHelper.AbsolutePath(xmlFilePath));
            });
        }

        public  List<CodePhrase> GetAllCodes(string name)
        {
            List<CodePhrase> results = new List<CodePhrase>();
            try
            {
                XPathNavigator navigator = termDocument.Value.CreateNavigator();
                foreach (XPathNavigator codeNav in navigator.Select("/terminology/codeset[@external_id='" + name + "']/*"))
                { 
                    string code = codeNav.SelectSingleNode("@value").Value;

                    CodePhrase codePhrase = new CodePhrase(code, name);

                    results.Add(codePhrase);
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(
                    string.Format("Could not read codes for {0} \n {1}", name, ex.Message));
            }

            return results;

        }

        public Dictionary<string, string> GetCodeDescriptions(string name)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();

            try
            {
                XPathNavigator navigator = termDocument.Value.CreateNavigator();
                foreach (XPathNavigator codeNav in navigator.Select("/terminology/codeset[@external_id='" + name + "']/*"))
                {
                    string code = codeNav.SelectSingleNode("@value").Value;
                    string description = codeNav.SelectSingleNode("@Description").Value;

                    results.Add(code, description);
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(
                    string.Format("Could not read codes for {0} \n {1}", name, ex.Message));
            }

            return results;
        }

        public string GetCodesetOpenEhrId(string name)
        {
            string result = string.Empty;

            try
            {
                XPathNavigator navigator = termDocument.Value.CreateNavigator();
                result = navigator.SelectSingleNode("/terminology/codeset[@external_id='" + name + "']/@openehr_id").Value;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(
                    string.Format("Could not read OpenEhrId for {0} \n {1}", name, ex.Message));
            }

            return result;
        }

        public bool HasLanguage(string codeString)
        {
            bool result = false;

            try
            {
                XPathNavigator navigator = termDocument.Value.CreateNavigator();
                string lanuage = navigator.SelectSingleNode("/terminology/@language").Value;
                result = lanuage == codeString;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(
                    string.Format("Error reading lanuage {0}", ex.Message));
            }
            return result;
        }

        public bool HasCode(string name, string code)
        {
            XPathNavigator result = null;

            try
            {
                XPathNavigator navigator = termDocument.Value.CreateNavigator();
                result = 
                    navigator.SelectSingleNode("/terminology/codeset[@external_id='" + name + "']/code[@value='" + code + "']");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(
                    string.Format("Error reading code {0}", ex.Message));

            }

            return result != null;
        }
    }
}
