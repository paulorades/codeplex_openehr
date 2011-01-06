using System;

using OpenEhr.DesignByContract;
using OpenEhr.Attributes;
using OpenEhr.RM.DataTypes.Basic;
using OpenEhr.Serialisation;
using OpenEhr.Factories;
using OpenEhr.RM.Impl;

namespace OpenEhr.RM.DataTypes.Quantity
{
    /// <summary>
    /// Generic class defining an interval (i.e. range) of a comparable type.
    /// An interval is a contiguous subrange of a comparable base type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    //public class DvInterval<T> : Basic.DataValue, Support.Assumed.Interval<DvOrdered>
    [System.Xml.Serialization.XmlSchemaProvider("GetXmlSchema")]
    [Serializable]
    [RmType("openEHR", "DATA_TYPES", "DV_INTERVAL")]
    public class DvInterval<T> 
        : Basic.DataValue, System.Xml.Serialization.IXmlSerializable, AssumedTypes.IInterval<T>
        where T : DvOrdered<T>
    {
        #region Constructors

        internal DvInterval() //: base(new EhrTypes.DV_INTERVAL()) { 
            : base()
        { }

        //internal DvInterval(EhrTypes.DV_INTERVAL intervalType) : base(intervalType) {
        //    CheckInvariants();
        //}

        public DvInterval(T lower, bool lowerIncluded, bool lowerUnbounded,
            T upper, bool upperIncluded, bool upperUnbounded)
            : this()
        {
            // lower/upper boundary value included in range if not lower_unbounded/upper_unbounded
            DesignByContract.Check.Require(!lowerIncluded || !lowerUnbounded);
            DesignByContract.Check.Require(!lowerIncluded || lower != null);
            DesignByContract.Check.Require(!upperIncluded || !upperUnbounded);
            DesignByContract.Check.Require(!upperIncluded || upper != null);
            DesignByContract.Check.Require(lower == null || !lowerUnbounded);
            DesignByContract.Check.Require(upper == null || !upperUnbounded);
            DesignByContract.Check.Require(lower == null || upper == null || (lower < upper || lower == upper));
            DesignByContract.Check.Require(lower == null || upper == null || lower.IsStrictlyComparableTo(upper));

            SetLower(lower, lowerIncluded);
            this.SetUpper(upper, upperIncluded);

            this.SetLowerUnbounded(lowerUnbounded);

            this.SetUpperUnbounded(upperUnbounded);

            //this.SetInnerData();
            CheckInvariants();
        }

        public DvInterval(T lower, bool lowerIncluded,
            T upper, bool upperIncluded)
            : this()
        {
            if (lower != null)
            {
                SetLower(lower, lowerIncluded);
            }

            if (upper != null)
            {
                SetUpper(upper, upperIncluded);
            }
           
            //this.SetInnerData();
            CheckInvariants();
        }       

        public DvInterval(T value, bool included, bool lowerUnbounded, 
            bool upperUnbounded)
            : this()
        {
            DesignByContract.Check.Require(value!=null);
            DesignByContract.Check.Require(!lowerUnbounded || !upperUnbounded);

            // CM: 11/02/08
            if (!lowerUnbounded)
            {
                this.SetLower(value, included);
            }
            if (!upperUnbounded)
            {
                this.SetUpper(value, included);
            }

            this.lowerUnbounded = lowerUnbounded;
            this.lowerUnboundedSet = true;
            this.upperUnbounded = upperUnbounded;
            this.upperUnboundedSet = true;

            //this.SetInnerData();
            
            CheckInvariants();
        }

        ///// <summary>
        ///// This constructor is used when it is assumed that both the lower_included
        ///// and the upper_included are true. This constructor has not been implemented 
        ///// since it may not be safter. 
        ///// </summary>
        //public DvInterval(EhrTypes.DV_ORDERED lower,
        //    EhrTypes.DV_ORDERED upper)
        //    : this()
        //{
        //    throw new NotImplementedException();
        //    //if (lower != null && upper != null)
        //    //{
        //    //    DvOrdered openEhrV1Lower = WrapperFactory.CreateDataValue(lower);
        //    //    DvOrdered openEhrV1Upper = WrapperFactory.CreateDataValue(upper);

        //    //    DesignByContract.Check.Require(openEhrV1Lower < openEhrV1Upper || openEhrV1Upper == openEhrV1Lower);
        //    //    DesignByContract.Check.Require(openEhrV1Lower.IsStrictlyComparableTo(openEhrV1Upper));
        //    //}

        //    //this.EhrType.lower = lower;
        //    //this.EhrType.upper = upper;
        //    //this.EhrType.lower_included = true;
        //    //this.EhrType.upper_included = true;
        //}
        #endregion

        //private EhrTypes.DV_INTERVAL ehrType;
        //protected new EhrTypes.DV_INTERVAL EhrType
        //{
        //    get
        //    {
        //        if (ehrType == null)
        //            ehrType = base.DataValueType as EhrTypes.DV_INTERVAL;

        //        DesignByContract.Check.Ensure(ehrType != null, "DataValue Type must not be null");
        //        return ehrType;
        //    }
        //}

        #region Interval<DvOrdered> Members

        private T lower;
        //internal bool lowerSet;

        public T Lower
        {
            get
            {
                //if (!lowerSet)
                //{
                //    T result = WrapperFactory.CreateDataValue(this.EhrType.lower) as T;
                //    this.lower = result as T;
                //    this.lowerSet = true;
                //}
                return this.lower;
            }
          
        }

        private void SetLower(T value, bool included)
        {
            this.lower = value as T;
            //this.lowerSet = true;

            if (lower != null)
            {
                this.lowerIncluded = included;
                this.lowerIncludedSet = true;
            }
        }

        private T upper;
        //internal bool upperSet;

        public T Upper
        {
            get
            {
                //if (!this.upperSet)
                //{
                //        T result = WrapperFactory.CreateDataValue(this.EhrType.upper) as T;
                //        this.upper = result as T;
                //        this.upperSet = true;
                //}
                return this.upper;
            }
           
        }

        private void SetUpper(T value, bool included)
        {
            //DesignByContract.Check.Require(value != null);

            this.upper = value as T;
            //this.upperSet = true;

            if (upper != null)
            {
                this.upperIncluded = included;
                this.upperIncludedSet = true;
            }
        }

        private bool lowerUnbounded;
        internal bool lowerUnboundedSet;

        /// <summary>
        /// lower boundary open (i.e. = -infinity)
        /// </summary>
        public bool LowerUnbounded
        {
            get
            {
                //if (!this.lowerUnboundedSet)
                //{
                //    SetLowerUnbounded(this.EhrType.lower_unbounded);
                //}
               
                return this.lowerUnbounded;
            }
        }

        private void SetLowerUnbounded(bool unbounded)
        {
            this.lowerUnbounded = unbounded;
            this.lowerUnboundedSet = true;
        }

        private bool upperUnbounded;
        internal bool upperUnboundedSet;

        public bool UpperUnbounded
        {
            get
            {
                //if (!this.upperUnboundedSet)
                //{
                //    SetUpperUnbounded(this.EhrType.upper_unbounded);
                //}

                return this.upperUnbounded;
            }
        }

        private void SetUpperUnbounded(bool unbounded)
        {
            this.upperUnbounded = unbounded;
            this.upperUnboundedSet = true;
        }

        private bool lowerIncluded;
        internal bool lowerIncludedSet;

        public bool LowerIncluded
        {
            get
            {
                //if (!this.lowerIncludedSet && this.EhrType.lower_includedSpecified)
                //{
                //    if (this.LowerUnbounded)
                //        this.EhrType.lower_included = false;

                //    this.lowerIncluded = this.EhrType.lower_included;

                //    this.lowerIncludedSet = true;
                //}
                return this.lowerIncluded;
            }
        }

        private bool upperIncluded;
        internal bool upperIncludedSet;

        public bool UpperIncluded
        {
            get
            {
                //if (!this.upperIncluded && this.EhrType.upper_includedSpecified)
                //{
                //    if (this.UpperUnbounded)
                //        this.EhrType.upper_included = false;

                //    this.upperIncluded = this.EhrType.upper_included;
                //    this.upperIncludedSet = true;
                //}
                return this.upperIncluded;
            }
            
        }

        /// <summary>
        /// True if e is within the Lower and Upper range
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool Has(T e)
        {
            DesignByContract.Check.Require(e!=null);

            if (this.LowerUnbounded && this.UpperUnbounded)
                return true;

            if (!this.LowerUnbounded)
            {
                if (this.LowerIncluded)
                {
                    if (e < Lower)
                        return false;
                }
                else
                {
                    if (e < Lower || e == Lower)
                        return false;
                }
            }
            if (!this.UpperUnbounded)
            {
                if (this.UpperIncluded)
                {
                    if (e > this.Upper)
                        return false;
                }
                else
                {
                    if (e > this.Upper || e == this.Upper)
                        return false;
                }
            }

            return true;
        }

        protected override void CheckInvariants()
        {
            ////if (this.LowerUnbounded)
            //    DesignByContract.Check.Invariant(!this.LowerIncluded);
            DesignByContract.Check.Invariant(!this.LowerUnbounded || !this.LowerIncluded);

            //if (this.UpperUnbounded)
            //    DesignByContract.Check.Invariant(!this.UpperIncluded);
            DesignByContract.Check.Invariant(!this.UpperUnbounded || !this.UpperIncluded);

            //if (!this.LowerUnbounded && !this.UpperUnbounded)
            //{
            //    DesignByContract.Check.Invariant(this.Upper > this.Lower ||
            //        this.Upper == this.Lower);
            //    DesignByContract.Check.Invariant(this.Lower.IsStrictlyComparableTo(this.Upper));
            //}
            DesignByContract.Check.Invariant(this.LowerUnbounded || this.UpperUnbounded 
                ||  (this.Upper > this.Lower || this.Upper == this.Lower));
            DesignByContract.Check.Invariant(this.LowerUnbounded || this.UpperUnbounded 
                || this.Lower.IsStrictlyComparableTo(this.Upper));
        }
        #endregion

        protected override void ReadXmlBase(System.Xml.XmlReader reader)
        {
            if (reader.LocalName == "lower")
            {
                //string type = reader.GetAttribute("type", XmlSerializer.XsiNamespace);

                //this.lower = DataValue.GetDataValue(type) as T;
                string type = RmXmlSerializer.ReadXsiType(reader);
                this.lower = RmFactory.DataValue(type) as T;
                Check.Assert(this.lower != null, "lower must not be null");

                this.lower.ReadXml(reader);
                //this.lowerSet = true;

                reader.MoveToContent();
            }

            if (reader.LocalName == "upper")
            {
                //string type = reader.GetAttribute("type", XmlSerializer.XsiNamespace);

                //this.upper = DataValue.GetDataValue(type) as T;
                string type = RmXmlSerializer.ReadXsiType(reader);
                this.upper = RmFactory.DataValue(type) as T;
                Check.Assert(this.upper != null, "lower must not be null");

                this.upper.ReadXml(reader);
                //this.upperSet = true;

                reader.MoveToContent();
            }

            if (reader.LocalName == "lower_included")
            {
                this.lowerIncluded = reader.ReadElementContentAsBoolean();
                this.lowerIncludedSet = true;
                reader.MoveToContent();
            }

            if (reader.LocalName == "upper_included")
            {
                this.upperIncluded = reader.ReadElementContentAsBoolean();
                this.upperIncludedSet = true;
                reader.MoveToContent();
            }

            Check.Assert(reader.LocalName == "lower_unbounded", "localName must be 'lower_unbounded'");
            this.lowerUnbounded = reader.ReadElementContentAsBoolean();
            //if (lowerUnbounded)
            //    Check.Assert(this.Lower == null);
            //else
            //    Check.Assert(this.Lower != null);
            this.lowerUnboundedSet = true;
            reader.MoveToContent();

            Check.Assert(reader.LocalName == "upper_unbounded", "localName must be 'upper_unbounded'");
            this.upperUnbounded = reader.ReadElementContentAsBoolean();
            //if (upperUnbounded)
            //    Check.Assert(this.Upper == null);
            //else
            //    Check.Assert(this.Upper != null);
            this.upperUnboundedSet = true;
            reader.MoveToContent();

            //SetInnerData();
            CheckInvariants();
        }

        protected override void WriteXmlBase(System.Xml.XmlWriter writer)
        {
            this.CheckInvariants();

            string openEhrNamespace = RmXmlSerializer.OpenEhrNamespace;
            string openEhrPrefix = RmXmlSerializer.UseOpenEhrPrefix(writer);
            string xsiType = RmXmlSerializer.UseXsiPrefix(writer);

            if (this.Lower != null)
            {
                writer.WriteStartElement("lower", openEhrNamespace);

                string type = ((IRmType)this.Lower).GetRmTypeName();
                if (!string.IsNullOrEmpty(openEhrPrefix))
                    type = openEhrPrefix + ":" + type;

                writer.WriteAttributeString(xsiType, "type", RmXmlSerializer.XsiNamespace, type);
                this.Lower.WriteXml(writer);
                writer.WriteEndElement();
            }

            if (this.Upper != null)
            {
                writer.WriteStartElement("upper", openEhrNamespace);

                string type = ((IRmType)this.Upper).GetRmTypeName();
                if (!string.IsNullOrEmpty(openEhrPrefix))
                    type = openEhrPrefix + ":" + type;
                writer.WriteAttributeString(xsiType, "type", RmXmlSerializer.XsiNamespace, type);

                this.Upper.WriteXml(writer);
                writer.WriteEndElement();
            }

            if (this.Lower != null && this.lowerIncludedSet)
            //if (this.Lower != null && this.EhrType.lower_includedSpecified)
                writer.WriteElementString(openEhrPrefix, "lower_included", openEhrNamespace, 
                    this.LowerIncluded.ToString().ToLower());

            if (this.Upper != null && this.upperIncludedSet)
            //if (this.Upper != null && this.EhrType.upper_includedSpecified) 
                writer.WriteElementString(openEhrPrefix, "upper_included", openEhrNamespace, 
                    this.UpperIncluded.ToString().ToLower());

            writer.WriteElementString(openEhrPrefix, "lower_unbounded", openEhrNamespace, 
                this.LowerUnbounded.ToString().ToLower());

            writer.WriteElementString(openEhrPrefix, "upper_unbounded", openEhrNamespace, 
                this.UpperUnbounded.ToString().ToLower());
        }      

        public static System.Xml.XmlQualifiedName GetXmlSchema(System.Xml.Schema.XmlSchemaSet xs)
        {
            //return GetXmlSchema(xs, "DV_INTERVAL");
            RmXmlSerializer.LoadBaseTypesSchema(xs);
            return new System.Xml.XmlQualifiedName("DV_INTERVAL", RmXmlSerializer.OpenEhrNamespace);

        }

        //protected override void SetInnerData()
        //{
        //    if (this.lower != null)
        //        ((DV_INTERVAL)(this.DataValueType)).lower = (DV_ORDERED)(((DataValue)(this.lower)).DataValueType);
        //    if (this.upper != null)
        //        ((DV_INTERVAL)(this.DataValueType)).upper = (DV_ORDERED)(((DataValue)(this.upper)).DataValueType);
        //    if (this.lowerIncludedSet)
        //    {
        //        ((DV_INTERVAL)(this.DataValueType)).lower_included = this.lowerIncluded;
        //        ((DV_INTERVAL)(this.DataValueType)).lower_includedSpecified = true;
        //    }
        //    if (this.upperIncludedSet)
        //    {
        //        ((DV_INTERVAL)(this.DataValueType)).upper_included = this.upperIncluded;
        //        ((DV_INTERVAL)(this.DataValueType)).upper_includedSpecified = true;
        //    }
        //    ((DV_INTERVAL)(this.DataValueType)).lower_unbounded = this.lowerUnbounded;
        //    ((DV_INTERVAL)(this.DataValueType)).upper_unbounded = this.upperUnbounded;

        //    //CheckInvariants();
        //}



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
