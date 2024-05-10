using SME.SGP.Dominio;

namespace SME.SGP.TesteIntegracao.RegistroIndividual
{
    public class FiltroRegistroIndividualDto
    {
        public string Perfil { get; set; }
        public Modalidade Modalidade { get; set; }
        public ModalidadeTipoCalendario TipoCalendario { get; set; }
        public bool EhAnoAnterior { get; set; }
        public bool CriarPeriodoReabertura { get; set; } = true;
        public long TipoCalendarioId { get; set; } = 1;
        public bool BimestreEncerrado { get; set; }
        public bool NaoCriarPeriodosEscolares { get; set; }
    }
} 