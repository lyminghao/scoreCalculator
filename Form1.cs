#define webDebug

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace scoreCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) // 
        {
            dataSet.dt = new DataTable();
            dataSet.dt.Columns.Add(new DataColumn("课程名称"));
            dataSet.dt.Columns.Add(new DataColumn("学分"));
            dataSet.dt.Columns.Add(new DataColumn("成绩"));
            dataSet.dt.Columns.Add(new DataColumn("绩点"));
            dataGridView1.DataSource = dataSet.dt;
        }
        
        private void button1_Click(object sender, EventArgs e) // 抓取成绩
        {
            if(textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("请将用户名/密码填写完整！");
                return;
            }
            int cnt = getWebRecords(textBox1.Text, textBox2.Text);
            if(cnt > 0) MessageBox.Show("抓取成功！导入了" + cnt + "条记录。");
        }

        private void button2_Click(object sender, EventArgs e) // 毕业计算
        {
            String path = System.Environment.CurrentDirectory + "\\BIYE_Report.txt";
            int flag = createReport(path);
            if (flag == 0)
            {
                MessageBox.Show("生成成功！");
                System.Diagnostics.Process.Start(path);
            }
            else if (flag == 1)
            {
                MessageBox.Show("生成失败！找不到Rules.txt文件。");
            }
            else
            {
                MessageBox.Show("生成失败！Rules.txt文件格式错误。");
            }
        }

        private void button3_Click(object sender, EventArgs e) // 手动添加
        {
            Form2 tmpForm = new Form2();
            tmpForm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e) // 文件导入
        {
            try
            {
                OpenFileDialog hFile = new OpenFileDialog();
                hFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                hFile.Filter = "文本文件|*.txt";
                hFile.RestoreDirectory = true;
                hFile.FilterIndex = 1;
                
                if (hFile.ShowDialog() == DialogResult.OK)
                {
                    // 读取文件
                    String filename = hFile.FileName, line;
                    FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    StreamReader sr = new StreamReader(fs, Encoding.Default);
                    List<String> list = new List<String>();
                    while((line = sr.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                    sr.Close();

                    // 格式判断
                    Dictionary<String, bool> existMap = new Dictionary<string, bool>(); // 文件内是否有重复课程名称
                    existMap.Clear();
                    foreach (String tline in list)
                    {
                        String[] recs = tline.Split(new char[] {' '});
                        
                        if(recs.Length < 4) throw new InvalidDataException();
                        if(dataSet.checkInput(recs[0], recs[1], recs[2]) > 0) // 格式有错
                        {
                            throw new InvalidDataException();
                        }
                        if(recs[3] != "1.0" && recs[3] != "1.2")
                        {
                            throw new InvalidDataException();
                        }
                        
                        if (existMap.ContainsKey(recs[0]))
                        {
                            throw new InvalidDataException();
                        }
                        else
                        {
                            existMap[recs[0]] = true;
                        }
                    }

                    // 录入表格
                    int cnt = 0;
                    foreach (String tline in list)
                    {
                        String[] recs = tline.Split(new char[] { ' ' });
                        int cj = Math.Max(int.Parse(recs[2]), 60);
                        double gpa = ((cj - 60) * 1.0 / 10 + 1.0) * double.Parse(recs[1]) * double.Parse(recs[3]); // GPA公式
                        dataSet.dt.Rows.Add(new object[] { recs[0], double.Parse(recs[1]), int.Parse(recs[2]), gpa });
                        cnt++;
                    }
                    MessageBox.Show("导入成功！导入了" + cnt + "条记录。");
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("导入失败！未找到该文件。");
            }
            catch (Exception)
            {
                MessageBox.Show("导入失败！文件格式错误。");
            }
        }

        private void button5_Click(object sender, EventArgs e) // 清空数据
        {
            DialogResult flag = MessageBox.Show("确认清空数据吗？此操作无法恢复。", "清空数据", MessageBoxButtons.OKCancel);
            if (flag == DialogResult.OK)
            {
                dataSet.dt.Rows.Clear();
                MessageBox.Show("数据已清空！");
            }
        }

        /// <summary>
        /// 网络抓取成绩记录
        /// </summary>
        /// <param name="username">教务系统用户名</param>
        /// <param name="password">教务系统密码</param>
        /// <returns>抓取到的记录条数</returns>
        private int getWebRecords(String username, String password)
        {
#if webDebug
            FileStream fs = new FileStream("./webDebug.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine ("网络抓取-测试输出");
            sw.WriteLine ("=================");
#endif

            Random rand = new Random();
            int tAddValue = rand.Next(10000, 99999);
            String tValue = "a9ddd922-9656-4aa6-b3d2-4b10898" + tAddValue.ToString();
            String url_bd = "http://api.dswjx.nenu.edu.cn/qz/authuser.aspx?t=" + tValue;
            String url_cx = "http://api.dswjx.nenu.edu.cn/qz/score.aspx?t=" + tValue;

            WebBrowser page = new WebBrowser();
            page.ScriptErrorsSuppressed = true;
            page.AllowWebBrowserDrop = true;
            int timeCounter = 0, timeLimit = 10000;
#if webDebug
            sw.WriteLine("WebBroswer 开启成功。");
#endif
            try
            {
                // 绑定账户
                if (page.Document != null) page.Document.Cookie.Remove(0, page.Document.Cookie.Length);
                page.Navigate(url_bd);

                timeCounter = 0;
                while (page.ReadyState != WebBrowserReadyState.Complete) // 加载url_bd的延时
                {
                    Delay(100);
                    timeCounter += 100;
                    if (timeCounter > timeLimit) throw new TimeoutException();
                }

#if webDebug
                sw.WriteLine("请求url_bd成功。网页内容：");
                sw.WriteLine(page.DocumentText);
                sw.WriteLine("Cookie:");
                sw.WriteLine(page.Document.Cookie);

#endif

                HtmlElement hUser = page.Document.GetElementById("fzuser");
                HtmlElement hPswd = page.Document.GetElementById("fzpwd");

                if (hUser == null || hPswd == null) throw new TimeoutException();

                hUser.SetAttribute("value", username);
                hPswd.SetAttribute("value", password);
                page.Document.InvokeScript("saveAuth"); // 请求绑定结果

                timeCounter = 0;
                Delay(3000); // 等待ajax结束（暂时没有判断加载完成的好办法）

                if (page.Document.Body.OuterHtml.IndexOf("[SUCCESS]绑定成功") == -1) throw new InvalidDataException();
                //MessageBox.Show(page.Document.Body.OuterHtml.Replace("\n", ""));

#if webDebug
                sw.WriteLine("Ajax请求完成。网页内容：");
                sw.WriteLine(page.Document.Body.OuterHtml);
                sw.WriteLine("Cookie:");
                sw.WriteLine(page.Document.Cookie);
#endif

                // 获取成绩
                page.Navigate(url_cx);

                timeCounter = 0;
                while (page.ReadyState != WebBrowserReadyState.Complete) // 加载url_cx的延时
                {
                    Delay(100);
                    timeCounter += 100;
                    if (timeCounter > timeLimit) throw new TimeoutException();
                }

#if webDebug
                sw.WriteLine("请求url_cx完成。网页内容：");
                sw.WriteLine(page.DocumentText);
                sw.WriteLine("Cookie:");
                sw.WriteLine(page.Document.Cookie);
#endif
                String htmlDoc = page.DocumentText.Trim();
                htmlDoc = htmlDoc.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                Match rx = Regex.Match(htmlDoc, "\\[.*\\}\\]");
                if (rx == null) throw new TimeoutException();
                page.Dispose();
#if webDebug
                sw.WriteLine("网页请求完成，开始解析Json。");
                sw.WriteLine(rx.ToString());
                sw.Flush();
                sw.Close();
#endif
                return processRecord(rx.ToString());
            }
            catch (InvalidDataException e)
            {
#if webDebug
                sw.WriteLine(e.ToString());
                sw.Flush();
                sw.Close();
#endif
                MessageBox.Show("抓取失败！用户名或密码错误。");
                page.Dispose();
                return 0;
            }
            catch (TimeoutException e)
            {
#if webDebug
                sw.WriteLine(e.ToString());
                sw.Flush();
                sw.Close();
#endif
                MessageBox.Show("抓取失败！网络请求超时。");
                page.Dispose();
                return 0;
            }
            catch(Exception e)
            {
#if webDebug
                sw.WriteLine(e.ToString());
                sw.Flush();
                sw.Close();
#endif
                MessageBox.Show("抓取失败！未定义的错误。");
                page.Dispose();
                return 0;
            }
        }

        /// <summary>
        /// 创建毕业计算报告
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns>状态码</returns>
        private int createReport(String filepath)
        {
            double[] creditLimit = new double[5];  // 各类别要求学分数
            double[] creditPassed = new double[5]; // 各类别已修学分数
            Dictionary<int, String> typeMap = new Dictionary<int, String>();   // 映射：nameTable下标-类别名称
            Dictionary<String, int> courseMap = new Dictionary<String, int>(); // 映射：课程名称-课程类型（课程计划）
            Dictionary<String, bool> passMap = new Dictionary<string, bool>(); // 映射：标记课程是否已经通过

            typeMap.Add(0, "通识教育必修课");
            typeMap.Add(1, "通识教育选修课"); // 不用
            typeMap.Add(2, "专业教育必修课");
            typeMap.Add(3, "专业教育选修课");
            typeMap.Add(4, "职业生涯规划课"); // 不用

            // 读取规则文件
            try
            {
                String rulepath = System.Environment.CurrentDirectory + "\\Rules.txt", line;
                FileStream fs_rule = new FileStream(rulepath, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs_rule, Encoding.Default);

                int type = -1; // 当前在读的课程类别，读到t时更新
                courseMap.Clear();
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Trim() == "" || line[0] == '#') continue; // 空行或注释行 跳过
                    if (line[0] == 's') // 分数要求行
                    {
                        String[] cres = line.Split(new char[] { ' ' });
                        if (cres.Length < 6) throw new InvalidDataException();
                        for (int i = 1; i <= 5; i++)
                        {
                            creditLimit[i - 1] = double.Parse(cres[i]);
                        }
                    }
                    if (line[0] == 't') // 课程类别行
                    {
                        String[] typs = line.Split(new char[] { ' ' });
                        if (typs.Length < 2) throw new InvalidDataException();
                        bool flag = false;
                        for (int i = 0; i < 5; i++)
                        {
                            if (typs[1] == typeMap[i])
                            {
                                type = i;
                                flag = true;
                            }
                        }
                        if (type == 1 || type == 4) throw new InvalidDataException();
                        if (flag == false) throw new InvalidDataException();
                    }
                    if (line[0] == 'c') // 课程名称行
                    {
                        String[] cors = line.Split(new char[] { ' ' });
                        if (cors.Length < 2) throw new InvalidDataException();
                        if (courseMap.ContainsKey(cors[1])) throw new InvalidDataException(); // 重复课程名
                        courseMap[cors[1]] = type;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                return 1; // 找不到Rules.txt
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                return 2; // Rules.txt数据格式错误
            }

            // 求creditPassed & passMap
            for (int i = 0; i < 5; i++) creditPassed[i] = 0;
            passMap.Clear();
            foreach (DataRow dr in dataSet.dt.Rows) // kcmc xf cj jd
            {
                String kcmc = dr[0].ToString();
                double xf = double.Parse(dr[1].ToString());
                int cj = int.Parse(dr[2].ToString());

                if (cj < 60) continue;
                passMap[kcmc] = true; // 该课程已通过
                if (courseMap.ContainsKey(kcmc)) creditPassed[courseMap[kcmc]] += xf; // 通必 专必 专选
                else creditPassed[1] += xf; // 通选
            }

            // 写报告文件
            FileStream fs = new FileStream(filepath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("毕业计算报告");
            sw.WriteLine("============");
            
            /* part 1 */
            sw.WriteLine("1 已修课程列表");
            
            sw.WriteLine("");
            sw.WriteLine("1.1 通识教育必修课，匹配到以下课程：");
            foreach (DataRow dr in dataSet.dt.Rows)
            {
                String kcmc = dr[0].ToString();
                int cj = int.Parse(dr[2].ToString());
                if (cj < 60) continue;
                if (courseMap.ContainsKey(kcmc) && courseMap[kcmc] == 0) sw.WriteLine(kcmc);
            }
            
            sw.WriteLine("");
            sw.WriteLine("1.2 通识教育选修课，匹配到以下课程：");
            foreach (DataRow dr in dataSet.dt.Rows)
            {
                String kcmc = dr[0].ToString();
                int cj = int.Parse(dr[2].ToString());
                if (cj < 60) continue;
                if (courseMap.ContainsKey(kcmc) == false) sw.WriteLine(kcmc);
            }
            
            sw.WriteLine("");
            sw.WriteLine("1.3 专业教育必修课，匹配到以下课程：");
            foreach (DataRow dr in dataSet.dt.Rows)
            {
                String kcmc = dr[0].ToString();
                int cj = int.Parse(dr[2].ToString());
                if (cj < 60) continue;
                if (courseMap.ContainsKey(kcmc) && courseMap[kcmc] == 2) sw.WriteLine(kcmc);
            }
            
            sw.WriteLine("");
            sw.WriteLine("1.4 专业教育选修课，匹配到以下课程：");
            foreach (DataRow dr in dataSet.dt.Rows)
            {
                String kcmc = dr[0].ToString();
                int cj = int.Parse(dr[2].ToString());
                if (cj < 60) continue;
                if (courseMap.ContainsKey(kcmc) && courseMap[kcmc] == 3) sw.WriteLine(kcmc);
            }
            sw.WriteLine("============");
            
            /* part 2 */
            sw.WriteLine("2 毕业学分统计");
            double sum1 = 0, sum2 = 0;
            foreach (double i in creditPassed) sum1 += i;
            foreach (double i in creditLimit)  sum2 += i;
            sw.WriteLine("毕业要求总学分 " + sum2 + " 分，已修课程学分 " + sum1 + " 分。");
            sw.WriteLine("");
            
            sw.WriteLine("1.1 通识教育必修课，要求 " + creditLimit[0] + " 分，已修 " + creditPassed[0] + " 分。");
            sw.WriteLine("还需要修以下课程：");
            foreach (KeyValuePair<String, int> pair in courseMap)
            {
                if (pair.Value != 0) continue; // 通必
                if (passMap.ContainsKey(pair.Key) == false) sw.WriteLine(pair.Key);
            }
            
            sw.WriteLine("");
            sw.WriteLine("2.2 通识教育选修课，要求 " + creditLimit[1] + " 分，已修 " + creditPassed[1] + " 分。");
            sw.WriteLine("还需要修 " + Math.Max(0, creditLimit[1] - creditPassed[1]) + " 分。");
            sw.WriteLine("注意：课程计划规定，各类型通选课均需达到一定学分。软件不能辨别课程类型，请根据 1.2 中的已修列表判断是否符合。");
            
            sw.WriteLine("");
            sw.WriteLine("2.3 专业教育必修课，要求 " + creditLimit[2] + " 分，已修 " + creditPassed[2] + " 分。");
            sw.WriteLine("还需要修以下课程，才能达到毕业要求：");
            foreach (KeyValuePair<String, int> pair in courseMap)
            {
                if (pair.Value != 2) continue; // 专必
                if (passMap.ContainsKey(pair.Key) == false) sw.WriteLine(pair.Key);
            }

            sw.WriteLine("");
            sw.WriteLine("2.4 专业教育选修课，要求 " + creditLimit[3] + " 分，已修 " + creditPassed[3] + " 分。");
            sw.WriteLine("还需要修 " + Math.Max(0, creditLimit[3] - creditPassed[3]) + " 分。");

            sw.WriteLine("");
            double sygh_sum = 0;
            for (int i = 0; i <= 3; i++)
            {
                sygh_sum += Math.Max(0, creditPassed[i] - creditLimit[i]);
            }
            sw.WriteLine("2.5 生涯规划课程，要求 " + creditLimit[4] + " 分，已修 " + sygh_sum + " 分。");
            sw.WriteLine("还需要修 " + Math.Max(0, creditLimit[4] - sygh_sum) + " 分。");
            sw.WriteLine("注意：生涯规划课程的已修学分，是前四个类别超出各自要求的学分之和。");
            sw.WriteLine("============");

            /* part 3 */
            sw.WriteLine("3 毕业绩点统计");
            double gpa_sum = 0;
            foreach (DataRow dr in dataSet.dt.Rows) // kcmc xf cj jd
            {
                int cj = int.Parse(dr[2].ToString());
                double jd = double.Parse(dr[3].ToString());

                if (cj < 60) continue;
                gpa_sum += jd;
            }
            if (sum1 > 0) gpa_sum /= sum1; // 平均学分绩点
            sw.WriteLine("及格课程平均学分绩点为 " + String.Format("{0:F}", gpa_sum) + " ，领取学位证要求达到 2.0 。");
            sw.WriteLine("============");

            /* part 4 */
            sw.WriteLine("4 学分学费计算");
            double select_sum = 0, free_limit = sum2 + 11;
            foreach (DataRow dr in dataSet.dt.Rows) // kcmc xf cj jd
            {
                double xf = double.Parse(dr[1].ToString());
                select_sum += xf;
            }
            double money = Math.Max(0, select_sum - free_limit) * 80;
            sw.WriteLine("毕业时总学分超出 " + free_limit + " 分的部分将收取学分学费（按毕业要求学分 + 11免费学分计算）。");
            sw.WriteLine("已选课程总学分 " + select_sum + " 分，需缴纳学分学费 " + money + " 元（按每学分80元计算）。");
            sw.WriteLine("============");
            sw.WriteLine("欢迎使用。软件作者： @lyminghao");

            sw.Flush();
            sw.Close();
            fs.Close();
            return 0;
        }

        /// <summary>
        /// 延迟系统时间，但系统同时能执行其它任务
        /// </summary>
        /// <param name="Millisecond">延迟的毫秒数</param>
        private void Delay(int Millisecond) //
        {
            DateTime current = DateTime.Now;
            while (current.AddMilliseconds(Millisecond) > DateTime.Now)
            {
                Application.DoEvents();//转让控制权
            }
            return;
        }

        /// <summary>
        /// 解析抓取到的Json数据并插入dt
        /// </summary>
        /// <param name="record">JSon数据</param>
        /// <returns>成功解析的条数</returns>
        private int processRecord(String record)
        {
            List<webRecord> wrList = JsonHelper.DeserializeJsonToList<webRecord>(record.Replace("\'", "\""));
            int retValue = 0;
            foreach(webRecord wr in wrList)
            {
                if (dataSet.checkInput(wr.kcmc, wr.xf.ToString(), wr.zcj.ToString()) > 0) continue; // 格式错误
                dataSet.dt.Rows.Add(new Object[] { wr.kcmc, wr.xf, wr.zcj, wr.jd });
                retValue++;
            }
            return retValue;
        }
    }

    public class webRecord
    {
        public double jd { get; set; }   // 绩点
        public String kcbh { get; set; } // 课程编号
        public int zcj { get; set; }     // 总成绩
        public String xm { get; set; }   // 姓名
        public String xqmc { get; set; } // 学期名称
        public String kcmc { get; set; } // 课程名称
        public double xf { get; set; }   // 学分
    }

    /// <summary>
    /// Json帮助类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeObject(object o)
        {
            string json = JsonConvert.SerializeObject(o);
            return json;
        }

        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
        }

        /// <summary>
        /// 反序列化JSON到给定的匿名对象.
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
        {
            T t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;
        }
    }
}
