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
    }
}
