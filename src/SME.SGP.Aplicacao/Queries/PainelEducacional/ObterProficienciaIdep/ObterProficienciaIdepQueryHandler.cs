using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdep
{
    public class ObterProficienciaIdepQueryHandler : IRequestHandler<ObterProficienciaIdepQuery, IEnumerable<PainelEducacionalProficienciaIdepDto>>
    {
        private readonly IRepositorioPainelEducacionalProficienciaIdep _repositorioProficienciaIdep;
        public ObterProficienciaIdepQueryHandler(IRepositorioPainelEducacionalProficienciaIdep repositorioIdepConsulta)
        {
            _repositorioProficienciaIdep = repositorioIdepConsulta;
        }
        public async Task<IEnumerable<PainelEducacionalProficienciaIdepDto>> Handle(ObterProficienciaIdepQuery request, CancellationToken cancellationToken)
        {
            var proficiencias = await ObterProficienciaConsolidadaAsync(request.AnoLetivo, request.CodigoUe);

            if (proficiencias == null || !proficiencias.Any())
                return Array.Empty<PainelEducacionalProficienciaIdepDto>();

            var resultadoFinal = proficiencias
                .GroupBy(g => g.AnoLetivo)
                .Select(g =>
                {
                    var anosIniciais = g.Where(p => p.SerieAno == SerieAnoIndiceDesenvolvimentoEnum.AnosIniciais);
                    var anosFinais = g.Where(p => p.SerieAno == SerieAnoIndiceDesenvolvimentoEnum.AnosFinais);
                    return new PainelEducacionalProficienciaIdepDto
                    {
                        AnoLetivo = g.Key,
                        PercentualInicial = anosIniciais.FirstOrDefault()?.Nota,
                        PercentualFinal = anosFinais.FirstOrDefault()?.Nota,
                        Boletim = g.FirstOrDefault(f => !string.IsNullOrWhiteSpace(f.Boletim))?.Boletim,
                        Proficiencia = new ProficienciaIdepResumidoDto
                        {
                            AnosIniciais = DefinirProficienciaPorComponenteCurricular(anosIniciais),
                            AnosFinais = DefinirProficienciaPorComponenteCurricular(anosFinais)
                        }
                    };
                });

            return resultadoFinal.OrderByDescending(r => r.AnoLetivo);
        }

        private async Task<IEnumerable<PainelEducacionalConsolidacaoProficienciaIdepUe>> ObterProficienciaConsolidadaAsync(int anoLetivo, string codigoUe)
        {
            if (anoLetivo <= 0)
                return await _repositorioProficienciaIdep.ObterConsolidacaoPorAnoVisaoUeAsync(PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE, anoLetivo, codigoUe);

            for (int anoUtilizado = anoLetivo; anoUtilizado >= PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE; anoUtilizado--)
            {
                var proficiencias = await _repositorioProficienciaIdep.ObterConsolidacaoPorAnoVisaoUeAsync(PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE, anoUtilizado, codigoUe);

                if (proficiencias != null && proficiencias.Any())
                    return proficiencias;
            }
            return Array.Empty<PainelEducacionalConsolidacaoProficienciaIdepUe>();
        }

        private List<ComponenteCurricularIdepResumidoDto> DefinirProficienciaPorComponenteCurricular(IEnumerable<PainelEducacionalConsolidacaoProficienciaIdepUe> proficiencias) =>
            new List<ComponenteCurricularIdepResumidoDto>
            {
                new ComponenteCurricularIdepResumidoDto
                {
                    ComponenteCurricular = ComponenteCurricularEnum.Portugues,
                    Percentual = proficiencias.FirstOrDefault(p => p.ComponenteCurricular == ComponenteCurricularEnum.Portugues)?.Proficiencia
                },

                new ComponenteCurricularIdepResumidoDto
                {
                    ComponenteCurricular = ComponenteCurricularEnum.Matematica,
                    Percentual = proficiencias.FirstOrDefault(p => p.ComponenteCurricular == ComponenteCurricularEnum.Matematica)?.Proficiencia
                },

                new ComponenteCurricularIdepResumidoDto
                {
                    ComponenteCurricular = ComponenteCurricularEnum.CienciasNatureza,
                    Percentual = proficiencias.FirstOrDefault(p => p.ComponenteCurricular == ComponenteCurricularEnum.CienciasNatureza)?.Proficiencia
                }
            };
    }
}