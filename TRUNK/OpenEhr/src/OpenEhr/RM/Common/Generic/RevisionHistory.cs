using System;
//using System.Collections.Generic;
//using System.Text;
using OpenEhr.DesignByContract;
using OpenEhr.Attributes;
using OpenEhr.Validation;
using OpenEhr.RM.Impl;
using OpenEhr.Serialisation;

namespace OpenEhr.RM.Common.Generic
{
    /// <summary>
    /// Defines the notion of a revision history of audit items, each associated with the
    /// version for which that audit was committed. The list is in most-recent-first order.
    /// </summary>
    [System.Xml.Serialization.XmlSchemaProvider("GetXmlSchema")]
    [Serializable]
    [RmType("openEHR", "COMMON", "REVISION_HISTORY")]
    public class RevisionHistory : RmType, System.Xml.Serialization.IXmlSerializable
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public RevisionHistory() { }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="items">The items in this history in most-recent-last order.</param>
        public RevisionHistory(AssumedTypes.List<RevisionHistoryItem> items)
        {
            DesignByContract.Check.Require(items!= null, "items must not be null.");
            // TODO: need to sort the order according to the spec
            //this.items = SortItemsInMostRecentLastOrder(items);
            this.items = items;
        }

        private AssumedTypes.List<RevisionHistoryItem> items;
        /// <summary>
        /// The items in this history in most-recent-last order.
        /// </summary>
        public AssumedTypes.List<RevisionHistoryItem> Items
        {
            get { return this.items; }
        }

        /// <summary>
        /// The version id of the most recent item, as a String.
        /// </summary>
        /// <returns></returns>
        public string MostRecentVersion()
        {
            DesignByContract.Check.Require(this.Items!= null, "RevisionHistory.Items must not be null.");
            DesignByContract.Check.Require(this.Items.Count >0, "RevisionHistory.Items must not be empty.");
           
            return this.Items.Last.VersionId.Value;
        }

        /// <summary>
        /// The commit date/time of the most recent item, as a String.
        /// </summary>
        /// <returns></returns>
        public string MostRecentVersionTimeCommitted()
        {
            DesignByContract.Check.Require(this.Items != null, "RevisionHistory.Items must not be null.");
            DesignByContract.Check.Require(this.Items.Count > 0, "RevisionHistory.Items must not be empty.");

            //if (this.Items.Count == 0)
            //    return null;

            return this.Items.Last.Audits.First.TimeCommitted.Value;
        }

        /////// <summary>
        /////// Sort the order of the items in most-recent-first order
        /////// </summary>
        /////// <param name="items"></param>
        /////// <returns></returns>
        //private Support.Assumed.List<RevisionHistoryItem> SortItemsInMostRecentFirstOrder(Support.Assumed.List<RevisionHistoryItem> items)
        //{
        //    DesignByContract.Check.Require(items != null, "items must not be null.");

        //    if (items.Count == 1 || items.Count == 0)
        //        return items;

        //    System.Collections.Generic.List<RevisionHistoryItem> sortedItems = new List<RevisionHistoryItem>();

        //    for (int i = items.Count - 1; i >= 0; i--)
        //    {
        //        RevisionHistoryItem item = items[i];
        //        // if the sortedItem is empty, just add item to it.
        //        if (sortedItems.Count == 0)
        //            sortedItems.Add(item);
        //        else
        //        {
        //            RevisionHistoryItem mostRecent = sortedItems[0];
        //            // if item is later than the mostRecent, add item to the end of the list
        //            if (mostRecent.VersionId.VersionTreeId < item.VersionId.VersionTreeId)
        //                sortedItems.Insert(0, item);
        //            else
        //            {
        //                // if item is earlier than the first, insert item as the first
        //                RevisionHistoryItem last = sortedItems[sortedItems.Count-1];
        //                if (last.VersionId.VersionTreeId > item.VersionId.VersionTreeId)
        //                    sortedItems.Add(item);
        //                else
        //                {
        //                    // go through the sortedItems in the order of most recent to the earliest,
        //                    // found the first revision item that is earlier than item, then insert the item before
        //                    // the revision item
        //                    for (int t = 1; t <sortedItems.Count; t++)
        //                    {
        //                        RevisionHistoryItem revisionItem = sortedItems[t];
        //                        if (revisionItem.VersionId.VersionTreeId < item.VersionId.VersionTreeId)
        //                            sortedItems.Insert(t -1, item);

