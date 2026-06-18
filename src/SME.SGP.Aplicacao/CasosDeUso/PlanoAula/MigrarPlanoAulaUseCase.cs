using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class MigrarPlanoAulaUseCase : AbstractUseCase, IMigrarPlanoAulaUseCase
    {
        public MigrarPlanoAulaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MigrarPlanoAulaDto param)
        {
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            return await mediator.Send(new MigrarPlanoAulaCommand(param, usuario));
        }
    }
}
