using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPorFechamentosTurmaQueryHandler : IRequestHandler<ObterPorFechamentosTurmaQuery, IEnumerable<FechamentoNotaAlunoAprovacaoDto>>
    {
        private readonly IRepositorioFechamentoNotaConsulta repositorioFechamentoNota;
        private readonly IMediator mediator;

        public ObterPorFechamentosTurmaQueryHandler(IRepositorioFechamentoNotaConsulta repositorioFechamentoNota, IMediator mediator)
        {
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
            this.mediator = mediator;
        }

        public async Task<IEnumerable<FechamentoNotaAlunoAprovacaoDto>> Handle(ObterPorFechamentosTurmaQuery request, CancellationToken cancellationToken)
        {
            var nomeChaveCache = $"fechamentoNotaFinais-{request.CodigoDisciplina}-{request.CodigoTurma}";

            var dadosCache = await mediator.Send(new ObterCacheAsyncQuery(nomeChaveCache), cancellationToken);
            var retornoCache = await MapearRetornoParaDto(dadosCache);

            if (retornoCache != null)
                return retornoCache;

            var retornoDb = await repositorioFechamentoNota.ObterPorFechamentosTurma(request.Ids);
            await mediator.Send(new SalvarCachePorValorObjetoCommand(nomeChaveCache, retornoDb));

            return retornoDb;
        }

        private static async Task<IEnumerable<FechamentoNotaAlunoAprovacaoDto>> MapearRetornoParaDto(string dadosCache)
        {
            return await Task.FromResult(JsonConvert.DeserializeObject<IEnumerable<FechamentoNotaAlunoAprovacaoDto>>(dadosCache));
        }
    }
}