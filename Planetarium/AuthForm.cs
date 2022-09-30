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
    public partial class AuthForm : Form
    {
        private RegForm _regForm; //Форма регистрации;
        private AdminMainForm _adminMainForm; //Форма главного меню для администратора;
        private UserAccForm _userAccForm; //Форма личного кабинета сотрудника;

        public AuthForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) //Ввод логина
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e) //Ввод пароля
        {
            
        }
        
        private void button1_Click(object sender, EventArgs e) //Кнопка "Войти"
        {
            if (!string.IsNullOrEmpty(textBox1.Text)) //Проверка введенного логина;
            {
                if (!string.IsNullOrEmpty(textBox2.Text)) //Проверка введенного пароля;
                {
                    //Хеширование пароля
                    string passw = Convert.ToString(textBox2.Text) + "iutYr1dcv2b7dsCv46blg2fhD";
                    byte[] asciiBytes = ASCIIEncoding.ASCII.GetBytes(passw);
                    byte[] hashedBytes = MD5CryptoServiceProvider.Create().ComputeHash(asciiBytes);
                    string hashedPass = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

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

                    string sql = "SELECT  id_account, id_account_type " +
                        "FROM account " +
                          "WHERE login = @log  AND password = @pass";

                 //   string sql = String.Format("SELECT id_account FROM account WHERE login = '{0}'", Convert.ToString(textBox1.Text));

                    MySqlCommand command = new MySqlCommand(sql, conn);
                    command.Parameters.Add("@log", MySqlDbType.VarChar).Value = Convert.ToString(textBox1.Text);
                    command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = hashedPass;
                    MySqlDataReader user = command.ExecuteReader();
                    

                    if (user.Read())
                    {
                        if(user[1].ToString() == "1")
                        {
                            this.Visible = false;
                            _adminMainForm = new AdminMainForm(this) { Visible = true }; //Переход в главную форму админа;
                        }
                        else if (user[1].ToString() == "3")
                        {
                            //переход в форму для сотрудника
                            this.Visible = false;
                            _userAccForm = new UserAccForm(this, Convert.ToInt32(user[0])) { Visible = true }; //Переход в личный кабинет сотрудника;
                          //  MessageBox.Show(user[0].ToString());
                        }
                        else
                        {
                            MessageBox.Show("Вы не сотрудник Планетария!");
                        }

                        textBox1.Clear();
                        textBox2.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не найден!");
                    }

                    user.Close();
                    conn.Close();
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

        private void button2_Click(object sender, EventArgs e) //Кнопка "Регистрация"
        {
            textBox1.Clear();
            textBox2.Clear();
            this.Visible = false;
            _regForm = new RegForm(this) { Visible = true }; //Переход в форму регистрации;
        }

        private void button3_Click(object sender, EventArgs e) //Выход
        {
            Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) | Char.IsLetter(e.KeyChar) | e.KeyChar == 8) return;
            else
                e.Handled = true;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 32) return;
            else
                e.Handled = true;
        }
    }
}
