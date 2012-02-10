using System;
using System.Collections;
using System.Data;
using System.Xml.Schema;
using System.Xml.Serialization;

using OpenEhr.RM.Composition;
using OpenEhr.RM.Common.ChangeControl;

namespace OpenEhr.EhrGate.WsClient.ResultSet
{
    [XmlSchemaProvider("GetResultsTableSchema")]
    public class ResultsTable : DataTable, ICollection, System.ComponentModel.IListSource, IXmlSerializable
    {
        public ResultsTable()
        { }

        public ResultsTable(string name)
            : base(name)
        { }

         int totalResults;

        public int TotalResults
        {
            get { return this.totalResults; }
        }


 
        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            //return this.DefaultView.GetEnumerator();
            return this.Rows.GetEnumerator();
        }

        #endregion

        #region ICollection Members

        public int Count
        {
            get { return this.Rows.Count; }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        bool ICollection.IsSynchronized
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        object ICollection.SyncRoot
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IListSource Members

        public System.Collections.IList GetList()
        {
            return this.DefaultView;
        }

        Boolean System.ComponentModel.IListSource.ContainsListCollection
        {
            get { return false; }
        }

        #endregion

        #region IXmlSerializable Members

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
        {
            reader.ReadStartElement();
            reader.MoveToContent();
            if (reader.LocalName == "name")
            {
                base.TableName = reader.ReadElementContentAsString();
                reader.MoveToContent();
            }

            //Check.Assert(reader.LocalName == "totalResults");
            if (reader.LocalName == "totalResults")
            {
                totalResults = reader.ReadElementContentAsInt("totalResults", reader.NamespaceURI);
                reader.MoveToContent();
            }

            if (reader.LocalName == "columns")
            {
                //System.Collections.Generic.List<IResultColumn> columns = new System.Collections.Generic.List<IResultColumn>();
                while (reader.LocalName == "columns")
                {
                    reader.ReadStartElement();
                    reader.MoveToContent();

                    string name = reader.ReadElementContentAsString("name", reader.NamespaceURI);
                    reader.MoveToContent();
                    string path = null;
                    if (reader.LocalName == "path")
                    {
                        path = reader.ReadElementContentAsString("path", reader.NamespaceURI);
                        reader.MoveToContent();

                    }
                    reader.ReadEndElement();
                    reader.MoveToContent();

                    ResultsColumn column = new ResultsColumn(name, path);
                    base.Columns.Add(column);
                    //columns.Add(column);
                }
                //this.columns = columns.ToArray();
            }

            if (reader.LocalName == "rows")
            {
                if (!reader.IsEmptyElement)
                {
                    reader.ReadStartElement();
                    reader.MoveToContent();

                    if (reader.LocalName == "row")
                    {
                        while (reader.LocalName == "row")
                        {
                            reader.ReadStartElement();
                            reader.MoveToContent();

                            DataRow row = NewRow();
                            ReadItemsXml(reader, row);
                            base.Rows.Add(row);

                            reader.ReadEndElement();
                            reader.MoveToContent();
                        }
                    }
                    reader.ReadEndElement();
                }
                else
                    reader.ReadStartElement();

                reader.MoveToContent();
            }
            reader.ReadEndElement();
            reader.MoveToContent();
        }

        void ReadItemsXml(System.Xml.XmlReader reader, DataRow row)
        {
            System.Collections.Generic.List<object> itemsList = new System.Collections.Generic.List<object>();
            if (reader.LocalName == "items")
            {
                while (reader.LocalName == "items")
                {
                    string nil = reader.GetAttribute("nil", OpenEhr.Serialisation.RmXmlSerializer.XsiNamespace);
                    if (!string.IsNullOrEmpty(nil) && nil == "true")
                    {
                        itemsList.Add(DBNull.Value);
                        reader.ReadStartElement();
                        reader.MoveToContent();
                    }
                    else
                    {
                        string typeName = reader.GetAttribute("type", OpenEhr.Serialisation.RmXmlSerializer.XsiNamespace);
                        typeName = typeName.Split(new char[] { ':' })[1];

                        if (OpenEhr.Factories.RmFactory.IsRmType(typeName))
                        {
                            if (typeName == "ORIGINAL_VERSION")
                            {
                                OriginalVersion<Composition> version = new OriginalVersion<Composition>();

                                (version as IXmlSerializable).ReadXml(reader);

                                itemsList.Add(version);
                            }
                            else
                            {
                                IXmlSerializable item = OpenEhr.Factories.RmFactory.CreateRmType(typeName) as IXmlSerializable;
                                if (item == null)
                                    throw new System.Xml.XmlException(string.Format("Unsupported RM Type {0}.", typeName));
                                item.ReadXml(reader);
                                itemsList.Add(item);
                            }
                        }
                        else
                        {
                            //reader.ReadStartElement();
                            //reader.MoveToContent();
                            switch (typeName)
                            {
                                case "string": itemsList.Add(reader.ReadElementContentAsString()); break;
                                case "double": itemsList.Add(reader.ReadElementContentAsDouble()); break;
                                case "int": itemsList.Add(reader.ReadElementContentAsInt()); break;
                                case "long": itemsList.Add(reader.ReadElementContentAsLong()); break;
                                case "boolean": itemsList.Add(reader.ReadElementContentAsBoolean()); break;
                                default: throw new System.Xml.XmlException(string.Format("Unsuported type {}.", typeName));
                            }
                            //reader.ReadEndElement();
                            //reader.MoveToContent();
                        }
                    }
                }
            }
            row.ItemArray = itemsList.ToArray();
        }

        void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
        {
            string xsPrefix = OpenEhr.Serialisation.RmXmlSerializer.UseXsdPrefix(writer);
            string xsiPrefix = OpenEhr.Serialisation.RmXmlSerializer.UseXsiPrefix(writer);
            string oePrefix = OpenEhr.Serialisation.RmXmlSerializer.UseOpenEhrPrefix(writer);

            string rsPrefix = UseResultSetPrefix(writer);

            writer.WriteElementString("name", ResultSetTargetNamespace, TableName);

            if (TotalResults > 0)
                writer.WriteElementString("totalResults", ResultSetTargetNamespace, TotalResults.ToString());

            foreach (DataColumn column in this.Columns)
            {
                writer.WriteStartElement("columns", ResultSetTargetNamespace);
                if (column.ColumnName != null)
                    writer.WriteElementString("name", ResultSetTargetNamespace, column.ColumnName);
                ResultsColumn resultColumn = column as ResultsColumn;
                if (resultColumn != null && resultColumn.Path != null)
                    writer.WriteElementString("path", ResultSetTargetNamespace, resultColumn.Path);
                writer.WriteEndElement();   // columns
            }

            // rows
            writer.WriteStartElement("rows", ResultSetTargetNamespace);

            foreach (DataRow row in Rows)
            {
                writer.WriteStartElement("row", ResultSetTargetNamespace);
                foreach (object item in row.ItemArray)
                {
                    writer.WriteStartElement("items", ResultSetTargetNamespace);
                    if (item != null)
                    {
                        IXmlSerializable serializableItem = item as IXmlSerializable;
                        if (serializableItem != null)
                        {
                            OpenEhr.RM.Impl.IRmType rmType = item as OpenEhr.RM.Impl.IRmType;
                            if (rmType != null)
                            {
                                writer.WriteAttributeString(xsiPrefix, "type", OpenEhr.Serialisation.RmXmlSerializer.XsiNamespace,
                                    oePrefix + ":" + rmType.GetRmTypeName());

                            }
                            else
                                throw new ApplicationException("Unable to specify xsi:type of " + item.GetType().ToString());

                            serializableItem.WriteXml(writer);
                        }
                        else if (item is DBNull)
                        {
                            writer.WriteAttributeString(xsiPrefix, "nil", Serialisation.RmXmlSerializer.XsiNamespace, "true");
                        }
                        else
                        {
                            String stringItem = item as String;
                            if (stringItem != null)
                            {
                                writer.WriteAttributeString(xsiPrefix, "type", Serialisation.RmXmlSerializer.XsiNamespace, xsPrefix + ":string");
                                writer.WriteValue(stringItem);
                            }
                            else
                            {
                                if (item is double)
                                {
                                    writer.WriteAttributeString(xsiPrefix, "type", Serialisation.RmXmlSerializer.XsiNamespace, xsPrefix + ":double");
                                    writer.WriteValue(item.ToString());
                                }

                                else if (item is int)
                                {
                                    writer.WriteAttributeString(xsiPrefix, "type", Serialisation.RmXmlSerializer.XsiNamespace, xsPrefix + ":int");
                                    writer.WriteValue(item.ToString());
                                }
                                else if (item is long)
                                {
                                    writer.WriteAttributeString(xsiPrefix, "type", Serialisation.RmXmlSerializer.XsiNamespace, xsPrefix + ":long");
                                    writer.WriteValue(item.ToString());
                                }
                                else if (item is System.Single)
                                {
                                    writer.WriteAttributeString(xsiPrefix, "type", Serialisation.RmXmlSerializer.XsiNamespace, xsPrefix + ":float");
                                    writer.WriteValue(item.ToString());
                                }
                                else
                                    throw new NotImplementedException("serialization on this primitive types (" + item.GetType().Name + " is not implemented");

                            }
                        }
                    }
                    else
                        throw new NotImplementedException("serialization of null row items not implemented");

                    writer.WriteEndElement();   //items
                }

                writer.WriteEndElement();   //row
            }

            writer.WriteEndElement();   //rows
        }

        #endregion

        public const string ResultSetTargetNamespace = "http://oceanehr.com/EhrGate";

        static string UseResultSetPrefix(System.Xml.XmlWriter writer)
        {
            string rsPrefix = writer.LookupPrefix(ResultSetTargetNamespace);
            if (rsPrefix == null)
            {
                rsPrefix = "rs";
                writer.WriteAttributeString("xmlns", rsPrefix, null, ResultSetTargetNamespace);
            }
            return rsPrefix;
        }

        static XmlSchema resultSetSchema;
        static object resultSetSchemaLock = new object();

        public static System.Xml.XmlQualifiedName GetResultsTableSchema(XmlSchemaSet xs)
        {
            if (resultSetSchema == null)
            {
                lock (resultSetSchemaLock)
                {
                    if (resultSetSchema == null)
                    {

                        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                        //string[] names = assembly.GetManifestResourceNames();
                        string resourceName = "OpenEhr.EhrGate.WsClient.ResultSet.ResultSet.xsd";

                        using (System.IO.Stream stream = assembly.GetManifestResourceStream(resourceName))
                        {
                            if (stream == null)
                                throw new ArgumentException("schemaId", "Schema resource " + resourceName + " not found in manifest");

                            XmlSerializer serializer = new XmlSerializer(typeof(XmlSchema));
                            XmlSchema schema = (XmlSchema)serializer.Deserialize(System.Xml.XmlReader.Create(stream));
                            resultSetSchema = schema;
                        }
                    }
                }
            }

            xs.Add(resultSetSchema);
            return new System.Xml.XmlQualifiedName("ResultSet", ResultSetTargetNamespace);
        }

        public static void Serialize(System.Xml.XmlWriter writer, ResultsTable resultSet)
        {
            DesignByContract.Check.Require(writer != null, "writer must not be null");
            DesignByContract.Check.Require(resultSet != null, "resultSet must not be null");

            writer.WriteStartDocument();
            writer.WriteStartElement("resultsTable", ResultSetTargetNamespace);

            ((System.Xml.Serialization.IXmlSerializable)resultSet).WriteXml(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
        }
    }
}
