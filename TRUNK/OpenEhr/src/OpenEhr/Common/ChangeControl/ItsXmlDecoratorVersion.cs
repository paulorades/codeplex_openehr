//using System;
//using System.Collections.Generic;
//using EhrTypes = OpenEhr.V1.Its.Xml.RM;
//using OpenEhr.DesignByContract;
//using OpenEhr.RM.Support.Identification;
//using OpenEhr.Common.ChangeControl;
//using OpenEhr.Common.Generic;

//namespace OpenEhr.Common.ChangeControl
//{
//    [Serializable]
//    public abstract class ItsXmlDecoratorVersion<T>
//        : Version<T> where T : class
//    {
//        protected ItsXmlDecoratorVersion(EhrTypes.VERSION innerVersion)
//        {
//            DesignByContract.Check.Require(innerVersion != null, "innerVersion must not be null");

//            this.innerVersion = innerVersion;
//        }

//        protected ItsXmlDecoratorVersion(EhrTypes.VERSION innerVersion, Common.Generic.AuditDetails commitAudit,
//           Support.Identification.ObjectRef contribution)
//        {
//            Check.Require(innerVersion != null, "innerVersion must not be null");
//            Check.Require(commitAudit != null, "commitAudit must not be null.");

//            this.innerVersion = innerVersion;

//            // Set commitAudit and contribution in innerVersion
//            this.innerVersion.commit_audit = commitAudit.EhrType;
//            if (contribution != null)
//                this.innerVersion.contribution = contribution.InternalType;

//            // set local data
//            this.contribution = contribution;
//            this.commitAudit = commitAudit;
//        }

//        private EhrTypes.VERSION innerVersion;

//        internal EhrTypes.VERSION InnerVersion
//        {
//            get { return this.innerVersion; }
//        }

//        #region IVersion Members

//        public override string CanonicalForm
//        {
//            get { throw new Exception("The method or operation is not implemented."); }
//        }

//        public override bool IsBranch
//        {
//            get { throw new Exception("The method or operation is not implemented."); }
//        }

//        private AuditDetails commitAudit;

//        public override AuditDetails CommitAudit
//        {
//            get
//            {
//                if (this.commitAudit == null)
//                    this.commitAudit = WrapperFactory.CreateAuditDetails(this.InnerVersion.commit_audit);
//                return this.commitAudit;
//            }
//            protected set
//            {
//                Check.Require(value != null, "value must not be null");

//                this.innerVersion.commit_audit = value.EhrType;
//                this.commitAudit = value;
//            }
//        }

//        private Support.Identification.ObjectRef contribution;

//        public override ObjectRef Contribution
//        {
//            get
//            {
//                if (this.contribution == null)
//                    this.contribution = WrapperFactory.CreateObjectRef(this.InnerVersion.contribution)
//                        as OpenEhr.RM.Support.Identification.ObjectRef;
//                return this.contribution;
//            }
//            protected set
//            {
//                Check.Require(value != null, "contribution must not be null");
//                this.contribution = value;
//                this.innerVersion.contribution = value.InternalType;
//            }
//        }

//        private string signature;

//        public override string Signature
//        {
//            get
//            {
//                if (this.signature == null)
//                    this.signature = this.InnerVersion.signature;
//                return this.signature;
//            }
//        }

//        #endregion

//        internal protected override OpenEhr.V1.Its.Xml.RM.VERSION ToItsXmlVersion()
//        {
//            return this.innerVersion;
//        }

//        #region serialization
//        internal protected void ReadXml(System.Xml.XmlReader reader)
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
//                this.contribution = OpenEhr.RM.Support.Identification.ObjectRef.GetObjectRefByType(contributionType);
//            else
//                this.contribution = new ObjectRef();
//            this.contribution.ReadXml(reader);

//            Check.Assert(reader.LocalName == "commit_audit",
//                "Expected LocalName is 'commit_audit' rather than " + reader.LocalName);
//            this.commitAudit = new OpenEhr.Common.Generic.AuditDetails();
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
//            this.InnerVersion.contribution = this.contribution.InternalType;
//            this.InnerVersion.commit_audit = this.commitAudit.EhrType;
//            if (this.signature != null)
//                this.InnerVersion.signature = this.signature;

//        }

//        protected virtual void CheckInvariants()
//        {
//            Check.Invariant(this.Contribution != null, "contribution must not be null");
//            Check.Invariant(this.CommitAudit != null, "CommitAudit must not be null");
//        }
//        #endregion
//    }
//}
