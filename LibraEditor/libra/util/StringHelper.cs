namespace libra.util
{
    class StringHelper
    {
        public static bool isNumberic(string _string)
        {
            if (string.IsNullOrEmpty(_string))
                return false;
            foreach (char c in _string)
            {
                if (!char.IsDigit(c))
                    //if(c<'0' c="">'9')//最好的方法,在下面测试数据中再加一个0，然后这种方法效率会搞10毫秒左右
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 首字母变大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToTitleCase(string str)
        {
            //return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(str);
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }
    }
}
