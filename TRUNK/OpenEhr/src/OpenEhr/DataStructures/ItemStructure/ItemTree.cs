using System;
using System.Xml;
//using System.Collections.Generic;

using OpenEhr.RM.DataStructures.ItemStructure.Representation;
using OpenEhr.RM.DataTypes.Text;
using OpenEhr.RM.Common.Archetyped.Impl;
using OpenEhr.Attributes;
using OpenEhr.Serialisation;
using OpenEhr.RM.Impl;
using OpenEhr.Factories;
using OpenEhr.AssumedTypes.Impl;


namespace OpenEhr.RM.DataStructures.ItemStructure
{
    [System.Xml.Serialization.XmlSchemaProvider("GetXmlSchema")]
    [Serializable]
    [RmType("openEHR", "DATA_STRUCTURES", "ITEM_TREE")]
    public class ItemTree : ItemStructure, System.Xml.Serialization.IXmlSerializable
    {
        public ItemTree()
            : base()
        { }

        public ItemTree(DvText name, string archetypeNodeId, Support.Identification.UidBasedId uid,
            Link[] links, Archetyped archetypeDetails, FeederAudit feederAudit, Item[] items)
            : base(name, archetypeNodeId, uid, links, archetypeDetails, feederAudit)
        {
            if (items != null)
            {
                this.items = RmFactory.LocatableList<Item>(this, items);
            }

            SetAttributeDictionary();
            CheckInvariants();
        }
        
        private AssumedTypes.List<Representation.Item> items;

        [RmAttribute("items")]
        public AssumedTypes.List<Representation.Item> Items
        {
            get
            {
                if(this.items == null)
                    this.items = base.attributesDictionary["items"] as LocatableList<Item>;
                return this.items;
            }
        }

        #region class functions

        // CM: 16/04/2008
        public override Item AsHierarchy()
        {
            if (this.Items == null || this.Items.Count == 0)
                return null;
            throw new NotImplementedException();
            //OpenEhr.V1.Its.Xml.RM.CLUSTER clusterType = ToItsXmlCluster();       

            //System.Collections.Generic.List<OpenEhr.V1.Its.Xml.RM.ITEM> itemsType = 
            //    new List<OpenEhr.V1.Its.Xml.RM.ITEM>();               

            //foreach (Item item in this.Items)
            //{
            //    itemsType.Add(Convert.ToItsXmlType(item) as OpenEhr.V1.Its.Xml.RM.ITEM);
            //}
            //clusterType.items = itemsType.ToArray();

            //Cluster newCluster = new Cluster(clusterType);

            //DesignByContract.Check.Ensure(newCluster.Name.Value == this.Name.Value,
            //   "returned cluster must have same name value as the ItemTree.");
            //DesignByContract.Check.Ensure(newCluster.ArchetypeNodeId == this.ArchetypeNodeId,
            //   "returned cluster must have same archetypeNodeId as the ItemTree.");

            //return newCluster;
        }

        /// <summary>
        /// True if path is a valid leaf path 'aPath'
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool HasElementPath(string aPath)
        {
            DesignByContract.Check.Require(!string.IsNullOrEmpty(aPath), "path must not be null or empty.");

            if (!this.PathExists(aPath) || !this.PathUnique(aPath))
                return false;

            Element ele = this.ItemAtPath(aPath) as Element;
            if (ele != null)
                return true;

            return false;
        }

        /// <summary>
        /// Return the leaf element at the path 'aPath'
        /// </summary>
        /// <param name="aPath"></param>
        /// <returns></returns>
        public Element ElementAtPath(string aPath)
        {
            DesignByContract.Check.Require(this.HasElementPath(aPath), "aPath must be a leaf path.");

            return (Element)(this.ItemAtPath(aPath));
        }

        #endregion

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

        public static XmlQualifiedName GetXmlSchema(System.Xml.Schema.XmlSchemaSet xs)
        {
            RmXmlSerializer.LoadCompositionSchema(xs);
            return new XmlQualifiedName("ITEM_TREE", RmXmlSerializer.OpenEhrNamespace);
        }

        protected override void ReadXmlBase(XmlReader reader)
        {
            base.ReadXmlBase(reader);

            if (reader.LocalName == "items")
            {
                OpenEhr.AssumedTypes.Impl.LocatableList<Item> items =
                    new OpenEhr.AssumedTypes.Impl.LocatableList<Item>();
                while (reader.LocalName == "items" && reader.NodeType == System.Xml.XmlNodeType.Element)
                {
                    string itemType = reader.GetAttribute("type", RmXmlSerializer.XsiNamespace);

                    Item anItem = Item.GetLocatableObjectByType(itemType) as Item;
                    if (anItem == null)
                        throw new InvalidOperationException("itemType must be type of Item: " + itemType);
                    anItem.ReadXml(reader);
                    anItem.Parent = this;
                    items.Add(anItem);
                }

                this.items = items;

            }
        }

        protected override void WriteXmlBase(XmlWriter writer)
        {
            base.WriteXmlBase(writer);

            string xsiPrefix = RmXmlSerializer.UseXsiPrefix(writer);
            string openEhrPrefix = RmXmlSerializer.UseOpenEhrPrefix(writer);

            if (this.Items != null && this.Items.Count > 0)
            {
                foreach (Item anItem in this.items)
                {
                    writer.WriteStartElement(openEhrPrefix, "items", RmXmlSerializer.OpenEhrNamespace);
                    string itemType = ((IRmType)anItem).GetRmTypeName();
                    if (!string.IsNullOrEmpty(openEhrPrefix))
                        itemType = openEhrPrefix + ":" + itemType;
                    writer.WriteAttributeString(xsiPrefix, "type", RmXmlSerializer.XsiNamespace, itemType);
                    anItem.WriteXml(writer);
                    writer.WriteEndElement();
                }
            }
        }

        //protected override void SetInnerData()
        //{
        //    if (this.innerType == null && this.LocatableType == null)
        //        SetInnerType(new OpenEhr.V1.Its.Xml.RM.ITEM_TREE());

        //    base.SetInnerData();

        //    if (this.items != null && this.items.Count > 0)
        //    {
        //        System.Collections.Generic.List<EhrTypes.ITEM> itemList = new List<OpenEhr.V1.Its.Xml.RM.ITEM>();
        //        foreach (Item anItem in this.items)
        //        {
        //            itemList.Add(anItem.LocatableType as EhrTypes.ITEM);
        //        }

        //        this.InnerType.items = itemList.ToArray();
        //    }
        //}

        protected override void SetAttributeDictionary()
        {
            base.SetAttributeDictionary();
            base.attributesDictionary["items"]= this.items;
        }

        //protected override void InitialiseAttributeDictionary()
        //{
        //    base.InitialiseAttributeDictionary();

        //    // add attributes to AttributesDictionary
        //    Support.Assumed.LocatableList<Representation.Item> items = null;

        //    if (this.InnerType.items != null)
        //    {
        //        items = new OpenEhr.Support.Assumed.LocatableList<Item>();
        //        if (this.InnerType.items.Length > 0)
        //        {
        //            foreach (EhrTypes.ITEM eachItem in this.InnerType.items)
        //            {
        //                Representation.Item openEhrItem = WrapperFactory.CreateLocatable(eachItem) as Representation.Item;
        //                openEhrItem.Parent = this;

        //                items.Add(openEhrItem);
        //            }
        //        }

        //    }

        //    base.attributesDictionary.Add("items", items);
        //}
    }
}
