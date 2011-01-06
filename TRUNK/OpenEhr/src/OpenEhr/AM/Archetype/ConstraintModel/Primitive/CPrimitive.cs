using System;
using System.Collections.Generic;
using System.Text;
using OpenEhr.DesignByContract;

namespace OpenEhr.AM.Archetype.ConstraintModel.Primitive
{
    [Serializable]
    public abstract class CPrimitive//: IRmType
    {
        /// <summary>
        /// Generate a default value from this constraint object
        /// </summary>
        public abstract object DefaultValue { get; }

        /// <summary>
        /// True if there is an assumed value
        /// </summary>
        /// <returns></returns>
        public abstract bool HasAssumedValue();

        /// <summary>
        /// Value to be assumed if none sent in data
        /// </summary>
        public abstract object AssumedValue { get; set;}

        internal abstract string ValidValue(object aValue);

        /// <summary>
        /// True if the current node constraints is narrower than other
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        internal abstract bool IsSubsetOf(CPrimitive other);

        //internal abstract bool IsSubsetOf(CPrimitive other);

        #region serialization/deserialization
        //protected abstract void ReadXmlBase(System.Xml.XmlReader reader);
        //protected abstract void WriteXmlBase(System.Xml.XmlWriter writer);

        //internal void ReadXml(System.Xml.XmlReader reader)
        //{
        //    Check.Require(!reader.IsEmptyElement, reader.LocalName + " element must not be empty.");

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

        //#region IRmType Members

        //string IRmType.GetRmTypeName()
        //{
        //    return this.GetRmTypeName();
        //}

        //protected abstract string GetRmTypeName();

        //#endregion
    }
}
