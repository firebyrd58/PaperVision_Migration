using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace PaperVision_Migration
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\PV_ControlFiles\PV_Import.xml";
            string xmlString = @"<?xml version=""1.0"" encoding=""UTF-8""?>";

            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(xmlString);
                sw.WriteLine("<import>");
            }

            {
                //SQL Server database connection string
                string connString = @"Server=ftwsql14d001\enterprise;Database=OpenText;User Id=opentext;Password=Ot2018**";

                string query = "SELECT FirstName,LastName,MailAddress FROM OpenText.KUAF;";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    //opens the connection
                    conn.Open();

                    //executes the sql command
                    SqlDataReader reader = cmd.ExecuteReader();

                    //checks for any records returned from the query
                    while (reader.Read())
                    {
                        using (StreamWriter sw = File.AppendText(path))
                        {
                            sw.WriteLine(@"   <node action=""create"" type=""document"">");
                            sw.WriteLine("      <location>Enterprise:test:import</location>");
                            sw.WriteLine("      <file>" + (reader["FirstName"].ToString() + "</file>"));
                            sw.WriteLine("      <title>" + (reader["LastName"].ToString() + "</title>"));
                            sw.WriteLine("      <Email>" + (reader["MailAddress"].ToString() + "</Email>"));
                            sw.WriteLine("   </node>");
                        }
                    }
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine("</import>");
                    }
                    //closes the connection
                    reader.Close();
                    conn.Close();
                }
            }
        }
    }
}




