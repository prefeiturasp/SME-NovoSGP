using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dommel;
using Npgsql;
using Postgres2Go;
using SME.SGP.Dominio;

namespace SME.SGP.TesteIntegracao.Setup
{
    public class InMemoryDatabase : IDisposable
    {
        public readonly NpgsqlConnection Conexao;
        private readonly PostgresRunner _postgresRunner;

        public InMemoryDatabase()
        {
            _postgresRunner = PostgresRunner.Start();
            Conexao = new NpgsqlConnection(_postgresRunner.GetConnectionString());
            Conexao.Open();
            new ConstrutorDeTabelas().Contruir(Conexao);
        }

        public void Inserir<T>(IEnumerable<T> objetos) where T : class, new()
        {
            foreach (var objeto in objetos)
            {
                Conexao.Insert(objeto);
            }
        }

        public void Inserir<T>(T objeto) where T : class, new()
        {
            Conexao.Insert(objeto);
        }

        public List<T> ObterTodos<T>() where T : class, new()
        {
            return Conexao.GetAll<T>().ToList();
        }

        public T ObterPorId<T, K>(K id)
            where T : class, new()
            where K : struct
        {
            return Conexao.Get<T>(id);
        }

        public void LimparBase()
        {
            var builder = new StringBuilder();
            builder.Append(" DO $$ DECLARE ");
            builder.Append(" r RECORD; ");
            builder.Append(" BEGIN ");
            builder.Append("     FOR r IN (SELECT tablename FROM pg_tables WHERE tableowner = 'Test' and schemaname='public') LOOP ");
            builder.Append("     EXECUTE 'TRUNCATE TABLE ' || quote_ident(r.tablename) || ' RESTART IDENTITY CASCADE '; ");
            builder.Append(" END LOOP; ");
            builder.Append(" END $$; ");

            using (var cmd = new NpgsqlCommand(builder.ToString(), Conexao))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            Conexao.Close();
            Conexao.Dispose();
            _postgresRunner.Dispose();
        }
    }
}
