using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using MetroFramework.Components;
//using MetroFramework.Forms;

namespace Cashbox
{
    public partial class Form1 : Form
    {
        //SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\DOCUMENTS\VISUAL STUDIO 2017\PROJECTS\CASHBOX\CASHBOX\DATABASECASHBOX.MDF;Integrated Security=True");
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\CashBox\DB\DatabaseCashbox.mdf;Integrated Security=True");
        public Form1()
        {
            InitializeComponent();
            textBox1.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) Button1_Click(new object(), new EventArgs()); };//Нажатие кнопки "Войти" с клавиатуры
            comboBoxF1.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) Button1_Click(new object(), new EventArgs()); };//Нажатие кнопки "Войти" с клавиатуры           
        }

        private void Button1_Click(object sender, EventArgs e)//Войти
        {
            con.Open();//Открываем соединение
            SqlCommand cmd1 = con.CreateCommand();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "SELECT * FROM [Table_Login] WHERE logins = @logins";
            cmd1.Parameters.AddWithValue("@logins", comboBoxF1.Text);
            cmd1.ExecuteNonQuery();
            DataTable dt1 = new DataTable();//создаем экземпляр класса DataTable
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);//создаем экземпляр класса SqlDataAdapter
            dt1.Clear();//чистим DataTable, если он был не пуст
            da1.Fill(dt1);//заполняем данными созданный DataTable
            con.Close();//Закрываем соединение
            Dostup.Login = dt1.Rows[0][1].ToString();//Логин
            Dostup.Access = dt1.Rows[0][3].ToString();//Доступ

            if (textBox1.Text != "" & dt1.Rows[0][2].ToString() == textBox1.Text & DateTime.Today <= Convert.ToDateTime("01.04.2025"))
            {
                //Clipboard.SetText(AccessF1.Text);//Скопировать текст в буфер обмена
                Form2 Form2 = new Form2();
                Form2.Show();
                this.Hide();
            }
            else if (dt1.Rows[0][2].ToString() != textBox1.Text)
            {
                MessageBox.Show("Неверный пароль", "Внимание!");
            }
            else
            {
                MessageBox.Show("Введите пароль", "Внимание!");
            }
        }
        public void Logins_select()//Вывод пользователей в Combobox
        {
            con.Open();//Открываем соединение
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT logins FROM [Table_Login] WHERE logins NOT IN ('root') ORDER BY logins";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();//создаем экземпляр класса DataTable
            SqlDataAdapter da = new SqlDataAdapter(cmd);//создаем экземпляр класса SqlDataAdapter
            dt.Clear();//чистим DataTable, если он был не пуст
            da.Fill(dt);//заполняем данными созданный DataTable
            foreach (DataRow row in dt.Rows)
            {
                comboBoxF1.Items.Add(row[0].ToString());
            }
            con.Close();//Закрываем соединение          
        }
        private void Form1_Load(object sender, EventArgs e)//Загрузка формы
        {
            textBox1.PasswordChar = '*';//Скрыть пароль
            Logins_select();
            comboBoxF1.SelectedIndex = 1;//пользователь по умолчанию
            textBox1.Select();//Установка курсора            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)//Закрытие формы Выход
        {
            Application.Exit();
        }
        private void ComboBoxF1_SelectedIndexChanged(object sender, EventArgs e)//Установить курсор после выбора
        {
            textBox1.Select();
        }

        private void Button6_Click(object sender, EventArgs e)//Закрыть программу
        {
            Application.Exit();
        }
    }
}
