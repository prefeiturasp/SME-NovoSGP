using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCiclosPorModalidadeECodigoUeQueryHandler : IRequestHandler<ObterCiclosPorModalidadeECodigoUeQuery, IEnumerable<RetornoCicloDto>>
    {
        private readonly IRepositorioCiclo repositorioCiclo;
        private readonly IMediator mediator;

        public ObterCiclosPorModalidadeECodigoUeQueryHandler(IRepositorioCiclo repositorioCiclo, IMediator mediator)
        {
            this.repositorioCiclo = repositorioCiclo ?? throw new ArgumentNullException(nameof(repositorioCiclo));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<RetornoCicloDto>> Handle(ObterCiclosPorModalidadeECodigoUeQuery request, CancellationToken cancellationToken)
        {
            if (request.ConsideraAbrangencia && request.CodigoUe != "-99")
            {
                var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
                if (usuario == null)
                    throw new NegocioException("Não foi possível obter o usuário logado.");

                var ciclosAbrangencia = await repositorioCiclo.ObterCiclosPorAnoModalidadeECodigoUeAbrangencia(new FiltroCicloPorModalidadeECodigoUeDto(request.Modalidade, request.CodigoUe, request.ConsideraAbrangencia), usuario.Id, usuario.PerfilAtual);

                if (ciclosAbrangencia == null || !ciclosAbrangencia.Any())
                    throw new NegocioException("Não foi possível obter os ciclos");

                return ciclosAbrangencia;
            }
            else
            {
                var ciclos = await repositorioCiclo.ObterCiclosPorAnoModalidadeECodigoUe(new FiltroCicloPorModalidadeECodigoUeDto(request.Modalidade, request.CodigoUe, request.ConsideraAbrangencia));

                if (ciclos == null || !ciclos.Any())
                    throw new NegocioException("Não foi possível obter os ciclos");

                return ciclos;
            }

        }
    }
}
