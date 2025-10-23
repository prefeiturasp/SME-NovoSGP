using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdep;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasProficienciaIdebPainelEducacionalUseCase : IConsultasProficienciaIdebPainelEducacionalUseCase
    {
        private readonly IMediator mediator;

        public ConsultasProficienciaIdebPainelEducacionalUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<PainelEducacionalProficienciaIdepDto>> ObterProficienciaIdep(int anoLetivo, string codigoUe)
        {
            if (string.IsNullOrWhiteSpace(codigoUe))
                throw new NegocioException("Informe a unidade escolar");

            var proficienciaIdeb = await mediator.Send(new ObterProficienciaIdepQuery(anoLetivo, codigoUe));
            var ideps = await mediator.Send(new ObterIdepsQuery(anoLetivo, codigoUe));

            var proficienciaIdebDto = MapearParaDto(proficienciaIdeb, ideps);

            if (anoLetivo <= 0)
            {
                proficienciaIdebDto = proficienciaIdebDto
                    .OrderByDescending(p => p.AnoLetivo)
                    .Take(5)
                    .ToList();
            }

            return proficienciaIdebDto;
        }

        private static IEnumerable<PainelEducacionalProficienciaIdepDto> MapearParaDto(IEnumerable<ProficienciaIdepAgrupadaDto> proficiencia, IEnumerable<Idep> ideps)
        {
            var resultadoFinal = proficiencia
                .GroupBy(d => d.AnoLetivo)
                .Select(group =>
                {
                    var anosIniciais = group.Where(item => item.EtapaEnsino == (int)SerieAnoIdepEnum.AnosIniciais);
                    var anosFinais = group.Where(item => item.EtapaEnsino == (int)SerieAnoIdepEnum.AnosFinais);

                    return new PainelEducacionalProficienciaIdepDto
                    {
                        AnoLetivo = group.Key,
                        PercentualInicial = ObterPercentualIdep(ideps, group.Key, (int)SerieAnoIdepEnum.AnosIniciais),
                        PercentualFinal = ObterPercentualIdep(ideps, group.Key, (int)SerieAnoIdepEnum.AnosFinais),
                        Proficiencia = new ProficienciaIdebResumidoDto
                        {
                            AnosIniciais = anosIniciais
                                .GroupBy(item => item.ComponenteCurricular)
                                .Select(componente => new ComponenteCurricularIdebResumidoDto
                                {
                                    ComponenteCurricular = ((SGP.Dominio.Enumerados.ComponenteCurricular)componente.Key).ObterDisplayName(),
                                    Percentual = Math.Round(componente.Average(item => item.ProficienciaMedia), 2)
                                })
                                .ToList(),
                            AnosFinais = anosFinais
                                .GroupBy(item => item.ComponenteCurricular)
                                .Select(componente => new ComponenteCurricularIdebResumidoDto
                                {
                                    ComponenteCurricular = ((SGP.Dominio.Enumerados.ComponenteCurricular)componente.Key).ObterDisplayName(),
                                    Percentual = Math.Round(componente.Average(item => item.ProficienciaMedia), 2)
                                })
                                .ToList()
                        },
                        Boletim = group.FirstOrDefault(b => !string.IsNullOrEmpty(b.Boletim))?.Boletim
                    };
                })
                .OrderByDescending(d => d.AnoLetivo)
                .ToList();

            return resultadoFinal;
        }

        private static decimal ObterPercentualIdep(IEnumerable<Idep> ideps, int anoLetivo, int serieAno)
        {
            var idep = ideps.FirstOrDefault(i => i.AnoLetivo == anoLetivo && i.SerieAno == serieAno)?.Nota;
            return idep.HasValue ? Math.Round(idep.Value, 2) : 0;
        }
    }
}
