using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace OpenEhr.RM.Support.Terminology.Impl.Configuration
{
    public class TerminologyAccessProviderData : NameTypeConfigurationElement
    {
        public TerminologyAccessProviderData() { }

        public TerminologyAccessProviderData(string name, Type type) : base(name, type) { }

    }
}
