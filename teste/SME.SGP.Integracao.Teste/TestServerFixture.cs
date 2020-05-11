using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Moq;
using Npgsql;
using Postgres2Go;
using SME.SGP.Api;
using SME.SGP.Aplicacao.Servicos;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Infra;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace SME.SGP.Integracao.Teste
{
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer _testServerCliente;
        private readonly PostgresRunner runner;
        private readonly ServicoTokenJwt servicoTokenJwt;

        public TestServerFixture()
        {
            try
            {
                runner = PostgresRunner.Start(new PostgresRunnerOptions() { Port = 5434 });
                MontaBaseDados(runner);

                var projectPath = GetContentRootPath("../src/SME.SGP.Api");

                var builderCliente = new WebHostBuilder()
                        .UseContentRoot(projectPath)
                        .UseEnvironment("teste-integrado")
                        .UseConfiguration(new ConfigurationBuilder()
                        .SetBasePath(projectPath)
                        .AddJsonFile("appsettings.teste-integrado.json")
                        .Build())
                        .UseStartup<Startup>();

                _testServerCliente = new TestServer(builderCliente);

                _clientApi = _testServerCliente.CreateClient();

                var config = new ConfigurationBuilder()
                    .AddJsonFile(ObterArquivoConfiguracao(), optional: false)
                    .Build();

                //TODO: INJETAR UM REPOSITORIO DE CACHE E HTTPCONTEXT
                var repositorioCache = new Mock<RepositorioCache>();
                servicoTokenJwt = new ServicoTokenJwt(config, null, repositorioCache.Object);
            }
            catch (Exception ex)
            {
                if (runner != null)
                    runner.Dispose();
                throw new Exception(ex.Message);
            }
        }

        public HttpClient _clientApi { get; }

        public void Dispose()
        {
            _clientApi.Dispose();
            _testServerCliente.Dispose();
            runner.Dispose();
        }

        public string GerarToken(Permissao[] permissoes, string login = "teste", string nomeLogin = "teste", string codigoRf = "123", string guidPerfil = "")
        {
            if (string.IsNullOrEmpty(guidPerfil))
                guidPerfil = Guid.NewGuid().ToString();

            string token = servicoTokenJwt.GerarToken(login, nomeLogin, codigoRf, Guid.Parse(guidPerfil), permissoes);

            return token;
        }

        public string ObterArquivoConfiguracao()
        {
            var testProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
            var relativePathToHostProject = @"../../../../../src/SME.SGP.Api/appsettings.teste-integrado.json";

            return Path.GetFullPath(Path.Combine(testProjectPath, relativePathToHostProject));
        }

        public string ObterScripts()
        {
            var testProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
            var relativePathToHostProject = @"../../../../../scripts";

            return Path.GetFullPath(Path.Combine(testProjectPath, relativePathToHostProject));
        }

        private string CleanStringOfNonDigits_V1(string s)
        {

            s = s.ToUpper().Replace("V", "");
            var clearStr = s.Split("__");
            return clearStr[0];

            //Regex rxNonDigits = new Regex(@"[^\d]+");

            //if (string.IsNullOrEmpty(s))
            //    return s;

            //return rxNonDigits.Replace(s, "");
        }

        private string GetContentRootPath(string projectName)
        {
            var testProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
            var relativePathToHostProject = @"../../../../" + projectName;

            return Path.GetFullPath(Path.Combine(testProjectPath, relativePathToHostProject));
        }

        private void MontaBaseDados(PostgresRunner runner)
        {
            var scripts = ObterScripts();
            using (var conn = new NpgsqlConnection(runner.GetConnectionString()))
            {
                conn.Open();

                DirectoryInfo d = new DirectoryInfo(scripts);

                var files = d.GetFiles("*.sql").OrderBy(a => int.Parse(CleanStringOfNonDigits_V1(a.Name)));

                foreach (var file in files)
                {
                    string script = File.ReadAllText(file.FullName);

                    script = RemoveDiacritics(script);

                    using (var cmd = new NpgsqlCommand(script, conn))
                    {
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Erro ao executar o script {file.FullName}. Erro: {ex.Message}");
                        }
                        
                    }
                }
            }
        }
        static string RemoveDiacritics(string text)
        {
            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(text);
            return  System.Text.Encoding.UTF8.GetString(tempBytes);
        }
    }
}