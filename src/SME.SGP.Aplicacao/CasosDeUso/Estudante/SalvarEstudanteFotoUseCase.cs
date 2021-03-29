using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarFotoEstudanteUseCase : AbstractUseCase, ISalvarFotoEstudanteUseCase
    {
        public SalvarFotoEstudanteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(EstudanteFotoDto dto)
            => await mediator.Send(new SalvarFotoEstudanteCommand(dto));
    }
}
