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
    public partial class FullScheduleForm : Form
    {
        private AuthForm _authForm; //Форма авторизации;
        private AdminMainForm _adminMainForm; //Форма меню админа;
        private UserAccForm _userAccForm; //Форма личного кабинета сотрудника
        private string _key; //Ключ по которому будет происходить отображение данных
        private Employee _some_event; //Мероприятие, которое хотят редактировать
        private string _selectedPosit; //Выбранная дата
        private Event _sched_some_event; //Мероприятие, на которое хотят посмотреть расписание
        private Employee _some_empl; //Сотрудник, расписание которого хотят посмотреть
        private Room _some_room; //Сотрудник, расписание которого хотят посмотреть
        private string _key_user=""; //Ключ по которому будет происходить отображение данных

        public FullScheduleForm() //Форма отображения расписаний
        {
            InitializeComponent();
        }

        public FullScheduleForm(AuthForm authForm, AdminMainForm adminForm, string key) //Конструктор для отображения всего расписания планетария
        {
            _authForm = authForm;
            _adminMainForm = adminForm;
            _key = key;
            InitializeComponent();
        }

        public FullScheduleForm(AuthForm authForm, AdminMainForm adminForm, string key, Event sched_some_event) //Конструктор для отображения всего расписания планетария
        {
            _authForm = authForm;
            _adminMainForm = adminForm;
            _key = key;
            _sched_some_event = sched_some_event;
            InitializeComponent();
        }

        public FullScheduleForm(AuthForm authForm, AdminMainForm adminForm, string key, Employee some_empl) //Конструктор для отображения расписания сотрудника
        {
            _authForm = authForm;
            _adminMainForm = adminForm;
            _key = key;
            _some_empl = some_empl;
            InitializeComponent();
        }

        public FullScheduleForm(AuthForm authForm, AdminMainForm adminForm, string key, Room some_room) //Конструктор для отображения расписания помещения
        {
            _authForm = authForm;
            _adminMainForm = adminForm;
            _key = key;
            _some_room = some_room;
            InitializeComponent();
        }

        public FullScheduleForm(AuthForm authForm, UserAccForm userAccForm, string key, Room some_room, string key2) //Конструктор для отображения расписания помещения с формы личного кабинета
        {
            _authForm = authForm;
            _userAccForm = userAccForm;
            _key = key;
            _some_room = some_room;
            _key_user = key2;
            InitializeComponent();
        }

        private void FullScheduleForm_Load(object sender, EventArgs e)
        {
            DateTime date1 = DateTime.Today; //Дата сейчас
            int days;
            if (_key == "tick")
            {
                days = 31;
            }
            else
            {
                days = 62;
            }
            for (int i = 0; i < days; i++)
            {
                comboBox1.Items.Add(Convert.ToString(date1.AddDays(i)).Substring(0, 10)); //Выводим в combobox1 даты
            }
            //в теч 30 дней устоявшееся расписание - далее можно менять

            switch (_key)
            {
                case "full":
                    label1.Text = "Полное расписание Планетария";
                    break;

                case "event": //Раписание конкретного мероприятия
                    label1.Text = "Расписание " + _sched_some_event.Name_event;
                    break;

                case "empl":
                    label1.Text = "Расписание сотрудника: " + _some_empl.Full_name;
                    break;

                case "room":
                    label1.Text = "Расписание " + _some_room.Name_room;
                    break;

                case "tick":
                    label1.Text = "Билеты на " + _sched_some_event.Name_event;
                    button3.Text = "Все билеты на эту дату";
                    button4.Text = "Рейтинг мероприятий";
                    button2.Text = "Подробнее о билетах";
                    break;
            }

            //время, название, длительность, помещение, сотрудник
            var column1 = new DataGridViewColumn();
            column1.HeaderText = "Время"; //текст в шапке
            column1.Name = "Time"; //текстовое имя колонки, его можно использовать вместо обращений по индексу
            column1.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки

            //Height
            var column2 = new DataGridViewColumn();
            column2.HeaderText = "Название";
            column2.Width = 200; //ширина колонки
            column2.Name = "Name";
            column2.CellTemplate = new DataGridViewTextBoxCell();

            var column3 = new DataGridViewColumn();
            column3.HeaderText = "Тип";
            column3.Name = "Type";
            column3.CellTemplate = new DataGridViewTextBoxCell();

            var column4 = new DataGridViewColumn();
            column4.HeaderText = "Длительность";
            column4.Name = "Duration";
            column4.CellTemplate = new DataGridViewTextBoxCell();

            var column5 = new DataGridViewColumn();
            column5.HeaderText = "Помещение";
            column5.Name = "Room";
            column5.CellTemplate = new DataGridViewTextBoxCell();

            var column6 = new DataGridViewColumn();
            column6.HeaderText = "Проводит";
            column6.Width = 250; //ширина колонки
            column6.Name = "Emloyee";
            column6.CellTemplate = new DataGridViewTextBoxCell();

            var column7 = new DataGridViewColumn();
            column7.HeaderText = "Места";
            column7.Name = "Seats";
            column7.CellTemplate = new DataGridViewTextBoxCell();

            dataGridView1.Columns.Add(column1);
            dataGridView1.Columns.Add(column2);
            dataGridView1.Columns.Add(column3);
            dataGridView1.Columns.Add(column4);
            dataGridView1.Columns.Add(column5);
            dataGridView1.Columns.Add(column6);
            dataGridView1.Columns.Add(column7);
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridView1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 10f, FontStyle.Regular); //жирный курсив размера 16

            if (_key_user == "user")
            {
                button2.Visible = false;
                button3.Visible = false;
                button4.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e) //Кнопка "назад"
        {
            if (_key_user != "user")
            {
                this.Close();
                _adminMainForm.Show(); //Переход к главной форме админа;
            }
            else
            {
                this.Close();
                _userAccForm.Show(); //Переход к главной форме админа;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //Выбор дня
        {
            _selectedPosit = comboBox1.SelectedItem.ToString();
            dataGridView1.Rows.Clear();
            OutputEvents();
        }

        private void OutputEvents() //Выводим в DataGridView новые значения
        {
            MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
            conn.Open();
            string sql = "SELECT time_event, name_event, name_event_type, duration_event, name_room, full_name, numb_seats " +
                "FROM event_type " +
                "JOIN event " +
                "ON event_type.id_event_type = event.id_event_type " +
                "JOIN schedule " +
                "ON event.id_event = schedule.id_event " +
                "JOIN room " +
                "ON schedule.id_room = room.id_room " +
                "JOIN schedule_empl " +
                "ON schedule.id_schedule_entry = schedule_empl.id_event_entry " +
                "JOIN employee " +
                "ON schedule_empl.id_employee = employee.id_employee " +
                "WHERE data_event = @data ";

            if (_key == "event" || _key == "tick") {
                sql += "AND name_event = @name ";
            }
            else if (_key == "empl")
            {
                sql += "AND full_name = @empl ";
            }
            else if (_key == "room")
            {
                sql += "AND name_room = @room ";
            }
            sql += "ORDER BY time_event";
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add("@data", MySqlDbType.VarChar).Value = _selectedPosit;
            if (_key == "event" || _key == "tick")
            {
                command.Parameters.Add("@name", MySqlDbType.VarChar).Value = _sched_some_event.Name_event;
            }
            else if (_key == "empl")
            {
                command.Parameters.Add("@empl", MySqlDbType.VarChar).Value = _some_empl.Full_name;
            }
            else if (_key == "room")
            {
                command.Parameters.Add("@room", MySqlDbType.VarChar).Value = _some_room.Name_room;
            }
            MySqlDataReader date_events = command.ExecuteReader();
            while (date_events.Read())
            {
                dataGridView1.Rows.Add(date_events[0].ToString(), date_events[1].ToString(), date_events[2].ToString(), date_events[3].ToString(), date_events[4].ToString(), date_events[5].ToString(), date_events[6].ToString()); 
            }

            date_events.Close();
            conn.Close();
            dataGridView1.AllowUserToAddRows = false; //запрещаем пользователю самому добавлять строки
        }

        private void button3_Click(object sender, EventArgs e) //Редактировать
        {
             try
            {
                if (_key != "tick")
                {
                    if (comboBox1.SelectedIndex > 30)
                    {
                        _some_event = new Employee((string)dataGridView1.CurrentRow.Cells[5].Value, (string)dataGridView1.CurrentRow.Cells[2].Value, (string)dataGridView1.CurrentRow.Cells[1].Value, _selectedPosit, (string)dataGridView1.CurrentRow.Cells[0].Value, (string)dataGridView1.CurrentRow.Cells[3].Value, (string)dataGridView1.CurrentRow.Cells[4].Value, (string)dataGridView1.CurrentRow.Cells[6].Value);
                        this.Hide();
                        AddEditForm Edit_event = new AddEditForm(_authForm, _adminMainForm, this, "edit", _key, _some_event) { Visible = true }; //Открываем форму для просмотра всего расписания
                    }
                    else
                    {
                        MessageBox.Show("Расписание на ближайшие 30 дней редактировать запрещено.");
                    }
                }
                else //Все билеты на дату
                {
                    _some_event = new Employee((string)dataGridView1.CurrentRow.Cells[5].Value, (string)dataGridView1.CurrentRow.Cells[2].Value, (string)dataGridView1.CurrentRow.Cells[1].Value, _selectedPosit, (string)dataGridView1.CurrentRow.Cells[0].Value, (string)dataGridView1.CurrentRow.Cells[3].Value, (string)dataGridView1.CurrentRow.Cells[4].Value, (string)dataGridView1.CurrentRow.Cells[6].Value);
                    TicketForm tickets = new TicketForm(_authForm, _adminMainForm, this, _some_event, "data") { Visible = true }; //Открываем форму для просмотра всего расписания
                    this.Hide();
                }
            }
            catch
            {
                MessageBox.Show("Данной ячейки не существует!");
            }
        }

        private void button2_Click(object sender, EventArgs e) //Добавить новое мероприятие на эту дату
        {
            if (_key != "tick")
            {
                if (comboBox1.SelectedIndex > 30)
                {
                    if (_key == "full")
                    {
                        this.Hide();
                        AddEditForm Add_event = new AddEditForm(_authForm, _adminMainForm, this, "add_in", _selectedPosit) { Visible = true }; //Открываем форму для просмотра всего расписания
                    }
                    else if (_key == "event")
                    {
                        this.Hide();
                        _sched_some_event.Date_event = _selectedPosit;
                        AddEditForm Edit_event = new AddEditForm(_authForm, _adminMainForm, this, "add_in", _key, _sched_some_event) { Visible = true }; //Открываем форму для просмотра всего расписания
                    }
                    else if (_key == "empl")
                    {
                        this.Hide();
                        _some_empl.Schedule.Add(new Event());
                        _some_empl.Schedule[0].Date_event = _selectedPosit;
                        AddEditForm Edit_event = new AddEditForm(_authForm, _adminMainForm, this, "add_in", _key, _some_empl) { Visible = true }; //Открываем форму для просмотра всего расписания
                    }
                    else if (_key == "room")
                    {
                        this.Hide();
                        Employee some_room = new Employee();
                        some_room.Schedule.Add(new Event());
                        some_room.Schedule[0].Date_event = _selectedPosit;
                        some_room.Schedule[0].Event_room = new Planetarium.Room();
                        some_room.Schedule[0].Event_room = _some_room;
                        AddEditForm Edit_event = new AddEditForm(_authForm, _adminMainForm, this, "add_in", _key, some_room) { Visible = true }; //Открываем форму для просмотра всего расписания
                    }
                }
                else
                {
                    MessageBox.Show("Расписание на ближайшие 30 дней редактировать запрещено.");
                }
            }
            else //Подробнее о билетах
            {
                try
                {
                    _some_event = new Employee((string)dataGridView1.CurrentRow.Cells[5].Value, (string)dataGridView1.CurrentRow.Cells[2].Value, (string)dataGridView1.CurrentRow.Cells[1].Value, _selectedPosit, (string)dataGridView1.CurrentRow.Cells[0].Value, (string)dataGridView1.CurrentRow.Cells[3].Value, (string)dataGridView1.CurrentRow.Cells[4].Value, (string)dataGridView1.CurrentRow.Cells[6].Value);
                    TicketForm tickets = new TicketForm(_authForm, _adminMainForm, this, _some_event, "data_ev") { Visible = true }; //Открываем форму для просмотра всего расписания
                    this.Hide();
                }
                catch
                {
                    MessageBox.Show("Данной ячейки не существует!");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e) //Удаление записи в расписании
        {
            try
            {
                if (_key != "tick")
                {
                    if (comboBox1.SelectedIndex > 30)
                    {
                        string id_entry_sched_empl;
                        string id_entry_sched;
                        MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                        conn.Open();
                        string sql = "SELECT schedule_empl.id_event_entry, schedule_empl.id_schedule_entry " +
                            "FROM event_type " +
                            "JOIN event " +
                            "ON event_type.id_event_type = event.id_event_type " +
                            "JOIN schedule " +
                            "ON event.id_event = schedule.id_event " +
                            "JOIN room " +
                            "ON schedule.id_room = room.id_room " +
                            "JOIN schedule_empl " +
                            "ON schedule.id_schedule_entry = schedule_empl.id_event_entry " +
                            "JOIN employee " +
                            "ON schedule_empl.id_employee = employee.id_employee " +
                            "WHERE data_event = @data AND name_room = @name AND time_event = @time";

                        MySqlCommand command = new MySqlCommand(sql, conn);
                        command.Parameters.Add("@data", MySqlDbType.VarChar).Value = _selectedPosit;
                        command.Parameters.Add("@name", MySqlDbType.VarChar).Value = (string)dataGridView1.CurrentRow.Cells[4].Value;
                        command.Parameters.Add("@time", MySqlDbType.VarChar).Value = (string)dataGridView1.CurrentRow.Cells[0].Value;
                        MySqlDataReader date_events = command.ExecuteReader();
                        date_events.Read();
                        id_entry_sched_empl = Convert.ToString(date_events[1]);
                        id_entry_sched = Convert.ToString(date_events[0]);
                        date_events.Close();
                        conn.Close();
                        DialogResult result = MessageBox.Show(
                              "Вы уверены, что хотите удалить эту запись в расписании?",
                              "Сообщение",
                              MessageBoxButtons.YesNo,
                              MessageBoxIcon.Information,
                              MessageBoxDefaultButton.Button1,
                              MessageBoxOptions.DefaultDesktopOnly);
                        if (result == DialogResult.Yes)
                        {
                            conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                            sql = "DELETE FROM schedule_empl WHERE id_schedule_entry = @id";

                            command = new MySqlCommand(sql, conn);
                            command.Parameters.AddWithValue("@id", id_entry_sched_empl);
                            command.Connection.Open();
                            command.ExecuteNonQuery();
                            command.Connection.Close();

                            conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                            sql = "DELETE FROM schedule WHERE id_schedule_entry = @id";
                            command = new MySqlCommand(sql, conn);
                            command.Parameters.AddWithValue("@id", id_entry_sched);
                            command.Connection.Open();
                            command.ExecuteNonQuery();
                            command.Connection.Close();

                            dataGridView1.Rows.Clear();
                            OutputEvents();
                        }
                        this.TopMost = true;
                    }
                    else
                    {
                        MessageBox.Show("Расписание на ближайшие 30 дней редактировать запрещено.");
                    }
                }
                else
                {
                    TicketForm tickets = new TicketForm(_authForm, _adminMainForm, this, "top") { Visible = true }; //Открываем форму для просмотра всего расписания
                    this.Hide();
                }
            }
            catch
            {
                MessageBox.Show("Данной ячейки не существует!");
            }
        }
    }
}
