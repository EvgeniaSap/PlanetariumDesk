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
    public partial class AdminMainForm : Form
    {
        private AuthForm _authForm; //Форма авторизации;
        private RegForm _regForm; //Форма регистрации;
        private int _i = 0; //Индекс для отображения мероприятий
        private int _listInd = -1; //Индекс выбранного элемента в listbox1
        private List<Event> _some_events; //Мароприятие, которое админ решит редактировать
        private List<Employee> _employees; //Сотрудник, которого решит редактировать админ
        private List<Room> _rooms; //Помещение, которое решит ред админ

        public AdminMainForm()
        {
            InitializeComponent();
        }

        public AdminMainForm(AuthForm authForm) //Переход с формы авторизации
        {
            _authForm = authForm;
            InitializeComponent();
        }

        public AdminMainForm(AuthForm authForm, RegForm regForm) //Переход с формы регистрации - надо ли?
        {
            _authForm = authForm;
            _regForm = regForm;
            InitializeComponent();
        }

        private void лекцииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _listInd = -1;
            button1.Text = "Расписание лекции";
            _i = 1;
            OutputEvents(); //Выводим перечень лекций
        }

        private void экскурсииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _listInd = -1;
            button1.Text = "Расписание экскурсии";
            _i = 2;
            OutputEvents(); //Выводим перечень экскурсий
        }

        private void фильмыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _listInd = -1;
            button1.Text = "Расписание фильма";
            _i = 3;
            OutputEvents(); //Выводим перечень фильмов
        }

        private void лабыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _listInd = -1;
            button1.Text = "Расписание лабораторного занятия";
            _i = 4;
            OutputEvents(); //Выводим перечень лабораторных занятий
        }
        
        private void всеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _listInd = -1;
            button1.Text = "Расписание мероприятия";
            _i = 0;
            OutputEvents(); //Выводим весь репертуар 
        }
        
        private void сотрудникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _listInd = -1;
            button1.Text = "Расписание сотрудника";
            OutputEmplOrRoom(0); //Выводим перечень сотрудников
        }

        private void помещенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _listInd = -1;
            button1.Text = "Расписание помещения";
            OutputEmplOrRoom(1); //Выводим перечеь помещений 
        }

        private void билетыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _listInd = -1;
            button1.Text = "Подробнее о билетах";
            _i = 5;
            OutputEvents(); //Выводим список мероприятий для предоставления просмотра билетов на них
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            _authForm.Visible = true; //Переход на форму авторизации
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _listInd = listBox1.SelectedIndex; //Индекс выбранного клиента;
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void AdminMainForm_Load(object sender, EventArgs e) //Загрузка формы
        {
            OutputEvents();
            button1.Text = "Расписание мероприятия";
            button2.Text = "Расписание планетария";
        }

        private void button1_Click(object sender, EventArgs e) //Нажатие на кнопку редактирования при выбранном пункте listbox1 
        {
            if (_listInd!=-1)
            {
                if (_i == 0 || _i == 1 || _i == 2 || _i == 3 || _i == 4)
                {
                    this.Hide();
                    FullScheduleForm full_sched = new FullScheduleForm(_authForm, this, "event", _some_events[_listInd]) { Visible = true }; //Открываем форму для просмотра расписания мероприятия
                }
                else if (_i == 5)
                {
                    this.Hide();
                    FullScheduleForm full_sched = new FullScheduleForm(_authForm, this, "tick", _some_events[_listInd]) { Visible = true }; //Открываем форму для просмотра расписания мероприятия

                }
                else if (_i == 6)
                {
                    this.Hide();
                    FullScheduleForm full_sched = new FullScheduleForm(_authForm, this, "empl", _employees[_listInd]) { Visible = true }; //Открываем форму для просмотра всего расписания сотрудника

                }
                else if (_i == 7)
                {
                    this.Hide();
                    FullScheduleForm full_sched = new FullScheduleForm(_authForm, this, "room", _rooms[_listInd]) { Visible = true }; //Открываем форму для просмотра всего расписания сотрудника
                }
            }
            else
            {
                MessageBox.Show("Для начала выберите элемент из списка.");
            }
        }

        private void button2_Click(object sender, EventArgs e) //Нажатие на кнопку перейти к полному расписанию планетария
        {
            this.Hide();
            FullScheduleForm full_sched = new FullScheduleForm(_authForm, this, "full") { Visible = true }; //Открываем форму для просмотра всего расписания
        }

        private void OutputEvents() //Выводим в listbox1 новые значения
        {
            listBox1.Items.Clear();
            string[] name_types = { "Все мероприятия:", "Лекции:", "Экскурсии:", "Фильмы:" , "Лабораторные занятия:", "Просмотр билетов на мероприятия:" };
            label2.Text = name_types[_i];
            _some_events = new List<Event>();

            MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

            conn.Open();

            string sql = "SELECT id_event, name_event " +
                    "FROM event ";

            if (_i != 0 && _i != 5)
            {
                sql += "WHERE event.id_event_type = @id";
            }

            MySqlCommand command = new MySqlCommand(sql, conn);

            if (_i != 0 && _i != 5)
            {
                command.Parameters.Add("@id", MySqlDbType.VarChar).Value = Convert.ToString(_i);
            }

                MySqlDataReader all_events = command.ExecuteReader();

                int i = 0; //Индекс для строки в списке;
                string str; //Текст для строки с списке;

                while (all_events.Read())
                {
                    _some_events.Add(new Event((int)all_events[0], all_events[1].ToString())); //Добавление мероприятия в список;

                    str = i + 1 + ") " + all_events[1].ToString();
                    listBox1.Items.Insert(i, str);
                
                    i++;
                }
                all_events.Close();
            conn.Close();
        }

        private void OutputEmplOrRoom(int _ind) //Выводим в listbox1 новые значения
        {
            listBox1.Items.Clear();
            string[] name_types = { "Сотрудники:", "Помещения:" };
            label2.Text = name_types[_ind];
            _employees = new List<Employee>();
            _rooms = new List<Room>();
            
            MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;
            conn.Open();
            if (_ind == 0)
            {
                _i = 6;
                string sql = "SELECT id_employee, full_name, name_position, position.id_position, employee.id_position " +
                    "FROM employee "+
                    "JOIN position "+
                    "ON employee.id_position = position.id_position";
                
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataReader all_empl = command.ExecuteReader();

                int i = 0; //Индекс для строки в списке;
                string str; //Текст для строки с списке;

                while (all_empl.Read())
                {
                    _employees.Add(new Employee((int)all_empl[0], all_empl[1].ToString(), (int)all_empl[4], all_empl[2].ToString())); //Добавление мероприятия в список;
                    str = i + 1 + ") " + all_empl[1].ToString() + ", " + all_empl[2].ToString();
                    listBox1.Items.Insert(i, str);
                    i++;
                }
                all_empl.Close();
            }
            else
            {
                _i = 7;
                string sql = "SELECT id_room, name_room, room.id_room_type, name_room_type " +
                    "FROM room " +
                    "JOIN room_type " +
                    "ON room.id_room_type = room_type.id_room_type";

                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataReader all_rooms = command.ExecuteReader();

                int i = 0; //Индекс для строки в списке;
                string str; //Текст для строки с списке;
                while (all_rooms.Read())
                {
                    _rooms.Add(new Room((int)all_rooms[0], all_rooms[1].ToString(), (int)all_rooms[2], all_rooms[3].ToString())); //Добавление мероприятия в список;

                    str = i + 1 + ") " + all_rooms[1].ToString() + " (" + all_rooms[3].ToString() + ")";
                    listBox1.Items.Insert(i, str);
                    i++;
                }
                all_rooms.Close();
            }
            conn.Close();
        }

        private void button3_Click(object sender, EventArgs e) //Кнопка редактирования репертуара
        {
            this.Hide();
            EditRepertForm full_sched = new EditRepertForm(_authForm, this) { Visible = true }; //Открываем форму для редактирования репертуара
        }
    }
}
