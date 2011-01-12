using System;
using OpenEhr.Attributes;
using OpenEhr.RM.DataStructures.ItemStructure;
using OpenEhr.RM.DataTypes.Text;
using OpenEhr.RM.Common.Archetyped.Impl;
using OpenEhr.RM.Common.Generic;
using OpenEhr.RM.Support.Identification;
using OpenEhr.RM.DataTypes.Quantity.DateTime;
using OpenEhr.DesignByContract;
using OpenEhr.Serialisation;
using OpenEhr.RM.Impl;

namespace OpenEhr.RM.Composition.Content.Entry
{
    [System.Xml.Serialization.XmlSchemaProvider("GetXmlSchema")]
    [Serializable]
    [RmType("openEHR", "EHR", "ACTION")]
    public class Action : CareEntry, System.Xml.Serialization.IXmlSerializable
    {
        public Action() //: base(new EhrTypes.ACTION()) 
            :base()
        { }

        public Action(DvText name, string archetypeNodeId, UidBasedId uid,
         Link[] links, Archetyped archetypeDetails, FeederAudit feederAudit,
         CodePhrase language, CodePhrase encoding, PartyProxy subject, PartyProxy proider,
         Participation[] otherParticipations, ObjectRef workflowId, ItemStructure protocol,
         ObjectRef guidelineId, DvDateTime time, ItemStructure description,
            IsmTransition ismTransition, InstructionDetails instructionDetails)
            : base(name, archetypeNodeId, uid, links, archetypeDetails, feederAudit, language,
          encoding, subject, proider, otherParticipations, workflowId, protocol, guidelineId)
        {
            Check.Require(time != null, "time must not be null");
            Check.Require(description != null, "description must not be null");
            Check.Require(ismTransition != null, "ismTransition must not be null");

            this.time = time;
            this.description = description;
            if (this.description != null)
                this.description.Parent = this;
            this.ismTransition = ismTransition;
            if (this.ismTransition != null)
                this.ismTransition.Parent = this;
            this.instructionDetails = instructionDetails;
            if (this.instructionDetails != null)
                this.instructionDetails.Parent = this;

            SetAttributeDictionary();
            //SetInnerData();
            CheckInvariants();
        }

        //internal Action(EhrTypes.ACTION entryType) : base(entryType) 
        //{            
        //    this.CheckInvariantsDefault();
        //    //// add attributes to AttributesDictionary
        //    //// TIME
        //    ////DataTypes.Quantity.DateTime.DvDateTime time = WrapperFactory.CreateDataValue(entryType.time) as DataTypes.Quantity.DateTime.DvDateTime;
        //    //this.time = WrapperFactory.CreateDataValue(entryType.time) as DataTypes.Quantity.DateTime.DvDateTime;
        //    ////if (time == null)
        //    ////    time = new OpenEhr.RM.DataTypes.Quantity.DateTime.DvDateTime(new System.DateTime());

        //    ////base.attributesDictionary.Add("time", this.time);
           
        //    //// description
        //    //DesignByContract.Check.Assert(entryType.description != null, "Description must not be null in an Action instance");

        //    ////DataStructures.ItemStructure.ItemStructure description = WrapperFactory.CreateLocatable(entryType.description) as DataStructures.ItemStructure.ItemStructure;
        //    //this.description = WrapperFactory.CreateLocatable(entryType.description) as DataStructures.ItemStructure.ItemStructure;
        //    //description.SetParent(this);
        //    ////this.description = description;

        //    ////Support.Assumed.LocatableList<Common.Archetyped.Locatable> descriptionList =
        //    ////    new OpenEhr.Support.Assumed.LocatableList<OpenEhr.Common.Archetyped.Locatable>();                       

        //    ////descriptionList.Add(description);
            
        //    ////base.attributesDictionary.Add("description", this.description);

        //    //// add ism_transition
        //    ////IsmTransition ismTransition = null;
        //    //if (entryType.ism_transition != null)
        //    //{
        //    //    this.ismTransition = WrapperFactory.CreateIsmTransition(entryType.ism_transition);
        //    //    this.ismTransition.SetParent(this);
        //    //}
        //    ////base.attributesDictionary.Add("ism_transition", this.ismTransition);
            
            
        //    //// Add instruction_details
        //    ////InstructionDetails instructionDetails = null;
        //    //if (entryType.instruction_details != null)
        //    //{
        //    //    instructionDetails = WrapperFactory.CreateInstructionDetails(entryType.instruction_details);
        //    //    instructionDetails.SetParent(this);
        //    //}
        //    ////this.instructionDetails = instructionDetails;
        //    ////base.attributesDictionary.Add("instruction_details", instructionDetails);

        //    //this.SetAttributeDictionary();
        //    //this.CheckInvariants();
        //}

