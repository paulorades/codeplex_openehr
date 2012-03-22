using System;
using System.Collections.Generic;
using System.Text;
using OpenEhr.RM.Composition;
using OpenEhr.RM.Common.Generic;
using OpenEhr.RM.Support.Identification;
using OpenEhr.EhrGate.WsClient.EhrService;
using System.Xml;
using System.Configuration;
using OpenEhr.Constants;
using OpenEhr.EhrGate.WsClient.ResultSet;
using System.Data;

namespace openEHR_RM_examples
{    

    class CompositionManager
    {
        protected string userId = ConfigurationManager.AppSettings["uid"];
        protected string password = ConfigurationManager.AppSettings["pwd"];

        EhrServiceProxy service;
        protected Composition _composition;

        string sessionTicket;
        PartyIdentified committer;
        PartyRef subject;
        string ehrId;

        int numberOfCommits;

        string url = System.Configuration.ConfigurationManager.AppSettings["url"];

        public CompositionManager()
        {
        }

        public CompositionManager(Composition pComposition)
        {
            _composition = pComposition;
        }

        public Composition Composition
        {
            get
            {
                return _composition;
            }
        }

        public void SaveToDisk(string pTargetPath)
        {
            using (XmlWriter writer = XmlWriter.Create(pTargetPath))
            {
                System.Xml.Serialization.XmlSerializer serializer =
                                new System.Xml.Serialization.XmlSerializer(
                                    typeof(Composition),
                                    new System.Xml.Serialization.XmlAttributeOverrides(),
                                    new Type[] { },
                                    new System.Xml.Serialization.XmlRootAttribute("composition"),
                                   OpenEhr.Serialisation.RmXmlSerializer.OpenEhrNamespace);

                serializer.Serialize(writer, _composition);
            }
        }

        //pPath is something like: @"C:\work\data\tmp\openehr-11817\TRUNK\EhrGateWsClient\Test\EhrGate.WsClient.Tests\CompositionXml\DischargeReport.xml"
        public void LoadFromDisk(string pPath)
        {            
            using (XmlReader reader = XmlReader.Create(pPath))
            {
                System.Xml.Serialization.XmlSerializer serializer =
                                new System.Xml.Serialization.XmlSerializer(
                                    typeof(Composition),
                                    new System.Xml.Serialization.XmlAttributeOverrides(),
                                    new Type[] { },
                                    new System.Xml.Serialization.XmlRootAttribute("composition"),
                                   OpenEhr.Serialisation.RmXmlSerializer.OpenEhrNamespace);

                _composition = serializer.Deserialize(reader) as Composition;
            }
            
        }

        public void CommitToServer()
        {
            InitServerSession();
            CommitCompositionToServer();
            CloseServerSession();
        }

        private void CommitCompositionToServer()
        {
            String compositionId = service.CommitComposition(sessionTicket, ehrId, committer, null, AuditChangeType.creation, VersionLifecycleState.complete,null, _composition);
            Console.WriteLine(compositionId);
        }

        public void CommitAndQueryServer()
        {
            InitServerSession();
            CommitCompositionToServer();
            string queryStatement = @"SELECT c/name FROM EHR e[ehr_id/value='" + ehrId +
                @"'] CONTAINS Composition c[openEHR-EHR-Composition.discharge.v1]";
            ResultsTable results = service.Query(sessionTicket, queryStatement);
            foreach (DataRow dr in results.Rows)
            {
                Console.WriteLine(dr[0].ToString());
            }
            CloseServerSession();
        }

        public void CommitTDDToServer(XmlNode pRootNode)
        {
            InitServerSession();
            CommitTDD(pRootNode);
            CloseServerSession();
        }

        private void CommitTDD(XmlNode pRootNode)
        {
            String tddId = service.CommitTemplateDocument(sessionTicket, ehrId, committer, AuditChangeType.creation, VersionLifecycleState.complete, null, pRootNode);
            Console.WriteLine(tddId);
        }

        private void CloseServerSession()
        {
            service.CloseSession(sessionTicket);
        }

        private void InitServerSession()
        {
            service = new EhrServiceProxy(url);
            sessionTicket = service.LoginSession(userId, password);
            committer = new PartyIdentified("EhrServiceTests");
            subject = NewPartyRef();
            ehrId = service.Create(sessionTicket, committer, subject);
        }

        private PartyRef NewPartyRef()
        {
            PartyRef subject = new PartyRef(HierObjectId.NewObjectId(), "DEMOGRAPHICS", "PERSON");
            
            return subject;
        }
    }
}
