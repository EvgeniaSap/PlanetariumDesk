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
    public partial class TicketForm : Form
    {
        private AuthForm _authForm; //Форма авторизации;
        private AdminMainForm _adminMainForm; //Форма меню админа;
        private FullScheduleForm _fullScheduleForm; //Форма полного расписания;
        private Employee _some_event; //Мероприятие, на которое хотят посмтреть билеты
        private List<int> _del_tick; //Массив id заказов, помеченных на удаление
        private List<int> _tick; //Массив id заказов
        string _key; //Ключ отображения 

        public TicketForm()
        {
            InitializeComponent();
        }

        //Билеты на дату или на мероприятие
        public TicketForm(AuthForm authForm, AdminMainForm adminMainForm, FullScheduleForm fullScheduleForm, Employee some_event, string key)
        {
            _authForm = authForm;
            _adminMainForm = adminMainForm;
            _fullScheduleForm = fullScheduleForm;
            _some_event = some_event;
            _del_tick = new List<int>();
            _tick = new List<int>();
            _key = key;

            InitializeComponent();
        }

        //Для рейтинга мероприятий
        public TicketForm(AuthForm authForm, AdminMainForm adminMainForm, FullScheduleForm fullScheduleForm, string key)
        {
            _authForm = authForm;
            _adminMainForm = adminMainForm;
            _fullScheduleForm = fullScheduleForm;
            _key = key;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) //Кнопка Назад
        {
            this.Close();
            _fullScheduleForm.Show(); //Переход к форме расписания;
        }

        private void TicketForm_Load(object sender, EventArgs e)
        {
            if (_key != "top")
            {
                label1.Text = "Билеты на " + _some_event.Schedule[0].Date_event;

                if (_key == "data_ev")
                {
                    label1.Text += " на " + _some_event.Schedule[0].Name_event;
                    label2.Text += " в " + _some_event.Schedule[0].Event_room.Name_room + " в " + _some_event.Schedule[0].Time_event + ":";
                }
                else
                {
                    label2.Text += ":";
                }
                //пользователь, контактное лицо, телефон, кол-во билетов, итоговая цена, пометка на удаление
                var column1 = new DataGridViewColumn();
                column1.HeaderText = "Пользователь"; //текст в шапке
                                                     //column1.Width = 300; //ширина колонки
                                                     //column1.ReadOnly = true; //значение в этой колонке нельзя править
                column1.Name = "Account"; //текстовое имя колонки, его можно использовать вместо обращений по индексу
                                          //column1.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
                column1.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки

                //Height
                var column2 = new DataGridViewColumn();
                column2.HeaderText = "Контакты";
                column2.Width = 250; //ширина колонки
                column2.Name = "Contacts";
                column2.CellTemplate = new DataGridViewTextBoxCell();

                var column3 = new DataGridViewColumn();
                column3.HeaderText = "Билеты";
                //  column2.Width = 50;
                column3.Name = "Tickets";
                column3.CellTemplate = new DataGridViewTextBoxCell();

                var column4 = new DataGridViewColumn();
                column4.HeaderText = "Цена";
                //  column2.Width = 40;
                column4.Name = "Price";
                column4.CellTemplate = new DataGridViewTextBoxCell();

                var column5 = new DataGridViewColumn();
                column5.HeaderText = "Пометка";
                // column2.Width = 70;
                column5.Name = "Status";
                column5.CellTemplate = new DataGridViewTextBoxCell();


                dataGridView1.Columns.Add(column1);
                dataGridView1.Columns.Add(column2);
                dataGridView1.Columns.Add(column3);
                dataGridView1.Columns.Add(column4);
                dataGridView1.Columns.Add(column5);
                dataGridView1.EnableHeadersVisualStyles = false;
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridView1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 10f, FontStyle.Regular); //жирный курсив размера 16

                OutputTickets();
            }
            else
            {
                label1.Text = "Рейтинг мероприятий за последний месяц ";
                label2.Text += ":";
                button2.Enabled = false;
                button2.Visible = false;
                button3.Enabled = false;
                button3.Visible = false;

                //мероприятие, тип, телефон, кол-во заказов, кол-во билетов, итоговая цена
                var column1 = new DataGridViewColumn();
                column1.HeaderText = "Мероприятие"; //текст в шапке
                column1.Width = 250; //ширина колонки
                column1.Name = "Event"; //текстовое имя колонки, его можно использовать вместо обращений по индексу
                column1.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки

                //Height
                var column2 = new DataGridViewColumn();
                column2.HeaderText = "Тип";
                column2.Name = "Type";
                column2.CellTemplate = new DataGridViewTextBoxCell();

                var column3 = new DataGridViewColumn();
                column3.HeaderText = "Сделано заказов";
                //  column2.Width = 50;
                column3.Name = "Order";
                column3.CellTemplate = new DataGridViewTextBoxCell();

                var column4 = new DataGridViewColumn();
                column4.HeaderText = "Заказано билетов";
                //  column2.Width = 40;
                column4.Name = "Tickets";
                column4.CellTemplate = new DataGridViewTextBoxCell();

                var column5 = new DataGridViewColumn();
                column5.HeaderText = "Общая цена";
                // column2.Width = 70;
                column5.Name = "Price";
                column5.CellTemplate = new DataGridViewTextBoxCell();

                dataGridView1.Columns.Add(column1);
                dataGridView1.Columns.Add(column2);
                dataGridView1.Columns.Add(column3);
                dataGridView1.Columns.Add(column4);
                dataGridView1.Columns.Add(column5);
                dataGridView1.EnableHeadersVisualStyles = false;
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridView1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 10f, FontStyle.Regular); //жирный курсив размера 16

                OutputTop();
            }

        }

        private void OutputTickets() //Выводим в DataGridView новые значения
        {
            MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

            conn.Open();
            string sql = "SELECT login, contact_person, phone, numb_ticket, final_price, ticket.status, ticket.id_ticket " +
                "FROM account " +
                "JOIN customer " +
                "ON account.id_account = customer.id_account " +
                "JOIN ticket " +
                "ON customer.id_customer = ticket.id_customer " +
                "JOIN schedule " +
                "ON ticket.id_schedule = schedule.id_schedule_entry " +
                "JOIN room " +
                "ON schedule.id_room = room.id_room " +
                "JOIN schedule_empl " +
                "ON  schedule.id_schedule_entry = schedule_empl.id_schedule_entry " +
                "WHERE data_event = @data ";
            if (_key == "data_ev")
            {
                sql += "AND name_room = @room AND time_event = @time";
            }

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add("@data", MySqlDbType.VarChar).Value = _some_event.Schedule[0].Date_event;
            if (_key == "data_ev")
            { 
                command.Parameters.Add("@room", MySqlDbType.VarChar).Value = _some_event.Schedule[0].Event_room.Name_room;
                command.Parameters.Add("@time", MySqlDbType.VarChar).Value = _some_event.Schedule[0].Time_event;
            }
            
            MySqlDataReader date_event_tick = command.ExecuteReader();
            
            while (date_event_tick.Read())
            {
                string stat = "";
                if (Convert.ToBoolean(date_event_tick[5]) != false)
                {
                    stat = "Отменён";
                    _del_tick.Add(Convert.ToInt32(date_event_tick[6]));
                }
               
                dataGridView1.Rows.Add(date_event_tick[0].ToString(), date_event_tick[1].ToString()+"("+ date_event_tick[2].ToString()+")", date_event_tick[3].ToString(), date_event_tick[4].ToString(), stat);
                _tick.Add(Convert.ToInt32(date_event_tick[6]));
            }


        
            date_event_tick.Close();
            conn.Close();

            int tickets = 0;
            int sum_price = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                tickets += Convert.ToInt32(row.Cells[2].Value);
                sum_price += Convert.ToInt32(row.Cells[3].Value);
            }

            dataGridView1.Rows.Add("Итого:", "", tickets, sum_price, "");

            dataGridView1.AllowUserToAddRows = false; //запрещаем пользователю самому добавлять строки

        }

        private void OutputTop() //Выводим в DataGridView новые значения рейтинга мероприятий
        {
            DateTime date1 = DateTime.Today; //Дата сейчас
           
            MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

            conn.Open();
            string sql = "SELECT name_event, name_event_type, COUNT(numb_ticket), SUM(numb_ticket), SUM(final_price), schedule.id_event " +
                "FROM event_type " + 
                "JOIN event " +
                "ON event_type.id_event_type = event.id_event_type " +
                "LEFT JOIN schedule " +
                "ON event.id_event = schedule.id_event " +
                "LEFT JOIN ticket " +
                "ON schedule.id_schedule_entry = ticket.id_schedule " +
                "LEFT JOIN schedule_empl " +
                "ON  schedule.id_schedule_entry = schedule_empl.id_schedule_entry " +
           //     "WHERE schedule.data_event > '" + Convert.ToString(date1).Substring(0, 10) + "' AND schedule.data_event < '"+ Convert.ToString(date1.AddDays(30)).Substring(0, 10)+ "' "+
                "GROUP BY schedule.id_event "+
                "ORDER BY SUM(numb_ticket) DESC ";
            

            MySqlCommand command = new MySqlCommand(sql, conn);
            
            MySqlDataReader event_tick = command.ExecuteReader();

            while (event_tick.Read())
            {
                dataGridView1.Rows.Add(event_tick[0].ToString(), event_tick[1].ToString(), event_tick[2].ToString(), event_tick[3].ToString(), event_tick[4].ToString());
            }

            event_tick.Close();
            conn.Close();

            int orders = 0;
            int tickets = 0;
            int sum_price = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                try
                {
                    orders += Convert.ToInt32(row.Cells[2].Value);
                    tickets += Convert.ToInt32(row.Cells[3].Value);
                    sum_price += Convert.ToInt32(row.Cells[4].Value);
                }
                catch
                {
                    orders += 0;
                    tickets += 0;
                    sum_price += 0;
                }
                
            }

            dataGridView1.Rows.Add("Итого:", "", orders, tickets, sum_price);

            dataGridView1.AllowUserToAddRows = false; //запрещаем пользователю самому добавлять строки

        }

        private void button2_Click(object sender, EventArgs e) //Удалить все заказы, помеченные на удаление
        {
            DialogResult result = MessageBox.Show(
                              "Вы уверены, что хотите удалить все отмененные заказы?",
                              "Сообщение",
                              MessageBoxButtons.YesNo,
                              MessageBoxIcon.Information,
                              MessageBoxDefaultButton.Button1,
                              MessageBoxOptions.DefaultDesktopOnly);
            if (result == DialogResult.Yes)
            {
                foreach (int id in _del_tick)
                {
                    MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                    string sql = "DELETE FROM ticket WHERE id_ticket = @id";

                    MySqlCommand command = new MySqlCommand(sql, conn);
                    command.Parameters.AddWithValue("@id", id);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
                dataGridView1.Rows.Clear();
                OutputTickets();
            }
            this.TopMost = true;
           
        }

        private void button3_Click(object sender, EventArgs e) //Удалить выбранный заказ
        {
            try
            {
                DialogResult result = MessageBox.Show(
                      "Вы уверены, что хотите удалить эту этот заказ?",
                      "Сообщение",
                      MessageBoxButtons.YesNo,
                      MessageBoxIcon.Information,
                      MessageBoxDefaultButton.Button1,
                      MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.Yes)
                {
                    // MessageBox.Show(_tick[dataGridView1.CurrentRow.Index].ToString());
                    MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                    string sql = "DELETE FROM ticket WHERE id_ticket = @id";

                    MySqlCommand command = new MySqlCommand(sql, conn);
                    command.Parameters.AddWithValue("@id", _tick[dataGridView1.CurrentRow.Index]);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();

                    dataGridView1.Rows.Clear();
                    OutputTickets();
                }
                this.TopMost = true;
                
            }
            catch
            {
                MessageBox.Show("Данной ячейки не существует!");
            }
        }
    }
}
