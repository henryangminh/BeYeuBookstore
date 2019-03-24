using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Utilities.GeneralFunction
{
    public class functionString
    {
        private static functionString instance;
        public static functionString Instance
        {
            get { if (instance == null) instance = new functionString(); return instance; }
            set { instance = value; }
        }
        //1. hàm cắt chuổi theo đấu "+", trả về những chuổi đã được IN HOA lên rồi
        public List<string> splitStringToUpper(string s)
        {
            // xóa những khoảng trống nếu có ở trước và sau đấu + 
            List<string> ret = new List<string>();
            if (s != null || s != "")
            {
                string[] _s = s.Split('+');
                foreach (var item in _s)
                {
                    var a = item.TrimStart().TrimEnd();
                    if (a != "")
                        ret.Add(a.ToUpper());
                }
            }
            return ret;
        }
    }
}
