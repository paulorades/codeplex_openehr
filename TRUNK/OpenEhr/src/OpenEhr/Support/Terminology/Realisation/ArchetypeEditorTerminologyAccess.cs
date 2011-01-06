//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
//using OpenEhr.RM.Support.Terminology.Impl.Configuration;
//using System.Data;
//using System.IO;
//using OpenEhr.DesignByContract;
//using Ocean.KnowledgeTools.Common.PathHelper;
//using OpenEhr.RM.DataTypes.Text;

//namespace OpenEhr.RM.Support.Terminology.Impl
//{
//    [ConfigurationElementType(typeof(ArchetypeEditorTerminologyAccessData))]
//    public class ArchetypeEditorTerminologyAccess : ITerminologyAccessProvider
//    {
//        private const string xmlResourceName = "OpenEhr.Support.Terminology.Realisation.TerminologyData.terminology.xml";
//        private const string xsdResourceName = "OpenEhr.Support.Terminology.Realisation.TerminologyData.Terminology.xsd";

//        private const int COL_CONCEPT_LANGUAGE = 0;
//        private const int COL_CONCEPT_CODE_ID = 1;
//        private const int COL_CONCEPT_RUBRIC = 2;
//        private const int COL_GROUPER_ID = 0;
//        private const int COL_GROUPER_CODE_ID = 1;
//        private const int COL_GROUPED_CONCEPT_CHILD_ID = 1;


//        private Dictionary<string, string> groupIdMap;

//        readonly string defaultLanguage = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
//        private DataSet Terminology = new DataSet("openEHRTerminology");

//        public ArchetypeEditorTerminologyAccess()
//        {
//            using (Stream data = GetResourceStream(xmlResourceName))
//            {
//                using (Stream schema = GetResourceStream(xsdResourceName))
//                {
//                    this.InitialiseTerminologyAccess(data, schema);
//                }
//            }
//        }

//        public ArchetypeEditorTerminologyAccess(string defaultLanguage)
//            : this()
//        {
//            Check.Require(!string.IsNullOrEmpty(defaultLanguage), "defaultLanguage must not be null or empty string.");
//            this.defaultLanguage = defaultLanguage;
//        }

//        public ArchetypeEditorTerminologyAccess(string xmlFilePath, string defaultLanguage)
//        {
//            Check.Require(!string.IsNullOrEmpty(xmlFilePath), "xmlFilesPath must not be null or empty");
//            xmlFilePath = PathHelper.AbsolutePath(xmlFilePath);
//            Check.Require(File.Exists(xmlFilePath), string.Format("Failed to initialise TerminologyAccess. Could not find file {0}", xmlFilePath));

//            if (!string.IsNullOrEmpty(defaultLanguage))
//                this.defaultLanguage = defaultLanguage;

//            using (Stream data = new FileStream(xmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
//            {
//                using (Stream schema = GetResourceStream(xsdResourceName))
//                {
//                    this.InitialiseTerminologyAccess(data, schema);
//                }
//            }
//        }

//        protected virtual void InitialiseTerminologyAccess(Stream data, Stream schema)
//        {
//            Terminology.ReadXmlSchema(schema);
//            Terminology.ReadXml(data);

//            DataColumn[] keyFields = new DataColumn[2];
//            keyFields[0] = Terminology.Tables["Concept"].Columns[0];
//            keyFields[1] = Terminology.Tables["Concept"].Columns[1];
//            Terminology.Tables["Concept"].PrimaryKey = keyFields;

//            keyFields = new DataColumn[1];
//            keyFields[0] = Terminology.Tables["TerminologyIdentifiers"].Columns[0];
//            Terminology.Tables["TerminologyIdentifiers"].PrimaryKey = keyFields;

//            keyFields = new DataColumn[1];
//            keyFields[0] = Terminology.Tables["Territory"].Columns[0];
//            Terminology.Tables["Territory"].PrimaryKey = keyFields;


