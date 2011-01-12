using System;
//using System.Collections.Generic;
using OpenEhr.Attributes;
using OpenEhr.Serialisation;

namespace OpenEhr.RM.Support.Identification
{
    [System.Xml.Serialization.XmlSchemaProvider("GetXmlSchema")]
    [Serializable]
    [RmType("openEHR", "SUPPORT", "ACCESS_GROUP_REF")]
    public sealed class AccessGroupRef : ObjectRef, System.Xml.Serialization.IXmlSerializable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AccessGroupRef() //: base(new EhrTypes.PARTY_REF()) { }
            : base()
        { }

        ///// <summary>
        ///// Constructor
        ///// </summary>
        ///// <param name="ehrType"></param>
        //internal AccessGroupRef(EhrTypes.ACCESS_GROUP_REF ehrType) : base(ehrType) { }

        public AccessGroupRef(ObjectId objectId, string namespaceValue, string typeValue)
            : this()
        {
            SetBaseData(objectId, namespaceValue, typeValue);
            //SetInnerData();
        }

        #region IXmlSerializable Members

        System.Xml.Schema.XmlSchema System.Xml.Serialization.IXmlSerializable.GetSchema()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        void System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
        {
            this.ReadXml(reader);
        }

        void System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
        {
            this.WriteXml(writer);
        }

        #endregion

        public static System.Xml.XmlQualifiedName GetXmlSchema(System.Xml.Schema.XmlSchemaSet xs)
        {
            RmXmlSerializer.LoadBaseTypesSchema(xs);
            return new System.Xml.XmlQualifiedName(RmTypeName, RmXmlSerializer.OpenEhrNamespace);

        }

        const string RmTypeName = "ACCESS_GROUP_REF";
    }
}
