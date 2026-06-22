using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos
{
    public record FiltroTurmasPorUeRequestDto
    {
        public Modalidade Modalidade { get; init; }
        public int Periodo { get; init; } = 0;
        public int AnoLetivo { get; init; } = 0;
        public int[] Tipos { get; init; }
        public bool ConsideraNovosAnosInfantil { get; init; } = false;
        public string[] AnosTurma { get; init; }
    }
}
