using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using System.Configuration;
using OpenEhr.RM.Support.Terminology.Impl;

namespace OpenEhr.RM.Support.Terminology.Impl.Configuration
{
    [Assembler(typeof(CodeSetAccessAssembler))]
    public class CodeSetAccessData : CodeSetAccessProviderData
    {
        const string xmlFilePathProperty = "xmlFilePath";

        public CodeSetAccessData() { }
        public CodeSetAccessData(string name, Type type) : this(name, type, string.Empty) { }
        public CodeSetAccessData(string name, Type type, string xmlFilePath)
            : base(name, type)
        {
            xmlFilePath = xmlFilePath;
        }

        [ConfigurationProperty(xmlFilePathProperty, IsRequired = false)]
        public string XmlFilePath
        {
            get { return (string)base[xmlFilePathProperty]; }
            set { base[xmlFilePathProperty] = value; }
        }
    }

    public class CodeSetAccessAssembler : IAssembler<ICodeSetAccessProvider, CodeSetAccessProviderData>
    {

        #region IAssembler<ICodeSetAccessProvider,CodeSetAccessProviderData> Members

        public ICodeSetAccessProvider Assemble(Microsoft.Practices.ObjectBuilder.IBuilderContext context, CodeSetAccessProviderData objectConfiguration, Microsoft.Practices.EnterpriseLibrary.Common.Configuration.IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            CodeSetAccessData castedConfiguration = objectConfiguration as CodeSetAccessData;
            return new CodeSetAccess(castedConfiguration.Name, castedConfiguration.XmlFilePath);
        }

        #endregion
    }
}
