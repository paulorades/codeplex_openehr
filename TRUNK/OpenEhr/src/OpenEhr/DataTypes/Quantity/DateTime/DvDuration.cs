using System;
//using System.Collections.Generic;
//using EhrTypes = OpenEhr.V1.Its.Xml.RM;
using System.Text.RegularExpressions;
using OpenEhr.DesignByContract;
using OpenEhr.RM.DataTypes.Text;
using OpenEhr.AssumedTypes;
using OpenEhr.Attributes;
using OpenEhr.Serialisation;

namespace OpenEhr.RM.DataTypes.Quantity.DateTime
{
    /// <summary>
    /// Represents a period of time with respect to a notional point in time, which is not specified.
    /// A sign may be used to indicate the duration is "backwards" in time rather than forwards.
    /// </summary>
    [System.Xml.Serialization.XmlSchemaProvider("GetXmlSchema")]
    [Serializable]
    [RmType("openEHR", "DATA_TYPES", "DV_DURATION")]
    public class DvDuration : DvAmount<DvDuration>, System.Xml.Serialization.IXmlSerializable 
    {
        private AssumedTypes.Iso8601Duration isoDuration;

        //internal DvDuration(EhrTypes.DV_DURATION duration): base(duration)
        //{
        //    Check.Require(duration != null && !string.IsNullOrEmpty(duration.value));
        //    this.isoDuration = new Support.Assumed.Iso8601Duration(duration.value);

        //    this.value = this.Value;           
        //}

        public DvDuration() 
            : this("PT0S") 
        { }

        public DvDuration(string durationString, float accuracy, bool accuracyIsPercent, string magnitudeStatus,
            //CodePhrase normalStatus, DvInterval<DvDuration> normalRange, List<ReferenceRange<DvDuration>> otherReferenceRanges)
            //: base(new EhrTypes.DV_DURATION())
            CodePhrase normalStatus, DvInterval<DvDuration> normalRange, ReferenceRange<DvDuration>[] otherReferenceRanges)
            : base()
        {
            Check.Require(!string.IsNullOrEmpty(durationString), "durationString must not be null or empty.");

            this.isoDuration =
                new OpenEhr.AssumedTypes.Iso8601Duration(durationString);
            //this.value = this.isoDuration.ToString();

            base.SetBaseData(accuracy, accuracyIsPercent, magnitudeStatus, normalStatus, normalRange, otherReferenceRanges);

            //SetInnerData();
            CheckInvariants();
        }


        public DvDuration(string durationString) 
            : this(durationString, -1, false, null, null, null, null) 
        { }

        public DvDuration(int years, int months, int days, int weeks, int hours,
            int minutes, int seconds, double fractionalSeconds)
            //: base(new EhrTypes.DV_DURATION())
            : base()
        {
            this.isoDuration = new OpenEhr.AssumedTypes.Iso8601Duration
                (years, months, days, weeks, hours, minutes, seconds, fractionalSeconds);

            //this.value = this.isoDuration.ToString();
            //SetInnerData();
            CheckInvariants();
        }

        //private string value;

        [RmAttribute("value", 1)]
        public string Value
        {
            get
            {
                //if (this.value == null)
                //    this.value = this.DurationType.value;
                //return this.value;
                return this.isoDuration.ToString();
            }
            //internal set
            //{
            //    this.value = value;
            //    ((OpenEhr.V1.Its.Xml.RM.DV_DURATION)(this.DataValueType)).value = this.value;
            //}
        }

        //protected EhrTypes.DV_DURATION DurationType
        //{
        //    get { return base.DataValueType as EhrTypes.DV_DURATION; }
        //}

        // numeric value of duration in seconds

        protected override double GetMagnitude()
        {
            Check.Require(this.isoDuration != null,
            "The ISO duration instance cannot be null.");

            return isoDuration.DurationValueInSeconds;
        }

        // CM: 30/05/07
        public double Magnitude
        {
            get
            {
                return this.GetMagnitude();
            }
        }

        public override String ToString()
        {
            return this.Value;
        }

        protected override DvAmount<DvDuration> Subtract(DvAmount<DvDuration> b)
        {
            DesignByContract.Check.Require(this.IsStrictlyComparableTo(b));

            DvDuration bObj = b as DvDuration;
            double result = this.Magnitude - bObj.Magnitude;
            return DvDuration.GetDurationByMagnitude(result);
        }

        protected override DvAmount<DvDuration> Plus(DvAmount<DvDuration> b)
        {
            DesignByContract.Check.Require(this.IsStrictlyComparableTo(b));

            DvDuration bObj = b as DvDuration;
            double result = this.Magnitude + bObj.Magnitude;
            return DvDuration.GetDurationByMagnitude(result);
        }

        public override bool IsStrictlyComparableTo(DvOrdered<DvDuration> other)
        {
            DesignByContract.Check.Require(other!=null);

            if (other is DvDuration)
                return true;
            return false;
        }

        protected override DvAmount<DvDuration> GetDvAmountWithZeroMagnitude()
        {
            return new DvDuration(0, 0, 0, 0, 0, 0, 0, 0);
        }

