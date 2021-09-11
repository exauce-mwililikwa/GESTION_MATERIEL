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
using System.IO;

namespace gestionmateriel
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        //executer les requete a distance
        SqlCommand cmd;
        public static string imgLon = "";
        public Form1()
        {
            InitializeComponent();
            
        }
        public void chargement()
        {
            try {
                //ouvrons notre connexion
                con.Open();
                cmd = con.CreateCommand();
                //creons une instance de la classe etudiant
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ETUDIANT",con);
                //gardons les donnees en memoire
                DataSet ds = new DataSet();
                da.Fill(ds, "ETUDIANT");
                //chargeons dans un control appeler datagridview
                dataGridView1.DataSource = ds.Tables[0];
                
                con.Close();
            }
            catch (Exception ex) {
                MessageBox.Show("erreur : "+ex);
            }
        }
        public void RECHERCHERt()
        {
            try
            {
                //ouvrons notre connexion
                con.Open();
                cmd = con.CreateCommand();
                //creons une instance de la classe etudiant
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ETUDIANT WHERE CONCAT(NOM,' ',POSTNOM,' ',PRENOM) LIKE '%"+textBox1.Text+"%'", con);
                //gardons les donnees en memoire
                DataSet ds = new DataSet();
                da.Fill(ds, "ETUDIANT");
                //chargeons dans un control appeler datagridview
                dataGridView1.DataSource = ds.Tables[0];

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("erreur : " + ex);
            }
        }
        public void inserer()
        {
            try {
                //transformons notre photo en tableau de byte
                MemoryStream ms = new MemoryStream();
                pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] photo = new Byte[ms.Length];
                ms.Position = 0;
                ms.Read(photo, 0, photo.Length);
                //ouvrons notre connection
                con.Open();
                //executons une requette sur le serveur
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                //requette sql
                cmd.CommandText = "INSERT INTO ETUDIANT (NOM,POSTNOM,PRENOM,GENRE,DATENAISSANCE,PHOTO) VALUES(@NOM,@POSTNOM,@PRENOM,@GENRE,@DATENAISSANCE,@PHOTO)";
                cmd.Parameters.AddWithValue("@nom", NOM.Text);
                cmd.Parameters.AddWithValue("@postnom", POSTNOM.Text);
                cmd.Parameters.AddWithValue("@PRENOM",PRENOM.Text);
                cmd.Parameters.AddWithValue("@GENRE", SEXE.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@DATENAISSANCE", dateTimePicker1.Value.ToString());
                cmd.Parameters.AddWithValue("@PHOTO", photo);
                //executons la requete
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("insertion reussi");
                //actualisons notre DataGridView
                chargement();
               

            }
            catch (Exception ex) {
                MessageBox.Show(""+ex);
            }
        }
        public void modifier()
        {
            try
            {
                //transformons notre photo en tableau de byte
                MemoryStream ms = new MemoryStream();
                pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] photo = new Byte[ms.Length];
                ms.Position = 0;
                ms.Read(photo, 0, photo.Length);
                //ouvrons notre connection
                con.Open();
                //executons une requette sur le serveur
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                //requette sql
                cmd.CommandText = "update ETUDIANT set NOM=@NOM,POSTNOM=@POSTNOM,PRENOM=@PRENOM,GENRE=@GENRE,DATENAISSANCE=@DATENAISSANCE,PHOTO=@PHOTO where MATRICULE=@MATRICULE";
                cmd.Parameters.AddWithValue("@nom", NOM.Text);
                cmd.Parameters.AddWithValue("@postnom", POSTNOM.Text);
                cmd.Parameters.AddWithValue("@PRENOM", PRENOM.Text);
                cmd.Parameters.AddWithValue("@GENRE", SEXE.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@DATENAISSANCE", dateTimePicker1.Value.ToString());
                cmd.Parameters.AddWithValue("@PHOTO", photo);
                cmd.Parameters.AddWithValue("@MATRICULE", int.Parse(MATRICULE.Text));
                //executons la requete
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("MODIFICATION reussi");
                //actualisons notre DataGridView
                chargement();


            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try {
                String chemin_d_acces = string.Format(@"Data Source=EXAUCE\SA;Initial Catalog=GESTIONMATERIELS;User ID=sa;Password=aliconnorecho");
                con = new SqlConnection(chemin_d_acces);
                chargement();
                // this.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("mamaeeeeeeee!"+ex.Message);
            } 
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            inserer();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {  
            try {
                OpenFileDialog dl = new OpenFileDialog();
              dl.InitialDirectory=@"C:\Users\EXAUCE\Pictures";
                dl.FilterIndex = 2;
                dl.Filter = "JPEG |*.jpg";
                if (dl.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(dl.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(""+ex);
            }
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                MATRICULE.Text = row.Cells[0].Value.ToString();
                NOM.Text = row.Cells[1].Value.ToString();
                POSTNOM.Text = row.Cells[2].Value.ToString();
                PRENOM.Text = row.Cells[3].Value.ToString();
                SEXE.SelectedItem = row.Cells[4].Value.ToString();
                // PRENOM.Text = row.Cells[3].Value.ToString();
               dateTimePicker1.Text = row.Cells[5].Value.ToString();
                MemoryStream ms = new MemoryStream();

                byte[] picture = (byte[])row.Cells[6].Value;
                ms.Write(picture, 0, picture.Length);
                pictureBox1.Image = new System.Drawing.Bitmap(ms);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            modifier();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            RECHERCHERt();    
        }
    }
}
