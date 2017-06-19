using System;
using System.Windows.Forms;
using System.Collections.Generic;
namespace BoredGamesWindowsform
{
    public partial class frmOrderData : Form
    {
        private clsOrder _OrderData;
        private static Dictionary<string, frmOrderData> _OrderFormList = new Dictionary<string, frmOrderData>();

        private frmOrderData()
        {
            InitializeComponent();
        }

        public static void Run(string prOrderDataID)

        {
            frmOrderData lcOrderDataForm;
            if (string.IsNullOrEmpty(prOrderDataID) || !_OrderFormList.TryGetValue(prOrderDataID, out lcOrderDataForm))
            {
                lcOrderDataForm = new frmOrderData();
                if (string.IsNullOrEmpty(prOrderDataID))
                    lcOrderDataForm.SetDetails(new clsOrder());
                else
                {
                    _OrderFormList.Add(prOrderDataID, lcOrderDataForm);
                    lcOrderDataForm.refreshFormFromDB(prOrderDataID);
                }
            }
            else
            {
                lcOrderDataForm.Show();
                lcOrderDataForm.Activate();
            }

        }

        private async void refreshFormFromDB(string prOrderID)
        {
            SetDetails(await ServiceClient.GetOrderAsync(prOrderID));
        }

        private void updateTitle(string prBoredNames)
        {
            if (!string.IsNullOrEmpty(prBoredNames))
                Text = "Order Details - " + prBoredNames;
        }
        public void UpdateForm()
        {
            txtOrderID.Text = _OrderData.OrderID;
            txtEmail.Text = _OrderData.Email;
            txtDate.Text = _OrderData.Date.ToShortDateString();
            txtAddress.Text = _OrderData.Address;
            txtQuantity.Text = _OrderData.Quantity.ToString();
            txtPrice.Text = _OrderData.Price.ToString();
            txtGameName.Text = _OrderData.GameName;
        }
        public void SetDetails(clsOrder prOrder)
        {
            _OrderData = prOrder;
            UpdateForm();
            Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}