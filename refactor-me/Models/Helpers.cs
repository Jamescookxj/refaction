using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Collections.Generic;

namespace refactor_me.Models
{
    public class Helpers
    {
        private const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DataDirectory}\Database.mdf;Integrated Security=True";

        public static SqlConnection NewConnection()
        {
            var connstr = ConnectionString.Replace("{DataDirectory}", HttpContext.Current.Server.MapPath("~/App_Data"));
            return new SqlConnection(connstr);
        }
    }

    // Data Access Layer Class
    public class DAL
    {
        private const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DataDirectory}\Database.mdf;Integrated Security=True";
        public static SqlConnection NewConnection()
        {
            var connstr = ConnectionString.Replace("{DataDirectory}", HttpContext.Current.Server.MapPath("~/App_Data"));
            return new SqlConnection(connstr);
        }

        // Return all records from an identified table
        public SqlDataReader QueryAllFromAtable(string tablename)
        {
            var conn = DAL.NewConnection();
            var cmd = new SqlCommand($"select * from " + tablename, conn);
            conn.Open();
            return cmd.ExecuteReader();
        }

        // General query to gain data from a identified table by a field equal-condition 
        public SqlDataReader QueryAtableByAFieldValue(string tablename,string fieldname,string fieldvalue)
        {            
            var conn = DAL.NewConnection();      
            var cmd = new SqlCommand($"select * from "+ tablename+" where lower("+ fieldname +")=@"+ fieldname, conn);
            conn.Open();

            if (fieldvalue != null)
            {
                cmd.Parameters.Add(new SqlParameter("@" + fieldname, fieldvalue.ToLower().Trim()));                         
            }         
            return cmd.ExecuteReader();          
        }

        // General query to gain data from a identified table by a ID value  
        public SqlDataReader QueryTableByID(string tablename, Guid idvalue)
        {
            var conn = DAL.NewConnection();
            var cmd = new SqlCommand($"select * from " + tablename + " where id=@id", conn);
            conn.Open();

            if (idvalue != null)
            {
                cmd.Parameters.Add(new SqlParameter("@id", idvalue));
            }
            return cmd.ExecuteReader();
        }

        // General method to Insert a record to a table  
        public int InsertARecord(string tablename, string fieldstr,string valuestr)
        {
            var conn = DAL.NewConnection(); int rec = 0;
            var cmd = new SqlCommand($"insert into " + tablename + " ("+ fieldstr+")values("+ valuestr + ")", conn);
            conn.Open();
            if (valuestr != null && fieldstr != null)
            {
                rec = cmd.ExecuteNonQuery();
            }
            return rec;
        }

        // General method to Update a record of a table by id 
        public int UpdateARecord(string tablename, string id, Dictionary<string, string> updatedic)
        {
            string updatestr = ""; int rec = 0;
            if (updatedic.Count > 0)
            {             
                foreach (var item in updatedic)
                {
                    updatestr = updatestr + item.Key + "='" + item.Value.ToString() + "',";
                }
                updatestr = updatestr.Substring(0, updatestr.Length-1);
                var conn = DAL.NewConnection(); 
                var cmd = new SqlCommand($"update " + tablename + " set " + updatestr + " where id='" + id+"'", conn);
                conn.Open();
                if (id != null && updatestr != null)
                {
                    rec = cmd.ExecuteNonQuery();
                }
            }
            return rec;           
        }

        // Delete some records which match some conditions. 
        public int DeleteRecords(string tablename, Dictionary<string, string> conditiondic)
        {
            string querycondition = "";
            foreach (var item in conditiondic)
            {
                querycondition = querycondition + item.Key + item.Value.ToString() + " and ";
            }
            querycondition = querycondition.Substring(0, querycondition.Length - 4);
            string updatestr = ""; int rec = 0;
            var conn = DAL.NewConnection();
            var cmd = new SqlCommand($"delete from "+ tablename+" where "+ querycondition, conn);
            conn.Open();
            if (querycondition != null && querycondition.Length > 0)
            {
                rec = cmd.ExecuteNonQuery();
            }
            return rec;
        }

        // Delete a record by specified table name and record id. 
        public int DeleteARecord(string tablename, string id )
        {
            string updatestr = ""; int rec = 0;            
            var conn = DAL.NewConnection();
            var cmd = new SqlCommand($"delete from " + tablename + " where id='" + id + "'", conn);
            conn.Open();
            if (id != null && id.Length>0)
            {
                rec = cmd.ExecuteNonQuery();
            }         
            return rec;
        }

        // General query method to query records by a group of conditions 
        public SqlDataReader QueryRecordsByCondition(string tablename, string id, Dictionary<string, string> conditiondic)
        {
            string querycondition="";
            foreach (var item in conditiondic)
            {
                querycondition = querycondition+ item.Key + item.Value.ToString() + " and ";
            }
            querycondition = querycondition.Substring(0, querycondition.Length - 4);
            var conn = DAL.NewConnection();
            var cmd = new SqlCommand($"select * from " + tablename + " where " + querycondition  , conn);
            conn.Open();
            if (id != null && querycondition != null)
            {
                return cmd.ExecuteReader();
            }
            else  return null;
        }
    }
}