        //private EhrTypes.ACTION innerType;
        //private EhrTypes.ACTION InnerType
        //{
        //    get
        //    {
        //        if (innerType == null)
        //            innerType = base.LocatableType as EhrTypes.ACTION;

        //        DesignByContract.Check.Ensure(innerType != null, "InnerType must not be null");
        //        return innerType;
        //    }
        //}

        private DataTypes.Quantity.DateTime.DvDateTime time;

        [RmAttribute("time")]
        public DataTypes.Quantity.DateTime.DvDateTime Time
        {
            get
            {
                if(this.time == null)
                    this.time = base.attributesDictionary["time"] as DataTypes.Quantity.DateTime.DvDateTime;
                return this.time;
                //return base.attributesDictionary["time"] as DataTypes.Quantity.DateTime.DvDateTime;
            }
            set
            {
                this.time = value;
                base.attributesDictionary["time"] = this.time;
            }
        }

        private DataStructures.ItemStructure.ItemStructure description;

        [RmAttribute("description")]
        public DataStructures.ItemStructure.ItemStructure Description
        {
            get
            {
                if(this.description == null)
                    this.description = base.attributesDictionary["description"] as DataStructures.ItemStructure.ItemStructure;
                return this.description;
                //return base.attributesDictionary["description"] as DataStructures.ItemStructure.ItemStructure;
            }
            set
            {
                if (this.description != null)
                    this.description.Parent = null;
                this.description = value;
                if (this.description != null)
                    this.description.Parent = this;
                base.attributesDictionary["description"] = this.description;
            }
        }

        private IsmTransition ismTransition;


        [RmAttribute("ism_transition", 1)]
        public IsmTransition IsmTransition
        {
            get
            {
                if (this.ismTransition == null)
                    this.ismTransition = base.attributesDictionary["ism_transition"] as IsmTransition;
                return this.ismTransition;
            }
            set
            {
                Check.Require(value != null, "value must not be null.");
                this.ismTransition = value;
                base.attributesDictionary["ism_transition"] = this.ismTransition;
            }
        }

        private InstructionDetails instructionDetails;

