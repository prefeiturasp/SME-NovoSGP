using Elasticsearch.Net;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandler : IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>
    {
        private readonly IMediator mediator;

        public ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        
        public async Task<bool> Handle(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery request, CancellationToken cancellationToken)
        {

            bool podePersistir = await mediator.Send(new PodePersistirTurmaDisciplinaQuery(request.Usuario.CodigoRf, request.CodigoTurma, request.ComponenteCurricularId.ToString(), request.Data.Ticks));
            await mediator.Send(new SalvarLogViaRabbitCommand($"Pode persistir o professor {request.Usuario.CodigoRf} na turma {request.CodigoTurma} ? {podePersistir} ", LogNivel.Informacao, LogContexto.Aula, string.Empty));

            return podePersistir;
        }
    }
}
