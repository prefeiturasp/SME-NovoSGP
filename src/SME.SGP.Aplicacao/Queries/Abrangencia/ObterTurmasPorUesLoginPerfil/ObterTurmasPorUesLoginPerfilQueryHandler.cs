using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorUesLoginPerfilQueryHandler : IRequestHandler<ObterTurmasPorUesLoginPerfilQuery, IEnumerable<AbrangenciaTurmaComUeRetorno>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterTurmasPorUesLoginPerfilQueryHandler(IMediator mediator, IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        private const int LimiteUmaChamadaEol = 50;
        private const int QuantidadeChunks = 10;

        private async Task<IEnumerable<TurmaApiEolDto>> ObterTurmasEolEmLotes(List<string> codigosTurmas)
        {
            if (codigosTurmas == null || !codigosTurmas.Any())
                return Enumerable.Empty<TurmaApiEolDto>();

            if (codigosTurmas.Count <= LimiteUmaChamadaEol)
                return await mediator.Send(new ObterTurmasApiEolQuery(codigosTurmas));

            var tamanhoChunk = (int)Math.Ceiling(codigosTurmas.Count / (double)QuantidadeChunks);

            var chunks = codigosTurmas
                .Select((codigo, i) => (codigo, i))
                .GroupBy(x => x.i / tamanhoChunk)
                .Select(g => g.Select(x => x.codigo).ToList());

            var resultados = new List<TurmaApiEolDto>();
            foreach (var chunk in chunks)
                resultados.AddRange(await mediator.Send(new ObterTurmasApiEolQuery(chunk)));

            return resultados;
        }

        public async Task<IEnumerable<AbrangenciaTurmaComUeRetorno>> Handle(ObterTurmasPorUesLoginPerfilQuery request, CancellationToken cancellationToken)
        {
            var anosInfantilDesconsiderar = await mediator.Send(
                new ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery(request.AnoLetivo, Modalidade.EducacaoInfantil));

            var result = await repositorioAbrangencia.ObterTurmasPorTiposListaUes(
                request.CodigosUes, request.Login, request.Perfil,
                request.Modalidade, null,
                request.Periodo, request.ConsideraHistorico, request.AnoLetivo,
                anosInfantilDesconsiderar);

            return result;
        }
    }
}
