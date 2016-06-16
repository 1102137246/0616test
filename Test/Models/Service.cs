using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Test.Models
{
    public class Service
    {
        public List<Models.Order> GetEmploee()
        {
            DataTable dataTable = new DataTable();
            string str = @"SELECT EmployeeID, FirstName + ' ' + LastName  as EmpName
                            FROM HR.Employees";
            using (SqlConnection conn = new SqlConnection(this.GetConnectString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(str, conn);
                SqlDataAdapter dad = new SqlDataAdapter(cmd);
                dad.Fill(dataTable);
                conn.Close();
            }
            return this.ConvertEmp(dataTable);
        }

        public List<Models.Order> GetShipper()
        {
            DataTable dataTable = new DataTable();
            string str = @"SELECT ShipperID, CompanyName
                            FROM Sales.Shippers";
            using (SqlConnection conn = new SqlConnection(this.GetConnectString()))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(str,conn);
                SqlDataAdapter ada = new SqlDataAdapter(comm);
                ada.Fill(dataTable);
                conn.Close();
            }
            return this.ConverShipper(dataTable);
        }

        public List<Models.Order> GetCustomer()
        {
            DataTable dataTable = new DataTable();
            string str = @"SELECT CustomerID, CompanyName
                            FROM Sales.Customers";
            using (SqlConnection conn = new SqlConnection(GetConnectString()))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(str,conn);
                SqlDataAdapter ada = new SqlDataAdapter(comm);
                ada.Fill(dataTable);
                conn.Close();
            }
            return converGetCustomer(dataTable);
        }

        public List<Models.Order> GetResult(Models.Order order)
        {
            DataTable dataTable = new DataTable();
            string str = @"SELECT A.OrderID, B.CompanyName, A.OrderDate, A.ShippedDate
                            FROM Sales.Orders AS A JOIN Sales.Customers AS B on A.CustomerID = B.CustomerID
                           WHERE (A.OrderID = @OrderID OR @OrderID = 0) AND
                                 (B.CompanyName = @CompanyName OR @CompanyName IS NULL) AND
                                 (A.EmployeeID = @EmployeeID OR @EmployeeID = 0) AND
                                 (A.ShipperID = @ShipperID OR @ShipperID = 0) AND
                                 (A.OrderDate = @OrderDate OR @OrderDate IS NULL) AND
                                 (A.ShippedDate = @ShippedDate OR @ShippedDate IS NULL) AND
                                 (A.RequiredDate = @RequiredDate OR @RequiredDate IS NULL)";
            using (SqlConnection conn = new SqlConnection(this.GetConnectString()))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(str,conn);
                comm.Parameters.Add(new SqlParameter("@OrderID", order.OrderId));
                comm.Parameters.Add(new SqlParameter("@CompanyName", order.CustName == null? (object)DBNull.Value : order.CustName));
                comm.Parameters.Add(new SqlParameter("@EmployeeID", order.EmpId));
                comm.Parameters.Add(new SqlParameter("@ShipperID", order.ShipperId));
                comm.Parameters.Add(new SqlParameter("@OrderDate", order.Orderdate == null? (object)DBNull.Value : order.Orderdate));
                comm.Parameters.Add(new SqlParameter("@ShippedDate", order.ShippedDate == null ? (object)DBNull.Value : order.ShippedDate));
                comm.Parameters.Add(new SqlParameter("@RequiredDate", order.RequireDate == null ? (object)DBNull.Value : order.RequireDate));
                SqlDataAdapter ada = new SqlDataAdapter(comm);
                ada.Fill(dataTable);
                conn.Close();
            }
            return this.converGetResult(dataTable);
        }

        public List<Models.Order> Insert(Models.Order order) {
            string str = @"INSERT INTO Sales.Orders(CustomerID, EmployeeID, OrderDate, 
							    RequiredDate, ShippedDate, ShipperID, Freight, 
							    ShipCountry, ShipCity, ShipRegion, ShipPostalCode, ShipAddress, ShipName)
                            VALUES (@CustomerID, @EmployeeID, @OrderDate, 
							    @RequiredDate, @ShippedDate, @ShipperID, @Freight, 
							    @ShipCountry, @ShipCity, @ShipRegion, @ShipPostalCode, @ShipAddress, @ShipName)
                            SELECT SCOPE_IDENTITY() OrderID";
            using (SqlConnection conn = new SqlConnection(GetConnectString())) {
                conn.Open();
                SqlCommand comm = new SqlCommand(str,conn);
                comm.Parameters.Add(new SqlParameter("@CustomerID", order.CustName));
                comm.Parameters.Add(new SqlParameter("@EmployeeID", order.EmpId));
                comm.Parameters.Add(new SqlParameter("@OrderDate", order.Orderdate));
                comm.Parameters.Add(new SqlParameter("@RequiredDate", order.RequireDate));
                comm.Parameters.Add(new SqlParameter("@ShippedDate", order.ShipAddress));
                comm.Parameters.Add(new SqlParameter("@ShipperID", order.ShipperId));
                comm.Parameters.Add(new SqlParameter("@Freight", order.Freight));
                comm.Parameters.Add(new SqlParameter("@ShipCountry", order.ShipCountry));
                comm.Parameters.Add(new SqlParameter("@ShipCity", order.ShipCity));
                comm.Parameters.Add(new SqlParameter("@ShipRegion", order.ShipRegion));
                comm.Parameters.Add(new SqlParameter("@ShipPostalCode", order.ShipPostalCode));
                comm.Parameters.Add(new SqlParameter("@ShipAddress", order.ShipAddress));
                comm.Parameters.Add(new SqlParameter("@ShipName", order.ShipName));


            }




                return;
        }

        private List<Order> converGetResult(DataTable dataTable)
        {
            List<Models.Order> list = new List<Order>();
            foreach (DataRow item in dataTable.Rows)
            {
                list.Add(new Order
                {
                    OrderId = (int)item["OrderId"],
                    CustName = item["CompanyName"].ToString(),
                    Orderdate = (DateTime)item["Orderdate"],
                    ShippedDate = item["ShippedDate"] == DBNull.Value? (DateTime?)null : (DateTime)item["ShippedDate"],
                });
            }
            return list;
        }

        private List<Order> converGetCustomer(DataTable dataTable)
        {
            List<Models.Order> list = new List<Models.Order>();
            foreach (DataRow item in dataTable.Rows)
            {
                list.Add(new Order
                {
                    CustId = item["CustomerID"].ToString(),
                    CustName = item["CompanyName"].ToString(),
                });
            }
            return list;
        }

        private List<Order> ConverShipper(DataTable dataTable)
        {
            List<Models.Order> list = new List<Order>();
            foreach (DataRow item in dataTable.Rows)
            {
                list.Add(new Order
                {
                    ShipperId = (int)item["ShipperID"],
                    ShipperName = item["CompanyName"].ToString(),
                });
            }
            return list;
        }

        private List<Order> ConvertEmp(DataTable dataTable)
        {
            List<Models.Order> list = new List<Order>();
            foreach (DataRow item in dataTable.Rows)
            {
                list.Add(new Order
                {
                    EmpId = (int)item["EmployeeID"],
                    EmpName = item["EmpName"].ToString(),
                });
            }
            return list;
        }

        public void DeleteOrderId(int OrderId)
        {
            DataTable dataTable = new DataTable();
            string str = @"DELETE
                            FROM Sales.OrderDetails
                            WHERE OrderID = @OrderID
                           DELETE 
                            FROM Sales.Orders
                            WHERE OrderID = @OrderID";
            using (SqlConnection conn = new SqlConnection(this.GetConnectString()))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(str,conn);
                comm.Parameters.Add(new SqlParameter("@OrderID", OrderId));
                comm.ExecuteNonQuery();
                conn.Close();
            }
        } 

        private string GetConnectString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString();
        }
    }
}