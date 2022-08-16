using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaComUeEDrePorCodigoQueryHandler : IRequestHandler<ObterTurmaComUeEDrePorCodigoQuery, Turma>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;
        private readonly IMediator mediator;

        public ObterTurmaComUeEDrePorCodigoQueryHandler(
                        IRepositorioTurmaConsulta repositorioTurma, 
                        IMediator mediator) 
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Turma> Handle(ObterTurmaComUeEDrePorCodigoQuery request, CancellationToken cancellationToken)
        {
            var id = await repositorioTurma.ObterTurmaIdPorCodigo(request.TurmaCodigo);

            if (id == 0)
                throw new NegocioException(MensagensNegocioFrequencia.Turma_informada_nao_foi_encontrada);
            
            return await this.mediator.Send(new ObterTurmaComUeEDrePorIdQuery(id)); 
        }
    }
}
