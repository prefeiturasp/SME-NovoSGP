using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ObterSupervisoresPorDreDto
    {
        public ObterSupervisoresPorDreDto(string dreCodigo, TipoResponsavelAtribuicao tipoResponsavelAtribuicao)
        {
            DreCodigo = dreCodigo;
            TipoResponsavelAtribuicao = tipoResponsavelAtribuicao;
        }

        public string DreCodigo { get; set; }
        public TipoResponsavelAtribuicao TipoResponsavelAtribuicao { get; set; }
    }
}
