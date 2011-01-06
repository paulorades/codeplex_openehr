//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using OpenEhr.RM.Common.Archetyped.Impl;

//namespace OpenEhr.AssumedTypes.Impl
//{
//    [Serializable]
//    class LocatableBindingListView<T>
//        : BindingListView<T>
//        where T : Locatable
//    {
//        //private int Find(DataTypes.Text.DvText name)
//        //{
//        //    DesignByContract.Check.Require(name != null, "name must not be null");

//        //    PropertyDescriptorCollection properties = System.Windows.Forms.ListBindingHelper.GetListItemProperties(this);
//        //    PropertyDescriptor property = properties.Find("Name", true);

//        //    DesignByContract.Check.Assert(property != null);
//        //    int index = base.FindCore(property, name);

//        //    return index;
//        //}

//        private int Find(string name)
//        {
//            DesignByContract.Check.Require(!string.IsNullOrEmpty(name), "name must not be null or empty");

//            //PropertyDescriptorCollection properties = System.Windows.Forms.ListBindingHelper.GetListItemProperties(this);
//            //PropertyDescriptor property = properties.Find("NameString", true);

//            //int index = base.FindCore(property, name);

//            for (int i = 0; i < this.Count; i++)
//            {
//                Common.Archetyped.Locatable item = this[i] as Common.Archetyped.Locatable;

//                ////// CM: 27/02/07
//                ////if (key.GetType() == typeof(OpenEhrV1.DataTypes.Text.DvText))
//                ////{
//                ////    OpenEhrV1.DataTypes.Text.DvText keyDvText = key as OpenEhrV1.DataTypes.Text.DvText;
//                ////    OpenEhrV1.DataTypes.Text.DvText itemDvText = property.GetValue(item) as OpenEhrV1.DataTypes.Text.DvText;

//                ////    if (keyDvText.Value == itemDvText.Value)
//                ////        return i;
//                ////}
//                //if (property.GetValue(item).Equals(key))
//                if (item.Name.Value == name)
//                    return i;
//            }
//            return -1; // not found

//            //return index;
//        }

//        public bool Contains(string name)
//        {
//            int index = this.Find(name);

//            if (index < 0)
//                return false;
//            else
//                return true;
//        }

//        public T this[string name]
//        {
//            get
//            {
//                //int index = this.Find(new DataTypes.Text.DvText(name));
//                int index = this.Find(name);

//                if (index < 0)
//                    return null;

//                return this[index];
//            }
//        }

//        // CM: 05/03/08
//        private int Find(string nameTerminologyId, string nameCodeString)
//        {
//            DesignByContract.Check.Require(!string.IsNullOrEmpty(nameCodeString), "nameCodeString must not be null or empty");
//            //DesignByContract.Check.Require(!string.IsNullOrEmpty(nameTerminologyId), "nameTerminologyId must not be null or empty");

//            for (int i = 0; i < this.Count; i++)
//            {
//                Common.Archetyped.Locatable item = this[i] as Common.Archetyped.Locatable;

//                OpenEhr.RM.DataTypes.Text.DvCodedText codeName = item.Name
//                    as OpenEhr.RM.DataTypes.Text.DvCodedText;
//                DesignByContract.Check.Assert(codeName != null, "Item name must be coded name.");

//                if (nameTerminologyId != null)
//                {
//                    if (codeName.DefiningCode.CodeString == nameCodeString &&
//                        codeName.DefiningCode.TerminologyId.Value == nameTerminologyId)
//                        return i;
//                }
//                else // assume that the nameTerminologyId is local.
//                {
//                    if (codeName.DefiningCode.CodeString == nameCodeString)
//                        return i;
//                }
//            }
//            return -1; // not found

//            //return index;
//        }

//        public T this[string nameTerminologyId, string nameCodeString]
//        {
//            get
//            {
//                //int index = this.Find(new DataTypes.Text.DvText(name));
//                int index = this.Find(nameTerminologyId, nameCodeString);

//                if (index < 0)
//                    return null;

//                return this[index];
//            }
//        }

//        public bool Contains(string nameTerminologyId, string nameCodeString)
//        {
//            int index = this.Find(nameTerminologyId, nameCodeString);

//            if (index < 0)
//                return false;
//            else
//                return true;
//        }

//    }

//}
