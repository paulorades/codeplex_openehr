using System.Text;
using System.Xml;

using OpenEhr.DesignByContract;
using OpenEhr.RM.Common.ChangeControl;
using OpenEhr.RM.Impl;
using OpenEhr.RM.DataTypes.Text;
using OpenEhr.RM.Support.Identification;

namespace OpenEhr.Serialisation
{

    public class RmXmlSerializer
    {
        public const string OpenEhrNamespace = "http://schemas.openehr.org/v1";
        public const string XsiNamespace = "http://www.w3.org/2001/XMLSchema-instance";
        public const string XsdNamespace = "http://www.w3.org/2001/XMLSchema";


        private static System.Xml.Schema.XmlSchema baseTypesSchema = null;
        private static object baseTypesSchemaLock = new object();

        public static void LoadBaseTypesSchema(System.Xml.Schema.XmlSchemaSet xs)
        {
            if (!xs.Contains(OpenEhrNamespace))
            {
                if (baseTypesSchema == null)
                {
                    lock (baseTypesSchemaLock)
                    {
                        if (baseTypesSchema == null)
                            baseTypesSchema = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("BaseTypes");
                    }
                }
                xs.Add(baseTypesSchema);

                // HKF: Causes problems in template data objects components 
                // using .NET 3 DataContractAttribute
                //xs.Compile();
            }
        }

        private static System.Xml.Schema.XmlSchema structureSchema = null;
        private static object structureSchemaLock = new object();

        public static void LoadStructureSchema(System.Xml.Schema.XmlSchemaSet xs)
        {
            if (!xs.Contains(OpenEhrNamespace))
            {
                //LoadBaseTypesSchema(xs);
                if (structureSchema == null)
                {
                    lock (structureSchemaLock)
                    {
                        if (structureSchema == null)
                        //structureSchema = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("Structure");
                        {
                            System.Xml.Schema.XmlSchema tempSchema
                                = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("Structure");
                            //System.Xml.Schema.XmlSchemaInclude schemaInclude;
                            tempSchema.Includes.RemoveAt(0);

                            //schemaInclude = contentSchema.Includes[0] as System.Xml.Schema.XmlSchemaInclude;
                            //schemaInclude.Schema = structureSchema;
                            //schemaInclude.SchemaLocation = null;

                            //contentSchema.Includes.RemoveAt(0);

                            System.Xml.Schema.XmlSchema includeSchema
                                = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("BaseTypes");
                            //schemaInclude = structureSchema.Includes[0] as System.Xml.Schema.XmlSchemaInclude;
                            //schemaInclude.Schema = baseTypesSchema;
                            //schemaInclude.SchemaLocation = null;

                            foreach (System.Xml.Schema.XmlSchemaObject item in includeSchema.Items)
                                tempSchema.Items.Add(item);

                            structureSchema = tempSchema;
                        }
                    }
                }
                xs.Add(structureSchema);
            }
        }

        public static void LoadCompositionSchema(System.Xml.Schema.XmlSchemaSet xs)
        {
            if (!xs.Contains(OpenEhrNamespace))
            {
                System.Xml.Schema.XmlSchema compositionSchema
                    = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("Composition");

                System.Xml.Schema.XmlSchema contentSchema
                    = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("Content");
                //System.Xml.Schema.XmlSchemaInclude schemaInclude; 
                //    = compositionSchema.Includes[0] as System.Xml.Schema.XmlSchemaInclude;
                //schemaInclude.Schema = contentSchema;
                //schemaInclude.SchemaLocation = null;
                compositionSchema.Includes.RemoveAt(0);

                foreach (System.Xml.Schema.XmlSchemaObject item in contentSchema.Items)
                    compositionSchema.Items.Add(item);

                System.Xml.Schema.XmlSchema structureSchema
                    = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("Structure");
                //schemaInclude = contentSchema.Includes[0] as System.Xml.Schema.XmlSchemaInclude;
                //schemaInclude.Schema = structureSchema;
                //schemaInclude.SchemaLocation = null;

                //contentSchema.Includes.RemoveAt(0);
                //compositionSchema.Includes.Add(schemaInclude);
                foreach (System.Xml.Schema.XmlSchemaObject item in structureSchema.Items)
                    compositionSchema.Items.Add(item);

                System.Xml.Schema.XmlSchema baseTypesSchema
                    = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("BaseTypes");
                //schemaInclude = structureSchema.Includes[0] as System.Xml.Schema.XmlSchemaInclude;
                //schemaInclude.Schema = baseTypesSchema;
                //schemaInclude.SchemaLocation = null;

                foreach (System.Xml.Schema.XmlSchemaObject item in baseTypesSchema.Items)
                    compositionSchema.Items.Add(item);

                xs.Add(compositionSchema);

                xs.Compile();
            }
        }

