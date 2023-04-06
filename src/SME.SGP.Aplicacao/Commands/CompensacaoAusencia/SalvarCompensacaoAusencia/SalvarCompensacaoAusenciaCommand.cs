using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarCompensacaoAusenciaCommand : IRequest<long>
    {
        public SalvarCompensacaoAusenciaCommand(CompensacaoAusencia compensacaoAusencia)
        {
            CompensacaoAusencia = compensacaoAusencia;
        }

        public CompensacaoAusencia CompensacaoAusencia { get; }
    }
}
