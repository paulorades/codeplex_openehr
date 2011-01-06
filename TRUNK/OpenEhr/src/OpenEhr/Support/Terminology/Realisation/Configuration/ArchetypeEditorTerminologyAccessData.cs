//using System;
////using System.Collections.Generic;
//using System.Text;
//using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
//using System.Configuration;
//using OpenEhr.RM.Support.Terminology.Impl;

//namespace OpenEhr.RM.Support.Terminology.Impl.Configuration
//{
//    [Assembler(typeof(ArchetypeEditorTerminologyAccessAssembler))]
//    public class ArchetypeEditorTerminologyAccessData : TerminologyAccessData
//    {
//        public ArchetypeEditorTerminologyAccessData() { }
//        public ArchetypeEditorTerminologyAccessData(string name, Type type) : base(name, type, string.Empty, string.Empty) { }
//        public ArchetypeEditorTerminologyAccessData(string name, Type type, string xmlFilePath)
//            : base(name, type, xmlFilePath, string.Empty) { }
//        public ArchetypeEditorTerminologyAccessData(string name, Type type, string xmlFilePath, string defaultLanguage)
//            : base(name, type, xmlFilePath, defaultLanguage) { }
//    }

//    public class ArchetypeEditorTerminologyAccessAssembler : IAssembler<ITerminologyAccessProvider, TerminologyAccessProviderData>
//    {
//        #region IAssembler<ITerminologyAccessProvider,TerminologyAccessProviderData> Members

//        public ITerminologyAccessProvider Assemble(Microsoft.Practices.ObjectBuilder.IBuilderContext context, TerminologyAccessProviderData objectConfiguration, Microsoft.Practices.EnterpriseLibrary.Common.Configuration.IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
//        {
//            ArchetypeEditorTerminologyAccessData castedConfiguration =
//                objectConfiguration as ArchetypeEditorTerminologyAccessData;

//            if (string.IsNullOrEmpty(castedConfiguration.XmlFilePath))
//                if (string.IsNullOrEmpty(castedConfiguration.DefaultLanguage))
//                    return new ArchetypeEditorTerminologyAccess();
//                else
//                    return new ArchetypeEditorTerminologyAccess(castedConfiguration.DefaultLanguage);
//            else
//                return new ArchetypeEditorTerminologyAccess(castedConfiguration.XmlFilePath, castedConfiguration.DefaultLanguage);
//        #endregion
//        }
//    }
//}

        