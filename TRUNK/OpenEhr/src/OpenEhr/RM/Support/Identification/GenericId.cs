using System;
//using System.Collections.Generic;
using System.ComponentModel;
using OpenEhr.RM.DataTypes.Basic;
using OpenEhr.DesignByContract;
using OpenEhr.Attributes;
using OpenEhr.Serialisation;

namespace OpenEhr.RM.Support.Identification
{
    [TypeConverter(typeof(TerminologyIdTypeConverter))]
    [System.Xml.Serialization.XmlSchemaProvider("GetXmlSchema")]
    [Serializable]
    [RmType("openEHR", "SUPPORT", "GENERIC_ID")]
    public class GenericId : ObjectId, System.Xml.Serialization.IXmlSerializable
    {
        public GenericId() //: base(new EhrTypes.GENERIC_ID()) { }
            : base()
        { }

        //internal GenericId(EhrTypes.GENERIC_ID objectIdType) : base(objectIdType) { }

        public GenericId(string value, string scheme)
            : this() 
        {
            this.scheme = scheme;
            SetBaseData(value);
            //SetInnerData();
            this.CheckInvariants();
        }

        //private EhrTypes.GENERIC_ID ehrType;
        //internal EhrTypes.GENERIC_ID EhrType
        //{
        //    get
        //    {
        //        if (ehrType == null)
        //            this.ehrType = base.EhrType as EhrTypes.GENERIC_ID;

        //        return this.ehrType;
        //    }
        //}      

        private string scheme;

        public string Scheme
        {
            get
            {
                //if (scheme == null)
                //{
                //    this.scheme = ((EhrTypes.GENERIC_ID)EhrType).scheme;
                //}
                return this.scheme;
            }
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

        protected override void ReadXmlBase(System.Xml.XmlReader reader)
        {
            base.ReadXmlBase(reader);
            DesignByContract.Check.Assert(reader.LocalName == "scheme", 
                "Expected LocalName is 'scheme' not "+reader.LocalName);
            this.scheme = reader.ReadElementString("scheme", RmXmlSerializer.OpenEhrNamespace);
        }

        protected override void WriteXmlBase(System.Xml.XmlWriter writer)
        {
            base.WriteXmlBase(writer);

            string openEhrNamespace = RmXmlSerializer.OpenEhrNamespace;
            string prefix = RmXmlSerializer.UseOpenEhrPrefix(writer);

            writer.WriteElementString(prefix, "scheme", openEhrNamespace, this.Scheme);
        }

        //protected override void SetInnerData()
        //{
        //    base.SetInnerData();
        //    this.EhrType.scheme = this.scheme;
        //}

        protected override void CheckInvariants()
        {
            base.CheckInvariants();
            DesignByContract.Check.Invariant(this.Scheme != null, "scheme must not be null");
        }

        public static System.Xml.XmlQualifiedName GetXmlSchema(System.Xml.Schema.XmlSchemaSet xs)
        {
            RmXmlSerializer.LoadBaseTypesSchema(xs);
            return new System.Xml.XmlQualifiedName(RmTypeName, RmXmlSerializer.OpenEhrNamespace);
        }

        const string RmTypeName = "GENERIC_ID";

        protected override bool IsValidValue(string value)
        {
            Check.Require(value != null, "value must not be null");

            return value != string.Empty;
        }
    }
}
