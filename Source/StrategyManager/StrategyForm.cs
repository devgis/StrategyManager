using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StrategyManager.DAL;

namespace StrategyManager
{
    public partial class StrategyForm : Form
    {
        public StrategyForm()
        {
            InitializeComponent();
        }

        private void StrategyForm_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = StrategyData.GetTestData();
        }
    }
}
