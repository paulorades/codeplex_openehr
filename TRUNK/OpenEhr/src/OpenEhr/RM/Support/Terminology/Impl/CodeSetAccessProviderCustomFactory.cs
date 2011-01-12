using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using OpenEhr.RM.Support.Terminology.Impl.Configuration;

namespace OpenEhr.RM.Support.Terminology.Impl
{
    public class CodeSetAccessProviderCustomFactory 
        :AssemblerBasedObjectFactory<ICodeSetAccessProvider,CodeSetAccessProviderData>
    {
        public static CodeSetAccessProviderCustomFactory Instance = new CodeSetAccessProviderCustomFactory();
    }
}
