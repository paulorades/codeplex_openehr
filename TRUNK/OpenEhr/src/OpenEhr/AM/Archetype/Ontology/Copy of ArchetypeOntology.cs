//using System;
//using System.Collections.Generic;
//using System.Text;
//using OpenEhr.DesignByContract;
//using OpenEhr.RM.DataTypes.Text;

//namespace OpenEhr.AM.Ontology
//{
//    /// <summary>
//    /// Local ontology of an archetype. This abstract class defines nearly all the semantics
//    /// of the ontology of an archetype. It is specialised into differential and flat subtypes 
//    /// which implement some routines and supply various different validation semantics.
//    /// </summary>
//    public abstract class ArchetypeOntology
//    {
//        private Support.Assumed.Set<string> terminologyesAvailable;
//        /// <summary>
//        /// List of terminologies to which term or constraint bindings exist in this terminology.
//        /// </summary>
//        public Support.Assumed.Set<string> TerminologyesAvailable
//        {
//            get { return this.terminologyesAvailable; }
//            set
//            {
//                if (value == null)
//                    throw new Exceptions.ValidationException("terminologysAvailable value to be set must not be null.");
//                this.terminologyesAvailable = value;
//            }
//        }

//        private int specialisationDepth;
//        /// <summary>
//        /// Specialisation depth of this archetype. Unspecialised archetypes have depth 0, 
//        /// with each additional level of specialisation adding 1 to the specialisation_depth.
//        /// </summary>
//        public int SpecialisationDepth
//        {
//            get { return this.specialisationDepth; }
//            set
//            {
//                if (value < 0)
//                    throw new Exceptions.ValidationException("specialisationDepth value to be set must not less than zero.");

//                this.specialisationDepth = value;
//            }
//        }

//        private Support.Assumed.List<string> termCodes;
//        /// <summary>
//        /// List of all term codes in the ontology. Most of these correspond to “at” codes in an ADL 
//        /// archetype, which are the node_ids on C_OBJECT descendants. There may be an 
//        /// extra one, if a different term is used as the overall archetype concept from that used as
//        /// the node_id of the outermost C_OBJECT in the definition part.
//        /// </summary>
//        public Support.Assumed.List<string> TermCodes
//        {
//            get { return this.termCodes; }
//            set
//            {
//                if (value != null)
//                    throw new Exceptions.ValidationException("termCodes value to be set must not be null.");
//                this.termCodes = value;
//            }
//        }

//        private Support.Assumed.List<string> constraintCodes;
//        /// <summary>
//        /// List of all term codes in the ontology. These correspond to the “ac” codes in an ADL 
//        /// archetype, or equivalently, the CONSTRAINT_REF.reference values in the archetype definition.
//        /// </summary>
//        public Support.Assumed.List<string> ConstraintCodes
//        {
//            get { return this.constraintCodes; }
//            set
//            {
//                if (value != null)
//                    throw new Exceptions.ValidationException("constraintCodes value to be set must not be null.");
//                this.constraintCodes = value;
//            }
//        }

//        private Support.Assumed.List<string> termAttributeNames;
//        /// <summary>
//        ///List of ‘attribute’ names in ontology terms, typically includes ‘text’, 
//        ///‘description’, ‘provenance’ etc.
//        /// </summary>
//        public Support.Assumed.List<string> TermAttributeNames
//        {
//            get { return this.termAttributeNames; }
//            set
//            {
//                if (value != null)
//                    throw new Exceptions.ValidationException("termAttributeNames value to be set must not be null.");

//                this.termAttributeNames = value;
//            }
//        }

//        private Archetype.Archetype parent;
//        /// <summary>
//        /// Archetype which owns this ontology.
//        /// </summary>
//        public Archetype.Archetype ParentArchetype
//        {
//            get { return this.parent; }
//            set
//            {
//                if (value != null)
//                    throw new Exceptions.ValidationException("parentArchetype value to be set must not be null.");

//                this.parent = value;
//            }
//        }

//        private Support.Assumed.Hash<Support.Assumed.Hash<ArchetypeTerm, string>, string> termDefinitions;
//        /// <summary>
//        /// Directory of term definitions as a two-level table. The outer hash keys are term codes,
//        /// e.g. “at0004”, and the inner hash key are term attribute names, e.g. “text”, “description” etc.
//        /// </summary>
//        public Support.Assumed.Hash<Support.Assumed.Hash<ArchetypeTerm, string>, string> TermDefinitions
//        {
//            get { return this.termDefinitions; }
//            set
//            {
//                if (value != null)
//                    throw new Exceptions.ValidationException("termDefinitions value to be set must not be null.");

//                this.termDefinitions = value;
//            }
//        }

