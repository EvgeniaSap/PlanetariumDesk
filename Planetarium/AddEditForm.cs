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
    public partial class AddEditForm : Form
    {
        private AuthForm _authForm; //Форма авторизации;
        private AdminMainForm _adminMainForm; //Форма меню админа;
        private FullScheduleForm _fullScheduleForm; //Форма полного расписания;
        private EditRepertForm _editRepertForm; //Форма полного расписания;
        private string _key; //Ключ отображения - редактировать или добавить мероприятие;
        private Employee _edit_event; //Выбранное на редактирование мероприятие
        private string _selectedPositData; //Выбранная дата
        private string _selectedPositTime; //Выбранное время
        private string _selectedPositRoom; //Выбранное помещение
        private string _selectedPositType; //Выбранный тип мероприятия
        private string _selectedPositEvent; //Выбранное мероприятие
        private string _selectedPositEmpl; //Выбранный сотрудник
        private Event _new_event; //Новое мероприятие на добавление

        private string _key_FullSchForm; //Ключ отображения - добавление конкретного мероприятия или мероприятий для конкретного сотрудника;

        public AddEditForm()
        {
            InitializeComponent();
        }

    /*    public AddEditForm(AuthForm authForm, AdminMainForm adminMainForm, FullScheduleForm fullScheduleForm, string key, Employee edit_event) //Редактировать мероприятие - из полного списка
        {
            _authForm = authForm;
            _adminMainForm = adminMainForm;
            _fullScheduleForm = fullScheduleForm;
            _key = key;
            _edit_event = edit_event;

            InitializeComponent();
        }*/

        public AddEditForm(AuthForm authForm, AdminMainForm adminMainForm, FullScheduleForm fullScheduleForm, string key, string date) // добавить мероприятие - в полный спискок
        {
            _authForm = authForm;
            _adminMainForm = adminMainForm;
            _fullScheduleForm = fullScheduleForm;
            _key = key;
            _edit_event = new Employee();
            _edit_event.Schedule.Add(new Event());
            _edit_event.Schedule[0].Name_event = "";
            _edit_event.Schedule[0].Date_event = date;

            InitializeComponent();
        }

        // добавить конкретное мероприятие - в расписание мероприятия
        public AddEditForm(AuthForm authForm, AdminMainForm adminMainForm, FullScheduleForm fullScheduleForm, string key, string key2, Event some_ev)
        {
            _authForm = authForm;
            _adminMainForm = adminMainForm;
            _fullScheduleForm = fullScheduleForm;
            _key = key;
            _key_FullSchForm = key2;
            _edit_event = new Employee();
            _edit_event.Schedule.Add(new Event());
            _edit_event.Schedule[0] = some_ev;

            InitializeComponent();
        }

        // добавить мероприятие для сотрудника (или для помещения?)
        public AddEditForm(AuthForm authForm, AdminMainForm adminMainForm, FullScheduleForm fullScheduleForm, string key, string key2, Employee some_empl)
        {
            _authForm = authForm;
            _adminMainForm = adminMainForm;
            _fullScheduleForm = fullScheduleForm;
            _key = key;
            _key_FullSchForm = key2;
            _edit_event = some_empl;
            //  _edit_event.Schedule.Add(new Event());
            //  _edit_event.Schedule[0] = some_ev;

            InitializeComponent();
        }

        // добавить новое мероприятие в репертуар
        public AddEditForm(AuthForm authForm, AdminMainForm adminMainForm, EditRepertForm editRepertForm, string key, Event some_ev)
        {
            _authForm = authForm;
            _adminMainForm = adminMainForm;
            _editRepertForm = editRepertForm;
            _key = key;
            _new_event = some_ev;

            InitializeComponent();
        }

        private void AddEditForm_Load(object sender, EventArgs e)
        {
            /* DateTime date1 = DateTime.Today; //Дата сейчас

             for (int i = 30; i < 62; i++)
             {
                 comboBox3.Items.Add(Convert.ToString(date1.AddDays(i)).Substring(0, 10)); //Выводим в combobox3 даты
             }*/

            //comboBox3.Items.Add(_edit_event.Schedule[0].Date_event); //Выводим в combobox3 выбранную дату

            switch (_key)
            {
                case "edit": //редактирование
                    DateTime date1 = DateTime.Today; //Дата сейчас

                    for (int i = 30; i < 62; i++)
                    {
                        comboBox3.Items.Add(Convert.ToString(date1.AddDays(i)).Substring(0, 10)); //Выводим в combobox3 даты
                    }
                    this.Text = "Редактирование";
                    textBox1.Enabled = false;
                    textBox1.Visible = false;
                    textBox4.Enabled = false;
                    textBox4.Visible = false;
                    comboBox1.Enabled = true;
                    comboBox1.Visible = true;
                    textBox5.Enabled = false;
                    textBox5.Visible = false;
                    textBox6.Enabled = false;
                    textBox6.Visible = false;
                    label1.Text = "Редактировать мероприятие";
                    label4.Text = "Перенести в ";
                    label7.Text = "Перенести на:";
                    button2.Text = "Сохранить изменения";
                    textBox2.Text = _edit_event.Schedule[0].Duration_event;
                    textBox3.Text = _edit_event.Schedule[0].Numb_of_seats;
                    OutputForEdit(); //Отображение данных для редактирования
                    break;

                case "add_new": //Добавление в репертуар
                    this.Text = "Добавление";
                    label1.Text = "Мероприятие";
                    if (!string.IsNullOrEmpty(_new_event.Name_event))
                    {
                        textBox6.Enabled = true;
                        textBox6.Visible = true;
                        textBox1.Text = _new_event.Name_event;
                        textBox5.Text = _new_event.Date_event;
                        textBox6.Text = _new_event.Name_event_type;
                    }
                    else
                    {
                        textBox6.Enabled = false;
                        textBox6.Visible = false;
                        textBox1.ReadOnly = false;
                        textBox5.Text = _new_event.Date_event;
                        OutputType(); //Вывод типа мероприятия
                    }
                    textBox1.Enabled = true;
                    textBox1.Visible = true;
                    textBox2.Enabled = false;
                    textBox2.Visible = false;
                    textBox3.Enabled = false;
                    textBox3.Visible = false;
                    textBox4.Enabled = true;
                    textBox4.Visible = true;
                    textBox5.Enabled = true;
                    textBox5.Visible = true;
                    
                    comboBox1.Enabled = false;
                    comboBox1.Visible = false;
                    label4.Text = "Цена: ";
                    label6.Visible = false;
                    label7.Text = "Установлено:";
                    label8.Visible = false;
                    label9.Visible = false;
                    label10.Visible = false;

                    comboBox4.Enabled = false;
                    comboBox4.Visible = false;
                    comboBox5.Enabled = false;
                    comboBox5.Visible = false;
                    comboBox6.Enabled = false;
                    comboBox6.Visible = false;
                    button2.Text = "Добавить";
                    

                    break;

                case "add_in": //Добавление в расписание
                    this.Text = "Добавление";
                    label1.Text = "Добавить мероприятие в расписание";
                    textBox1.Enabled = false;
                    textBox1.Visible = false;
                    textBox4.Enabled = false;
                    textBox4.Visible = false;
                    textBox5.Enabled = false;
                    textBox5.Visible = false;
                    textBox6.Enabled = false;
                    textBox6.Visible = false;
                    comboBox1.Enabled = true;
                    comboBox1.Visible = true;
                    label4.Text = "Помещение";
                    label7.Text = "Выберите: ";
                    button2.Text = "Добавить";
                    comboBox3.Items.Add(_edit_event.Schedule[0].Date_event); //Выводим в combobox3 выбранную дату
                    if (_key_FullSchForm == "event")
                    {
                        comboBox1.Items.Add(_edit_event.Schedule[0].Name_event); //Выводим в combobox1 выбранное мероприятие
                    }
                    else if (_key_FullSchForm == "empl" || _key_FullSchForm == "room")
                    {
                        OutputForAddForER(); //Отображение данных для добавления мероприятия для сотрудника
                        //comboBox1.Items.Add(_edit_event.Schedule[0].Name_event); //Выводим в combobox1 выбранное мероприятие
                    }
                    else
                    {
                        OutputEvent(); //Вывод мероприятий
                    }
                    break;

            }

            // OutputEvent();

        }

        private void button1_Click(object sender, EventArgs e) //Кнопка назад
        {
            if (_key != "add_new")
            {
                this.Close();
                _fullScheduleForm.Show(); //Переход к форме полного расписания;
            }
            else
            {
                this.Close();
                _editRepertForm.Show(); //Переход к форме полного расписания;
            }
        }

        private void button2_Click(object sender, EventArgs e) //Кнопка сохраниения изменений или добавления новой записи
        {
            switch (_key)
            {
                case "edit":
                    SaveEdit(); //Сохранение изменений
                    break;

                case "add_new":
                    AddInEvTick(); //Установка новой цены
                    break;

                case "add_in":
                    AddInSchedule(); //Добавление мероприятия в расписание
                    break;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //Выбор мероприятия
        {
            _selectedPositEvent = comboBox1.SelectedItem.ToString();
            switch (_key)
            {
                case "edit":
                    OutputTypeForEdit(); //Вывод типов мероприятия
                    break;

                case "add_new":
                    break;

                case "add_in":
                    OutputTypeForEdit(); //Вывод типов мероприятия
                    break;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedPositType = comboBox2.SelectedItem.ToString(); //Выбранный тип мероприятия

            OutputRoom(); //Вывод помещений
            OutputEmpl(); //Вывод сотрудников

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e) //Выбор дня
        {
            _selectedPositData = comboBox3.SelectedItem.ToString();

            OutputTime();

            //comboBox4.SelectedItem = _edit_event.Schedule[0].Time_event;
        }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e) //Выбор времени 
        {
            _selectedPositTime = comboBox4.SelectedItem.ToString();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedPositRoom = comboBox5.SelectedItem.ToString(); //Выбранное помещение

            // OutputRoom();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedPositEmpl = comboBox6.SelectedItem.ToString(); //Выбранный сотрудник
        }
        //как сделать чтобы пользователь мог сам добавлять текст в некотор стр, но в некоторые нет
        //также возм огр ввод
        //добавление нового мероприятия по заявке сотрудника форма с кнопкой добавить

        private void OutputEvent() //Вывод мероприятий
        {
            MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

            conn.Open();
            string sql = "SELECT name_event " +
                "FROM event ";

            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader all_event = command.ExecuteReader();

            while (all_event.Read())
            {
                comboBox1.Items.Add(Convert.ToString(all_event[0]));
            }

            all_event.Close();
            conn.Close();
        }

        private void OutputTime() //Заполнение времени с расчетом свободного
        {
            comboBox4.Items.Clear();

            // if (string.IsNullOrEmpty(_edit_event.Full_name))
            //   {
            /*if (_key =="edit")
            {
                if (_selectedPositEvent == _edit_event.Schedule[0].Name_event)
                {
                    comboBox4.Items.Add(Convert.ToString(_edit_event.Schedule[0].Time_event));
                }
            }*/
            // }

            MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

            conn.Open();
            string sql = "SELECT time_event, duration_event, data_event, room.name_room " +
                "FROM schedule_empl " +
                "JOIN schedule " +
                "ON schedule_empl.id_event_entry = schedule.id_schedule_entry " +
                "JOIN room " +
                "ON schedule.id_room = room.id_room " +
                "WHERE data_event = @data AND room.name_room = @name_r " +
                "ORDER BY time_event ";
           
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add("@data", MySqlDbType.VarChar).Value = _selectedPositData;
            command.Parameters.Add("@name_r", MySqlDbType.VarChar).Value = _selectedPositRoom;
            MySqlDataReader busy_time = command.ExecuteReader();

            List<string> busy_time1 = new List<string>();
            List<string> durat = new List<string>();
            while (busy_time.Read())
            {
                busy_time1.Add(Convert.ToString(busy_time[0]));
                durat.Add(Convert.ToString(busy_time[1]));
            }

            busy_time.Close();
            conn.Close();

            List<int> busy_time2 = new List<int>(); //Занятые часы с учетом длительности

            for (int i = 10; i < 20; i++)
            {
                int hour;
                int end_hour;
                busy_time2.Clear();

                for (int j = 0; j < busy_time1.Count(); j++)
                {
                    hour = Convert.ToInt32(busy_time1[j].Substring(0, 2));

                    end_hour = hour + Convert.ToInt32(durat[j]);

                    if (i >= hour && i <= end_hour)
                    {
                        busy_time2.Add(i);

                    }

                }

                if (busy_time2.Count() == 0)
                {
                    comboBox4.Items.Add(Convert.ToString(i + ":00"));

                }
            }

            if (comboBox4.Items.Count != 10 && _key == "edit")
            {
                    if (_selectedPositEvent == _edit_event.Schedule[0].Name_event)
                    {
                        comboBox4.Items.Add(Convert.ToString(_edit_event.Schedule[0].Time_event));
                    }
                
            }

        }

        private void OutputType() //Вывод типа мероприятия
        {
            MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

            conn.Open();

            string sql = "SELECT name_event_type " +
                    "FROM event_type ";

            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader all_event_type = command.ExecuteReader();

            while (all_event_type.Read())
            {
                comboBox2.Items.Add(Convert.ToString(all_event_type[0]));
            }

            all_event_type.Close();

            conn.Close();
        }

        private void OutputTypeForEdit() //Вывод типа мероприятия для редактирования
        {
            comboBox2.Items.Clear();

            MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

            conn.Open();

            string sql = "SELECT name_event_type, name_event " +
                    "FROM event_type " +
                    "JOIN event " +
                    "ON event_type.id_event_type = event.id_event_type " +
                    "WHERE name_event = @name";

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = _selectedPositEvent;
            MySqlDataReader all_event_type = command.ExecuteReader();

            while (all_event_type.Read())
            {
                comboBox2.Items.Add(Convert.ToString(all_event_type[0]));
            }

            all_event_type.Close();

            conn.Close();

        }

        private void OutputEmpl() //Вывод сотрудников
        {
            comboBox6.Items.Clear();

            if (_key_FullSchForm != "empl")
            {
                MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

                conn.Open();
                string sql = "SELECT full_name, employee.id_position, name_position " +
                    "FROM employee " +
                    "JOIN position " +
                    "ON employee.id_position = position.id_position " +
                    "WHERE name_position LIKE '" + _selectedPositType.Substring(0, 3) + "%'";

                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataReader empl_for_type = command.ExecuteReader();

                while (empl_for_type.Read())
                {
                    comboBox6.Items.Add(Convert.ToString(empl_for_type[0]));
                }

                empl_for_type.Close();
                conn.Close();
            }
            else
            {
                comboBox6.Items.Add(_edit_event.Full_name);
            }
        }

        private void OutputRoom() //Вывод помещений
        {
            comboBox5.Items.Clear();

            if (_key_FullSchForm != "room")
            {
                MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

                conn.Open();
                string sql = "SELECT name_room , name_room_type " +
                    "FROM room " +
                    "JOIN room_type " +
                    "ON room.id_room_type = room_type.id_room_type " +
                    "WHERE name_room_type LIKE ";


                if (_selectedPositType.Substring(0, 3) == "экс")
                {
                    sql += "'муз%'";
                }
                else if (_selectedPositType.Substring(0, 3) == "кин")
                {
                    sql += "'зал%'";
                }
                else
                {
                    sql += "'" + _selectedPositType.Substring(0, 3) + "%'";
                }

                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataReader room_for_type = command.ExecuteReader();

                while (room_for_type.Read())
                {
                    comboBox5.Items.Add(Convert.ToString(room_for_type[0]));
                }

                room_for_type.Close();
                conn.Close();
            }
            else
            {
                comboBox5.Items.Add(_edit_event.Schedule[0].Event_room.Name_room);
            }
        }

        private void OutputForEdit() //Отображение данных для редактирования
        {
            if (_key_FullSchForm == "event")
            {
                comboBox1.Items.Add(_edit_event.Schedule[0].Name_event); //Выводим в combobox1 выбранное мероприятие
            }
            else if (_key_FullSchForm == "empl" || _key_FullSchForm == "room")
            {
                OutputForAddForER(); //Отображение данных для добавления мероприятия для сотрудника
                                     //comboBox1.Items.Add(_edit_event.Schedule[0].Name_event); //Выводим в combobox1 выбранное мероприятие
            }
            else
            {
                OutputEvent(); //Вывод мероприятий
            }
           // OutputEvent();
            comboBox1.SelectedItem = _edit_event.Schedule[0].Name_event;

            comboBox2.SelectedItem = _edit_event.Schedule[0].Name_event_type;
            comboBox5.SelectedItem = _edit_event.Schedule[0].Event_room.Name_room;
            comboBox6.SelectedItem = _edit_event.Full_name;

            comboBox3.SelectedItem = _edit_event.Schedule[0].Date_event;


            comboBox4.SelectedItem = _edit_event.Schedule[0].Time_event;

        }

        private void OutputForAddForER() //Отображение данных для добавления мероприятия для сотрудника или для помещения
        {
            /* OutputEvent();
             comboBox1.SelectedItem = _edit_event.Schedule[0].Name_event;

             comboBox2.SelectedItem = _edit_event.Schedule[0].Name_event_type;
             comboBox5.SelectedItem = _edit_event.Schedule[0].Event_room.Name_room;
             comboBox6.SelectedItem = _edit_event.Full_name;

             comboBox3.SelectedItem = _edit_event.Schedule[0].Date_event;

             comboBox4.SelectedItem = _edit_event.Schedule[0].Time_event;*/

            if (_key_FullSchForm == "empl") //выгрузить данные для сотрудника
            {
                //должность сотр, выгружать из бд по типу?
                //Находим сотрудника, которому добавляем мероприятие
                  MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                  conn.Open();

                  string sql = "SELECT id_employee, name_position " +
                     "FROM employee " +
                     "JOIN position " +
                     "ON employee.id_position = position.id_position " +
                     "WHERE full_name = @name ";

                  MySqlCommand command = new MySqlCommand(sql, conn);

                  command.Parameters.AddWithValue("@name", _edit_event.Full_name);
                  MySqlDataReader id_empl = command.ExecuteReader();
                  id_empl.Read();

                  _edit_event.Id_emloyee = Convert.ToInt32(id_empl[0]);
                  _edit_event.Name_posinion = Convert.ToString(id_empl[1]);

                  id_empl.Close();
                  conn.Close();

                conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

                conn.Open();
                sql = "SELECT name_event, name_event_type " +
                    "FROM event " +
                    "JOIN event_type " +
                    "ON event.id_event_type = event_type.id_event_type " +
                    "WHERE name_event_type LIKE '" + _edit_event.Name_posinion.Substring(0, 3) + "%'";

                command = new MySqlCommand(sql, conn);
                MySqlDataReader events_for_empl = command.ExecuteReader();

                while (events_for_empl.Read())
                {
                    comboBox1.Items.Add(Convert.ToString(events_for_empl[0]));
                }

                events_for_empl.Close();
                conn.Close();



            }
            else if (_key_FullSchForm == "room")
            {
                MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                conn.Open();

                string sql = "SELECT name_room, name_room_type " +
                   "FROM room " +
                   "JOIN room_type " +
                   "ON room.id_room_type = room_type.id_room_type " +
                   "WHERE name_room = @name ";

                MySqlCommand command = new MySqlCommand(sql, conn);

              //  MessageBox.Show(_edit_event.Schedule[0].Event_room.Name_room);

                command.Parameters.AddWithValue("@name", _edit_event.Schedule[0].Event_room.Name_room);
                MySqlDataReader id_room = command.ExecuteReader();
                id_room.Read();

                //_edit_event.Id_emloyee = Convert.ToInt32(id_room[0]);
                _edit_event.Schedule[0].Event_room.Name_room_type = Convert.ToString(id_room[1]);

                id_room.Close();
                conn.Close();

                conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

                conn.Open();
                sql = "SELECT name_event, name_event_type " +
                    "FROM event " +
                    "JOIN event_type " +
                    "ON event.id_event_type = event_type.id_event_type " +
                    "WHERE name_event_type LIKE ";

              //  MessageBox.Show(_edit_event.Schedule[0].Event_room.Name_room_type);

                if (_edit_event.Schedule[0].Event_room.Name_room_type.Substring(0, 3) == "муз")
                {
                    sql += "'экс%'";
                }
                else if (_edit_event.Schedule[0].Event_room.Name_room_type.Substring(0, 3) == "зал")
                {
                    sql += "'кин%'";
                }
                else
                {
                    sql += "'" + _edit_event.Schedule[0].Event_room.Name_room_type.Substring(0, 3) + "%'";
                }

                command = new MySqlCommand(sql, conn);
                MySqlDataReader events_for_room = command.ExecuteReader();

                while (events_for_room.Read())
                {
                    comboBox1.Items.Add(Convert.ToString(events_for_room[0]));
                }

                events_for_room.Close();
                conn.Close();


            }

        }

        private void SaveEdit() //Сохранение изменений
        {
            if (!string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrEmpty(_selectedPositData) && !string.IsNullOrEmpty(_selectedPositEmpl) && !string.IsNullOrEmpty(_selectedPositEvent) && !string.IsNullOrEmpty(_selectedPositRoom) && !string.IsNullOrEmpty(_selectedPositTime) && !string.IsNullOrEmpty(_selectedPositType))
            {
                if (Convert.ToInt32(textBox2.Text) < 4 && Convert.ToInt32(textBox2.Text) >= 1)
                {
                    if (Convert.ToInt32(textBox3.Text) < 40 && Convert.ToInt32(textBox3.Text) >= 10)
                    {
                        Dictionary<string, string> edit_event = new Dictionary<string, string>(); //Ассоциативный массив 

                        //Находим id выбранного мероприятия
                        MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                        conn.Open();

                        string sql = "SELECT id_event, name_event " +
                            "FROM event " +
                            "WHERE name_event = @name ";

                        MySqlCommand command = new MySqlCommand(sql, conn);
                        command.Parameters.Add("@name", MySqlDbType.VarChar).Value = _selectedPositEvent;
                        MySqlDataReader ed_event = command.ExecuteReader();
                        ed_event.Read();

                        edit_event.Add("id_event", Convert.ToString(ed_event[0]));

                        ed_event.Close();
                        conn.Close();

                        //Находим id выбранного помещения
                        conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                        conn.Open();

                        sql = "SELECT id_room, name_room " +// name_room, data_event, time_event, full_name " +
                            "FROM room " +
                            // "JOIN schedule " +
                            //  "ON event.id_event = schedule.id_event " +
                            // "JOIN room " +
                            //  "ON schedule.id_room = room.id_room " +
                            //  "JOIN schedule_empl " +
                            // "ON schedule.id_schedule_entry = schedule_empl.id_event_entry " +
                            //  "JOIN employee " +
                            //   "ON schedule_empl.id_employee = employee.id_employee " +
                            "WHERE name_room = @name ";

                        command = new MySqlCommand(sql, conn);
                        command.Parameters.Add("@name", MySqlDbType.VarChar).Value = _selectedPositRoom;
                        ed_event = command.ExecuteReader();
                        ed_event.Read();

                        edit_event.Add("id_room", Convert.ToString(ed_event[0]));

                        ed_event.Close();
                        conn.Close();

                        //Находим id редактируемой записи
                        conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                        conn.Open();

                        sql = "SELECT id_schedule_entry " +
                            "FROM schedule " +
                            "WHERE id_room = @id_r AND data_event = @data AND time_event = @time";

                        command = new MySqlCommand(sql, conn);
                        command.Parameters.AddWithValue("@id_r", edit_event["id_room"]);
                        command.Parameters.AddWithValue("@data", _edit_event.Schedule[0].Date_event);
                        command.Parameters.AddWithValue("@time", _edit_event.Schedule[0].Time_event);
                        ed_event = command.ExecuteReader();
                        ed_event.Read();

                        edit_event.Add("id_schedule", Convert.ToString(ed_event[0]));

                        ed_event.Close();
                        conn.Close();

                        //Исправляем запись в расписании
                        conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

                        sql = "UPDATE schedule SET id_event = @id_ev, id_room = @id_r, " +
                            "data_event = @data, duration_event = @duration, numb_seats =  @numb, time_event = @time " +
                            "WHERE id_schedule_entry = @id";
                        command = new MySqlCommand(sql, conn);

                        command.Parameters.AddWithValue("@id_ev", edit_event["id_event"]);
                        command.Parameters.AddWithValue("@id_r", edit_event["id_room"]);
                        command.Parameters.AddWithValue("@data", _selectedPositData);
                        command.Parameters.AddWithValue("@duration", textBox2.Text);
                        command.Parameters.AddWithValue("@numb", textBox3.Text);
                        command.Parameters.AddWithValue("@time", _selectedPositTime);
                        command.Parameters.AddWithValue("@id", edit_event["id_schedule"]);

                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        command.Connection.Close();

                        //Нахадим id выбнанного сотрудника
                        conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                        conn.Open();

                        sql = "SELECT id_employee " +
                           "FROM employee " +
                           "WHERE full_name = @name ";

                        command = new MySqlCommand(sql, conn);

                        command.Parameters.AddWithValue("@name", _selectedPositEmpl);
                        ed_event = command.ExecuteReader();
                        ed_event.Read();

                        edit_event.Add("id_empl", Convert.ToString(ed_event[0]));

                        ed_event.Close();
                        conn.Close();

                        //Нахадим id редактируемой записи в расписании сотрудников
                        conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                        conn.Open();

                        sql = "SELECT id_schedule_entry " +
                           "FROM schedule_empl " +
                           "WHERE id_event_entry = @id_ev AND id_employee = @id_empl";

                        command = new MySqlCommand(sql, conn);

                        command.Parameters.AddWithValue("@id_ev", edit_event["id_schedule"]);
                        command.Parameters.AddWithValue("@id_empl", edit_event["id_empl"]);

                        ed_event = command.ExecuteReader();
                        ed_event.Read();

                        edit_event.Add("id_sched_emp", Convert.ToString(ed_event[0]));

                        ed_event.Close();
                        conn.Close();


                        //Исправляем запись в расписании сотрудников
                        conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

                        sql = "UPDATE schedule_empl SET id_employee = @id_empl " +
                            "WHERE  id_schedule_entry = @id_sch";
                        command = new MySqlCommand(sql, conn);

                        command.Parameters.AddWithValue("@id_sch", edit_event["id_sched_emp"]);
                        command.Parameters.AddWithValue("@id_empl", edit_event["id_empl"]);

                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        command.Connection.Close();

                        MessageBox.Show("Изменения сохранены.");
                        this.Close();
                        _fullScheduleForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Планетарий не располагает таким количеством мест (необходимо указать от 10 до 40 мест)");
                    }
                }
                else
                {
                    MessageBox.Show("Укажите реальную длительность (не более 4-х часов)");
                }
            }
            else
            {
                MessageBox.Show("Проверьте, все ли поля заполнены!");
            }
        }

        private void AddInSchedule() //Добавление мероприятия в расписание
        {
            if (!string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrEmpty(_selectedPositData) && !string.IsNullOrEmpty(_selectedPositEmpl) && !string.IsNullOrEmpty(_selectedPositEvent) && !string.IsNullOrEmpty(_selectedPositRoom) && !string.IsNullOrEmpty(_selectedPositTime) && !string.IsNullOrEmpty(_selectedPositType))
            {
                if (Convert.ToInt32(textBox2.Text) < 4 && Convert.ToInt32(textBox2.Text) >= 1)
                {
                    if (Convert.ToInt32(textBox3.Text) < 40 && Convert.ToInt32(textBox3.Text) >= 10)
                    {
                        Dictionary<string, string> edit_event = new Dictionary<string, string>(); //Ассоциативный массив 

                        //Находим id выбранного мероприятия
                        MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                        conn.Open();

                        string sql = "SELECT id_event, name_event " +
                            "FROM event " +
                            "WHERE name_event = @name ";

                        MySqlCommand command = new MySqlCommand(sql, conn);
                        command.Parameters.Add("@name", MySqlDbType.VarChar).Value = _selectedPositEvent;
                        MySqlDataReader ed_event = command.ExecuteReader();
                        ed_event.Read();

                        edit_event.Add("id_event", Convert.ToString(ed_event[0]));

                        ed_event.Close();
                        conn.Close();

                        //Находим id выбранного помещения
                        conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                        conn.Open();

                        sql = "SELECT id_room, name_room " +
                            "FROM room " +
                            "WHERE name_room = @name ";

                        command = new MySqlCommand(sql, conn);
                        command.Parameters.Add("@name", MySqlDbType.VarChar).Value = _selectedPositRoom;
                        ed_event = command.ExecuteReader();
                        ed_event.Read();

                        edit_event.Add("id_room", Convert.ToString(ed_event[0]));

                        ed_event.Close();
                        conn.Close();

                        /* //Находим id редактируемой записи
                         conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                         conn.Open();

                         sql = "SELECT id_schedule_entry " +
                             "FROM schedule " +
                             "WHERE id_room = @id_r AND data_event = @data AND time_event = @time";

                         command = new MySqlCommand(sql, conn);
                         command.Parameters.AddWithValue("@id_r", edit_event["id_room"]);
                         command.Parameters.AddWithValue("@data", _edit_event.Schedule[0].Date_event);
                         command.Parameters.AddWithValue("@time", _edit_event.Schedule[0].Time_event);
                         ed_event = command.ExecuteReader();
                         ed_event.Read();

                         edit_event.Add("id_schedule", Convert.ToString(ed_event[0]));

                         ed_event.Close();
                         conn.Close();*/

                        //Вставляем запись в расписание
                        conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

                        sql = "INSERT INTO schedule (id_event, id_room, data_event, duration_event, numb_seats, time_event) " +
                            "VALUES (@id_ev, @id_r, @data, @duration, @numb, @time)";
                        command = new MySqlCommand(sql, conn);

                        command.Parameters.AddWithValue("@id_ev", edit_event["id_event"]);
                        command.Parameters.AddWithValue("@id_r", edit_event["id_room"]);
                        command.Parameters.AddWithValue("@data", _selectedPositData);
                        command.Parameters.AddWithValue("@duration", textBox2.Text);
                        command.Parameters.AddWithValue("@numb", textBox3.Text);
                        command.Parameters.AddWithValue("@time", _selectedPositTime);
                        //  command.Parameters.AddWithValue("@id", edit_event["id_schedule"]);

                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        command.Connection.Close();

                        //Находим id только что вставленной записи
                        conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                        conn.Open();

                        sql = "SELECT id_schedule_entry " +
                            "FROM schedule " +
                            "WHERE id_room = @id_r AND data_event = @data AND time_event = @time";

                        command = new MySqlCommand(sql, conn);
                        command.Parameters.AddWithValue("@id_r", edit_event["id_room"]);
                        command.Parameters.AddWithValue("@data", _selectedPositData);
                        command.Parameters.AddWithValue("@time", _selectedPositTime);
                        ed_event = command.ExecuteReader();
                        ed_event.Read();

                        edit_event.Add("id_schedule", Convert.ToString(ed_event[0]));

                        ed_event.Close();
                        conn.Close();

                        //Нахадим id выбнанного сотрудника
                        conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                        conn.Open();

                        sql = "SELECT id_employee " +
                           "FROM employee " +
                           "WHERE full_name = @name ";

                        command = new MySqlCommand(sql, conn);

                        command.Parameters.AddWithValue("@name", _selectedPositEmpl);
                        ed_event = command.ExecuteReader();
                        ed_event.Read();

                        edit_event.Add("id_empl", Convert.ToString(ed_event[0]));

                        ed_event.Close();
                        conn.Close();

                        /*    //Нахадим id редактируемой записи в расписании сотрудников
                            conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                            conn.Open();

                            sql = "SELECT id_schedule_entry " +
                               "FROM schedule_empl " +
                               "WHERE id_event_entry = @id_ev AND id_employee = @id_empl";

                            command = new MySqlCommand(sql, conn);

                            command.Parameters.AddWithValue("@id_ev", edit_event["id_schedule"]);
                            command.Parameters.AddWithValue("@id_empl", edit_event["id_empl"]);

                            ed_event = command.ExecuteReader();
                            ed_event.Read();

                            edit_event.Add("id_sched_emp", Convert.ToString(ed_event[0]));

                            ed_event.Close();
                            conn.Close();*/


                        //Добавляем запись в расписание сотрудников
                        conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

                        sql = "INSERT INTO schedule_empl (id_event_entry, id_employee) " +
                            "VALUES  (@id_sch, @id_empl)";
                        command = new MySqlCommand(sql, conn);

                        command.Parameters.AddWithValue("@id_sch", edit_event["id_schedule"]);
                        command.Parameters.AddWithValue("@id_empl", edit_event["id_empl"]);

                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        command.Connection.Close();

                        MessageBox.Show("Изменения сохранены.");
                        this.Close();
                        _fullScheduleForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Планетарий не располагает таким количеством мест (необходимо указать от 10 до 40 мест)");
                    }
                }
                else
                {
                    MessageBox.Show("Укажите реальную длительность (не более 4-х часов)");
                }
            }
            else
            {
                MessageBox.Show("Проверьте, все ли поля заполнены!");
            }
        }

        private void AddInEvTick() //Установка новой цены
        {
            if (!string.IsNullOrEmpty(textBox4.Text))
            {
                if (Convert.ToInt32(textBox4.Text) < 4000 && Convert.ToInt32(textBox4.Text) >= 100)
                {
                    if (!string.IsNullOrEmpty(_new_event.Name_event))
                    {
                        string id_event; //id мероприятия, на которое устанавливается новая цена

                        //Находим id выбранного мероприятия
                        MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                        conn.Open();

                        string sql = "SELECT id_event, name_event " +
                            "FROM event " +
                            "WHERE name_event = @name ";

                        MySqlCommand command = new MySqlCommand(sql, conn);
                        command.Parameters.Add("@name", MySqlDbType.VarChar).Value = _new_event.Name_event;
                        MySqlDataReader ed_event = command.ExecuteReader();
                        ed_event.Read();

                        id_event = Convert.ToString(ed_event[0]);

                        ed_event.Close();
                        conn.Close();


                        //Вставляем запись в таблицу цен мероприятий
                        conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

                        sql = "INSERT INTO ticket_price (id_event, price, install_date) " +
                            "VALUES (@id_ev, @price, @data)";
                        command = new MySqlCommand(sql, conn);

                        command.Parameters.AddWithValue("@id_ev", id_event);
                        command.Parameters.AddWithValue("@price", textBox4.Text);
                        command.Parameters.AddWithValue("@data", textBox5.Text);

                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        command.Connection.Close();

                        MessageBox.Show("Изменения сохнанены.");
                        this.Close();
                        _editRepertForm.Show();
                    }
                    else
                    {

                        //Находим id выбранного типа мероприятия

                        MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                        conn.Open();

                        string sql = "SELECT id_event_type " +
                            "FROM event_type " +
                            "WHERE name_event_type = @name ";

                        MySqlCommand command = new MySqlCommand(sql, conn);
                        command.Parameters.Add("@name", MySqlDbType.VarChar).Value = _selectedPositType;
                        MySqlDataReader ed_event = command.ExecuteReader();
                        ed_event.Read();

                        string id_type = Convert.ToString(ed_event[0]);

                        ed_event.Close();
                        conn.Close();

                        //Вставляем новое мероприятие в репертуар
                        conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                        

                        sql = "INSERT INTO event (id_event_type, name_event) " +
                            "VALUES (@id_type, @name)";

                        command = new MySqlCommand(sql, conn);

                        command.Parameters.AddWithValue("@id_type", id_type);
                        command.Parameters.AddWithValue("@name", textBox1.Text);

                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        command.Connection.Close();

                        //Находим id выбранного мероприятия

                        conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
                        conn.Open();

                        sql = "SELECT id_event " +
                            "FROM event " +
                            "WHERE name_event = @name ";

                        command = new MySqlCommand(sql, conn);
                        command.Parameters.Add("@name", MySqlDbType.VarChar).Value = textBox1.Text;
                        ed_event = command.ExecuteReader();
                        ed_event.Read();

                        string id_ev = Convert.ToString(ed_event[0]);

                        ed_event.Close();
                        conn.Close();

                        //Вставляем запись в таблицу цен мероприятий
                        conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

                        sql = "INSERT INTO ticket_price (id_event, price, install_date) " +
                            "VALUES (@id_ev, @price, @data)";
                        command = new MySqlCommand(sql, conn);

                        command.Parameters.AddWithValue("@id_ev", id_ev);
                        command.Parameters.AddWithValue("@price", textBox4.Text);
                        command.Parameters.AddWithValue("@data", textBox5.Text);

                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        command.Connection.Close();

                        MessageBox.Show("Изменения сохнанены.");
                        this.Close();
                        _editRepertForm.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Укажите реальную цену (от 100 до 4000 рублей)") ;
                }
            }
            else
            {
                MessageBox.Show("Проверьте, все ли поля заполнены!");
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Ограничение на ввод букв
            if (Char.IsDigit(e.KeyChar) | e.KeyChar==8) return;
            else
                e.Handled = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Ограничение на ввод букв
            if (Char.IsDigit(e.KeyChar) | e.KeyChar == 8) return;
            else
                e.Handled = true;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Ограничение на ввод букв
            if (Char.IsDigit(e.KeyChar) | e.KeyChar == 8) return;
            else
                e.Handled = true;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar) | e.KeyChar == 8 | e.KeyChar == 32) return;
            else
                e.Handled = true;
        }
    }
}
