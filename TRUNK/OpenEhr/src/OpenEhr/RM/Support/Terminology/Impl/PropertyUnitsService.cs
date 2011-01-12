using System;
using System.Collections.Generic;
using System.Text;

namespace OpenEhr.RM.Support.Terminology.Impl
{
    public class PropertyUnitsService : IPropertyUnitsService
    {
        public PropertyUnitsService(string terminologyServiceProvider) { }
        public PropertyUnitsService(string terminologyServiceProvider, string xmlFilenamePath) { }

        #region IPropertyUnitsService Members

        public Unit[] Units(string openEhrPropertyCode)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Unit[] Units()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
