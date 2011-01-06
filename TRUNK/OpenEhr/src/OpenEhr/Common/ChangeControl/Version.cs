//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Runtime.Serialization;
//using OceanEhr.OpenEhrV1.Composition;
//using EhrTypes = OpenEhr.V1.Its.Xml.RM;
//using DesignByContract;
//using OceanEhr.OpenEhrV1.Support.Identification;
//using System.IO;
//using System.Xml;

//namespace OceanEhr.OpenEhrV1.Common.ChangeControl
//{
//    [Serializable]
//    [Obsolete("Use OriginalVersion<T>")]
//    public class Version : IVersion, IItsXmlConvertible
//    {
//        public static Version Deserialize(System.Xml.XmlReader reader)
//        {
//            Check.Require(reader != null, "reader must not be null");

//            EhrTypes.ORIGINAL_VERSION versionType
//                = OpenEhr.V1.Its.Xml.XmlSerializer.DeserializeVersion(reader) as EhrTypes.ORIGINAL_VERSION;

//            return new Version(versionType);
//        }

//        //public static Version Deserialize(string versionXml)
//        //{
//        //    EhrTypes.ORIGINAL_VERSION versionType
//        //        = OpenEhr.V1.Its.Xml.XmlSerializer.DeserializeVersion(versionXml) as EhrTypes.ORIGINAL_VERSION;

//        //    return new Version(versionType);
//        //}

//        static public string Serialize(Version version)
//        {
//            Check.Require(version != null, "version must not be null");

//            StringBuilder builder = new StringBuilder();
//            TextWriter writer = new StringWriter(builder);

//            Version.Serialize(writer, version);

//            writer.Close();

//            return builder.ToString();
//        }

//        static public void Serialize(TextWriter writer, Version version)
//        {
//            Check.Require(version != null, "version must not be null");
//            Check.Require(writer != null, "writer must not be null");

//            EhrTypes.VERSION innerVersion = version.InternalType;
//            XmlTextWriter xmlWriter = new XmlTextWriter(writer);
//            OpenEhr.V1.Its.Xml.XmlSerializer.Serialize(xmlWriter, innerVersion);
//        }

//        internal Version(EhrTypes.ORIGINAL_VERSION versionType)
//        {
//            this.versionType = versionType;
//        }

//        private EhrTypes.ORIGINAL_VERSION versionType;
//        internal EhrTypes.VERSION InternalType
//        {
//            get { return this.versionType; }
//        }

//        #region IVersion Members

//        private Support.Identification.ObjectVersionId uid;
//        public Support.Identification.ObjectVersionId Uid
//        {
//            get
//            {
//                // CM: 08/02/08
//                if (this.uid == null)
//                    this.uid = WrapperFactory.CreateObjectId(this.versionType.uid) as ObjectVersionId;
//                return this.uid;
//                //return new Support.Identification.ObjectVersionId(versionType.uid.value); 
//            }
//        }

//        private Support.Identification.ObjectVersionId precedingVersionUid;
//        public Support.Identification.ObjectVersionId PrecedingVersionUid
//        {
//            get
//            {
//                if (this.precedingVersionUid == null)
//                {
//                    if (versionType.preceding_version_uid != null)
//                        this.precedingVersionUid = new Support.Identification.ObjectVersionId(versionType.preceding_version_uid.value);

//                }

//                return this.precedingVersionUid;
//                //if (versionType.preceding_version_uid != null)
//                //    return new Support.Identification.ObjectVersionId(versionType.preceding_version_uid.value);
//                //else
//                //    return null;
//            }
//        }

//        private DataTypes.Text.DvCodedText lifecycleState;
//        public DataTypes.Text.DvCodedText LifecycleState
//        {
//            get
//            {
//                if (this.lifecycleState == null)
//                    this.lifecycleState = WrapperFactory.CreateDataValue(this.versionType.lifecycle_state)
//                        as OceanEhr.OpenEhrV1.DataTypes.Text.DvCodedText;
//                return this.lifecycleState;
//            }
//        }

//        public Support.Identification.HierObjectId OwnerId
//        {
//            get { return new OceanEhr.OpenEhrV1.Support.Identification.HierObjectId(this.Uid.ObjectId.Value); }
//        }

//        public bool IsBranch
//        {
//            get { throw new Exception("The method or operation is not implemented."); }
//        }

//        private Support.Identification.ObjectRef contribution;
//        public Support.Identification.ObjectRef Contribution
//        {
//            get
//            {
//                // CM: 08/02/08
//                if (this.contribution == null)
//                    this.contribution = WrapperFactory.CreateObjectRef(this.versionType.contribution)
//                        as OceanEhr.OpenEhrV1.Support.Identification.ObjectRef;
//                return this.contribution;
//            }
//        }