//            groupIdMap = new Dictionary<string, string>();
//            groupIdMap.Add(TerminologyService.GroupIdAuditChangeType, "13");
//            groupIdMap.Add(TerminologyService.GroupIdAttestationReason, "11");
//            groupIdMap.Add(TerminologyService.GroupIdCompositionCategory, "20");
//            groupIdMap.Add(TerminologyService.GroupIdEventMathFunction, "14");
//            groupIdMap.Add(TerminologyService.GroupIdInstructionStates, "21");
//            groupIdMap.Add(TerminologyService.GroupIdInstructionTransitions, "22");
//            groupIdMap.Add(TerminologyService.GroupIdNullFlavours, "15");
//            groupIdMap.Add(TerminologyService.GroupIdProperty, "8");
//            groupIdMap.Add(TerminologyService.GroupIdParticipationFunction, "26");
//            groupIdMap.Add(TerminologyService.GroupIdParticipationMode, "9");
//            groupIdMap.Add(TerminologyService.GroupIdSetting, "10");
//            groupIdMap.Add(TerminologyService.GroupIdTermMappingPurpose, "27");
//            groupIdMap.Add(TerminologyService.GroupIdSubjectRelationship, "1");
//            groupIdMap.Add(TerminologyService.GroupIdVersionLifecycleState, "12");

//            //Hmmmm have to support this because Archetype editor uses it.
//            groupIdMap.Add("MultiMedia", "19");

//        }


//        #region ITerminologyAccess Members

//        public string Id
//        {
//            get { return TerminologyService.TerminologyIdOpenehr; }
//        }

//        private List<string> codes;
//        private List<CodePhrase> allCodes;
//        public List<CodePhrase> AllCodes
//        {
//            get
//            {
//                if (this.allCodes == null)
//                {
//                    this.allCodes = new List<OpenEhr.RM.DataTypes.Text.CodePhrase>();
//                    string code;
//                    codes = new List<string>();
//                    foreach (DataRow row in Terminology.Tables["Concept"].Rows)
//                    {
//                        code = row[COL_CONCEPT_CODE_ID].ToString();
//                        Check.Assert(!string.IsNullOrEmpty(code));
//                        if (!codes.Contains(code)) //check if code already is in list
//                        {
//                            codes.Add(code);
//                            CodePhrase codePhrase = new CodePhrase(code, this.Id);
//                            this.allCodes.Add(codePhrase);
//                        }
//                    }
//                    Check.Assert(codes.Count == this.allCodes.Count);
//                }
//                return this.allCodes;
//            }
//        }

//        public List<CodePhrase> CodesForGroupId(string groupId)
//        {
//            //Check.Require(groupIdMap.ContainsKey(groupId), "Unknown groupId: " + groupId);

//            List<CodePhrase> groupCodes = new List<CodePhrase>();

//            if (groupIdMap.ContainsKey(groupId))
//            {

//                string groupConceptFilter = string.Format("GrouperID={0}", groupIdMap[groupId]);
//                DataRow[] filteredRows = Terminology.Tables["GroupedConcept"].Select(groupConceptFilter);
//                string childCode;
//                foreach (DataRow row in filteredRows)
//                {
//                    childCode = row[COL_GROUPED_CONCEPT_CHILD_ID].ToString();
//                    Check.Assert(!string.IsNullOrEmpty(childCode));
//                    CodePhrase codePhrase = new CodePhrase(childCode, this.Id);
//                    groupCodes.Add(codePhrase);
//                }
//            }
//            return groupCodes;
//        }

//        public bool HasCodeForGroupId(string groupId, CodePhrase code)
//        {
//            if (!groupIdMap.ContainsKey(groupId))
//                return false;


//            string groupFilter = string.Format("GrouperID={0} and ChildID={1}", groupIdMap[groupId], code.CodeString);
//            DataRow[] filteredRows = Terminology.Tables["GroupedConcept"].Select(groupFilter);
//            return filteredRows.Length > 0;
//        }

