using Draeger.Pdms.Services.Extensions;
using Draeger.Pdms.Services.Json;
using Draeger.Pdms.Services.Json.Entities;
using DragerXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DraegerXML
{
    public struct UsedMedicament
    {
        public string Id;
        public string Name;
        public string Dose;
    }
    //релизует интерфейс IObjectforJsonprovider
    public class DraegerHospitalProvider : IHospitalProvider
    {
        public DraegerHospitalProvider(
            string certificate,
            string certificateFilePassword,
            string clappId,
            string serverHostName,
            int serverPort,
            string domainId,
            DateTime fromTimestamp,
            DateTime toTimestamp
           
        ) 
        {
            this.certificate = certificate;
            this.certificateFilePassword = certificateFilePassword;
            this.clappId = clappId;
            this.serverHostName = serverHostName;
            this.domainId = domainId;
            this.serverPort = serverPort;
            this.fromTimestamp = fromTimestamp;
            this.toTimestamp = toTimestamp;
            
        }
        public CLAPPConfiguration CreateConfig()
        {
            return new CLAPPConfiguration()
            {
                Certificate = certificate,
                CertificateFilePassword = certificateFilePassword.ToSecureString(),
                CLAPPID = clappId,
                DomainID = domainId,
                ServerHostname = serverHostName,
                ServerPort = serverPort
            };
        }
        public Hospital? GetHospital()
        {
            CLAPPConfiguration config = CreateConfig();
            try
            {
                return BuildHospital(config);
            }
            catch( Exception e) 
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        private Hospital BuildHospital(CLAPPConfiguration config)
        {
            Hospital hospital = new Hospital();

            using (CLAPP clapp = new CLAPP(config))
            {
                PatientsList pList = clapp.GetPatientsList(); 
                foreach (var p in pList.PatientList) 
                {
                    ArrivalSick patient = BuildPatient(clapp, p);
                    hospital.Patients.Add(patient);
                }
            } 
            return hospital;
        }

        private ArrivalSick BuildPatient(CLAPP clapp, Draeger.Pdms.Services.Json.Entities.Patient p)
        {
            clapp.SetPatient(p.CaseID);
            ArrivalSick patient = new ArrivalSick { Id = p.CaseID };
            var proc = BuildProcedure(patient, clapp, p);
            foreach (var medicamID in medicamIDs)
            {
                string template = CreateParamsTemplate(medicamID.Name);
                var pt = clapp.ParseTemplate(
                    p.CaseID,
                    template,
                    fromTimestamp,
                    toTimestamp
                );
                BuildParameterFromTemplate(proc, pt, medicamID);
                TEMPORARY_writeResultToFile(pt);

            }
            
            clapp.ReleasePatient();
            return patient;
        }

        private Operation BuildProcedure(ArrivalSick patient, CLAPP clapp, Draeger.Pdms.Services.Json.Entities.Patient p)
        {
            var pt = clapp.ParseTemplate(
                   p.CaseID,
                   CreateProcedureTemplate(),
                   new DateTime(1990, 1, 1),
                   DateTime.Now
               );
            Operation proc = new Operation();
            proc.Id = pt.TextResult;
            patient.Procedures.Add(proc);   
               
            return proc;
        }

        private void TEMPORARY_writeResultToFile(ParseTemplate pt)
        {
            if (File.Exists("result.txt"))
                return;
            File.AppendAllText("result.txt", pt.TextResult);
        }

        private void BuildParameterFromTemplate(Operation proc, ParseTemplate pt, UsedMedicament medicament)
        {
            var tokens = pt.TextResult.Split(';', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tokens.Length; i++) 
            {
                proc.Medicaments.Add(
                    new Medicament
                    {
                        Id = medicament.Id,
                        Name = medicament.Name,
                        Dose = medicament.Dose
                    }
                ); 
            }
          
        }


        private string certificate;
        private string certificateFilePassword;
        private string clappId;
        private string serverHostName;
        private string domainId;
        private int serverPort;
        private DateTime fromTimestamp;
        private DateTime toTimestamp;

        private static List <UsedMedicament> medicamIDs = new List< UsedMedicament >()
        {
            new UsedMedicament { Id = "700734,1", Name = "Propofol 1% Propofol 20 ml 1 AMP." },
            new UsedMedicament { Id = "700277,1", Name = "Fentanyl Fentanyl 0.1mg 1 AMP."},
            new UsedMedicament { Id = "700278,1", Name = "Fentanyl Fentanyl 0.5mg 1 AMP."},
            new UsedMedicament { Id = "701130,1", Name = "Ultiva Remifentanil 1 mg 1 VIAL"},
            new UsedMedicament { Id = "700918,1", Name = "Esmeron Rocuronium 50mg 1 AMP."},
            new UsedMedicament { Id = "701238,1", Name = "Lysthenon Suxamethon 5% 100mg/2ml 1 Amp."},
            new UsedMedicament { Id = "701377,1", Name = "Dormicum Midazolam 5 mg/5 ml 1 Amp."},
            new UsedMedicament { Id = "701745,1", Name = "Droperidol Droperidol 1 mg/2 ml 1 AMP."},
            new UsedMedicament { Id = "700494,1", Name = "Morphium Morphin 10mg 1 AMP."},
            new UsedMedicament { Id = "701111,1", Name = "Catapresan Clonidin 0.15mg 1 Amp."},
            new UsedMedicament { Id = "701454,1", Name = "Robinul-Neostigmin Glycopyrrolat 1 Amp."},
            new UsedMedicament { Id = "703119", Name = "Mepivacain 1% 50 ml 1 Amp."},
            new UsedMedicament { Id = "700092,1", Name = "Atropin Atropinsulfat 0.5mg/ml1 Amp."},
            new UsedMedicament { Id = "703115,1", Name = "Nalbuphin 20 mg 1 Amp." },
            new UsedMedicament { Id = "703163,1", Name = "Ketalar 10 mg/ml 20 ml 1 Amp."},
            new UsedMedicament { Id = "700631,1", Name = "Ultiva Remifentanil 2 mg 1 VIAL"},
            new UsedMedicament { Id = "700891,1", Name = "Anexate 0.5 mg Flumazenil 1 AMP."},
            new UsedMedicament { Id = "700635,1", Name = "Rapifen Alfentanil 1mg 1 Amp." },
            new UsedMedicament { Id = "700366,1", Name = "Etomidat Etomidat 20mg 1 Amp." },
            new UsedMedicament { Id = "703162", Name = "Bupivacain 0.5% 20 ml 1 Amp." },
            new UsedMedicament { Id = "700016,1", Name = "Ebrantil Urapidil 50mg 1 AMP." },
            new UsedMedicament { Id = "700743,1", Name = "Carbostesin 0.5% h'bar Bupivacain 1 Amp." },
            new UsedMedicament { Id = "310678", Name = "AMBU Larynxmaske mit Cuff und universalKonnektor 15 mm Gr.5.0" },
            new UsedMedicament { Id = "321950", Name = "Wärmedecke Snuggle Warm Oberkörper klein76 x 203cm" },
            new UsedMedicament { Id = "316788", Name = "Verbinder TCi steril 200cm, D:0.9/2.0mm" },
            new UsedMedicament { Id = "316888", Name = "Intubationsspatel Metall einweg Gr. 4 MacIntosh" },
            new UsedMedicament { Id = "320603", Name = "Ultraschallbezug Safersonic Plus 18 x 120cm, mit Haftfolie, ohne Gel" },
            new UsedMedicament { Id = "316139", Name = "Elektrode CPR One Step R-Serie, inkl. EKG Elektroden für Erwachsene" },
            new UsedMedicament { Id = "319903", Name = "AMBU AuraGain Einweg Larynxmaske Gr 5" },
            new UsedMedicament { Id = "300671", Name = "Sensor BIS XP f.Erwachsene" },
            new UsedMedicament { Id = "302961", Name = "Tubus Endotracheal mit Cuff RAE oral ID:7.0mm" },
            new UsedMedicament { Id = "311295", Name = "Blasenkatheter mit Ballon Rüsch SilikonBrillant CH 16, zylindrisch, 2 Augen geg" },
            new UsedMedicament { Id = "313781", Name = "Katheterset Blase steril, Spital ThurgauAG mit Instillagel" },
            new UsedMedicament { Id = "321465", Name = "Urinsammelsystem mit Wechselb. UnoMeter2L, Messkammer 500ml" },
            new UsedMedicament { Id = "321923", Name = "MRI EKG Elektrode Hextrode" },
            new UsedMedicament { Id = "321924", Name = "MRI Einweg SpO2 Grip-Sensor Adult IR3880" },
            new UsedMedicament { Id = "315805", Name = "Kanüle Spinal Pencan 25G, 120mm, orange,mit Introducer" },
            new UsedMedicament { Id = "309731", Name = "Trachealtubus RAE Microcuff oral vorgeformt D:5.5mm" },
            new UsedMedicament { Id = "316886", Name = "Intubationsspatel Metall einweg Gr. 2 MacIntosh" },
            new UsedMedicament { Id = "300845", Name = "Kanüle Arterie mit Floswitch rot 20G ,1.1x 45mm, 49ml/min." },
            new UsedMedicament { Id = "302964", Name = "Tubus Endotracheal Spiral verstärkt Safety-Flex mit Cuff ID:6.5mm" },
            new UsedMedicament { Id = "302971", Name = "Tubus Endotr.Spezialcuff MLT mikrolaryngeale Eingriffe ID:5.0mm" },
            new UsedMedicament { Id = "312701", Name = "Tubus Larynx einweg LTS-D Gr. 4" },
            new UsedMedicament { Id = "314093", Name = "Monitoringsystem 1-fach mit xtrans" },
            new UsedMedicament { Id = "320072", Name = "Verneblereinheit für intubierte Patienten mit Mundstück" },
            new UsedMedicament { Id = "302652", Name = "Hypafix stretch 10m x 10cm" },
            new UsedMedicament { Id = "313894", Name = "Tegaderm CHG Chlorhexidingluconat IV Fixierverband 10.0 x 12.0cm / 3.0 x 4.0cm" },
            new UsedMedicament { Id = "314569", Name = "Schlauchset steril Hotline, f. Flüssigkeitsaufwärmer" },
            new UsedMedicament { Id = "315856", Name = "Double D Beatmungsschlauch 180cm, mit Beutel und Zusatzschlauch" },
            new UsedMedicament { Id = "316104", Name = "Ableitungskabel EKG einweg 5-fach, IEC11/1.5m" },
            new UsedMedicament { Id = "316107", Name = "Manschette NBP einweg f.Erwachsene Gr.L,31-40cm/40cm" },
            new UsedMedicament { Id = "316131", Name = "Sensor SpO2 Finger einweg Adult LNCS Adtx Masimo" },
            new UsedMedicament { Id = "300743", Name = "Filter Peridural Perifix 0.2um" },
            new UsedMedicament { Id = "310650", Name = "Endotrachealtubus Laser Flex ID:6.0mm" },
            new UsedMedicament { Id = "311669", Name = "Kanüle Epidural Anästhesie Perican TuohySchliff Luer Lock 1.3 mm x 120 mm 18 G" },
            new UsedMedicament { Id = "312089", Name = "Absaugventil Einweg MAJ-209 BF" },
            new UsedMedicament { Id = "312437", Name = "Perfusorleitung UV-Protect, PVC frei 200cm, gelb" },
            new UsedMedicament { Id = "313340", Name = "PlexusKath. - Set Contiplex S 1.3 x 150mm / 18G x 6" },
            new UsedMedicament { Id = "313835", Name = "Infusionsset intraossär EZ-IO LD-Nadelsatz Erw. ab 40 kg, 45 mm, 15 G gelb" },
            new UsedMedicament { Id = "316146", Name = "ZVK Europ. Max.l Barrrier Set 2-Lumen ZVK Arrow Gard Blue Plus 7 Fr. x 20 cm" },
            new UsedMedicament { Id = "316216", Name = "EKG Verlängerungskabel zur LokalisationZVK" },
            new UsedMedicament { Id = "318003", Name = "Maske Endoskopie Gr. 5 für Bronchoskopie/ Fiberoptische Intubation" },
            new UsedMedicament { Id = "320359", Name = "Güdelthubus geschlitzt Gr. 9 gelb für fiberoptische Intubation" },
            new UsedMedicament { Id = "320428", Name = "Arterien Katheter Set Arrow Radialis 20Gx 8cm" },
            new UsedMedicament { Id = "322033", Name = "PlasmaBag Standard für Plasma Typhoon, 40 x 47 x 13cm, 15KG" },
            new UsedMedicament { Id = "700524.1", Name ="NaCl 9 g/l NaCl 100 ml 1 Ecoflac" }, 
            new UsedMedicament { Id = "701672.1", Name ="Ketesse Dexketoprofen 50 mg 1 AMP." },
            new UsedMedicament { Id = "701822.1", Name ="Viscotears SDU Carbomer 0.6 g 1 GEL" },
            new UsedMedicament { Id = "700938.1", Name ="Zofran Ondansetron 4mg 1 Amp." },
            new UsedMedicament { Id = "703003.1", Name ="Mephameson 4 mg 1 AMP. " },
            new UsedMedicament { Id = "703145.1", Name ="Paracetamol i.v. 1  g 1 Infusion" }, 
            new UsedMedicament { Id = "700007.1", Name ="Novalgin 50% Novaminsulfon 2ml  1 Amp." }, 
            new UsedMedicament { Id = "705631.1", Name ="Endosgel Gleitmittel  6ml 1 Spritze" }, 
            new UsedMedicament { Id = "701600.1", Name ="NaCl 9 g/l NaCl 250 ml 1 Ecoflac" }, 
            new UsedMedicament { Id = "702139.1", Name ="Voltaren Diclofenac 25 mg 1 Supp." }, 
            new UsedMedicament { Id = "703660.1", Name ="Voltaren 12.5 mg  1  Supp." },
            new UsedMedicament { Id = "700711.1", Name ="Syntocinon Oxytocin 5 IE/ml 1 Amp." }, 
            new UsedMedicament { Id = "700885.1", Name ="Cordarone Amiodaron 150mg 1 Amp." }, 
            new UsedMedicament { Id = "704294.1", Name ="Octenisept Lös farblos 10ml" },
            new UsedMedicament { Id = "700580.1", Name ="Primperan Metoclopramid 10mg 1 Amp." },
            new UsedMedicament { Id = "700200",   Name ="Diamox 500 mg 1 Amp." }, 
            new UsedMedicament { Id = "700749",   Name ="Magnesiumsulfat 20 mmol SPT 10 ml Add." }, 
            new UsedMedicament { Id = "701366.1", Name ="Adrenalin Adrenalin 0.1 mg 1 Amp." },
            new UsedMedicament { Id = "700846.1", Name ="Voltaren Diclofenac 50mg 1 Supp." }, 
            new UsedMedicament { Id = "700941.1", Name ="Tavegyl Clemastin 2mg 1 AMP." },
            new UsedMedicament { Id = "709551.1", Name ="Noradrenalin Ultra 0.01 mg /ml 5ml 1 Amp" },
            new UsedMedicament { Id = "700102.1", Name ="Bactrim 400/80mg Cotrimoxazol 5ml 1 Amp." },
            new UsedMedicament { Id = "700417.1", Name ="Lasix Furosemid 20 mg 1 Amp." }, 
            new UsedMedicament { Id = "701001.1", Name ="Cytotec Misoprostol 0.2mg 1 Tabl." },
            new UsedMedicament { Id = "706659.1", Name ="Co-Amoxicillin 2.2 g 1 Amp." }, 
            new UsedMedicament { Id = "700318.1", Name ="Glucose  50 g/l Glucose 500 ml 1 INFUSIO" },
            new UsedMedicament { Id = "700686", Name ="Solu-Cortef 100 mg 2 ml 1 Amp." },
            new UsedMedicament { Id = "702102", Name ="Nevanac 5 ml Augentropfen" }, 
            new UsedMedicament { Id = "700127.1", Name ="Buscopan Butylscopolamin 20mg 1 Amp." }, 
            new UsedMedicament { Id = "700218.1", Name ="Temesta expidet Lorazepam 1mg 1 Tabl." },  
            new UsedMedicament { Id = "700521.1", Name ="NaCl 9 g/l NaCl 1000 ml 1 Ecoflac" },
            new UsedMedicament { Id = "700602.1", Name ="Mefenamin Mefenaminsäure 125 mg 1 Supp." },  
            new UsedMedicament { Id = "701243.1", Name ="Dafalgan Paracetamol 300mg 1 Supp." },
            new UsedMedicament { Id = "701321", Name ="Maxitrol 5 ml 1 Lösung" },
            new UsedMedicament { Id = "707099", Name ="Neosynephrin-POS (D) 5 % 10 ml Gtt Opth." }, 
            new UsedMedicament { Id = "700629", Name ="Protamin 1000 IE/ml 5 ml 1 Amp." }, 
            new UsedMedicament { Id = "700694", Name ="Spersacarpine 2% 10 ml 1 Lösung " },
            new UsedMedicament { Id = "701242.1", Name ="Dafalgan Paracetamol 150mg 1 Supp." },  
            new UsedMedicament { Id = "701397",  Name ="Solu-Medrol SAB Act o Vial 125 mg 1 Vial" },  
            new UsedMedicament { Id = "703157.1", Name ="Mefenamin  500 mg 1 Supp." }, 
            new UsedMedicament { Id = "707523.1", Name ="Co-Amoxicillin 1.2 g 1 Amp." },  
            new UsedMedicament { Id = "700155.1", Name ="Dafalgan Paracetamol 600mg 1 Supp." },  
            new UsedMedicament { Id = "700179.1", Name ="Clindamycin Clindamycin 600 mg 1 AMP." },  
            new UsedMedicament { Id = "700801.1", Name ="Voltaren Diclofenac 100mg 1 Supp." }, 
            new UsedMedicament { Id = "700811", Name ="Ventolin 200 Dosen 1 Spray" },  
            new UsedMedicament { Id = "701723.1", Name ="Co-Amoxicillin 550 mg 1 Amp." },  
            new UsedMedicament { Id = "701867", Name ="Novorapid 10 ml 1 Amp." },  
            new UsedMedicament { Id = "702078.1", Name ="Heparin 25'000 IE Heparin 5 ml 1 Amp." },  
            new UsedMedicament { Id = "703027.1", Name ="NaCl 9 g/l 3000 ml 1 SPÜLLÖS." }, 
            new UsedMedicament { Id = "703112.1", Name ="Cefazolin 1000 mg 1 Amp." },  
            new UsedMedicament { Id = "703322.1", Name ="Aqua dest. Spül Lös 1 Ecotainer 1000 ml" },
            new UsedMedicament { Id = "703374.1", Name ="OCTOSTIM Inj Lös 15 mcg/ml 1 Amp. 1 ml" },
            new UsedMedicament { Id = "705544.1", Name ="LISINOPRIL  5mg 1 Tabl." }, 
            new UsedMedicament { Id = "707182", Name ="Nitroglycerin 0.01% 10 ml 1 Vial" },
            new UsedMedicament { Id = "250142", Name = "Sevoflurane: Narkoseunterhalt in Min. Patientenalter ab 17 Jahre (0.15ml/min) (K"},
            new UsedMedicament { Id = "250143", Name = "Sevoflurane: Narkoseunterhalt in Min. Patientenalter bis u. mit 16 Jahre (0.25ml"},
            new UsedMedicament { Id = "250141", Name = "Desfluran (Supran): Narkoseunterhalt inMinuten (0.6ml/min) (KSM)"},
            new UsedMedicament { Id = "701834.1", Name = "Ringerfundin Elektrolyte 1000 ml 1 Ecofl "},
            new UsedMedicament { Id = "703443.1", Name = "Ropivacain 0.5 % 5 mg/ml 10 ml 1 Amp. "},
            new UsedMedicament { Id = "700085", Name = "Propofol 2% 50 ml 1 Amp. "},
            new UsedMedicament { Id = "706771", Name = "Ephedrin 50 mg/10 ml 1 Vial "},
            new UsedMedicament { Id = "703152.1", Name = "Lidocain Bichsel Inj Lös 1 % 1 Amp 5 ml "},
            new UsedMedicament { Id = "312864", Name = "Flächen Desinfektionm. Cleanisept Desinfektionstü.ALKOHOLFREI Nachfüller 100 Tüc"},
            new UsedMedicament { Id = "705952.1", Name = "Set Universal STG Münsterlingen 1 ST "},
            new UsedMedicament { Id = "701350.1", Name = "Noradrenalin Noradrenalin 1mg 1 AMP. "},
            new UsedMedicament { Id = "703442.1", Name = "Ropivacain 0.2 % 2 mg/ml 20 ml 1 Amp. "},
            new UsedMedicament { Id = "705955.1", Name = "Set Spinal SPT Münsterlingen 1 ST "},
            new UsedMedicament { Id = "705032.1", Name = "Cefuroxim 1.5 g 1 Amp. "},
            new UsedMedicament { Id = "707316.1", Name = "Set Caudal STG Münsterlingen 1 ST "},
            new UsedMedicament { Id = "703211.1", Name = "Bridion 200 mg/2ml 1 Amp. "},
            new UsedMedicament { Id = "700491", Name = "Prilocain 0.75% 40 ml Amp. "},
            new UsedMedicament { Id = "702179.1", Name = "Mivacron Mivacurium 10 mg 1 AMP. "},
            new UsedMedicament { Id = "250144", Name = "Sevoflurane: Inhalative Einleitung (12ml) (KSM)"},
            new UsedMedicament { Id = "705206.1", Name = "KETAMIN 500 mg/10ml 1 Amp. "},
            new UsedMedicament { Id = "701833.1", Name = "Ringerfundin Elektrolyte 500 ml 1 Ecofl "},
            new UsedMedicament { Id = "707008", Name = "Ropivacain 1 % 10 mg/ml 5 Amp 10 ml "},
            new UsedMedicament { Id = "700056.1", Name = "Adrenalin Adrenalin 1mg 1 AMP. "},
            new UsedMedicament { Id = "700248", Name = "Alkohol 70% SPT 250 ml Spray "},
            new UsedMedicament { Id = "700571", Name = "Ringerlac/Glucose 1% 500 ml 1 INF. PPF "},
            new UsedMedicament { Id = "703246.1", Name = "Naloxon Actavis Inj Lös 0.4 mg/ml 1  Amp "},
            new UsedMedicament { Id = "700427", Name = "Lignocain  1% SPT 50 ml Fl. "},
            new UsedMedicament { Id = "706593.1", Name = "Lidocain Streuli 2% 40 mg/2ml 1 Amp. "},
            new UsedMedicament { Id = "701577.1", Name = "Dafalgan Odis Paracetamol 500mg 1 Tabl. "},
            new UsedMedicament { Id = "701971.1", Name = "Ringerfundin Elektrolyte 500 ml 1 Ecobag "},
            new UsedMedicament { Id = "706455.1", Name = "Tranexam 500 mg/5ml 1 Amp. "},
            new UsedMedicament { Id = "700285.1", Name = "Metronidazol Metronidazol 500 mg 1 INFUS "},
            new UsedMedicament { Id = "700680", Name = "Cyclogyl 1% 10 ml Augenropfen "},
            new UsedMedicament { Id = "703114.1", Name = "Tropicamid 0.5% 0.4 ml 1 SDU "},
            new UsedMedicament { Id = "700398", Name = "Kaliumchlorid 2.5mmol/ml SPT 50 ml 1 ADD "},
            new UsedMedicament { Id = "702191.1", Name = "Bridion Sugammadex 500 mg 1 Amp. "},
            new UsedMedicament { Id = "707829.1", Name = "Phenylephrin 0.1 mg/ml 5 ml 1 Amp "},
            new UsedMedicament { Id = "701201.1", Name = "Salbutamol 0.05%-Ipratropium 0.01% 10 ml "},
            new UsedMedicament { Id = "701388", Name = "Cocain 10% SPT 0.5 ml SDU "},
            new UsedMedicament { Id = "702062.1", Name = "Ultra Stop 5 ml 1 Flasche "},
            new UsedMedicament { Id = "702208", Name = "Morphin 0.1% SPT ad infus 200 ml PP Inf. "},
            new UsedMedicament { Id = "704527.1", Name = "Pabal 0.1 mg/ml 1 ml 1 Amp. "},
            new UsedMedicament { Id = "704707.1", Name = "Physiogel balanced 500 ml 1 bag "},
            new UsedMedicament { Id = "704895", Name = "Anticholium (D) 2 mg/5 ml 1 Amp. " }
                                               

        };

        private string CreateParamsTemplate( string medicamName)
        {           
            string t2 =
               $" orders: treatmentname = {medicamName}; " +
               $" Range = All;" +
               $" admins = all;" +
               " format = !({admindate};" +
               "{treatmentname};" +
               "{SubstanceIdent};" +
               "{ct_dose};" +
               "{SubstanceUnit}\\LF)]";


            return t2;
        }
        private string CreateProcedureTemplate ()
        {
            return "[PreOP: Format=!({OP_ID})]";
        }
    }
}
