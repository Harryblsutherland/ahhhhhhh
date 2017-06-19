
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoredGamesWindowsform
{
    public sealed partial class frmOrders : Form
    {   
//////////////////////////////////////////////////Singleton/////////////////////////////////////////////////////////////
        private static readonly frmOrders _Instance = new frmOrders();
        public static frmOrders Instance
        {
            get { return frmOrders._Instance; }
        }

        private frmOrders()
        {
            InitializeComponent();
        }
        public async void UpdateDisplay()
        {
            try
            {
                lstOrders.DataSource = null;
                lstOrders.DataSource = await ServiceClient.GetOrderInfoAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WHat have you done frmOrders UPDATEDISPLAY() is no goods);");
                    
            }
           
        }
/////////////////////////////////////////////////////////////EVENTS/////////////////////////////////////////////////////////
        private void lstOrders_DoubleClick(object sender, EventArgs e)
        {
            string lcKey;

            lcKey = Convert.ToString(lstOrders.SelectedItem);
            if (lcKey != null)
                try
                {   
                    frmOrderData.Run(lstOrders.SelectedItem as string);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "This should never occur");
                }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
                   Hide();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            string lcKey;

            lcKey = Convert.ToString(lstOrders.SelectedItem);
            if (lcKey != null && MessageBox.Show("Are you sure?", "Deleting Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                try
                {
                    MessageBox.Show(await ServiceClient.DeleteOrderAsync(lcKey));
                    lstOrders.ClearSelected();
                    UpdateDisplay();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error deleting artist");
                }
        }
        private void frmOrders_Load(object sender, EventArgs e)
        {
            UpdateDisplay();

        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            UpdateDisplay();

        }

        private async void btnNewOrder_Click(object sender, EventArgs e)
        {
            MessageBox.Show(await ServiceClient.InsertOrderAsync());
            lstOrders.ClearSelected();
            UpdateDisplay();
        }

        private void lstOrders_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}