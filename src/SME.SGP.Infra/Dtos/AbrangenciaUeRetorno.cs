using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Dto
{
    public class AbrangenciaUeRetorno
    {
        public string Codigo { get; set; }
        public string NomeSimples { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public long Id { get; set; }
        public string Nome
        {
            get
            {
                if (TipoEscola == TipoEscola.Nenhum)
                    return NomeSimples;
                else return $"{TipoEscola.ShortName()} {NomeSimples}";
            }
        }
        public bool EhInfantil => TipoEscola == TipoEscola.EMEI || TipoEscola == TipoEscola.CEUEMEI ||
                                  TipoEscola == TipoEscola.CECI || TipoEscola == TipoEscola.CEMEI ||
                                  TipoEscola == TipoEscola.CEUCEMEI;
    }
}