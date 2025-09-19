using MediatR;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNumeroEstudantesPap;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap
{
    public class ObterIndicadoresPapQueryHandler : IRequestHandler<ObterIndicadoresPapQuery, IEnumerable<PainelEducacionalInformacoesPapDto>>
    {
        private readonly IMediator mediator;

        public ObterIndicadoresPapQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<PainelEducacionalInformacoesPapDto>> Handle(ObterIndicadoresPapQuery request, CancellationToken cancellationToken)
        {
            if ((string.IsNullOrWhiteSpace(request.CodigoDre) && string.IsNullOrWhiteSpace(request.CodigoUe)) || (request.CodigoDre == "-99" && request.CodigoUe == "-99"))
            {
                return await ObterIndicadoresConsolidadosGerais(cancellationToken);
            }

            return await ObterIndicadoresComFiltros(request, cancellationToken);
        }

        private async Task<IEnumerable<PainelEducacionalInformacoesPapDto>> ObterIndicadoresConsolidadosGerais(CancellationToken cancellationToken)
        {
            var indicadoresCombinados = new List<PainelEducacionalInformacoesPapDto>();

            var dificuldadesGerais = await mediator.Send(new ObterIndicadoresPapSgpConsolidadoQuery(), cancellationToken);

            var numerosEolGerais = await mediator.Send(new PainelEducacionalIndicadoresPapEolQuery("", ""), cancellationToken);

            var numerosPorTipo = new Dictionary<TipoPap, NumerosPapDto>();
            
            if (numerosEolGerais?.Any() == true)
            {
                foreach (var grupo in numerosEolGerais.GroupBy(x => x.TipoPap))
                {
                    numerosPorTipo[grupo.Key] = new NumerosPapDto
                    {
                        QuantidadeTurmas = grupo.Sum(x => x.QuantidadeTurmas),
                        QuantidadeEstudantes = grupo.Sum(x => x.QuantidadeEstudantes),
                        QuantidadeEstudantesComFrequenciaInferiorLimite = grupo.Sum(x => x.QuantidadeEstudantesComFrequenciaInferiorLimite)
                    };
                }
            }

            var dificuldadesPorTipo = dificuldadesGerais?.ToDictionary(x => x.TipoPap, x => x)
                ?? new Dictionary<TipoPap, ContagemDificuldadePorTipoDto>();

            foreach (var tipoPap in Enum.GetValues<TipoPap>())
            {
                var numeros = numerosPorTipo.ContainsKey(tipoPap) ? numerosPorTipo[tipoPap] : null;
                var dificuldades = dificuldadesPorTipo.ContainsKey(tipoPap) ? dificuldadesPorTipo[tipoPap] : null;

                if (numeros != null || dificuldades != null)
                {
                    indicadoresCombinados.Add(new PainelEducacionalInformacoesPapDto(
                        id: 0,
                        tipoPap: tipoPap,
                        dreCodigo: "",
                        ueCodigo: "",
                        dreNome: "",
                        ueNome: "",
                        quantidadeTurmas: numeros?.QuantidadeTurmas ?? 0,
                        quantidadeEstudantes: numeros?.QuantidadeEstudantes ?? 0,
                        quantidadeEstudantesComFrequenciaInferiorLimite: numeros?.QuantidadeEstudantesComFrequenciaInferiorLimite ?? 0,
                        dificuldadeAprendizagemTop1: dificuldades?.QuantidadeEstudantesDificuldadeTop1 ?? 0,
                        dificuldadeAprendizagemTop2: dificuldades?.QuantidadeEstudantesDificuldadeTop2 ?? 0,
                        outrasDificuldadesAprendizagem: dificuldades?.OutrasDificuldadesAprendizagem ?? 0,
                        nomeDificuldadeTop1: dificuldades?.NomeDificuldadeTop1 ?? "",
                        nomeDificuldadeTop2: dificuldades?.NomeDificuldadeTop2 ?? ""
                    ));
                }
            }

            return indicadoresCombinados.OrderBy(i => i.TipoPap);
        }

        private async Task<IEnumerable<PainelEducacionalInformacoesPapDto>> ObterIndicadoresComFiltros(ObterIndicadoresPapQuery request, CancellationToken cancellationToken)
        {
            var indicadoresCombinados = new List<PainelEducacionalInformacoesPapDto>();

            var numerosSgp = await mediator.Send(new ObterIndicadoresPapSgpQuery(request.CodigoDre, request.CodigoUe), cancellationToken);

            var dificuldadesPorDreUeTipo = numerosSgp?.Any() == true
                ? numerosSgp
                    .Where(n => n != null && n.TipoPap != default && !string.IsNullOrWhiteSpace(n.CodigoDre))
                    .GroupBy(n => new { n.TipoPap, n.CodigoDre, n.CodigoUe })
                    .ToDictionary(
                        g => (g.Key.TipoPap, g.Key.CodigoDre, g.Key.CodigoUe),
                        g => new ContagemDificuldadePorTipoDto
                        {
                            TipoPap = g.Key.TipoPap,
                            CodigoDre = g.Key.CodigoDre ?? string.Empty,
                            CodigoUe = g.Key.CodigoUe ?? string.Empty,
                            NomeDre = g.FirstOrDefault()?.NomeDre ?? string.Empty,
                            NomeUe = g.FirstOrDefault()?.NomeUe ?? string.Empty,
                            QuantidadeEstudantesDificuldadeTop1 = g.Sum(x => x.QuantidadeEstudantesDificuldadeTop1),
                            QuantidadeEstudantesDificuldadeTop2 = g.Sum(x => x.QuantidadeEstudantesDificuldadeTop2),
                            OutrasDificuldadesAprendizagem = g.Sum(x => x.OutrasDificuldadesAprendizagem),
                            NomeDificuldadeTop1 = g.FirstOrDefault()?.NomeDificuldadeTop1 ?? string.Empty,
                            NomeDificuldadeTop2 = g.FirstOrDefault()?.NomeDificuldadeTop2 ?? string.Empty
                        })
                : new Dictionary<(TipoPap, string, string), ContagemDificuldadePorTipoDto>();

            var combinacoesDreUe = dificuldadesPorDreUeTipo.Values
                .GroupBy(d => new { d.CodigoDre, d.CodigoUe })
                .Select(g => g.First())
                .ToList();

            foreach (var combinacao in combinacoesDreUe)
            {
                var numerosEolEspecifico = await mediator.Send(
                    new PainelEducacionalIndicadoresPapEolQuery(combinacao.CodigoDre, combinacao.CodigoUe),
                    cancellationToken);

                var numerosPorTipo = new Dictionary<TipoPap, ContagemNumeroAlunosPapDto>();
                if (numerosEolEspecifico?.Any() == true)
                {
                    foreach (var item in numerosEolEspecifico)
                    {
                        if (!numerosPorTipo.ContainsKey(item.TipoPap))
                        {
                            numerosPorTipo[item.TipoPap] = item;
                        }
                        else
                        {
                            var existente = numerosPorTipo[item.TipoPap];
                            existente.QuantidadeTurmas += item.QuantidadeTurmas;
                            existente.QuantidadeEstudantes += item.QuantidadeEstudantes;
                            existente.QuantidadeEstudantesComFrequenciaInferiorLimite += item.QuantidadeEstudantesComFrequenciaInferiorLimite;
                        }
                    }
                }

                var todosTiposPap = Enum.GetValues<TipoPap>().AsEnumerable();

                foreach (var tipoPap in todosTiposPap)
                {
                    var chave = (tipoPap, combinacao.CodigoDre, combinacao.CodigoUe);
                    var dificuldades = dificuldadesPorDreUeTipo.ContainsKey(chave) ? dificuldadesPorDreUeTipo[chave] : null;
                    var numerosEol = numerosPorTipo.ContainsKey(tipoPap) ? numerosPorTipo[tipoPap] : null;

                    if (numerosEol != null || dificuldades != null)
                    {
                        indicadoresCombinados.Add(new PainelEducacionalInformacoesPapDto(
                            id: 0,
                            tipoPap: tipoPap,
                            dreCodigo: combinacao.CodigoDre,
                            ueCodigo: combinacao.CodigoUe,
                            dreNome: dificuldades?.NomeDre ?? combinacao.NomeDre,
                            ueNome: dificuldades?.NomeUe ?? combinacao.NomeUe,
                            quantidadeTurmas: numerosEol?.QuantidadeTurmas ?? 0,
                            quantidadeEstudantes: numerosEol?.QuantidadeEstudantes ?? 0,
                            quantidadeEstudantesComFrequenciaInferiorLimite: numerosEol?.QuantidadeEstudantesComFrequenciaInferiorLimite ?? 0,
                            dificuldadeAprendizagemTop1: dificuldades?.QuantidadeEstudantesDificuldadeTop1 ?? 0,
                            dificuldadeAprendizagemTop2: dificuldades?.QuantidadeEstudantesDificuldadeTop2 ?? 0,
                            outrasDificuldadesAprendizagem: dificuldades?.OutrasDificuldadesAprendizagem ?? 0,
                            nomeDificuldadeTop1: dificuldades?.NomeDificuldadeTop1 ?? "",
                            nomeDificuldadeTop2: dificuldades?.NomeDificuldadeTop2 ?? ""
                        ));
                    }
                }
            }

            if (!indicadoresCombinados.Any())
            {
                var numerosEolGerais = await mediator.Send(new PainelEducacionalIndicadoresPapEolQuery(request.CodigoDre, request.CodigoUe), cancellationToken);
                var todosTiposPap = Enum.GetValues<TipoPap>().AsEnumerable();

                foreach (var tipoPap in todosTiposPap)
                {
                    var numerosEol = numerosEolGerais?.FirstOrDefault(n => n.TipoPap == tipoPap);

                    if (numerosEol != null)
                    {
                        indicadoresCombinados.Add(new PainelEducacionalInformacoesPapDto(
                            id: 0,
                            tipoPap: tipoPap,
                            dreCodigo: request.CodigoDre ?? "",
                            ueCodigo: request.CodigoUe ?? "",
                            dreNome: "",
                            ueNome: "",
                            quantidadeTurmas: numerosEol.QuantidadeTurmas,
                            quantidadeEstudantes: numerosEol.QuantidadeEstudantes,
                            quantidadeEstudantesComFrequenciaInferiorLimite: numerosEol.QuantidadeEstudantesComFrequenciaInferiorLimite,
                            dificuldadeAprendizagemTop1: 0,
                            dificuldadeAprendizagemTop2: 0,
                            outrasDificuldadesAprendizagem: 0,
                            nomeDificuldadeTop1: "",
                            nomeDificuldadeTop2: ""
                        ));
                    }
                }
            }

            return indicadoresCombinados.OrderBy(i => i.TipoPap)
                                       .ThenBy(i => i.DreCodigo)
                                       .ThenBy(i => i.UeCodigo);
        }

        private class NumerosPapDto
        {
            public int QuantidadeTurmas { get; set; }
            public int QuantidadeEstudantes { get; set; }
            public int QuantidadeEstudantesComFrequenciaInferiorLimite { get; set; }
        }
    }
}