using Dommel;
using Npgsql;
using Postgres2Go;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.TesteIntegracao.Setup
{
    public class InMemoryDatabase : IDisposable
    {
        public NpgsqlConnection Conexao;
        private readonly PostgresRunner _postgresRunner;

        public InMemoryDatabase()
        {
            _postgresRunner = PostgresRunner.Start();
            CriarConexaoEAbrir();
            new ConstrutorDeTabelas().Construir(Conexao);
        }

        public void CriarConexaoEAbrir()
        {
            Conexao = new NpgsqlConnection(_postgresRunner.GetConnectionString());
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

        public void LimparBase()
        {
            var builder = new StringBuilder();
            builder.Append(" DO $$ DECLARE ");
            builder.Append(" r RECORD; ");
            builder.Append(" BEGIN ");
            builder.Append("     FOR r IN ( ");
            builder.Append("                select tab.tablename, seq.sequencename,");
            builder.Append("                       (select true from information_schema.columns ");
            builder.Append("                        where table_schema = 'public' and column_name = 'id' and is_identity = 'YES' and columns.table_name = tab.tablename) as is_identity");
            builder.Append("                from pg_tables tab ");
            builder.Append("                left join pg_sequences seq on REPLACE(seq.sequencename,'_id_seq', '') = tab.tablename");
            builder.Append("                where tab.tableowner = 'Test' and tab.schemaname = 'public' and (last_value > 0 or seq.sequencename is null)");
            builder.Append("                ) LOOP");
            builder.Append("     EXECUTE 'DELETE FROM ' || quote_ident(r.tablename);");
            builder.Append(" IF not r.is_identity is null THEN EXECUTE 'ALTER TABLE ' || quote_ident(r.tablename) || ' ALTER COLUMN id RESTART'; END IF;");
            builder.Append(" IF not r.sequencename is null THEN EXECUTE 'ALTER SEQUENCE ' || quote_ident(r.sequencename) || ' restart'; END IF;");
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
