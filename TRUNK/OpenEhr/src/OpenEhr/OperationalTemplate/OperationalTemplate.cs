using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Xml.Serialization;

using OpenEhr.RM.DataTypes.Text;
using OpenEhr.RM.Support.Identification;
using OpenEhr.AM;
using OpenEhr.RM.Common.Resource;
using OpenEhr.RM.Common.Generic;
using OpenEhr.AssumedTypes;
using OpenEhr.Serialisation;

namespace OpenEhr.Futures.OperationalTemplate
{
    [System.Xml.Serialization.XmlSchemaProvider("GetXmlSchema")]
    public class OperationalTemplate : System.Xml.Serialization.IXmlSerializable
    {
        CodePhrase language;
        public CodePhrase Language
        {
            get { return language; }
            set { language = value; }
        }

        //BJP: isControlled may be null
        //bool isControlled;
        bool? isControlled = null;
        public bool? IsControlled
        {
            get { return isControlled; }
            set { isControlled = value; }
        }

        ResourceDescription description;
        public ResourceDescription Description
        {
            get { return description; }
            set { description = value; }
        }

        RevisionHistory revisionHistory;
        public RevisionHistory RevisionHistory
        {
            get { return revisionHistory; }
            set { revisionHistory = value; }
        }

        HierObjectId uid;
        public HierObjectId Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        TemplateId templateId;
        public TemplateId TemplateId
        {
            get { return templateId; }
            set { templateId = value; }
        }

        string concept;
        public string Concept
        {
            get { return concept; }
            set { concept = value; }
        }

        CArchetypeRoot definition;
        public CArchetypeRoot Definition
        {
            get { return definition; }
            set { definition = value; }
        }

        private List<OpenEhr.RM.Common.Resource.Annotation> annotations;
        public List<OpenEhr.RM.Common.Resource.Annotation> Annotations
        {
            get { return this.annotations; }
            set { this.annotations = value; }
        }

        private TConstraint constraints;
        public TConstraint Constraints
        {
            get { return this.constraints; }
            set { this.constraints = value; }
        }

        private TView view;
        public TView View
        {
            get { return this.view; }
            set { this.view = value; }
        }

        #region IXmlSerializable Members

        System.Xml.Schema.XmlSchema System.Xml.Serialization.IXmlSerializable.GetSchema()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        void System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
        {
            OperationalTemplateXmlReader templateReader = new OperationalTemplateXmlReader();
            templateReader.ReadOperationalTemplate(reader, this);
        }

        void System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
        {
            OperationalTemplateXmlWriter templateWriter = new OperationalTemplateXmlWriter();
            templateWriter.WriteOperationalTemplate(writer, this);
        }

        public static System.Xml.XmlQualifiedName GetXmlSchema(System.Xml.Schema.XmlSchemaSet xs)
        {
            LoadOperationalTemplateSchemas(xs);
            return new System.Xml.XmlQualifiedName("OPERATIONAL_TEMPLATE", RmXmlSerializer.OpenEhrNamespace);
        }

        static void LoadOperationalTemplateSchemas(System.Xml.Schema.XmlSchemaSet xs)
        {
            //System.Collections.ICollection schemas = xs.Schemas(XmlSerializer.OpenEhrNamespace);
            if (!xs.Contains(RmXmlSerializer.OpenEhrNamespace))
            {
                System.Xml.Schema.XmlSchema baseTypesSchema
                    = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("BaseTypes");
                //xs.Add(baseTypesSchema);

                System.Xml.Schema.XmlSchema resourceSchema
                    = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("Resource");
                resourceSchema.Includes.Clear();
                System.Xml.Schema.XmlSchemaInclude include = new System.Xml.Schema.XmlSchemaInclude();
                include.Schema = baseTypesSchema;
                resourceSchema.Includes.Add(include);
                //xs.Add(resourceSchema);

                System.Xml.Schema.XmlSchema archetypeSchema
                    = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("Archetype");
                archetypeSchema.Includes.Clear();
                include = new System.Xml.Schema.XmlSchemaInclude();
                include.Schema = resourceSchema;
                archetypeSchema.Includes.Add(include);
                //xs.Add(archetypeSchema);

                System.Xml.Schema.XmlSchema openEhrProfileSchema
                    = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("OpenehrProfile");
                openEhrProfileSchema.Includes.Clear();
                include = new System.Xml.Schema.XmlSchemaInclude();
                include.Schema = archetypeSchema;
                openEhrProfileSchema.Includes.Add(include);
                //xs.Add(openEhrProfileSchema);

                System.Xml.Schema.XmlSchema templateSchema
                    = OpenEhr.V1.Its.Xml.XmlSerializer.GetOpenEhrSchema("Template");
                templateSchema.Includes.Clear();
                include = new System.Xml.Schema.XmlSchemaInclude();
                include.Schema = openEhrProfileSchema;
                templateSchema.Includes.Add(include);
                xs.Add(templateSchema);

                //schemas = xs.Schemas(XmlSerializer.OpenEhrNamespace);
                xs.Compile();
            }
        }
        #endregion
    }
}
