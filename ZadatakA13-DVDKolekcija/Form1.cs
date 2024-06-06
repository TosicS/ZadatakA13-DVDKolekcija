using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ZadatakA13_DVDKolekcija
{
    public partial class Form1 : Form
    {
        SqlConnection konekcija = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Skola\MATURA\Programiranje\ZadatakA13-DVDKolekcija\ZadatakA13-DVDKolekcija\A13.mdf;Integrated Security=True;");
        DataTable dt = new DataTable();

        public Form1()
        {
            InitializeComponent();
        }

        public void PrikaziLB()
        {
            string sqlUpit = "Select ProducentID, Ime, Email from Producent";
            SqlCommand komanda = new SqlCommand(sqlUpit, konekcija);
            SqlDataAdapter adapter = new SqlDataAdapter(komanda);

            try
            {
                listBox1.Items.Clear();
                dt.Clear();
                adapter.Fill(dt);

                foreach (DataRow red in dt.Rows)
                {
                    listBox1.Items.Add(string.Format("{0,-7}{1,-40}{2,-30}", red[0], red[1], red[2]));
                }

                DataRow[] sortirani = dt.Select("", "ProducentID ASC");
                DataRow najmanji = sortirani.FirstOrDefault();
                int indMin = dt.Rows.IndexOf(najmanji);
                listBox1.SelectedIndex = indMin;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            labelListBoxPrikaz.Text = string.Format("{0,-7}{1,-40}{2,-30}", "Sifra", "Naziv producenteske kuce", "E-mail adresa");
            PrikaziLB();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0)
            {
                textBoxSifra.Text = dt.Rows[listBox1.SelectedIndex][0].ToString();
                textBoxIme.Text = dt.Rows[listBox1.SelectedIndex][1].ToString();
                textBoxEmail.Text = dt.Rows[listBox1.SelectedIndex][2].ToString();
            }
        }

        private void toolStripButtonIzmeni_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == -1)
            {
                MessageBox.Show("Niste izabrali producenta kog menjate!");
                return;
            }

            if (textBoxIme.Text == "" || textBoxEmail.Text == "")
            {
                MessageBox.Show("Morate da popunite sva polja!");
                return;
            }

            string sqlUpit = "Update Producent set Ime=@parIme, Email=@parEmail where ProducentID=@parSifra";
            SqlCommand komanda = new SqlCommand(sqlUpit, konekcija);
            komanda.Parameters.AddWithValue("@parIme", textBoxIme.Text);
            komanda.Parameters.AddWithValue("@parEmail", textBoxEmail.Text);
            komanda.Parameters.AddWithValue("@parSifra", textBoxSifra.Text);
            int selIndex = listBox1.SelectedIndex;

            try
            {
                konekcija.Open();
                komanda.ExecuteNonQuery();
                MessageBox.Show("Producent je uspesno izmenjen!");
                PrikaziLB();
                listBox1.SelectedIndex = selIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                konekcija.Close();
            }
        }

        private void toolStripButtonAnaliza_Click(object sender, EventArgs e)
        {
            Analiza forma = new Analiza();
            forma.ShowDialog(); // tako pise u resenje
        }

        private void toolStripButtonOAplikaciji_Click(object sender, EventArgs e)
        {
            OAplikaciji forma = new OAplikaciji();
            forma.Show();
        }

        private void toolStripButtonIzlaz_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
