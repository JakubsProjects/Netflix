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



//
// Netflix Database Application using N-Tier Design.
//
// <<Jakub Glebocki>>
// U. of Illinois, Chicago
// CS341, Spring 2018
// Project 08
//

namespace Project8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //private void Form1_Load(object sender, EventArgs e)
        //{
        //    this.clearForm();
        //}

        private bool fileExists(string filename)
        {
            if (!System.IO.File.Exists(filename))
            {
                string msg = string.Format("Input file not found: '{0}'",
                  filename);

                MessageBox.Show(msg);
                return false;
            }

            // exists!
            return true;
        }


      

        private void button1_Click(object sender, EventArgs e)
        {
            string filename = this.textBox1.Text;

            if (!fileExists(filename))
                return;

           

            string version, connectionInfo;
            SqlConnection db;
            version = "MSSQLLocalDB";
            connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};
            AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);
            db = new SqlConnection(connectionInfo);
            db.Open();

            string sql = string.Format(@"Select MovieName FROM Movies ORDER BY MovieName ASC;");

            //MessageBox.Show(sql);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            cmd.CommandText = sql;
            adapter.Fill(ds);

            foreach (DataRow row in ds.Tables["TABLE"].Rows)
            {
              string msg = string.Format("{0}", Convert.ToString(row["MovieName"]));
              this.listBox1.Items.Add(msg);
            }



            db.Close();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.listBox2.Items.Clear();
            this.listBox5.Items.Clear();
            string dbfilename = this.textBox1.Text;
            BusinessTier.Business biztier = new BusinessTier.Business(dbfilename);
            BusinessTier.Movie movie = biztier.GetMovie(this.listBox1.Text);

            BusinessTier.MovieDetail AvgMovieRating = biztier.GetMovieDetail(movie.MovieID);


           // BusinessTier.Review review1 = biztier.
            if (movie == null || AvgMovieRating == null)
            {
                MessageBox.Show("ERROR, movie not found");
            }
            else
            {
                //MessageBox.Show(Convert.ToString(movie.MovieID));
                string msg = string.Format(@"MovieID is: {0}", movie.MovieID);
                string msg2 = string.Format(@"Average Movie Rating is: {0}", AvgMovieRating.AvgRating);
                this.listBox2.Items.Add(msg);
                this.listBox2.Items.Add(msg2);
            

            //Step 4, Get reviews for that particular movie

            this.listBox5.Items.Add(movie.MovieName);
            this.listBox5.Items.Add("\n");
            foreach (BusinessTier.Review a in AvgMovieRating.Reviews)
            {
                
                if (movie.MovieID == a.MovieID)
                {
                    string msg3 = string.Format(@"{0}: {1}", a.UserID, a.Rating);
                    this.listBox5.Items.Add(msg3);
                }
            }
           }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.listBox3.Items.Clear();

            string dbfilename = this.textBox1.Text;
            BusinessTier.Business biztier = new BusinessTier.Business(dbfilename);
            IReadOnlyList<BusinessTier.User> ManyUsers = biztier.GetAllNamedUsers();

            
            foreach (BusinessTier.User a in ManyUsers)
            {
                string msg = string.Format("{0}", Convert.ToString(a.UserName));
                this.listBox3.Items.Add(msg);
            }
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.listBox4.Items.Clear();
            this.listBox6.Items.Clear();
            string dbfilename = this.textBox1.Text;
            BusinessTier.Business biztier = new BusinessTier.Business(dbfilename);
            BusinessTier.User SingleUser = biztier.GetNamedUser(this.listBox3.Text);
            BusinessTier.UserDetail SinguleUserInfo = biztier.GetUserDetail(SingleUser.UserID);
            
            if(SinguleUserInfo == null)
            {
                MessageBox.Show("Error");
            }
            else { 
            this.listBox4.Items.Add("User ID is: " + Convert.ToString(SingleUser.UserID));
            this.listBox4.Items.Add("User Occup. is: " + Convert.ToString(SingleUser.Occupation));

            this.listBox6.Items.Clear();


            // BusinessTier.User UserReviews = biztier2.GetUserDetail(SingleUser.UserID);
            //Step 5
            this.listBox6.Items.Add(SingleUser.UserName);
            this.listBox6.Items.Add("\n");
            foreach (BusinessTier.Review a in SinguleUserInfo.Reviews)
            {

                BusinessTier.Movie movie = biztier.GetMovie(a.MovieID);
                string msg = string.Format("'{0}' -> '{1}'", Convert.ToString(movie.MovieName), Convert.ToString(a.Rating));
                this.listBox6.Items.Add(msg);
            }
           }
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.listBox7.Items.Clear();
            string dbfilename = this.textBox1.Text;
            BusinessTier.Business biztier = new BusinessTier.Business(dbfilename);

            //if (textBox2.Text.Length == null || textBox2.Text.Length)
            //{
               
            //}

            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Movie textbox is empty");
                return;
            }
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("User name textbox is empty");
                return;
            }
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Rating textbox is empty");
                return;
            }

            string MovieName = (string)textBox2.Text;
            string username = (string)textBox3.Text;
            int RateofMovie = Convert.ToInt32(textBox4.Text);

            BusinessTier.User UserSave = biztier.GetNamedUser(username);
            BusinessTier.Movie MovieSave = biztier.GetMovie(MovieName);

            if(UserSave == null)
            {
                this.listBox7.Items.Add("User Doesn't exist");
            }
            else
            {
                if (MovieSave == null)
                {
                    this.listBox7.Items.Add("Movie Name Doesn't Exist");
                }
                else
                {
                    if(RateofMovie >= 0 && RateofMovie <= 5)
                    {
                        biztier.AddReview(MovieSave.MovieID, UserSave.UserID, RateofMovie);
                        this.listBox7.Items.Add("Successfully Added Rating!");
                    }
                    else
                    {
                        this.listBox7.Items.Add("Rating Out of Bounds");
                    }
                }
            }
            this.textBox2.Clear();
            this.textBox3.Clear();
            this.textBox4.Clear();
        }

        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.listBox8.Items.Clear();
            string dbfilename = this.textBox1.Text;
            BusinessTier.Business biztier = new BusinessTier.Business(dbfilename);

            if (string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show("No Movie Name in textbox");
                return;
            }

            string MovieName = this.textBox5.Text;
            BusinessTier.Movie MovieInput = biztier.GetMovie(MovieName);

            
            if(MovieName == null)
            {
                this.listBox8.Items.Add("Sorry, Invalid Movie.");
            }
            else
            {
                this.listBox8.Items.Add(MovieName);

                BusinessTier.MovieDetail DetailsOfMovie = biztier.GetMovieDetail(MovieInput.MovieID);

                int review1 = 0;
                int review2 = 0;
                int review3 = 0;
                int review4 = 0;
                int review5 = 0;

                int current = 0;
                foreach(BusinessTier.Review a in DetailsOfMovie.Reviews)
                {
                    current = a.Rating;
                    if (current == 1)
                    {
                        review1 = review1 +1;
                    }  
                    else if (current == 2)
                    {
                        review2 = review2 + 1;
                    }
                    else if(current == 3)
                    {
                        review3 = review3 + 1;
                    }
                    else if(current == 4)
                    {
                        review4 = review4 + 1;
                    }
                    else if(current == 5)
                    {
                        review5 = review5 + 1;
                    }
                    

                }

                string totalRatings = string.Format(@"total: {0}", DetailsOfMovie.NumReviews);
                string ratings5 = string.Format(@"5: {0}", review5);
                string ratings4 = string.Format(@"4: {0}", review4);
                string ratings3 = string.Format(@"3: {0}", review3);
                string ratings2 = string.Format(@"2: {0}", review2);
                string ratings1 = string.Format(@"1: {0}", review1);

                this.listBox8.Items.Add("\n");
                this.listBox8.Items.Add(ratings5);
                this.listBox8.Items.Add(ratings4);
                this.listBox8.Items.Add(ratings3);
                this.listBox8.Items.Add(ratings2);
                this.listBox8.Items.Add(ratings1);
                this.listBox8.Items.Add("\n");
                this.listBox8.Items.Add(totalRatings);
            }
            this.textBox5.Clear();
        }

        private void listBox8_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void listBox9_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.listBox9.Items.Clear();
            //

            string dbfilename = this.textBox1.Text;
            BusinessTier.Business biztier = new BusinessTier.Business(dbfilename);
            
            if (string.IsNullOrEmpty(textBox6.Text))
            {
                MessageBox.Show("No 'N' input in textbox");
                return;
            }

            int Nmovies = Convert.ToInt32(this.textBox6.Text);

          //  

            if (!int.TryParse(textBox6.Text, out Nmovies))
            {
                this.textBox6.Clear();
                this.listBox9.Items.Add("Please enter Number Only!!! 1-10!!!");
            }
            else
            {
                this.textBox6.Clear();
                IReadOnlyList< BusinessTier.Movie> NnumMovies = biztier.GetTopMoviesByAvgRating(Nmovies);

                foreach(BusinessTier.Movie a in NnumMovies)
                {
                    BusinessTier.MovieDetail DetailsOfMovie = biztier.GetMovieDetail(a.MovieID);
                    this.listBox9.Items.Add(string.Format("{0}: {1}", a.MovieName, DetailsOfMovie.AvgRating));
                }
              
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            this.listBox2.Items.Clear();
            this.listBox5.Items.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.listBox3.Items.Clear();
            this.listBox4.Items.Clear();
            this.listBox6.Items.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.listBox7.Items.Clear();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.listBox8.Items.Clear();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.listBox9.Items.Clear();
        }
    }
}
