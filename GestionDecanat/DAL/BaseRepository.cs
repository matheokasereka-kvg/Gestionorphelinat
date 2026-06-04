using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GestionDecanat.DAL
{
    public abstract class BaseRepository
    {
        protected abstract string TableName { get; }
        protected abstract string IdColumn { get; }
        protected abstract string[] EditableColumns { get; }
        protected virtual string SelectQuery { get { return "SELECT * FROM " + TableName + " ORDER BY " + IdColumn + " DESC"; } }

        public virtual DataTable GetAll()
        {
            return ExecuteTable(SelectQuery);
        }

        public virtual DataTable Search(string keyword, params string[] columns)
        {
            if (string.IsNullOrWhiteSpace(keyword) || columns == null || columns.Length == 0) return GetAll();
            string where = string.Join(" OR ", columns.Select(c => c + " LIKE @keyword"));
            using (SqlConnection cn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM " + TableName + " WHERE " + where + " ORDER BY " + IdColumn + " DESC", cn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                DataTable table = new DataTable();
                da.Fill(table);
                return table;
            }
        }

        public virtual void Insert(params SqlParameter[] parameters)
        {
            string cols = string.Join(", ", EditableColumns);
            string vals = string.Join(", ", EditableColumns.Select(c => "@" + c));
            ExecuteNonQuery("INSERT INTO " + TableName + " (" + cols + ") VALUES (" + vals + ")", parameters);
        }

        public virtual void Update(int id, params SqlParameter[] parameters)
        {
            string sets = string.Join(", ", EditableColumns.Select(c => c + "=@" + c));
            SqlParameter[] all = parameters.Concat(new[] { new SqlParameter("@id", id) }).ToArray();
            ExecuteNonQuery("UPDATE " + TableName + " SET " + sets + " WHERE " + IdColumn + "=@id", all);
        }

        public virtual void Delete(int id)
        {
            ExecuteNonQuery("DELETE FROM " + TableName + " WHERE " + IdColumn + "=@id", new SqlParameter("@id", id));
        }

        protected DataTable ExecuteTable(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection cn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                DataTable table = new DataTable();
                da.Fill(table);
                return table;
            }
        }

        protected int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection cn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        protected object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection cn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                cn.Open();
                return cmd.ExecuteScalar();
            }
        }
    }
}
