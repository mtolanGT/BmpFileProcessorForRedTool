using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BmpFileProcessor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Event handler for the "Select Directory" button click
        private void buttonSelectDirectory_Click(object sender, EventArgs e)
        {
            // Open a FolderBrowserDialog to select a directory
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select the directory containing BMP files.";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedDirectory = folderDialog.SelectedPath;

                    // Create an instance of SampleProcessing and process the BMP files
                    SampleProcessing processor = new SampleProcessing();
                    processor.ProcessBmpFiles(selectedDirectory);

                    MessageBox.Show("Processing complete.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Event handler for the Form Load event
        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialization code can go here
        }
    }
}
