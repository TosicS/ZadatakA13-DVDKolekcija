using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZadatakA13_DVDKolekcija
{
    public partial class Analiza : Form
    {
        SqlConnection konekcija = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Skola\MATURA\Programiranje\ZadatakA13-DVDKolekcija\ZadatakA13-DVDKolekcija\A13.mdf;Integrated Security=True;");

        public Analiza()
        {
            InitializeComponent();
        }

        private void buttonPrikazi_Click(object sender, EventArgs e)
        {
            string sqUpit = "Select Producent.Ime as Producent, count(Producirao.FilmID) as 'Broj filmova' from Producent " +
                "inner join Producirao on Producirao.ProducentID = Producent.ProducentID " +
                "group by Producent.Ime";

            SqlCommand komanda = new SqlCommand(sqUpit, konekcija);
            SqlDataAdapter adapter = new SqlDataAdapter(komanda);
            DataTable dt = new DataTable();

            try
            {
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;

                chart1.DataSource = dt;
                chart1.Series[0].XValueMember = "Producent";
                chart1.Series[0].YValueMembers = "Broj filmova";
                chart1.Series[0].IsValueShownAsLabel = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonIzadji_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
