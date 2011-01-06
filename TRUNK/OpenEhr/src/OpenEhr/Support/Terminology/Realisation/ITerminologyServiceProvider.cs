using System;
using System.Collections.Generic;
using System.Text;
using OpenEhr.RM.Support.Terminology;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using OpenEhr.RM.Support.Terminology.Impl.Configuration;

namespace OpenEhr.RM.Support.Terminology.Impl
{
    [ConfigurationNameMapper(typeof(TerminologyServiceDataRetriever))]
    [CustomFactory(typeof(TerminologyServiceProviderCustomFactory))]
    public interface ITerminologyServiceProvider : ITerminologyService { }



}