//        public List<OpenEhr.RM.DataTypes.Text.CodePhrase> CodesForGroupName(
//            string name, string language)
//        {
//            if (string.IsNullOrEmpty(language))
//                language = this.defaultLanguage;

//            DataRow[] groupConceptRow;
//            string groupConceptFilter =
//                string.Format("Language='{0}' AND Rubric='{1}'", language, name);
//            groupConceptRow = Terminology.Tables["Concept"].Select(groupConceptFilter);
//            if (groupConceptRow.Length < 1)
//                return null;

//            string grouperFilter = string.Format(
//                "ConceptID={0}", groupConceptRow[0][COL_CONCEPT_CODE_ID].ToString());
//            groupConceptRow = Terminology.Tables["Grouper"].Select(grouperFilter);

//            DataRow[] selectedRows;
//            string groupedConceptFilter =
//                string.Format("GrouperID={0}", groupConceptRow[0][COL_GROUPER_ID]);
//            selectedRows = Terminology.Tables["GroupedConcept"].Select(groupedConceptFilter);
//            DataRow[] concepts = this.GetConcepts(selectedRows, language);

//            if (concepts.Length < 1)
//                return null;

//            List<CodePhrase> codesForGroupName = new List<CodePhrase>();
//            string conceptCode;
//            foreach (DataRow row in concepts)
//            {
//                conceptCode = row[COL_CONCEPT_CODE_ID].ToString();
//                Check.Assert(!string.IsNullOrEmpty(conceptCode));
//                CodePhrase codePhrase = new CodePhrase(conceptCode, this.Id);
//                codesForGroupName.Add(codePhrase);
//            }
//            return codesForGroupName;
//        }

//        public string RubricForCode(string code, string lang)
//        {
//            Check.Require(!string.IsNullOrEmpty(code), "code must not be null or empty");
//            Check.Require(!string.IsNullOrEmpty(lang), "lang must not be null or empty");

//            object[] keys = new object[2];
//            keys[0] = lang;
//            keys[1] = code;
//            DataRow selectedRow = Terminology.Tables["Concept"].Rows.Find(keys);

//            string result = null;

//            if (selectedRow != null)
//                result = selectedRow[COL_CONCEPT_RUBRIC].ToString();
//            else
//            {
//                // If there is a standard version of the languageCode return that
//                int indexOfDash = lang.IndexOf("-");
//                if (indexOfDash > 0)
//                {
//                    keys[0] = lang.Substring(0, indexOfDash);
//                    selectedRow = Terminology.Tables["Concept"].Rows.Find(keys);
//                    if ((selectedRow != null))
//                        result = selectedRow[COL_CONCEPT_RUBRIC].ToString();
//                }

//                if (result == null)
//                {
//                    // Nothing was found so return the default language text if not
//                    // seeking default language already. Otherwise "?" because has failed.
//                    result = lang != this.defaultLanguage ?
//                        this.RubricForCode(code, this.defaultLanguage) : "?";
//                }
//            }
//            //return result.ToLower();
//            return result;
//        }
//        #endregion

//        #region Helpers
//        private Stream GetResourceStream(string resourceName)
//        {
//            Check.Require(!string.IsNullOrEmpty(resourceName), "Resource name cannot be null or empty string.");
//            Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
//            Check.Ensure(stream != null, "Stream must not be null.");
//            return stream;
//        }

//        private DataRow[] GetConcepts(DataRow[] dataRows, string languageCode)
//        {
//            DataRow[] selectedTerms;
//            string filterString;
//            StringBuilder stringBuilder1 = new StringBuilder();
//            StringBuilder stringBuilder2 = new StringBuilder();
//            stringBuilder1.AppendFormat("Language='{0}' AND (ConceptID=", languageCode);

