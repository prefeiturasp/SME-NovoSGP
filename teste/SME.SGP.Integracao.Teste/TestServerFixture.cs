using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Npgsql;
using Postgres2Go;
using SME.SGP.Api;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace SME.SGP.Integracao.Teste
{
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer _testServerCliente;
        private readonly PostgresRunner runner;
        private Regex rxNonDigits = new Regex(@"[^\d]+");
        public TestServerFixture()
        {
            try
            {
                runner = PostgresRunner.Start(new PostgresRunnerOptions() { Port = 5432 });
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

        public string ObterScripts()
        {
            var testProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
            var relativePathToHostProject = @"../../../../../scripts";

            return Path.GetFullPath(Path.Combine(testProjectPath, relativePathToHostProject));
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

                var files = d.GetFiles("*.sql").OrderBy( a => int.Parse(CleanStringOfNonDigits_V1(a.Name)));

                //var files1 = d.GetFiles("*.sql")
                //    .Select(a => CleanStringOfNonDigits_V1(a.Name));

                //var file1 = d.GetFiles("*.sql").FirstOrDefault();
                //var file12 = file1.Name.Remove(0, 1).Remove(file1.Name.IndexOf("__") - 1, file1.Name.Length - 3);


                foreach (var file in files)
                {
                    string script = File.ReadAllText(file.FullName);

                    using (var cmd = new NpgsqlCommand(script, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        private string CleanStringOfNonDigits_V1(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            string cleaned = rxNonDigits.Replace(s, "");
            return cleaned;
        }
        //public void Dispose()
        //{
        //    _clientApi.Dispose();
        //    _testServerCliente.Dispose();
        //}
        }
}