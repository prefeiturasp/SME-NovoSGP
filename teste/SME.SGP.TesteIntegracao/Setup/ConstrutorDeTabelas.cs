using Microsoft.Extensions.PlatformAbstractions;
using Npgsql;
using SME.SGP.Dominio;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace SME.SGP.TesteIntegracao.Setup
{
    public class ConstrutorDeTabelas
    {
        public void Construir(NpgsqlConnection connection)
        {
            MontaBaseDados(connection);
            RemoverTodasForeingKey(connection);
            RemoverSequencesNaoUsadas(connection);
        }

        private void MontaBaseDados(NpgsqlConnection connection)
        {
            ExecutarPreScripts(connection);

            var scripts = ObterScripts();
            DirectoryInfo d = new DirectoryInfo(scripts);

            var files = d.GetFiles("*.sql").OrderBy(a => int.Parse(CleanStringOfNonDigits_V1(a.Name.Replace("\uFEFF",""))));

            foreach (var file in files)
            {
                var b = File.ReadAllBytes(file.FullName);

                Encoding enc = null;

                var textoComEncodeCerto = ReadFileAndGetEncoding(b, ref enc);

                using (var cmd = new NpgsqlCommand(textoComEncodeCerto, connection))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Erro ao executar o script {file.FullName}. Erro: {ex.Message}", ex);
                    }

                }
            }
        }

        private string ReadFileAndGetEncoding(Byte[] docBytes, ref Encoding encoding)
        {
            if (encoding.EhNulo())
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

        private Int32 TestUtf8(Byte[] binFile, Int32 offset)
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

        private void ExecutarPreScripts(NpgsqlConnection connection)
        {
            var builder = new StringBuilder();
            builder.Append("CREATE USER postgres;");
            builder.Append("SET client_encoding TO 'UTF8';");
            using (var cmd = new NpgsqlCommand(builder.ToString(), connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void RemoverTodasForeingKey(NpgsqlConnection connection)
        {
            var builder = new StringBuilder();
            builder.Append(" DO $$ DECLARE ");
            builder.Append(" r RECORD; ");
            builder.Append(" BEGIN ");
            builder.Append(" for r in (select table_name, constraint_name from information_schema.table_constraints where table_schema = 'public'and constraint_type = 'FOREIGN KEY'");
            builder.Append(" ) loop");
            builder.Append(" execute CONCAT('ALTER TABLE ' || r.table_name || ' DROP CONSTRAINT ' || r.constraint_name);");
            builder.Append(" END LOOP; ");
            builder.Append(" END $$; ");

            using (var cmd = new NpgsqlCommand(builder.ToString(), connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void RemoverSequencesNaoUsadas(NpgsqlConnection connection)
        {
            var sql = @"ALTER TABLE tipo_ciclo ALTER COLUMN id DROP IDENTITY;
                        ALTER TABLE componente_curricular_jurema ALTER COLUMN id DROP IDENTITY;
                        ALTER TABLE tipo_escola ALTER COLUMN id DROP IDENTITY;
                        ALTER TABLE tipo_ciclo_ano ALTER COLUMN id DROP IDENTITY;";

            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
