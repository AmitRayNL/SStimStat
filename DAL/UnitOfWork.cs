using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MySql.Data.MySqlClient;

namespace SStimStat.DAL
{
    public class UnitOfWork: IDisposable
    {

        /// <summary>
        /// Used to execute RAW SQL commands while at the same time avoiding SQL injection attacks by 
        /// parameterizing the command parameters passed through Filter inputs.
        /// </summary>
        public class SqlCommandWrapper
        {
            public string SqlQueryString { get; set; }
            public List<MySqlParameter> ListSqlParameter { get; set; }

            public SqlCommandWrapper(string queryString)
            {
                SqlQueryString = queryString;
                ListSqlParameter = new List<MySqlParameter>();
            }

            public SqlCommandWrapper(string queryString, List<MySqlParameter> sqlParameters)
            {
                SqlQueryString = queryString;
                ListSqlParameter = sqlParameters;
            }

            public void AddSqlParameter(string parameterName, string value)
            {
                MySqlParameter sqlParameter = new MySqlParameter(parameterName, value);
                ListSqlParameter.Add(sqlParameter);
            }

            public void AddSqlParameter(MySqlParameter sqlParameter)
            {
                ListSqlParameter.Add(sqlParameter);
            }
        }

        private readonly raysstim_statsEntities _context = new raysstim_statsEntities();
        private GenericRepository<user_profile> _userProfileRepository;
        private GenericRepository<z_distribution> _zRepository;
        private GenericRepository<t_distribution> _tRepository;
        private GenericRepository<f_alpha_0_025> _f_alpha_0_025Repository;
        private GenericRepository<f_alpha_0_05> _f_alpha_0_05Repository;

        private List<SqlCommandWrapper> listUpdateCommands = new List<SqlCommandWrapper>();

        public void UpdateWithSqlQuery(SqlCommandWrapper updateSqlQuery)
        {
            listUpdateCommands.Add(updateSqlQuery);
        }

        public GenericRepository<user_profile> UserProfileRepository
        {
            get
            {
                if (this._userProfileRepository == null)
                {
                    this._userProfileRepository = new GenericRepository<user_profile>(_context);
                }
                return _userProfileRepository;
            }
        }

        public GenericRepository<z_distribution> ZRepository
        {
            get
            {
                if (this._zRepository == null)
                {
                    this._zRepository = new GenericRepository<z_distribution>(_context);
                }
                return _zRepository;
            }
        }

        public GenericRepository<t_distribution> TRepository
        {
            get
            {
                if (this._tRepository == null)
                {
                    this._tRepository = new GenericRepository<t_distribution>(_context);
                }
                return _tRepository;
            }
        }

        public GenericRepository<f_alpha_0_025> F_alpha_0_025Repository
        {
            get
            {
                if (this._f_alpha_0_025Repository == null)
                {
                    this._f_alpha_0_025Repository = new GenericRepository<f_alpha_0_025>(_context);
                }
                return _f_alpha_0_025Repository;
            }
        }


        public GenericRepository<f_alpha_0_05> F_alpha_0_05Repository
        {
            get
            {
                if (this._f_alpha_0_05Repository == null)
                {
                    this._f_alpha_0_05Repository = new GenericRepository<f_alpha_0_05>(_context);
                }
                return _f_alpha_0_05Repository;
            }
        }

        public UnitOfWork()
        {
        }
        public async Task SaveAsync()
        {
            try
            {

                if (listUpdateCommands.Count > 0)
                {
                    foreach (SqlCommandWrapper sqlCommandWrapper in listUpdateCommands)
                    {
                        await _context.Database.ExecuteSqlCommandAsync(sqlCommandWrapper.SqlQueryString, sqlCommandWrapper.ListSqlParameter.ToArray());
                    }

                    listUpdateCommands.Clear();
                }

                await _context.SaveChangesAsync();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dex)
            {
                //Logger logger = Logger.GetInstance();
                //String error = dex.Message; // log error
                //logger.WriteLog(error);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException dbUpdateEx)
            {
                //Logger logger = Logger.GetInstance();
                //String error = dbUpdateEx.Message; // log error
                //logger.WriteLog(error);
            }

        }


        #region IDispose Pattern
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}