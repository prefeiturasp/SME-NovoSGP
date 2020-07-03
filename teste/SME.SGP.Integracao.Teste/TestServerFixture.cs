using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Npgsql;
using Postgres2Go;
using Redis2Go;
using SME.SGP.Api;
using SME.SGP.Aplicacao.Servicos;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;


namespace SME.SGP.Integracao.Teste
{
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer _testServerCliente;
        private readonly PostgresRunner _postgresRunner;
        private readonly ServicoTokenJwt servicoTokenJwt;
        private readonly RedisRunner _redisRunner;

        public TestServerFixture()
        {
            try
            {

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                _postgresRunner = PostgresRunner.Start(new PostgresRunnerOptions() { Port = 5434 });
                MontaBaseDados(_postgresRunner);


                _redisRunner = RedisRunner.Start();

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

                var contextoTesteIntegrado = new ContextoTesteIntegrado("");

                servicoTokenJwt = new ServicoTokenJwt(config, contextoTesteIntegrado);
            }
            catch (Exception ex)
            {
                if (_postgresRunner != null)
                    _postgresRunner.Dispose();

                if (_redisRunner != null)
                    _redisRunner.Dispose();

                throw new Exception(ex.Message);
            }
        }

        public HttpClient _clientApi { get; }

        public void Dispose()
        {
            _clientApi.Dispose();
            _testServerCliente.Dispose();
            _redisRunner.Dispose();
            _postgresRunner.Dispose();
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
        }

        private string GetContentRootPath(string projectName)
        {
            var testProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
            var relativePathToHostProject = @"../../../../" + projectName;

            return Path.GetFullPath(Path.Combine(testProjectPath, relativePathToHostProject));
        }

        private void MontaBaseDados(PostgresRunner runner)
        {
            ExecutarPreScripts();

            var scripts = ObterScripts();
            using (var conn = new NpgsqlConnection(runner.GetConnectionString()))
            {
                conn.Open();

                DirectoryInfo d = new DirectoryInfo(scripts);

                var files = d.GetFiles("*.sql").OrderBy(a => int.Parse(CleanStringOfNonDigits_V1(a.Name)));

                foreach (var file in files)
                {
                    string script = File.ReadAllText(file.FullName);

                    byte[] b = File.ReadAllBytes(file.FullName);

                    Encoding enc = null;

                    var textoComEncodeCerto = ReadFileAndGetEncoding(b, ref enc);

                    using (var cmd = new NpgsqlCommand(textoComEncodeCerto, conn))
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

        private void ExecutarPreScripts()
        {

            using (var conn = new NpgsqlConnection(_postgresRunner.GetConnectionString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("CREATE USER postgres;", conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        //throw new Exception($"Erro ao executar o script {file.FullName}. Erro: {ex.Message}");
                    }

                }
            }
        }

        public static String ReadFileAndGetEncoding(Byte[] docBytes, ref Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.GetEncoding(1252);
            Int32 len = docBytes.Length;
            // byte order mark for utf-8. Easiest way of detecting encoding.
            if (len > 3 && docBytes[0] == 0xEF && docBytes[1] == 0xBB && docBytes[2] == 0xBF)
            {
                encoding = new UTF8Encoding(true);
                // Note that even when initialising an encoding to have
                // a BOM, it does not cut it off the front of the input.
                return encoding.GetString(docBytes, 3, len - 3);
            }
            Boolean isPureAscii = true;
            Boolean isUtf8Valid = true;
            for (Int32 i = 0; i < len; ++i)
            {
                Int32 skip = TestUtf8(docBytes, i);
                if (skip == 0)
                    continue;
                if (isPureAscii)
                    isPureAscii = false;
                if (skip < 0)
                {
                    isUtf8Valid = false;
                    // if invalid utf8 is detected, there's no sense in going on.
                    break;
                }
                i += skip;
            }
            if (isPureAscii)
                encoding = new ASCIIEncoding(); // pure 7-bit ascii.
            else if (isUtf8Valid)
                encoding = new UTF8Encoding(false);
            // else, retain given encoding. This should be an 8-bit encoding like Windows-1252.
            return encoding.GetString(docBytes);
        }

        /// <summary>
        /// Tests if the bytes following the given offset are UTF-8 valid, and
        /// returns the amount of bytes to skip ahead to do the next read if it is.
        /// If the text is not UTF-8 valid it returns -1.
        /// </summary>
        /// <param name="binFile">Byte array to test</param>
        /// <param name="offset">Offset in the byte array to test.</param>
        /// <returns>The amount of bytes to skip ahead for the next read, or -1 if the byte sequence wasn't valid UTF-8</returns>
        public static Int32 TestUtf8(Byte[] binFile, Int32 offset)
        {
            // 7 bytes (so 6 added bytes) is the maximum the UTF-8 design could support,
            // but in reality it only goes up to 3, meaning the full amount is 4.
            const Int32 maxUtf8Length = 4;
            Byte current = binFile[offset];
            if ((current & 0x80) == 0)
                return 0; // valid 7-bit ascii. Added length is 0 bytes.
            Int32 len = binFile.Length;
            for (Int32 addedlength = 1; addedlength < maxUtf8Length; ++addedlength)
            {
                Int32 fullmask = 0x80;
                Int32 testmask = 0;
                // This code adds shifted bits to get the desired full mask.
                // If the full mask is [111]0 0000, then test mask will be [110]0 0000. Since this is
                // effectively always the previous step in the iteration I just store it each time.
                for (Int32 i = 0; i <= addedlength; ++i)
                {
                    testmask = fullmask;
                    fullmask += (0x80 >> (i + 1));
                }
                // figure out bit masks from level
                if ((current & fullmask) == testmask)
                {
                    if (offset + addedlength >= len)
                        return -1;
                    // Lookahead. Pattern of any following bytes is always 10xxxxxx
                    for (Int32 i = 1; i <= addedlength; ++i)
                    {
                        if ((binFile[offset + i] & 0xC0) != 0x80)
                            return -1;
                    }
                    return addedlength;
                }
            }
            // Value is greater than the maximum allowed for utf8. Deemed invalid.
            return -1;
        }

        public bool ValidarStatusCodeComSucesso(HttpResponseMessage response)
        {
            return response.IsSuccessStatusCode || (int)response.StatusCode == 601  || (int)response.StatusCode == 602;
        }

    }
}