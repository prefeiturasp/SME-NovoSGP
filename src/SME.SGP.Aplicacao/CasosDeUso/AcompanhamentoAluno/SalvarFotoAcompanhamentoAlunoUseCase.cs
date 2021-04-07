using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarFotoAcompanhamentoAlunoUseCase : AbstractUseCase, ISalvarFotoAcompanhamentoAlunoUseCase
    {
        public SalvarFotoAcompanhamentoAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(AcompanhamentoAlunoDto acompanhamento)
            => await mediator.Send(new SalvarFotoAcompanhamentoAlunoCommand(acompanhamento));
    }
}
