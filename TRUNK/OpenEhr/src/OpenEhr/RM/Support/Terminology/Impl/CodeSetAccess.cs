using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using OpenEhr.RM.Support.Terminology.Impl.Configuration;
using OpenEhr.DesignByContract;
using OpenEhr.RM.Support.Terminology.Impl.Data.ref_impl_java;
using OpenEhr.Utilities;

namespace OpenEhr.RM.Support.Terminology.Impl
{
    [ConfigurationElementType(typeof(CodeSetAccessData))]
    public class CodeSetAccess : ICodeSetAccessProvider
    {
        private string name;
        readonly Lazy<CodesetData> codesetData;

        public CodeSetAccess(string name, string xmlFilePath)
        {
            Check.Require(!string.IsNullOrEmpty(name), "name must not be null or empty.");
            this.name = name;

            codesetData = new Lazy<CodesetData>(delegate()
            {
                return string.IsNullOrEmpty(xmlFilePath) ?
                          new CodesetData() :
                          new CodesetData(xmlFilePath);

            });
        }

        private CodesetData CodesetData
        {
            get { return codesetData.Value; }
        }

        internal string InternalId
        {
            get
            {
                string id = CodesetData.GetCodesetOpenEhrId(this.name);

                Check.Ensure(!string.IsNullOrEmpty(id), "id must not be null or empty.");

                return id;
            }
        }

        public Dictionary<string, string> AllCodesAndDescriptions
        {
            get { return CodesetData.GetCodeDescriptions(name); }
        }

        #region ICodeSetAccess Members

        public string Id
        {
            get 
            {
                return name;
            }
        }

        public List<OpenEhr.RM.DataTypes.Text.CodePhrase> AllCodes
        {
            get 
            {
                return CodesetData.GetAllCodes(name);    
            }
        }

        public bool HasLang(OpenEhr.RM.DataTypes.Text.CodePhrase lang)
        {
            return CodesetData.HasLanguage(lang.CodeString);
        }

        public bool HasCode(OpenEhr.RM.DataTypes.Text.CodePhrase code)
        {
            return CodesetData.HasCode(name, code.CodeString);           
        }

        #endregion
    }
}
