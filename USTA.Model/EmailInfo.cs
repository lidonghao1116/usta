using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    [Serializable]
    public class EmailInfo
    {
        public int sendingEmailListId
        {
            set;
            get;
        }

        public string senderDisplayName
        {
            set;
            get;
        }

        public string senderEmailAddress
        {
            get;
            set;
        }

        public string senderEmailPassword
        {
            get;
            set;
        }

        public string receiverEmailAddress
        {
            get;
            set;
        }

        public string emailTitle
        {
            get;
            set;
        }

        public string emailContent
        {
            get;
            set;
        }

        public List<string>[] fileNameList
        {
            get;
            set;
        }

        public string senderEmailServer
        {
            get;
            set;
        }


        public int senderEmailServerPort
        {
            get;
            set;
        }
    }
}
