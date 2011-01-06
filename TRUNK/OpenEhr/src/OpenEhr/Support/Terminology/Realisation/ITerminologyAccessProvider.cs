using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using System.Collections.Generic;
using System.Text;
using OpenEhr.RM.Support.Terminology;


namespace OpenEhr.RM.Support.Terminology.Impl
{

    [CustomFactory(typeof(TerminologyAccessProviderCustomFactory))]
    public interface ITerminologyAccessProvider : ITerminologyAccess
    {
    }
}
