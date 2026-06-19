using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dommel;
using Npgsql;
using Testcontainers.PostgreSql;

namespace SME.SGP.TesteIntegracao.Setup
{
    public class InMemoryDatabase : IAsyncDisposable
    {
        public NpgsqlConnection Conexao;

        private readonly PostgreSqlContainer _postgresContainer;
        private ConstrutorDeTabelas _construtorDeTabelas;

        public InMemoryDatabase()
        {
            // Ajuste a imagem conforme sua necessidade. Ex: "postgres:15-alpine" ou "postgres:16"
            _postgresContainer = new PostgreSqlBuilder()
                .WithImage("postgres:15-alpine")
                .WithDatabase("sgp_testes")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();
        }

        public async Task InitializeAsync()
        {
            await _postgresContainer.StartAsync();

            await CriarConexaoEAbrirAsync();

            _construtorDeTabelas = new ConstrutorDeTabelas(Conexao);
            _construtorDeTabelas.Construir();
        }

        private async Task CriarConexaoEAbrirAsync()
        {
            var cs = _postgresContainer.GetConnectionString();
            Conexao = new NpgsqlConnection($"{cs};Include Error Detail=true;");
            await Conexao.OpenAsync();
        }

        public void Inserir<T>(IEnumerable<T> objetos) where T : class, new()
        {
            foreach (var objeto in objetos)
                Conexao.Insert(objeto);
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
            // Removi o filtro tableowner='Test' porque no container o owner tende a ser 'postgres'
            var builder = new StringBuilder();
            builder.Append("DO $$ DECLARE ");
            builder.Append("r RECORD; ");
            builder.Append("BEGIN ");
            builder.Append("  FOR r IN (SELECT tablename FROM pg_tables WHERE schemaname='public') LOOP ");
            builder.Append("    EXECUTE 'TRUNCATE TABLE ' || quote_ident(r.tablename) || ' RESTART IDENTITY CASCADE'; ");
            builder.Append("  END LOOP; ");
            builder.Append("END $$;");

            using var cmd = new NpgsqlCommand(builder.ToString(), Conexao);
            cmd.ExecuteNonQuery();
        }

        public void Inserir(string tabela, params string[] campos)
        {
            var builder = new StringBuilder();
            builder.Append($"Insert into {tabela} Values (");
            builder.Append(string.Join(", ", campos));
            builder.Append(")");

            using var cmd = new NpgsqlCommand(builder.ToString(), Conexao);
            cmd.ExecuteNonQuery();
        }

        public void Inserir(string tabela, string[] campos, string[] valores)
        {
            var builder = new StringBuilder();
            builder.Append($"Insert into {tabela} (");
            builder.Append(string.Join(", ", campos));
            builder.Append(") Values (");
            builder.Append(string.Join(", ", valores));
            builder.Append(")");

            using var cmd = new NpgsqlCommand(builder.ToString(), Conexao);
            cmd.ExecuteNonQuery();
        }

        public async ValueTask DisposeAsync()
        {
            if (Conexao != null)
            {
                await Conexao.CloseAsync();
                await Conexao.DisposeAsync();
            }

            await _postgresContainer.DisposeAsync(); // para e remove o container
        }
    }
}