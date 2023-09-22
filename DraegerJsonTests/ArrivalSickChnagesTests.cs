using DraegerJson;
using Hl7.Fhir.Model;

namespace DraegerJsonTests
{
    public class ArrivalSickChangesTests
    {
        Parameter p1 = null!;
        Parameter p2 = null!;
        Parameter p3 = null!;
        Parameter p4 = null!;
        Parameter p5 = null!;
        Parameter p6 = null!;
        Parameter chP1 = null!;
        Parameter chP2 = null!;
        Parameter chP3 = null!;
        Parameter chP4 = null!;
        Parameter chP5 = null!;
        Parameter chP6 = null!;

        Operation proc1 = null!;
        Operation proc2 = null!;
        Operation proc3 = null!;
        Operation chProc1 = null!;
        Operation chProc2 = null!;
        Operation chProc3 = null!;
        Operation chProc4 = null!;
        Operation proc4 = null!;

        [SetUp]
        public void SetUp()
        {
            CreateParameters();
            CreateChangedParameters();
            CreateProcedures();
            CreateChangedProcedures();


        }
        public void CreateProcedures()
        {
            proc1 = new Operation
            {
                Id = "proc1",
                ResourceType = "Procedure",
                Status = "Completed",
                Params = new List<Parameter> { p1, p2 }
            };
            proc2 = new Operation
            {
                Id = "proc2",
                ResourceType = "Procedure",
                Status = "Completed",
                Params = new List<Parameter> { p3 }
            };
            proc3 = new Operation
            {
                Id = "proc3",
                ResourceType = "Procedure",
                Status = "Completed",
                Params = new List<Parameter> { p4, p5, p6 }
            };
            proc4 = new Operation
            {
                Id = "proc4",
                ResourceType = "Procedure",
                Status = "Completed",
                Params = new List<Parameter> { p4, p5, p6 }
            };

        }
        public void CreateChangedProcedures()
        {
            chProc1 = new Operation
            {
                Id = "proc1",
                ResourceType = "Procedure",
                Status = "Completed",
                Params = new List<Parameter> { chP1, p2 }
            };
            chProc2 = new Operation
            {
                Id = "proc2",
                ResourceType = "Procedure",
                Status = "Completed",
                Params = new List<Parameter> { chP3 }
            };
            chProc3 = new Operation
            {
                Id = "chProc3",
                ResourceType = "Procedure",
                Status = "Completed",
                Params = new List<Parameter> { chP4, p5, chP6 }
            };
            chProc4 = new Operation
            {
                Id = "proc4",
                ResourceType = "Procedure",
                Status = "Completed",
                Params = new List<Parameter> { chP4, p5, chP6 }
            };

        }
        private void CreateParameters()
        {
            p1 = new Parameter
            {
                Id = "1",
                Date = new DateTime(2023, 1, 1, 12, 0, 0),
                Name = "p1",
                Milestone = "1",
                PatientId = "1"
            };
            p2 = new Parameter
            {
                Id = "2",
                Date = new DateTime(2023, 1, 1, 13, 0, 0),
                Name = "p2",
                Milestone = "2",
                PatientId = "1"
            };
            p3 = new Parameter
            {
                Id = "3",
                Date = new DateTime(2023, 1, 1, 14, 0, 0),
                Name = "p3",
                Milestone = "3",
                PatientId = "1"
            };
            p4 = new Parameter
            {
                Id = "4",
                Date = new DateTime(2023, 1, 1, 15, 0, 0),
                Name = "p4",
                Milestone = "4",
                PatientId = "2"
            };
            p5 = new Parameter
            {
                Id = "5",
                Date = new DateTime(2023, 1, 1, 16, 0, 0),
                Name = "p5",
                Milestone = "5",
                PatientId = "2"
            };
            p6 = new Parameter
            {
                Id = "6",
                Date = new DateTime(2023, 1, 1, 17, 0, 0),
                Name = "p6",
                Milestone = "6",
                PatientId = "2"
            };
        }
        private void CreateChangedParameters()
        {
            chP1 = new Parameter
            {
                Id = "1",
                Date = new DateTime(2023, 1, 1, 12, 30, 0),
                Name = "p1",
                Milestone = "1",
                PatientId = "1"
            };
            chP2 = new Parameter
            {
                Id = "2",
                Date = new DateTime(2023, 1, 1, 13, 30, 0),
                Name = "p2",
                Milestone = "2",
                PatientId = "1"
            };
            chP3 = new Parameter
            {
                Id = "3",
                Date = new DateTime(2023, 1, 1, 14, 30, 0),
                Name = "p3",
                Milestone = "3",
                PatientId = "1"
            };
            chP4 = new Parameter
            {
                Id = "4",
                Date = new DateTime(2023, 1, 1, 15, 30, 0),
                Name = "p4",
                Milestone = "4",
                PatientId = "2"
            };
            chP5 = new Parameter
            {
                Id = "5",
                Date = new DateTime(2023, 1, 1, 16, 30, 0),
                Name = "p5",
                Milestone = "5",
                PatientId = "2"
            };
            chP6 = new Parameter
            {
                Id = "6",
                Date = new DateTime(2023, 1, 1, 17, 30, 0),
                Name = "p6",
                Milestone = "6",
                PatientId = "2"
            };
        }

