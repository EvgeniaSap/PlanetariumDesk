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
    public partial class UserAccForm : Form
    {
        private AuthForm _authForm; //Форма авторизации;
        private Employee _user; //ID сотрудника;
        private string _selectedPosit; //Выбранная дата
        private List<Room> _rooms; //Список помещений
        private int _listInd = -1; //Индекс выбранного элемента в listbox1

        public UserAccForm()
        {
            InitializeComponent();
        }

        public UserAccForm(AuthForm authForm, int id)
        {
            _authForm = authForm;
            _user = new Employee();
            _user.Id_account = id;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) //Кнопка выйти
        {
            this.Close();
            _authForm.Show();
        }

        private void просмотрРасписанияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label3.Visible = true;
            comboBox1.Visible = true;
            dataGridView1.Visible = true;
            listBox1.Visible = false;
            label4.Text = "Расписание:";

           // dataGridView1.Rows.Clear();
            //OutputDataGrid(); //Отображение столбцов
           // OutputEvents(); //Выводим в DataGridView новые значения
        }

        private void UserAccForm_Load(object sender, EventArgs e)
        {
            MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

            conn.Open();
            string sql = "SELECT full_name, id_employee, employee.id_position, name_position " +
                "FROM account " +
                "JOIN employee " +
                "ON account.id_account = employee.id_account " +
                "JOIN position " +
                "ON employee.id_position = position.id_position " +
                "WHERE employee.id_account = @id ";

         //   MessageBox.Show(_user.Id_account.ToString());
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = _user.Id_account;
            MySqlDataReader employee = command.ExecuteReader();
            employee.Read();

            _user = new Employee(Convert.ToInt32(employee[1]), employee[0].ToString(), Convert.ToInt32(employee[2]), employee[3].ToString(), _user.Id_account);
             
            employee.Close();
            conn.Close();

            label2.Text += " " + _user.Full_name + ", " + _user.Name_posinion + ".";

            DateTime date1 = DateTime.Today; //Дата сейчас
            
            for (int i = 0; i < 31; i++)
            {
                comboBox1.Items.Add(Convert.ToString(date1.AddDays(i)).Substring(0, 10)); //Выводим в combobox1 даты
            }

            OutputDataGrid(); //Отображение столбцов

        }

        private void OutputEvents() //Выводим в DataGridView новые значения
        {
            MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

            conn.Open();
            string sql = "SELECT time_event, name_event, duration_event, name_room " +
                "FROM event " +
                "JOIN schedule " +
                "ON event.id_event = schedule.id_event " +
                "JOIN room " +
                "ON schedule.id_room = room.id_room " +
                "JOIN schedule_empl " +
                "ON schedule.id_schedule_entry = schedule_empl.id_event_entry " +
                "JOIN employee " +
                "ON schedule_empl.id_employee = employee.id_employee " +
                "WHERE data_event = @data AND full_name = @name " +
                "ORDER BY time_event";

          //  MessageBox.Show(_selectedPosit +" "+ _user.Id_account);
            
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add("@data", MySqlDbType.VarChar).Value = _selectedPosit;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = _user.Full_name;
            MySqlDataReader date_events = command.ExecuteReader();


            while (date_events.Read())
            {
                dataGridView1.Rows.Add(date_events[0].ToString(), date_events[1].ToString(), date_events[2].ToString(), date_events[3].ToString());

            }

            date_events.Close();
            conn.Close();
            dataGridView1.AllowUserToAddRows = false; //запрещаем пользователю самому добавлять строки


        }

        private void OutputDataGrid() //Отображение столбцов
        {
            //время, название, длительность, помещение
            var column1 = new DataGridViewColumn();
            column1.HeaderText = "Время"; //текст в шапке
            //column1.Width = 300; //ширина колонки
            //column1.ReadOnly = true; //значение в этой колонке нельзя править
            column1.Name = "Time"; //текстовое имя колонки, его можно использовать вместо обращений по индексу
            //column1.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
            column1.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки

            //Height
            var column2 = new DataGridViewColumn();
            column2.HeaderText = "Название";
            column2.Width = 200; //ширина колонки
            column2.Name = "Name";
            column2.CellTemplate = new DataGridViewTextBoxCell();

            var column3 = new DataGridViewColumn();
            column3.HeaderText = "Длительность";
            column3.Width = 130; //ширина колонки
            column3.Name = "Duration";
            column3.CellTemplate = new DataGridViewTextBoxCell();

            var column4 = new DataGridViewColumn();
            column4.HeaderText = "Помещение";
            column4.Name = "Room";
            column4.CellTemplate = new DataGridViewTextBoxCell();


            dataGridView1.Columns.Add(column1);
            dataGridView1.Columns.Add(column2);
            dataGridView1.Columns.Add(column3);
            dataGridView1.Columns.Add(column4);
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridView1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 10f, FontStyle.Regular); //жирный курсив размера 16

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedPosit = comboBox1.SelectedItem.ToString();
            dataGridView1.Rows.Clear();
            OutputEvents();
        }

        private void расписаниеПомещенийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label3.Visible = false;
            comboBox1.Visible =false;
            dataGridView1.Visible = false;
            label4.Text = "Помещения:";
            listBox1.Visible = true;
            OutputRoom(); //Выводим в listbox1 новые значения


        }

        private void OutputRoom() //Выводим в listbox1 новые значения
        {
            listBox1.Items.Clear();
            _rooms = new List<Room>();


            MySqlConnection conn = BDUtils.GetDBConnection(); //Получаем объект, подключенный к бд;

            conn.Open();
           
               // _i = 7;
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
            
            conn.Close();

           
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _listInd = listBox1.SelectedIndex; //Индекс выбранного клиента;
               this.Hide();
               FullScheduleForm full_sched = new FullScheduleForm(_authForm, this, "room", _rooms[_listInd], "user") { Visible = true }; //Открываем форму для просмотра всего расписания сотрудника
        }

     /*  private void предложитьМероприятиеToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }*/
    }
}
