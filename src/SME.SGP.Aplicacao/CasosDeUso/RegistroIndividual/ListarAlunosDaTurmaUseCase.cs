using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarAlunosDaTurmaUseCase : AbstractUseCase, IListarAlunosDaTurmaUseCase
    {
        public ListarAlunosDaTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AlunoDadosBasicosDto>> Executar(FiltroRegistroIndividualBase filtro)
        {
            var registroIndividual = await mediator.Send(new ListarAlunosDaTurmaPorComponenteQuery(filtro.TurmaId, filtro.ComponenteCurricularId));

            return registroIndividual;
        }
    }
}
