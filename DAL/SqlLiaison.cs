using Dictionary.Enums;
using Dictionary.Models.Data;
using Microsoft.Data.SqlClient;

namespace DAL
{
    /// <summary>
    /// This class manages all SQL connection & raw data manipulation
    /// </summary>
    public static class SqlLiaison
    {
        /// <summary>
        /// Connects to database, execute stored procedure for the selected page then transform raw datas in business objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="view"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataBase"></param>
        /// <param name="exception"></param>
        /// <param name="reference"></param>
        /// <param name="libelle"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetData<T>(ViewsEnum view, string dataSource, string dataBase, string dbUserName, string password, out Exception? exception, string reference = "", string libelle = "") where T : BaseModel
        {
            // SQL connection

            //var ConnectionString = string.Format("Data Source={0};Database={1};Integrated Security=SSPI;", dataSource, dataBase); To use if AD Link works
            var ConnectionString = string.Format(@"Data Source={0};Database={1};User ID={2};Password={3}", dataSource, dataBase, dbUserName, password);
            try
            {
                using SqlConnection conn = new(ConnectionString);
                var SqlString = "exec {0} @Utilisateur='{1}'";

                var userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToUpper();

                if (userName.IndexOf(@"\") != -1) userName = userName.Substring(userName.IndexOf(@"\") + 1);
                var toReturn = new List<T>();
                SqlCommand command = new();
                var sqlView = "";

                // Stored procedure selection
                switch (view)
                {
                    case ViewsEnum.Reception:
                        sqlView = "";
                        break;
                    case ViewsEnum.Parts:
                        sqlView = "";
                        if (!string.IsNullOrWhiteSpace(reference)) SqlString += ", @ = '" + reference + "'";
                        if (!string.IsNullOrWhiteSpace(libelle)) SqlString += ", @ = '" + libelle + "'";
                        break;
                }
                command = new SqlCommand(string.Format(SqlString, sqlView, userName), conn);
                // Execution
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    exception = ex;
                    return Enumerable.Empty<T>();
                }

                var reader = command.ExecuteReader();

                // Data transformation
                try
                {
                    while (reader.Read())
                    {
                        // to convert to BO
                        switch (view)
                        {
                            case ViewsEnum.Reception:
                                _ = int.TryParse(reader[""].ToString(), out var recepQty);
                                toReturn.Add(new ReceptionModel()
                                {
                                    Reference = reader[""].ToString().Replace("\"", string.Empty),
                                    Libelle = reader[""].ToString().Replace("\"", string.Empty),
                                    ExceedLimit = recepQty > 100,
                                    NumericProperty = recepQty > 100 ? 0 : recepQty,
                                    Order = new ReceptionModel.ReceptionOrderModel(reader[""].ToString(), reader[""].ToString()),

                                } as T);
                                break;
                            case ViewsEnum.Parts:
                                toReturn.Add(new PartsModel()
                                {
                                    Reference = reader[""].ToString().Replace("\"", string.Empty),
                                    Libelle = reader[""].ToString().Replace("\"", string.Empty),
                                    Emplacement = reader[""].ToString().Replace("\"", string.Empty),
                                    Search = new PartsModel.SearchModel(libelle, reference),
                                    StoreCode = reader[""].ToString().Replace("\"", string.Empty),
                                } as T);
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    exception = ex;
                    return Enumerable.Empty<T>();
                }

                finally
                {
                    reader.Close();
                }
                exception = null;
                return toReturn;
            }
            catch(Exception ex)
            {
                exception = ex;
                return Enumerable.Empty<T>();
            }
        }
    }
}