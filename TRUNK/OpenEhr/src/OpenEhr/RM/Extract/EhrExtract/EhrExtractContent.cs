using System;
using OpenEhr.RM.Extract.Common;
using OpenEhr.AssumedTypes;
using OpenEhr.Attributes;
using OpenEhr.RM.Common.Archetyped.Impl;

namespace OpenEhr.RM.Extract.EhrExtract
{
    /// <summary> 
    /// Form of EHR Extract content containing openEHR serialised VERSIONED_OBJECTs.
    /// </summary>
    [Serializable]
    [RmType("openEHR", "EXTRACT", "EXTRACT_ENTITY_CONTENT")]
    public class EhrExtractContent : ExtractEntityContent
    {
        //const string defaultArchetypeId = "openEHR-EXTRACT-EHR_EXTRACT_CONTENT.default.v1";
        //const string defaultName = "EHR extract content";

        XVersionedObject<Ehr.EhrStatus> ehrStatus;
        List<XVersionedObject<Composition.Composition>> compositions;
        //XVersionedObject<Ehr.EhrAccess> ehrAccess;
        //List<XVersionedObject<Demographics.Party>> demographics;
        //XVersionedObject<OpenEhrV1.Common.Directory.Folder> directory;
        List<XVersionedObject<Locatable>> otherItems;

        internal EhrExtractContent()
        { }

        public EhrExtractContent(string archetypeNodeId, DataTypes.Text.DvText name)
            :base(archetypeNodeId, name)
        {
            // TODO: implement SetAttributeDictionary and CheckInvariant overrides
            SetAttributeDictionary();
            CheckInvariants();
        }

        public XVersionedObject<Ehr.EhrStatus> EhrStatus
        {
            get { return this.ehrStatus; }
            set { this.ehrStatus = value; }
        }

        /// <summary> Compositions from source EHR
        /// Compositions_valid: compositions /= Void implies not compositions.is_empty
        /// </summary>
        public List<XVersionedObject<Composition.Composition>> Compositions
        {
            get { return this.compositions; }
            set { this.compositions = value; }
        }

        /// <summary> Other items from source EHR.
        /// Other_items_valid: other_items /= Void implies not other_items.is_empty
        /// </summary>
        public List<XVersionedObject<Locatable>> OtherItems
        {
            get { return this.otherItems; }
            set { this.otherItems = value; }
        }

        //public Common.XVersionedObject<Ehr.EhrAccess> EhrAccess
        //{
        //    get
        //    {
        //        throw new System.NotImplementedException();
        //    }
        //    set
        //    {
        //    }
        //}

        ///<summary>Folder tree from source EHR</summary>
        //public Common.XVersionedObject<Folder> Directory
        //{
        //    get
        //    {
        //        throw new System.NotImplementedException();
        //    }
        //    set
        //    {
        //    }
        //}

        /// <summary> Demographic entities from source EHR.
        /// Demographics_valid: demographics /= Void implies not demographics.is_empt
        /// </summary>
        //public Support.Assumed.List<Common.XVersionedObject<Party>> Demographics
        //{
        //    get
        //    {
        //        throw new System.NotImplementedException();
        //    }
        //    set
        //    {
        //    }
        //}

        /// <summary>
        /// Add versioned composition extract object to compositions collection
        /// </summary>
        /// <param name="versionedComposition"></param>
        public void AddComposition(XVersionedObject<Composition.Composition> versionedComposition)
        {
            DesignByContract.Check.Require(versionedComposition != null, "VersionedComposition must not be null");

            if (this.compositions == null)
                this.compositions = new List<XVersionedObject<Composition.Composition>>();

            this.compositions.Add(versionedComposition);
        }
    }
}
