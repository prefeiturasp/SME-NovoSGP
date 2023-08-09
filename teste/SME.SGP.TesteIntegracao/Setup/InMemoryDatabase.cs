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
    //mesmo localmente e em memoria a execucao de testes de integracao estao extremamente lentos (pelo menos no meu pc)
    //o ideal para os teste de integracao nesse caso era subir apenas um runner de postgres buildar o schema da base apenas uma vez
    //e cada caso de testes trabalhar apenas com abertura de transacao e rollback no final para jogar todos os dados foras

    //fazendo profile a maior parte do tempo de execucao de cada teste de integracao fica em subir o runner gerar a base e limpar toda vez
    //eu avaliaria outra estrategia de execucao de testes de integracao pois em pouco tempo essa estrategia nao escala e vai demorar cada vez mais conforme a suite de testes vao crescendo
    public class InMemoryDatabase : IDisposable
    {
        public NpgsqlConnection Conexao;
        private readonly PostgresRunner _postgresRunner;

        public InMemoryDatabase()
        {
            _postgresRunner = PostgresRunner.Start();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            CriarConexaoEAbrir();
            new ConstrutorDeTabelas().Contruir(Conexao);
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
