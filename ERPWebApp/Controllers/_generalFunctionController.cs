using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BeYeuBookstore.Controllers
{
    public class _generalFunctionController
    {
        private static _generalFunctionController instance;
        public static _generalFunctionController Instance
        {
            get { if (instance == null) instance = new _generalFunctionController(); return instance; }
            set { instance = value; }
        }
        public string getClaimType(ClaimsPrincipal a, string type)
        {
           var temp= a.Claims.FirstOrDefault(x => x.Type == type);
            if (temp == null)
                return null;
            return temp.Value;
        }
    }
}
