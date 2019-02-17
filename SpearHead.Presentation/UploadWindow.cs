using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpearHead.Presentation.SpearHeadService;
using System.IO;
using System.Threading;

namespace DotCorePoc.Presentation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Excel Worksheets|*.xlsx",
                CheckFileExists = true,
                CheckPathExists = true
            };

            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK) // Test result.
            {
                byte[] files = File.ReadAllBytes(openFileDialog.FileName);
                Submit(files);

            }
        }

        private void Submit(byte[] files)
        {
            try
            {
                using (ExcelUploadServicecsClient uploadServicecsClient = new ExcelUploadServicecsClient())
                {
                    var res = uploadServicecsClient.UploadAsync(new ExcelUploadModel() { Name = "est", Content = files }).Result;

                    if (res.HttpStatusCode != StatusCodes.Sucess)
                    {
                        string messsages = string.Join(Environment.NewLine, res.ErrorMessages.Select(p =>
                        string.Concat($"Row { p.Row.ToString()} ", string.Join(",", p.ErrorMessagees))));
                        MessageBox.Show(messsages);
                        return;
                    }
                    MessageBox.Show("Successfully Uploaded");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
