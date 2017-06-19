
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoredGamesWindowsform
{
    public sealed partial class frmMain : Form
    {   
        //Singleton
        private static readonly frmMain _Instance = new frmMain();


        public delegate void Notify(string prWholesalename);
        private frmMain()
        {
            InitializeComponent();
        }

        public static frmMain Instance
        {
            get { return frmMain._Instance; }
        }
        private void updateTitle(string prGalleryName)
        {
            if (!string.IsNullOrEmpty(prGalleryName))
                Text = "Gallery (v3 C) - " + prGalleryName;
        }

        public async void UpdateDisplay()
        {
            try
            {
                lstPublishers.DataSource = null;
                lstPublishers.DataSource = await ServiceClient.GetPublisherNamesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WHat have you done frmMain UPDATEDISPLAY() is no goods);");
                    
            }
           
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                frmPublisher.Run(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error adding new publisher");
            }
        }

        private void lstArtists_DoubleClick(object sender, EventArgs e)
        {
            string lcKey;

            lcKey = Convert.ToString(lstPublishers.SelectedItem);
            if (lcKey != null)
                try
                {
                    frmPublisher.Run(lstPublishers.SelectedItem as string);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "This should never occur");
                }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            
            Close();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            string lcKey;

            lcKey = Convert.ToString(lstPublishers.SelectedItem);
            if (lcKey != null && MessageBox.Show("Are you sure?", "Deleting artist", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                try
                {
                    MessageBox.Show(await ServiceClient.DeletePublisherAsync(lcKey));
                    lstPublishers.ClearSelected();
                    UpdateDisplay();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error deleting artist");
                }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
          
            UpdateDisplay();

        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            frmOrders.Instance.Show();
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {

            try
            {
                frmPublisher.Run(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error adding new artist");
            }

        }
    }
}