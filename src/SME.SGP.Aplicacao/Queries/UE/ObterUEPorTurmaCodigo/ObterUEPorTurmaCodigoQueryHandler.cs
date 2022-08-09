using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUEPorTurmaCodigoQueryHandler: IRequestHandler<ObterUEPorTurmaCodigoQuery, Ue>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;
        private readonly IMediator mediator;

        public ObterUEPorTurmaCodigoQueryHandler(
                    IRepositorioTurmaConsulta repositorioTurma,
                    IMediator mediator) 
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Ue> Handle(ObterUEPorTurmaCodigoQuery request, CancellationToken cancellationToken)
        {
            var id = await repositorioTurma.ObterTurmaIdPorCodigo(request.TurmaCodigo);
            var turma = await this.mediator.Send(new ObterTurmaComUeEDrePorIdQuery(id));

            return turma?.Ue;
        }
    }
}