        [Test]
        public void ContainsChanges_TwoSickIsEquals_ReturnFalse()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as1, as2);
            Assert.False(chAs1.ContainsChange);
        }
        [Test]
        public void ContainsChanges_TwoSickIdIsDifferent_ReturnTrue()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "2",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as1, as2);
            Assert.True(chAs1.ContainsChange);
        }
        [Test]
        public void IdChanges_TwoSickIdIsDifferent_ReturnTrue()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "2",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as1, as2);
            Assert.True(chAs1.IdChanged);
        }
        [Test]
        public void ContainsChanges_TwoSickAdmissionToWardDateIsDifferent_ReturnTrue()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 10, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as1, as2);
            Assert.True(chAs1.ContainsChange);
        }
        [Test]
        public void AdmissionToWardChanged_TwoSickAdmissionToWardDateIsDifferent_ReturnTrue()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 10, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as1, as2);
            Assert.True(chAs1.AdmissionToWardDateChanged);
        }
        public void ContainsChanges_TwoSickAufnahmeNrDifferent_ReturnTrue()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "2",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as1, as2);
            Assert.True(chAs1.ContainsChange);
        }
        [Test]
        public void AufnahmeNrChanged_TwoSickAufnahneNrIsDifferent_ReturnTrue()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 10, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "2",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 10, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as1, as2);
            Assert.True(chAs1.AufnahmeNRChanged);
        }
        [Test]
        public void ContainsChanges_TwoSickFullNameIsDifferent_ReturnTrue()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as2",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as1, as2);
            Assert.True(chAs1.ContainsChange);
        }
        [Test]
        public void FullNameChanged_TwoSickAdmissionToWardDateIsDifferent_ReturnTrue()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as2",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as1, as2);
            Assert.True(chAs1.FullNameChanged);
        }
        public void ContainsChanges_TwoSickLocationIsDifferent_ReturnTrue()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location2",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as1, as2);
            Assert.True(chAs1.ContainsChange);
        }
        [Test]
        public void LocationChanged_TwoSickLocationIsDifferent_ReturnTrue()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location2",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as1, as2);
            Assert.True(chAs1.LocationChanged);
        }
        public void ContainsChanges_TwoSickOPDateIsDifferent_ReturnTrue()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 12, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as1, as2);
            Assert.True(chAs1.ContainsChange);
        }
        [Test]
        public void OPDateChanged_TwoSickOPDateisDifferent_ReturnTrue()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 12, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as1, as2);
            Assert.True(chAs1.OPDateChanged);
        }
        public void ContainsChanges_TwoSickProcedureIsDifferent_ReturnTrue()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 12, 0, 0),
                Procedures = new List<Operation> { chProc1, chProc2 }
            };
            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as1, as2);
            Assert.True(chAs1.ContainsChange);
        }
        [Test]
        public void ProcedureChanged_TwoSickProceduresDifferent_ReturnTrue()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc1, proc2 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { chProc1, chProc2 }
            };
            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as1,as2);
            Assert.True(chAs1.ProceduresChanged);
        }
        [Test]
        public void ProcedureChanged_ChangeOneProcedure_CountOfChangeEqualsOne()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc2, proc3 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc2, chProc3 }
            };
            
            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as1, as2);
            Assert.That(chAs1.NewProcedures.Count,Is.EqualTo(1));
        }
        [Test]
        public void ProcedureChanged_ChangeTwoParameters_CountOfChangeEqualsTwo()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc2, proc4 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc2, chProc4 }
            };

            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as1, as2);
            Assert.That(chAs1.NewProcedures[0].Params.Count, Is.EqualTo(2));
        }
        [Test]
        public void ProcedureChanged_ChangeOneProcedure_ContainsChangedProcedure()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc2, proc3 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc3, chProc2 }
            };

            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as1, as2);
            Assert.That(chAs1.NewProcedures[0].Id, Is.EqualTo(proc2.Id));
        }
        [Test]
        public void ProcedureChanged_ChangeTwoParameters_ContainsChangedParameters()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc2, proc3 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc2, chProc3 }
            };

            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as2, as1);
            Assert.True(chAs1.NewProcedures[0].Params.Contains(chP4));
            Assert.True(chAs1.NewProcedures[0].Params.Contains(chP6));
        }
        [Test]
        public void ProcedureChanged_ChangeProcedureWithId_ResultContainsThreeParameters()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc2, proc3 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc2, chProc3 }
            };

            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as1, as2);
            Assert.That(chAs1.NewProcedures[0].Params.Count, Is.EqualTo(3));
        }
        [Test]
        public void ProcedureChanged_ChangeProcedureWithId_ResultContainsAllParameters()
        {
            ArrivalSick as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc2, proc3 }
            };
            ArrivalSick as2 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 9, 0, 0),
                AufnahmeNR = "1",
                FullName = "as1",
                Location = "location1",
                OPDate = new DateTime(2023, 1, 1, 11, 0, 0),
                Procedures = new List<Operation> { proc2, chProc3 }
            };

            ArrivalSickChanges chAs1 = new ArrivalSickChanges(as2, as1);
            Assert.True(chAs1.NewProcedures[0].Params.Contains(chP4));
            Assert.True(chAs1.NewProcedures[0].Params.Contains(p5));
            Assert.True(chAs1.NewProcedures[0].Params.Contains(chP6));
        }
    }
}