//        private Generic.AuditDetails commitAudit;

//        public Generic.AuditDetails CommitAudit
//        {
//            get
//            {
//                if (this.commitAudit == null)
//                    this.commitAudit = WrapperFactory.CreateAuditDetails(this.versionType.commit_audit);
//                return this.commitAudit;
//            }
//            //set
//            //{
//            //    Check.Require(value != null, "CommitAudit must not be null");

//            //    this.commitAudit = value;
//            //    this.versionType.commit_audit = value.EhrType;
//            //}
//        }

//        //public List<EhrTypes.ATTESTATION> Attestations
//        //{
//        //    get { throw new Exception("The method or operation is not implemented."); }
//        //    set { throw new Exception("The method or operation is not implemented."); }
//        //}

//        private object data;
//        public object Data
//        {
//            get
//            {
//                if (this.data == null)
//                    this.data = new Composition.Composition(versionType.data as EhrTypes.COMPOSITION);
//                return this.data;
//            }
//        }

//        public string CanonicalForm
//        {
//            get { throw new Exception("The method or operation is not implemented."); }
//        }

//        private string signature;
//        public string Signature
//        {
//            get
//            {
//                if (this.signature == null)
//                    this.signature = this.versionType.signature;
//                return this.signature;
//            }
//        }

//        #endregion


//        #region IItsXmlConvertable Members

//        //object IItsXmlConvertable.ToItsXmlType()
//        object IItsXmlConvertible.ToItsXmlType()
//        {
//            return this.versionType;
//        }

//        #endregion

//        #region openEhrV1 serialization
//        internal void ReadXml(System.Xml.XmlReader reader)
//        {
//            reader.ReadStartElement();
//            reader.MoveToContent();

//            this.ReadXmlBase(reader);

//            Check.Assert(reader.NodeType == System.Xml.XmlNodeType.EndElement, "Expect an endElement");
//            reader.ReadEndElement();
//            reader.MoveToContent();

//            this.SetInnerData();
//            this.CheckInvariants();
//        }

//        internal void WriteXml(System.Xml.XmlWriter writer)
//        {
//            this.WriteXmlBase(writer);
//        }

//        protected virtual void ReadXmlBase(System.Xml.XmlReader reader)
//        {
//            Check.Assert(reader.LocalName == "contribution",
//                "Expected LocalName is 'contribution' rather than " + reader.LocalName);
//            string contributionType = reader.GetAttribute("type", XmlSerializer.XsiNamespace);
//            if (contributionType != null)
//                this.contribution = ObjectRef.GetObjectRefByType(contributionType);
//            else
//                this.contribution = new ObjectRef();
//            this.contribution.ReadXml(reader);

//            Check.Assert(reader.LocalName == "commit_audit",
//                "Expected LocalName is 'commit_audit' rather than " + reader.LocalName);
//            this.commitAudit = new OceanEhr.OpenEhrV1.Common.Generic.AuditDetails();
//            this.commitAudit.ReadXml(reader);

//            if (reader.LocalName == "signature")
//            {
//                this.signature = reader.ReadElementString("signature", XmlSerializer.OpenEhrNamespace);
//            }

//            reader.MoveToContent();

//        }

//        protected virtual void WriteXmlBase(System.Xml.XmlWriter writer)
//        {
//            this.CheckInvariants();

//            string prefix = XmlSerializer.UseOpenEhrPrefix(writer);
//            string xsiPrefix = XmlSerializer.UseXsiPrefix(writer);

//            writer.WriteStartElement("contribution", XmlSerializer.OpenEhrNamespace);
//            if (this.Contribution.GetType() != typeof(ObjectRef))
//            {
//                string contriType = ((IRmType)this.Contribution).GetRmTypeName();
//                if (!string.IsNullOrEmpty(prefix))
//                    contriType = prefix + ":" + contriType;
//                writer.WriteAttributeString(xsiPrefix, "type", XmlSerializer.XsiNamespace, contriType);
//            }
//            this.Contribution.WriteXml(writer);
//            writer.WriteEndElement();

//            writer.WriteStartElement("commit_audit", XmlSerializer.OpenEhrNamespace);
//            this.CommitAudit.WriteXml(writer);
//            writer.WriteEndElement();
//        }

//        protected virtual void SetInnerData()
//        {
//            this.versionType.contribution = this.contribution.InternalType;
//            this.versionType.commit_audit = this.commitAudit.EhrType;
//            if (this.signature != null)
//                this.versionType.signature = this.signature;

//            //this.CheckInvariants();
//        }

//        protected virtual void CheckInvariants()
//        {
//            Check.Invariant(this.Contribution != null, "contribution must not be null");
//            Check.Invariant(this.CommitAudit != null, "CommitAudit must not be null");
//        }
//        #endregion
//    }
//}
