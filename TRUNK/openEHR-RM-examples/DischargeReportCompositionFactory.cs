using System;
using System.Collections.Generic;
using System.Text;
using OpenEhr.RM.Composition;
using OpenEhr.RM.Common.Generic;
using OpenEhr.RM.Common.Archetyped.Impl;
using OpenEhr.RM.DataTypes.Text;
using OpenEhr.RM.DataStructures.ItemStructure;
using OpenEhr.RM.DataStructures.ItemStructure.Representation;
using OpenEhr.RM.Support.Identification;
using OpenEhr.RM.DataTypes.Quantity.DateTime;
using OpenEhr.RM.Composition.Content;
using OpenEhr.RM.Composition.Content.Entry;
using OpenEhr.RM.DataTypes.Encapsulated;

namespace openEHR_RM_examples
{    
    class DischargeReportCompositionFactory
    {
        public static Composition GetComposition()
        {
            return new DischargeReportCompositionFactory().InternalComposition;
        }
        private Composition _composition;

        private DischargeReportCompositionFactory() //singleton factory
        {
            BuildComposition();
        }

        private Composition InternalComposition{
            get{
                return _composition;
            }
        }

        private void BuildComposition()
        {

            string archetypeNodeId =  "openEHR-EHR-COMPOSITION.discharge.v1";
            DvText name = new OpenEhr.RM.DataTypes.Text.DvText("Discharge report");
            Archetyped archetypeDetails = GetArchetypeDetails();            
            CodePhrase language = new CodePhrase("en", "ISO_639-1");
            CodePhrase territory = new CodePhrase("AU", "ISO_3166-1");
            DvCodedText category = new DvCodedText("event", "433", "openehr");
            PartyIdentified composer = new PartyIdentified("EhrGateUnit");
            EventContext context = GetContext();
            ContentItem[] contents = getContent();
            _composition = new Composition(name, archetypeNodeId, null, null, archetypeDetails, null, language, territory, category, context, contents, composer);
        }

        private void buildComposition()
        {
            Composition comp = new Composition();
            DvCodedText category = new DvCodedText("event", "433", "openehr");
            comp.Category = category;
            //....
        }

        private ContentItem[] getContent()
        {
            ContentItem[] list = new ContentItem[3];
            //EVALUATION
            list[0] = getEvaluationClinicalSynopsis();
            
            //EVALUATION
            list[1] =  getEvaluationProblemDiagnosis();
            
            //INSTRUCTION
            list[2] =  getInstructinLabReqestREFACTORED();

            return list;
        }

        private ContentItem getInstructinLabReqestREFACTORED()
        {
            
                //element
            Element el0005 = new Element(new DvText("Test name"), "at0005", null, null, null, null, new DvText("Wound swap"), null);
            //cluster at0004            
            Cluster cl0004 = new Cluster(new DvText("Test required"), "at0004", null, null, null, null, new Item[] { el0005 });            

            //element at0006
            Element el0006 = new Element(new DvText("DateTime requested"), "at0006", null,null, null, null, new DvDateTime("20070620T155936"), null);            
            
                //element
            Element el0009 = new Element(new DvText("Name"), "at0009", null, null, null, null, new DvText("Dr W Wise"), null);
                    //element
            Element el0014 = new Element(new DvText("Address"), "at0014", null, null, null, null, new DvText("TRAKCARE"), null);
                //cluster
            string cl0010ArchetypeNodeId = "at0010";
            DvText cl0010Name = new DvText("Contact details");
            Cluster cl0010 = new Cluster(cl0010Name, cl0010ArchetypeNodeId, null, null, null, null, new Item[]{el0014});
            //cluster at0008             
            string cl0008ArchetypeNodeId = "at0008";
            DvText cl0008Name = new DvText("Requesting provider details");
            Cluster cl0008 = new Cluster(cl0008Name, cl0008ArchetypeNodeId, null, null, null, null, new Item[]{el0009, cl0010});            
            
                //element
            Element el0016 = new Element(new DvText("name"), "at0016", null, null, null, null, new DvText("CERNERLAB"), null);
            //cluster at0015
            Cluster cl0015 = new Cluster(new DvText("Laboratory details"), "at0015", null, null, null, null, new Item[]{el0016});

                    //items/Element at0030
            Element el0030 = new Element(new DvText("Descriptions"), "at0030", null, null, null, null, new DvText("wound swab for culture from infected biopsy"), null);
                //cluster at0029            
            Cluster cl0029 = new Cluster(new DvText("Specimen details"), "at0029", null, null, null, null, new Item[]{el0030});
            //cluster at0022            
            Cluster cl0022 = new Cluster(new DvText("Process"), "at0022", null, null, null, null, new Item[]{cl0029}) ;

            //itemtree
            ItemTree act001Description = new ItemTree(new DvText("Laboratory test request"), "openEHR-EHR-ITEM_TREE.laboratory_request.v1", null, null, null, null, new Item[]{ cl0008, cl0015, cl0022, el0006, cl0004});

            Activity act001 = new Activity(new DvText("Swab"), "at0001", null, null, null, null, act001Description, new DvParsable("20070620155800", "HL7"), "openEHR-EHR-ACTION.laboratory.v1");
            
            Instruction inst = new Instruction(new DvText("General Swab"), 
                                                "openEHR-EHR-INSTRUCTION.laboratory_request.v1", 
                                                null, null, null, null, new CodePhrase("en", "ISO_639-1"),
                                                new CodePhrase("UTF-8", "IANA_character-sets"), 
                                                new PartySelf(), null, null, null, null, null, new DvText("wound swab for culture from infected biopsy"), null, new Activity[]{act001}, null); 
            return inst;
        }

