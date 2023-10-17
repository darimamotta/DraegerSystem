
using DraegerJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerJsonTests
{
    internal class ConverterJsonWithLimitedUpdateTimeTests
    {
        Parameter p1 = null!;
        Parameter p2 = null!;
        Parameter p3 = null!;
        Parameter p4 = null!;
        Parameter p5 = null!;
        Parameter p6 = null!;
        Parameter p7 = null!;

        Parameter chP1 = null!;
        Parameter chP3 = null!;

        Operation op1 = null!;
        Operation op2 = null!;
        Operation op3 = null!;
        Operation op4 = null!;

        Operation chOp1 = null!;


        ArrivalSick as1 = null!;
        ArrivalSick as2 = null!;

        ArrivalSick chAs1 = null!;

        Hospital emptyHospital = null!;
        Hospital hospitalWithAs1 = null!;
        Hospital hospitalWithAs1As2 = null!;
        Hospital hospitalWithchAs1As2 = null!;
        Hospital hospitalWithOldAs1As2 = null!;


        [SetUp]
        public void SetUp()
        {
            CreateParameters();
            CreateChangedParameters();
            CreateProcedures();
            CreateChangedProcedures();
            CreateArrivalSick();
            CreateHospitals();

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
                Date = new DateTime(2023, 1, 1, 17, 1, 0),
                Name = "p2",
                Milestone = "2",
                PatientId = "2"
            };
            p3 = new Parameter
            {
                Id = "3",
                Date = new DateTime(2023, 1, 1, 13, 0, 0),
                Name = "p3",
                Milestone = "3",
                PatientId = "1"
            };
            p4 = new Parameter
            {
                Id = "4",
                Date = new DateTime(2023, 1, 1, 18, 1, 0),
                Name = "p4",
                Milestone = "4",
                PatientId = "2"
            };
            p5 = new Parameter
            {
                Id = "5",
                Date = new DateTime(2023, 1, 1, 14, 0, 0),
                Name = "p5",
                Milestone = "5",
                PatientId = "1"
            };
            p6 = new Parameter
            {
                Id = "6",
                Date = new DateTime(2023, 1, 1, 19, 1, 0),
                Name = "p6",
                Milestone = "6",
                PatientId = "2"
            };
            p7 = new Parameter
            {
                Id = "7",
                Date = new DateTime(2023, 1, 1, 15, 0, 0),
                Name = "p7",
                Milestone = "7",
                PatientId = "1"
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
            chP3 = new Parameter
            {
                Id = "3",
                Date = new DateTime(2023, 1, 1, 13, 30, 0),
                Name = "p3",
                Milestone = "3",
                PatientId = "1"
            };
        }
        public void CreateProcedures()
        {
            op1 = new Operation
            {
                Id = "proc1",
                ResourceType = "Procedure",
                Status = "Completed",
                Params = new List<Parameter> { p1, p3 }
            };
            op2 = new Operation
            {
                Id = "proc2",
                ResourceType = "Procedure",
                Status = "Completed",
                Params = new List<Parameter> { p2, p4 }
            };
            op3 = new Operation
            {
                Id = "proc3",
                ResourceType = "Procedure",
                Status = "Completed",
                Params = new List<Parameter> { p5 }
            };
            op4 = new Operation
            {
                Id = "proc4",
                ResourceType = "Procedure",
                Status = "Completed",
                Params = new List<Parameter> { p6 }
            };

        }
        public void CreateChangedProcedures()
        {
            chOp1 = new Operation
            {
                Id = "proc1",
                ResourceType = "Procedure",
                Status = "Completed",
                Params = new List<Parameter> { chP1, p3, p7 }
            };
        }
        public void CreateArrivalSick()
        {
            as1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 0, 0, 0),
                AufnahmeNR = "111",
                FullName = "as1",
                Location = "01bed",
                OPDate = new DateTime(2023, 1, 1, 11, 30, 0),
                Procedures = new List<Operation> { op1, op3 }
            };
            as2 = new ArrivalSick
            {
                Id = "2",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 16, 0, 0),
                AufnahmeNR = "222",
                FullName = "as2",
                Location = "02bed",
                OPDate = new DateTime(2023, 1, 1, 16, 30, 0),
                Procedures = new List<Operation> { op2, op4 }
            };
            chAs1 = new ArrivalSick
            {
                Id = "1",
                AdmissionToWardDate = new DateTime(2023, 1, 1, 0, 0, 0),
                AufnahmeNR = "111",
                FullName = "as1",
                Location = "01bed",
                OPDate = new DateTime(2023, 1, 1, 11, 30, 0),
                Procedures = new List<Operation> { chOp1, op3 }
            };

        }
        public void CreateHospitals()
        {
            emptyHospital = new Hospital
            {
                Timestamp = new DateTime(2022, 12, 31, 22, 0, 0),
                Patients = new List<ArrivalSick>()
            };

            hospitalWithAs1 = new Hospital
            {
                Timestamp = new DateTime(2023, 1, 1, 15, 30, 0),
                Patients = new List<ArrivalSick> { as1 }
            };

            hospitalWithAs1As2 = new Hospital
            {
                Timestamp = new DateTime(2023, 1, 1, 22, 0, 0),
                Patients = new List<ArrivalSick> { as1, as2 }
            };
            hospitalWithchAs1As2 = new Hospital
            {
                Timestamp = new DateTime(2023, 1, 1, 22, 10, 0),
                Patients = new List<ArrivalSick> { chAs1, as2 }
            };
            hospitalWithOldAs1As2 = new Hospital
            {
                Timestamp = new DateTime(2023, 1, 2, 1, 0, 0),
                Patients = new List<ArrivalSick> { as1, as2 }
            };
        }
        [Test]
        public void Convert_EmptyHospital_CountEqualsZero()
        {
            ConverterJsonWithLimitedUpdateTime converter =
                new ConverterJsonWithLimitedUpdateTime(new TimeSpan(24, 0, 0));
            var result = converter.Convert(emptyHospital);
            Assert.That(result.Count, Is.EqualTo(0));
        }
        [Test]
        public void Convert_NewPatientInHospital_CountEqualsOne()
        {
            ConverterJsonWithLimitedUpdateTime converter =
                new ConverterJsonWithLimitedUpdateTime(new TimeSpan(24, 0, 0));
            var result = converter.Convert(emptyHospital);
            result = converter.Convert(hospitalWithAs1);
            Assert.That(result.Count, Is.EqualTo(1));
        }
        [Test]
        public void Convert_NewPatientInHospital_ContainsCorrectPatient()
        {
            ConverterJsonWithLimitedUpdateTime converter =
                new ConverterJsonWithLimitedUpdateTime(new TimeSpan(24, 0, 0));
            var result = converter.Convert(emptyHospital);
            result = converter.Convert(hospitalWithAs1);
            Assert.That(result[0].PatientId, Is.EqualTo(as1.Id));
        }
        [Test]
        public void Convert_AddSecondPatientInHospital_CountEqualsOne()
        {
            ConverterJsonWithLimitedUpdateTime converter =
                new ConverterJsonWithLimitedUpdateTime(new TimeSpan(24, 0, 0));
            var result = converter.Convert(emptyHospital);
            result = converter.Convert(hospitalWithAs1);
            result = converter.Convert(hospitalWithAs1As2);
            Assert.That(result.Count, Is.EqualTo(1));
        }
        [Test]
        public void Convert_AddSecondPatientInHospital_ContainsCorrectPatient()
        {
            ConverterJsonWithLimitedUpdateTime converter =
                new ConverterJsonWithLimitedUpdateTime(new TimeSpan(24, 0, 0));
            var result = converter.Convert(emptyHospital);
            result = converter.Convert(hospitalWithAs1);
            result = converter.Convert(hospitalWithAs1As2);
            Assert.That(result[0].PatientId, Is.EqualTo(as2.Id));
        }
        [Test]
        public void Convert_OnePatientTimeout_CountEqualsOne()
        {
            ConverterJsonWithLimitedUpdateTime converter =
                new ConverterJsonWithLimitedUpdateTime(new TimeSpan(24, 0, 0));
            var result = converter.Convert(hospitalWithOldAs1As2);         
            Assert.That(result.Count, Is.EqualTo(1));
        }
        [Test]
        public void Convert_OnePatientTimeout_ContainsCorrectPatient()
        {
            ConverterJsonWithLimitedUpdateTime converter =
                new ConverterJsonWithLimitedUpdateTime(new TimeSpan(24, 0, 0));
            var result = converter.Convert(hospitalWithOldAs1As2);            
            Assert.That(result[0].PatientId, Is.EqualTo(as2.Id));
        }
        [Test]
        public void Convert_OnePatientChangedAndTimeout_ContainsCorrectPatient()
        {
            ConverterJsonWithLimitedUpdateTime converter =
                new ConverterJsonWithLimitedUpdateTime(new TimeSpan(24, 0, 0));
            var result = converter.Convert(hospitalWithchAs1As2);
            result = converter.Convert(hospitalWithOldAs1As2);
            Assert.That(result.Count, Is.EqualTo(0));
        }
        [Test]
        public void Convert_OnePatientChangedAndOneAdded_CountEqualsTwo()
        {
            ConverterJsonWithLimitedUpdateTime converter =
                new ConverterJsonWithLimitedUpdateTime(new TimeSpan(24, 0, 0));
            var result = converter.Convert(hospitalWithAs1);
            result = converter.Convert(hospitalWithchAs1As2);
            Assert.That(result.Count, Is.EqualTo(2));
        }
        [Test]
        public void Convert_OnePatientChangedAndOneAdded_ContainsCorrectPatient()
        {
            ConverterJsonWithLimitedUpdateTime converter =
                new ConverterJsonWithLimitedUpdateTime(new TimeSpan(24, 0, 0));
            var result = converter.Convert(hospitalWithAs1);
            result = converter.Convert(hospitalWithchAs1As2);
            Assert.That(result.Find(pj=>pj.PatientId==chAs1.Id), Is.Not.Null);
            Assert.That(result.Find(pj => pj.PatientId == as2.Id), Is.Not.Null);
        }
        [Test]
        public void Convert_OnePatientTimeoutButFlagIsSet_CountEqualsTwo()
        {
            ConverterJsonWithLimitedUpdateTime converter =
                new ConverterJsonWithLimitedUpdateTime(new TimeSpan(24, 0, 0));
            converter.CanAddOldPatient=true;
            var result = converter.Convert(hospitalWithOldAs1As2);
            Assert.That(result.Count, Is.EqualTo(2));
        }
        [Test]
        public void Convert_OnePatientTimeoutButFlagIsSet_ContainsCorrectPatient()
        {
            ConverterJsonWithLimitedUpdateTime converter =
                new ConverterJsonWithLimitedUpdateTime(new TimeSpan(24, 0, 0));
            converter.CanAddOldPatient = true;
            var result = converter.Convert(hospitalWithOldAs1As2);
            Assert.That(result.Find(pj => pj.PatientId == as1.Id), Is.Not.Null);
            Assert.That(result.Find(pj => pj.PatientId == as2.Id), Is.Not.Null);
        }

    }
}
