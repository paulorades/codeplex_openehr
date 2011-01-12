using System;
using System.ComponentModel;
using OpenEhr.RM.DataTypes.Basic;
using OpenEhr.Attributes;
using OpenEhr.RM.Impl;
using OpenEhr.RM.DataTypes.Text.Impl;
using OpenEhr.Serialisation;

namespace OpenEhr.RM.DataTypes.Text
{
    [TypeConverter(typeof(CodePhraseTypeConverter))]
    [System.Xml.Serialization.XmlSchemaProvider("GetXmlSchema")]
    [Serializable]
    [RmType("openEHR", "DATA_TYPES", "CODE_PHRASE")]
    public class CodePhrase : RmType, System.Xml.Serialization.IXmlSerializable
    {
        public CodePhrase()
            : base()
        { }

        public CodePhrase(string codeString, string terminologyId) 
            : this()
		{
			this.terminologyId = new Support.Identification.TerminologyId(terminologyId);
			this.codeString = codeString;
		}

        Support.Identification.TerminologyId terminologyId;

        public Support.Identification.TerminologyId TerminologyId
        {
            get 
            { 
                return this.terminologyId;
            }
            set
            {
                this.terminologyId = value;
            }
        }

        private string codeString;

        public string CodeString
        {
            get
            {
                return this.codeString;
            }
            set
            {
                this.codeString = value;
            }
        }

        public override string ToString()
        {
            return this.TerminologyId.Value + "::" + this.CodeString;
        }

        public override bool Equals(object obj)
        {
            CodePhrase codePhrase = obj as CodePhrase;
            if (codePhrase == null)
                return false;

            if (this.CodeString != codePhrase.CodeString)
                return false;

            return this.TerminologyId.Equals(codePhrase.TerminologyId);
        }

        #region IXmlSerializable Members
        
        internal virtual void ReadXml(System.Xml.XmlReader reader)
        {
            reader.ReadStartElement();
            reader.MoveToContent();

            string openEhrNamespace = RmXmlSerializer.OpenEhrNamespace;

            // terminologyId
            if(this.terminologyId == null)
            this.terminologyId = 
                new OpenEhr.RM.Support.Identification.TerminologyId();
            this.terminologyId.ReadXml(reader);
            //this.terminologyIdSet = true;

            // codeString
            reader.MoveToContent();
            this.codeString = reader.ReadElementString("code_string", openEhrNamespace);
            //this.codeStringSet = true;

            //while (!reader.IsStartElement() && reader.NodeType != System.Xml.XmlNodeType.None)
            //{
                reader.ReadEndElement();
                reader.MoveToContent();
            //}

            //this.SetInnerData();
        }

        //private void SetInnerData()
        //{
        //    DesignByContract.Check.Require(this.codePhraseType != null, "Inner type must not be null");
        //    DesignByContract.Check.Require(this.codeString != null, "Code string must not be null");
        //    DesignByContract.Check.Require(this.terminologyId != null, "Terminology ID must not be null");

        //    this.codePhraseType.code_string = this.codeString;
        //    this.codePhraseType.terminology_id = this.terminologyId.EhrType;
        //}

        internal virtual void WriteXml(System.Xml.XmlWriter writer)
        {
            string openEhrNamespace = RmXmlSerializer.OpenEhrNamespace;
            string prefix = RmXmlSerializer.UseOpenEhrPrefix(writer); 

            writer.WriteStartElement("terminology_id", openEhrNamespace);
            this.TerminologyId.WriteXml(writer);
            writer.WriteEndElement();

            writer.WriteElementString(prefix, "code_string", openEhrNamespace, this.CodeString);
            
        }

        public static System.Xml.XmlQualifiedName GetXmlSchema(System.Xml.Schema.XmlSchemaSet xs)
        {
            //return Basic.DataValue.GetXmlSchema(xs, "CODE_PHRASE");
            RmXmlSerializer.LoadBaseTypesSchema(xs);
            return new System.Xml.XmlQualifiedName("CODE_PHRASE", RmXmlSerializer.OpenEhrNamespace);

        }

        #endregion

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
    }
}