        //private ContentItem getInstructionLabRequest()
        //{
        //    Instruction inst = new Instruction();
        //    inst.ArchetypeNodeId = "openEHR-EHR-INSTRUCTION.laboratory_request.v1";
        //    inst.Name = new DvText("General Swab");
        //    inst.Language = new CodePhrase("en", "ISO_639-1");
        //    inst.Encoding = new CodePhrase("UTF-8", "IANA_character-sets");
        //    inst.Subject = new PartySelf();
        //    inst.Narrative = new DvText("wound swab for culture from infected biopsy");
        //    Activity act001 = new Activity();
        //    inst.Activities.Add(act001);

        //    act001.ArchetypeNodeId = "at0001";
        //    act001.Name = new DvText("Swab");
        //    act001.Description = new ItemTree();
        //    (act001.Description as ItemTree).ArchetypeNodeId = "openEHR-EHR-ITEM_TREE.laboratory_request.v1";
        //    (act001.Description as ItemTree).Name = new DvText("Laboratory test request");
        //    //cluster at0004
        //    Cluster cl0004 = new Cluster();
        //    cl0004.ArchetypeNodeId = "at0004";
        //    cl0004.Name = new DvText("Test required");
        //    Element el0005 = new Element();
        //    el0005.Name = new DvText("Test name");
        //    el0005.Value = new DvText("Wound swap");
        //    cl0004.Items.Add(el0005);

        //    (act001.Description as ItemTree).Items.Add(cl0004);

        //    //element at0006
        //    Element el0006 = new Element();
        //    el0006.ArchetypeNodeId = "at0006";
        //    el0006.Name = new DvText("DateTime requested");
        //    el0006.Value = new DvDateTime("20070620T155936");
        //    (act001.Description as ItemTree).Items.Add(el0006);

        //    //cluster at0008 
        //    Cluster cl0008 = new Cluster();
        //    cl0008.ArchetypeNodeId = "at0008";
        //    cl0008.Name = new DvText("Requesting provider details");
        //        //element
        //    Element el0009 = new Element();
        //    el0009.ArchetypeNodeId = "at0009";
        //    el0009.Name = new DvText("Name");
        //    el0009.Value = new DvText("Dr W Wise");
        //    cl0008.Items.Add(el0009);
        //        //cluster
        //    Cluster cl0010 = new Cluster();
        //    cl0010.ArchetypeNodeId = "at0010";
        //    cl0010.Name = new DvText("Contact details");
        //            //element
        //    Element el0014 = new Element();
        //    el0014.ArchetypeNodeId = "at0014";
        //    el0014.Name = new DvText("Address");
        //    el0014.Value = new DvText("TRAKCARE");
        //    cl0010.Items.Add(el0014);

