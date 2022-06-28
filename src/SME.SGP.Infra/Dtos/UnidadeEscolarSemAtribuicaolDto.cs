using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class UnidadeEscolarSemAtribuicaolDto
    {
        public string Codigo { get; set; }
        public string UeNome { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public int TipoAtribuicao { get; set; }
        public bool AtribuicaoExcluida { get; set; }

        public string Nome
        {
            get
            {
                return $"{TipoEscola.ShortName()} {UeNome}";
            }
        }
    }
}