        [RmAttribute("instruction_details")]
        public InstructionDetails InstructionDetails
        {
            get
            {
                if (this.instructionDetails == null)
                    this.instructionDetails = base.attributesDictionary["instruction_details"] as InstructionDetails;
                return this.instructionDetails;
            }
            set
            {
                this.instructionDetails = value;
                base.attributesDictionary["instruction_details"] = this.instructionDetails;
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

        public static System.Xml.XmlQualifiedName GetXmlSchema(System.Xml.Schema.XmlSchemaSet xs)
        {
            RmXmlSerializer.LoadCompositionSchema(xs);
            return new System.Xml.XmlQualifiedName("ACTION", RmXmlSerializer.OpenEhrNamespace);
        }

        protected override void ReadXmlBase(System.Xml.XmlReader reader)
        {
            base.ReadXmlBase(reader);

            DesignByContract.Check.Assert(reader.LocalName == "time",
                "Expected LocalName is 'time', but it is " + reader.LocalName);
            this.time = new OpenEhr.RM.DataTypes.Quantity.DateTime.DvDateTime();
            this.time.ReadXml(reader);

            DesignByContract.Check.Assert(reader.LocalName == "description",
               "Expected LocalName is 'description', but it is " + reader.LocalName);
            string descriptionType = reader.GetAttribute("type", RmXmlSerializer.XsiNamespace);
            this.description = OpenEhr.RM.Common.Archetyped.Impl.Locatable.GetLocatableObjectByType(descriptionType)
                as ItemStructure;
            if (this.description == null)
                throw new InvalidOperationException("descriptionType in Action must be type of ItemStructure: " + descriptionType);
            this.description.ReadXml(reader);
            this.description.Parent = this;
            

            if (reader.LocalName == "ism_transition")
            {
                this.ismTransition = new IsmTransition();
                this.ismTransition.ReadXml(reader);  
            }

            if (reader.LocalName == "instruction_details")
            {
                this.instructionDetails = new InstructionDetails();
                this.instructionDetails.ReadXml(reader);
            }

        }

        protected override void WriteXmlBase(System.Xml.XmlWriter writer)
        {
            //this.CheckInvariants();

            base.WriteXmlBase(writer);

            string openEhrPrefix = RmXmlSerializer.UseOpenEhrPrefix(writer);
            string xsiPrefix = RmXmlSerializer.UseXsiPrefix(writer);

            writer.WriteStartElement(openEhrPrefix, "time", RmXmlSerializer.OpenEhrNamespace);
            this.Time.WriteXml(writer);
            writer.WriteEndElement();

            writer.WriteStartElement(openEhrPrefix, "description", RmXmlSerializer.OpenEhrNamespace);
            string descriptionType = ((IRmType)this.Description).GetRmTypeName();
            if (!string.IsNullOrEmpty(openEhrPrefix))
                descriptionType = openEhrPrefix + ":" + descriptionType;
            writer.WriteAttributeString(xsiPrefix, "type", RmXmlSerializer.XsiNamespace, descriptionType);
            this.Description.WriteXml(writer);
            writer.WriteEndElement();

            // ism_transition
            if (this.IsmTransition != null)
            {
                writer.WriteStartElement(openEhrPrefix, "ism_transition", RmXmlSerializer.OpenEhrNamespace);
                this.IsmTransition.WriteXml(writer);
                writer.WriteEndElement();
            }

            // Instruction_details
            if (this.InstructionDetails != null)
            {
                writer.WriteStartElement(openEhrPrefix, "instruction_details", RmXmlSerializer.OpenEhrNamespace);
                this.InstructionDetails.WriteXml(writer);
                writer.WriteEndElement();
            }
        }

        //protected override void SetInnerData()
        //{
        //    if (this.innerType == null && this.LocatableType == null)
        //        SetInnerType(new OpenEhr.V1.Its.Xml.RM.ACTION());

        //    base.SetInnerData();

        //    //SetAttributeDictionaryData("time", this.time);
        //    //SetAttributeDictionaryData("description", this.description);
        //    //SetAttributeDictionaryData("ism_transition", this.ismTransition);
        //    //SetAttributeDictionaryData("instruction_details", this.instructionDetails);

        //    this.InnerType.time = this.time.DataValueType as EhrTypes.DV_DATE_TIME;
        //    this.InnerType.description = this.description.LocatableType as EhrTypes.ITEM_STRUCTURE;

        //    if(this.ismTransition!= null)
        //        this.InnerType.ism_transition =
        //            //((IItsXmlConvertable)(this.ismTransition)).ToItsXmlType() as EhrTypes.ISM_TRANSITION;
        //            ((IItsXmlConvertible)(this.ismTransition)).ToItsXmlType() as EhrTypes.ISM_TRANSITION;

        //    if(this.instructionDetails != null)
        //        this.InnerType.instruction_details = 
        //            //((IItsXmlConvertable)(this.instructionDetails)).ToItsXmlType() as EhrTypes.INSTRUCTION_DETAILS;
        //            ((IItsXmlConvertible)(this.instructionDetails)).ToItsXmlType() as EhrTypes.INSTRUCTION_DETAILS;
            
        //    //this.CheckInvariants();

        //}

        protected override void CheckInvariants()
        {
            base.CheckInvariants();

            //DesignByContract.Check.Invariant(this.Time != null, "Time must not be null.");
            //DesignByContract.Check.Invariant(this.Description != null, "Description must not be null.");

            // TODO: Ism_transition_valid: ism_transition /= Void
        }

        protected void CheckInvariantsDefault()
        {
            base.CheckInvariantsDefault();
            //DesignByContract.Check.Invariant(this.Time != null, "Time must not be null.");
        }

        protected override void SetAttributeDictionary()
        {
            base.SetAttributeDictionary();

           base.attributesDictionary["time"] = this.time;
             base.attributesDictionary["description"] = this.description;
           base.attributesDictionary["ism_transition"] = this.ismTransition;
           base.attributesDictionary["instruction_details"] =this.instructionDetails;

        }

        //protected override void InitialiseAttributeDictionary()
        //{
        //    base.InitialiseAttributeDictionary();

        //    base.attributesDictionary.Add("time", WrapperFactory.CreateDataValue(this.InnerType.time)
        //        as DataTypes.Quantity.DateTime.DvDateTime);

        //    // description
        //    //DesignByContract.Check.Assert(entryType.description != null, "Description must not be null in an Action instance");

        //    DataStructures.ItemStructure.ItemStructure description = null;
        //    if (this.InnerType.description != null)
        //    {
        //        description = WrapperFactory.CreateLocatable(this.InnerType.description) as DataStructures.ItemStructure.ItemStructure;
        //        DesignByContract.Check.Assert(description != null, "description must not be null.");
        //        description.Parent = this;
        //    }
        //    base.attributesDictionary.Add("description", description);

        //    // add ism_transition
        //    IsmTransition ismTransition = null;
        //    if (this.InnerType.ism_transition != null)
        //    {
        //        ismTransition = WrapperFactory.CreateIsmTransition(this.InnerType.ism_transition);
        //        ismTransition.Parent = this;
        //    }
        //    base.attributesDictionary.Add("ism_transition", ismTransition);


        //    // Add instruction_details
        //    InstructionDetails instructionDetails = null;
        //    if (this.InnerType.instruction_details != null)
        //    {
        //        instructionDetails = WrapperFactory.CreateInstructionDetails(this.InnerType.instruction_details);
        //        instructionDetails.Parent = this;
        //    }
        //    base.attributesDictionary.Add("instruction_details", instructionDetails);

        //}
    }
}
