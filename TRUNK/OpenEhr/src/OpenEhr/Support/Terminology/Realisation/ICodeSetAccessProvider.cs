using System;
using System.Collections.Generic;
using System.Text;
using OpenEhr.RM.Support.Terminology;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace OpenEhr.RM.Support.Terminology.Impl
{
    [CustomFactory(typeof(CodeSetAccessProviderCustomFactory))]
    public interface ICodeSetAccessProvider : ICodeSetAccess
    {
    }
}
