using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using Image = System.Drawing.Image;
using Rectangle = System.Drawing.Rectangle;
//using MetroFramework.Components;
//using MetroFramework.Forms;

namespace Cashbox
{
    public partial class Form2 : Form
    {
        Form1 Form1 = new Form1();
        //SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\DOCUMENTS\VISUAL STUDIO 2017\PROJECTS\CASHBOX\CASHBOX\DATABASECASHBOX.MDF;Integrated Security=True");
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\CashBox\DB\DatabaseCashbox.mdf;Integrated Security=True");
        public Form2()
        {
            InitializeComponent();
            //Text += "  версия - " + CurrentVersion;//Версия программы
            label63.Text = "  версия - " + CurrentVersion;
            textBox6.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) Button12_Click(new object(), new EventArgs()); };//Нажатие кнопки "Войти" с клавиатуры
            //DGV2.KeyUp += (s, e) => { if (e.KeyCode == Keys.Delete) button3_Click(new object(), new EventArgs()); };//Нажатие кнопки Delete
            Timer timer = new Timer();
            timer.Interval = 200;
            timer.Enabled = true;
            timer.Tick += new EventHandler(Timer_Tick);
        }

        static void Main()
        {
            // Копировать из текущего каталога, включить подкаталоги.
            DirectoryCopy(".", @".\temp", true);
        }
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Получить подкаталоги для указанного каталога.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Исходный каталог не существует или не может быть найден: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // Если каталог назначения не существует, создайте его.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Получить файлы в каталоге и скопировать их в новое место.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // При копировании подкаталогов, скопируйте их и их содержимое в новое место.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
        public string CurrentVersion//Версия программы
        {
            get
            {
                return ApplicationDeployment.IsNetworkDeployed
                      ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
                      : Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        private void Form2_Load(object sender, EventArgs e)//Загрузка формы
        {
            Form1.AccessF1.Text = Clipboard.GetText();//Считать текст из буфера обмена 
            label10.Text = Form1.AccessF1.Text;//Имя касира
            string dostup = Dostup.Value;//Доступ
            if (dostup == "Low")
            {
                tabPage2.Enabled = false;
                tabPage4.Enabled = false;
                button13.Enabled = false;
                button14.Enabled = false;
                button15.Enabled = false;
                button16.Enabled = false;
                button5.Visible = false;
            }
            else if (dostup == "Full")
            {
                tabPage2.Enabled = true;
                tabPage3.Enabled = true;
                tabPage4.Enabled = true;
                button13.Enabled = false;
                button14.Enabled = false;
                button5.Visible = false;
            }
            else if (dostup == "root")
            {
                tabPage2.Enabled = true;
                tabPage3.Enabled = true;
                tabPage4.Enabled = true;
                button13.Enabled = true;
                button14.Enabled = true;
                button5.Visible = true;
            }
            Rerequisites();
            //-----------------Окраска Гридов-------------------//
            DataGridViewRow row1 = this.DGV1.RowTemplate;
            row1.DefaultCellStyle.BackColor = Color.FromArgb(227, 226, 221);//цвет строк
            row1.DefaultCellStyle.ForeColor = Color.FromArgb(33, 40, 47);//цвет текста
            row1.Height = 40;
            row1.MinimumHeight = 17;
            DGV1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;//автоподбор ширины столбца по содержимому
            //dataGridView1.Columns[0].Width = 5;//Ширина столбца
            DGV1.EnableHeadersVisualStyles = false;
            DGV1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(228, 201, 156);//цвет заголовка
            DGV1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//Выравнивание текста в заголовке
            DataGridViewRow row2 = this.DGV2.RowTemplate;
            row2.DefaultCellStyle.BackColor = Color.FromArgb(227, 226, 221);//цвет строк
            row2.DefaultCellStyle.ForeColor = Color.FromArgb(33, 40, 47);//цвет текста
            row2.Height = 40;
            row2.MinimumHeight = 17;
            DGV2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;//автоподбор ширины столбца по содержимому
            DGV2.EnableHeadersVisualStyles = false;
            DGV2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(228, 201, 156);//цвет заголовка
            DGV2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//Выравнивание текста в заголовке
            DataGridViewRow magaz = this.DGVmagaz.RowTemplate;
            magaz.DefaultCellStyle.BackColor = Color.FromArgb(227, 226, 221);//цвет строк
            magaz.DefaultCellStyle.ForeColor = Color.FromArgb(33, 40, 47);//цвет текста
            magaz.Height = 40;
            magaz.MinimumHeight = 17;
            DGVmagaz.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;//автоподбор ширины столбца по содержимому
            DGVmagaz.EnableHeadersVisualStyles = false;
            DGVmagaz.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(228, 201, 156);//цвет заголовка
            DGVmagaz.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//Выравнивание текста в заголовке
            DataGridViewRow row3 = this.DGV3.RowTemplate;
            row3.DefaultCellStyle.BackColor = Color.LightSkyBlue;
            row3.Height = 20;
            row3.MinimumHeight = 17;
            DGV3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;//автоподбор ширины столбца по содержимому
            DGV3.EnableHeadersVisualStyles = false;
            DGV3.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSlateGray;//цвет заголовка
            DGV3.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//Выравнивание текста в заголовке
            DataGridViewRow login = this.DGVlogin.RowTemplate;
            login.DefaultCellStyle.BackColor = Color.LightYellow;//цвет строк
            login.Height = 20;
            login.MinimumHeight = 17;
            DGVlogin.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;//автоподбор ширины столбца по содержимому
            DGVlogin.EnableHeadersVisualStyles = false;
            DGVlogin.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSeaGreen;//цвет заголовка
            DGVlogin.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//Выравнивание текста в заголовке
            DataGridViewRow requisites = this.DGVrequisites.RowTemplate;
            requisites.DefaultCellStyle.BackColor = Color.LightYellow;//цвет строк
            requisites.Height = 20;
            requisites.MinimumHeight = 17;
            DGVrequisites.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;//автоподбор ширины столбца по содержимому
            DGVrequisites.EnableHeadersVisualStyles = false;
            DGVrequisites.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSeaGreen;//цвет заголовка
            DGVrequisites.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//Выравнивание текста в заголовке
            DataGridViewRow row4 = this.DGVsklad1.RowTemplate;
            row4.DefaultCellStyle.BackColor = Color.LightSkyBlue;
            row4.Height = 20;
            row4.MinimumHeight = 17;
            DGVsklad1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;//автоподбор ширины столбца по содержимому
            DGVsklad1.EnableHeadersVisualStyles = false;
            DGVsklad1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSlateGray;//цвет заголовка
            DGVsklad1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//Выравнивание текста в заголовке
            DataGridViewRow row5 = this.DGVotchet.RowTemplate;
            row5.DefaultCellStyle.BackColor = Color.LightYellow;//цвет строк
            row5.Height = 20;
            row5.MinimumHeight = 17;
            DGVotchet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;//автоподбор ширины столбца по содержимому
            DGVotchet.EnableHeadersVisualStyles = false;
            DGVotchet.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSeaGreen;//цвет заголовка
            DGVotchet.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//Выравнивание текста в заголовке
            //----------------Окраска Гридов--------------------//   

            //-------------Отключить сортировку гридов----------------------//
            foreach (DataGridViewColumn column in DGV2.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn column in DGVmagaz.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            //-------------Отключить сортировку гридов----------------------//
            DGV1.RowHeadersVisible = false;//Самая левая колонка
            DGV2.RowHeadersVisible = false;//Самая левая колонка
            DGVmagaz.RowHeadersVisible = false;//Самая левая колонка

            Select_sklad();
            Select_zakaz();
            Select_chek();
            Select_chek_magaz();
            Disp_data();
            Logins_select();
            Select_Categories();
            Produkt_select_admin();
            Produkt_select_sklad();
            Podschet();

        }
        private void Timer_Tick(object sender, EventArgs e)//Отображение часиков
        {
            label7.Text = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString();
            //label7.Text = DateTime.Now.ToString("yyyy.MM.dd, HH.mm.ss");
        }
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)//Закрытие формы Выход
        {
            Application.Exit();
        }
        private void DGV1_SelectionChanged(object sender, EventArgs e)//получить данные выделенной строки
        {
            if (DGV1.Rows.Count >= 2)
            {
                label2.Text = DGV1.CurrentRow.Cells[0].Value.ToString();
            }
        }

        private void DGV1_Click(object sender, EventArgs e)//Кликая на грид вызываем метод select_chek()
        {
            Select_chek();
            Select_chek_magaz();
            Podschet();
        }
        public void Select_chek()//Чек
        {
            con.Open();//Открываем соединение
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT product, kol_vo, price, summ FROM [Table] WHERE product IS NOT NULL AND zakaz = @zakaz AND status = @status AND kasir = @kasir AND magaz = @magaz ORDER BY zakaz DESC";
            cmd.Parameters.AddWithValue("@zakaz", label2.Text);
            cmd.Parameters.AddWithValue("@kasir", label10.Text);
            cmd.Parameters.AddWithValue("@status", "Открыт");
            cmd.Parameters.AddWithValue("@magaz", "Нет");
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();//создаем экземпляр класса DataTable
            SqlDataAdapter da = new SqlDataAdapter(cmd);//создаем экземпляр класса SqlDataAdapter
            dt.Clear();//чистим DataTable, если он был не пуст
            da.Fill(dt);//заполняем данными созданный DataTable
            DGV2.DataSource = dt;//в качестве источника данных у dataGridView используем DataTable заполненный данными
            con.Close();//Закрываем соединение
            DGV2.Columns[0].HeaderText = "Наименование";
            DGV2.Columns[1].HeaderText = "Кол";
            DGV2.Columns[2].HeaderText = "Цена";
            DGV2.Columns[3].HeaderText = "Сумма";
        }
        public void Select_chek_magaz()//Чек магазин
        {
            con.Open();//Открываем соединение
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT product, kol_vo, price, summ FROM [Table] WHERE product IS NOT NULL AND zakaz = @zakaz AND status = @status AND kasir = @kasir AND magaz = @magaz ORDER BY zakaz DESC";
            cmd.Parameters.AddWithValue("@zakaz", label2.Text);
            cmd.Parameters.AddWithValue("@kasir", label10.Text);
            cmd.Parameters.AddWithValue("@status", "Открыт");
            cmd.Parameters.AddWithValue("@magaz", "Да");
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();//создаем экземпляр класса DataTable
            SqlDataAdapter da = new SqlDataAdapter(cmd);//создаем экземпляр класса SqlDataAdapter
            dt.Clear();//чистим DataTable, если он был не пуст
            da.Fill(dt);//заполняем данными созданный DataTable
            DGVmagaz.DataSource = dt;//в качестве источника данных у dataGridView используем DataTable заполненный данными
            con.Close();//Закрываем соединение
            DGVmagaz.Columns[0].HeaderText = "Наименование";
            DGVmagaz.Columns[1].HeaderText = "Кол";
            DGVmagaz.Columns[2].HeaderText = "Цена";
            DGVmagaz.Columns[3].HeaderText = "Сумма";
        }
        private void Select_zakaz()//Заказы
        {
            con.Open();//Открываем соединение
            SqlCommand cmd = new SqlCommand("SELECT MIN(zakaz) AS 'Заказ №', MIN(datazapisi) AS 'Время' FROM [Table] " +
                "WHERE status = @status AND kasir = @kasir GROUP BY zakaz ORDER BY zakaz DESC", con);
            cmd.Parameters.AddWithValue("@kasir", label10.Text);
            cmd.Parameters.AddWithValue("@status", "Открыт");
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();//создаем экземпляр класса DataTable
            SqlDataAdapter da = new SqlDataAdapter(cmd);//создаем экземпляр класса SqlDataAdapter
            dt.Clear();//чистим DataTable, если он был не пуст
            da.Fill(dt);//заполняем данными созданный DataTable
            DGV1.DataSource = dt;//в качестве источника данных у dataGridView используем DataTable заполненный данными
            con.Close();//Закрываем соединение
            DGV1.Columns["Время"].DefaultCellStyle.Format = "HH:mm:ss";
        }
        private void Disp_data()
        {
            con.Open();//Открываем соединение
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM [Table] WHERE product IS NOT NULL AND kasir=@kasir AND status=@status AND smena = @smena AND zakaz=@zakaz ORDER BY zakaz DESC";
            cmd.Parameters.AddWithValue("@zakaz", label2.Text);
            cmd.Parameters.AddWithValue("@status", "Открыт");
            cmd.Parameters.AddWithValue("@kasir", label10.Text);
            cmd.Parameters.AddWithValue("@smena", "Открыта");
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();//создаем экземпляр класса DataTable
            SqlDataAdapter da = new SqlDataAdapter(cmd);//создаем экземпляр класса SqlDataAdapter
            dt.Clear();//чистим DataTable, если он был не пуст
            da.Fill(dt);//заполняем данными созданный DataTable
            dataGridView1.DataSource = dt;//в качестве источника данных у dataGridView используем DataTable заполненный данными
            con.Close();//Закрываем соединение

            con.Open();//открыть соединение
            for (int i = 0; i < dataGridView1.Rows.Count; i++)//Цикл
            {
                SqlCommand cmd1 = new SqlCommand("UPDATE [Table] SET summ = (kol_vo * price) " +
                    "WHERE id = @id AND zakaz = @zakaz AND status = @status AND kasir = @kasir AND smena = @smena", con);
                cmd1.Parameters.AddWithValue("@id", Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value));
                cmd1.Parameters.AddWithValue("@zakaz", label2.Text);
                cmd1.Parameters.AddWithValue("@kasir", label10.Text);
                cmd1.Parameters.AddWithValue("@status", "Открыт");
                cmd1.Parameters.AddWithValue("@smena", "Открыта");
                cmd1.ExecuteNonQuery();
            }
            con.Close();//закрыть соединение
        }
        private void Rerequisites()//Реквизиты
        {
            con.Open();//Открываем соединение
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT organization, adress, name FROM [Table_requisites]";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();//создаем экземпляр класса DataTable
            SqlDataAdapter da = new SqlDataAdapter(cmd);//создаем экземпляр класса SqlDataAdapter
            dt.Clear();//чистим DataTable, если он был не пуст
            da.Fill(dt);//заполняем данными созданный DataTable
            DGVrequisites.DataSource = dt;//в качестве источника данных у dataGridView используем DataTable заполненный данными
            con.Close();//Закрываем соединение    
            if (DGVrequisites.Rows.Count >= 1)
            {
                label29.Text = DGVrequisites.Rows[0].Cells[0].Value.ToString();
            }
            else MessageBox.Show("Заполните реквизиты!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void Podschet()//Произвести подсчет
        {
            if (DGV2.Rows.Count >= 1 | DGVmagaz.Rows.Count >= 1)
            {
                //Итого 
                double summa = 0;
                foreach (DataGridViewRow row in DGV2.Rows)
                {
                    double incom;
                    double.TryParse((row.Cells[3].Value ?? "0").ToString().Replace(".", ","), out incom);
                    summa += incom;
                }
                label25.Text = summa.ToString();
                //Итого магазин
                double summa_magaz = 0;
                foreach (DataGridViewRow row in DGVmagaz.Rows)
                {
                    double incom;
                    double.TryParse((row.Cells[3].Value ?? "0").ToString().Replace(".", ","), out incom);
                    summa_magaz += incom;
                }
                label64.Text = summa_magaz.ToString();
                double combo = Convert.ToDouble(comboBox16.Text);
                double itog = Convert.ToDouble(label25.Text);
                double magaz = Convert.ToDouble(label64.Text);
                double skidka1 = Math.Round((itog * combo) / 100);
                double skidka2 = Math.Round((magaz * combo) / 100);
                double skidka = skidka1 + skidka2;
                double ZP = Math.Round(((itog - skidka1) * 15) / 100);
                double itogo = Math.Round(((itog + magaz) - skidka) + ZP);
                label20.Text = skidka.ToString();
                label23.Text = itogo.ToString() + " Сом";
                label38.Text = ZP.ToString();
            }
            if (DGV2.Rows.Count == 0)
            {
                label38.Text = "0";
                label25.Text = "0";
            }
            if (DGVmagaz.Rows.Count == 0)
            {
                label64.Text = "0";
            }
            //----------------------------------------------------------------------------------------------//
            //Подсчет количества строк (не учитывая пустые строки и колонки)
            int count = 0;
            for (int j = 0; j < DGV2.RowCount; j++)
            {
                for (int i = 0; i < DGV2.ColumnCount; i++)
                {
                    if (DGV2[i, j].Value != null)
                    {
                        label21.Text = Convert.ToString(DGV2.Rows.Count) + " ";// -1 это нижняя пустая строка
                        count++;
                        break;
                    }
                }
            }
            //Cклад
            if (DGVsklad1.Rows.Count >= 1)
            {
                //сумма закупки
                double summ = 0;
                foreach (DataGridViewRow row in DGVsklad1.Rows)
                {
                    double incom;
                    double.TryParse((row.Cells[5].Value ?? "0").ToString().Replace(".", ","), out incom);
                    summ += incom;
                }
                label44.Text = summ.ToString();

                //сумма продаж
                double summ1 = 0;
                foreach (DataGridViewRow row in DGVsklad1.Rows)
                {
                    double incom;
                    double.TryParse((row.Cells[6].Value ?? "0").ToString().Replace(".", ","), out incom);
                    summ1 += incom;
                }
                label57.Text = summ1.ToString();

                //сумма остатка
                double summ2 = 0;
                foreach (DataGridViewRow row in DGVsklad1.Rows)
                {
                    double incom;
                    double.TryParse((row.Cells[9].Value ?? "0").ToString().Replace(".", ","), out incom);
                    summ2 += incom;
                }
                label55.Text = summ2.ToString();

                int dohod = Convert.ToInt32(label42.Text);
                dohod = (-Convert.ToInt32(label55.Text)) - (-Convert.ToInt32(label44.Text));//сумма остатка - сумма закупки
            }
        }

        private void Button1_Click(object sender, EventArgs e)//Новый заказ
        {
            if (MessageBox.Show("Вы создаете новый заказ, подтвердите действие", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                if (DGVrequisites.Rows.Count >= 1)
                {
                    con.Open();//открыть соединение
                    SqlCommand cmd = new SqlCommand("INSERT INTO [Table] (zakaz,kasir,status,skidka,price,kol_vo,smena) VALUES (@zakaz,@kasir,@status,@skidka,@price,@kol_vo,@smena)", con);
                    cmd.Parameters.AddWithValue("@zakaz", Convert.ToInt32(DGV1.Rows[0].Cells[0].Value) + 1);
                    cmd.Parameters.AddWithValue("@kasir", label10.Text);
                    cmd.Parameters.AddWithValue("@status", "Открыт");
                    cmd.Parameters.AddWithValue("@skidka", 0);
                    cmd.Parameters.AddWithValue("@price", 0);
                    cmd.Parameters.AddWithValue("@kol_vo", 0);
                    cmd.Parameters.AddWithValue("@smena", "Открыта");
                    cmd.ExecuteNonQuery();
                    con.Close();//закрыть соединение 
                    Select_zakaz();
                    Disp_data();
                    Select_chek();
                    Select_chek_magaz();
                }
                else MessageBox.Show("Заполните реквизиты!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void Button2_Click(object sender, EventArgs e)//Kol-vo +
        {
            if (checkBox1.Checked == false & DGV2.Rows.Count >= 1)
            {
                int currRowIndex = DGV2.CurrentCell.RowIndex;//  Запоминаем строку, которую выбрал пользователь.
                con.Open();//открыть соединение
                SqlCommand cmd = new SqlCommand("UPDATE [Table] SET kol_vo = @kol_vo WHERE product = @product AND zakaz = @zakaz AND status = @status AND kasir = @kasir AND magaz = @magaz", con);
                if (checkBox1.Checked == false & DGV2.Rows.Count >= 1)
                {
                    cmd.Parameters.AddWithValue("@product", DGV2.CurrentRow.Cells[0].Value.ToString());
                    cmd.Parameters.AddWithValue("@magaz", "Нет");
                    cmd.Parameters.AddWithValue("@kol_vo", Convert.ToInt32(DGV2.CurrentRow.Cells[1].Value) + 1);
                }
                cmd.Parameters.AddWithValue("@zakaz", label2.Text);
                cmd.Parameters.AddWithValue("@kasir", label10.Text);
                cmd.Parameters.AddWithValue("@status", "Открыт");
                cmd.ExecuteNonQuery();
                con.Close();//закрыть соединение   

                Disp_data();
                Select_chek();
                Select_chek_magaz();
                Disp_data();
                Podschet();
                DGV2.CurrentCell = DGV2[0, currRowIndex];//  Выбираем нашу строку (именно выбираем, не выделяем).
            }
            if (checkBox1.Checked == true & DGVmagaz.Rows.Count >= 1)
            {
                int currRowIndex2 = DGVmagaz.CurrentCell.RowIndex;//  Запоминаем строку, которую выбрал пользователь.           
                con.Open();//открыть соединение
                SqlCommand cmd = new SqlCommand("UPDATE [Table] SET kol_vo = @kol_vo WHERE product = @product AND zakaz = @zakaz AND status = @status AND kasir = @kasir AND magaz = @magaz", con);
                if (checkBox1.Checked == true & DGVmagaz.Rows.Count >= 1)
                {
                    cmd.Parameters.AddWithValue("@product", DGVmagaz.CurrentRow.Cells[0].Value.ToString());
                    cmd.Parameters.AddWithValue("@magaz", "Да");
                    cmd.Parameters.AddWithValue("@kol_vo", Convert.ToInt32(DGVmagaz.CurrentRow.Cells[1].Value) + 1);
                }
                cmd.Parameters.AddWithValue("@zakaz", label2.Text);
                cmd.Parameters.AddWithValue("@kasir", label10.Text);
                cmd.Parameters.AddWithValue("@status", "Открыт");
                cmd.ExecuteNonQuery();
                con.Close();//закрыть соединение       

                Disp_data();
                Select_chek();
                Select_chek_magaz();
                Disp_data();
                Podschet();
                DGVmagaz.CurrentCell = DGVmagaz[0, currRowIndex2];//  Выбираем нашу строку (именно выбираем, не выделяем).
            }
        }
        private void Button18_Click(object sender, EventArgs e)//Kol-vo -
        {
            if (checkBox1.Checked == false & DGV2.Rows.Count >= 1)
            {
                int currRowIndex = DGV2.CurrentCell.RowIndex;//  Запоминаем строку, которую выбрал пользователь.
                con.Open();//открыть соединение
                SqlCommand cmd = new SqlCommand("UPDATE [Table] SET kol_vo = @kol_vo WHERE product = @product AND zakaz = @zakaz AND status = @status AND kasir = @kasir AND magaz = @magaz", con);
                if (checkBox1.Checked == false & DGV2.Rows.Count >= 1)
                {
                    cmd.Parameters.AddWithValue("@product", DGV2.CurrentRow.Cells[0].Value.ToString());
                    cmd.Parameters.AddWithValue("@magaz", "Нет");
                    cmd.Parameters.AddWithValue("@kol_vo", Convert.ToInt32(DGV2.CurrentRow.Cells[1].Value) - 1);
                }
                cmd.Parameters.AddWithValue("@zakaz", label2.Text);
                cmd.Parameters.AddWithValue("@kasir", label10.Text);
                cmd.Parameters.AddWithValue("@status", "Открыт");
                cmd.ExecuteNonQuery();
                con.Close();//закрыть соединение   

                Disp_data();
                Select_chek();
                Select_chek_magaz();
                Disp_data();
                Podschet();
                DGV2.CurrentCell = DGV2[0, currRowIndex];//  Выбираем нашу строку (именно выбираем, не выделяем).
            }
            if (checkBox1.Checked == true & DGVmagaz.Rows.Count >= 1)
            {
                int currRowIndex2 = DGVmagaz.CurrentCell.RowIndex;//  Запоминаем строку, которую выбрал пользователь.           
                con.Open();//открыть соединение
                SqlCommand cmd = new SqlCommand("UPDATE [Table] SET kol_vo = @kol_vo WHERE product = @product AND zakaz = @zakaz AND status = @status AND kasir = @kasir AND magaz = @magaz", con);
                if (checkBox1.Checked == true & DGVmagaz.Rows.Count >= 1)
                {
                    cmd.Parameters.AddWithValue("@product", DGVmagaz.CurrentRow.Cells[0].Value.ToString());
                    cmd.Parameters.AddWithValue("@magaz", "Да");
                    cmd.Parameters.AddWithValue("@kol_vo", Convert.ToInt32(DGVmagaz.CurrentRow.Cells[1].Value) - 1);
                }
                cmd.Parameters.AddWithValue("@zakaz", label2.Text);
                cmd.Parameters.AddWithValue("@kasir", label10.Text);
                cmd.Parameters.AddWithValue("@status", "Открыт");
                cmd.ExecuteNonQuery();
                con.Close();//закрыть соединение       

                Disp_data();
                Select_chek();
                Select_chek_magaz();
                Disp_data();
                Podschet();
                DGVmagaz.CurrentCell = DGVmagaz[0, currRowIndex2];//  Выбираем нашу строку (именно выбираем, не выделяем).
            }
        }
        private void Button3_Click(object sender, EventArgs e)//Удалить из чека
        {
            if (DGV2.Rows.Count >= 1 | DGVmagaz.Rows.Count >= 1)
            {
                try
                {
                    con.Open();//открыть соединение
                    SqlCommand cmd = new SqlCommand("DELETE FROM [Table] WHERE product=@product AND zakaz = @zakaz AND status = @status AND kasir = @kasir AND magaz = @magaz", con);
                    cmd.Parameters.AddWithValue("@zakaz", label2.Text);
                    cmd.Parameters.AddWithValue("@kasir", label10.Text);
                    cmd.Parameters.AddWithValue("@status", "Открыт");
                    if (checkBox1.Checked == false & DGV2.Rows.Count >= 1)
                    {
                        cmd.Parameters.AddWithValue("@product", DGV2.CurrentRow.Cells[0].Value.ToString());
                        cmd.Parameters.AddWithValue("@magaz", "Нет");
                    }
                    else if (checkBox1.Checked == true & DGVmagaz.Rows.Count >= 1)
                    {
                        cmd.Parameters.AddWithValue("@product", DGVmagaz.CurrentRow.Cells[0].Value.ToString());
                        cmd.Parameters.AddWithValue("@magaz", "Да");
                    }
                    cmd.ExecuteNonQuery();
                    con.Close();//закрыть соединение
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка! Один из чеков не заполнен!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    con.Close();//закрыть соединение
                }
                Disp_data();
                Select_chek();
                Select_chek_magaz();
                Disp_data();
                Podschet();
            }
            else MessageBox.Show("Один из чеков не заполнен!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)//Поиск в Кассе продуктов
        {
            ProductsPanel.Controls.Clear();

            con.Open();//Открываем соединение
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT product,image FROM [Table_Items] WHERE product LIKE N'%" + textBox1.Text.ToString() + "%' ";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();//создаем экземпляр класса DataTable
            SqlDataAdapter da = new SqlDataAdapter(cmd);//создаем экземпляр класса SqlDataAdapter
            dt.Clear();//чистим DataTable, если он был не пуст
            da.Fill(dt);//заполняем данными созданный DataTable
            con.Close();//Закрываем соединение
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                int x = i / 6;
                int y = i % 6;
                Button ProductButton = new Button();
                ProductButton.Text = row[0].ToString();
                ProductButton.Size = new Size(120, 70);
                ProductButton.ForeColor = Color.FromArgb(41, 53, 65);
                ProductButton.BackColor = Color.FromArgb(191, 205, 219);
                ProductButton.FlatStyle = FlatStyle.Flat;
                ProductButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(127, 136, 146);
                //ProductButton.Location = new Point(20, 72 * (i + 1));
                ProductButton.Location = new Point(ProductButton.Width * x, ProductButton.Height * y);

                if (row["image"] != DBNull.Value)
                {
                    byte[] img = (byte[])row["image"];
                    MemoryStream ms = new MemoryStream(img);
                    ProductButton.Image = Image.FromStream(ms);
                    ProductButton.Image = new Bitmap(ProductButton.Image);
                    ProductButton.ImageAlign = ContentAlignment.TopCenter;
                    ProductButton.TextAlign = ContentAlignment.BottomCenter;
                }
                ProductButton.Tag = row[0].ToString();
                ProductsPanel.Controls.Add(ProductButton);
                i++;
                ProductButton.Click += ProductButton_Click;
            }
        }

        public void Select_Categories()//Вывод категорий в Кнопки и comboBoxCategory
        {
            CategoriesPanel.AutoScroll = true;
            ProductsPanel.AutoScroll = true;
            con.Open();//Открываем соединение
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT categories,image FROM [Table_Categories] WHERE categories IS NOT NULL ORDER BY categories DESC";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();//создаем экземпляр класса DataTable
            SqlDataAdapter da = new SqlDataAdapter(cmd);//создаем экземпляр класса SqlDataAdapter
            dt.Clear();//чистим DataTable, если он был не пуст
            da.Fill(dt);//заполняем данными созданный DataTable 
            con.Close();//Закрываем соединение

            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                comboBoxCategory.Items.Add(row[0].ToString());
                comboBox13.Items.Add(row[0].ToString());

                int x = i / 9;
                int y = i % 9;
                Button btn = new Button();
                btn.Text = row[0].ToString();
                btn.Size = new Size(120, 70);
                btn.ForeColor = Color.FromArgb(33, 40, 47);
                btn.BackColor = Color.FromArgb(227, 226, 221);
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(228, 201, 156);
                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(126, 150, 162);
                //btn.Location = new Point(20, 72 * (i + 1));//Вертикальное расположение
                btn.Location = new Point(btn.Width * x, btn.Height * y);

                if (row["image"] != DBNull.Value)
                {
                    byte[] img = (byte[])row["image"];
                    MemoryStream ms = new MemoryStream(img);
                    btn.Image = Image.FromStream(ms);
                    btn.Image = new Bitmap(btn.Image);
                    btn.ImageAlign = ContentAlignment.TopCenter;
                    btn.TextAlign = ContentAlignment.BottomCenter;
                }
                btn.Tag = row[0].ToString();
                CategoriesPanel.Controls.Add(btn);
                i++;
                btn.Click += CategoryButtonClick;
            }
        }
        void CategoryButtonClick(object sender, EventArgs e)//Вывод продуктов в Кнопки
        {
            ProductsPanel.Controls.Clear();
            Button btn = (Button)sender;
            string CategoryID = Convert.ToString(btn.Tag);

            con.Open();//Открываем соединение
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT product,image FROM [Table_Items] WHERE category=@category";
            cmd.Parameters.AddWithValue("@category", CategoryID);
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();//создаем экземпляр класса DataTable
            SqlDataAdapter da = new SqlDataAdapter(cmd);//создаем экземпляр класса SqlDataAdapter
            dt.Clear();//чистим DataTable, если он был не пуст
            da.Fill(dt);//заполняем данными созданный DataTable
            con.Close();//Закрываем соединение
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                int x = i / 7;
                int y = i % 7;
                Button ProductButton = new Button();
                ProductButton.Text = row[0].ToString();
                ProductButton.Size = new Size(120, 70);
                ProductButton.ForeColor = Color.FromArgb(33, 40, 47);
                ProductButton.BackColor = Color.FromArgb(227, 226, 221);
                ProductButton.FlatStyle = FlatStyle.Flat;
                ProductButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(228, 201, 156);
                ProductButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(197, 165, 171);
                //ProductButton.Location = new Point(20, 72 * (i + 1));
                ProductButton.Location = new Point(ProductButton.Width * x, ProductButton.Height * y);

                if (row["image"] != DBNull.Value)
                {
                    byte[] img = (byte[])row["image"];
                    MemoryStream ms = new MemoryStream(img);
                    ProductButton.Image = Image.FromStream(ms);
                    ProductButton.Image = new Bitmap(ProductButton.Image);
                    ProductButton.ImageAlign = ContentAlignment.TopCenter;
                    ProductButton.TextAlign = ContentAlignment.BottomCenter;
                }
                ProductButton.Tag = row[0].ToString();
                ProductsPanel.Controls.Add(ProductButton);
                i++;
                ProductButton.Click += ProductButton_Click;
            }
        }
        void ProductButton_Click(object sender, EventArgs e)////Добавить продукт в чек
        {
            Button ProductButton = sender as Button;
            string ProductID = Convert.ToString(ProductButton.Tag);
            comboBoxProduct.Text = ProductID;
            if (DGV1.Rows.Count >= 2)
            {
                con.Open();//открыть соединение
                SqlCommand cmd = new SqlCommand("INSERT INTO [Table] (category,zakaz,kasir,status,product,price,kol_vo,skidka,smena,magaz) VALUES (@category,@zakaz,@kasir,@status,@product,@price,@kol_vo,@skidka,@smena,@magaz)", con);
                cmd.Parameters.AddWithValue("@product", DGV3.Rows[0].Cells[1].Value.ToString());
                cmd.Parameters.AddWithValue("@price", Convert.ToInt32(DGV3.Rows[0].Cells[2].Value));
                cmd.Parameters.AddWithValue("@kol_vo", 1);
                cmd.Parameters.AddWithValue("@kasir", label10.Text);
                cmd.Parameters.AddWithValue("@status", "Открыт");
                cmd.Parameters.AddWithValue("@zakaz", label2.Text);
                cmd.Parameters.AddWithValue("@skidka", 0);
                cmd.Parameters.AddWithValue("@smena", "Открыта");
                cmd.Parameters.AddWithValue("@category", DGV3.Rows[0].Cells[3].Value.ToString());
                if (checkBox1.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@magaz", "Да");
                }
                else if (checkBox1.Checked == false)
                {
                    cmd.Parameters.AddWithValue("@magaz", "Нет");
                }
                cmd.ExecuteNonQuery();
                con.Close();//закрыть соединение
            }
            else MessageBox.Show("Сначала создайте новый заказ!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            Disp_data();
            Select_chek();
            Select_chek_magaz();
            Disp_data();
            Podschet();
        }
        private void TextBoxprice_KeyPress(object sender, KeyPressEventArgs e)//Использование в TextBox только цифр
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }
        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }
        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)//Режим обслуживания
        {
            if (checkBox1.Checked == true)
            {
                label66.Text = "Включен";
                label66.BackColor = Color.Red;
            }
            else if (checkBox1.Checked == false)
            {
                label66.Text = "Отключен";
                label66.BackColor = Color.Gray;
            }
        }
        private void ComboBox16_SelectedIndexChanged(object sender, EventArgs e)//Скидка
        {
            Disp_data();
            Select_chek();
            Select_chek_magaz();
            Disp_data();
            Podschet();
        }
        private void Button4_Click(object sender, EventArgs e)//Оплатить
        {
            const string filePatch = @"C:\\PDFs\\Chek.pdf";//путь к файлу чека
            FileStream stream = null;
            try
            {
                stream = File.Open(filePatch, FileMode.Open, FileAccess.Read, FileShare.None);//Проверка открыт ли файл если да то отказ
            }
            catch (Exception)
            {
                MessageBox.Show("Закройте файл PDF!", "ОТКАЗАНО", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                //запускаем_закрытый_файл();
                if (DGV2.Rows.Count >= 1 | DGVmagaz.Rows.Count >= 1)
                {
                    if (MessageBox.Show("Подтвердите действие, при оплате заказ будет закрыт", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        button4.Enabled = false;
                        Disp_data();
                        Select_chek();
                        Select_chek_magaz();
                        Disp_data();
                        if (DGV2.Rows.Count >= 1)
                        {
                            for (int i = 0; i < DGV2.Rows.Count; i++)//Остановить время формула (цена за час * на интервал / на 60минут)
                            {
                                string sauna = DGV2.Rows[i].Cells[0].Value.ToString();
                                if (sauna.Contains("Сауна") | sauna.Contains("сауна") | sauna.Contains("Sauna") | sauna.Contains("sauna"))
                                {
                                    var start = Convert.ToDateTime(DGV1.CurrentRow.Cells[1].Value);
                                    var end = DateTime.Now;
                                    var interval = (end - start);                                   
                                    double totaltime = interval.TotalMinutes / 60;
                                    label40.Text = totaltime.ToString("F2");
                                    double summ = (Convert.ToInt32(DGV2.Rows[i].Cells[2].Value) * interval.TotalMinutes) / 60;
                                    label41.Text = Convert.ToString(Math.Round(summ));
                                    //label40.Text = interval.ToString("hh':'mm");
                                    //string time = label40.Text;//время
                                    //double Time = double.Parse(time, new NumberFormatInfo() { NumberDecimalSeparator = ":" });//Сепаратор : на .
                                    con.Open();//открыть соединение
                                        SqlCommand cmd = new SqlCommand("UPDATE [Table] SET kol_vo = @kol_vo, summ = @summ" +
                                            " WHERE product = @product AND zakaz = @zakaz AND status = @status AND kasir = @kasir AND smena = @smena AND magaz = @magaz", con);
                                        cmd.Parameters.AddWithValue("@product", DGV2.Rows[i].Cells[0].Value.ToString());
                                        
                                        if (interval.TotalMinutes < 60)
                                        {
                                            cmd.Parameters.AddWithValue("@kol_vo", 1);
                                            cmd.Parameters.AddWithValue("@summ", DGV2.Rows[i].Cells[2].Value.ToString());
                                        }
                                        else if (interval.TotalMinutes >= 60)
                                        {
                                        cmd.Parameters.AddWithValue("@kol_vo", Convert.ToDouble(totaltime.ToString("F2")));
                                        //cmd.Parameters.AddWithValue("@kol_vo", Convert.ToDouble(Time.ToString("F2")));//F2 - формат строки обеспечивает 2 цифры после десятичной точки
                                        cmd.Parameters.AddWithValue("@summ", Math.Round(summ));
                                        } 
                                        cmd.Parameters.AddWithValue("@kasir", label10.Text);
                                        cmd.Parameters.AddWithValue("@status", "Открыт");
                                        cmd.Parameters.AddWithValue("@zakaz", label2.Text);
                                        cmd.Parameters.AddWithValue("@smena", "Открыта");
                                        cmd.Parameters.AddWithValue("@magaz", "Нет");
                                        cmd.ExecuteNonQuery();
                                        con.Close();//закрыть соединение
                                    
                                    Disp_data();
                                    Select_chek();//показать обновленный чек
                                    Select_chek_magaz();
                                    Podschet();//пересчитать
                                }
                            }
                        }
                        if (DGV2.Rows.Count < 1 | DGVmagaz.Rows.Count < 1) 
                        {//Пустое поле (необходимо)
                            con.Open();//открыть соединение
                            SqlCommand cmd = new SqlCommand("INSERT INTO [Table] (zakaz,kasir,status,product,price,kol_vo,skidka,smena,magaz) VALUES (@zakaz,@kasir,@status,@product,@price,@kol_vo,@skidka,@smena,@magaz)", con);
                            cmd.Parameters.AddWithValue("@product", "Нет заказов");
                            cmd.Parameters.AddWithValue("@price", 0);
                            cmd.Parameters.AddWithValue("@kol_vo", 0);
                            cmd.Parameters.AddWithValue("@kasir", label10.Text);
                            cmd.Parameters.AddWithValue("@status", "Открыт");
                            cmd.Parameters.AddWithValue("@zakaz", label2.Text);
                            cmd.Parameters.AddWithValue("@skidka", 0);
                            cmd.Parameters.AddWithValue("@smena", "Открыта");
                            if(DGV2.Rows.Count < 1) { cmd.Parameters.AddWithValue("@magaz", "Нет"); }
                            if(DGVmagaz.Rows.Count < 1) { cmd.Parameters.AddWithValue("@magaz", "Да"); }
                            cmd.ExecuteNonQuery();
                            con.Close();//закрыть соединение
                        }
                        Select_chek();
                        Select_chek_magaz();
                        //Зарплата
                        con.Open();//открыть соединение
                        SqlCommand cmdi = new SqlCommand("INSERT INTO [Table] (product,zarplata,zakaz,status,kol_vo,kasir,skidka,price,smena,chek) VALUES (@product,@zarplata,@zakaz,@status,@kol_vo,@kasir,@skidka,@price,@smena,@chek)", con);
                        cmdi.Parameters.AddWithValue("@zakaz", label2.Text);
                        cmdi.Parameters.AddWithValue("@product", "Зарплата");
                        cmdi.Parameters.AddWithValue("@zarplata", label38.Text);
                        cmdi.Parameters.AddWithValue("@status", "Открыт");
                        cmdi.Parameters.AddWithValue("@smena", "Открыта");
                        cmdi.Parameters.AddWithValue("@kol_vo", 0);
                        cmdi.Parameters.AddWithValue("@kasir", label10.Text);
                        cmdi.Parameters.AddWithValue("@skidka", label20.Text);
                        cmdi.Parameters.AddWithValue("@price", 0);
                        cmdi.Parameters.AddWithValue("@chek", 1);
                        cmdi.ExecuteNonQuery();
                        con.Close();//закрыть соединение

                        con.Open();//открыть соединение
                        SqlCommand cmd1 = new SqlCommand("UPDATE [Table] SET status=@status WHERE zakaz = @zakaz AND kasir = @kasir", con);
                        cmd1.Parameters.AddWithValue("@zakaz", label2.Text);
                        cmd1.Parameters.AddWithValue("@kasir", label10.Text);
                        cmd1.Parameters.AddWithValue("@status", "Закрыт");
                        cmd1.ExecuteNonQuery();
                        con.Close();//закрыть соединение

                        for (int i = 0; i < dataGridView1.Rows.Count; i++)//
                        {
                            for (int y = 0; y < DGVsklad1.Rows.Count; y++)
                            {
                                if (Convert.ToString(dataGridView1.Rows[i].Cells[5].Value) == Convert.ToString(DGVsklad1.Rows[y].Cells[2].Value))
                                {
                                    int summ_sale = Convert.ToInt32(dataGridView1.Rows[i].Cells[8].Value) - Convert.ToInt32(dataGridView1.Rows[i].Cells[9].Value);
                                    int kol_vo_sale = Convert.ToInt32(dataGridView1.Rows[i].Cells[7].Value);
                                    int summ_zakup = Convert.ToInt32(DGVsklad1.Rows[y].Cells[5].Value);
                                    int kol_vo_zakup = Convert.ToInt32(DGVsklad1.Rows[y].Cells[4].Value);
                                    con.Open();//открыть соединение
                                    SqlCommand cmd = new SqlCommand("INSERT INTO [Table_sklad] (product,price_zakup,kol_vo_zakup,summ_sale,kol_vo_sale,kol_vo_itog,price_ostatok,data_sale)" +
                                        "VALUES (@product,@price_zakup,@kol_vo_zakup,@summ_sale,@kol_vo_sale,@kol_vo_itog,@price_ostatok,@data_sale)", con);
                                    cmd.Parameters.AddWithValue("@product", Convert.ToString(dataGridView1.Rows[i].Cells[5].Value));
                                    cmd.Parameters.AddWithValue("@price_zakup", 0);
                                    cmd.Parameters.AddWithValue("@kol_vo_zakup", 0);
                                    cmd.Parameters.AddWithValue("@summ_sale", summ_sale);//Math.Round округляет до целого
                                    cmd.Parameters.AddWithValue("@kol_vo_sale", kol_vo_sale);
                                    cmd.Parameters.AddWithValue("@kol_vo_itog", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@price_ostatok", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@data_sale", DateTime.Now);
                                    cmd.ExecuteNonQuery();
                                    con.Close();//закрыть соединение
                                }
                            }
                        }
                        Select_sklad();
                        Select_sklad();
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)//
                        {
                            for (int y = 0; y < DGVsklad1.Rows.Count; y++)
                            {
                                if (Convert.ToString(dataGridView1.Rows[i].Cells[5].Value) == Convert.ToString(DGVsklad1.Rows[y].Cells[2].Value))
                                {
                                    int summ_sale = Convert.ToInt32(DGVsklad1.Rows[y].Cells[6].Value);
                                    int kol_vo_sale = Convert.ToInt32(DGVsklad1.Rows[y].Cells[7].Value);
                                    int summ_zakup = Convert.ToInt32(DGVsklad1.Rows[y].Cells[5].Value);
                                    int kol_vo_zakup = Convert.ToInt32(DGVsklad1.Rows[y].Cells[4].Value);
                                    int price_sale = Convert.ToInt32(DGVsklad1.Rows[y].Cells[3].Value);
                                    con.Open();//открыть соединение
                                    SqlCommand cmd = new SqlCommand("INSERT INTO [Table_sklad] (product,kol_vo_itog,price_ostatok)" +
                                        "VALUES (@product,@kol_vo_itog,@price_ostatok)", con);
                                    cmd.Parameters.AddWithValue("@product", Convert.ToString(dataGridView1.Rows[i].Cells[5].Value));
                                    cmd.Parameters.AddWithValue("@kol_vo_itog", (kol_vo_zakup - kol_vo_sale));
                                    cmd.Parameters.AddWithValue("@price_ostatok", ((price_sale * kol_vo_sale) - summ_zakup));
                                    cmd.ExecuteNonQuery();
                                    con.Close();//закрыть соединение
                                }
                            }
                        }
                        ChekPDF();//выдача чека
                        Select_sklad();
                        Select_sklad();
                        Disp_data();
                        Select_chek();
                        Select_chek_magaz();
                        Select_zakaz();
                        Disp_data();
                        //очистка текстовых полей
                        comboBox16.Text = "0";
                        button4.Enabled = true;
                        if (DGV1.Rows.Count >= 2)
                        {
                            DGV1.Rows[0].Selected = true;// выбираем следующий заказ
                        }
                        
                    }
                }
                else MessageBox.Show("Чек пуст!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }
        private void ChekPDF()//Выдача чека
        {
            //Определение шрифта необходимо для сохранения кириллического текста
            //Иначе мы не увидим кириллический текст
            //Если мы работаем только с англоязычными текстами, то шрифт можно не указывать
            BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 6, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font fontBold = new iTextSharp.text.Font(baseFont, 6, iTextSharp.text.Font.BOLD);
            //Обход по всем таблицам датасета
            for (int i = 0; i < DGV2.Rows.Count; i++)
            {
                for (int w = 0; w < DGVmagaz.Rows.Count; w++)
                {
                    //Создаем объект таблицы и передаем в нее число столбцов таблицы из нашего датасета
                    PdfPTable table = new PdfPTable(DGV2.Rows[i].Cells.Count);
                    table.DefaultCell.Padding = 1;
                    table.WidthPercentage = 100;
                    float[] widths = new float[] { 45f, 20f, 25f, 25f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.DefaultCell.BorderWidth = 0;

                    //Добавим в таблицу общий заголовок
                    string organization = label29.Text;
                    PdfPCell cell = new PdfPCell(new Phrase(organization, fontBold));
                    cell.Colspan = DGV2.Rows[i].Cells.Count;
                    cell.HorizontalAlignment = 1;
                    //Убираем границу первой ячейки, чтобы была как заголовок
                    cell.Border = 0;
                    table.AddCell(cell);
                    //Сначала добавляем заголовки таблицы
                    for (int j = 0; j < DGV2.Columns.Count; j++)
                    {
                        cell = new PdfPCell(new Phrase(DGV2.Columns[j].HeaderText.ToString(), font));
                        //Фоновый цвет (необязательно, просто сделаем по красивее)
                        //cell.BackgroundColor = BaseColor.LIGHT_GRAY;                   
                        cell.Border = 0;
                        table.AddCell(cell);
                    }
                    //Добавляем все остальные ячейки
                    for (int x = 0; x < DGV2.Rows.Count; x++)
                    {
                        for (int k = 0; k < DGV2.Columns.Count; k++)
                        {
                            table.AddCell(new Phrase(DGV2.Rows[x].Cells[k].Value.ToString(), font));
                        }
                    }
                    /////////------------------------------------------Вторая таблица Магазин--------------------------------------------------////////////
                    //Создаем объект таблицы и передаем в нее число столбцов таблицы из нашего датасета
                    PdfPTable table2 = new PdfPTable(DGVmagaz.Rows[w].Cells.Count);
                    table2.DefaultCell.Padding = 1;
                    table2.WidthPercentage = 100;
                    table2.SetWidths(widths);
                    table2.HorizontalAlignment = Element.ALIGN_LEFT;
                    table2.DefaultCell.BorderWidth = 0;

                    if (DGV2.Rows[0].Cells[0].Value.ToString() == "Нет заказов")
                    {
                        //Добавим во вторую таблицу общий заголовок фирмы если первая таблица пустая
                        PdfPCell cell2 = new PdfPCell(new Phrase(organization, fontBold));
                        cell2.Colspan = DGVmagaz.Rows[i].Cells.Count;
                        cell2.HorizontalAlignment = 1;
                        //Убираем границу первой ячейки, чтобы была как заголовок
                        cell2.Border = 0;
                        table2.AddCell(cell2);
                    }
                    //Сначала добавляем заголовки таблицы
                    for (int j = 0; j < DGVmagaz.Columns.Count; j++)
                    {
                        cell = new PdfPCell(new Phrase(DGVmagaz.Columns[j].HeaderText.ToString(), font));
                        cell.Border = 0;
                        table2.AddCell(cell);
                    }
                    //Добавляем все остальные ячейки
                    for (int x = 0; x < DGVmagaz.Rows.Count; x++)
                    {
                        for (int k = 0; k < DGVmagaz.Columns.Count; k++)
                        {
                            table2.AddCell(new Phrase(DGVmagaz.Rows[x].Cells[k].Value.ToString(), font));
                        }
                    }
                    //----------------------------------------------------------------------------------------------------------//
                    //Exporting to PDF
                    string folderPath = "C:\\PDFs\\";
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    using (FileStream stream = new FileStream(folderPath + "Chek.pdf", FileMode.Create))
                    {
                        //Document Doc = new Document(PageSize.A8, 1f, 1f, 1f, 1f);
                        //Document Doc = new Document(new iTextSharp.text.Rectangle(Width, Height), 0, 0, 0, 0);
                        Document Doc = new Document(new iTextSharp.text.Rectangle(120, 1000), 0f, 0f, 0f, 0f);
                        PdfWriter.GetInstance(Doc, stream);
                        Doc.Open();
                        DateTime date = DateTime.Now;
                        string summ = label23.Text;
                        string skidka = label20.Text;
                        string ZP = label38.Text;
                        string allsumm = label25.Text;
                        string chek = label2.Text;
                        string vremia = DGV1.CurrentRow.Cells[1].Value.ToString();
                        string adres = DGVrequisites.Rows[0].Cells[1].Value.ToString();
                        string magazin = label64.Text;
                        Doc.Add(new Paragraph("CashBox " + "Чек № " + chek, font));
                        Doc.Add(new Paragraph("Время посещения: " + vremia, font));
                        Doc.Add(new Paragraph("Время ухода: " + date, font));
                        Doc.Add(new Paragraph("Касир - " + label10.Text, font));
                        Doc.Add(new Paragraph("Адрес: " + adres, font));
                        Doc.Add(new Paragraph("******-------------------------------------------******", font));
                        if (DGV2.Rows[0].Cells[0].Value.ToString() != "Нет заказов")
                        {
                            Doc.Add(table);//таблица
                            Doc.Add(new Paragraph("******-------------------------------------------******", font));
                        }
                        //магазин//
                        if (DGVmagaz.Rows[0].Cells[0].Value.ToString() != "Нет заказов")
                        {
                            Doc.Add(table2);//таблица  
                            Doc.Add(new Paragraph("******-------------------------------------------******", font));
                        }
                        Doc.Add(new Paragraph("Сумма: " + allsumm + " Сом", font));
                        Doc.Add(new Paragraph("Без обслуживания: " + magazin + " Сом", font));
                        Doc.Add(new Paragraph("Скидка: " + skidka + " Сом", font));
                        Doc.Add(new Paragraph("Обслуживание 15% : " + ZP + " Сом", font));
                        Doc.Add(new Paragraph("Итого: " + summ + " Сом", font));
                        Doc.Add(new Paragraph("***---СПАСИБО! ЖДЕМ ВАС СНОВА!---***", font));
                        Doc.Close();
                        stream.Close();
                    }
                }
            }
            // Печать на устройство, установленное используемым по умолчанию
            Process printJob = new Process();
            printJob.StartInfo.FileName = @"C:\\PDFs\\Chek.pdf";
            printJob.StartInfo.UseShellExecute = true;
            //printJob.StartInfo.Verb = "print";
            printJob.Start();

            printJob.WaitForInputIdle();
            //printJob.Kill();
        }
        private void ALLchekPDF()//Выдача чека смены 
        {
            //Определение шрифта необходимо для сохранения кириллического текста
            //Иначе мы не увидим кириллический текст
            //Если мы работаем только с англоязычными текстами, то шрифт можно не указывать
            BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 6, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font fontBold = new iTextSharp.text.Font(baseFont, 6, iTextSharp.text.Font.BOLD);

            //Обход по всем таблицам датасета
            for (int i = 0; i < DGV2.Rows.Count; i++)
            {
                //Создаем объект таблицы и передаем в нее число столбцов таблицы из нашего датасета
                PdfPTable table = new PdfPTable(DGV2.Rows[i].Cells.Count);
                //Creating iTextSharp Table from the DataTable data
                table.DefaultCell.Padding = 1;
                table.WidthPercentage = 100;
                float[] widths = new float[] { 30f, 20f, 20f, 30f };
                table.SetWidths(widths);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.DefaultCell.BorderWidth = 0;
                //Добавим в таблицу общий заголовок
                //PdfPCell cell = new PdfPCell(new Phrase("Сауна " + " НИАГАРА", font));
                string organization = label29.Text;
                PdfPCell cell = new PdfPCell(new Phrase(organization, fontBold));

                cell.Colspan = DGV2.Rows[i].Cells.Count;
                cell.HorizontalAlignment = 1;
                //Убираем границу первой ячейки, чтобы была как заголовок
                cell.Border = 0;
                table.AddCell(cell);

                //Сначала добавляем заголовки таблицы
                for (int j = 0; j < DGV2.Columns.Count; j++)
                {
                    cell = new PdfPCell(new Phrase(DGV2.Columns[j].HeaderText.ToString(), font));
                    //Фоновый цвет (необязательно, просто сделаем по красивее)
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell.Border = 0;
                    table.AddCell(cell);
                }
                //Добавляем все остальные ячейки
                for (int x = 0; x < DGV2.Rows.Count; x++)
                {
                    for (int k = 0; k < DGV2.Columns.Count; k++)
                    {
                        table.AddCell(new Phrase(DGV2.Rows[x].Cells[k].Value.ToString(), font));
                    }
                }

                //Exporting to PDF
                string folderPath = "C:\\PDFs\\";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                using (FileStream stream = new FileStream(folderPath + "AllChek.pdf", FileMode.Create))
                {
                    //Document Doc = new Document(PageSize.A8, 1f, 1f, 1f, 1f);
                    Document Doc = new Document(new iTextSharp.text.Rectangle(120, 400), 0f, 0f, 0f, 0f);
                    PdfWriter.GetInstance(Doc, stream);
                    Doc.Open();
                    DateTime date = Convert.ToDateTime(label41.Text);
                    string summ = label23.Text;
                    string skidka = label20.Text;
                    string chek = label21.Text;
                    string ZP = label38.Text;
                    string allsumm = label25.Text;
                    Doc.Add(new Paragraph("CashBox", font));
                    Doc.Add(new Paragraph("Дата открытия смены: " + date, font));
                    Doc.Add(new Paragraph("Дата закрытия смены: " + DateTime.Now, font));
                    Doc.Add(new Paragraph("Касир - " + label10.Text, font));
                    Doc.Add(new Paragraph("******-------------------------------------------******", font));
                    Doc.Add(table);//таблица
                    Doc.Add(new Paragraph("******-------------------------------------------******", font));
                    Doc.Add(new Paragraph("Сумма: " + summ, font));
                    Doc.Add(new Paragraph("Зарплата: " + ZP, font));
                    Doc.Add(new Paragraph("Скидка: " + skidka + " Сом", font));
                    Doc.Add(new Paragraph("Итого: " + allsumm + " Сом", font));
                    Doc.Add(new Paragraph("Количество чеков: " + chek, font));
                    Doc.Add(new Paragraph("----------------------------------------------------------", font));
                    Doc.Close();
                    stream.Close();
                }
            }
            // Печать на устройство, установленное используемым по умолчанию
            Process printJob = new Process();
            printJob.StartInfo.FileName = @"C:\\PDFs\\AllChek.pdf";
            printJob.StartInfo.UseShellExecute = true;
            //printJob.StartInfo.Verb = "print";
            printJob.Start();

            printJob.WaitForInputIdle();
            //printJob.Kill();
        }

        private void Button8_Click(object sender, EventArgs e)//Закрыть смену
        {
            try
            {
                if (DGV1.Rows.Count == 1)
                {
                    if (MessageBox.Show("Подтвердите действие, при закрытии смены будет выдан чек", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        con.Open();//Открываем соединение
                        SqlCommand cmd1 = new SqlCommand("SELECT SUM(chek) AS ЧЕК FROM [Table]" +
                            "WHERE kasir=@kasir AND smena=@smena", con);
                        cmd1.Parameters.AddWithValue("@smena", "Открыта");
                        cmd1.Parameters.AddWithValue("@kasir", label10.Text);
                        cmd1.ExecuteNonQuery();
                        DataTable dt1 = new DataTable();//создаем экземпляр класса DataTable
                        SqlDataAdapter da1 = new SqlDataAdapter(cmd1);//создаем экземпляр класса SqlDataAdapter
                        dt1.Clear();//чистим DataTable, если он был не пуст
                        da1.Fill(dt1);//заполняем данными созданный DataTable                   
                        con.Close();//Закрываем соединение
                        foreach (DataRow row in dt1.Rows)
                        {
                            label21.Text = row[0].ToString();
                        }

                        con.Open();//Открываем соединение
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT category AS Категории, SUM(summ) AS Сумма, SUM(skidka) AS Скидка, SUM(zarplata) AS Зарплата FROM [Table]" +
                            "WHERE kasir=@kasir AND smena=@smena GROUP BY category ORDER BY category DESC";
                        cmd.Parameters.AddWithValue("@smena", "Открыта");
                        cmd.Parameters.AddWithValue("@kasir", label10.Text);
                        cmd.ExecuteNonQuery();
                        DataTable dt = new DataTable();//создаем экземпляр класса DataTable
                        SqlDataAdapter da = new SqlDataAdapter(cmd);//создаем экземпляр класса SqlDataAdapter
                        dt.Clear();//чистим DataTable, если он был не пуст
                        da.Fill(dt);//заполняем данными созданный DataTable
                        DGV2.DataSource = dt;//в качестве источника данных у dataGridView используем DataTable заполненный данными
                        con.Close();//Закрываем соединение

                        //--------------------------------------------------Вытащим дату открытия смены
                        con.Open();//Открываем соединение
                        SqlCommand cmd2 = con.CreateCommand();
                        cmd2.CommandType = CommandType.Text;
                        cmd2.CommandText = "SELECT datazapisi FROM [Table] WHERE kasir=@kasir AND status=@status AND zakaz=@zakaz AND smena = @smena ORDER BY zakaz DESC";
                        cmd2.Parameters.AddWithValue("@zakaz", 1);
                        cmd2.Parameters.AddWithValue("@smena", "Открыта");
                        cmd2.Parameters.AddWithValue("@status", "Закрыт");
                        cmd2.Parameters.AddWithValue("@kasir", label10.Text);
                        cmd2.ExecuteNonQuery();
                        DataTable dt2 = new DataTable();//создаем экземпляр класса DataTable
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);//создаем экземпляр класса SqlDataAdapter
                        dt2.Clear();//чистим DataTable, если он был не пуст
                        da2.Fill(dt2);//заполняем данными созданный DataTable
                        dataGridView1.DataSource = dt2;//в качестве источника данных у dataGridView используем DataTable заполненный данными
                        con.Close();//Закрываем соединение
                        label41.Text = dataGridView1.Rows[0].Cells[0].Value.ToString();

                        if (DGV2.Rows.Count >= 1 | DGVmagaz.Rows.Count >= 1)
                        {
                            //сумма скидки
                            double skidka = 0;
                            foreach (DataGridViewRow row in DGV2.Rows)
                            {
                                double incom;
                                double.TryParse((row.Cells[2].Value ?? "0").ToString().Replace(".", ","), out incom);
                                skidka += incom;
                            }
                            label20.Text = skidka.ToString();
                        }
                        //Итого к оплате - скидка - зарплата
                        if (DGV2.Rows.Count >= 1 | DGVmagaz.Rows.Count >= 1)
                        {
                            double summa = 0;
                            foreach (DataGridViewRow row in DGV2.Rows)
                            {
                                double incom;
                                double.TryParse((row.Cells[1].Value ?? "0").ToString().Replace(".", ","), out incom);
                                summa += incom;
                            }
                            label25.Text = summa.ToString();

                            double zarplata = 0;
                            foreach (DataGridViewRow row in DGV2.Rows)
                            {
                                double incom;
                                double.TryParse((row.Cells[3].Value ?? "0").ToString().Replace(".", ","), out incom);
                                zarplata += incom;
                            }
                            label38.Text = zarplata.ToString();
                            double ZP = Convert.ToDouble(label38.Text);
                            double itog = Convert.ToDouble(label25.Text);
                            double skidka = Convert.ToDouble(label20.Text);
                            double itogo = Math.Round(itog + skidka + ZP);
                            label20.Text = skidka.ToString() + " Сом";
                            label38.Text = ZP.ToString() + " Сом";
                            label23.Text = itogo.ToString() + " Сом";
                        }
                        //----------------------------------------------------------------------------------------------//
                        ALLchekPDF();

                        con.Open();//открыть соединение
                        SqlCommand cmd3 = new SqlCommand("UPDATE [Table] SET smena=@smena, datazakrytiya=@datazakrytiya WHERE kasir=@kasir", con);
                        cmd3.Parameters.AddWithValue("@kasir", label10.Text);
                        cmd3.Parameters.AddWithValue("@smena", "Закрыта");
                        cmd3.Parameters.AddWithValue("@datazakrytiya", DateTime.Now);
                        cmd3.ExecuteNonQuery();
                        con.Close();//закрыть соединение

                        //выход
                        Form1.Show();
                        this.Hide();
                    }
                }
                else MessageBox.Show("Остались открытые заказы!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Нет открытых смен!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }
        private void Button11_Click(object sender, EventArgs e)//Выход
        {
            Form1.Show();
            this.Hide();
        }
        //Админ панель
        private void ComboBoxProduct_TextChanged(object sender, EventArgs e)//Поиск продукта
        {
            con.Open();//открыть соединение
            SqlCommand cmd = new SqlCommand("SELECT id, product, price_sale, category FROM [Table_Items]" +
                "WHERE product LIKE N'%" + comboBoxProduct.Text.ToString() + "%'", con);
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();//создаем экземпляр класса DataTable
            SqlDataAdapter da = new SqlDataAdapter(cmd);//создаем экземпляр класса SqlDataAdapter
            dt.Clear();//чистим DataTable, если он был не пуст
            da.Fill(dt);//заполняем данными созданный DataTable
            DGV3.DataSource = dt;//в качестве источника данных у dataGridView используем DataTable заполненный данными
            con.Close();//закрыть соединение
            DGV3.Columns[1].HeaderText = "Наименование";
            if (comboBoxProduct.Text == "")//если поле очищено, отобразить базу
            {
                dt.Clear();//чистим DataTable, если он был не пуст
                foreach (DataRow row in dt.Rows)
                {
                    comboBoxProduct.Items.Add(row[0].ToString());
                }
            }
        }
        private void ComboBoxCategory_TextChanged(object sender, EventArgs e)//Поиск категории
        {
            con.Open();//открыть соединение
            SqlCommand cmd = new SqlCommand("SELECT id, categories FROM [Table_Categories]" +
                "WHERE categories IS NOT NULL AND categories LIKE N'%" + comboBoxCategory.Text.ToString() + "%'", con);
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();//создаем экземпляр класса DataTable
            SqlDataAdapter da = new SqlDataAdapter(cmd);//создаем экземпляр класса SqlDataAdapter
            dt.Clear();//чистим DataTable, если он был не пуст
            da.Fill(dt);//заполняем данными созданный DataTable
            DGV3.DataSource = dt;//в качестве источника данных у dataGridView используем DataTable заполненный данными
            con.Close();//закрыть соединение
            DGV3.Columns[1].HeaderText = "Наименование";
            if (comboBoxCategory.Text == "")//если поле очищено, отобразить базу
            {
                dt.Clear();//чистим DataTable, если он был не пуст
                foreach (DataRow row in dt.Rows)
                {
                    comboBoxCategory.Items.Add(row[0].ToString());
                }
            }
            Produkt_select_admin();
        }
        private void Button22_Click(object sender, EventArgs e)//Добавить
        {
            MemoryStream ms = new MemoryStream();
            /*saving the image in raw format from picture box*/
            pictureBox3.Image.Save(ms, pictureBox3.Image.RawFormat);
            /*Array of Binary numbers that have been converted*/
            byte[] ProductPicture = ms.GetBuffer();
            /*closing the memory stream*/
            ms.Close();

                if (comboBoxCategory.Text != "" & comboBoxProduct.Text != "" & TextBoxprice.Text != "" )
                {
                    con.Open();//открыть соединение
                    SqlCommand cmd = new SqlCommand("INSERT INTO [Table_Items] (product, price_sale,category,image) VALUES (@product,@price_sale,@category,@image)", con);
                    cmd.Parameters.AddWithValue("@product", comboBoxProduct.Text);
                    cmd.Parameters.AddWithValue("@price_sale", TextBoxprice.Text);
                    cmd.Parameters.AddWithValue("@category", comboBoxCategory.Text);
                    cmd.Parameters.AddWithValue("@image", ProductPicture);
                    cmd.ExecuteNonQuery();
                    con.Close();//закрыть соединение
                    MessageBox.Show("Вы успешно добавили продукт!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (comboBoxCategory.Text != "" & comboBoxProduct.Text == "" & TextBoxprice.Text == "" )
                {
                    con.Open();//открыть соединение
                    SqlCommand cmd = new SqlCommand("INSERT INTO [Table_Categories] (categories,image) VALUES (@categories,@image)", con);
                    cmd.Parameters.AddWithValue("@categories", comboBoxCategory.Text);
                    cmd.Parameters.AddWithValue("@image", ProductPicture);
                    cmd.ExecuteNonQuery();
                    con.Close();//закрыть соединение
                    MessageBox.Show("Вы успешно добавили категорию!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (comboBoxProduct.Text == "" | TextBoxprice.Text == "")
                {
                    MessageBox.Show("Не все поля заполнены!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (comboBoxProduct.Text == DGV3.Rows[0].Cells[1].Value.ToString())
                {
                    MessageBox.Show("Данный продукт уже имеется!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (comboBoxCategory.Text == DGV3.Rows[0].Cells[1].Value.ToString())
                {
                    MessageBox.Show("Данная категория уже имеется!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else MessageBox.Show("Не все поля заполнены!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Button7_Click(object sender, EventArgs e)//Изменить
        {
            MemoryStream ms = new MemoryStream();
            /*saving the image in raw format from picture box*/
            pictureBox3.Image.Save(ms, pictureBox3.Image.RawFormat);
            /*Array of Binary numbers that have been converted*/
            byte[] ProductPicture = ms.GetBuffer();
            /*closing the memory stream*/
            ms.Close();
            /*HASHING END HERE*/
            if (comboBoxCategory.Text != "" & comboBoxProduct.Text != "" & TextBoxprice.Text != "")
            {
                con.Open();//открыть соединение
                SqlCommand cmd = new SqlCommand("UPDATE [Table_Items] SET product = @product, price_sale=@price_sale,category=@category, image=@image WHERE id=@id", con);
                cmd.Parameters.AddWithValue("@id", DGV3.Rows[0].Cells[0].Value);
                cmd.Parameters.AddWithValue("@product", comboBoxProduct.Text);
                cmd.Parameters.AddWithValue("@price_sale", TextBoxprice.Text);
                cmd.Parameters.AddWithValue("@category", comboBoxCategory.Text);
                cmd.Parameters.AddWithValue("@image", ProductPicture);
                cmd.ExecuteNonQuery();
                con.Close();//закрыть соединение
                MessageBox.Show("Вы успешно изменили продукт!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (comboBoxCategory.Text != "" & comboBoxProduct.Text == "" & TextBoxprice.Text == "")
            {
                con.Open();//открыть соединение
                SqlCommand cmd = new SqlCommand("UPDATE [Table_Categories] SET categories = @categories, image=@image WHERE id=@id", con);
                cmd.Parameters.AddWithValue("@id", DGV3.Rows[0].Cells[0].Value);
                cmd.Parameters.AddWithValue("@categories", comboBoxCategory.Text);
                cmd.Parameters.AddWithValue("@image", ProductPicture);
                cmd.ExecuteNonQuery();
                con.Close();//закрыть соединение
                MessageBox.Show("Вы успешно изменили категорию!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (comboBoxProduct.Text == "" | TextBoxprice.Text == "")
            {
                MessageBox.Show("Не все поля заполнены!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else MessageBox.Show("Не все поля заполнены!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Button9_Click(object sender, EventArgs e)//Удалить
        {
            if (comboBoxProduct.Text != "" & comboBoxProduct.Text == DGV3.Rows[0].Cells[1].Value.ToString())
            {
                con.Open();//открыть соединение
                SqlCommand cmd = new SqlCommand("DELETE FROM [Table_Items] WHERE id=@id", con);
                cmd.Parameters.AddWithValue("@id", DGV3.Rows[0].Cells[0].Value);
                cmd.ExecuteNonQuery();
                con.Close();//закрыть соединение
                MessageBox.Show("Вы успешно удалили продукт!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (comboBoxCategory.Text != "" & comboBoxProduct.Text == "" & TextBoxprice.Text == "" & comboBoxCategory.Text == DGV3.Rows[0].Cells[1].Value.ToString())
            {
                con.Open();//открыть соединение
                SqlCommand cmd = new SqlCommand("DELETE FROM [Table_Categories] WHERE id=@id", con);
                cmd.Parameters.AddWithValue("@id", DGV3.Rows[0].Cells[0].Value);
                cmd.ExecuteNonQuery();
                con.Close();//закрыть соединение
                MessageBox.Show("Вы успешно удалили категорию!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else MessageBox.Show("Не все поля заполнены!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Button10_Click(object sender, EventArgs e)//Загрузить изображение
        {
            /*selecting image*/
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Image file.."; //"Выберите файлы для обработки";
            ofd.DefaultExt = ".jpg";
            ofd.Filter = "Media Files|*.jpg;*.png;*.gif;*.bmp;*.jpeg|All Files|*.*";
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    //изменить размер изображения при загрузке               
                    using (Bitmap Original = new Bitmap(Image.FromFile(ofd.FileName)))
                    {
                        int newHeight = 40;
                        int newWidth = 40;
                        using (Bitmap bitmap = new Bitmap(newWidth, newHeight))
                        {
                            using (Graphics Graphics = Graphics.FromImage(bitmap))
                            {
                                Graphics.DrawImage(Original, 0, 0, (newWidth), (newHeight));
                                string newFle = "Image";
                                bitmap.Save(newFle, ImageFormat.Png);
                                pictureBox3.Load(newFle);
                            }
                            bitmap.Dispose();
                        }
                        Original.Dispose();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Файл занят!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void Button13_Click(object sender, EventArgs e)//Добавить реквизиты
        {
            if (textBox7.Text != "" & textBox8.Text != "" & textBox9.Text != "" & DGVrequisites.Rows.Count <= 0)
            {
                con.Open();//открыть соединение
                SqlCommand cmd = new SqlCommand("INSERT INTO [Table_requisites] (organization, adress, name) VALUES (@organization, @adress, @name)", con);
                cmd.Parameters.AddWithValue("@organization", textBox7.Text);
                cmd.Parameters.AddWithValue("@adress", textBox8.Text);
                cmd.Parameters.AddWithValue("@name", textBox9.Text);
                cmd.ExecuteNonQuery();
                con.Close();//закрыть соединение
                MessageBox.Show("Вы успешно добавили реквизиты!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (DGVrequisites.Rows.Count >= 0)
            {
                MessageBox.Show("Реквизиты уже заполнены!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else MessageBox.Show("Небходимо заполнить все поля!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Button14_Click(object sender, EventArgs e)//Изменить реквизиты
        {
            if (textBox7.Text != "" & textBox8.Text != "" & textBox9.Text != "")
            {
                con.Open();//открыть соединение
                SqlCommand cmd = new SqlCommand("UPDATE [Table_requisites] SET organization=@organization, adress=@adress, name=@name", con);
                cmd.Parameters.AddWithValue("@organization", textBox7.Text);
                cmd.Parameters.AddWithValue("@adress", textBox8.Text);
                cmd.Parameters.AddWithValue("@name", textBox9.Text);
                cmd.ExecuteNonQuery();
                con.Close();//закрыть соединение
                MessageBox.Show("Вы успешно изменили реквизиты!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else MessageBox.Show("Небходимо заполнить все поля!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Button15_Click(object sender, EventArgs e)//Добавить Юзера
        {
            if (comboBox15.Text != "" & textBox11.Text != "" & comboBox14.Text != "")
            {
                con.Open();//открыть соединение
                SqlCommand cmd = new SqlCommand("INSERT INTO [Table_Login] (logins, password, dostup) VALUES (@logins, @password, @dostup)", con);
                cmd.Parameters.AddWithValue("@logins", comboBox15.Text);
                cmd.Parameters.AddWithValue("@password", textBox11.Text);
                cmd.Parameters.AddWithValue("@dostup", comboBox14.Text);
                cmd.ExecuteNonQuery();
                con.Close();//закрыть соединение
                MessageBox.Show("Вы успешно добавили нового юзера!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void Button16_Click(object sender, EventArgs e)//Удалить Юзера
        {
            if (DGVlogin.Rows[0].Cells[2].Value.ToString() != "root")
            {
                con.Open();//открыть соединение
                SqlCommand cmd = new SqlCommand("DELETE FROM [Table_Login] WHERE id=@id", con);
                cmd.Parameters.AddWithValue("@id", DGVlogin.Rows[0].Cells[0].Value.ToString());
                cmd.ExecuteNonQuery();
                con.Close();//закрыть соединение
                MessageBox.Show("Вы успешно удалили юзера!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (DGVlogin.Rows[0].Cells[2].Value.ToString() == "root")
            {
                MessageBox.Show("root не возможно удалить это SuperUser!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            //DGVF1.DataSource = dt;//в качестве источника данных у dataGridView используем DataTable заполненный данными
            foreach (DataRow row in dt.Rows)
            {
                comboBox15.Items.Add(row[0].ToString());
                comboBox18.Items.Add(row[0].ToString());
            }
            con.Close();//Закрываем соединение
        }
        public void Produkt_select_admin()//Вывод продуктов в Combobox Admin
        {
            comboBoxProduct.Items.Clear();//Очищаем комбо перед каждым заполнением
            con.Open();//Открываем соединение
            SqlCommand cmd = new SqlCommand("SELECT product FROM [Table_Items] WHERE category=@category ORDER BY product", con);
            cmd.Parameters.AddWithValue("@category", comboBoxCategory.Text);
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();//создаем экземпляр класса DataTable
            SqlDataAdapter da = new SqlDataAdapter(cmd);//создаем экземпляр класса SqlDataAdapter
            dt.Clear();//чистим DataTable, если он был не пуст
            da.Fill(dt);//заполняем данными созданный DataTable
            con.Close();//Закрываем соединение           
            foreach (DataRow row in dt.Rows)
            {
                comboBoxProduct.Items.Add(row[0].ToString());
            }
        }
        public void Produkt_select_sklad()//Вывод продуктов в Combobox Склад
        {
            comboBox12.Items.Clear();//Очищаем комбо перед каждым заполнением
            con.Open();//Открываем соединение
            SqlCommand cmd = new SqlCommand("SELECT product FROM [Table_Items] WHERE category=@category ORDER BY product", con);
            cmd.Parameters.AddWithValue("@category", comboBox13.Text);
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();//создаем экземпляр класса DataTable
            SqlDataAdapter da = new SqlDataAdapter(cmd);//создаем экземпляр класса SqlDataAdapter
            dt.Clear();//чистим DataTable, если он был не пуст
            da.Fill(dt);//заполняем данными созданный DataTable
            con.Close();//Закрываем соединение           
            foreach (DataRow row in dt.Rows)
            {
                comboBox12.Items.Add(row[0].ToString());
            }
        }
        //Поиск в админке
        private void ComboBox15_TextChanged(object sender, EventArgs e)//Юзеры
        {
            con.Open();//открыть соединение
            SqlCommand cmd = new SqlCommand("SELECT id, logins, dostup FROM [Table_Login]" +
                "WHERE logins LIKE N'%" + comboBox15.Text.ToString() + "%'", con);//префикс N для отображения русских слов в localDB
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();//создаем экземпляр класса DataTable
            SqlDataAdapter da = new SqlDataAdapter(cmd);//создаем экземпляр класса SqlDataAdapter
            dt.Clear();//чистим DataTable, если он был не пуст
            da.Fill(dt);//заполняем данными созданный DataTable
            DGVlogin.DataSource = dt;//в качестве источника данных у dataGridView используем DataTable заполненный данными
            con.Close();//закрыть соединение
            DGVlogin.Columns[1].HeaderText = "Имя";
            if (comboBox15.Text == "")//если поле очищено, отобразить базу
            {
                dt.Clear();//чистим DataTable, если он был не пуст
                foreach (DataRow row in dt.Rows)
                {
                    comboBox15.Items.Add(row[0].ToString());
                }
            }
        }
        //Склад
        private void ComboBox12_TextChanged(object sender, EventArgs e)//Поиск продукта склад
        {
            con.Open();//открыть соединение
            SqlCommand cmd = new SqlCommand("SELECT product,price_sale FROM [Table_Items]" +
                "WHERE product LIKE N'%" + comboBox12.Text.ToString() + "%'", con);
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();//создаем экземпляр класса DataTable
            SqlDataAdapter da = new SqlDataAdapter(cmd);//создаем экземпляр класса SqlDataAdapter
            dt.Clear();//чистим DataTable, если он был не пуст
            da.Fill(dt);//заполняем данными созданный DataTable
            con.Close();//закрыть соединение
            DGV3.DataSource = dt;//в качестве источника данных у dataGridView используем DataTable заполненный данными
            if (comboBox12.Text == "")//если поле очищено, отобразить базу
            {
                dt.Clear();//чистим DataTable, если он был не пуст
                foreach (DataRow row in dt.Rows)
                {
                    comboBox12.Items.Add(row[0].ToString());
                }
            }
        }
        private void ComboBox13_TextChanged(object sender, EventArgs e)//Поиск категории склад
        {
            con.Open();//открыть соединение
            SqlCommand cmd = new SqlCommand("SELECT categories FROM [Table_Categories]" +
                "WHERE categories IS NOT NULL AND categories LIKE N'%" + comboBox13.Text.ToString() + "%'", con);
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();//создаем экземпляр класса DataTable
            SqlDataAdapter da = new SqlDataAdapter(cmd);//создаем экземпляр класса SqlDataAdapter
            dt.Clear();//чистим DataTable, если он был не пуст
            da.Fill(dt);//заполняем данными созданный DataTable
            con.Close();//закрыть соединение
            DGV3.DataSource = dt;//в качестве источника данных у dataGridView используем DataTable заполненный данными
            if (comboBox13.Text == "")//если поле очищено, отобразить базу
            {
                dt.Clear();//чистим DataTable, если он был не пуст
                foreach (DataRow row in dt.Rows)
                {
                    comboBox13.Items.Add(row[0].ToString());
                }
            }
            //--------------Для склада-------
            Produkt_select_sklad();
        }
        private void Button12_Click(object sender, EventArgs e)//Приход
        {
            if (textBox5.Text != "" & textBox6.Text != "" & comboBox12.Text != "")
            {
                con.Open();//открыть соединение
                SqlCommand cmd = new SqlCommand("INSERT INTO [Table_sklad] (product,price_zakup,kol_vo_zakup,summ_sale,kol_vo_sale,price_sale)" +
                    " VALUES (@product,@price_zakup,@kol_vo_zakup,@summ_sale,@kol_vo_sale,@price_sale)", con);
                cmd.Parameters.AddWithValue("@product", comboBox12.Text);
                cmd.Parameters.AddWithValue("@price_sale", DGV3.Rows[0].Cells[1].Value.ToString());
                cmd.Parameters.AddWithValue("@price_zakup", textBox5.Text);
                cmd.Parameters.AddWithValue("@kol_vo_zakup", textBox6.Text);
                cmd.Parameters.AddWithValue("@summ_sale", 0);
                cmd.Parameters.AddWithValue("@kol_vo_sale", 0);
                cmd.Parameters.AddWithValue("@kol_vo_itog", DBNull.Value);
                cmd.Parameters.AddWithValue("@price_ostatok", DBNull.Value);
                cmd.ExecuteNonQuery();
                con.Close();//закрыть соединение
            }
            else MessageBox.Show("Не заполнено поле цена или кол-во!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Select_sklad();
            Select_sklad();
            textBox5.Text = "";
            textBox6.Text = "";
        }
        private void Select_sklad()//формирование склада
        {
            con.Open();//Открываем соединение
            SqlCommand cmd = new SqlCommand("SELECT MAX(id) AS ID, Max(data_zakup) AS 'Дата закупки', product AS 'Товар', MAX(price_sale) AS 'Цена продажи', SUM(kol_vo_zakup) AS 'Кол-во закупки'," +
                "SUM(summ_zakup) AS 'Сумма закупки', SUM(summ_sale) AS 'Сумма продаж', SUM(kol_vo_sale) AS 'Кол-во продажи', MIN(kol_vo_itog) AS 'Остаток', MIN(price_ostatok) AS 'Сумма остатка'," +
                "MIN(data_sale) AS 'Дата продажи' FROM [Table_sklad]" +
                "GROUP BY product ORDER BY product DESC", con);
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();//создаем экземпляр класса DataTable
            SqlDataAdapter da = new SqlDataAdapter(cmd);//создаем экземпляр класса SqlDataAdapter
            dt.Clear();//чистим DataTable, если он был не пуст
            da.Fill(dt);//заполняем данными созданный DataTable
            DGVsklad1.DataSource = dt;//в качестве источника данных у dataGridView используем DataTable заполненный данными
            con.Close();//Закрываем соединение

            con.Open();//открыть соединение
            for (int i = 0; i < DGVsklad1.Rows.Count; i++)//Цикл
            {
                SqlCommand cmd1 = new SqlCommand("UPDATE [Table_sklad] SET summ_zakup = (kol_vo_zakup * price_zakup) WHERE id = @id", con);
                cmd1.Parameters.AddWithValue("@id", Convert.ToInt32(DGVsklad1.Rows[i].Cells[0].Value));
                cmd1.ExecuteNonQuery();
            }
            con.Close();//закрыть соединение                  
        }
        //Отчеты
        private void Button17_Click(object sender, EventArgs e)//Выборка
        {           
            if (checkBox2.Checked == true)
            {
                con.Open();//Открываем соединение
                SqlCommand cmd = new SqlCommand("SELECT kasir, product, kol_vo, price, summ,skidka, zarplata, datazapisi, datazakrytiya FROM [Table]" +
                    " WHERE product IS NOT NULL AND product NOT IN (N'Нет заказов') AND kasir = @kasir AND datazapisi BETWEEN @StartDate AND @EndDate ORDER BY datazapisi DESC", con);
                cmd.Parameters.AddWithValue("@kasir", comboBox18.Text);
                cmd.Parameters.AddWithValue("@StartDate", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@EndDate", dateTimePicker2.Value);
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();//создаем экземпляр класса DataTable
                SqlDataAdapter da = new SqlDataAdapter(cmd);//создаем экземпляр класса SqlDataAdapter
                dt.Clear();//чистим DataTable, если он был не пуст
                da.Fill(dt);//заполняем данными созданный DataTable
                DGVotchet.DataSource = dt;//в качестве источника данных у dataGridView используем DataTable заполненный данными
                con.Close();//Закрываем соединение
            }
            if (checkBox2.Checked == false)
            {
                con.Open();//Открываем соединение
                SqlCommand cmd = new SqlCommand("SELECT kasir, product, kol_vo, price, summ,skidka, zarplata, datazapisi, datazakrytiya FROM [Table]" +
                    " WHERE product IS NOT NULL AND product NOT IN (N'Нет заказов') AND kasir = @kasir AND datazapisi = @datazapisi ORDER BY datazapisi DESC", con);
                cmd.Parameters.AddWithValue("@kasir", comboBox18.Text);
                cmd.Parameters.AddWithValue("@datazapisi", dateTimePicker1.Value);
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();//создаем экземпляр класса DataTable
                SqlDataAdapter da = new SqlDataAdapter(cmd);//создаем экземпляр класса SqlDataAdapter
                dt.Clear();//чистим DataTable, если он был не пуст
                da.Fill(dt);//заполняем данными созданный DataTable
                DGVotchet.DataSource = dt;//в качестве источника данных у dataGridView используем DataTable заполненный данными
                con.Close();//Закрываем соединение
            }                           
            DGVotchet.Columns[0].HeaderText = "Касир";
            DGVotchet.Columns[1].HeaderText = "Наименование";
            DGVotchet.Columns[2].HeaderText = "Кол-во";
            DGVotchet.Columns[3].HeaderText = "Цена";
            DGVotchet.Columns[4].HeaderText = "Сумма";
            DGVotchet.Columns[5].HeaderText = "Скидка";
            DGVotchet.Columns[6].HeaderText = "Зарплата";
            DGVotchet.Columns[7].HeaderText = "Дата записи";
            DGVotchet.Columns[8].HeaderText = "Дата закрытия смены";

            if (DGVotchet.Rows.Count >= 1)
            {
                //сумма
                double summ = 0;
                foreach (DataGridViewRow row in DGVotchet.Rows)
                {
                    double.TryParse((row.Cells[4].Value ?? "0").ToString().Replace(".", ","), out double incom);
                    summ += incom;
                }
                label49.Text = summ.ToString();
            }
            if (DGVotchet.Rows.Count >= 1)
            {
                //сумма скидки
                double skidka = 0;
                foreach (DataGridViewRow row in DGVotchet.Rows)
                {
                    double.TryParse((row.Cells[5].Value ?? "0").ToString().Replace(".", ","), out double incom);
                    skidka += incom;
                }
                label53.Text = skidka.ToString();
            }
            //Итого к оплате - скидка - зарплата
            if (DGVotchet.Rows.Count >= 1)
            {
                //сумма ЗП
                double zarplata = 0;
                foreach (DataGridViewRow row in DGVotchet.Rows)
                {
                    double.TryParse((row.Cells[6].Value ?? "0").ToString().Replace(".", ","), out double incom);
                    zarplata += incom;
                }
                label47.Text = zarplata.ToString();
            }
        }
        private void Button21_Click(object sender, EventArgs e)//Удалить
        {
            if (MessageBox.Show("Вы действительно хотите удалить эти данные? Подтвердите действие", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                if (DGVotchet.Rows.Count >= 1)
                {
                    for (int i = 0; i < DGVotchet.Rows.Count; i++)//
                    {
                        con.Open();//открыть соединение
                        SqlCommand cmd = new SqlCommand("DELETE FROM [Table] WHERE kasir=@kasir ", con);
                        cmd.Parameters.AddWithValue("@kasir", DGVotchet.Rows[i].Cells[0].Value);
                        cmd.ExecuteNonQuery();
                        con.Close();//закрыть соединение
                    }
                    MessageBox.Show("Удалено из базы!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else MessageBox.Show("Ничего не найдено", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//Сылка на страничку
        {
            Process.Start("https://www.facebook.com/alesunix");
            linkLabel1.BackColor = Color.Transparent;
        }
        private void Button5_Click(object sender, EventArgs e)//SQL добавление столбца в таблицу базы данных
        {
            con.Open();//открыть соединение
            SqlCommand cmd = new SqlCommand("ALTER TABLE [Table] ADD chek INT NULL", con);
            cmd.ExecuteNonQuery();
            con.Close();//закрыть соединение
            MessageBox.Show("Столбец добавлен в таблицу!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Button6_Click(object sender, EventArgs e)//Закрыть программу
        {
            Application.Exit();
        }

        private void InstallUpdateSyncWithInfo()//Метод обновления приложения
        {
            UpdateCheckInfo info = null;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = ad.CheckForDetailedUpdate();

                }
                catch (DeploymentDownloadException dde)
                {
                    MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
                    return;
                }
                catch (InvalidDeploymentException ide)
                {
                    MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
                    return;
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
                    return;
                }

                if (info.UpdateAvailable)
                {
                    Boolean doUpdate = true;

                    if (!info.IsUpdateRequired)
                    {
                        DialogResult dr = MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButtons.OKCancel);
                        if (!(DialogResult.OK == dr))
                        {
                            doUpdate = false;
                        }
                    }
                    else
                    {
                        // Display a message that the app MUST reboot. Display the minimum required version.
                        MessageBox.Show("This application has detected a mandatory update from your current " +
                            "version to version " + info.MinimumRequiredVersion.ToString() +
                            ". The application will now install the update and restart.",
                            "Update Available", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }

                    if (doUpdate)
                    {
                        try
                        {
                            ad.Update();
                            MessageBox.Show("The application has been upgraded, and will now restart.");
                            Application.Restart();
                        }
                        catch (DeploymentDownloadException dde)
                        {
                            MessageBox.Show("Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde);
                            return;
                        }
                    }
                }
            }
        }
    }
}