//            int rowIndexer;
//            for (rowIndexer = 0; rowIndexer <= dataRows.Length - 1; rowIndexer++)
//            {
//                stringBuilder2.Append(dataRows[rowIndexer][COL_CONCEPT_CODE_ID].ToString());
//                if (rowIndexer < dataRows.Length - 1)
//                    stringBuilder2.Append(" OR ConceptID=");
//            }
//            stringBuilder2.Append(")");
//            filterString = string.Format("{0}{1}", stringBuilder1.ToString(), stringBuilder2.ToString());
//            selectedTerms = Terminology.Tables["Concept"].Select(filterString);

//            if (selectedTerms.Length > 0 && (selectedTerms.Length < dataRows.Length))
//            {
//                // Differential translation or not completely translated
//                int indexOfDash;
//                string standardLanguage;

//                indexOfDash = languageCode.IndexOf("-");

//                if (indexOfDash > -1)
//                {
//                    string code;
//                    string last_code;
//                    bool subsequentStandard = false;
//                    bool subsequentLanguage = false;

//                    // Use standard language as master
//                    standardLanguage = languageCode.Substring(0, indexOfDash);
//                    filterString = string.Format("(Language='{0}' OR Language='{1}') AND (ConceptID={2}", standardLanguage, languageCode, stringBuilder2.ToString());
//                    selectedTerms = Terminology.Tables["Concept"].Select(filterString, "ConceptID ASC, Language DESC");

//                    stringBuilder2 = new StringBuilder();
//                    stringBuilder2.AppendFormat("Language='{0}' AND (ConceptID=", standardLanguage);

//                    last_code = "";

//                    for (rowIndexer = 0; rowIndexer <= selectedTerms.Length - 1; rowIndexer++)
//                    {
//                        code = selectedTerms[rowIndexer][COL_CONCEPT_CODE_ID].ToString();
//                        if (code != last_code)
//                        {
//                            // Needed
//                            if (selectedTerms[rowIndexer][COL_CONCEPT_LANGUAGE].ToString() == languageCode)
//                            {
//                                if (subsequentLanguage)
//                                    stringBuilder1.Append("OR ConceptID=");
//                                else
//                                    subsequentLanguage = true;
//                                stringBuilder1.Append(selectedTerms[rowIndexer][COL_CONCEPT_CODE_ID].ToString());
//                            }
//                            else
//                            {
//                                // Standard language
//                                if (subsequentStandard)
//                                    // first one so no or
//                                    stringBuilder2.Append("OR ConceptID=");
//                                else
//                                    subsequentStandard = true;
//                                stringBuilder2.Append(selectedTerms[rowIndexer][COL_CONCEPT_CODE_ID].ToString());
//                            }

//                        }
//                        last_code = code;
//                    }

//                    filterString = string.Format("(({0}) OR ({1}))", stringBuilder1.ToString(), stringBuilder2.ToString());
//                    selectedTerms = Terminology.Tables["Concept"].Select(filterString);
//                }
//            }

//            else if (languageCode != "" & selectedTerms.Length == 0)
//            {
//                // No terms in this language
//                this.TranslateGroup(filterString, languageCode);
//                selectedTerms = Terminology.Tables["Concept"].Select(filterString);
//            }

//            return selectedTerms;
//        }

//        private void TranslateGroup(string filterString, string languageCode)
//        {
//            DataRow[] selectedRows;
//            DataRow newRow;
//            int indexOfAnd;
//            indexOfAnd = filterString.IndexOf("AND");
//            filterString = string.Format("Language='en' {0}", filterString.Substring(indexOfAnd));
//            selectedRows = Terminology.Tables["Concept"].Select(filterString);
//            foreach (DataRow row in selectedRows)
//            {
//                newRow = Terminology.Tables["Concept"].NewRow();
//                newRow[0] = languageCode;
//                newRow[1] = row[1];
//                newRow[2] = string.Format("*{0}(en)", row[COL_CONCEPT_RUBRIC]);
//                Terminology.Tables["Concept"].Rows.Add(newRow);
//            }
//        }
//        #endregion
//    }
//}
