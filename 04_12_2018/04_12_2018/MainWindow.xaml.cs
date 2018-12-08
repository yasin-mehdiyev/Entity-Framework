using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using _04_12_2018.Models;

namespace _04_12_2018
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FillCombo();
        }

        private void FillCombo()
        {
            cmbCities.Items.Add(new City
            {
                Key="Baku",
                Name="Bakı"
            });

            cmbCities.Items.Add(new City
            {
                Key = "Sumgait",
                Name = "Sumqayıt"
            });

            cmbCities.Items.Add(new City
            {
                Key = "Ganja",
                Name = "Gəncə"
            });

            cmbCities.Items.Add(new City
            {
                Key = "Shaki",
                Name = "Şəki"
            });

            cmbCities.Items.Add(new City
            {
                Key = "Gabala",
                Name = "Qəbələ"
            });
        }

        private void cmbCities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCities.SelectedItem==null)
            {
                return;
            }

            City city = cmbCities.SelectedItem as City;

            var client = new RestClient("http://api.aladhan.com/v1/timingsByCity");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest(RestSharp.Method.GET);
            request.AddParameter("city", city.Key); // adds to POST or URL querystring based on Method
            request.AddParameter("country", "Azerbaijan");
            request.AddParameter("method", 8);

            // execute the request
            IRestResponse response = client.Execute(request);

            ResPrayer res = JsonConvert.DeserializeObject<ResPrayer>(response.Content);

            lblFajr.Content = res.data.timings.Fajr;
            lblAsr.Content = res.data.timings.Asr;
            lblDhuhr.Content = res.data.timings.Dhuhr;
            lblIsha.Content = res.data.timings.Isha;
            lblMaghrib.Content = res.data.timings.Maghrib;

        }
    }
}
