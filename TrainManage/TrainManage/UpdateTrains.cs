using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json;
using TrainManage.Models;

namespace TrainManage
{
    public partial class UpdateTrains : Form
    {
        private const string apiUrl = "https://localhost:7069/api/Trains";
        private List<TrainDto> trains;

        public UpdateTrains()
        {
            InitializeComponent();
        }

        private async void UpdateTrains_Load(object sender, EventArgs e)
        {
            await LoadTrainDetails();
        }

        private async Task LoadTrainDetails()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        trains = JsonConvert.DeserializeObject<List<TrainDto>>(jsonResponse);
                        dataGridView1.DataSource = trains;
                    }
                    else
                    {
                        MessageBox.Show($"Failed to load train details. Status code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                TrainDto selectedTrain = trains[selectedIndex];
                textBoxTrainId.Text = selectedTrain.TrainId.ToString();
                textBoxTrainName.Text = selectedTrain.TrainName;
                textBoxStartLocation.Text = selectedTrain.StartLocation;
                textBoxEndLocation.Text = selectedTrain.EndLocation;
                textBoxStartTime.Text = selectedTrain.StartTime;
                textBoxEndTime.Text = selectedTrain.EndTime;
                textBoxSeats.Text = selectedTrain.Seats.ToString();
                textBoxDistance.Text = selectedTrain.Distance.ToString();
                textBoxPrice.Text = selectedTrain.Price.ToString();
            }
        }

        private async void buttonUpdate_Click(object sender, EventArgs e)
        {
            int selectedIndex = dataGridView1.SelectedRows[0].Index;
            int trainId = trains[selectedIndex].TrainId;
            string trainName = textBoxTrainName.Text;
            string startLocation = textBoxStartLocation.Text;
            string endLocation = textBoxEndLocation.Text;
            string startTime = textBoxStartTime.Text;
            string endTime = textBoxEndTime.Text;
            int seats = int.Parse(textBoxSeats.Text);
            double distance = double.Parse(textBoxDistance.Text);
            double price = double.Parse(textBoxPrice.Text);

            var updatedTrain = new TrainDto
            {
                TrainId = trainId,
                TrainName = trainName,
                StartLocation = startLocation,
                EndLocation = endLocation,
                StartTime = startTime,
                EndTime = endTime,
                Seats = seats,
                Distance = distance,
                Price = price
            };

            var jsonData = JsonConvert.SerializeObject(updatedTrain);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PutAsync($"{apiUrl}/{trainId}", content);
                    response.EnsureSuccessStatusCode();

                    MessageBox.Show("Train data updated successfully!");
                    await LoadTrainDetails();
                    ClearTextBoxes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating train data: {ex.Message}");
            }
        }

        private void ClearTextBoxes()
        {
            textBoxTrainId.Text = "";
            textBoxTrainName.Text = "";
            textBoxStartLocation.Text = "";
            textBoxStartLocation.Text = "";
            textBoxStartTime.Text = "";
            textBoxEndTime.Text = "";
            textBoxSeats.Text = "";
            textBoxDistance.Text = "";
            textBoxPrice.Text = "";
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    int selectedIndex = dataGridView1.SelectedRows[0].Index;
                    int trainId = trains[selectedIndex].TrainId;
                    string trainName = textBoxTrainName.Text;
                    string startLocation = textBoxStartLocation.Text;
                    string endLocation = textBoxEndLocation.Text;
                    string startTime = textBoxStartTime.Text;
                    string endTime = textBoxEndTime.Text;
                    int seats = int.Parse(textBoxSeats.Text);
                    double distance = double.Parse(textBoxDistance.Text);
                    double price = double.Parse(textBoxPrice.Text);

                    var updatedTrain = new TrainDto
                    {
                        TrainId = trainId,
                        TrainName = trainName,
                        StartLocation = startLocation,
                        EndLocation = endLocation,
                        StartTime = startTime,
                        EndTime = endTime,
                        Seats = seats,
                        Distance = distance,
                        Price = price
                    };

                    var jsonData = JsonConvert.SerializeObject(updatedTrain);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.PutAsync($"{apiUrl}/{trainId}", content);
                        response.EnsureSuccessStatusCode();

                        MessageBox.Show("Train data updated successfully!");
                        await LoadTrainDetails();
                        ClearTextBoxes();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating train data: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please select a train from the list.");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            this.Hide();
        }
    }
}
