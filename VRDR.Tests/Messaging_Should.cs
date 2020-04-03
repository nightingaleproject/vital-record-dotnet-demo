using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using Xunit;

namespace VRDR.Tests
{
    public class Messaging_Should
    {
        private ArrayList XMLRecords;

        private ArrayList JSONRecords;

        public Messaging_Should()
        {
            XMLRecords = new ArrayList();
            XMLRecords.Add(new DeathRecord(File.ReadAllText(FixturePath("fixtures/xml/1.xml"))));
            JSONRecords = new ArrayList();
            JSONRecords.Add(new DeathRecord(File.ReadAllText(FixturePath("fixtures/json/1.json"))));
        }

        [Fact]
        public void CreateSubmission()
        {
            DeathRecordSubmission submission = new DeathRecordSubmission();
            Assert.Equal("vrdr_submission", submission.MessageType);
        }

        [Fact]
        public void CreateSubmissionFromDeathRecord()
        {
            DeathRecordSubmission submission = new DeathRecordSubmission((DeathRecord)XMLRecords[0]);
            Assert.Equal("2018-02-20T16:48:06-05:00", submission.MessagePayload.DateOfDeathPronouncement);
            Assert.Equal("vrdr_submission", submission.MessageType);
        }

        [Fact]
        public void CreateSubmissionFromJSON()
        {
            DeathRecordSubmission submission = new DeathRecordSubmission(File.ReadAllText(FixturePath("fixtures/json/DeathRecordSubmission.json")));
            Assert.Equal("2018-02-20T16:48:06-05:00", submission.MessagePayload.DateOfDeathPronouncement);
            Assert.Equal("vrdr_submission", submission.MessageType);
        }

        [Fact]
        public void CreateUpdate()
        {
            DeathRecordUpdate submission = new DeathRecordUpdate();
            Assert.Equal("vrdr_submission_update", submission.MessageType);
        }

        [Fact]
        public void CreateUpdateFromDeathRecord()
        {
            DeathRecordUpdate submission = new DeathRecordUpdate((DeathRecord)XMLRecords[0]);
            Assert.Equal("2018-02-20T16:48:06-05:00", submission.MessagePayload.DateOfDeathPronouncement);
            Assert.Equal("vrdr_submission_update", submission.MessageType);
        }

        [Fact]
        public void CreateUpdateFromJSON()
        {
            DeathRecordUpdate submission = new DeathRecordUpdate(File.ReadAllText(FixturePath("fixtures/json/DeathRecordUpdate.json")));
            Assert.Equal("2018-02-20T16:48:06-05:00", submission.MessagePayload.DateOfDeathPronouncement);
            Assert.Equal("vrdr_submission_update", submission.MessageType);
        }

        [Fact]
        public void CreateAckForMessage()
        {
            DeathRecordSubmission submission = new DeathRecordSubmission(File.ReadAllText(FixturePath("fixtures/json/DeathRecordSubmission.json")));
            AckMessage ack = new AckMessage(submission);
            Assert.Equal("vrdr_acknowledgement", ack.MessageType);
            Assert.Equal(submission.MessageId, ack.AckedMessageId);
            Assert.Equal(submission.MessageSource, ack.MessageDestination);
        }

        [Fact]
        public void CreateAckFromJSON()
        {
            AckMessage ack = new AckMessage(File.ReadAllText(FixturePath("fixtures/json/AckMessage.json")));
            Assert.Equal("vrdr_acknowledgement", ack.MessageType);
            Assert.Equal("a9d66d2e-2480-4e8d-bab3-4e4c761da1b7", ack.AckedMessageId);
            Assert.Equal("nightingale", ack.MessageDestination);
        }

        [Fact]
        public void CreateAckFromXML()
        {
            AckMessage ack = new AckMessage(File.ReadAllText(FixturePath("fixtures/xml/AckMessage.xml")));
            Assert.Equal("vrdr_acknowledgement", ack.MessageType);
            Assert.Equal("a9d66d2e-2480-4e8d-bab3-4e4c761da1b7", ack.AckedMessageId);
            Assert.Equal("nightingale", ack.MessageDestination);
        }

