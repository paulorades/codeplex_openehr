using System;
using OpenEhr.Attributes;
using OpenEhr.RM.Common.Archetyped.Impl;
using OpenEhr.RM.DataTypes.Text;
using OpenEhr.RM.Impl;

namespace OpenEhr.RM.Composition.Content
{
    [Serializable]
    [RmType("openEHR", "EHR", "CONTENT_ITEM")]
    public abstract class ContentItem : Locatable, IVisitable
    {
        //protected override void Serialize(System.Xml.XmlTextWriter writer, 
        //    System.Xml.Serialization.XmlSerializerNamespaces xmlns)
        //{
        //    DesignByContract.Check.Require(writer != null, "writer must not be null");
        //    DesignByContract.Check.Require(xmlns != null, "xmlns must not be null");

        //    OpenEhr.V1.Its.Xml.XmlSerializer.ContentSerializer.Serialize(writer, base.LocatableType, xmlns);
        //}

        //protected ContentItem(EhrTypes.LOCATABLE tLocatable)
        //    : base(tLocatable)
        protected ContentItem()
            : base()
        { }

        protected ContentItem(DvText name, string archetypeNodeId, Support.Identification.UidBasedId uid,
           Link[] links, Archetyped archetypeDetails, FeederAudit feederAudit)
            : base(name, archetypeNodeId, uid, links, archetypeDetails, feederAudit)
        { }

        #region IVisitable Members

        protected virtual void Accept(IVisitor visitor)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        void IVisitable.Accept(IVisitor visitor)
        {
            this.Accept(visitor);
        }

        #endregion
    }
}
