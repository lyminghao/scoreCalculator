using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace scoreCalculator
{
    public static class dataSet // 存放全局变量dt及校验函数
    {
        public static DataTable dt;
        public static int checkInput(String name, String credit, String score)
        {
            if (name.Trim() == "") return 1; // name为空
            foreach (DataRow dr in dataSet.dt.Rows)
            {
                String kcmc = dr[0].ToString();
                if (name == kcmc) return 1; // name与已有记录重复
            }

            if (!isDoubleValue(credit)) return 2; // credit格式错误
            if (double.Parse(credit) < 0 || double.Parse(credit) > 20) return 2; // credit范围错误

            if (!isIntValue(score)) return 3; // score格式错误
            if (int.Parse(score) < 0 || int.Parse(score) > 100) return 3; // score范围错误

            return 0; // 没有错误
        }
        public static bool isDoubleValue(String str) // 判断字符串是否可转换为double
        {
            double num;
            try
            {
                num = double.Parse(str);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool isIntValue(String str) // 判断字符串是否可转换为int
        {
            int num;
            try
            {
                num = int.Parse(str);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
