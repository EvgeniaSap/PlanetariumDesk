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

namespace Planetarium
{
    public partial class EditRepertForm : Form
    {
        private AuthForm _authForm; //Форма авторизации;
        private AdminMainForm _adminMainForm; //Форма меню админа;
        private string _key;
        private List<int> _data_tick;

        public EditRepertForm()
        {
            InitializeComponent();
        }

        public EditRepertForm(AuthForm authForm, AdminMainForm adminForm)
        {
            _authForm = authForm;
            _adminMainForm = adminForm;
            _data_tick = new List<int>();
            _key = "";
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) //Кнопка Назад
        {
            this.Close();
            _adminMainForm.Show();
        }

        private void EditRepertForm_Load(object sender, EventArgs e)
        {
            label2.Text += "изменить:";

            //название, тип, цена
            var column1 = new DataGridViewColumn();
            column1.HeaderText = "Название"; //текст в шапке
            column1.Width = 250; //ширина колонки
            column1.Name = "Event"; //текстовое имя колонки, его можно использовать вместо обращений по индексу
            column1.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки

            //Height
            var column2 = new DataGridViewColumn();
            column2.HeaderText = "Тип";
            column2.Name = "Type";
            column2.CellTemplate = new DataGridViewTextBoxCell();

            var column3 = new DataGridViewColumn();
            column3.HeaderText = "Цена";
            column3.Name = "Price";
            column3.CellTemplate = new DataGridViewTextBoxCell();

            var column4 = new DataGridViewColumn();
            column4.HeaderText = "Дата начала действия цены";
            column4.Name = "Date";
            column4.CellTemplate = new DataGridViewTextBoxCell();


            dataGridView1.Columns.Add(column1);
            dataGridView1.Columns.Add(column2);
            dataGridView1.Columns.Add(column3);
            dataGridView1.Columns.Add(column4);
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridView1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 10f, FontStyle.Regular); //жирный курсив размера 16

            OutputEventPrice();
        }

        private void OutputEventPrice() //Выводим в DataGridView весь репертуар
        {
            MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
            conn.Open();
            string sql = "SELECT name_event, name_event_type, price, install_date, ticket_price.id_price " +
                "FROM event_type " +
                "JOIN event " +
                "ON event_type.id_event_type = event.id_event_type " +
                "JOIN ticket_price " +
                "ON event.id_event = ticket_price.id_event " +
                "ORDER BY  name_event, install_date DESC";
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader event_tick = command.ExecuteReader();
            if (_key != "actual")
            {
                while (event_tick.Read())
                {
                    dataGridView1.Rows.Add(event_tick[0].ToString(), event_tick[1].ToString(), event_tick[2].ToString(), event_tick[3].ToString());
                    _data_tick.Add(Convert.ToInt32(event_tick[4]));
                }
            }
            else //Выводим только актуальные цены
            {
                while (event_tick.Read())
                {
                    if (dataGridView1.RowCount != 0)
                    {
                        if (dataGridView1.Rows[dataGridView1.RowCount-1].Cells[0].Value.ToString() != event_tick[0].ToString())
                        {
                            dataGridView1.Rows.Add(event_tick[0].ToString(), event_tick[1].ToString(), event_tick[2].ToString(), event_tick[3].ToString());
                            _data_tick.Add(Convert.ToInt32(event_tick[4]));
                        }
                    }
                    else
                    {
                        dataGridView1.Rows.Add(event_tick[0].ToString(), event_tick[1].ToString(), event_tick[2].ToString(), event_tick[3].ToString());
                        _data_tick.Add(Convert.ToInt32(event_tick[4]));
                    }
                }
            }
            event_tick.Close();
            conn.Close();
            dataGridView1.AllowUserToAddRows = false; //запрещаем пользователю самому добавлять строки
        }

        private void button2_Click(object sender, EventArgs e) //Изменить или добавить мероприятие
        {
            Event _some_event;
            try
            {
                if (button2.Text == "Изменить") //Добавить новую запись в таблицу с ценами
                {
                    DateTime date1 = DateTime.Today; //Дата сейчас
                    _some_event = new Event((string)dataGridView1.CurrentRow.Cells[1].Value, (string)dataGridView1.CurrentRow.Cells[0].Value, Convert.ToString(date1).Substring(0, 10));
                    this.Hide();
                    AddEditForm Edit_event = new AddEditForm(_authForm, _adminMainForm, this, "add_new", _some_event) { Visible = true }; //Открываем форму для просмотра всего расписания
                }
                else //Добавить новое мероприятие ВЫГРУЗИТЬ ИЗ ФАЙЛА СОТР
                {
                  //  _some_event = new Employee((string)dataGridView1.CurrentRow.Cells[5].Value, (string)dataGridView1.CurrentRow.Cells[2].Value, (string)dataGridView1.CurrentRow.Cells[1].Value, _selectedPosit, (string)dataGridView1.CurrentRow.Cells[0].Value, (string)dataGridView1.CurrentRow.Cells[3].Value, (string)dataGridView1.CurrentRow.Cells[4].Value, (string)dataGridView1.CurrentRow.Cells[6].Value);

                  //  TicketForm tickets = new TicketForm(_authForm, _adminMainForm, this, _some_event, "data") { Visible = true }; //Открываем форму для просмотра всего расписания

                  //  this.Hide();
                }
            }
            catch
            {
                MessageBox.Show("Данной ячейки не существует!");
            }
        }

        private void button4_Click(object sender, EventArgs e) //Отобразить только актуальные цены
        {
            dataGridView1.Rows.Clear();
            _key = "actual";
            OutputEventPrice();
        }

        private void button3_Click(object sender, EventArgs e) //Добавить новое мероприятие //Отобразить предложенные мероприятия
        {
            Event _some_event;
            DateTime date1 = DateTime.Today; //Дата сейчас
            _some_event = new Event();
            _some_event.Date_event = Convert.ToString(date1).Substring(0, 10);
            this.Hide();
            AddEditForm Edit_event = new AddEditForm(_authForm, _adminMainForm, this, "add_new", _some_event) { Visible = true }; //Открываем форму для просмотра всего расписания
        }

        private void button5_Click(object sender, EventArgs e) //Переход к полному списку
        {
            dataGridView1.Rows.Clear();
            _key = "";
            OutputEventPrice();
        }
    }
}
