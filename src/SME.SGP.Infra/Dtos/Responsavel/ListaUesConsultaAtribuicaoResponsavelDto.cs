using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public  class ListaUesConsultaAtribuicaoResponsavelDto
    {
        public long Id { get; set; }
        public string Codigo { get; set; }
        public string NomeSimples { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public string Nome
        {
            get
            {
                if (TipoEscola == TipoEscola.Nenhum)
                    return NomeSimples;
                else return $"{TipoEscola.ShortName()} {NomeSimples}";
            }
        }
    }
}
