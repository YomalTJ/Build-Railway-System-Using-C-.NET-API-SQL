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
using Newtonsoft.Json;
using System.Collections.Generic;
using TrainManage.Models;

namespace TrainManage
{
    public partial class Form1 : Form
    {
        private HttpClient httpClient;
        private const string apiUrl = "https://localhost:7069/swagger/v1/swagger.json";
        public Form1()
        {
            InitializeComponent(); httpClient = new HttpClient();
            dataGridView1.AutoGenerateColumns = true;
            RefreshData();
        }

        public async void RefreshData()
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7069/api/Trains");
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("JSON Response: " + jsonResponse);

                    var trains = JsonConvert.DeserializeObject<List<TrainDto>>(jsonResponse);
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = trains;
                }
                else
                {
                    labelError.Text = "Failed to read data from the API. Status code: " + response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                labelError.Text = "An error occurred: " + ex.Message;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var trainId = int.Parse(textBox1.Text);
                var trainName = textBox2.Text;
                var startLocation = textBox3.Text;
                var endLocation = textBox4.Text;
                var startTime = textBox5.Text;
                var endTime = textBox6.Text;
                var capacity = int.Parse(textBox7.Text);
                var distance = double.Parse(textBox8.Text);
                var price = double.Parse(textBox9.Text);

                var trainData = new
                {
                    trainId,
                    trainName,
                    startLocation,
                    endLocation,
                    startTime,
                    endTime,
                    capacity,
                    distance,
                    price
                };

                var jsonData = JsonConvert.SerializeObject(trainData);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync("https://localhost:7069/api/Trains", content);

                    if (response.IsSuccessStatusCode)
                    {
                        labelError.Text = "Data added successfully!";
                        RefreshData(); // Refresh the data grid view after adding a new train
                    }
                    else
                    {
                        labelError.Text = "Error: " + response.StatusCode + " " + response.ReasonPhrase;
                    }
                }
            }
            catch (Exception ex)
            {
                labelError.Text = "Error: " + ex.Message;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    int selectedIndex = dataGridView1.SelectedRows[0].Index;
                    int trainId = (dataGridView1.Rows[selectedIndex].DataBoundItem as TrainDto)?.TrainId ?? -1;

                    if (trainId != -1)
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            HttpResponseMessage response = await client.DeleteAsync($"https://localhost:7069/api/Trains/{trainId}");

                            if (response.IsSuccessStatusCode)
                            {
                                labelError.Text = "Train deleted successfully!";
                                RefreshData(); // Refresh the data grid view after deleting the train
                            }
                            else
                            {
                                labelError.Text = "Error: " + response.StatusCode + " " + response.ReasonPhrase;
                            }
                        }
                    }
                    else
                    {
                        labelError.Text = "Error: No train selected for deletion.";
                    }
                }
                catch (Exception ex)
                {
                    labelError.Text = "Error: " + ex.Message;
                }
            }
            else
            {
                labelError.Text = "Error: No train selected for deletion.";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new UpdateTrains().Show();
            this.Hide();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
