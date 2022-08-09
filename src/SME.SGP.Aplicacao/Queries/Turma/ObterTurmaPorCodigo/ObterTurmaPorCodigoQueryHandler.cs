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
    public class ObterTurmaPorCodigoQueryHandler : IRequestHandler<ObterTurmaPorCodigoQuery, Turma>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;
        private readonly IMediator mediator;

        public ObterTurmaPorCodigoQueryHandler(
                        IRepositorioTurmaConsulta repositorioTurmaConsulta, 
                        IMediator mediator) 
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        public async Task<Turma> Handle(ObterTurmaPorCodigoQuery request, CancellationToken cancellationToken)
        {
            var id = await repositorioTurmaConsulta.ObterTurmaIdPorCodigo(request.TurmaCodigo);

            return await this.mediator.Send(new ObterTurmaComUeEDrePorIdQuery(id));
        }
    }
}
