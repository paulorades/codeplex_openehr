using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace OpenEhr.RM.Support.Terminology.Impl
{
    [CustomFactory(typeof(PropertyUnitsServiceCustomFactory))]
    public interface IPropertyUnitsService
    {
        Unit[] Units(string openEhrPropertyCode);
        Unit[] Units();
    }


    public sealed class Unit
    {
        private string name = null;
        private string symbol = null;

        internal Unit(string name, string symbol)
        {
            this.name = name;
            this.symbol = symbol;
        }

        public string Symbol
        { get { return this.symbol; } }

        public string Name
        { get { return this.name; } }

    }
}
