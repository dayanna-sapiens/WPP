using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;

namespace WPP.Entities.Objects.Generales
{
    public class SystemVars : Entity
    {
        public string senderEmail { set; get; }
        public string senderName { set; get; }
        public string host { set; get; }
        public string username { set; get; }
        public string password { set; get; }
        public bool useCredentials { set; get; }
        public bool sendMails { set; get; }
    }
}