        /// <summary>
        /// Returns DvDuration instance with the given magnitude
        /// </summary>
        /// <param name="magnitude">magnitude is the numeric value of the duration as seconds
        /// </param>
        /// <returns></returns>
        internal static DvDuration GetDurationByMagnitude(double magnitude)
        {
            DesignByContract.Check.Require(magnitude >= 0);

            int secondsInDay = AssumedTypes.TimeDefinitions.secondsInMinute *
                AssumedTypes.TimeDefinitions.minutesInHour *
                AssumedTypes.TimeDefinitions.hoursInDay;

            //int totalDaysInMagnitude = (int)(Math.Truncate(magnitude/secondsInDay));
            double totalDaysInMagnitude = magnitude / secondsInDay;

            int yearInMagnitude = 0;
            int monthInMagnitude = 0;
            int dateInMagnitude = 0;
           
            if (totalDaysInMagnitude > 0)
            {
                
                if (totalDaysInMagnitude >= AssumedTypes.Iso8601Duration.nominalDaysInYear)
                    yearInMagnitude = (int)Math.Truncate(totalDaysInMagnitude / AssumedTypes.TimeDefinitions.nominalDaysInYear);

                double remainderDays = totalDaysInMagnitude - yearInMagnitude * AssumedTypes.TimeDefinitions.nominalDaysInYear;
                               
                if (remainderDays >= AssumedTypes.Iso8601Duration.nominalDaysInMonth)
                    monthInMagnitude = (int)(Math.Truncate(remainderDays / AssumedTypes.Iso8601Duration.nominalDaysInMonth));

                 dateInMagnitude = (int)(Math.Truncate(remainderDays - monthInMagnitude * AssumedTypes.Iso8601Duration.nominalDaysInMonth));
            }

            //double remainderSeconds = magnitude - dateInMagnitude * secondsInDay;
            double remainderSeconds = magnitude - (yearInMagnitude * AssumedTypes.Iso8601Duration.nominalDaysInYear
                 + monthInMagnitude * AssumedTypes.Iso8601Duration.nominalDaysInMonth + dateInMagnitude)
                 * secondsInDay;

            int secondsInHour = AssumedTypes.Iso8601Duration.secondsInMinute *
                AssumedTypes.Iso8601Duration.minutesInHour;

            int hourInMagnitude = 0;
            if(remainderSeconds>=secondsInHour)
                hourInMagnitude = (int)(Math.Truncate(remainderSeconds / secondsInHour));

            remainderSeconds = remainderSeconds - hourInMagnitude* secondsInHour;
            int minutes = 0;
            if (remainderSeconds >= AssumedTypes.Iso8601Duration.secondsInMinute)
                minutes = (int)(Math.Truncate(remainderSeconds / AssumedTypes.Iso8601Duration.secondsInMinute));

            remainderSeconds = remainderSeconds-minutes*AssumedTypes.Iso8601Duration.secondsInMinute;
            int secondsInMagnitude = (int)(Math.Truncate(remainderSeconds));
            double fractionalSeconds = remainderSeconds - secondsInMagnitude;

            //double secondsInYear = AssumedTypes.TimeDefinitions.daysInYear*
            //    Support.Assumed.TimeDefinitions.hoursInDay * 
            //    Support.Assumed.TimeDefinitions.minutesInHour *
            //    Support.Assumed.TimeDefinitions.secondsInMinute;

            //int yearsInMagnitude = (int)(Math.Truncate(magnitude/secondsInYear));

            return new DvDuration(yearInMagnitude, monthInMagnitude, dateInMagnitude, 0, hourInMagnitude, minutes, secondsInMagnitude, fractionalSeconds);
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
            return new System.Xml.XmlQualifiedName("DV_DURATION", RmXmlSerializer.OpenEhrNamespace);
        }

        protected override void ReadXmlBase(System.Xml.XmlReader reader)
        {
            base.ReadXmlBase(reader);

            // Get value
            Check.Assert(reader.LocalName == "value", "reader.LocalName must be 'value'");
            string value = reader.ReadElementString("value", RmXmlSerializer.OpenEhrNamespace);

            reader.MoveToContent();

            this.isoDuration = new OpenEhr.AssumedTypes.Iso8601Duration(value);
        }

        protected override void WriteXmlBase(System.Xml.XmlWriter writer)
        {
            base.WriteXmlBase(writer);

            string prefix = RmXmlSerializer.UseOpenEhrPrefix(writer);

            Check.Assert(!string.IsNullOrEmpty(this.Value), "value must not be null or empty.");

            //writer.WriteElementString(prefix, "value", XmlSerializer.OpenEhrNamespace, this.Value);
            string durationValue = this.Value;
            durationValue = durationValue.Replace(',', '.');
            writer.WriteElementString(prefix, "value", RmXmlSerializer.OpenEhrNamespace, durationValue);
        }

        //protected override void SetInnerData()
        //{
        //    base.SetInnerData();
        //    ((OpenEhr.V1.Its.Xml.RM.DV_DURATION)(this.DataValueType)).value = this.value;

        //    //CheckInvariants();
        //}

        protected void CheckInvariants()
        {
            base.CheckInvariants();
            Check.Invariant(!string.IsNullOrEmpty(this.Value), "Value must not be null or empty.");
        }
    }
}
