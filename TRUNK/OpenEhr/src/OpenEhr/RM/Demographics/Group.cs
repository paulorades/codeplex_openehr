using System;
using OpenEhr.RM.DataTypes.Text;
using OpenEhr.AssumedTypes;
using OpenEhr.RM.Common.Archetyped;
using OpenEhr.RM.DataStructures.ItemStructure;
using OpenEhr.RM.Support.Identification;
using OpenEhr.Attributes;

namespace OpenEhr.RM.Demographic
{
    [RmType("openEHR", "Demographic", "GROUP")]
    public abstract class Group : Actor
    {
        protected Group(string archetypeNodeId)
            : base(archetypeNodeId, new DvText("GROUP"))
        { }

        protected Group():base() { }
    }
}
