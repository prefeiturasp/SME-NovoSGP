using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroIndividualPorAlunoDataUseCase : AbstractUseCase, IObterRegistroIndividualPorAlunoDataUseCase
    {
        public ObterRegistroIndividualPorAlunoDataUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<RegistroIndividualDto> Executar(FiltroRegistroIndividualAlunoData filtro)
        {
            var registroIndividual = await mediator.Send(new ObterRegistroIndividualPorAlunoDataQuery(filtro.TurmaId, filtro.AlunoCodigo, filtro.ComponenteCurricularId, filtro.Data));

            return registroIndividual;
        }
    }
}
