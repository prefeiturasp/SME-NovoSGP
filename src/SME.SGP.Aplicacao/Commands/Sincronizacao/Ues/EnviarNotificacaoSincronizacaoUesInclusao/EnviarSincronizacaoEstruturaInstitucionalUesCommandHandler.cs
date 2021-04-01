using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviarSincronizacaoEstruturaInstitucionalUesCommandHandler : IRequestHandler<EnviarSincronizacaoEstruturaInstitucionalUesCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioUe repositorioUe;

        public EnviarSincronizacaoEstruturaInstitucionalUesCommandHandler(IMediator mediator, IRepositorioUe repositorioUe)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<bool> Handle(EnviarSincronizacaoEstruturaInstitucionalUesCommand request, CancellationToken cancellationToken)
        {
            var estruturaInstitucionalVigentePorDre = await mediator.Send(new ObterEstruturaInstitucionalVigenteQuery(request.CodigoDre));

            var dreId = await mediator.Send(new ObterDREIdPorCodigoQuery(request.CodigoDre));
            
            if (estruturaInstitucionalVigentePorDre != null && estruturaInstitucionalVigentePorDre.Dres != null && estruturaInstitucionalVigentePorDre.Dres.Count > 0)
            {
                var dres = estruturaInstitucionalVigentePorDre.Dres.Select(x => new Dre() { Abreviacao = x.Abreviacao, CodigoDre = x.Codigo, Nome = x.Nome, Id = dreId });
                var ues = estruturaInstitucionalVigentePorDre.Dres.SelectMany(x => x.Ues.Select(y => new Ue { CodigoUe = y.Codigo, TipoEscola = y.CodTipoEscola, Nome = y.Nome, Dre = new Dre() { CodigoDre = x.Codigo } }));

                ues = await repositorioUe.SincronizarAsync(ues, dres);
            }
            else
            {
                var erro = new NegocioException($"Não foi possível obter dados de estrutura institucional do EOL. {estruturaInstitucionalVigentePorDre?.Dres?.Count}");
                SentrySdk.CaptureException(erro);
                throw erro;
            }

            return true;
        }
    }
}
