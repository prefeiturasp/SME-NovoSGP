using MediatR;

namespace SME.SGP.Aplicacao
{
    public class PodeExecutarPendenciaComponenteSemAulaQuery : IRequest<bool>
    {
        private static PodeExecutarPendenciaComponenteSemAulaQuery _instance;
        public static PodeExecutarPendenciaComponenteSemAulaQuery Instance => _instance ??= new();
    }
}
