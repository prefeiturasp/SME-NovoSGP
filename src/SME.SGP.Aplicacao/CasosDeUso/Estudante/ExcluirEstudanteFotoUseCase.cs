using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirEstudanteFotoUseCase : AbstractUseCase, IExcluirEstudanteFotoUseCase
    {
        public ExcluirEstudanteFotoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(string codigoAluno)
            => await mediator.Send(new ExcluirFotoEstudanteCommand(codigoAluno));
    }
}
