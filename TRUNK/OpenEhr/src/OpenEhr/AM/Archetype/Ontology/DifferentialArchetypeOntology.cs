//using System;
//using System.Collections.Generic;
//using System.Text;
//using OpenEhr.DesignByContract;
//using OpenEhr.RM.DataTypes.Uri;
//using OpenEhr.RM.DataTypes.Text;

//namespace OpenEhr.AM.Ontology
//{
//    /// <summary>
//    /// Differential form of an archetype ontology, containing only codes and bindings
//    /// introduced in the current archetype.
//    /// </summary>
//    public class DifferentialArchetypeOntology: ArchetypeOntology
//    {
//        public override bool HasTermCode(string aCode)
//        {
//            Check.Require(!string.IsNullOrEmpty(aCode), "aCode must not be null or empty.");

//            return this.TermCodes.Contains(aCode);
//        }

//        public override bool HasConstraintCode(string aCode)
//        {
//            Check.Require(!string.IsNullOrEmpty(aCode), "aCode must not be null or empty.");

//            return this.ConstraintCodes.Contains(aCode);
//        }

//        public override ArchetypeTerm TermDefinition(string aLang, string aCode)
//        {
//            Check.Require(this.HasLanguage(aLang), "this ArchetypeOntology.HasLanguage(aLang) must be true.");
//            Check.Require(this.HasTermCode(aCode), "this ArchetypeOntology.HasTermCode(aCode) must be true.");

//            Support.Assumed.Hash<ArchetypeTerm, string> languageSpecificTermDefinitions = this.TermDefinitions.Item(aLang);

//            if (languageSpecificTermDefinitions == null)
//                throw new ApplicationException("languageSpecificTermDefinitions must not be null.");

//            return languageSpecificTermDefinitions.Item(aCode);
//        }

//        public override ArchetypeTerm ConstraintDefinition(string aLang, string aCode)
//        {
//            Check.Require(this.HasLanguage(aLang), "this ArchetypeOntology.HasLanguage(aLang) must be true.");
//            Check.Require(this.HasConstraintCode(aCode), "this ArchetypeOntology.HasConstraintCode(aCode) must be true.");

//            Support.Assumed.Hash<ArchetypeTerm, string> languageSpecificConstraintDefinitions = this.TermDefinitions.Item(aLang);

//            if (languageSpecificConstraintDefinitions == null)
//                throw new ApplicationException("languageSpecificConstraintDefinitions must not be null.");

//            return languageSpecificConstraintDefinitions.Item(aCode);
//        }

//        public override OpenEhr.RM.DataTypes.Text.CodePhrase TermBinding(string aTerminologyId, string aCode)
//        {
//            Check.Require(this.HasTerminology(aTerminologyId), "this ArchetypeOntology.HasTerminology(aTerminologyId) must be true.");
//            Check.Require(this.HasTermCode(aCode), "this ArchetypeOntology.HasTermCode(aCode) must be true.");

//            Support.Assumed.Hash<CodePhrase, string> terminologySpecificTermBinding = this.TermBindings.Item(aTerminologyId);

//            if (terminologySpecificTermBinding == null)
//                throw new ApplicationException("terminologySpecificTermBinding must not be null.");

//            return terminologySpecificTermBinding.Item(aCode);

//        }

//        public override string ConstraintBinding(string aTerminologyId, string aCode)
//        {
//            Check.Require(this.HasTerminology(aTerminologyId), "this ArchetypeOntology.HasTerminology(aTerminologyId) must be true.");
//            Check.Require(this.HasConstraintCode(aCode), "this ArchetypeOntology.HasConstraintCode(aCode) must be true.");

//            Support.Assumed.Hash<DvUri, string> terminologySpecificConstraintBinding = this.ConstraintBindings.Item(aTerminologyId);

//            if (terminologySpecificConstraintBinding == null)
//                throw new ApplicationException("terminologySpecificConstraintBinding must not be null.");

//            return terminologySpecificConstraintBinding.Item(aCode).Value;
//        }
//    }
//}
