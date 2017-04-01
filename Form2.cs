using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace scoreCalculator
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ;
        }

        private void button2_Click(object sender, EventArgs e) // 重置Button
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            radioButton1.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e) // 添加Button
        {
            String name   = textBox1.Text.Trim();
            String credit = textBox2.Text.Trim();
            String score  = textBox3.Text.Trim();

            int flag;
            if ((flag = dataSet.checkInput(name, credit, score)) > 0) // 输入有非法内容
            {
                if (flag == 1)
                {
                    MessageBox.Show("添加失败！课程名称为空或重复。");
                    return;
                }
                if (flag == 2)
                {
                    MessageBox.Show("添加失败！学分错误，请输入0-20小数。");
                    return;
                }
                if (flag == 3)
                {
                    MessageBox.Show("添加失败！成绩错误，请输入0-100整数。");
                    return;
                }
            }

            /***** 以下部分可认为输入值均合法 *****/
            
            double dpoint = 1.0; // 默认 radioButton1.Checked
            if (radioButton2.Checked)
            {
                dpoint = 1.2;
            }
            int cj = Math.Max(int.Parse(score), 60);
            double gpa = ((cj - 60) * 1.0 / 10 + 1.0) * double.Parse(credit) * dpoint; // GPA公式
            dataSet.dt.Rows.Add(new object[] { name, double.Parse(credit), int.Parse(score), gpa });
            MessageBox.Show("添加成功！");
            this.Close();
        }
    }
}
