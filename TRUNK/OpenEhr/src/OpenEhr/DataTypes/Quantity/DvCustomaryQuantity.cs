//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace OpenEhr.RM.DataTypes.Quantity
//{
//    public abstract class DvCustomaryQuantity : DvMeasurable
//    {
//        protected DvCustomaryQuantity(EhrTypes.DV_QUANTIFIED dataValueType) : base(dataValueType) { }
        
//        public EhrTypes.DV_QUANTIFIED EhrType
//        {
//            get
//            {
//                return base.DataValueType as EhrTypes.DV_QUANTIFIED;
//            }
            
//        }
//        public override string Units
//        {
//            get
//            {
//                return this.GetUnits();
//            }
           
//        }

//        protected abstract string GetUnits();


//        public abstract DvQuantity ToQuantity();
//        //{
//        //    DvQuantity1 quantity = new DvQuantity1(this.GetMagnitude());
//        //    quantity.Units = this.Units;
//        //    quantity.NormalRange = this.NormalRange;
//        //    quantity.MagnitudeStatus = this.MagnitudeStatus;
//        //    quantity.OtherReferenceRanges = this.OtherReferenceRanges;

//        //    DesignByContract.Check.Ensure(quantity!=null);

//        //    return quantity;
//        //}
//        //protected override double GetMagnitude()
//        //{
//        //    return base.Magnitude;
//        //}

//        public override int CompareTo(object obj)
//        {
//            DvCustomaryQuantity dvObj = obj as DvCustomaryQuantity;
//            DesignByContract.Check.Require(dvObj!=null);

//            if (this.GetMagnitude() > dvObj.Magnitude)
//                return 1;
//            else if (this.GetMagnitude() < dvObj.Magnitude)
//                return -1;
//            else
//                return 0;
//        }

//        public override bool IsStrictlyComparableTo(DvOrdered other)
//        {
//            DvCustomaryQuantity dvObj = other as DvCustomaryQuantity;
//            DesignByContract.Check.Require(dvObj != null);

//            return string.Compare(this.Units, dvObj.Units, true)==0;
//        }

//        //public override DvInterval<DvOrdered> NormalRange
//        //{
//        //    get
//        //    {
//        //        throw new Exception("The method or operation is not implemented.");
//        //    }
           
//        //}
              
//    }
//}