        [Fact]
        public void CreateCodingResponseFromJSON()
        {
            CodingResponseMessage message = new CodingResponseMessage(File.ReadAllText(FixturePath("fixtures/json/CauseOfDeathCodingMessage.json")), false);
            Assert.Equal("vrdr_coding", message.MessageType);
            Assert.Equal("destination", message.MessageDestination);
            Assert.Equal("cert101010", message.CertificateNumber);
            Assert.Equal("id101010", message.StateIdentifier);
            var ethnicity = message.Ethnicity;
            Assert.Equal(2, ethnicity.Count);
            Assert.True(ethnicity.ContainsKey(CodingResponseMessage.HispanicOrigin.DETHNICE));
            Assert.Equal("123", ethnicity.GetValueOrDefault(CodingResponseMessage.HispanicOrigin.DETHNICE, "foo"));
            Assert.True(ethnicity.ContainsKey(CodingResponseMessage.HispanicOrigin.DETHNIC5C));
            Assert.Equal("456", ethnicity.GetValueOrDefault(CodingResponseMessage.HispanicOrigin.DETHNIC5C, "foo"));
            var race = message.Race;
            Assert.Equal(3, race.Count);
            Assert.True(race.ContainsKey(CodingResponseMessage.RaceCode.RACE1E));
            Assert.Equal("foo", race.GetValueOrDefault(CodingResponseMessage.RaceCode.RACE1E,"yyz"));
            Assert.True(race.ContainsKey(CodingResponseMessage.RaceCode.RACE17C));
            Assert.Equal("bar", race.GetValueOrDefault(CodingResponseMessage.RaceCode.RACE17C,"yyz"));
            Assert.True(race.ContainsKey(CodingResponseMessage.RaceCode.RACEBRG));
            Assert.Equal("baz", race.GetValueOrDefault(CodingResponseMessage.RaceCode.RACEBRG,"yyz"));
            Assert.Equal("A04.7", message.UnderlyingCauseOfDeath);
            var recordAxisCodes = message.CauseOfDeathRecordAxis;
            Assert.Equal(4, recordAxisCodes.Count);
            Assert.Equal("A04.7", recordAxisCodes[0]);
            Assert.Equal("A41.9", recordAxisCodes[1]);
            Assert.Equal("J18.9", recordAxisCodes[2]);
            Assert.Equal("J96.0", recordAxisCodes[3]);
            var entityAxisEntries = message.CauseOfDeathEntityAxis;
            Assert.Equal(2, (int)entityAxisEntries.Count);
            Assert.Equal("DEATH CERT LINE 1 TEXT", entityAxisEntries[0].DeathCertificateText);
            Assert.Equal("abcde", entityAxisEntries[0].CauseOfDeathConditionId);
            Assert.Equal(2, (int)entityAxisEntries[0].AssignedCodes.Count);
            Assert.Equal("code1_1", entityAxisEntries[0].AssignedCodes[0]);
            Assert.Equal("code1_2", entityAxisEntries[0].AssignedCodes[1]);
            Assert.Equal("DEATH CERT LINE 2 TEXT", entityAxisEntries[1].DeathCertificateText);
            Assert.Equal("xyzzy", entityAxisEntries[1].CauseOfDeathConditionId);
            Assert.Equal(1, (int)entityAxisEntries[1].AssignedCodes.Count);
            Assert.Equal("code2_1", entityAxisEntries[1].AssignedCodes[0]);
        }

