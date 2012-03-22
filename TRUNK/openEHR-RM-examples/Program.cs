using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using OpenEhr.RM.Composition;
using OpenEhr.AM.Archetype;
using System.Data;
using System.IO;

namespace openEHR_RM_examples
{
    class Program
    {
        
        static void Main(string[] args)
        {
            testDischargeReport();            
        }

        public static void testTDDCommit()
        {
            XmlDocument doc = new XmlDocument();
            if (File.Exists(@"c:\work\data\tmp\Discharge_reportTest.xml"))
            {
                FileStream fs = new FileStream(@"c:\work\data\tmp\Discharge_reportTest.xml", FileMode.Open);                
                doc.Load(fs);
                XmlNode node = doc.SelectSingleNode("/");
                Console.WriteLine(node.Name);
            }
            CompositionManager manager = new CompositionManager();
            manager.CommitTDDToServer(doc.SelectSingleNode("/"));            
        }
     
        public static void testDischargeReport()
        {
            Composition dischargeReport = DischargeReportCompositionFactory.GetComposition();
            CompositionManager manager = new CompositionManager(dischargeReport);
            manager.CommitToServer();
            manager.CommitAndQueryServer();
            Console.WriteLine("done");            
        }

        public static void testSaveDischargeReportToDisk()
        {
            Composition dischargeReport = DischargeReportCompositionFactory.GetComposition();
            CompositionManager manager = new CompositionManager(dischargeReport);
            manager.SaveToDisk(@"c:\work\data\tmp\DischargeReport.xml");
        }

        public static Composition LoadFromDisk(Archetype pArchetype, string pPath)
        {
            Composition ar = null;
            using (XmlReader reader = XmlReader.Create(pPath))
            {
                System.Xml.Serialization.XmlSerializer serializer =
                                new System.Xml.Serialization.XmlSerializer(
                                    typeof(Composition),
                                    new System.Xml.Serialization.XmlAttributeOverrides(),
                                    new Type[] { },
                                    new System.Xml.Serialization.XmlRootAttribute("composition"),
                                   OpenEhr.Serialisation.RmXmlSerializer.OpenEhrNamespace);

                ar = serializer.Deserialize(reader) as Composition;                
            }
            return ar;

        }        
    }
}
