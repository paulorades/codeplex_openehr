using System;
using OpenEhr.RM.DataTypes.Text;
using OpenEhr.AssumedTypes;
using OpenEhr.Attributes;
using OpenEhr.Serialisation;

namespace OpenEhr.RM.DataTypes.Quantity
{
    [Serializable]
    [RmType("openEHR", "DATA_TYPES", "DV_AMOUNT")]
    public abstract class DvAmount<T> : DvQuantified<T>, IFormattable
        where T: DvAmount<T>
    {
        #region constructor
        //protected DvAmount(EhrTypes.DV_AMOUNT dataValueType)
        //    : base(dataValueType)
        protected DvAmount()
            : base()
        {
            //this.CheckInvariants();           
        }

        #endregion

        const float unknownAccuracyValue = -1.0F;
        
        //private EhrTypes.DV_AMOUNT ehrType;
        //protected new EhrTypes.DV_AMOUNT EhrType
        //{
        //    get
        //    {
        //        if (this.ehrType == null)
        //            this.ehrType = base.DataValueType as EhrTypes.DV_AMOUNT;

        //        DesignByContract.Check.Ensure(ehrType != null, "DataValue Type must not be null");
        //        return ehrType;

        //    }
        //}

        // CM: 13/01/10 EHR-739 openEHR v1.0.2 accuracy is -1 when it is not set
        //private float accuracy = 0;
        private float accuracy = -1.0F;
       // private bool accuracySet;

        public float Accuracy
        {
            get
            {
                if (this.accuracy != unknownAccuracyValue)
                    return this.accuracy;

                //if (this.EhrType.accuracySpecified)
                //{
                //    this.accuracy = this.EhrType.accuracy;
                //   // this.accuracySet = true;
                //}
                return this.accuracy;
            }

        }

        private bool accuracyIsPercent;
        private bool accuracyIsPercentSet;

        public bool AccuracyIsPercent
        {
            get
            {

                //if (this.EhrType.accuracy_is_percentSpecified)
                //{
                //    this.accuracyIsPercent = this.EhrType.accuracy_is_percent;
                //    this.accuracyIsPercentSet = true;
                //}

                return this.accuracyIsPercent;
            }
           
        }

        public static DvAmount<T> operator -(DvAmount<T> a, DvAmount<T> b)
        {
           return a.Subtract(b);
        }

        protected abstract DvAmount<T> Subtract(DvAmount<T> b);
        protected abstract DvAmount<T> Plus(DvAmount<T> b);

        public static DvAmount<T> operator +(DvAmount<T> a, DvAmount<T> b)
        {
            return a.Plus(b);
        }

        // CM: 21/05/07
        // prefix "-": like Current
        public static DvAmount<T> operator -(DvAmount<T> b)
        {
            return b.GetDvAmountWithZeroMagnitude() - b;           
        }

        public override bool AccuracyUnknown()
        {
            return this.Accuracy == unknownAccuracyValue;
        }

        protected override object GetAccuracy()
        {
            return this.Accuracy;
        }

        public bool ValidPercentage()
        {
            return this.Accuracy >= 0.0 && this.Accuracy <= 100.0;
        }

        protected abstract DvAmount<T> GetDvAmountWithZeroMagnitude();

        protected void SetBaseData(float accuracy, bool accuracyIsPercent, string magnitudeStatus, CodePhrase normalStatus,
            DvInterval<T> normalRange, ReferenceRange<T>[] otherReferenceRanges)
        {
            DesignByContract.Check.Require(accuracy >= -1, "accuracy must be greater or equal to -1");           
            // this invariants checking should happen in RmValidator.
            //DesignByContract.Check.Require(!accuracyIsPercent || ValidPercentage(),
            //    "if accuracyIsPercent is true, accuracy must be within 0-100.");           

            base.SetBaseData(magnitudeStatus, normalStatus, normalRange, otherReferenceRanges);

            // CM: 07/05/09 this is not correct. accuracy is 0 means 100% correct. -1 means not recorded
            //if (accuracy != 0) // a value of 0 means that accuracy was not recorded.
            // means accuracy should be set
            if (accuracy != unknownAccuracyValue) 
            {
                this.accuracy = accuracy;
                //this.accuracySet = true;
                this.accuracyIsPercent = accuracyIsPercent;
                this.accuracyIsPercentSet = true;
            }
        }

        protected void SetBaseData(string magnitudeStatus, CodePhrase normalStatus, DvInterval<T> normalRange,
          ReferenceRange<T>[] otherReferenceRanges)
        {
            base.SetBaseData(magnitudeStatus, normalStatus, normalRange, otherReferenceRanges);
           
        }

        protected virtual void CheckInvariants()
        {
            DesignByContract.Check.Invariant(this.Accuracy != 0 || !AccuracyIsPercent);
            DesignByContract.Check.Invariant(!AccuracyIsPercent || ValidPercentage());
        }

        protected override void ReadXmlBase(System.Xml.XmlReader reader)
        {
            //base.ReadXml(reader, innerValue);
            //reader.ReadStartElement();
            //reader.MoveToContent();   
            base.ReadXmlBase(reader);

            if (reader.LocalName == "accuracy")
            {
                // only set accuracy when the value is greater than -1
                //this.accuracy = reader.ReadElementContentAsFloat("accuracy", XmlSerializer.OpenEhrNamespace);
                //this.accuracySet = true;
                float accuracyValue = reader.ReadElementContentAsFloat("accuracy", RmXmlSerializer.OpenEhrNamespace);
                if (accuracyValue < unknownAccuracyValue)
                    throw new ApplicationException("accuracy must be greater or equal to -1");
               
                this.accuracy = accuracyValue;
                //this.accuracySet = true;
            }
            if (reader.LocalName == "accuracy_is_percent")
            {
                this.accuracyIsPercent = reader.ReadElementContentAsBoolean("accuracy_is_percent",
                    RmXmlSerializer.OpenEhrNamespace);
                this.accuracyIsPercentSet = true;
            }

            if (!reader.IsStartElement())
            {
                reader.ReadEndElement();
                reader.MoveToContent();
            }
            
            //if (this.accuracySet || this.accuracyIsPercentSet)
            //    this.SetInnerData();
        }

        protected override void WriteXmlBase(System.Xml.XmlWriter writer)
        {
            base.WriteXmlBase(writer);
            string prefix = RmXmlSerializer.UseOpenEhrPrefix(writer); 

            //if (this.Accuracy != 0)
            //if(this.accuracySet)
            if(!this.AccuracyUnknown())
            {
                writer.WriteElementString(prefix, "accuracy", RmXmlSerializer.OpenEhrNamespace, this.Accuracy.ToString());
                writer.WriteElementString(prefix, "accuracy_is_percent", RmXmlSerializer.OpenEhrNamespace,
                        this.AccuracyIsPercent.ToString().ToLower());
            }
           
        }

        //protected override void SetInnerData()
        //{
        //    base.SetInnerData();
        //    //if (this.accuracySet)
        //    if(!this.AccuracyUnknown())
        //    {
        //        ((DV_AMOUNT)(this.DataValueType)).accuracy = this.accuracy;
        //        ((DV_AMOUNT)(this.DataValueType)).accuracySpecified = true;
        //    }
        //    if (this.accuracyIsPercentSet)
        //    {
        //        ((DV_AMOUNT)(this.DataValueType)).accuracy_is_percent = this.accuracyIsPercent;
        //        ((DV_AMOUNT)(this.DataValueType)).accuracy_is_percentSpecified = true;
        //    }
        //}


        #region IFormattable Members

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.ToString();
        }

        #endregion
    }
}
