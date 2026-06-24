using Microsoft.Extensions.PlatformAbstractions;
using MongoDB.Driver.Core.Configuration;
using Npgsql;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SME.SGP.TesteIntegracao.Setup
{
    public class ConstrutorDeTabelas
    {
        private readonly string _connectionString;

        public ConstrutorDeTabelas(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            _connectionString = connectionString;
        }

        public void Construir()
        {
            MontaBaseDados();
        }

        public void ExecutarScripts(List<ScriptCarga> scriptsCarga = null)
        {
            var scriptsPath = ObterScripts();
            var dir = new DirectoryInfo(scriptsPath);

            var files = dir.GetFiles("*.sql").ToList();

            if (scriptsCarga.NaoEhNulo())
                files = files.FindAll(file => scriptsCarga.Exists(script => script.Name() == file.Name));

            files = files
                .OrderBy(a => int.Parse(CleanStringOfNonDigits_V1(a.Name.Replace("\uFEFF", ""))))
                .ToList();

            foreach (var file in files)
            {
                var bytes = File.ReadAllBytes(file.FullName);
                Encoding enc = null;
                var sql = ReadFileAndGetEncoding(bytes, ref enc);

                using var conexaoScript = new NpgsqlConnection(_connectionString);
                conexaoScript.Open();

                // Scripts com muitas linhas executam em lotes para evitar timeout de socket
                var linhas = sql.Count(c => c == '\n');
                var ehScriptGrande = linhas > 5000;

                try
                {
                    if (ehScriptGrande)
                        ExecutarScriptEmLotes(conexaoScript, sql, file.FullName);
                    else
                    {
                        using var cmd = new NpgsqlCommand(sql, conexaoScript) { CommandTimeout = 300 };
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) when (ex is not Exception { InnerException: null })
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao executar o script {file.FullName}. Erro: {ex.Message}", ex);
                }
            }
        }

        private void MontaBaseDados()
        {
            ExecutarPreScripts();
            ExecutarScripts();
        }

        private void ExecutarScriptEmLotes(NpgsqlConnection conexao, string sql, string caminhoArquivo, int tamanhLote = 1000)
        {
            var comandos = sql
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Trim())
                .Where(l => !string.IsNullOrWhiteSpace(l) && l != ";")
                .ToList();

            var lotes = comandos
                .Select((cmd, idx) => new { cmd, idx })
                .GroupBy(x => x.idx / tamanhLote)
                .Select(g => string.Join("\n", g.Select(x => x.cmd)))
                .ToList();

            foreach (var lote in lotes)
            {
                using var cmd = new NpgsqlCommand(lote, conexao) { CommandTimeout = 0 };
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao executar lote do script {caminhoArquivo}. Erro: {ex.Message}", ex);
                }
            }
        }

        private void ExecutarPreScripts()
        {
            using var conexao = new NpgsqlConnection(_connectionString);
            conexao.Open();

            using var cmd = new NpgsqlCommand("SET client_encoding TO 'UTF8';", conexao)
            {
                CommandTimeout = 60
            };
            cmd.ExecuteNonQuery();
        }

        private string ReadFileAndGetEncoding(byte[] docBytes, ref Encoding encoding)
        {
            if (encoding.EhNulo())
                encoding = Encoding.GetEncoding(1252);

            int len = docBytes.Length;

            if (len > 3 && docBytes[0] == 0xEF && docBytes[1] == 0xBB && docBytes[2] == 0xBF)
            {
                encoding = new UTF8Encoding(true);
                return encoding.GetString(docBytes, 3, len - 3);
            }

            bool isPureAscii = true;
            bool isUtf8Valid = true;

            for (int i = 0; i < len; ++i)
            {
                int skip = TestUtf8(docBytes, i);
                if (skip == 0) continue;

                if (isPureAscii) isPureAscii = false;

                if (skip < 0)
                {
                    isUtf8Valid = false;
                    break;
                }

                i += skip;
            }

            if (isPureAscii)
                encoding = new ASCIIEncoding();
            else if (isUtf8Valid)
                encoding = new UTF8Encoding(false);

            return encoding.GetString(docBytes);
        }

        private int TestUtf8(byte[] binFile, int offset)
        {
            const int maxUtf8Length = 4;
            byte current = binFile[offset];

            if ((current & 0x80) == 0)
                return 0;

            int len = binFile.Length;

            for (int addedlength = 1; addedlength < maxUtf8Length; ++addedlength)
            {
                int fullmask = 0x80;
                int testmask = 0;

                for (int i = 0; i <= addedlength; ++i)
                {
                    testmask = fullmask;
                    fullmask += (0x80 >> (i + 1));
                }

                if ((current & fullmask) == testmask)
                {
                    if (offset + addedlength >= len)
                        return -1;

                    for (int i = 1; i <= addedlength; ++i)
                    {
                        if ((binFile[offset + i] & 0xC0) != 0x80)
                            return -1;
                    }

                    return addedlength;
                }
            }

            return -1;
        }

        private string CleanStringOfNonDigits_V1(string s)
        {
            try
            {
                s = s.ToUpper().Replace("V", "");
                var clearStr = s.Split("__");
                return clearStr[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private string ObterScripts()
        {
            var testProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
            var relativePathToHostProject = @"../../../../../scripts";
            return Path.GetFullPath(Path.Combine(testProjectPath, relativePathToHostProject));
        }
    }
}