        //    cl0008.Items.Add(cl0010);

        //    (act001.Description as ItemTree).Items.Add(cl0008);
            
        //    //cluster at0015
        //    Cluster cl0015 = new Cluster();
        //    cl0015.ArchetypeNodeId = "at0015";
        //    cl0015.Name = new DvText("Laboratory details");
        //        //items
        //    Element el0016 = new Element();
        //    el0016.ArchetypeNodeId = "at0016";
        //    el0016.Name = new DvText("Name");
        //    el0016.Value = new DvText("CERNERLAB");
        //    cl0015.Items.Add(el0016);

        //    (act001.Description as ItemTree).Items.Add(cl0015);

        //    //cluster at0022
        //    Cluster cl0022 = new Cluster();
        //    cl0022.ArchetypeNodeId = "at0022";
        //    cl0022.Name = new DvText("Process");
        //        //cluster at0029
        //    Cluster cl0029 = new Cluster();
        //    cl0029.ArchetypeNodeId = "at0029";
        //    cl0029.Name = new DvText("Specimen details");
        //            //items/Element at0030
        //    Element el0030 = new Element();
        //    el0030.ArchetypeNodeId = "at0030";
        //    el0030.Name = new DvText("Descriptions");
        //    el0030.Value = new DvText("wound swab for culture from infected biopsy");
        //    cl0029.Items.Add(el0030);

        //    cl0022.Items.Add(cl0029);

        //    (act001.Description as ItemTree).Items.Add(cl0022);

        //    //activity timing
        //    act001.Timing = new OpenEhr.RM.DataTypes.Encapsulated.DvParsable("20070620155800", "HL7");
        //    act001.ActionArchetypeId = "openEHR-EHR-ACTION.laboratory.v1";

        //    return inst;
        //}

        #region openEHR-EHR-EVALUATION.problem-diagnosis.v1 Evaluation
        private ContentItem getEvaluationProblemDiagnosis()
        {
            Evaluation ev = new Evaluation(new DvText("Clinical diagnosis"), "openEHR-EHR-EVALUATION.problem-diagnosis.v1", null,null,null,null,
                                            new CodePhrase("en", "ISO_639-1"), new CodePhrase("UTF-8", "IANA_character-sets"), new PartySelf(), null,null,null, getProtocolAt0032(),null, getDataAt0001());
            

            return ev;

        }

        private ItemStructure getDataAt0001()
        {
            ItemTree t = new ItemTree(new DvText("data"), "at0001", null, null, null, null, getAt0002_1Items());
            return t;
        }

        private Item[] getAt0002_1Items()
        {
            Item[] items = new Item[1];
            Element el0002_1 = new Element(new DvText("Diagnosis"), "at0002.1", null, null, null, null, new DvCodedText("Wound abscess", "238382001", "SNOMED"), null);
            items[0] = el0002_1;
            return items;
        }

        private ItemStructure getProtocolAt0032()
        {
            ItemTree protocol = new ItemTree(new DvText("protocol"), "at0032", null, null, null, null, null);
            return protocol;
        }
        #endregion

        #region openEHR-EHR-EVALUATION.clinical_synopsis.v1 Evaluation (clinical Synopsis)
        private ContentItem getEvaluationClinicalSynopsis()
        {
            Evaluation ev = new Evaluation(new DvText("Clinical synopsis"), "openEHR-EHR-EVALUATION.clinical_synopsis.v1", null, null, null, null,
                                            new CodePhrase("en", "ISO_639-1"), new CodePhrase("UTF-8", "IANA_character-sets"), new PartySelf(),null, null, null, null, null, 
                                            getDataClinSnopsisV1());            

            return ev;
        }

        private ItemStructure getDataClinSnopsisV1()
        {
            Element el0002 = new Element(new DvCodedText("Clinical synopsis", "at0000", "local"), "at0002", null, null, null, null, new DvText("Presented to A&amp;E on referral from Afterhours clinicwith discharging abscess in left groin, complicating a node biopsyperformed 3	days ago.The abscess was drained under local anaesthesiaand a wound swab taken. For review by GP please."), null);            
            ItemList lst = new ItemList(new DvText("data"), "at0001", null, null, null, null, new Element[]{el0002});
            return lst;
        }
        #endregion

