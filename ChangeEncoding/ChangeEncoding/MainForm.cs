using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChangeEncoding
{
    public partial class MainForm : Form
    {
        private string[] mCodeNames = { "UTF8", "UTF8 no BOM", "utf-16", "utf-32", "Unicode", "ASCII", "gb2312" };
        private Encoding[] mEnCodes = { new UTF8Encoding(true), new UTF8Encoding(false), Encoding.GetEncoding("utf-16"), Encoding.UTF32, Encoding.Unicode, Encoding.ASCII, Encoding.GetEncoding("gb2312") };
        private string[] mFileTypes = { "*.csv", "*.json", "*.txt" };
        public MainForm()
        {
            InitializeComponent();
            InitializeFileTypeList();
            InitializeEncodingTypeList();
        }

        private void InitializeEncodingTypeList()
        {
            DataTable encodingTypeList = new DataTable();
            DataColumn ftc1 = new DataColumn("F_ID", typeof(int));
            DataColumn ftc2 = new DataColumn("F_Name", typeof(string));
            encodingTypeList.Columns.Add(ftc1);
            encodingTypeList.Columns.Add(ftc2);
            for (int i = 0; i < mCodeNames.Length; i++)
            {
                DataRow ADR = encodingTypeList.NewRow();
                ADR[0] = i;
                ADR[1] = mCodeNames[i];
                encodingTypeList.Rows.Add(ADR);
            }
            //进行绑定  
            enCodeComboBox.DisplayMember = "F_Name";//控件显示的列名  
            enCodeComboBox.ValueMember = "F_ID";//控件值的列名  
            enCodeComboBox.DataSource = encodingTypeList;
        }

        private void InitializeFileTypeList()
        {
            DataTable fileTypeList = new DataTable();  
            DataColumn ftc1 = new DataColumn("F_ID", typeof(int));
            DataColumn ftc2 = new DataColumn("F_Name", typeof(string));
            fileTypeList.Columns.Add(ftc1);
            fileTypeList.Columns.Add(ftc2);
            for (int i = 0; i < mFileTypes.Length; i++)
            {
                DataRow ADR = fileTypeList.NewRow();
                ADR[0] = i;
                ADR[1] = mFileTypes[i];
                fileTypeList.Rows.Add(ADR);
            }
            //进行绑定
            fileTypeComboBox.DisplayMember = "F_Name";//控件显示的列名  
            fileTypeComboBox.ValueMember = "F_ID";//控件值的列名  
            fileTypeComboBox.DataSource = fileTypeList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string folderPath = dialog.SelectedPath;
                //MessageBox.Show("已选择文件夹:" + foldPath, "选择文件夹提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.textBox1.Text = folderPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBox1.Text) == false)
            {
                MessageBox.Show(string.Format("已选择文件夹: ' {0} ' 不存在，请重新选择！", textBox1.Text), "运行错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                StartChangeEncoding();
            }
        }

        private void StartChangeEncoding()
        {
            string folderPath = textBox1.Text;
            int fileTypeIndex = fileTypeComboBox.SelectedIndex;
            int encodeIndex = enCodeComboBox.SelectedIndex;
            string fileType = mFileTypes[fileTypeIndex];
            string encodeType = mCodeNames[encodeIndex];
            string[] files = Directory.GetFiles(folderPath, fileType, SearchOption.AllDirectories);
            int count = 0;
            progressBar.Maximum = files.Length;
            foreach (string file in files)
            {
                string contents = File.ReadAllText(file);
                File.WriteAllText(file, contents, mEnCodes[encodeIndex]);
                count++;
                progressBar.Value = count;
            }
            if (count == files.Length)
            {
                MessageBox.Show(string.Format("转换成功，共转换{0}个文件", count), "转换提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