        public static void LoadEhrStatusSchema(System.Xml.Schema.XmlSchemaSet xs)
        {
           if (!xs.Contains(OpenEhrNamespace))
            {
                System.Xml.Schema.XmlSchema ehrStatusSchema
                    = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("EhrStatus");
                ehrStatusSchema.Includes.RemoveAt(0);
                
                System.Xml.Schema.XmlSchema structureSchema
                    = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("Structure");
              
                foreach (System.Xml.Schema.XmlSchemaObject item in structureSchema.Items)
                    ehrStatusSchema.Items.Add(item);

                System.Xml.Schema.XmlSchema baseTypesSchema
                    = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("BaseTypes");
              
                foreach (System.Xml.Schema.XmlSchemaObject item in baseTypesSchema.Items)
                    ehrStatusSchema.Items.Add(item);

                xs.Add(ehrStatusSchema);

                xs.Compile();
            }
        }

        public static void LoadVersionSchema(System.Xml.Schema.XmlSchemaSet xs)
        {
            if (!xs.Contains(OpenEhrNamespace))
            {
                xs.Add(OpenEhr.V1.Its.Xml.XmlSerializer.VersionSchema);
            }
        }

        public static void LoadExtractSchema(System.Xml.Schema.XmlSchemaSet xs)
        {
            if (!xs.Contains(OpenEhrNamespace))
            {
                System.Xml.Schema.XmlSchema extractSchema
                    = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("Extract");
                extractSchema.Includes.RemoveAt(0);

                System.Xml.Schema.XmlSchema schema
                    = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("Version");
                foreach (System.Xml.Schema.XmlSchemaObject item in schema.Items)
                    extractSchema.Items.Add(item);

                System.Xml.Schema.XmlSchema compositionSchema
                    = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("Composition");
                foreach (System.Xml.Schema.XmlSchemaObject item in compositionSchema.Items)
                    extractSchema.Items.Add(item);

                System.Xml.Schema.XmlSchema contentSchema
                    = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("Content");
                foreach (System.Xml.Schema.XmlSchemaObject item in contentSchema.Items)
                    extractSchema.Items.Add(item);

                System.Xml.Schema.XmlSchema structureSchema
                    = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("Structure");
                foreach (System.Xml.Schema.XmlSchemaObject item in structureSchema.Items)
                    extractSchema.Items.Add(item);

                System.Xml.Schema.XmlSchema baseTypesSchema
                    = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("BaseTypes");
                foreach (System.Xml.Schema.XmlSchemaObject item in baseTypesSchema.Items)
                    extractSchema.Items.Add(item);

                xs.Add(extractSchema);
            }
        }

        public static string UseOpenEhrPrefix(System.Xml.XmlWriter writer)
        {
            string oePrefix = writer.LookupPrefix(OpenEhrNamespace);
            if (oePrefix == null)
            {
                oePrefix = "oe";
                writer.WriteAttributeString("xmlns", oePrefix, null, OpenEhrNamespace);
            }
            return oePrefix;
        }

        public static string UseXsiPrefix(System.Xml.XmlWriter writer)
        {
            string xsiPrefix = writer.LookupPrefix(XsiNamespace);
            if (xsiPrefix == null)
            {
                xsiPrefix = "xsi";
                writer.WriteAttributeString("xmlns", xsiPrefix, null, XsiNamespace);
            }

            return xsiPrefix;
        }

