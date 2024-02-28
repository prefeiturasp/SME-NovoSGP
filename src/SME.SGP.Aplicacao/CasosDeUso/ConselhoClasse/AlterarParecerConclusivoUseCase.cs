using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.ConselhoClasse
{
    public class AlterarParecerConclusivoUseCase : AbstractUseCase, IAlterarParecerConclusivoUseCase
    {
        public AlterarParecerConclusivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ParecerConclusivoDto> Executar(AlterarParecerConclusivoDto param)
        {
            return await mediator.Send(new AlterarManualParecerConclusivoCommand(param));
        }
    }
}
