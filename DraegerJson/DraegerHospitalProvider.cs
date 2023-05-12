using Draft_Draeger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerJson
{
    //релизует интерфейс IObjectforJsonprovider
    public class DraegerHospitalProvider : IHospitalProvider
    {
        public DraegerHospitalProvider(
            string certificate,
            string certificateFilePassword,
            string clappId,
            string domainId,
            string clientName,
            string dsiName
        ) 
        {
            this.certificate = certificate;
            this.certificateFilePassword = certificateFilePassword;
            this.clappId = clappId;
            this.domainId = domainId;
            this.clientName = clientName;
            this.dsiName = dsiName;
        }
        public Hospital GetHospital()
        {
            throw new NotImplementedException();
        }

        private string certificate;
        private string certificateFilePassword;
        private string clappId;
        private string domainId;
        private string clientName;
        private string dsiName;


    }
}
