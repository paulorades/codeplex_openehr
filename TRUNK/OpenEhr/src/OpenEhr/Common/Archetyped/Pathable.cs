using System;
using OpenEhr.RM.DataStructures.ItemStructure;
using OpenEhr.DesignByContract;
using OpenEhr.AM.Archetype.ConstraintModel;
using OpenEhr.AssumedTypes;
using OpenEhr.RM.Impl;
using OpenEhr.Paths;
using OpenEhr.AssumedTypes.Impl;

namespace OpenEhr.RM.Common.Archetyped
{
    /// <summary>
    /// Abstract parent of all classes whose instances are reachable by paths, 
    /// and which know how to locate child object by paths. 
    /// The parent feature may be implemented as a function or attribute.
    /// </summary>
    [Serializable]
    public abstract class Pathable : RmType
    {
        /// <summary>
        /// Parent of this node in compositional hierarchy.
        /// </summary>
        private Pathable parent;

        public virtual Pathable Parent
        {
            get { return this.parent; }
            internal set { this.parent = value; }
        }

        public abstract bool PathExists(string path);
        public abstract bool PathUnique(string path);
        public abstract object ItemAtPath(string path);
        public abstract List<object> ItemsAtPath(string path);
        public abstract string PathOfItem(Pathable item);

        //internal abstract object FindItem(Path path);

        protected new CComplexObject Constraint
        {
            get
            {
                CComplexObject constraint = base.Constraint as CComplexObject;

                Check.Ensure(!HasConstraint || constraint != null, "Constraint must be of type CComplexObject");
                return constraint;
            }
            set { base.Constraint = value; }
        }