        //                    }
        //                }

        //            }
        //        }
        //    }

        //    OpenEhr.Support.Assumed.List<RevisionHistoryItem> results = new OpenEhr.Support.Assumed.List<RevisionHistoryItem>();
        //    foreach (RevisionHistoryItem item in sortedItems)
        //    {
        //        results.Add(item);
        //    }
        //    return results;
        //}

        ///// <summary>
        ///// Sort the order of the items in most-recent-last order
        ///// </summary>
        ///// <param name="items"></param>
        ///// <returns></returns>
        //private Support.Assumed.List<RevisionHistoryItem> SortItemsInMostRecentLastOrder(Support.Assumed.List<RevisionHistoryItem> items)
        //{
        //    DesignByContract.Check.Require(items != null, "items must not be null.");

        //    if (items.Count == 1 || items.Count == 0)
        //        return items;

        //    System.Collections.Generic.List<RevisionHistoryItem> sortedItems = new List<RevisionHistoryItem>();

        //    for (int i = items.Count - 1; i >= 0; i--)
        //    {
        //        RevisionHistoryItem item = items[i];
        //        // if the sortedItem is empty, just add item to it.
        //        if (sortedItems.Count == 0)
        //            sortedItems.Add(item);
        //        else
        //        {
        //            RevisionHistoryItem mostRecent = sortedItems[sortedItems.Count - 1];
        //            // if item is later than the mostRecent, add item to the end of the list
        //            if (mostRecent.VersionId.VersionTreeId < item.VersionId.VersionTreeId)
        //                sortedItems.Add(item);
        //            else
        //            {
        //                // if item is earlier than the first, insert item as the first
        //                RevisionHistoryItem first = sortedItems[0];
        //                if (first.VersionId.VersionTreeId > item.VersionId.VersionTreeId)
        //                    sortedItems.Insert(0, item);
        //                else
        //                {
        //                    // go through the sortedItems in the order of most recent to the earliest,
        //                    // found the first revision item that is earlier than item, then insert the item after
        //                    // the revision item
        //                    for (int t = sortedItems.Count - 2; t >= 0; t--)
        //                    {
        //                        RevisionHistoryItem revisionItem = sortedItems[t];
        //                        if (revisionItem.VersionId.VersionTreeId < item.VersionId.VersionTreeId)
        //                            sortedItems.Insert(t + 1, item);


        //                    }
        //                }

        //            }
        //        }
        //    }

        //    OpenEhr.Support.Assumed.List<RevisionHistoryItem> results = new OpenEhr.Support.Assumed.List<RevisionHistoryItem>();
        //    foreach (RevisionHistoryItem item in sortedItems)
        //    {
        //        results.Add(item);
        //    }
        //    return results;
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

        public static System.Xml.XmlQualifiedName GetXmlSchema(System.Xml.Schema.XmlSchemaSet xs)
        {
            RmXmlSerializer.LoadBaseTypesSchema(xs);
            return new System.Xml.XmlQualifiedName("REVISION_HISTORY", RmXmlSerializer.OpenEhrNamespace);
        }

        internal void ReadXml(System.Xml.XmlReader reader)
        {
            reader.ReadStartElement();
            reader.MoveToContent();

            if (reader.LocalName != "items")
                throw new ValidationException("Expected local name is items, but it is " + reader.LocalName);
           
            this.items = new OpenEhr.AssumedTypes.List<RevisionHistoryItem>();
           
            while (reader.LocalName == "items")
            {
                RevisionHistoryItem item = new RevisionHistoryItem();
                item.ReadXml(reader);
                this.items.Add(item);
            }

            // TODO: sort the items
        }

        internal void WriteXml(System.Xml.XmlWriter writer)
        {
            Check.Require(this.Items != null, "RevisionHistory.Items must not be null.");
         
            string xsiPrefix = RmXmlSerializer.UseXsiPrefix(writer);
            string openEhrPrefix = RmXmlSerializer.UseOpenEhrPrefix(writer);

            foreach (RevisionHistoryItem item in this.Items)
            {
                writer.WriteStartElement(openEhrPrefix, "items", RmXmlSerializer.OpenEhrNamespace);
                item.WriteXml(writer);
                writer.WriteEndElement();
            }           
        }
    }
}
