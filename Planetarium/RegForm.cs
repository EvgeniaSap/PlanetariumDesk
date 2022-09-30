using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;

namespace Planetarium
{
    public partial class RegForm : Form
    {
        private AuthForm _authForm; //Форма авторизации;
        private AdminMainForm _adminMainForm; //Форма главного меню для администратора;
        string _selectedPosit;

        public RegForm()
        {
            InitializeComponent();
        }

        public RegForm(AuthForm authForm)
        {
            _authForm = authForm;

            InitializeComponent();

            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        private void button1_Click(object sender, EventArgs e) //Кнопка "Зарегистрироваться"
        {
            if (!string.IsNullOrEmpty(textBox1.Text)) //Проверка введенного ФИО;
            {
                if (!string.IsNullOrEmpty(_selectedPosit)) //Проверка выбранной должности;
                {
                    if (!string.IsNullOrEmpty(textBox2.Text)) //Проверка введенного логина;
                    {
                        if (!string.IsNullOrEmpty(textBox3.Text)) //Проверка введенного пароля;
                        {
                            if (!string.IsNullOrEmpty(textBox4.Text)) //Проверка повторно введенного пароля;
                            {
                                if (Convert.ToString(textBox3.Text)== Convert.ToString(textBox4.Text)) //Проверка на совпадение введенных паролей
                                {
                                    
                                    MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

                                    //conn.Open();
                                    try
                                    {
                                        conn.Open();
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Отсутствует соединение с сервером");
                                    }

                                    string sql = "SELECT  login " +
                                        "FROM account " +
                                          "WHERE login = @log";

                                    MySqlCommand command = new MySqlCommand(sql, conn);
                                    command.Parameters.Add("@log", MySqlDbType.VarChar).Value = Convert.ToString(textBox2.Text);

                                    MySqlDataReader user = command.ExecuteReader();

                                    if (user.Read())
                                    {
                                        MessageBox.Show("Пользователь с таким логином уже существует. Придумайте другой логин.");
                                        user.Close();
                                        conn.Close();
                                    }
                                    else
                                    {
                                        user.Close();
                                        conn.Close();
                                        //Хеширование пароля
                                        string passw = Convert.ToString(textBox3.Text) + "iutYr1dcv2b7dsCv46blg2fhD";
                                        byte[] asciiBytes = ASCIIEncoding.ASCII.GetBytes(passw);
                                        byte[] hashedBytes = MD5CryptoServiceProvider.Create().ComputeHash(asciiBytes);
                                        string hashedPass = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                                        conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

                                        //Создание новой учетной записи
                                        sql = "INSERT INTO account (id_account_type,login,password,status) VALUES ('3',@log,@pass,'0')";
                                        command = new MySqlCommand(sql, conn);

                                        command.Parameters.AddWithValue("@log", Convert.ToString(textBox2.Text));
                                        command.Parameters.AddWithValue("@pass", hashedPass);

                                        command.Connection.Open();
                                        command.ExecuteNonQuery();
                                        command.Connection.Close();


                                        conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

                                        conn.Open();
                                        sql = "SELECT  id_account, id_account_type " +
                                       "FROM account " +
                                         "WHERE login = @log AND password = @pass";

                                        command = new MySqlCommand(sql, conn);
                                        command.Parameters.Add("@log", MySqlDbType.VarChar).Value = Convert.ToString(textBox2.Text);
                                        command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = hashedPass;

                                        MySqlDataReader newuser = command.ExecuteReader(); //Только что добавленный аккаунт сотрудника

                                        if (newuser.Read())
                                        {
                                            string newuser_id = newuser[0].ToString();
                                            string newuser_id_type = newuser[1].ToString();
                                            newuser.Close();
                                            conn.Close();

                                            conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                                            conn.Open();
                                            sql = "SELECT  id_position " +
                                            "FROM position " +
                                         "WHERE name_position = @pos";

                                            command = new MySqlCommand(sql, conn);
                                            command.Parameters.Add("@pos", MySqlDbType.VarChar).Value = _selectedPosit;
                                            MySqlDataReader posit = command.ExecuteReader();

                                            if (posit.Read())
                                            {
                                                string posit_id = posit[0].ToString();
                                                posit.Close();
                                                conn.Close();

                                                conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                                                                                  //Вставка нового сотрудника
                                                sql = "INSERT INTO employee (id_position,id_account,full_name) VALUES (@pos,@acc,@name)";
                                                command = new MySqlCommand(sql, conn);

                                                command.Parameters.AddWithValue("@pos", posit_id); //value combobox
                                                command.Parameters.AddWithValue("@acc", newuser_id);
                                                command.Parameters.AddWithValue("@name", Convert.ToString(textBox1.Text));

                                                command.Connection.Open();
                                                command.ExecuteNonQuery();
                                                command.Connection.Close();

                                                if (newuser_id_type == "1")
                                                {
                                                    this.Close();
                                                    _adminMainForm = new AdminMainForm(_authForm, this) { Visible = true }; //Переход в форму регистрации;
                                                }
                                                else
                                                {
                                                    this.Close();
                                                    _authForm.Visible = true; //Переход к форме авторизации;
                                                }


                                                textBox1.Clear();
                                                textBox2.Clear();
                                                textBox3.Clear();

                                            }
                                            else
                                            {
                                                posit.Close();
                                                conn.Close();
                                                MessageBox.Show("Выбранная должность не найдена");
                                            }
                                            //  posit.Close();
                                        }
                                        else
                                        {
                                            MessageBox.Show("только что добавленный акк не найден");
                                            newuser.Close();
                                            conn.Close();

                                        }
                                        // newuser.Close();
                                    }

                                    // user.Close();
                                    conn.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Повторно введенный пароль не совпадает.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Повторно введите пароль.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Введите пароль.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введите логин.");
                    }
                }
                else
                {
                    MessageBox.Show("Выберите Вашу должность.");
                }
            }
            else
            {
                MessageBox.Show("Введите Ваше ФИО.");
            }
        }

        private void button2_Click(object sender, EventArgs e) //Кнопка "Назад"
        {
            this.Close();
            _authForm.Visible = true; //Переход к форме авторизации;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedPosit = comboBox1.SelectedItem.ToString();
           // MessageBox.Show(selectedPosit);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*char фио = e.KeyChar; //Символьная переменная, e.KeyChar - параметр;

            if (Char.IsDigit(фио) && фио != 240 && фио != 8) //цифра, не пробел и не клавиша BackSpace, используя коды ASCII;
            {
                e.Handled = true; //Тогда не обрабатывать введенный символ (и, следовательно, не выводить его в TextBox1);
            }*/

            if (Char.IsLetter(e.KeyChar) | e.KeyChar == 8 | e.KeyChar == 32) return;
            else
                e.Handled = true;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) | Char.IsLetter(e.KeyChar) | e.KeyChar == 8) return;
            else
                e.Handled = true;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 32) return;
            else
                e.Handled = true;
        }

        private void RegForm_Load(object sender, EventArgs e)
        {
            MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

            conn.Open();
            string sql = "SELECT  name_position " +
                "FROM position ";

            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader posit = command.ExecuteReader();

            while (posit.Read())
            {
                comboBox1.Items.Add(posit[0].ToString());
            }
            posit.Close();
            conn.Close();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 32) return;
            else
                e.Handled = true;
        }
    }
}
