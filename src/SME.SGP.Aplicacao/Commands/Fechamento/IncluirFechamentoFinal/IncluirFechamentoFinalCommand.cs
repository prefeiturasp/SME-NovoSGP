using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class IncluirFechamentoFinalCommand : IRequest<long>
    {
        public IncluirFechamentoFinalCommand(IncluirFechamentoDto incluirFechamentoDto)
        {
            IncluirFechamentoDto = incluirFechamentoDto;
        }

        public IncluirFechamentoDto IncluirFechamentoDto { get; }
    }
}
