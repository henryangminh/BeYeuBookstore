using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Utilities.GeneralFunction
{
    public class _convertFunction
    {
        private static _convertFunction instance;
        public static _convertFunction Instance
        {
            get { if (instance == null) instance = new _convertFunction(); return instance; }
            set { instance = value; }
        }
        #region  các hàm chuyển đổi

        #region chuyển một chuổi số sang chuôi có định dang tiền tệ.
        public string ToMoney(object obj)
        {
            string kq = "0";
            if (obj != null)
            {
                string s = obj.ToString();
                if (s == "" || s == "0")
                    return kq;
                try
                {
                    int len = s.Length;
                    while (len > 0 && (s[len - 1] < '0' || s[len - 1] > '9'))
                    {
                        s = s.Remove(len - 1, 1);
                        len--;
                    }
                    if (len > 0)
                    {
                        decimal temp = Convert.ToDecimal(s);
                        if (temp > 0)
                            kq = temp.ToString("#,#"); //đ
                    }
                }
                catch
                {
                    ////_AddMessages.Instance.Notification("Chuổi = " + s + "\n Nhập sai định dạng yêu cầu. Vui lòng kiểm tra lại !");
                }
            }
            return kq;
        }
        public string ToMoney(string s) //0.0000
        {
            string s1 = "0";
            if (s == null || s == "" || s == "0 đ" || s == "0")
                return s1;
            try
            {
                int len = s.Length;
                while (len > 0 && (s[len - 1] < '0' || s[len - 1] > '9'))
                {
                    s = s.Remove(len - 1, 1);
                    len--;
                }
                if (len > 0)
                {
                    decimal temp = Convert.ToDecimal(s);
                    if (temp > 0)
                        s1 = temp.ToString("#,#"); //đ
                }
            }
            catch
            {
                //_AddMessages.Instance.Notification("Chuổi = " + s + "\n Nhập sai định dạng yêu cầu. Vui lòng kiểm tra lại !");
            }
            return s1;
        }
        public string ToMoney(decimal? s) //0.0000
        {
            string s1 = "0";
            if (s == null || s == 0)
                return s1;

            try
            {
                decimal temp = Convert.ToDecimal(s);
                if (temp > 0)
                    s1 = temp.ToString("#,#0"); //đ
            }
            catch
            {
                //_AddMessages.Instance.Notification("Chuổi = " + s + "\n Nhập sai định dạng yêu cầu. Vui lòng kiểm tra lại !");
            }
            return s1;
        }
        #endregion

        #region chuyển một chuổi số sang chuôi --> định đang %
        public string ToTax(string s) // 0.12
        {
            if (s == null || s == "0" || s == "") return "0 %";
            try
            {
                int len = s.Length;
                while (len > 0 && (s[len - 1] < '0' || s[len - 1] > '9'))
                {
                    s = s.Remove(len - 1, 1);
                    len--;
                }
                return Convert.ToDouble(s).ToString("#0.## %"); // tự động *100 vào ở hàm này 12%
            }
            catch
            {
                //_AddMessages.Instance.Notification("Chuổi = " + s + "\n Nhập sai định dạng yêu cầu. Vui lòng kiểm tra lại !");
            }
            return "0 %";
        }
        public string ToTax(object obj) // 0.12
        {
            if (obj != null)
            {
                string s = obj.ToString();
                if (s == null || s == "0" || s == "") return "0 %";
                try
                {
                    int len = s.Length;
                    while (len > 0 && (s[len - 1] < '0' || s[len - 1] > '9'))
                    {
                        s = s.Remove(len - 1, 1);
                        len--;
                    }
                    return Convert.ToDouble(s).ToString("#0.## %"); // tự động *100 vào ở hàm này 12%
                }
                catch
                {
                    //_AddMessages.Instance.Notification("Chuổi = " + s + "\n Nhập sai định dạng yêu cầu. Vui lòng kiểm tra lại !");
                }
            }
            return "0 %";
        }
        #endregion

        #region chuyển một đuổi định dạng tiền tệ/dạng %/string số ----> kiểu number
        public int toInt(string s)
        {
            if (s == "" || s == "0" || s == null) return 0;
            int i = s.Length;
            int temp = 0;
            try
            {
                if (s[i - 1] == 'đ' || s[i - 1] == '%')
                    temp = Convert.ToInt32(s.Remove(i - 1, 1));
                else
                    temp = Convert.ToInt32(s);
            }
            catch
            {
                //_AddMessages.Instance.Notification("Chuổi = " + s + "\n Nhập sai định dạng yêu cầu. Vui lòng kiểm tra lại !");
            }
            return temp;
        }

        public int toInt(object obj)
        {
            if (obj != null)
            {
                string s = obj.ToString();
                if (s == "" || s == "0") return 0;
                int i = s.Length;
                int temp = 0;
                try
                {
                    if (s[i - 1] == 'đ' || s[i - 1] == '%')
                        temp = Convert.ToInt32(s.Remove(i - 1, 1));
                    else
                        temp = Convert.ToInt32(s);
                }
                catch
                {
                    //_AddMessages.Instance.Notification("Chuổi = " + s + "\n Nhập sai định dạng yêu cầu. Vui lòng kiểm tra lại !");
                    return 0;
                }
                return temp;
            }
            return 0;
        }
        public decimal ToDecimal(object obj) //tiền tệ
        {
            if (obj != null)
            {
                string s = obj.ToString();
                if (s == "" || s == "0" || s == null) return 0;
                int i = s.Length;
                decimal temp = 0;
                try
                {
                    if (s[i - 1] == 'đ' || s[i - 1] == '%')
                        temp = Convert.ToDecimal(s.Remove(i - 1, 1));
                    else
                        temp = Convert.ToDecimal(s);
                }
                catch
                {
                    //_AddMessages.Instance.Notification("Chuổi = " + s + "\n Nhập sai định dạng yêu cầu. Vui lòng kiểm tra lại !");
                    return 0;
                }

                return temp;
            }
            return 0;
        }
        public decimal ToDecimal(object obj, int round) //tiền tệ
        {
            if (obj != null)
            {
                string s = obj.ToString();
                if (s == "" || s == "0" || s == null) return 0;
                int i = s.Length;
                decimal temp = 0;
                try
                {
                    if (s[i - 1] == 'đ' || s[i - 1] == '%')
                        temp = Convert.ToDecimal(s.Remove(i - 1, 1));
                    else
                        temp = Convert.ToDecimal(s);
                }
                catch
                {
                    //_AddMessages.Instance.Notification("Chuổi = " + s + "\n Nhập sai định dạng yêu cầu. Vui lòng kiểm tra lại !");
                    return 0;
                }

                return temp;// decimal.Round(temp,round);
            }
            return 0;
        }

        //----------- khoa do sai số khi thực hiện phép công: 0.1+0.2=0.30...01 :https://docs.oracle.com/cd/E19957-01/806-3568/ncg_goldberg.html
        //https://stackoverflow.com/questions/588004/is-floating-point-math-broken/51723472#51723472
        //public double toDouble(string s)
        //{
        //    if (s == "" || s == "0" || s == null) return 0;
        //    int i = s.Length;
        //    double temp = 0;
        //    try
        //    {
        //        if (s[i - 1] == 'đ' || s[i - 1] == '%')
        //            temp = Convert.ToDouble(s.Remove(i - 1, 1));
        //        else
        //            temp = Convert.ToDouble(s);
        //    }
        //    catch
        //    {
        //        //_AddMessages.Instance.Notification("Chuổi = " + s + "\n Nhập sai định dạng yêu cầu. Vui lòng kiểm tra lại !");
        //    }

        //    return temp;

        //}
        public double toDouble(object obj)
        {
            if (obj != null)
            {
                string s = obj.ToString();
                if (s == "" || s == "0" || s == null) return 0;
                int i = s.Length;
                double temp = 0;
                try
                {
                    if (s[i - 1] == 'đ' || s[i - 1] == '%')
                        temp = Convert.ToDouble(s.Remove(i - 1, 1));
                    else
                        temp = Convert.ToDouble(s);
                }
                catch
                {
                    //_AddMessages.Instance.Notification("Chuổi = " + s + "\n Nhập sai định dạng yêu cầu. Vui lòng kiểm tra lại !");
                    return 0;
                }

                return temp;
            }
            return 0;
        }
        // input number % , output number, ex: 12,12% =0,1212
        public float toNumberTax(string s = null)
        {
            if (s == null || s == "")
                return 0;
            int i = s.Length;
            float f;
            if (s[i - 1] == '%')
                float.TryParse(s.Remove(i - 1, 1), out f); // thấy lỗi muôn test thử thì đổi sang Parse
            else
                float.TryParse(s, out f);
            return f / 100;
        }
        public float toNumberTax(object obj = null)
        {
            if (obj != null)
            {
                string s = obj.ToString();
                if (s == null || s == "")
                    return 0;
                int i = s.Length;
                float f;
                if (s[i - 1] == '%')
                    float.TryParse(s.Remove(i - 1, 1), out f); // thấy lỗi muôn test thử thì đổi sang Parse
                else
                    float.TryParse(s, out f);
                return f / 100;
            }
            return 0;
        }
        /// <summary>
        /// Trả về chuổi, nếu =null thì trả về ""
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string To_StringNotNull(object s)
        {
            if (s != null)
            {
                var ret = s.ToString().Trim();

                return ret.Replace('\'', '′');
            }
            else
                return "";
        }
        public string ToUpper(string s)
        {
            if (s == null || s == "")
                return s;
            return s.ToUpper();
        }
        #endregion

        #region bỏ dấu tiếng Việt
        private static readonly string[] VietnameseSigns = new string[]

    {

        "aAeEoOuUiIdDyY",

        "áàạảãâấầậẩẫăắằặẳẵ",

        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

        "éèẹẻẽêếềệểễ",

        "ÉÈẸẺẼÊẾỀỆỂỄ",

        "óòọỏõôốồộổỗơớờợởỡ",

        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

        "úùụủũưứừựửữ",

        "ÚÙỤỦŨƯỨỪỰỬỮ",

        "íìịỉĩ",

        "ÍÌỊỈĨ",

        "đ",

        "Đ",

        "ýỳỵỷỹ",

        "ÝỲỴỶỸ"

    };
        public string RemoveSign4VietnameseString(string str)

        {

            //Tiến hành thay thế , lọc bỏ dấu cho chuỗi

            for (int i = 1; i < VietnameseSigns.Length; i++)

            {

                for (int j = 0; j < VietnameseSigns[i].Length; j++)

                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);

            }

            return str;

        }

        #endregion

        #region chuyển object sang bool
        public bool? ToBool(object obj)
        {
            if (obj != null)
            {
                bool kt = false;
                if (bool.TryParse(obj.ToString(), out kt))
                    return kt;
                else
                {
                    //_AddMessages.Instance.Notification("Lỗi: Giá trị " + obj.ToString() + "không thuộc kiểm bool, vui lòng kiểm tra lại !");
                    return null;
                }
            }
            return null;
        }
        #endregion
        #region
        public string CreatePeriod(int month, int year)
        {
            string period;
            if (month < 10)
            {
                period = "0" + month + "-" + year;
            }
            else
                period = month + "-" + year;
            return period;
        }
        /// <summary>
        /// Trả lại mã tương ứng
        /// </summary>
        /// <param name="len">Độ dài của chuỗi số (nên bé hơn 9)</param>
        /// <param name="value">giá trị mã phần số (có thể sử dụng int_64 >18*10^18 nếu dùng cho hóa đơn lớn)</param>
        /// <param name="position">chỉ vị trí: trước(true) hoặc sau (false)</param>
        /// <param name="s">Mã của bảng</param>
        /// <returns></returns>
        public string Create_Code(bool position, int value, byte len, string s = null) // do s là mã thì có thể trống nên phải đặt ở sau cùng
        {
            string Code = "";
            byte temp = (byte)value.ToString().Length;
            while (temp < len)
            {
                Code += '0';
                temp++;
            }
            Code += value.ToString().Trim();
            if (s != null)
            {
                if (position == true) // Mã của bảng phía trước số
                    return s.Trim() + Code; // C00012
                else  // mã nằm ở sau số thứ tự
                    return Code + "/" + s.Trim(); // 00012/C
            }
            return Code;
        }
        #endregion
        #endregion
    }
}
