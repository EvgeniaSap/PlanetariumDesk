using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planetarium
{
    public class Employee
    {
        public int Id_emloyee { set; get; } //ID сотрудника
        public string Full_name { set; get; } //ФИО сотрудника
        public int Id_position { set; get; } //ID должности
        public string Name_posinion { set; get; } //Название должности
        public int Id_account { set; get; } //ID аккаунта сотрудника
        public List<Event> Schedule { set; get; } //Расписание мероприятий сотрудника;

        public Employee()
        {
            Schedule = new List<Event>();
        }

        public Employee(string full, string name_type, string name, string date, string time, string duration, string name_room, string numb_of_seats)
        {
            Full_name = full;
            Schedule = new List<Event>();
            Schedule.Add(new Planetarium.Event(name_type, name, date, time, duration, name_room, numb_of_seats));
        }

        public Employee(int id, string full, int id_pos, string name_pos)
        {
            Schedule = new List<Event>();
            Id_emloyee = id;
            Full_name = full;
            Id_position = id_pos;
            Name_posinion = name_pos;
        }

        public Employee(int id, string full, int id_pos, string name_pos, int id_acc)
        {
            Schedule = new List<Event>();
            Id_emloyee = id;
            Full_name = full;
            Id_position = id_pos;
            Name_posinion = name_pos;
            Id_account = id_acc;
        }
    }

    public class Event
    {
        public int Id_schedule { set; get; } //ID мероприятия в общем расписании
        public string Name_event_type { set; get; } //Название типа мероприятия
        public string Name_event { set; get; } //Название мероприятия
        public Room Event_room { set; get; } //Помещения, в котором проходит мероприятие
        public string Date_event { set; get; } //Дата проведения
        public string Time_event { set; get; } //Время проведения
        public string Duration_event { set; get; } //Длительность проведения
        public string Numb_of_seats { set; get; } //Количество мест
        public string Id_event { set; get; } //ID мероприятия

        public Event()
        {
            Event_room = new Room();
        }

        public Event(int id_sch, string name)
        {
            Id_schedule = id_sch;
            Name_event = name;
            Event_room = new Room();
        }

        public Event(string name_type, string name, string date, string time, string duration, string name_room, string numb_of_seats)
        {
            Name_event_type = name_type;
            Name_event = name;
            Date_event = date;
            Time_event = time;
            Duration_event = duration;
            Numb_of_seats = numb_of_seats;
            Event_room = new Room(name_room);
        }

        public Event(string name_type, string name, string data)
        {
            Name_event_type = name_type;
            Name_event = name;
            Date_event = data;
            Event_room = new Room();
        }
    }

    public class Room
    {
        public int Id_room { set; get; } //ID помещения
        public string Name_room { set; get; } //Название помещения
        public int Id_room_type { set; get; } //ID типа помещения
        public string Name_room_type { set; get; } //Название типа помещения

        public Room()
        {
        }

        public Room(string name)
        {
            Name_room = name;
        }

        public Room(int id, string name, int id_type, string name_type)
        {
            Id_room = id;
            Name_room = name;
            Id_room_type = id_type;
            Name_room_type = name_type;
        }
    }
}
