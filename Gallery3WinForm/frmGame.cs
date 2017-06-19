using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BoredGamesWindowsform
{
    public partial class frmGame : Form
    {
        protected clsAllGames _Game;

        public frmGame()
        { 
            InitializeComponent();
        }
        public delegate void LoadWorkFormDelegate(clsAllGames prWork);
        public static Dictionary <string, Delegate> _WorksForm = new Dictionary <string, Delegate>
        {
                    {"P", new LoadWorkFormDelegate(frmPartyGame.Run)},
                    {"B", new LoadWorkFormDelegate(frmBoardGame.Run)}
        };
        public static void DispatchWorkForm(clsAllGames prGame)
        {
            _WorksForm[prGame.Type].DynamicInvoke(prGame);
        }

        public void SetDetails(clsAllGames prGame)
        {
            _Game = prGame;
            updateForm();
            ShowDialog();
        }

        private async void btnOK_Click(object sender, EventArgs e)
        {
            if (isValid())
            {
                pushData();
                if (txtName.Enabled)
                    MessageBox.Show(await ServiceClient.InsertGameAsync(_Game));
                else
                    MessageBox.Show(await ServiceClient.UpdateGameAsync(_Game));
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected virtual bool isValid()
        {
            return true;
        }

        protected virtual void updateForm()
        {
            txtName.Enabled = string.IsNullOrEmpty(_Game.Name);
            txtName.Text = _Game.Name;
            txtReleaseDate.Text = _Game.Date.ToShortDateString();
            txtPublisher.Text = _Game.PublisherName.ToString();
            txtPrice.Text = _Game.Price.ToString();
            txtQuantity.Text = _Game.Quantity.ToString();
            txtType.Text = _Game.Type.ToString();
            
        }

        protected virtual void pushData()
        {
            _Game.Name = txtName.Text;
            _Game.PublisherName = txtPublisher.Text;
            _Game.Date = DateTime.Parse(txtReleaseDate.Text);
            _Game.Price = Convert.ToInt32(txtPrice.Text);
            _Game.Quantity = Convert.ToInt32(txtQuantity.Text);
            _Game.Type = txtType.Text;
        }


    }
}