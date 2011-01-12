using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using OpenEhr.RM.Support.Terminology.Impl.Configuration;

namespace OpenEhr.RM.Support.Terminology.Impl
{
    public class PropertyUnitsServiceCustomFactory
        :AssemblerBasedObjectFactory<IPropertyUnitsService, PropertyUnitsServiceData>
    {
        public static PropertyUnitsServiceCustomFactory Instance = new PropertyUnitsServiceCustomFactory();
    }
}
