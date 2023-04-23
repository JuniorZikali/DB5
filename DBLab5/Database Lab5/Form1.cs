using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Database_Lab5
{
    public partial class Form1 : Form
    {
        private MySqlConnection conn;
        private string server;
        private string database;
        private string uid;
        private string password;
        public Form1()
        {
            server = "localhost";
            database = "lab5";
            uid = "root";
            password = " ";

            string connString;
            connString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";

            conn = new MySqlConnection(connString);

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            string user = tbUser.Text;
            string pass = tbPass.Text;
            if (user.Length == 0 || pass.Length == 0)
            {
                MessageBox.Show("Enter all fields");
            }
            else
            {
                if (Register(user, pass))
                {
                    MessageBox.Show($"User {user} has been created");
                }
                else
                {
                    MessageBox.Show($"User {user} has not been created");
                }
            }
            
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = tbUser.Text;
            string pass = tbPass.Text;
            
            if (user.Length == 0 || pass.Length == 0)
            {
                MessageBox.Show("Enter in all fileds");
                
            }
            else
            {
                if (IsLogin(user, pass))
                {
                    MessageBox.Show($"Welcome User {user}");
                    main_form();
                }
                else
                {
                    MessageBox.Show($"Failed to login User {user}");
                }
                
            }
           
        }
        public bool Register(string user, string pass)
        {
            string query = $"INSERT INTO login (username,password) VALUES ('{user}','{pass}');";

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                else
                {
                    conn.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                conn.Close ();
                return false;
            }
        }
        public bool IsLogin(string user , string pass)
        {
            string query = $"SELECT * FROM login WHERE username='{user}' AND password='{pass}';";
          
            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        reader.Close();
                        conn.Close();
                        return true;
                    }
                    else
                    {
                        reader.Close();
                        conn.Close();
                        return false;
                    }
                }
                else
                {
                    conn.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                return false;
            }

        }
        private bool OpenConnection()
        {
            try
            {
                conn.Open();
                return true;

            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Connection to server has failed");
                        break;
                    case 1045:
                        MessageBox.Show("Server username or password is incorrect");
                        break;

                }
                return false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbPass.Text = string.Empty;
            tbUser.Text = string.Empty;
        }

        private void main_form()
        {
            this.Hide();
            Form2 form2 = new Form2();
            form2.ShowDialog();
            this.Close();
        }
    }
}
