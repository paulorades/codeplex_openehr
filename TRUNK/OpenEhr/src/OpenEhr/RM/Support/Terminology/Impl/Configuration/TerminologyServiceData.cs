using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
using OpenEhr.RM.Support.Terminology;

namespace OpenEhr.RM.Support.Terminology.Impl.Configuration
{
    //[Assembler(typeof(OpenEhrTerminologyServiceAssembler))]
    public class TerminologyServiceData : TerminologyServiceProviderData
    {
        private const string terminologyAccessProvidersProperty = "terminologyAccessProviders";
        private const string codeSetAccessProvidersProperty = "codeSetAccessProviders";

        public TerminologyServiceData()
            : base()
        { }

        public TerminologyServiceData(string name, Type type)
            : base(name, type)
        { }

        [ConfigurationProperty(terminologyAccessProvidersProperty, IsRequired = true)]
        public NameTypeConfigurationElementCollection<TerminologyAccessProviderData> TerminologyAccessProviders
        {
            get
            {
                return base[terminologyAccessProvidersProperty] as
                    NameTypeConfigurationElementCollection<TerminologyAccessProviderData>;
            }
        }

        [ConfigurationProperty(codeSetAccessProvidersProperty, IsRequired = true)]
        public NameTypeConfigurationElementCollection<CodeSetAccessProviderData> codeSetAccessProviders
        {
            get
            {
                return base[codeSetAccessProvidersProperty] as
                    NameTypeConfigurationElementCollection<CodeSetAccessProviderData>;
            }
        }
    }

    //public class OpenEhrTerminologyServiceAssembler : IAssembler<ITerminologyServiceProvider, TerminologyServiceProviderData>
    //{
    //    #region IAssembler<ITerminologyServiceProvider,TerminologyServiceProviderData> Members

    //    public ITerminologyServiceProvider Assemble(Microsoft.Practices.ObjectBuilder.IBuilderContext context, TerminologyServiceProviderData objectConfiguration, Microsoft.Practices.EnterpriseLibrary.Common.Configuration.IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
    //    {
    //        TerminologyServiceData castedConfiguration = objectConfiguration as TerminologyServiceData;

    //        Dictionary<string, ICodeSetAccess> codeSetAccessDictionary = new Dictionary<string, ICodeSetAccess>();

    //        foreach (CodeSetAccessProviderData codeSetAccessProviderData in castedConfiguration.codeSetAccessProviders)
    //        {
    //            codeSetAccessDictionary.Add(codeSetAccessProviderData.Name,
    //                (ICodeSetAccess)CodeSetAccessProviderCustomFactory.Instance.Create(context, codeSetAccessProviderData, configurationSource, reflectionCache));
    //        }


    //        Dictionary<string, ITerminologyAccess> TemplateTool = new Dictionary<string, ITerminologyAccess>();

    //        foreach (TerminologyAccessProviderData terminologyAccessProviderData in castedConfiguration.TerminologyAccessProviders)
    //        {
    //            TemplateTool.Add(terminologyAccessProviderData.Name,
    //                (ITerminologyAccess)TerminologyAccessProviderCustomFactory.Instance.Create(context, terminologyAccessProviderData, configurationSource, reflectionCache));
    //        }

    //        ITerminologyService terminologyService = new TerminologyService(TemplateTool, codeSetAccessDictionary);

    //        return terminologyService;

    //    }

    //    #endregion
    //}
}
