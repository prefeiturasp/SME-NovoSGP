using System.IO;
using System.Linq;

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
    }
}
