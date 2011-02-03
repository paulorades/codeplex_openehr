using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using System.Configuration;

namespace OpenEhr.RM.Support.Terminology.Impl.Configuration
{
    [Assembler(typeof(TerminologyAccessAssembler))]
    public class TerminologyAccessData : TerminologyAccessProviderData
    {
        const string xmlFilePathProperty = "xmlFilePath";
        const string defaultLanguageProperty = "defaultLanguage";

        public TerminologyAccessData() { }
        public TerminologyAccessData(string name, Type type) : this(name, type, string.Empty, string.Empty) { }
        public TerminologyAccessData(string name, Type type, string xmlFilePath) : this(name, type, xmlFilePath, string.Empty) { }
        public TerminologyAccessData(string name, Type type, string xmlFilePath, string defaultLanguage)
            : base(name, type)
        {
            XmlFilePath = xmlFilePath;
            DefaultLanguage = defaultLanguage;
        }

        [ConfigurationProperty(defaultLanguageProperty, IsRequired = false)]
        public string DefaultLanguage
        {
            get { return (string)base[defaultLanguageProperty]; }
            set { base[defaultLanguageProperty] = value; }
        }

        [ConfigurationProperty(xmlFilePathProperty, IsRequired = false)]
        public string XmlFilePath
        {
            get { return (string)base[xmlFilePathProperty]; }
            set { base[xmlFilePathProperty] = value; }
        }
    }

    public class TerminologyAccessAssembler : IAssembler<ITerminologyAccessProvider, TerminologyAccessProviderData>
{
        #region IAssembler<ITerminologyAccessProvider,TerminologyAccessProviderData> Members

        public ITerminologyAccessProvider  Assemble(Microsoft.Practices.ObjectBuilder.IBuilderContext context, TerminologyAccessProviderData objectConfiguration, Microsoft.Practices.EnterpriseLibrary.Common.Configuration.IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            TerminologyAccessData castedConfiguration = objectConfiguration as TerminologyAccessData;

            if (string.IsNullOrEmpty(castedConfiguration.XmlFilePath))
                if (string.IsNullOrEmpty(castedConfiguration.DefaultLanguage))
                    return new TerminologyAccess();
                else
                    return new TerminologyAccess(castedConfiguration.DefaultLanguage);
            else
                return new TerminologyAccess(castedConfiguration.XmlFilePath, castedConfiguration.DefaultLanguage);
        }

        #endregion
}
}
