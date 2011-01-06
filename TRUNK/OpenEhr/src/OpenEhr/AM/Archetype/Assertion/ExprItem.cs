using System;
using System.Collections.Generic;
using System.Text;
using OpenEhr.DesignByContract;
using OpenEhr.Resources;

namespace OpenEhr.AM.Archetype.Assertion
{
    /// <summary>
    /// Abstract parent of all expression tree items.
    /// </summary>
    [Serializable]
    public abstract class ExprItem
    {
        #region Constructors        
        protected ExprItem(string type)
        {
            this.Type = type;
        }

        protected ExprItem() { }
        #endregion

        #region Class properties
        private string type;
        /// <summary>
        /// Type name of this item in the mathematical sense. For leaf nodes, must be the name of a primitive type, 
        /// or else a reference model type. The type for any relational or boolean operator will be “Boolean”, 
        /// while the type for any arithmetic operator, will be “Real” or “Integer”.
        /// </summary>
        public string Type
        {
            get { return type; }
            set
            {
                Check.Require(!string.IsNullOrEmpty(value), string.Format(CommonStrings.XMustNotBeNullOrEmpty, "Type value"));
                type = value;
            }
        }
        #endregion

        #region serialization/deserialization
        //protected virtual void ReadXmlBase(System.Xml.XmlReader reader)
        //{
        //    DesignByContract.Check.Require(reader.LocalName == "type", 
        //        "reader.LocalName must be 'type', but it's :" + reader.LocalName); //TODO internationalise

        //    this.Type = reader.ReadElementContentAsString("type", XmlSerializer.OpenEhrNamespace);
        //}

        //protected virtual void WriteXmlBase(System.Xml.XmlWriter writer)
        //{
        //    DesignByContract.Check.Require(!string.IsNullOrEmpty(this.Type), string.Format(CommonStrings.XMustNotBeNullOrEmpty, "Type"));

        //    string openEhrNamespace = XmlSerializer.OpenEhrNamespace;
        //    string prefix = XmlSerializer.UseOpenEhrPrefix(writer);

        //    writer.WriteElementString(prefix, "type", openEhrNamespace, this.Type);
        //}

        //internal void ReadXml(System.Xml.XmlReader reader)
        //{
        //    Check.Require(!reader.IsEmptyElement, reader.LocalName + " element must not be empty."); //TODO internationalise

        //    reader.ReadStartElement();
        //    reader.MoveToContent();

        //    this.ReadXmlBase(reader);

        //    DesignByContract.Check.Assert(reader.NodeType == System.Xml.XmlNodeType.EndElement,
        //        "Expected endElement after calling ReadXml");
        //    reader.ReadEndElement();
        //    reader.MoveToContent();

        //}

        //internal void WriteXml(System.Xml.XmlWriter writer)
        //{
        //    this.WriteXmlBase(writer);
        //}
        #endregion

        #region class functions
        /// <summary>
        /// An abstract function evaluating the obj against the ExprItem. 
        /// </summary>
        /// <param name="obj">The object to be evaluated</param>
        /// <returns>Can be an object, a list of object or boolean</returns>
        internal object Evaluate(object obj)
        {
            OpenEhr.Paths.AssertionContext contextObj = new OpenEhr.Paths.AssertionContext(obj, null);

            OpenEhr.Paths.AssertionContext returnedObject = Evaluate(contextObj);
            if (returnedObject == null)
                return null;

            object result = returnedObject.Data;
            AssumedTypes.IList iList = result as AssumedTypes.IList;
            if (iList != null && iList.Count == 1)
                return iList[0];

            return result;

        }
        internal abstract OpenEhr.Paths.AssertionContext Evaluate(OpenEhr.Paths.AssertionContext contextObj);

        internal static string GetTypeValue(string stringValue)
        {            
            if (AssumedTypes.Iso8601DateTime.ValidIso8601DateTime(stringValue))
                return "DV_DATE_TIME";
            if (AssumedTypes.Iso8601Time.ValidIso8601Time(stringValue))
                return "DV_TIME";
            if (AssumedTypes.Iso8601Duration.ValidIso8601Duration(stringValue))
                return "DV_DURATION";

            return "String";
        }
        #endregion

        //#region IRmType Members

        //string IRmType.GetRmTypeName()
        //{
        //    return this.GetRmTypeName();
        //}

        //protected abstract string GetRmTypeName();

        //#endregion
    }
}