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
    public sealed partial class pgPublisher : Page
    {
        private clsPublisher _Publisher;
        public pgPublisher()
        {
            this.InitializeComponent();
        }

        private void UpdateDisplay()
        {
            lblPublisherTitle.Text = _Publisher.Name;
            lstGames.ItemsSource = _Publisher.GamesList;
        }
        private void ViewGame(clsAllGames prGame)
        {
            Frame.Navigate(typeof(pgGame), prGame);
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null)
            {
                string lcPublisherName = e.Parameter.ToString();
                _Publisher = await ServiceClient.GetPublishersAsync(lcPublisherName);
                UpdateDisplay();
            }
            else // no parameter -> new artist!
                _Publisher = new clsPublisher();
        }

        private void lstGames_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            ViewGame(lstGames.SelectedItem as clsAllGames);
        }
    }
}
