using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
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
        private readonly IRepositorioPainelEducacionalProficienciaIdep repositorioProficienciaIdeb;
        public ObterProficienciaIdepQueryHandler(IRepositorioPainelEducacionalProficienciaIdep repositorioIdebConsulta)
        {
            this.repositorioProficienciaIdeb = repositorioIdebConsulta ?? throw new ArgumentNullException(nameof(repositorioIdebConsulta));
        }
        public async Task<IEnumerable<PainelEducacionalProficienciaIdepDto>> Handle(ObterProficienciaIdepQuery request, CancellationToken cancellationToken)
        {
            var proficiencia = await repositorioProficienciaIdeb.ObterProficienciaIdep(request.AnoLetivo, request.CodigoUe);

            var resultadoFinal = proficiencia
                .GroupBy(d => d.AnoLetivo)
                .Select(group =>
                {
                    var anosIniciais = group.Where(item => item.EtapaEnsino == 1);
                    var anosFinais = group.Where(item => item.EtapaEnsino == 2);

                    var mediaProficienciaIniciais = anosIniciais.Any()
                        ? anosIniciais.Average(item => item.ProficienciaMedia)
                        : 0;

                    var mediaProficienciaFinais = anosFinais.Any()
                        ? anosFinais.Average(item => item.ProficienciaMedia)
                        : 0;

                    return new PainelEducacionalProficienciaIdepDto
                    {
                        AnoLetivo = group.Key,
                        PercentualInicial = Math.Round(mediaProficienciaIniciais, 2),
                        PercentualFinal = Math.Round(mediaProficienciaFinais, 2),
                        Proficiencia = new ProficienciaIdebResumidoDto
                        {
                            AnosIniciais = anosIniciais
                                .GroupBy(item => item.ComponenteCurricular)
                                .Select(componente => new ComponenteCurricularIdebResumidoDto
                                {
                                    ComponenteCurricular = ((ComponenteCurricular)componente.Key).ObterDisplayName(),
                                    Percentual = Math.Round(componente.Average(item => item.ProficienciaMedia), 2)
                                })
                                .ToList(),
                            AnosFinais = anosFinais
                                .GroupBy(item => item.ComponenteCurricular)
                                .Select(componente => new ComponenteCurricularIdebResumidoDto
                                {
                                    ComponenteCurricular = ((ComponenteCurricular)componente.Key).ObterDisplayName(),
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
    }
}
