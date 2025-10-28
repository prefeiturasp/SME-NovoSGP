using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.PainelEducacional.ProficienciaIdeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdeb
{
    public class ObterProficienciaIdebQueryHandler : IRequestHandler<ObterProficienciaIdebQuery, IEnumerable<PainelEducacionalProficienciaIdebDto>>
    {
        private readonly IRepositorioPainelEducacionalProficienciaIdeb _repositorioProficienciaIdeb;
        public ObterProficienciaIdebQueryHandler(IRepositorioPainelEducacionalProficienciaIdeb repositorioProficienciaIdeb)
        {
            _repositorioProficienciaIdeb = repositorioProficienciaIdeb;
        }

        public async Task<IEnumerable<PainelEducacionalProficienciaIdebDto>> Handle(ObterProficienciaIdebQuery request, CancellationToken cancellationToken)
        {
            var proficiencias = await ObterProficienciaConsolidadaAsync(request.AnoLetivo, request.CodigoUe);

            if (proficiencias == null || !proficiencias.Any())
                return Array.Empty<PainelEducacionalProficienciaIdebDto>();

            var resultadoFinal = proficiencias
                .GroupBy(g => g.AnoLetivo)
                .Select(g =>
                {
                    var anosIniciais = g.Where(p => p.SerieAno == SerieAnoIndiceDesenvolvimentoEnum.AnosIniciais);
                    var anosFinais = g.Where(p => p.SerieAno == SerieAnoIndiceDesenvolvimentoEnum.AnosFinais);
                    var ensinoMedio = g.Where(p => p.SerieAno == SerieAnoIndiceDesenvolvimentoEnum.EnsinoMedio);
                    return new PainelEducacionalProficienciaIdebDto
                    {
                        AnoLetivo = g.Key,
                        NotaInicial = anosIniciais.FirstOrDefault()?.Nota,
                        NotaFinal = anosFinais.FirstOrDefault()?.Nota,
                        NotaEnsinoMedio = ensinoMedio.FirstOrDefault()?.Nota,
                        Boletim = g.FirstOrDefault(f => !string.IsNullOrWhiteSpace(f.Boletim))?.Boletim,
                        Proficiencia = new ProficienciaIdebResumidoDto
                        {
                            AnosIniciais = DefinirProficienciaPorComponenteCurricular(anosIniciais),
                            AnosFinais = DefinirProficienciaPorComponenteCurricular(anosFinais),
                            EnsinoMedio = DefinirProficienciaPorComponenteCurricular(ensinoMedio)
                        }
                    };
                });

            return resultadoFinal.OrderByDescending(r => r.AnoLetivo);
        }

        private async Task<IEnumerable<PainelEducacionalConsolidacaoProficienciaIdebUe>> ObterProficienciaConsolidadaAsync(int anoLetivo, string codigoUe)
        {
            if (anoLetivo <= 0)
                return await _repositorioProficienciaIdeb.ObterConsolidacaoPorAnoVisaoUeAsync(PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE, anoLetivo, codigoUe);

            for (int anoUtilizado = anoLetivo; anoUtilizado >= PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE; anoUtilizado--)
            {
                var proficiencias = await _repositorioProficienciaIdeb.ObterConsolidacaoPorAnoVisaoUeAsync(PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE, anoUtilizado, codigoUe);

                if (proficiencias != null && proficiencias.Any())
                    return proficiencias;
            }
            return Array.Empty<PainelEducacionalConsolidacaoProficienciaIdebUe>();
        }

        private List<ComponenteCurricularIdebResumidoDto> DefinirProficienciaPorComponenteCurricular(IEnumerable<PainelEducacionalConsolidacaoProficienciaIdebUe> proficiencias) =>
            new List<ComponenteCurricularIdebResumidoDto>
            {
                new ComponenteCurricularIdebResumidoDto
                {
                    ComponenteCurricular = ComponenteCurricularEnum.Portugues,
                    Percentual = proficiencias.FirstOrDefault(p => p.ComponenteCurricular == ComponenteCurricularEnum.Portugues)?.Proficiencia
                },

                new ComponenteCurricularIdebResumidoDto
                {
                    ComponenteCurricular = ComponenteCurricularEnum.Matematica,
                    Percentual = proficiencias.FirstOrDefault(p => p.ComponenteCurricular == ComponenteCurricularEnum.Matematica)?.Proficiencia
                },

                new ComponenteCurricularIdebResumidoDto
                {
                    ComponenteCurricular = ComponenteCurricularEnum.CienciasNatureza,
                    Percentual = proficiencias.FirstOrDefault(p => p.ComponenteCurricular == ComponenteCurricularEnum.CienciasNatureza)?.Proficiencia
                }
            };
    }
}