        protected override void SetAttributeValue(string attributeName, object value)
        {
            // Set Constraint
            if (value != null &&  HasConstraint)
            {
                CAttribute attributeConstraint = Constraint.GetAttribute(attributeName);
                if (attributeConstraint != null)
                {
                    CMultipleAttribute multipleAttributeConstraint = attributeConstraint as CMultipleAttribute;
                    if (multipleAttributeConstraint != null)
                    {
                        IAggregate aggregate = value as IAggregate;
                        Check.Assert(aggregate != null, "value must implement IAggregate when attribute constraint is CMultipleAttribute");
                        aggregate.Constraint = multipleAttributeConstraint;
                    }
                    else
                    {
                        bool isValid = false;
                        foreach(CObject objectConstraint in attributeConstraint.Children)
                        {
                            if (objectConstraint.ValidValue(value))
                            {
                                isValid = true;
                                IRmType rmType = value as IRmType;
                                if (rmType != null)
                                    rmType.Constraint = objectConstraint;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}

namespace OpenEhr.RM.Common.Archetyped.Impl
{
    [Serializable]
    public abstract class AttributeDictionaryPathable : Pathable
    {
        private System.Collections.Generic.Dictionary<string, object> itemAtPathDictionary;

        protected AttributeDictionaryPathable()
        {
            this.attributesDictionary = new System.Collections.Generic.Dictionary<string, object>();
        }

        protected readonly System.Collections.Generic.Dictionary<string, object> attributesDictionary;

        ///// <summary>
        ///// An internal property for class attributes collection
        ///// </summary>
        //internal protected abstract System.Collections.Generic.Dictionary<string, object> AttributesDictionary
        //{
        //    get;
        //    //set;
        //}

        public override bool PathExists(string path)
        {
            DesignByContract.Check.Require(!string.IsNullOrEmpty(path), "Path must not be null or empty.");
            //DesignByContract.Check.Require(Path.IsValidPath(path), "Path must be a valid path against the path pattern: " + path);

            object itemInDictionary = null;
            if (this.itemAtPathDictionary != null &&
                this.itemAtPathDictionary.ContainsKey(path))
                itemInDictionary = itemAtPathDictionary[path];
            else
                itemInDictionary = ItemAtPathUtil(path);
            if (itemInDictionary == null)
                return false;
            return true;
        }

        public override bool PathUnique(string path)
        {
            DesignByContract.Check.Require(!string.IsNullOrEmpty(path), "Path must not be null or empty.");
            //DesignByContract.Check.Require(Path.IsValidPath(path), "Path must be a valid path against the path pattern: " + path);

            object itemInDictionary = null;
            if (this.itemAtPathDictionary != null && this.itemAtPathDictionary.ContainsKey(path))
                itemInDictionary = itemAtPathDictionary[path];
            else
                itemInDictionary = ItemAtPathUtil(path);

            if (itemInDictionary == null)
                throw new PathNotExistException(path);

            AssumedTypes.IList list = itemInDictionary as AssumedTypes.IList;
            if (list != null)
            {
                if (list.Count > 1)
                    return false;
                else
                    return true;
            }
            else
                return true;
        }

        public override object ItemAtPath(string path)
        {
            DesignByContract.Check.Require(!string.IsNullOrEmpty(path), "Path must not be null or empty.");
            //DesignByContract.Check.Require(Path.IsValidPath(path), "Path must be a valid path against the path pattern: " + path);

            object itemInDictionary = null;
            if (this.itemAtPathDictionary != null && this.itemAtPathDictionary.ContainsKey(path))
                itemInDictionary = itemAtPathDictionary[path];
            else
                itemInDictionary = ItemAtPathUtil(path);

            if (itemInDictionary == null)
                throw new PathNotExistException(path);

            AssumedTypes.IList list = itemInDictionary as AssumedTypes.IList;
            if (list != null)
            {
                if (list.Count > 1)
                    throw new PathNotUniqueException(path);
                else
                    return list[0];
            }
            else
                return itemInDictionary;
        }

        public override List<object> ItemsAtPath(string path)
        {
            DesignByContract.Check.Require(!string.IsNullOrEmpty(path), "Path must not be null or empty.");
            //DesignByContract.Check.Require(Path.IsValidPath(path), "Path must be a valid path against the path pattern: " + path);

            object itemInDictionary = null;
            if (this.itemAtPathDictionary != null && this.itemAtPathDictionary.ContainsKey(path))
                itemInDictionary = itemAtPathDictionary[path];
            else
                itemInDictionary = ItemAtPathUtil(path);

            if (itemInDictionary == null)
                throw new PathNotExistException(path);
            //else if (itemInDictionary is Support.Assumed.IList)
            //{
            //    //return itemInDictionary as Support.Assumed.IList;
            //    return itemInDictionary as List<object>;
            //}

            //// CM: 07/07/09 the path must be unique path. Need to create a list and add the object to the list.
            //// Even though the spec shows the the precondition should be that the path must not be unique path, but this
            //// may be too restrictive. So for the moment, when the path is unique, then need to create a list instance
            //// and add the single object to the list rather than throw an exception.
            ////throw new Exceptions.PathUniqueException("An IList instance is expected: " + path);

            //Locatable locatableObj = itemInDictionary as Locatable;
            //if (locatableObj != null)
            //{
            //    Support.Assumed.LocatableList<Locatable> locatableList = new Support.Assumed.LocatableList<Locatable>();
            //    locatableList.Add(locatableObj);
            //    return locatableList;
            //}

            //Support.Assumed.List<object> items = new OpenEhr.Support.Assumed.List<object>();
            List<object> items = itemInDictionary as List<object>;
            if (items == null)
            {
                Check.Assert(!(itemInDictionary is IList), "itemInDiction must not be of type IList");

                items = new OpenEhr.AssumedTypes.List<object>();
                items.Add(itemInDictionary);
            }
            Check.Ensure(items != null, "items must not be null");

            return items;
        }

        protected void ClearItemAtPathCache()
        {
            itemAtPathDictionary = null;
        }

        private object ItemAtPathUtil(string path)
        {
            DesignByContract.Check.Require(!string.IsNullOrEmpty(path), "Path must not be null or empty.");

            object itemAtPath;

            if (path == "/")
            {
                Pathable pathableObject = this;

                while (pathableObject.Parent != null)
                    pathableObject = pathableObject.Parent;

                itemAtPath = pathableObject;
            }
            else
            {
                Path pathProcessor = new Path(path);

                //// The itemAtPath can be a single item which is a pathable, a locatable 
                //// or a non-pathable item.
                //// The itemAtPath can be an IList instance when the path points to 
                //// a locatable collection with multiple items or a single item; 
                //// or it can be an IList with one or more
                //// locatables when the path looks like //*[archetype ID] (i.e. the path is not
                //// named and is terminal)
                //// The itemAtPath can be null when the path doesn't exist.
                //object itemAtPath = this.ItemAtPathPathProcessorUtil(pathProcessor);
                itemAtPath = pathProcessor.ItemAtPath(this);

                if (this.itemAtPathDictionary == null)
                    itemAtPathDictionary = new System.Collections.Generic.Dictionary<string, object>();
                // this function is called only when the itemAtPathDictionary doesn't have the key - path,
                // therefore, don't need to check whether the dictionary has the key or not before
                // adding the key-value pair.      
                Check.Assert(!this.itemAtPathDictionary.ContainsKey(path), "itemAtPathDiction must not contain path");
                if (itemAtPath != null)
                    this.itemAtPathDictionary.Add(path, itemAtPath);
            }
            return itemAtPath;
        }

        /// <summary>
        /// In this method, it is assumed that the locatable must be the child of this class.
        /// </summary>
        /// <param name="locatable"></param>
        /// <returns></returns>
        private string PathOfItemSimple(Pathable item)
        {
            Check.Require(item != null);

            string path = null;
            string pathSeperator = "/";

            System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<string, object>> keyValueList
                = this.attributesDictionary as System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<string, object>>;
            foreach (System.Collections.Generic.KeyValuePair<string, object> listItem in keyValueList)
            {
                ILocatableList locatableList = listItem.Value as ILocatableList;
                Pathable locatableItem = listItem.Value as Pathable;

                if (locatableList != null)
                {
                    Locatable itemLocatable = item as Locatable;
                    if (locatableList.Contains(itemLocatable))
                    {
                        DesignByContract.Check.Assert(itemLocatable.Name != null);

                        return path = pathSeperator + listItem.Key + "[" + itemLocatable.ArchetypeNodeId +
                            " and name/value='" + itemLocatable.Name.Value + "']";
                    }
                }
                else if (listItem.Value == item)
                {
                    // must be locatable
                    if (item is Locatable)
                        return path = pathSeperator + listItem.Key + "[" + ((Locatable)item).ArchetypeNodeId + "]";
                    else
                        return path = pathSeperator + listItem.Key;
                }
            }

            // raise exception?
            //return null;
            throw new InvalidOperationException("item not found in parent's items");
        }

        /// <summary>
        /// The path to an item relative to the root of this archetyped structure.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override string PathOfItem(Pathable item)
        {
            //throw new Exception("The method or operation is not implemented.");
            DesignByContract.Check.Require(item != null);

            //Locatable item = locatable as Locatable;
            DesignByContract.Check.Assert(item != null);

            AttributeDictionaryPathable itemParent = item.Parent as AttributeDictionaryPathable;
            Check.Assert(item.Parent == null || itemParent != null, "itemParent must not be null");

            // when the item is the top level (e.g. composition) or doesn't have a parent, 
            // the path is "/"
            //if (this == item || itemParent == null)
            if (itemParent == null)
                return "/";

            string path = "";

            while (!this.Equals(itemParent))
            {
                path = itemParent.PathOfItemSimple(item) + path;
                item = itemParent;
                if (item.Parent == null)
                    throw new ApplicationException("root of item not found, item is not a decendent of this object");

                itemParent = item.Parent as AttributeDictionaryPathable;
                Check.Assert(item.Parent == null || itemParent != null, "itemParent must not be null");
            }

            path = itemParent.PathOfItemSimple(item) + path;

            return path;
        }

        protected abstract void SetAttributeDictionary();
        //protected abstract void InitialiseAttributeDictionary();

        protected override void SetAttributeValue(string attributeName, object value)
        {
            Check.Require(attributesDictionary.ContainsKey(attributeName), "RM type does not have attribute " + attributeName);

            base.SetAttributeValue(attributeName, value);
            this.attributesDictionary[attributeName] = value;
        }

        protected internal override object GetAttributeValue(string attributeName)
        {
            Check.Require(attributesDictionary.ContainsKey(attributeName), "RM type does not have attribute " + attributeName);

            return attributesDictionary[attributeName];
        }

        protected internal override System.Collections.IEnumerable GetAllAttributeValues()
        {
            return attributesDictionary.Values;
        }
    }
}
