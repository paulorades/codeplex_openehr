using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using OpenEhr.RM.Support.Terminology.Impl.Configuration;
using System.Xml.XPath;
using OpenEhr.RM.Support.Terminology.Impl.Data;
using OpenEhr.Utilities;
using OpenEhr.RM.DataTypes.Text;

namespace OpenEhr.RM.Support.Terminology.Impl
{
    [ConfigurationElementType(typeof(TerminologyAccessData))]
    public class TerminologyAccess : ITerminologyAccessProvider
    {
        readonly string defaultLanguage = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        readonly string xmlFilePath;

        Lazy<XPathDocument> terminologyDoc = null;
        
        public TerminologyAccess() : this(null,null) { }

        public TerminologyAccess(string defaultLanguage) : this(null,defaultLanguage) { }


        public TerminologyAccess(string xmlFilePath, string defaultLanguage)
        {
            if (!string.IsNullOrEmpty(xmlFilePath))
            {
                terminologyDoc = new Lazy<XPathDocument>(delegate()
                    {
                        return new XPathDocument(xmlFilePath);
                    });
            }

            if (!string.IsNullOrEmpty(defaultLanguage))
                this.defaultLanguage = defaultLanguage;
        }

        private XPathDocument TerminologyDoc
        {
            get
            {
                if (terminologyDoc != null)
                    return terminologyDoc.Value;
                else
                    return TerminologyDocument.Value;
            }
        }

        #region ITerminologyAccess Members

        public string Id
        {
            get { return TerminologyService.TerminologyIdOpenehr; }
        }

        public List<OpenEhr.RM.DataTypes.Text.CodePhrase> AllCodes
        {
           
            get 
            {
                List<CodePhrase> results = new List<CodePhrase>();
                try
                {
                    XPathNavigator navigator = TerminologyDoc.CreateNavigator();
                    XPathExpression expression = navigator.Compile("/terminology/group/concept/@id");
                    expression.AddSort("id", XmlSortOrder.Ascending,XmlCaseOrder.None,"",XmlDataType.Number);

                    foreach (XPathNavigator idNav in navigator.Select(expression))
                        results.Add(new CodePhrase(idNav.Value,TerminologyService.TerminologyIdOpenehr));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(
                    string.Format("Could not read codes {0}", ex.Message));
                }
                return results;
            }
        }

        public List<OpenEhr.RM.DataTypes.Text.CodePhrase> CodesForGroupId(string groupId)
        {
            List<CodePhrase> results = new List<CodePhrase>();
            try
            {
                XPathNavigator navigator = TerminologyDoc.CreateNavigator();
                XPathExpression expression = navigator.Compile("/terminology/group[@name='"+ groupId +"']/concept/@id");
                expression.AddSort("id", XmlSortOrder.Ascending,XmlCaseOrder.None,"",XmlDataType.Number);

                foreach (XPathNavigator idNav in navigator.Select(expression))
                    results.Add(new CodePhrase(idNav.Value,TerminologyService.TerminologyIdOpenehr));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(
                string.Format("Could not read codes {0}", ex.Message));
            }
            return results;
        }

        public bool HasCodeForGroupId(string groupId, OpenEhr.RM.DataTypes.Text.CodePhrase code)
        {
            if (code.TerminologyId.Value != TerminologyService.TerminologyIdOpenehr)
                return false;

            return TerminologyDoc.CreateNavigator()
                .Select("/terminology/group[@name='" + groupId + "']/concept[@id='" + code.CodeString + "']").Count > 0;
        }

        public List<OpenEhr.RM.DataTypes.Text.CodePhrase> CodesForGroupName(string name, string lang)
        {
            return CodesForGroupId(name);
        }

        public string RubricForCode(string code, string lang)
        {

           XPathNavigator navigator = TerminologyDoc.CreateNavigator();
           foreach (XPathNavigator nav in navigator.Select("/terminology/group/concept[@id='" + code + "']/@rubric"))
               return nav.Value;
           return string.Empty;
        }

        #endregion
    }
}
