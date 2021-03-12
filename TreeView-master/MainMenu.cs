using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewTree
{
    public partial class MainMenu : Form
    {

        SqlConnection connection = new SqlConnection(); // Строка подключения
        TreeNode node_here = new TreeNode();            // ВЫбранная нода TreeView
        List<string> ItemsName = new List<string>();    // Коллекция имен предметов
        List<string> Types = new List<string>();        // Коллекция названий типов предметов
        List<string> Equipments = new List<string>();   // Коллекция названий видов снаряжения

        int indexName;                                  // Позиция выбранного предмета            

        // Конструктор класса
        public MainMenu()
        {
            InitializeComponent();
        }

        // Запуск приложения
        #region AppLoad
        
            // Обработчик главного окна при его загрузке
            private void MainMenu_Load(object sender, EventArgs e)
            {
                DBConnection();
                comboBox_ItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                comboBox_AttributeName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            }

            // Подключение к БД
            public void DBConnection()
        {
            connection.ConnectionString = @"Data Source=localhost;Initial Catalog=HSE;Integrated Security=True";
            connection.Open();
        }

        #endregion

        // Показать TreeView
        #region ShowTreeView

        // Получить виды снаряжения для вершин TreeView 
        public void GetFaculties()
            {
                SqlCommand command = new SqlCommand("SELECT * FROM faculties", connection);
                try
                {
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        TreeNode node = new TreeNode(dataReader["name"].ToString().Trim());
                        treeView1.Nodes.Add(node);
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Получить типы предметов для вершин TreeView 
            public void GetDirections()
            {
                SqlCommand command = new SqlCommand("SELECT * FROM directions", connection);
                try
                {
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Types.Add(dataReader["name"].ToString().Trim());
                        TreeNode node = new TreeNode(dataReader["name"].ToString().Trim());
                        treeView1.Nodes[Convert.ToInt32(dataReader["faculty"]) - 1].Nodes.Add(node);
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Получить предметы для вершин TreeView 
            public void GetCourses()
            {
                SqlCommand command = new SqlCommand("SELECT * FROM courses ORDER BY id ASC", connection);
                try
                {
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {                      

                        ItemsName.Add(dataReader["name"].ToString().Trim());
                        TreeNode node_item = new TreeNode(dataReader["name"].ToString().Trim());

                      

                        

                        if(Convert.ToInt32(dataReader["direction"]) <= 4)
                        {
                            treeView1.Nodes[0].Nodes[Convert.ToInt32(dataReader["direction"]) - 1].Nodes.Add(node_item);
                        }                     
                        else
                        {
                            treeView1.Nodes[1].Nodes[Convert.ToInt32(dataReader["direction"]) - 5].Nodes.Add(node_item);
                        }
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Загрузить БД в TreeView
            public void DBtoTreeView()
            {
                treeView1.Nodes.Clear();
                GetFaculties();
                GetDirections();
                GetCourses();
            }

            // Оброботчик событый для кнопки ЗАГРУЗИТЬ БАЗУ ДАННЫХ
            private void button_GetItems_Click(object sender, EventArgs e)
            {
                DBtoTreeView();
                button_GetItems.Enabled = false;
            }

        #endregion
    

    }
}