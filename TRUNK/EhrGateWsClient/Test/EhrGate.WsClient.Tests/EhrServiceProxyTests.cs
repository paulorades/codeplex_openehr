using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Diagnostics;

using NUnit.Framework;

using OpenEhr.Constants;
using OpenEhr.RM.Composition;
using OpenEhr.RM.Support.Identification;
using OpenEhr.RM.Common.Generic;
using OpenEhr.RM.Ehr;
using OpenEhr.EhrGate.WsClient.EhrService;
using OpenEhr.EhrGate.WsClient.ResultSet;

namespace OpenEhr.EhrGate.WsClient.Tests
{
    [TestFixture]
    public class EhrServiceProxyTests
    {
        protected string userId = ConfigurationManager.AppSettings["uid"];
        protected string password = ConfigurationManager.AppSettings["pwd"];

        EhrServiceProxy service;
        protected Composition labReport;      

        string sessionTicket;
        PartyIdentified committer;
        PartyRef subject;
        string ehrId;
       
        int numberOfCommits;

        string url = ConfigurationManager.AppSettings["url"];

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            service = new EhrServiceProxy(url);
            sessionTicket = service.LoginSession(userId, password);
            committer = new PartyIdentified("EhrServiceTests");
            subject = NewPartyRef();
            ehrId = service.Create(sessionTicket, committer, subject);

            using (XmlReader reader = XmlReader.Create(@"CompositionXml\DischargeReport.xml"))
            {
                System.Xml.Serialization.XmlSerializer serializer =    
                                new System.Xml.Serialization.XmlSerializer(
                                    typeof(Composition),
                                    new System.Xml.Serialization.XmlAttributeOverrides(),
                                    new Type[] { },
                                    new System.Xml.Serialization.XmlRootAttribute("composition"),
                                   OpenEhr.Serialisation.RmXmlSerializer.OpenEhrNamespace);

                labReport = serializer.Deserialize(reader) as Composition;
            }

            numberOfCommits = 5;
            PopulateData(numberOfCommits);
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            service.CloseSession(sessionTicket);
        }

