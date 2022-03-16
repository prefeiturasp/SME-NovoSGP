using System;
using System.Collections.Generic;
using System.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;

namespace SME.SGP.TesteIntegracao
{
    public class InMemoryDatabase : IDisposable
    {
        private readonly OrmLiteConnectionFactory _dbFactory;

        public IDbConnection OpenConnection() => _dbFactory.OpenDbConnection();
        public readonly IDbConnection Conexao;

        public InMemoryDatabase()
        {
            _dbFactory = new OrmLiteConnectionFactory(":memory:", SqliteOrmLiteDialectProvider.Instance);
            Conexao = _dbFactory.OpenDbConnection();
        }

        public void Inserir<T>(IEnumerable<T> items)
        {
            Conexao.CreateTableIfNotExists<T>();
            foreach (var item in items)
            {
                Conexao.Insert(item);
            }
        }

        public void Dispose()
        {
            Conexao.Close();
        }
    }
}
