using System;
using System.Windows.Forms;
using System.Collections.Generic;
namespace BoredGamesWindowsform
{
    public partial class frmPublisher : Form
    {
        private clsPublisher _Publisher;
        private static Dictionary<string, frmPublisher> _PublisherFormList =
            new Dictionary<string, frmPublisher>();

        private frmPublisher()
        {
            InitializeComponent();
        }

        public static void Run(string prPublisherName)

        {
            frmPublisher lcPublisherForm;
            if (string.IsNullOrEmpty(prPublisherName) || !_PublisherFormList.TryGetValue(prPublisherName, out lcPublisherForm))
            {
                lcPublisherForm = new frmPublisher();
                if (string.IsNullOrEmpty(prPublisherName))
                    lcPublisherForm.SetDetails(new clsPublisher());
                else
                {

                    _PublisherFormList.Add(prPublisherName, lcPublisherForm);
                    lcPublisherForm.refreshFormFromDB(prPublisherName);
                }
            }
            else
            {
                lcPublisherForm.Show();
                lcPublisherForm.Activate();

            }

        }

        private async void refreshFormFromDB(string prPublisherName)
        {
            SetDetails(await ServiceClient.GetPublishersAsync(prPublisherName));
        }

        private void updateTitle(string prBoredNames)
        {
            if (!string.IsNullOrEmpty(prBoredNames))
                Text = "Publisher Details - " + prBoredNames;
        }

        private void UpdateDisplay()
        {
            lstGames.DataSource = null;
            if (_Publisher.GamesList != null)
                lstGames.DataSource = _Publisher.GamesList;
        }

        public void UpdateForm()
        {
            txtName.Text = _Publisher.Name;
            txtSpeciality.Text = _Publisher.Email;
        }

        public void SetDetails(clsPublisher prPublisher)
        {
            _Publisher = prPublisher;
            txtName.Enabled = string.IsNullOrEmpty(_Publisher.Name);
            UpdateForm();
            UpdateDisplay();
            Show();
        }

        private void pushData()
        {
            _Publisher.Name = txtName.Text;
            _Publisher.Email = txtSpeciality.Text;
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            string lcReply = new InputBox(clsAllGames.FACTORY_PROMPT).Answer;
            if (!string.IsNullOrEmpty(lcReply)) 
            {
                clsAllGames lcWork = clsAllGames.NewWork(lcReply[0]);
                if (lcWork != null) 
                {
                    if (txtName.Enabled)      
                    {
                        pushData();
                        await ServiceClient.InsertPublisherAsync(_Publisher);
                        txtName.Enabled = false;
                    }
                    lcWork.PublisherName = _Publisher.Name;
                    frmGame.DispatchWorkForm(lcWork);
                    if (!string.IsNullOrEmpty(lcWork.Name)) // not cancelled?
                    {
                        refreshFormFromDB(_Publisher.Name);
                        frmMain.Instance.UpdateDisplay();
                    }
                }
            }

        }

        private void lstWorks_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                frmGame.DispatchWorkForm(lstGames.SelectedValue as clsAllGames);
                UpdateDisplay();
                frmMain.Instance.UpdateDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            int lcIndex = lstGames.SelectedIndex;

            if (lcIndex >= 0 && MessageBox.Show("Are you sure?", "Deleting work", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MessageBox.Show(await ServiceClient.DeleteGamesAsync(lstGames.SelectedItem as clsAllGames));
                refreshFormFromDB(_Publisher.Name);
                frmMain.Instance.UpdateDisplay();

            }
        }

        private async void btnClose_Click(object sender, EventArgs e)
        {
            if (isValid())
                try
                {
                    pushData();

                    if (txtName.Enabled)
                    {
                        MessageBox.Show(await ServiceClient.InsertPublisherAsync(_Publisher));
                        frmMain.Instance.UpdateDisplay();
                        txtName.Enabled = false;
                    }
                    else
                        
                        MessageBox.Show(await ServiceClient.UpdatePublisherAsync(_Publisher));
                    Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private Boolean isValid()
        {
                return true;
        }

        private void rbByDate_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            Hide();
        }
    }
}