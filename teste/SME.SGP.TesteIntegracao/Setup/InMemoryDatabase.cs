using Dommel;
using Npgsql;
using Postgres2Go;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Setup
{
    public class InMemoryDatabase : IDisposable
    {
        public NpgsqlConnection Conexao;
        private readonly PostgresRunner _postgresRunner;
        private readonly ConstrutorDeTabelas _construtorDeTabelas;

        public InMemoryDatabase()
        {
            _postgresRunner = PostgresRunner.Start();
            CriarConexaoEAbrir();
            _construtorDeTabelas = new ConstrutorDeTabelas(Conexao);
            _construtorDeTabelas.Construir();
        }

        public void CriarConexaoEAbrir()
        {
            Conexao = new NpgsqlConnection(string.Concat(_postgresRunner.GetConnectionString(), ";Include Error Detail=true;"));
            Conexao.Open();
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

        public async Task<long> InserirAsync<T>(T objeto) where T : class, new()
        {
            return (long)(await Conexao.InsertAsync(objeto));
        }

        public void Atualizar<T>(T objeto) where T : class, new()
        {
            Conexao.Update(objeto);
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

        public void ExecutarScripts(List<ScriptCarga> scriptsCarga)
        {
            _construtorDeTabelas.ExecutarScripts(scriptsCarga);
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

        public void Inserir(string tabela, params string[] campos)
        {
            var builder = new StringBuilder();
            builder.Append($"Insert into {tabela} Values (");
            builder.Append(string.Join(", ", campos));
            builder.Append(")");

            using (var cmd = new NpgsqlCommand(builder.ToString(), Conexao))
            {
                cmd.ExecuteNonQuery();
            }
        }
        
        public void Inserir(string tabela, string[] campos, string[] valores)
        {
            var builder = new StringBuilder();
            builder.Append($"Insert into {tabela} (");
            builder.Append(string.Join(", ", campos));
            builder.Append(@") Values (");
            builder.Append(string.Join(", ", valores));
            builder.Append(")");

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
