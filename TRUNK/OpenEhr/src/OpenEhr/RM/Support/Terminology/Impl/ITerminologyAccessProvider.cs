using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace OpenEhr.RM.Support.Terminology.Impl
{

    [CustomFactory(typeof(TerminologyAccessProviderCustomFactory))]
    public interface ITerminologyAccessProvider : ITerminologyAccess
    {
    }
}
