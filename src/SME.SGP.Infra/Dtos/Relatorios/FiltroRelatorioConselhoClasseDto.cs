using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.Relatorios
{
    public  class FiltroRelatorioConselhoClasseDto
    {
        public long FechamentoTurmaId{ get; set; }

        public long ConselhoClasseId { get; set; }

        public Usuario Usuario { get; set; }
    }
}