//        private Support.Assumed.Hash<Support.Assumed.Hash<ArchetypeTerm, string>, string> constraintDefinitions;
//        /// <summary>
//        /// Directory of constraint definitions as a twolevel table. The outer hash keys are constraint
//        /// codes, e.g. “ac0004”, and the inner hash keys are constraint attribute names, e.g. “text”, “description” etc.
//        /// </summary>
//        public Support.Assumed.Hash<Support.Assumed.Hash<ArchetypeTerm, string>, string> ConstraintDefinitions
//        {
//            get { return this.constraintDefinitions; }
//            set { this.constraintDefinitions = value; }
//        }

//        private Support.Assumed.Hash<Support.Assumed.Hash<CodePhrase, string>, string> termBindings;
//        /// <summary>
//        /// Directory of term bindings as a two-level table. The outer hash keys are terminology ids, 
//        /// e.g. “SNOMED-CT”, and the inner hash keys are term codes, e.g. “at0004” etc. 
//        /// The indexed CODE_PHRASE objects represent the bound external codes, e.g. Snomed or ICD
//        /// codes in string form, e.g. “SNOMEDCT::10094842”.
//        /// </summary>
//        public Support.Assumed.Hash<Support.Assumed.Hash<CodePhrase, string>, string> TermBindings
//        {
//            get { return this.termBindings; }
//            set { this.termBindings = value; }
//        }

//        private Support.Assumed.Hash<Support.Assumed.Hash<DataTypes.Uri.DvUri, string>, string> constraintBindings;
//        /// <summary>
//        /// Directory of constraint bindings as a twolevel table. The outer hash keys are terminology
//        /// ids, e.g. “SNOMED-CT”, and the inner hash keys are constraint codes, e.g. “ac0004” etc. 
//        /// The indexed URI objects represent references to externally defined resources, usually a terminology subset.
//        /// </summary>
//        public Support.Assumed.Hash<Support.Assumed.Hash<DataTypes.Uri.DvUri, string>, string> ConstraintBindings
//        {
//            get { return this.constraintBindings; }
//            set { this.constraintBindings = value; }
//        }

//        /// <summary>
//        /// True if term_codes has a_code.
//        /// </summary>
//        /// <param name="aCode"></param>
//        /// <returns></returns>
//        public abstract bool HasTermCode(string aCode);       

//        /// <summary>
//        /// True if constraint_codes has a_code.
//        /// </summary>
//        /// <param name="aCode"></param>
//        /// <returns></returns>
//        public abstract bool HasConstraintCode(string aCode);

//        /// <summary>
//        /// Term definition for a code, in a specified language.
//        /// </summary>
//        /// <param name="aLang"></param>
//        /// <param name="aCode"></param>
//        /// <returns></returns>
//        public abstract ArchetypeTerm TermDefinition(string aLang, string aCode);

//        /// <summary>
//        /// Constraint definition for a code, in a specified language.
//        /// </summary>
//        /// <param name="aLang"></param>
//        /// <param name="aCode"></param>
//        /// <returns></returns>
//        public abstract ArchetypeTerm ConstraintDefinition(string aLang, string aCode);

//        /// <summary>
//        /// Binding of term corresponding to a_code in target external terminology a_terminology_id as a CODE_PHRASE.
//        /// </summary>
//        /// <param name="aTerminologyId"></param>
//        /// <param name="aCode"></param>
//        /// <returns></returns>
//        public abstract CodePhrase TermBinding(string aTerminologyId, string aCode);

//        /// <summary>
//        /// Binding of constraint corresponding to a_code in target external terminology a_terminology_id, 
//        /// as a string, which is usually a formal query expression.
//        /// </summary>
//        /// <param name="aTerminologyId"></param>
//        /// <param name="aCode"></param>
//        /// <returns></returns>
//        public abstract string ConstraintBinding(string aTerminologyId, string aCode);

//        /// <summary>
//        /// True if language ‘a_lang’ is present in archetype ontology.
//        /// </summary>
//        /// <param name="language"></param>
//        /// <returns></returns>
//        public bool HasLanguage(string language)
//        {
//            Check.Require(this.ParentArchetype != null, "ArchetypeOntology.Parent must not be null.");

//            return this.ParentArchetype.LanguagesAvailable().Contains(language);
//        }

//        /// <summary>
//        /// True if terminology ‘a_terminology’ is present in archetype ontology.
//        /// </summary>
//        /// <param name="terminology"></param>
//        /// <returns></returns>
//        public bool HasTerminology(string aTerminologyId)
//        {
//            Check.Require(!string.IsNullOrEmpty(aTerminologyId), "terminology must not be null or empty.");

//            return this.TerminologyesAvailable.Contains(aTerminologyId);
//        }

//    }
//}
