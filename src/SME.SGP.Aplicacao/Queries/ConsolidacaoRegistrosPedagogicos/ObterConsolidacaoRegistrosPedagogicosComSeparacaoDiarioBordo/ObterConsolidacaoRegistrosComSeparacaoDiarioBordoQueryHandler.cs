using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacaoRegistrosComSeparacaoDiarioBordoQueryHandler :
        IRequestHandler<ObterConsolidacaoRegistrosComSeparacaoDiarioBordoQuery, IEnumerable<ConsolidacaoRegistrosPedagogicosDto>>
    {
        private readonly IRepositorioConsolidacaoRegistrosPedagogicos repositorio;

        public ObterConsolidacaoRegistrosComSeparacaoDiarioBordoQueryHandler(IRepositorioConsolidacaoRegistrosPedagogicos repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<ConsolidacaoRegistrosPedagogicosDto>> Handle(ObterConsolidacaoRegistrosComSeparacaoDiarioBordoQuery request, CancellationToken cancellationToken)
        {
            
            return await repositorio.GerarRegistrosPedagogicosComSeparacaoDiarioBordo(request.TurmaCodigo, request.AnoLetivo, request.ComponentesCurricularesIds);
        }
    }
}