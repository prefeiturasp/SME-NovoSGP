using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ObterResponsaveisPorDreDto
    {
        public ObterResponsaveisPorDreDto(string dreCodigo, TipoResponsavelAtribuicao? tipoResponsavelAtribuicao)
        {
            DreCodigo = dreCodigo;
            TipoResponsavelAtribuicao = tipoResponsavelAtribuicao;
        }

        public string DreCodigo { get; set; }
        public TipoResponsavelAtribuicao? TipoResponsavelAtribuicao { get; set; }
    }
}
