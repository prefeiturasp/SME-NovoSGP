using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class UnidadeEscolarResponsavelDto : AuditoriaDto
    {
        public string Codigo { get; set; }
        public string UeNome { get; set; }
        public TipoResponsavelAtribuicao Tipo { get; set; }
        public TipoEscola TipoEscola { get; set; }

        public string Nome
        {
            get
            {
                return $"{TipoEscola.ShortName()} {UeNome}";
            }
        }

        public string TipoAtribuicao
        {
            get
            {
                return Tipo.Name();
            }
        }
    }
}