        #region EventContext
        private EventContext GetContext()
        {
            EventContext ec = new EventContext(new OpenEhr.RM.DataTypes.Quantity.DateTime.DvDateTime("20080102T042746,0468Z"), null,
                                                new DvCodedText("primary medical care", "228", "openehr"), null, null, GetOtherContext(), null);
            return ec;
        }

        private ItemStructure GetOtherContext()
        {
            Item[] arr = getOtherContextItems();
            ItemTree tree = new ItemTree(new DvText("other context"), "at0001", null, null, null, null, arr);
            return tree;
        }

        private Item[] getOtherContextItems()
        {
            List<Item> lst = new List<Item>();

            //element at0002
            Element el = new OpenEhr.RM.DataStructures.ItemStructure.Representation.Element(new DvText("Encounter ID"), "at0002", null, null, null, null, new DvText("7bf7eab2-5e82-4cfd-8299-3d9bed082fd1"), null );            //TODO: AUTO GENERATE THIS GUID?

                //element at0005
            Element clElement = new Element(new DvText("Date/time admitted"), "at0005", null, null, null, null, new DvDateTime("20070620"), null );            
                    //element at0022
            Element el0022 = new Element(new DvText("Family name"), "at0022", null, null, null, null, new DvText("Afterhours clinic"), null);            
                //cluster at0021
            Cluster cl0021 = new Cluster(new DvText("Referring doctor"), "at0021", null, null, null, null, new Item[] { el0022 });
            

                    //element at0022
            Element el0022_2 = new Element(new DvText("Family name"), "at0022", null, null, null, null, new DvText("Dr. Urgent"),null);
            //cluster at0024
            Cluster cl0024 = new Cluster(new DvText("Admitting doctor"),"at0024", null, null, null, null, new Item[]{el0022_2} );

                    //element at0022
            Element el0022_3 = new Element(new DvText("Family name"), "at0022", null, null, null, null, new DvText("Peter Adams"), null );                        
                //cluster at0025
            Cluster cl0025 = new Cluster(new DvText("Consulting doctor"), "at0025", null, null, null, null, new Item[]{el0022_3});
            

                    //element
            Element el0009 = new Element(new DvText("Point of care/unit"), "at0009", null, null, null, null, new DvText("TRAKCARE"), null);
                    //element
            Element el0011 = new Element(new DvText("Ward"), "at0011", null, null, null, null, new DvText("Emergency"), null );
                //cluster at0008
            Cluster cl0008 = new Cluster(new DvText("Assigned patient location"), "at0008", null, null, null, null, new Item[]{el0009, el0011});
            
                //element
            Element cl_at0004_el = new Element(new DvText("Date/time discharged"), "at0013", null, null, null, null, new DvDateTime("20070620T155700"), null);
                    //element
            Element el0022_59 = new Element(new DvText("Family name"), "at0022", null, null, null, null, new DvText("Adams"), null);            
               //cluster
            Cluster cl0059 = new Cluster(new DvText("Discharge doctor"), "at0059", null, null, null, null, new Item[]{el0022_59});                        
            //cluster at0004
            Cluster cl_at0004 = new Cluster(new DvText("Discharge details"), "at0004", null, null, null, null, new Item[]{cl_at0004_el, cl0059});
            
            //cluster at0003
            Cluster cl = new Cluster(new DvText("Admission details"), "at0003", null, null, null, null, new Item[] { clElement, cl0021, cl0024, cl0025, cl0008 });
            
            lst.Add(cl);            
            lst.Add(cl_at0004);
            lst.Add(el);

            return (Item[])lst.ToArray();
        }
        #endregion

        private Archetyped GetArchetypeDetails()
        {
            
            ArchetypeId aId = new ArchetypeId("openEHR-EHR-COMPOSITION.discharge.v1");
            TemplateId tId = new TemplateId("Discharge_report");
            string rmVersion = "1.0.1";
            Archetyped details = new Archetyped(aId, rmVersion, tId);
            return details;
        }
    }
}