        void PopulateData(int numberOfCommits)
        {
            long totalTime = 0;
            for (int i = 0; i < numberOfCommits; i++)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                service.CommitComposition(sessionTicket, ehrId, committer, null, AuditChangeType.creation, VersionLifecycleState.complete,
                    null, labReport);
                sw.Stop();
                totalTime += sw.ElapsedMilliseconds;

                Console.WriteLine("Commit: "+sw.ElapsedMilliseconds+" ms");

            }
            Console.WriteLine("Average commit: "+totalTime/numberOfCommits+" ms");
        }

        private PartyRef NewPartyRef()
        {
            PartyRef subject = new PartyRef(HierObjectId.NewObjectId(), "DEMOGRAPHICS", "PERSON");

            return subject;
        }

        #region session management
        [Test]
        public void T01_LoginSession()
        {
            //string authenticationMessage;
            string sessionTicket1 = service.LoginSession(userId, password);//, out authenticationMessage);

            Assert.IsNotNullOrEmpty(sessionTicket1);
        }

        [Test]
        public void T01_IsOpenSession()
        {
            //string authenticationMessage;
            string sessionTicket1 = service.LoginSession(userId, password);//, out authenticationMessage);
            bool sessionOpen = service.IsOpenSession(sessionTicket1);
            Assert.IsTrue(sessionOpen);
        }

        [Test]
        public void T01_CloseSession()
        {
            //string authenticationMessage;
            string sessionTicket1 = service.LoginSession(userId, password);//, out authenticationMessage);
            bool sessionOpen = service.IsOpenSession(sessionTicket1);
            Assert.IsTrue(sessionOpen);

            service.CloseSession(sessionTicket1);
            sessionOpen = service.IsOpenSession(sessionTicket1);
            Assert.IsFalse(sessionOpen);
        }

        [Test]
        public void T01_AuthenticateUser()
        {
            string authenticationToken = service.AuthenticateUser(userId, password);
            Assert.IsTrue(!string.IsNullOrEmpty(authenticationToken));

        }

        [Test]
        public void T01_OpenSession()
        {
            string authenticationToken = service.AuthenticateUser(userId, password);
            string sessionTicket_1 = service.OpenSession(userId, authenticationToken);
            Assert.IsTrue(!string.IsNullOrEmpty(sessionTicket_1));
        }

        [Test]
        public void T01_LoginSession_CanReAccessSession()
        {
            //string authenticationMessage;
            string sessionTicket1 = service.LoginSession(userId, password);//, out authenticationMessage);
            string sessionTicket2=service.LoginSession(userId, password);
            Assert.IsTrue(sessionTicket1 != sessionTicket2);

            PartyRef newSubj = NewPartyRef();
            string ehrId1 = service.Create(sessionTicket1, committer, newSubj);
            Assert.IsNotNullOrEmpty(ehrId1);

        }

        [Test]
        public void T01_LoginSession_CanAccessSameSessionFromNewInstance()
        {
            //string authenticationMessage;
            string sessionTicket1 = service.LoginSession(userId, password);//, out authenticationMessage);
            EhrServiceProxy newService = new EhrServiceProxy(url);
           
            PartyRef newSubj = NewPartyRef();
            string ehrId1 = newService.Create(sessionTicket1, committer, newSubj);
            Assert.IsNotNullOrEmpty(ehrId1);

        }
        #endregion

        #region record management
        [Test]
        public void T02_Create()
        {
            PartyRef newSubj = NewPartyRef();
            string ehrId1 =  service.Create(sessionTicket, committer, newSubj);
            Assert.IsNotNullOrEmpty(ehrId1);
        }

        [Test]
        public void T02_Find()
        {           
            PartyRef newSubj = NewPartyRef();
            string ehrId = service.Create(sessionTicket, committer, newSubj);
            string ehrId1 = service.Find(sessionTicket, newSubj);
            Assert.AreEqual(ehrId, ehrId1);
        }

        [Test]
        public void T02_GetSubjectId()
        {
            PartyRef newSubj = NewPartyRef();
            string ehrId1 = service.Create(sessionTicket, committer, newSubj);
            PartyRef subject = service.GetSubject(sessionTicket, ehrId1);

            Assert.AreEqual(subject.Id.Value, newSubj.Id.Value);
            Assert.AreEqual(subject.Namespace, newSubj.Namespace);
            Assert.AreEqual(subject.Type, newSubj.Type);
        }

        [Test]
        public void T02_FindEhr()
        {  
            PartyRef newSubj = NewPartyRef();
            string ehrId = service.Create(sessionTicket, committer, newSubj);
            string ehrId1 = service.Find(sessionTicket, newSubj);
            Assert.AreEqual(ehrId, ehrId1);
        }
        #endregion

        [Test]
        public void ModifyEhrStatus_ModifySubjectID()
        {
            PartyIdentified committer = new PartyIdentified();
            committer.Name = "EhrServiceTests";

            // Create ehrA with subjectIdA
            string subjectIdA = Guid.NewGuid().ToString();
            string subjectNamespace = "local";

            PartyRef subjectA = new PartyRef(new HierObjectId(subjectIdA), subjectNamespace, "PERSON");

            string ehrA = service.Create(sessionTicket, committer, subjectA);


            // modify ehrA with subjectIdB
            string subjectIdB = Guid.NewGuid().ToString();
            PartyRef subjectB = new PartyRef(new HierObjectId(subjectIdB), subjectNamespace,"PERSON");

            EhrStatus ehrStatus = service.GetEhrStatus(sessionTicket, ehrA);

            ehrStatus.Subject = new PartySelf(subjectB);

            service.CommitEhrStatus(sessionTicket, ehrA, committer, AuditChangeType.modification, ehrStatus);

            // find using subjectIdA
            string ehrId = service.Find(sessionTicket, subjectA);
            Assert.AreEqual(ehrA, ehrId);

            ehrId = service.Find(sessionTicket, subjectB);
            Assert.AreEqual(ehrA, ehrId);
        }

        [Test]
        public void ModifyEhrStatus_AmendSubjectID()
        {
            PartyIdentified committer = new PartyIdentified();
            committer.Name = "EhrServiceTests";

            // Create ehrA with subjectIdA
            string subjectIdA = Guid.NewGuid().ToString();
            string subjectNamespace = "local";

            PartyRef subjectA = new PartyRef(new HierObjectId(subjectIdA), subjectNamespace, "PERSON");

            string ehrA = service.Create(sessionTicket, committer, subjectA);


            // modify ehrA with subjectIdB
            string subjectIdB = Guid.NewGuid().ToString();
            PartyRef subjectB = new PartyRef(new HierObjectId(subjectIdB), subjectNamespace, "PERSON");

            EhrStatus ehrStatus = service.GetEhrStatus(sessionTicket, ehrA);

            ehrStatus.Subject = new PartySelf(subjectB);

            service.CommitEhrStatus(sessionTicket, ehrA, committer, AuditChangeType.amendment, ehrStatus);

            // find using subjectIdA
            string ehrId = service.Find(sessionTicket, subjectA);
            Assert.IsNullOrEmpty(ehrId);

            ehrId = service.Find(sessionTicket, subjectB);
            Assert.AreEqual(ehrA, ehrId);
        }

        [Test]
        public void CommitContribution_EhrStatus_AmendSubjectID()
        {
            PartyIdentified committer = new PartyIdentified();
            committer.Name = "EhrServiceTests";

            // Create ehrA with subjectIdA
            string subjectIdA = Guid.NewGuid().ToString();
            string subjectNamespace = "local";

            PartyRef subjectA = new PartyRef(new HierObjectId(subjectIdA), subjectNamespace, "PERSON");

            string ehrA = service.Create(sessionTicket, committer, subjectA);


            // modify ehrA with subjectIdB
            string subjectIdB = Guid.NewGuid().ToString();
            PartyRef subjectB = new PartyRef(new HierObjectId(subjectIdB),subjectNamespace,"PERSON");

            EhrStatus ehrStatus = service.GetEhrStatus(sessionTicket, ehrA);

            ehrStatus.Subject = new PartySelf(subjectB);

            ContributionVersion ehrStatusVersion = new ContributionVersion();
            ehrStatusVersion.changeType = AuditChangeType.amendment;
            ehrStatusVersion.Item = ehrStatus;
            ehrStatusVersion.lifecycleState = VersionLifecycleState.complete;
            ehrStatusVersion.precedingVersionUid = ehrStatus.Uid.Value;

            service.CommitContribution(sessionTicket, ehrA, committer, new ContributionVersion[] { ehrStatusVersion });

            // find using subjectIdA
            string ehrId = service.Find(sessionTicket, subjectA);
            Assert.IsNullOrEmpty(ehrId);

            ehrId = service.Find(sessionTicket, subjectB);
            Assert.AreEqual(ehrA, ehrId);
        }

        [Test]
        [ExpectedException()]
        public void ModifyEhrStatus_DeleteEHR()
        {
            PartyIdentified committer = new PartyIdentified();
            committer.Name = "EhrServiceTests";

            // Create ehrA with subjectIdA
            string subjectIdA = Guid.NewGuid().ToString();
            string subjectNamespace = "local";

            PartyRef subjectA = new PartyRef(new HierObjectId(subjectIdA), subjectNamespace, "PERSON");

            string ehrA = service.Create(sessionTicket, committer, subjectA);

            service.CommitEhrStatus(sessionTicket, ehrA, committer, AuditChangeType.deleted, null);

            //// find using subjectIdA
            //string ehrId = service.Find(sessionTicket, subjectA);
            //Assert.IsNullOrEmpty(ehrId);
        }

        [Test]
        public void DeleteEHR()
        {
            PartyIdentified committer = new PartyIdentified();
            committer.Name = "EhrServiceTests";

            // Create ehrA with subjectIdA
            string subjectIdA = Guid.NewGuid().ToString();
            string subjectNamespace = "local";

            PartyRef subjectA = new PartyRef(new HierObjectId(subjectIdA), subjectNamespace, "PERSON");

            string ehrA = service.Create(sessionTicket, committer, subjectA);

            service.DeleteEhr(sessionTicket, ehrA, committer);

            // find using subjectIdA
            string ehrId = service.Find(sessionTicket, subjectA);
            Assert.IsNullOrEmpty(ehrId);
        }

        [Test]
        public void AmendEhrStatus_SwitchSubjectID()
        {
            string subjectIdA = Guid.NewGuid().ToString();
            string subjectNamespace = "local";

            PartyRef subjectA = new PartyRef(new HierObjectId(subjectIdA), subjectNamespace, "PERSON");

            string ehrIdA = service.Create(sessionTicket, committer, subjectA);

            string subjectIdB = Guid.NewGuid().ToString();
            PartyRef subjectB = new PartyRef(new HierObjectId(subjectIdB), subjectNamespace, "PERSON");

            string ehrIdB = service.Create(sessionTicket, committer, subjectB);

            // change ehrA to subjectB
            //service.OpenAndUseEhr(sessionTicket, ehrIdA);
            //EhrStatus ehrStatus = service.GetEhrStatus(sessionTicket);
            EhrStatus ehrStatus = service.GetEhrStatus(sessionTicket, ehrIdA);
            //ehrStatus.Subject.ExternalRef.Id.Value = subjectIdB;
            ehrStatus.Subject = new PartySelf(subjectB);
            //service.CreateContribution(sessionTicket);
            //service.AmendEhrStatus(sessionTicket, ehrStatus);
            //service.CommitContribution(sessionTicket, committer, null, AuditChangeType.amendment);
            //service.CloseEhr(sessionTicket, ehrIdA);
            service.CommitEhrStatus(sessionTicket, ehrIdA, committer, AuditChangeType.amendment, ehrStatus);

            // change ehrB to subjectA
            //service.OpenAndUseEhr(sessionTicket, ehrIdB);
            //ehrStatus = service.GetEhrStatus(sessionTicket);
            ehrStatus = service.GetEhrStatus(sessionTicket, ehrIdB);
            //ehrStatus.Subject.ExternalRef.Id.Value = subjectIdA;
            ehrStatus.Subject = new PartySelf(subjectA);
            //service.CreateContribution(sessionTicket);
            //service.AmendEhrStatus(sessionTicket, ehrStatus);
            //service.CommitContribution(sessionTicket, committer, null, AuditChangeType.amendment);
            //service.CloseEhr(sessionTicket, ehrIdB);
            service.CommitEhrStatus(sessionTicket, ehrIdB, committer, AuditChangeType.amendment, ehrStatus);

            //string ehrId = service.FindEhr(sessionTicket, subjectIdA, subjNs);
            string ehrId = service.Find(sessionTicket, subjectA);
            Assert.AreEqual(ehrIdB, ehrId);

            //ehrId = service.FindEhr(sessionTicket, subjectIdB, subjNs);
            ehrId = service.Find(sessionTicket, subjectB);
            Assert.AreEqual(ehrIdA, ehrId);
        }

        [Test]
        public void FindEhr_MultipleSameSubjectId()
        {
            string subjectIdA = Guid.NewGuid().ToString();
            string subjectNamespace = "local";

            PartyRef subjectA = new PartyRef(new HierObjectId(subjectIdA), subjectNamespace, "PERSON");

            string ehrIdA = service.Create(sessionTicket, committer, subjectA);

            string ehrIdB = service.Create(sessionTicket, committer, subjectA);

            // this should raise exception
            string ehrId = service.Find(sessionTicket, subjectA);
            Assert.AreEqual(ehrIdB, ehrId);
        }

        [Test]
        public void FindEHRs_MultipleSameSubjectId()
        {
            string subjectIdA = Guid.NewGuid().ToString();
            string subjectNamespace = "local";

            PartyRef subjectA = new PartyRef(new HierObjectId(subjectIdA), subjectNamespace, "PERSON");

            string ehrIdA = service.Create(sessionTicket, committer, subjectA);

            string ehrIdB = service.Create(sessionTicket, committer, subjectA);

            string[] ehrIds = service.FindEHRs(sessionTicket, subjectA);
            Assert.IsNotNull(ehrIds);
            Assert.AreEqual(2, ehrIds.Length);
            Assert.Contains(ehrIdA, ehrIds);
            Assert.Contains(ehrIdB, ehrIds);
        }

        [Test]
        public void FindEHRs_MergedEHR()
        {
            string subjectIdA = Guid.NewGuid().ToString();
            string subjectNamespace = "local";

            PartyRef subjectA = new PartyRef(new HierObjectId(subjectIdA), subjectNamespace, "PERSON");

            string ehrIdA = service.Create(sessionTicket, committer, subjectA);

            string subjectIdB = Guid.NewGuid().ToString();
            PartyRef subjectB = new PartyRef(new HierObjectId(subjectIdB), subjectNamespace, "PERSON");

            string ehrIdB = service.Create(sessionTicket, committer, subjectB);

            // merge ehrA to subjectB ( can find ehrA using subjectB or subjectA, also find ehrB with subjectB)
            //service.OpenAndUseEhr(sessionTicket, ehrIdA);
            //EhrStatus ehrStatus = service.GetEhrStatus(sessionTicket);
            EhrStatus ehrStatus = service.GetEhrStatus(sessionTicket, ehrIdA);
            //ehrStatus.Subject.ExternalRef.Id.Value = subjectIdB;
            ehrStatus.Subject = new PartySelf(subjectB);
            //service.CreateContribution(sessionTicket);
            //service.ModifyEhrStatus(sessionTicket, ehrStatus);
            //service.CommitContribution(sessionTicket, committer, null, AuditChangeType.amendment);
            //service.CloseEhr(sessionTicket, ehrIdA);
            service.CommitEhrStatus(sessionTicket, ehrIdA, committer, AuditChangeType.modification, ehrStatus);

            string ehrId = service.Find(sessionTicket, subjectA);
            Assert.AreEqual(ehrIdA, ehrId);

            string[] ehrIds = service.FindEHRs(sessionTicket, subjectA);
            Assert.IsNotNull(ehrIds);
            Assert.AreEqual(1, ehrIds.Length);
            Assert.Contains(ehrIdA, ehrIds);

            ehrIds = service.FindEHRs(sessionTicket, subjectB);
            Assert.IsNotNull(ehrIds);
            Assert.AreEqual(2, ehrIds.Length);
            Assert.Contains(ehrIdA, ehrIds);
            Assert.Contains(ehrIdB, ehrIds);
        }

        #region commit
        [Test]
        public void T03_CommitContribution_NewComposition()
        {
            PartyRef newSubj = NewPartyRef();
            string ehrId1 = service.Create(sessionTicket, committer, newSubj);

            ContributionVersion contributionVersion = new ContributionVersion();
            contributionVersion.changeType = AuditChangeType.creation;
            //contributionVersion.data = labReport;
            contributionVersion.lifecycleState = VersionLifecycleState.complete;
            contributionVersion.precedingVersionUid = null;

            contributionVersion.Item = labReport;

            List<ContributionVersion> contributionVersions = new List<ContributionVersion>();
            contributionVersions.Add(contributionVersion);
            string[] compositionUids = service.CommitContribution
                (sessionTicket, ehrId1, committer, contributionVersions.ToArray());

            Assert.IsTrue(compositionUids.Length == 1);

            Composition c = service.GetComposition(sessionTicket, compositionUids[0]);

            Assert.IsNotNull(c);
        }

        [Test]
        public void T03_CommitComposition_New()
        {
            PartyRef newSubj = NewPartyRef();
            string ehrId1 = service.Create(sessionTicket, committer, newSubj);

            string compositionUid = service.CommitComposition(sessionTicket, ehrId1, committer, null, AuditChangeType.creation, VersionLifecycleState.complete,
                null, labReport);

            Assert.IsTrue(!string.IsNullOrEmpty(compositionUid));
        }

        [Test]
        public void T03_CommitComposition_Modify()
        {
            PartyRef newSubj = NewPartyRef();
            string ehrId1 = service.Create(sessionTicket, committer, newSubj);

            string compositionUid = service.CommitComposition(sessionTicket, ehrId1, committer, null, AuditChangeType.creation, VersionLifecycleState.complete,
                null, labReport);

            Composition retrievedComposition = service.GetComposition(sessionTicket, compositionUid);
            string uid2= service.CommitComposition(sessionTicket, ehrId1, committer, null,
                 AuditChangeType.modification, VersionLifecycleState.complete,
                 retrievedComposition.Uid.Value, labReport);
            retrievedComposition = service.GetComposition(sessionTicket, compositionUid);

            Assert.IsTrue(compositionUid == uid2);
           
            Assert.IsTrue( retrievedComposition.Uid.Value.EndsWith("2"));
        }

        [Test]
        public void T03_CommitComposition_with_comments()
        {
            PartyRef newSubj = NewPartyRef();
            string ehrId1 = service.Create(sessionTicket, committer, newSubj);
            string compositionUid = service.CommitComposition(sessionTicket, ehrId1, committer, "T04_CommitComposition_with_comments", AuditChangeType.creation, VersionLifecycleState.complete,
                 null, labReport);

            Assert.IsTrue(!string.IsNullOrEmpty(compositionUid));
        }
        #endregion

        #region query
        [Test]
        public void T04_QueryEhrContent()
        {           
            string queryStatement = @"SELECT c/name FROM EHR e[ehr_id/value='"+ehrId+
                @"'] CONTAINS Composition c[openEHR-EHR-Composition.discharge.v1]";
            ResultsTable results = service.Query(sessionTicket, queryStatement);

            Assert.AreEqual(numberOfCommits, results.Rows.Count);
        }

        [Test]
        public void T04_GetCandidateCompositions()
        {
            string queryStatement = @"SELECT c/name FROM EHR e[ehr_id/value='" + ehrId +
                @"'] CONTAINS Composition c[openEHR-EHR-Composition.discharge.v1]";
            ResultsTable results = service.FindCompositions(sessionTicket, ehrId,
                new string[] { "name/value" }, "openEHR-EHR-Composition.discharge.v1", 
                new string[] { "context/other_context/items[at0002]/value/value='7bf7eab2-5e82-4cfd-8299-3d9bed082fd1'"});

            Assert.AreEqual(numberOfCommits, results.Rows.Count);
        }

        [Test]
        public void T04_FindCompositions()
        {
            string queryStatement = @"SELECT c/name FROM EHR e[ehr_id/value='" + ehrId +
                @"'] CONTAINS Composition c[openEHR-EHR-Composition.discharge.v1]";
            ResultsTable results = service.FindCompositions(sessionTicket, ehrId,
                new string[] { "name/value" }, "openEHR-EHR-Composition.discharge.v1", 
                new string[]{"context/other_context/items[at0002]/value/value='7bf7eab2-5e82-4cfd-8299-3d9bed082fd1'"});

            Assert.AreEqual(numberOfCommits, results.Rows.Count);
        }

        [Test]
        public void T04_GetComposition()
        {
            PartyRef subject = NewPartyRef();
            string ehrId1 = service.Create(sessionTicket, committer, subject);
            string compositionObjectId = service.CommitComposition(sessionTicket, ehrId1, committer,
                null, AuditChangeType.creation, VersionLifecycleState.complete, null, labReport);

            Composition c = service.GetComposition(sessionTicket, compositionObjectId);
            Assert.IsNotNull(c);
            
        }

        [Test]
        public void T04_GetCompositions()
        {
            string queryStatement = @"SELECT c/name FROM EHR e[ehr_id/value='" + ehrId +
                @"'] CONTAINS Composition c[openEHR-EHR-Composition.discharge.v1]";
            Composition[] results = service.GetCompositions(sessionTicket, ehrId,
                 "openEHR-EHR-Composition.discharge.v1",
                new string[] { "context/other_context/items[at0002]/value/value='7bf7eab2-5e82-4cfd-8299-3d9bed082fd1'" });

            Assert.AreEqual(numberOfCommits, results.Length);
        }

        [Test]
        public void T04_QueryCompositions()
        {
            string queryStatement = @"SELECT c FROM EHR e[ehr_id/value='" + ehrId +
                @"'] CONTAINS Composition c[openEHR-EHR-Composition.discharge.v1]";
            Composition[] results = service.QueryCompositions(sessionTicket, queryStatement);
                
            Assert.AreEqual(numberOfCommits, results.Length);
        }


        [Test]
        public void T04_RunQuery_RetrieveResults()
        {
            //string queryId = "T04_RunQuery_RetrieveResults";
            string queryStatement = @"SELECT c/name FROM EHR e[ehr_id/value='" + ehrId +
                @"'] CONTAINS Composition c[openEHR-EHR-Composition.discharge.v1]";
            string queryId = service.RunQuery(sessionTicket, queryStatement);

            for (int i = 0; i < numberOfCommits; i++)
            {
                ResultsTable results = service.GetResults(sessionTicket, queryId, 1);
                Assert.AreEqual(1, results.Rows.Count);
            }

            ResultsTable rs = service.GetResults(sessionTicket, queryId, 0);
            Assert.IsTrue(rs.Rows.Count ==0);
            //bool hasResults = service.HasQueryResults(sessionTicket, queryId);
            //Assert.IsFalse(hasResults);
            rs = service.GetResults(sessionTicket, queryId, 0);
            Assert.IsNull(rs);
        }

        [Test]
        public void T04_QueryCompositions_ExistingQueryStrategy()
        {
            string queryStatement = @"SELECT c FROM EHR e[ehr_id/value='" + ehrId +
                @"'] CONTAINS VERSION v CONTAINS Composition c[openEHR-EHR-Composition.discharge.v1]";
            Composition[] results = service.QueryCompositions(sessionTicket, queryStatement);

            Assert.AreEqual(numberOfCommits, results.Length);
        }

        [Test]
        public void T04_RunEhrQuery_RetrieveResults_FromNewService()
        {
            string queryStatement = @"SELECT c/name FROM EHR e[ehr_id/value='" + ehrId +
                @"'] CONTAINS Composition c[openEHR-EHR-Composition.discharge.v1]";
            string queryId = service.RunQuery(sessionTicket, queryStatement);

            ResultsTable results = service.GetResults(sessionTicket, queryId, 1);
            EhrServiceProxy newService = new EhrServiceProxy(url);
            for (int i = 1; i < numberOfCommits; i++)
            {
                results = newService.GetResults(sessionTicket, queryId, 1);
                Assert.AreEqual(1, results.Rows.Count);
            }

            ResultsTable rs = newService.GetResults(sessionTicket, queryId, 0);
            Assert.IsTrue(rs.Rows.Count == 0);
            //bool hasResults = service.HasQueryResults(sessionTicket, queryId);
            //Assert.IsFalse(hasResults);
            rs = newService.GetResults(sessionTicket, queryId, 0);
            Assert.IsNull(rs);
        }

        [Test]
        public void T04_BeginQuery_GetResults_FromNewService()
        {
            string queryStatement = @"SELECT c/name FROM EHR e CONTAINS Composition c[openEHR-EHR-Composition.discharge.v1] WHERE c/context/start_time/value>'20100316'";

            ResultsTable rs = service.Query(sessionTicket, queryStatement);
            int expectedNumberOfResults = rs.Rows.Count;

            string queryId = service.RunQuery(sessionTicket, queryStatement);

            ResultsTable results = service.GetResults(sessionTicket, queryId, 1);
            EhrServiceProxy newService = new EhrServiceProxy(url);
            int n = 1;
            int remaining = 0;
            while (results != null && results.Rows.Count > 0)
            {
                results = newService.GetResults(sessionTicket, queryId, 10);
                if (results.Rows.Count > 0)
                {
                    n = n + results.Rows.Count;

                    Assert.IsTrue(results.Rows.Count == 10 || results.Rows.Count == remaining);
                    remaining = expectedNumberOfResults - n;
                }
            }

            Assert.IsTrue(expectedNumberOfResults == n);
            Assert.IsTrue(remaining == 0);
        }


        [Test]
        public void T04_BeginQuery_GetResults()
        {
           // string queryId = "T05_GetCandidateCompositions";
            string queryStatement = @"SELECT c/name FROM EHR e[ehr_id/value='" + ehrId +
                @"'] CONTAINS Composition c[openEHR-EHR-Composition.discharge.v1]";
           string queryId = service.RunQuery(sessionTicket, queryStatement);

            for (int i = 0; i < numberOfCommits; i++)
            {
                ResultsTable results = service.GetResults(sessionTicket, queryId, 1);
                Assert.AreEqual(1, results.Rows.Count);
            }

            ResultsTable rs = service.GetResults(sessionTicket, queryId, 0);
            Assert.IsTrue(rs.Rows.Count == 0);
            //bool hasResults = service.HasQueryResults(sessionTicket, queryId);
            //Assert.IsFalse(hasResults);
            rs = service.GetResults(sessionTicket, queryId, 0);
            Assert.IsNull(rs);
        }


        [Test]
        public void T04_ReleaseResults()
        {
            //string queryId = "T05_GetCandidateCompositions";
            string queryStatement = @"SELECT c/name FROM EHR e[ehr_id/value='" + ehrId +
                @"'] CONTAINS Composition c[openEHR-EHR-Composition.discharge.v1]";
            string queryId = service.RunQuery(sessionTicket, queryStatement);

            ResultsTable results = service.GetResults(sessionTicket, queryId, 1);
            Assert.AreEqual(1, results.Rows.Count);

            //bool hasResults = service.HasQueryResults(sessionTicket, queryId);
            //Assert.IsTrue(hasResults);

            service.ReleaseResults(sessionTicket, queryId);
            //hasResults = service.HasQueryResults(sessionTicket, queryId);
            //Assert.IsFalse(hasResults);
            results = service.GetResults(sessionTicket, queryId, 1);
            Assert.IsNull(results);
        }
        #endregion
        //[Test]
        //public void T02_Login()
        //{
        //    string authenticationMessage;
        //    string sessionTicket = service.Login(userId, password, out authenticationMessage);

        //    Assert.IsNotNullOrEmpty(sessionTicket);

        //    string sessionTicket = service.CommitComposition(userId, password, out authenticationMessage);

        //}

    }
}
