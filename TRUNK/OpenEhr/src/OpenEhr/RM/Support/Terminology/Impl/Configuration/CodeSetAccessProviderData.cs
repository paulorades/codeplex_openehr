using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace OpenEhr.RM.Support.Terminology.Impl.Configuration
{
    public class CodeSetAccessProviderData : NameTypeConfigurationElement
    {
        public CodeSetAccessProviderData() { }
        public CodeSetAccessProviderData(string name, Type type) : base(name, type) { }
    }
}
