using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Dto
{
    public class AbrangenciaUeRetorno
    {
        public string Codigo { get; set; }
        public string NomeSimples { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public string Nome => $"{TipoEscola.ShortName()} {NomeSimples}";
    }
}