using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;
using Railway_Application.Models;
using System.Net.Http;

namespace Railway_Application
{
    public partial class Home : Form
    {
        private HttpClient httpClient;
        private const string apiUrl = "https://localhost:7069/api/Trains";
        private List<TrainsDto> allTrains;

        public Home()
        {
            InitializeComponent();
            httpClient = new HttpClient();
            RefreshData();
        }

        private async void RefreshData(string searchQuery = "")
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    allTrains = JsonConvert.DeserializeObject<List<TrainsDto>>(jsonResponse);

                    List<TrainsDto> filteredTrains = allTrains;
                    if (!string.IsNullOrEmpty(searchQuery))
                    {
                        filteredTrains = allTrains.Where(train => train.TrainName.ToLower().Contains(searchQuery.ToLower())).ToList();
                    }

                    PopulateDataGridView(filteredTrains);
                }
                else
                {
                    MessageBox.Show($"Failed to fetch train details. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching train details: {ex.Message}");
            }
        }

        private void PopulateDataGridView(List<TrainsDto> trains)
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = trains;
        }

        private void Home_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string searchQuery = textBoxSearch.Text;
            RefreshData(searchQuery);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            textBoxSearch.Text = "";
            RefreshData();
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the selected train's name and price
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                string trainName = dataGridView1.Rows[selectedIndex].Cells["TrainName"].Value.ToString();
                string trainPrice = dataGridView1.Rows[selectedIndex].Cells["Price"].Value.ToString();

                // Display a custom message box
                string message = $"You have booked the train: {trainName}\nPrice: {trainPrice}";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, "Booking Confirmation", buttons);
            }
            else
            {
                MessageBox.Show("Please select a train to book.");
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            new Login_Form().Show();
            this.Hide();
        }
    }
}
