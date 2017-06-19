using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalBoredGames
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class pgGame : Page
    {
        private clsAllGames _game;
        public pgGame()
        {
            this.InitializeComponent();
            _gamesContent = new Dictionary<string, Delegate>
                {
                {"P", new LoadWorkControlDelegate(RunPartyGame)},
                {"B", new LoadWorkControlDelegate(RunBoardGame)},
};

        }

       
        private delegate void LoadWorkControlDelegate(clsAllGames prGame);
        private Dictionary<string, Delegate> _gamesContent;
        private void dispatchGameContent(clsAllGames prGame)
        {
            _gamesContent[prGame.Type].DynamicInvoke(prGame);
            updatePage(prGame);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            dispatchGameContent(e.Parameter as clsAllGames);
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
        private async void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(txtToBuy.Text) > Convert.ToInt32(txtQuantity.Text))
            {
                lblGameName.Text = "Not enough in stock order less";
            }
            else
            {
                _game.Quantity -= Convert.ToInt32(txtToBuy.Text);
                await ServiceClient.UpdateGameAsync(_game);
                Random rnd = new Random();
                clsOrder lcOrder = new clsOrder();
                lcOrder.Address = txtAdress.Text;
                lcOrder.Date = DateTime.Now;
                lcOrder.Email = txtEmail.Text;
                lcOrder.GameName = _game.Name;
                lcOrder.OrderID = rnd.Next().ToString();
                lcOrder.Price = Convert.ToDecimal(txtPrice.Text);
                lcOrder.Quantity = Convert.ToInt32(txtToBuy.Text);
                await ServiceClient.InsertOrderAsync(lcOrder);

            }

        }
        private void RunPartyGame(clsAllGames prGame)
        {
            ctcGameSpecs.Content = new ucPartyGame();
        }
        private void RunBoardGame(clsAllGames prGame)
        {
            ctcGameSpecs.Content = new ucBoardGame();
        }

        private void updatePage(clsAllGames prGame)
        {
            _game = prGame;

            lblGameName.Text = _game.Name;
            lblpublishersName.Text = _game.PublisherName;

            txtDate.Text = _game.Date.ToString("d");
            txtType.Text = _game.Type.ToString();
            txtPrice.Text = _game.Price.ToString();
            txtQuantity.Text = _game.Quantity.ToString();

            (ctcGameSpecs.Content as IGameControl).UpdateControl(prGame);
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
