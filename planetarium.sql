-- phpMyAdmin SQL Dump
-- version 4.8.4
-- https://www.phpmyadmin.net/
--
-- Хост: 127.0.0.1:3306
-- Время создания: Сен 30 2022 г., 02:49
-- Версия сервера: 5.7.24
-- Версия PHP: 7.2.14

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `planetarium`
--

-- --------------------------------------------------------

--
-- Структура таблицы `account`
--

DROP TABLE IF EXISTS `account`;
CREATE TABLE IF NOT EXISTS `account` (
  `id_account` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id аккаунта',
  `id_account_type` int(11) NOT NULL COMMENT 'id типа учетной записи',
  `login` varchar(30) NOT NULL COMMENT 'Логин',
  `password` varchar(32) NOT NULL COMMENT 'Пароль',
  `status` tinyint(1) NOT NULL COMMENT 'Пометка на удаление',
  PRIMARY KEY (`id_account`),
  UNIQUE KEY `id_account` (`id_account`),
  KEY `id_account_type` (`id_account_type`)
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=utf8 COMMENT='Учетные записи';

--
-- Дамп данных таблицы `account`
--

INSERT INTO `account` (`id_account`, `id_account_type`, `login`, `password`, `status`) VALUES
(17, 2, 'customer3', 'dad42d35ce6e8bb87abf89c87d78f007', 0),
(18, 2, 'customer4', '547a0f7d90884a1e5899af0d60355acd', 0),
(19, 2, 'Павел', '27091349945a40f23dddb7a6c32c24e1', 0),
(20, 2, 'Алина', 'a314ec99ed83b01292478f7c09f45ccf', 0),
(21, 2, 'customer5', '1f6a55c62713f87fa3c4e7f4f2ffcb02', 0),
(23, 1, 'admin', '34f7ee4dc6b206f9d9443734da97b165', 0),
(28, 3, 'lector1', 'a59a4e705f872d9284034e560f01bcac', 0),
(30, 3, 'emp1', 'c6010108e1182f262fc8640cab9e2f4f', 0),
(31, 2, 'admin1', 'a314ec99ed83b01292478f7c09f45ccf', 0),
(32, 2, 'Sass', 'a314ec99ed83b01292478f7c09f45ccf', 0),
(33, 2, 'Вова', 'a314ec99ed83b01292478f7c09f45ccf', 0),
(34, 2, 'abcde', 'a314ec99ed83b01292478f7c09f45ccf', 0),
(36, 3, 'lector2', '494c8fe856d373fbfb25736f97ab8334', 0),
(37, 3, 'kino1', 'd8a0beb68808b7be57b761e2f2a04027', 0),
(38, 3, 'empl1', '50ec7e255e2dca062e8643cb2cc7fa58', 0);

-- --------------------------------------------------------

--
-- Структура таблицы `account_type`
--

DROP TABLE IF EXISTS `account_type`;
CREATE TABLE IF NOT EXISTS `account_type` (
  `id_account_type` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id типа учетной записи',
  `name_account_type` varchar(20) NOT NULL COMMENT 'Название типа учетной записи',
  PRIMARY KEY (`id_account_type`),
  UNIQUE KEY `id_account_type` (`id_account_type`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COMMENT='Типы учетных записей';

--
-- Дамп данных таблицы `account_type`
--

INSERT INTO `account_type` (`id_account_type`, `name_account_type`) VALUES
(1, 'администратор'),
(2, 'заказчик'),
(3, 'сотрудник');

-- --------------------------------------------------------

--
-- Структура таблицы `coefficient`
--

DROP TABLE IF EXISTS `coefficient`;
CREATE TABLE IF NOT EXISTS `coefficient` (
  `id_coeff` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id коэффициента скидки',
  `name_coeff` varchar(30) NOT NULL COMMENT 'Название коэффициента',
  `value_coeff` double NOT NULL COMMENT 'Значение коэффициента',
  PRIMARY KEY (`id_coeff`),
  UNIQUE KEY `id_coeff` (`id_coeff`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='Коэффициенты для расчета цены билета с учетом скидок';

--
-- Дамп данных таблицы `coefficient`
--

INSERT INTO `coefficient` (`id_coeff`, `name_coeff`, `value_coeff`) VALUES
(1, 'детский билет', 0.5),
(2, 'взрослый билет', 1);

-- --------------------------------------------------------

--
-- Структура таблицы `customer`
--

DROP TABLE IF EXISTS `customer`;
CREATE TABLE IF NOT EXISTS `customer` (
  `id_customer` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id заказчика билетов',
  `id_account` int(11) NOT NULL COMMENT 'id аккаунта, с которого совершается покупка',
  `id_customers_type` int(11) NOT NULL COMMENT 'id типа заказчика',
  `name_organization` varchar(40) DEFAULT NULL COMMENT 'Название организации',
  `contact_person` varchar(50) NOT NULL COMMENT 'ФИО контактного лица',
  `phone` varchar(15) NOT NULL COMMENT 'Контактный телефон',
  PRIMARY KEY (`id_customer`),
  UNIQUE KEY `id_customer` (`id_customer`),
  KEY `id_account` (`id_account`),
  KEY `id_customers_type` (`id_customers_type`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8 COMMENT='Заказчики';

--
-- Дамп данных таблицы `customer`
--

INSERT INTO `customer` (`id_customer`, `id_account`, `id_customers_type`, `name_organization`, `contact_person`, `phone`) VALUES
(1, 17, 1, NULL, 'Иванов Иван Иванович', '89997776655'),
(9, 17, 1, '', 'Петров Петр Петрович', '89776554321'),
(10, 20, 1, '', 'Ромашкина Алина Семенова', '89445662345'),
(13, 17, 1, '', 'Павлов Павел Павлович', '89667553421'),
(14, 17, 1, '', 'Александров Александр Александрович', '89888998899'),
(19, 17, 1, '', 'Бокова Ксения Дмитриевна', '89997456722'),
(20, 17, 1, '', 'Бокова Ксения Дмитриевна', '89167851759'),
(21, 20, 1, 'ООО &#34;Солнышко&#34;', 'Бокова Ксения Дмитриевна', '89112334567'),
(22, 20, 1, 'рпавы', 'ывапр', 'орпав'),
(23, 20, 1, 'вап', 'ывап', 'ывап'),
(24, 20, 1, '', 'Степанов Степан Степанович', '89997456722'),
(25, 20, 2, 'ООО &#34;Солнышко&#34;', 'Романов Роман Романович', '89112334567'),
(26, 20, 1, '', 'Романов Роман Романович', '89167851759'),
(27, 20, 1, '', 'Романов Роман Романович', '8(953)818-20-81'),
(28, 20, 1, '', 'Романов Роман Романович', '89167851759'),
(29, 20, 1, '', 'Романов Роман Романович', '89112334567'),
(30, 20, 1, 'sdfghjiko', 'vgyhjkl', '34567890'),
(31, 20, 1, '', 'rtygfd', '34567890'),
(32, 20, 2, 'ООО &#34;Солнышко&#34;', 'Романов Роман Романович', '8(953)818-20-81');

-- --------------------------------------------------------

--
-- Структура таблицы `customers_type`
--

DROP TABLE IF EXISTS `customers_type`;
CREATE TABLE IF NOT EXISTS `customers_type` (
  `id_customers_type` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id типа заказчика',
  `name_customers_type` varchar(20) NOT NULL COMMENT 'Название типа заказчика',
  PRIMARY KEY (`id_customers_type`),
  UNIQUE KEY `id_customers_type` (`id_customers_type`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='Типы заказчиков билетов';

--
-- Дамп данных таблицы `customers_type`
--

INSERT INTO `customers_type` (`id_customers_type`, `name_customers_type`) VALUES
(1, 'индивидуально'),
(2, 'юридическое лицо');

-- --------------------------------------------------------

--
-- Структура таблицы `employee`
--

DROP TABLE IF EXISTS `employee`;
CREATE TABLE IF NOT EXISTS `employee` (
  `id_employee` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id сотрудника',
  `id_position` int(11) NOT NULL COMMENT 'id должности сотрудника',
  `id_account` int(11) NOT NULL COMMENT 'id учетной записи сотрудника',
  `full_name` varchar(50) NOT NULL COMMENT 'ФИО сотрудника',
  PRIMARY KEY (`id_employee`),
  UNIQUE KEY `id_employee` (`id_employee`),
  KEY `id_position` (`id_position`),
  KEY `id_account` (`id_account`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8 COMMENT='Перечень сотрудников';

--
-- Дамп данных таблицы `employee`
--

INSERT INTO `employee` (`id_employee`, `id_position`, `id_account`, `full_name`) VALUES
(1, 1, 28, 'Константинов Константин Константинович'),
(3, 2, 30, 'Павлов Павел Павлович'),
(5, 1, 36, 'Семенов Семен Семенович'),
(6, 3, 37, 'Ленин Владимир Ильич'),
(7, 2, 38, 'Яковлева Валерия Витальевна');

-- --------------------------------------------------------

--
-- Структура таблицы `event`
--

DROP TABLE IF EXISTS `event`;
CREATE TABLE IF NOT EXISTS `event` (
  `id_event` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id мероприятия',
  `id_event_type` int(11) NOT NULL COMMENT 'id типа мероприятия',
  `name_event` varchar(50) NOT NULL COMMENT 'Название мероприятия',
  PRIMARY KEY (`id_event`),
  UNIQUE KEY `id_event` (`id_event`),
  UNIQUE KEY `id_event_2` (`id_event`),
  KEY `id_event_type` (`id_event_type`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8 COMMENT='Репертуар планетария';

--
-- Дамп данных таблицы `event`
--

INSERT INTO `event` (`id_event`, `id_event_type`, `name_event`) VALUES
(1, 1, 'цикл лекций \"Жизнь звезд\"'),
(2, 1, 'Астрономия - наука о Вселенной'),
(3, 2, 'Первые искусственные спутники Земли'),
(4, 3, '\"Первый полет человека в космос\"'),
(5, 4, 'Юные исследователи Вселенной'),
(6, 2, 'Спутники - выход на орбиту'),
(7, 3, 'Космос вокруг нас');

-- --------------------------------------------------------

--
-- Структура таблицы `event_type`
--

DROP TABLE IF EXISTS `event_type`;
CREATE TABLE IF NOT EXISTS `event_type` (
  `id_event_type` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id ипа мероприятия',
  `name_event_type` varchar(20) NOT NULL COMMENT 'Название типа мероприятия',
  PRIMARY KEY (`id_event_type`),
  UNIQUE KEY `id_event_type` (`id_event_type`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COMMENT='Типы мероприятий';

--
-- Дамп данных таблицы `event_type`
--

INSERT INTO `event_type` (`id_event_type`, `name_event_type`) VALUES
(1, 'лекция'),
(2, 'экскурсия'),
(3, 'кинематография'),
(4, 'лабораторное занятие');

-- --------------------------------------------------------

--
-- Структура таблицы `position`
--

DROP TABLE IF EXISTS `position`;
CREATE TABLE IF NOT EXISTS `position` (
  `id_position` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id должности',
  `name_position` varchar(20) NOT NULL COMMENT 'Название должности',
  PRIMARY KEY (`id_position`),
  UNIQUE KEY `id_position` (`id_position`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COMMENT='Должности сотрудников';

--
-- Дамп данных таблицы `position`
--

INSERT INTO `position` (`id_position`, `name_position`) VALUES
(1, 'лектор'),
(2, 'экскурсовод'),
(3, 'киномеханик'),
(4, 'лаборант');

-- --------------------------------------------------------

--
-- Структура таблицы `room`
--

DROP TABLE IF EXISTS `room`;
CREATE TABLE IF NOT EXISTS `room` (
  `id_room` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id помещения планетария',
  `id_room_type` int(11) NOT NULL COMMENT 'id типа помещения',
  `name_room` varchar(30) NOT NULL COMMENT 'Название помещения',
  PRIMARY KEY (`id_room`),
  UNIQUE KEY `id_room` (`id_room`),
  KEY `id_room_type` (`id_room_type`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8 COMMENT='Помещения планетария';

--
-- Дамп данных таблицы `room`
--

INSERT INTO `room` (`id_room`, `id_room_type`, `name_room`) VALUES
(1, 1, 'Конференц-зал'),
(2, 3, 'Искусственные спутники Земли'),
(3, 4, 'Большой кинозал'),
(4, 2, 'Лаборатория 1'),
(5, 4, 'Малый кинозал');

-- --------------------------------------------------------

--
-- Структура таблицы `room_type`
--

DROP TABLE IF EXISTS `room_type`;
CREATE TABLE IF NOT EXISTS `room_type` (
  `id_room_type` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id типа помещения',
  `name_room_type` varchar(20) NOT NULL COMMENT 'Название типа помещения',
  PRIMARY KEY (`id_room_type`),
  UNIQUE KEY `id_room_type` (`id_room_type`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COMMENT='Тип помещения планетария';

--
-- Дамп данных таблицы `room_type`
--

INSERT INTO `room_type` (`id_room_type`, `name_room_type`) VALUES
(1, 'лекторий'),
(2, 'лаборатория'),
(3, 'музей'),
(4, 'зал');

-- --------------------------------------------------------

--
-- Структура таблицы `schedule`
--

DROP TABLE IF EXISTS `schedule`;
CREATE TABLE IF NOT EXISTS `schedule` (
  `id_schedule_entry` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id записи в расписании',
  `id_event` int(11) NOT NULL COMMENT 'id мероприятия из репертуара',
  `id_room` int(11) NOT NULL COMMENT 'id помещения для мероприятия',
  `data_event` varchar(16) NOT NULL COMMENT 'Дата проведения мероприятия',
  `duration_event` int(11) NOT NULL COMMENT 'Длительность мероприятия',
  `numb_seats` int(11) NOT NULL COMMENT 'Количество мест',
  `time_event` varchar(5) NOT NULL COMMENT 'Время проведения мероприятия',
  PRIMARY KEY (`id_schedule_entry`),
  UNIQUE KEY `id_schedule_entry` (`id_schedule_entry`),
  KEY `id_event` (`id_event`),
  KEY `id_room` (`id_room`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8 COMMENT='Расписание мероприятий';

--
-- Дамп данных таблицы `schedule`
--

INSERT INTO `schedule` (`id_schedule_entry`, `id_event`, `id_room`, `data_event`, `duration_event`, `numb_seats`, `time_event`) VALUES
(1, 2, 1, '22.06.2019', 2, 20, '10:30'),
(2, 2, 1, '24.06.2019', 2, 20, '16:15'),
(3, 2, 1, '27.06.2019', 2, 20, '13:00'),
(4, 2, 1, '22.06.2019', 2, 20, '16:00'),
(5, 1, 1, '22.06.2019', 2, 23, '13:15'),
(6, 2, 1, '01.07.2019', 2, 22, '10:00'),
(7, 1, 1, '01.07.2019', 1, 24, '13:00'),
(8, 3, 2, '01.07.2019', 2, 30, '12:00'),
(9, 6, 2, '01.07.2019', 2, 20, '14:30'),
(10, 3, 2, '01.07.2019', 1, 23, '10:00'),
(15, 6, 2, '02.07.2019', 1, 30, '10:00'),
(17, 4, 5, '03.07.2019', 1, 34, '13:00'),
(18, 6, 2, '08.07.2019', 2, 46, '16:00');

-- --------------------------------------------------------

--
-- Структура таблицы `schedule_empl`
--

DROP TABLE IF EXISTS `schedule_empl`;
CREATE TABLE IF NOT EXISTS `schedule_empl` (
  `id_schedule_entry` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id записи в расписании сотрудников',
  `id_event_entry` int(11) NOT NULL COMMENT 'id записи в расписании мероприятий',
  `id_employee` int(11) NOT NULL COMMENT 'id сотрудника',
  PRIMARY KEY (`id_schedule_entry`),
  UNIQUE KEY `id_schedule_entry` (`id_schedule_entry`),
  KEY `id_event_entry` (`id_event_entry`),
  KEY `id_employee` (`id_employee`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8 COMMENT='Расписание работы сотрудников';

--
-- Дамп данных таблицы `schedule_empl`
--

INSERT INTO `schedule_empl` (`id_schedule_entry`, `id_event_entry`, `id_employee`) VALUES
(1, 1, 1),
(2, 4, 1),
(3, 5, 1),
(4, 6, 1),
(5, 7, 1),
(6, 8, 3),
(7, 9, 3),
(12, 15, 3),
(14, 17, 6),
(15, 18, 7);

-- --------------------------------------------------------

--
-- Структура таблицы `ticket`
--

DROP TABLE IF EXISTS `ticket`;
CREATE TABLE IF NOT EXISTS `ticket` (
  `id_ticket` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id заказанных билетов',
  `id_customer` int(11) NOT NULL COMMENT 'id заказчика',
  `id_schedule` int(11) NOT NULL COMMENT 'id записи мероприятия в расписании',
  `id_coeff` int(11) NOT NULL COMMENT 'id коэффициента скидки',
  `numb_ticket` int(11) NOT NULL COMMENT 'Количество заказанных билетов',
  `final_price` double NOT NULL COMMENT 'Итоговая цена заказа билетов',
  `status` tinyint(1) NOT NULL COMMENT 'Пометка на удаление',
  PRIMARY KEY (`id_ticket`),
  UNIQUE KEY `id_ticket` (`id_ticket`),
  KEY `id_customer` (`id_customer`),
  KEY `id_schedule` (`id_schedule`,`id_coeff`),
  KEY `id_coeff` (`id_coeff`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8 COMMENT='Заказанные билеты';

--
-- Дамп данных таблицы `ticket`
--

INSERT INTO `ticket` (`id_ticket`, `id_customer`, `id_schedule`, `id_coeff`, `numb_ticket`, `final_price`, `status`) VALUES
(1, 1, 1, 2, 1, 1000, 0),
(3, 9, 1, 2, 1, 1000, 0),
(4, 10, 4, 2, 2, 1000, 0),
(7, 13, 1, 2, 18, 18000, 0),
(8, 14, 4, 2, 1, 1000, 0),
(11, 19, 4, 2, 1, 1000, 0),
(12, 20, 4, 2, 3, 3000, 0),
(13, 21, 5, 1, 1, 1000, 0),
(14, 22, 2, 1, 1, 500, 1);

-- --------------------------------------------------------

--
-- Структура таблицы `ticket_price`
--

DROP TABLE IF EXISTS `ticket_price`;
CREATE TABLE IF NOT EXISTS `ticket_price` (
  `id_price` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id записи цены билета',
  `id_event` int(11) NOT NULL COMMENT 'id мероприятия из репертуара',
  `price` double NOT NULL COMMENT 'Цена билета',
  `install_date` varchar(16) NOT NULL COMMENT 'Дата, в которую эта цена была установлена',
  PRIMARY KEY (`id_price`),
  UNIQUE KEY `id_price` (`id_price`),
  KEY `id_event` (`id_event`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8 COMMENT='Цены билетов';

--
-- Дамп данных таблицы `ticket_price`
--

INSERT INTO `ticket_price` (`id_price`, `id_event`, `price`, `install_date`) VALUES
(1, 2, 1000, '12.05.2019'),
(3, 1, 2000, '14.05.2019'),
(4, 3, 1500, '14.05.2019'),
(5, 4, 700, '14.05.2019'),
(6, 5, 1800, '14.05.2019'),
(7, 6, 1400, '14.05.2019'),
(8, 1, 1500, '26.05.2019'),
(9, 4, 1000, '26.05.2019'),
(10, 7, 1700, '27.05.2019'),
(11, 4, 1000, '27.05.2019');

--
-- Ограничения внешнего ключа сохраненных таблиц
--

--
-- Ограничения внешнего ключа таблицы `account`
--
ALTER TABLE `account`
  ADD CONSTRAINT `account_ibfk_1` FOREIGN KEY (`id_account_type`) REFERENCES `account_type` (`id_account_type`);

--
-- Ограничения внешнего ключа таблицы `customer`
--
ALTER TABLE `customer`
  ADD CONSTRAINT `customer_ibfk_1` FOREIGN KEY (`id_account`) REFERENCES `account` (`id_account`),
  ADD CONSTRAINT `customer_ibfk_2` FOREIGN KEY (`id_customers_type`) REFERENCES `customers_type` (`id_customers_type`);

--
-- Ограничения внешнего ключа таблицы `employee`
--
ALTER TABLE `employee`
  ADD CONSTRAINT `employee_ibfk_1` FOREIGN KEY (`id_position`) REFERENCES `position` (`id_position`),
  ADD CONSTRAINT `employee_ibfk_2` FOREIGN KEY (`id_account`) REFERENCES `account` (`id_account`);

--
-- Ограничения внешнего ключа таблицы `event`
--
ALTER TABLE `event`
  ADD CONSTRAINT `event_ibfk_1` FOREIGN KEY (`id_event_type`) REFERENCES `event_type` (`id_event_type`);

--
-- Ограничения внешнего ключа таблицы `room`
--
ALTER TABLE `room`
  ADD CONSTRAINT `room_ibfk_1` FOREIGN KEY (`id_room_type`) REFERENCES `room_type` (`id_room_type`);

--
-- Ограничения внешнего ключа таблицы `schedule`
--
ALTER TABLE `schedule`
  ADD CONSTRAINT `schedule_ibfk_1` FOREIGN KEY (`id_event`) REFERENCES `event` (`id_event`),
  ADD CONSTRAINT `schedule_ibfk_2` FOREIGN KEY (`id_room`) REFERENCES `room` (`id_room`);

--
-- Ограничения внешнего ключа таблицы `schedule_empl`
--
ALTER TABLE `schedule_empl`
  ADD CONSTRAINT `schedule_empl_ibfk_1` FOREIGN KEY (`id_event_entry`) REFERENCES `schedule` (`id_schedule_entry`),
  ADD CONSTRAINT `schedule_empl_ibfk_2` FOREIGN KEY (`id_employee`) REFERENCES `employee` (`id_employee`);

--
-- Ограничения внешнего ключа таблицы `ticket`
--
ALTER TABLE `ticket`
  ADD CONSTRAINT `ticket_ibfk_1` FOREIGN KEY (`id_customer`) REFERENCES `customer` (`id_customer`),
  ADD CONSTRAINT `ticket_ibfk_2` FOREIGN KEY (`id_schedule`) REFERENCES `schedule` (`id_schedule_entry`),
  ADD CONSTRAINT `ticket_ibfk_3` FOREIGN KEY (`id_coeff`) REFERENCES `coefficient` (`id_coeff`);

--
-- Ограничения внешнего ключа таблицы `ticket_price`
--
ALTER TABLE `ticket_price`
  ADD CONSTRAINT `ticket_price_ibfk_1` FOREIGN KEY (`id_event`) REFERENCES `event` (`id_event`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
