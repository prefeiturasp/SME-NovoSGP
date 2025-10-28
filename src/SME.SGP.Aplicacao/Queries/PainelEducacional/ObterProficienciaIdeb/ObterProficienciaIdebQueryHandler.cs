using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.PainelEducacional.ProficienciaIdeb;
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
            var proficiencia = await _repositorioProficienciaIdeb.ObterConsolidacaoPorAnoVisaoUeAsync(PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE, request.AnoLetivo, request.CodigoUe);
            var resultadoFinal = proficiencia
                .GroupBy(d => new { d.AnoLetivo, d.CodigoUe })
                .Select(group =>
                {
                    var anosIniciais = group.Where(item => item.SerieAno == SerieAnoIndiceDesenvolvimentoEnum.AnosIniciais);
                    var anosFinais = group.Where(item => item.SerieAno == SerieAnoIndiceDesenvolvimentoEnum.AnosFinais);
                    var ensinoMedio = group.Where(item => item.SerieAno == SerieAnoIndiceDesenvolvimentoEnum.EnsinoMedio);
                    return new PainelEducacionalProficienciaIdebDto
                    {
                        AnoLetivo = group.Key.AnoLetivo,
                        Proficiencia = new ProficienciaIdebResumidoDto
                        {
                            AnosIniciais = group
                                .Where(item => item.SerieAno == SerieAnoIndiceDesenvolvimentoEnum.AnosIniciais)
                                .GroupBy(item => item.ComponenteCurricular)
                                .Select(componente => new ComponenteCurricularIdebResumidoDto
                                {
                                    ComponenteCurricular = componente.Key.ToString(),
                                    Percentual = componente.Average(item => item.Proficiencia)
                                })
                                .ToList(),
                            AnosFinais = group
                                .Where(item => item.SerieAno == SerieAnoIndiceDesenvolvimentoEnum.AnosFinais)
                                .GroupBy(item => item.ComponenteCurricular)
                                .Select(componente => new ComponenteCurricularIdebResumidoDto
                                {
                                    ComponenteCurricular = componente.Key.ToString(),
                                    Percentual = componente.Average(item => item.Proficiencia)
                                })
                                .ToList(),
                            EnsinoMedio = group
                                .Where(item => item.SerieAno == SerieAnoIndiceDesenvolvimentoEnum.EnsinoMedio)
                                .GroupBy(item => item.ComponenteCurricular)
                                .Select(componente => new ComponenteCurricularIdebResumidoDto
                                {
                                    ComponenteCurricular = componente.Key.ToString(),
                                    Percentual = componente.Average(item => item.Proficiencia)
                                })
                                .ToList()
                        }
                    };
                })
                .OrderByDescending(d => d.AnoLetivo)
                .ToList();

            return resultadoFinal;
        }
    }
}
