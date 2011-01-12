using System;
using OpenEhr.AssumedTypes;
using OpenEhr.RM.Support.Identification;
using OpenEhr.RM.DataTypes.Text;

namespace OpenEhr.RM.Demographic
{
    public abstract class Actor 
        : Party
    {
        protected Actor():base()
        { }

        protected Actor(string archetypeNodeId, DvText name)
            : base(archetypeNodeId, name)
        { }

        //List<PartyRef> roles;

        //protected virtual List<PartyRef> RolesBase
        //{
        //    get { return roles; }
        //    set { roles = value; }
        //}

        //List<DvText> languages;

        //protected virtual List<DvText> LanguagesBase
        //{
        //    get { return languages; }
        //    set { languages = value; }
        //}

        protected abstract List<PartyRef> RolesBase
        {
            get;
            set;
        }

        protected abstract List<DvText> LanguagesBase
        {
            get;
            set;
        }


        #region ACTOR

        /// <summary>
        /// Identifiers of the Version container for each Role played by this party.
        /// </summary>
        public List<PartyRef> Roles
        {
            get { return RolesBase; }
            set { RolesBase = value; }
        }

        /// <summary>
        /// Languages which can be used to communicate with this actor, 
        /// in preferred order of use (if known, else order irrelevant).
        /// </summary>
        public List<DvText> Languages
        {
            get { return LanguagesBase; }
            //set { LanguagesBase = value; }
        }

        /// <summary>
        /// True if one there is an identity with purpose “legal identity”
        /// </summary>
        /// <returns></returns>
        public bool HasLegalIdentity()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
