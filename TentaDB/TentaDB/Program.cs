using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TentaDB
{
    class Program
    {
        static string cns = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NORTHWND;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        static void Main(string[] args)
        {
            //Fråga 1
            //ProdutsByCategoryName("Confections");

            //Fråga 2
            //SalesByTerritory();

            //Fråga 3
            //EmployeesPerRegion();

            //Fråga 4 (inte korrekt, fastnade)
            //OrdersPerEmployee();

            //Fråga 5
            //CustomersWithNamesLongerThan25Characters();


        }

        private static void CustomersWithNamesLongerThan25Characters()
        {
            using (var db = new Model1())
            {
                var query = (from c in db.Customers
                             where c.CompanyName.Length > 25
                             select c);

                foreach (var item in query)
                {
                    Console.WriteLine(item.CompanyName);
                }
                Console.ReadLine();
            }
            }

        private static void OrdersPerEmployee()
        {
            //using (var db = new Model1())
            //{
            //    var query = (from e in db.Employees
            //                 join o in db.Orders on e.EmployeeID equals o.EmployeeID
            //                 join od in db.Order_Details on o.OrderID equals od.OrderID
            //                 //group e by new { FirstName = e.FirstName, LastName = e.LastName, OrderID = od.OrderID } into q
            //                 select new { e.FirstName, e.LastName,  m = (from e in db.Employees
            //                                                             join o in db.Orders on e.EmployeeID equals o.EmployeeID
            //                                                             join oe in db.Order_Details on o.OrderID equals od.OrderID
            //                                                             select oe.OrderID).Count() }).Distinct();

            //    foreach (var item in query)
            //    {
            //        Console.WriteLine(item.FirstName+ " " + item.LastName + " " + item.m);
            //    }
            //    Console.ReadLine();


            //    //SELECT Employees.FirstName, Employees.LastName, COUNT([Order Details].Quantity) as Orders
            //    //FROM Employees INNER JOIN
            //    //Orders ON Employees.EmployeeID = Orders.EmployeeID INNER JOIN
            //    //[Order Details] ON Orders.OrderID = [Order Details].OrderID
            //    //GROUP BY Employees.FirstName, Employees.LastName
            //    //ORDER BY FirstName ASC
            }
            }

        private static void EmployeesPerRegion()
        {
            SqlConnection cn = new SqlConnection(cns);
            cn.Open();
            SqlCommand cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT Region.RegionDescription, COUNT(DISTINCT Employees.EmployeeID) AS Employees" +
                              " FROM Employees INNER JOIN" +
                              " EmployeeTerritories ON Employees.EmployeeID = EmployeeTerritories.EmployeeID INNER JOIN" +
                              " Territories ON EmployeeTerritories.TerritoryID = Territories.TerritoryID INNER JOIN" +
                              " Region ON Territories.RegionID = Region.RegionID" +
                              " GROUP BY Region.RegionDescription";
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                Console.WriteLine("Region: " + rd.GetString(0));
                Console.WriteLine("Employees: " + rd.GetInt32(1) + "\n");
            }
            rd.Close();
            cn.Close();
            Console.ReadLine();
        }

        private static void SalesByTerritory()
        {
            SqlConnection cn = new SqlConnection(cns);
            cn.Open();
            SqlCommand cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT TOP 5 Territories.TerritoryDescription, SUM([Order Details].Quantity * [Order Details].UnitPrice) AS TotalSales" +
                              " FROM Employees INNER JOIN" +
                              " EmployeeTerritories ON Employees.EmployeeID = EmployeeTerritories.EmployeeID INNER JOIN" +
                              " Orders ON Employees.EmployeeID = Orders.EmployeeID INNER JOIN" +
                              " [Order Details] ON Orders.OrderID = [Order Details].OrderID INNER JOIN" +
                              " Territories ON EmployeeTerritories.TerritoryID = Territories.TerritoryID" +
                              " GROUP BY Territories.TerritoryDescription" +
                              " ORDER BY TotalSales DESC";
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                Console.WriteLine("Territory: " + rd.GetString(0));
                Console.WriteLine("Total sales: " + rd.GetDecimal(1) + "\n");

            }
            rd.Close();
            cn.Close();
            Console.ReadLine();
        }

        private static void ProdutsByCategoryName(string CategoryName)
        {
            SqlConnection cn = new SqlConnection(cns);
            cn.Open();
            SqlCommand cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT Products.ProductName, Products.UnitPrice, Products.UnitsInStock FROM Products INNER JOIN Categories ON Products.CategoryID = Categories.CategoryID"
                + " WHERE Categories.CategoryName = @CategoryName";
            cmd.Parameters.AddWithValue("@CategoryName", CategoryName);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                Console.WriteLine("Product name: " + rd.GetString(0));
                Console.WriteLine("Unit price: " + rd.GetDecimal(1));
                Console.WriteLine("Units in stock: " + rd.GetInt16(2) + "\n");
            }
            rd.Close();
            cn.Close();
            Console.ReadLine();
        }
    }
}
