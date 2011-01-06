using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using OpenEhr.RM.Support.Terminology.Impl;

namespace OpenEhr.RM.Support.Terminology.Impl.Configuration
{
    [Assembler(typeof(PropertyUnitsServiceAssembler))]
    public class PropertyUnitsServiceData : NameTypeConfigurationElement
    {
        const string XmlFilePathProperty = "xmlFilePath";
        const string TerminologyServiceProviderProperty = "terminologyServiceProvider";

        public PropertyUnitsServiceData() { }
        public PropertyUnitsServiceData(string name, Type type) : this(name,type,string.Empty,string.Empty) {}
        public PropertyUnitsServiceData(string name, Type type, string xmlFilePath) : this(name, type, xmlFilePath, string.Empty) { }
        public PropertyUnitsServiceData(string name, Type type, string xmlFilePath, string terminologyServiceProvider)
            : base(name, type)
        {
            XmlFilePath = xmlFilePath;
            TerminologyServiceProvider = terminologyServiceProvider;
        }

        [ConfigurationProperty(XmlFilePathProperty, IsRequired = false)]
        public string XmlFilePath
        {
            get { return (string)base[XmlFilePathProperty]; }
            set { base[XmlFilePathProperty] = value; }
        }

        [ConfigurationProperty(TerminologyServiceProviderProperty, IsRequired = false)]
        public string TerminologyServiceProvider
        {
            get { return (string)base[TerminologyServiceProviderProperty]; }
            set { base[TerminologyServiceProviderProperty] = value; }
        }
    }

    public class PropertyUnitsServiceAssembler : IAssembler<IPropertyUnitsService,PropertyUnitsServiceData>
    {
        #region IAssembler<IPropertyUnitsService,PropertyUnitsServiceData> Members

        public IPropertyUnitsService Assemble(Microsoft.Practices.ObjectBuilder.IBuilderContext context, PropertyUnitsServiceData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            if (string.IsNullOrEmpty(objectConfiguration.XmlFilePath))
                return new PropertyUnitsService(objectConfiguration.TerminologyServiceProvider);
            else
                return new PropertyUnitsService(objectConfiguration.TerminologyServiceProvider, objectConfiguration.XmlFilePath);
        }

        #endregion
    }
}
