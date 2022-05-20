using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ObterSupervisoresPorDreDto
    {
        public ObterSupervisoresPorDreDto(string dreCodigo, string supervisorNome, TipoResponsavelAtribuicao tipoResponsavelAtribuicao)
        {
            DreCodigo = dreCodigo;
            SupervisorNome = supervisorNome;
            TipoResponsavelAtribuicao = tipoResponsavelAtribuicao;
        }

        public string DreCodigo { get; set; }
        public string SupervisorNome { get; set; }
        public TipoResponsavelAtribuicao TipoResponsavelAtribuicao { get; set; }
    }
}
