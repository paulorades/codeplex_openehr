using System;

using OpenEhr.DesignByContract;
using OpenEhr.AssumedTypes;
using OpenEhr.Paths;
//using OpenEhr.Serialisation;

namespace OpenEhr.RM.Common.Archetyped.Impl
{
    public abstract class ReflectivePathable : Pathable
    {
        //private System.Collections.Generic.Dictionary<string, object> itemAtPathDictionary;    

        public override bool PathExists(string path)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool PathUnique(string path)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override object ItemAtPath(string path)
        {
            Check.Require(!string.IsNullOrEmpty(path), "Path must not be null or empty.");

            object itemInDictionary = null;
            //if (this.itemAtPathDictionary != null && this.itemAtPathDictionary.ContainsKey(path))
            //    itemInDictionary = itemAtPathDictionary[path];
            //else
            //    itemInDictionary = ItemAtPathUtil(path);
            Path pathObject = new Path(path);


            if (itemInDictionary == null)
                throw new PathNotExistException(path);

            if (itemInDictionary is AssumedTypes.IList)
            {
                AssumedTypes.IList list = itemInDictionary as AssumedTypes.IList;

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
            throw new Exception("The method or operation is not implemented.");
        }

        public override string PathOfItem(Pathable item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        //internal override object FindItem(Path path)
        //{
        //    Check.Require(path != null, "pathProcessor must not be null");

        //    // when the path starts with wildcard key
        //    if (pathProcessor.IsCurrentWildcard)
        //        //return ItemAtPathForWildcardKeyPathUtil(pathProcessor);
        //        throw new NotImplementedException();

        //    string attributeName = pathProcessor.CurrentAttribute;

        //    //if (!this.AttributesDictionary.ContainsKey(attributeName))
        //    //    return null;

        //    object attributeValue = GetAttributeValue(attributeName);
        //    if (attributeValue == null)
        //        return null;

        //    Pathable pathable = attributeValue as Pathable;

        //    if (pathable != null)
        //    {
        //        if (!path.Current.Matches(pathable))
        //            return null;

        //        if (path.IsCurrentTerminal)
        //            return pathable;

        //        if (!path.NextStep())
        //            throw new System.InvalidOperationException("Next node expected");

        //        return pathable.FindItem(path);
        //    }

        //    Support.Assumed.ILocatableList locatableList
        //       = attributeObj as Support.Assumed.ILocatableList;

        //    if (locatableList != null)
        //        //return ProcessPredicate(locatableList, pathProcessor);
        //        throw new NotImplementedException();

        //    // not pathable object or pathable List
        //    // check no node predicate 
        //    if (path.IsCurrentIdentified)
        //        return null;

        //    if (pathProcessor.IsCurrentTerminal)
        //        return attributeValue;

        //    if (!pathProcessor.NextStep())
        //        throw new System.InvalidOperationException("A non-terminal path should have next node.");

        //    //return NonPathablePathProcessor.ItemAtPathUtil(attributeObj, pathProcessor);
        //    throw new NotImplementedException();
        //}
    }
}
