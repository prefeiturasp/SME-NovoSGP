using SME.SGP.Dominio.Constantes;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SME.SGP.Infra
{
    public static class StringExtension
    {
        static public string EncodeTo64(this string toEncode)
        {

            byte[] toEncodeAsBytes

                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);

            string returnValue

                  = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }

        public static bool ExcedeuQuantidadeImagensPermitidas(this string textoParaAvaliar, int quantidadePermitida)
        {
            return textoParaAvaliar.Split().LongCount(x => x.Contains("src=")) > quantidadePermitida;
        }

        public static bool EhLoginCpf(this string login)
        {
            return (login.ToCharArray().All(c => char.IsDigit(c)) && login.Length > 7);
        } 
        
        public static bool EstaPreenchido(this string str)
        {
            return !string.IsNullOrEmpty(str) || !string.IsNullOrWhiteSpace(str);
        }
        
        public static bool NaoEstaPreenchido(this string str)
        {
            return (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str));
        }
        
        public static bool EhExtensaoImagemParaOtimizar(this string extensao)
        {
            return (extensao.ToLower().Equals(".jpg") || extensao.ToLower().Equals(".jpeg") || extensao.ToLower().Equals(".png"));
        }
        
        public static bool EhArquivoImagemParaOtimizar(this string nomeArquivo)
        {
            return EhExtensaoImagemParaOtimizar(Path.GetExtension(nomeArquivo));
        }
        
        public static bool EhExtensaoVideoParaOtimizar(this string extensao)
        {
            return (extensao.ToLower().Equals(".mp4") || extensao.ToLower().Equals(".mpeg"));
        }
        
        public static bool EhArquivoVideoParaOtimizar(this string nomeArquivo)
        {
            return EhExtensaoVideoParaOtimizar(Path.GetExtension(nomeArquivo));
        }

        public static bool EhIdComponenteCurricularTerritorioSaberAgrupado(this string source)
        {
            long componenteCurricularTerritorioSaberAgrupadoId = 0;
            if (long.TryParse(source, out componenteCurricularTerritorioSaberAgrupadoId))
            {
                return componenteCurricularTerritorioSaberAgrupadoId.EhIdComponenteCurricularTerritorioSaberAgrupado();
            }
            return false;
        }

        public static bool EhIdComponenteCurricularTerritorioSaberAgrupado(this long source)
        {
            return source >= TerritorioSaberConstants.COMPONENTE_AGRUPAMENTO_TERRITORIO_SABER_ID_INICIAL;
        }
        
        
        public static bool EhTelefoneValido(this string telefone)
        {
            return new Regex(@"^\(\d{2}\) \d{4}-\d{4}$").Match(telefone).Success;
        }
        
        public static bool NaoEhTelefoneValido(this string telefone)
        {
            return !EhTelefoneValido(telefone);
        }
        
        public static bool EhEmailValido(this string email)
        {
            return new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").Match(email).Success;
        }
        
        public static bool NaoEhEmailValido(this string email)
        {
            return !EhEmailValido(email);
        }

        public static bool NaoEhCpfValido(this string cpf)
        {
            return !EhCpfValido(cpf);
        }

        public static bool EhCpfValido(this string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;

            for (int j = 0; j < 10; j++)
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                    return false;

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;
            
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto;

            return cpf.EndsWith(digito);
        }
    }
}
