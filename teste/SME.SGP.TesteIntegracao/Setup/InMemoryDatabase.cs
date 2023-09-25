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
            builder.Append("     FOR r IN ( with tab_pk as (");
            builder.Append("                select tab.tablename");
            builder.Append("                from pg_tables tab ");
            builder.Append("                left join pg_sequences seq on REPLACE(seq.sequencename,'_id_seq', '') = tab.tablename");
            builder.Append("                where tab.tableowner = 'Test' and tab.schemaname = 'public' and (last_value > 0 or seq.sequencename is null))");
            builder.Append("                select tab_pk.tablename from tab_pk");
            builder.Append("                union");
            builder.Append("                select const.confrelid::regclass::text AS tablename from tab_pk");
            builder.Append("                inner join pg_constraint const on const.contype = 'f' and const.conrelid = tab_pk.tablename::regclass");
            builder.Append("                ) LOOP");
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
