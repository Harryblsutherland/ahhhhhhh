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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UniversalBoredGames
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class pgMain : Page
    {
        public pgMain()
        {
            this.InitializeComponent();
        }
        private void EditPublisher()
        {
            if (lstPublishers.SelectedItem != null)
                Frame.Navigate(typeof(pgPublisher), lstPublishers.SelectedItem);
        }
        private async void pgMain1_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                lstPublishers.ItemsSource = await ServiceClient.GetPublisherNamesAsync();

            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.GetBaseException().ToString();
                throw;
            }
        }
        private void lstPublishers_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            EditPublisher();
        }
    }
}