        public static string UseXsdPrefix(System.Xml.XmlWriter writer)
        {
            string xsdPrefix = writer.LookupPrefix(XsdNamespace);
            if (xsdPrefix == null)
            {
                xsdPrefix = "xsd";
                writer.WriteAttributeString("xmlns", xsdPrefix, null, XsdNamespace);
            }
            return xsdPrefix;
        }

        internal static void WriteXml(XmlWriter writer, ObjectId objectId)
        {
            string xsiPrefix = RmXmlSerializer.UseXsiPrefix(writer);
            string oePrefix = RmXmlSerializer.UseOpenEhrPrefix(writer);

            string typeName = ((IRmType)objectId).GetRmTypeName();
            if (!string.IsNullOrEmpty(oePrefix))
                typeName = oePrefix + ":" + typeName;

            writer.WriteAttributeString(xsiPrefix, "type", XsiNamespace, typeName);

            ((System.Xml.Serialization.IXmlSerializable)objectId).WriteXml(writer);
        }

        internal static void WriteXml(XmlWriter writer, ObjectRef objectRef)
        {
            if (objectRef.GetType() != typeof(ObjectRef))
            {
                string xsiPrefix = RmXmlSerializer.UseXsiPrefix(writer);
                string oePrefix = RmXmlSerializer.UseOpenEhrPrefix(writer);

                string typeName = ((IRmType)objectRef).GetRmTypeName();
                if (!string.IsNullOrEmpty(oePrefix))
                    typeName = oePrefix + ":" + typeName;

                writer.WriteAttributeString(xsiPrefix, "type", XsiNamespace, typeName);
            }

            ((System.Xml.Serialization.IXmlSerializable)objectRef).WriteXml(writer);
        }

        internal static void WriteXml(XmlWriter writer, DvText text)
        {
            if (text.GetType() != typeof(DvText))
            {
                string xsiPrefix = RmXmlSerializer.UseXsiPrefix(writer);
                string oePrefix = RmXmlSerializer.UseOpenEhrPrefix(writer);

                string typeName = ((IRmType)text).GetRmTypeName();
                if (!string.IsNullOrEmpty(oePrefix))
                    typeName = oePrefix + ":" + typeName;

                writer.WriteAttributeString(xsiPrefix, "type", XsiNamespace, typeName);
            }

            ((System.Xml.Serialization.IXmlSerializable)text).WriteXml(writer);
        }

        public static string ReadXsiType(XmlReader reader)
        {
            Check.Require(reader != null, "reader must not be null");

            string xsiType = reader.GetAttribute("type", XsiNamespace);

            if (xsiType != null)
            {
                int index = xsiType.IndexOf(':');
                if (index >= 0)
                    xsiType = xsiType.Substring(index + 1);
            }
            return xsiType;
        }

        /// <summary>
        /// XML serialisation to TextWriter
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="version"></param>
        static public void Serialize<T>(XmlWriter writer, Version<T> version)
            where T : class
        {
            Check.Require(writer != null, "writer type must not be null");
            Check.Require(version != null, "version type must not be null");

            //OpenEhr.V1.Its.Xml.XmlSerializer.Serialize(xmlWriter, version.ToItsXmlVersion());
            writer.WriteStartElement("version", OpenEhrNamespace);
            version.WriteXml(writer);
            writer.WriteEndElement();
            writer.Flush();
        }

        static public Version<T> Deserialize<T>(XmlReader reader)
            where T : class
        {
            //ORIGINAL_VERSION versionType
            //    = OpenEhr.V1.Its.Xml.XmlSerializer.DeserializeVersion(reader) as ORIGINAL_VERSION;

            //Check.Ensure(versionType != null, "version type must not be null");
            //return new OriginalVersion<T>(versionType);
            OriginalVersion<T> version = new OriginalVersion<T>();
            version.ReadXml(reader);
            return version;
        }
    }
}
