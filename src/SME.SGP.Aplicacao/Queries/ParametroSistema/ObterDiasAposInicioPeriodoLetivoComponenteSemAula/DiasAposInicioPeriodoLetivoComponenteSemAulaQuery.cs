using MediatR;

namespace SME.SGP.Aplicacao
{
    public class DiasAposInicioPeriodoLetivoComponenteSemAulaQuery : IRequest<int>
    {
        private static DiasAposInicioPeriodoLetivoComponenteSemAulaQuery _instance;
        public static DiasAposInicioPeriodoLetivoComponenteSemAulaQuery Instance => _instance ??= new();
    }
}