        [Fact]
        public void CreateCodingResponse()
        {
            CodingResponseMessage message = new CodingResponseMessage("destination", "http://nchs.cdc.gov/vrdr_submission");
            Assert.Equal("vrdr_coding", message.MessageType);
            Assert.Equal("destination", message.MessageDestination);
            
            Assert.Null(message.CertificateNumber);
            message.CertificateNumber = "cert101010";
            Assert.Equal("cert101010", message.CertificateNumber);

            Assert.Null(message.StateIdentifier);
            message.StateIdentifier = "id101010";
            Assert.Equal("id101010", message.StateIdentifier);
            
            Assert.Empty(message.Ethnicity);
            var ethnicity = new Dictionary<CodingResponseMessage.HispanicOrigin, string>();
            ethnicity.Add(CodingResponseMessage.HispanicOrigin.DETHNICE, "123");
            ethnicity.Add(CodingResponseMessage.HispanicOrigin.DETHNIC5C, "456");
            message.Ethnicity = ethnicity;
            ethnicity = message.Ethnicity;
            Assert.Equal(2, ethnicity.Count);
            Assert.True(ethnicity.ContainsKey(CodingResponseMessage.HispanicOrigin.DETHNICE));
            Assert.Equal("123", ethnicity.GetValueOrDefault(CodingResponseMessage.HispanicOrigin.DETHNICE, "foo"));
            Assert.True(ethnicity.ContainsKey(CodingResponseMessage.HispanicOrigin.DETHNIC5C));
            Assert.Equal("456", ethnicity.GetValueOrDefault(CodingResponseMessage.HispanicOrigin.DETHNIC5C, "foo"));

            Assert.Empty(message.Race);
            var race = new Dictionary<CodingResponseMessage.RaceCode, string>();
            race.Add(CodingResponseMessage.RaceCode.RACE1E, "foo");
            race.Add(CodingResponseMessage.RaceCode.RACE17C, "bar");
            race.Add(CodingResponseMessage.RaceCode.RACEBRG, "baz");
            message.Race = race;
            race = message.Race;
            Assert.Equal(3, race.Count);
            Assert.True(race.ContainsKey(CodingResponseMessage.RaceCode.RACE1E));
            Assert.Equal("foo", race.GetValueOrDefault(CodingResponseMessage.RaceCode.RACE1E,"yyz"));
            Assert.True(race.ContainsKey(CodingResponseMessage.RaceCode.RACE17C));
            Assert.Equal("bar", race.GetValueOrDefault(CodingResponseMessage.RaceCode.RACE17C,"yyz"));
            Assert.True(race.ContainsKey(CodingResponseMessage.RaceCode.RACEBRG));
            Assert.Equal("baz", race.GetValueOrDefault(CodingResponseMessage.RaceCode.RACEBRG,"yyz"));

            Assert.Null(message.UnderlyingCauseOfDeath);
            message.UnderlyingCauseOfDeath = "A04.7";
            Assert.Equal("A04.7", message.UnderlyingCauseOfDeath);

            Assert.Empty(message.CauseOfDeathRecordAxis);
            var recordAxisCodes = new List<string>();
            recordAxisCodes.Add("A04.7");
            recordAxisCodes.Add("A41.9");
            recordAxisCodes.Add("J18.9");
            recordAxisCodes.Add("J96.0");
            message.CauseOfDeathRecordAxis = recordAxisCodes;
            recordAxisCodes = message.CauseOfDeathRecordAxis;
            Assert.Equal(4, recordAxisCodes.Count);
            Assert.Equal("A04.7", recordAxisCodes[0]);
            Assert.Equal("A41.9", recordAxisCodes[1]);
            Assert.Equal("J18.9", recordAxisCodes[2]);
            Assert.Equal("J96.0", recordAxisCodes[3]);

            Assert.Empty(message.CauseOfDeathEntityAxis);
            var entityAxisEntries = new List<CauseOfDeathEntityAxisEntry>();
            var entry1 = new CauseOfDeathEntityAxisEntry("DEATH CERT LINE 1 TEXT", "abcde");
            entry1.AssignedCodes.Add("code1_1");
            entry1.AssignedCodes.Add("code1_2");
            entityAxisEntries.Add(entry1);
            var entry2 = new CauseOfDeathEntityAxisEntry("DEATH CERT LINE 2 TEXT", "xyzzy");
            entry2.AssignedCodes.Add("code2_1");
            entityAxisEntries.Add(entry2);
            message.CauseOfDeathEntityAxis = entityAxisEntries;
            entityAxisEntries = message.CauseOfDeathEntityAxis;
            Assert.Equal(2, (int)entityAxisEntries.Count);
            Assert.Equal("DEATH CERT LINE 1 TEXT", entityAxisEntries[0].DeathCertificateText);
            Assert.Equal("abcde", entityAxisEntries[0].CauseOfDeathConditionId);
            Assert.Equal(2, (int)entityAxisEntries[0].AssignedCodes.Count);
            Assert.Equal("code1_1", entityAxisEntries[0].AssignedCodes[0]);
            Assert.Equal("code1_2", entityAxisEntries[0].AssignedCodes[1]);
            Assert.Equal("DEATH CERT LINE 2 TEXT", entityAxisEntries[1].DeathCertificateText);
            Assert.Equal("xyzzy", entityAxisEntries[1].CauseOfDeathConditionId);
            Assert.Equal(1, (int)entityAxisEntries[1].AssignedCodes.Count);
            Assert.Equal("code2_1", entityAxisEntries[1].AssignedCodes[0]);
        }

        [Fact]
        public void TestCauseOfDeathEntityAxisEntry()
        {
            var entry = new CauseOfDeathEntityAxisEntry("FooBarBaz", "id101010");
            Assert.Equal("FooBarBaz", entry.DeathCertificateText);
            Assert.Equal("id101010", entry.CauseOfDeathConditionId);
            Assert.Empty(entry.AssignedCodes);
            entry.AssignedCodes.Add("A10.4");
            entry.AssignedCodes.Add("J01.5");
            Assert.Equal(2, entry.AssignedCodes.Count);
            Assert.Equal("A10.4", entry.AssignedCodes[0]);
            Assert.Equal("J01.5", entry.AssignedCodes[1]);
        }

        private string FixturePath(string filePath)
        {
            if (Path.IsPathRooted(filePath))
            {
                return filePath;
            }
            else
            {
                return Path.GetRelativePath(Directory.GetCurrentDirectory(), filePath);
            }
        }
    }
}
