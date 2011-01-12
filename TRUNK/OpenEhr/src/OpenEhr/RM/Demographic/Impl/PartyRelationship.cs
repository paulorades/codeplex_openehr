using System;
using OpenEhr.RM.Support.Identification;
using OpenEhr.RM.DataTypes.Quantity;
using OpenEhr.RM.DataTypes.Quantity.DateTime;
using OpenEhr.RM.DataStructures.ItemStructure;
using OpenEhr.RM.DataTypes.Text;
using OpenEhr.DesignByContract;

namespace OpenEhr.RM.Demographic.Impl
{
    public class PartyRelationship 
        : OpenEhr.RM.Demographic.PartyRelationship
    {
        //public PartyRelationship(string archetypeNodeId, DvText name, PartyRef source, PartyRef target)
        //    : base(archetypeNodeId, name, source, target)
        public PartyRelationship(string archetypeNodeId, DvText name, PartyRef target)
            : base(archetypeNodeId, name, target)
        { }

        protected PartyRelationship(): base() { }

        ItemStructure details;

        protected override ItemStructure DetailsBase
        {
            get { return details; }
            set { details = value; }
        }

        DvInterval<DvDate> timeValidity;

        protected override DvInterval<DvDate> TimeValidityBase
        {
            get { return timeValidity; }
            set { timeValidity = value; }
        }

        PartyRef source;

        protected override PartyRef SourceBase
        {
            get {

                if (Parent == null)
                    this.source = null;
                else
                {
                    OpenEhr.RM.Demographic.Party party
                        //= Parent as OpenEhr.RM.Demographic.Role;
                        = Parent as OpenEhr.RM.Demographic.Party;

                    //if (party != null)
                    //    type = "ROLE";

                    //else
                    //{
                    //    party = Parent as OpenEhr.RM.Demographic.Actor;
                    Check.Assert(party != null, "parent must be type of Actor or Role");

                    //    type = party.Name.Value;
                    //}
                    if (party.Uid != null)
                    {
                        string type = OpenEhr.RM.Demographic.Party.GetRmTypeName(party);
                        string @namespace = "local";    // TODO: derive @namespace from parent?

                        this.source = new PartyRef(party.Uid, @namespace, type);
                    }
                }
                return source; 
            }
            //set { source = value; }
        }

        PartyRef target;

        protected override PartyRef TargetBase
        {
            get { return target; }
            set { target = value; }
        }
    }
}
