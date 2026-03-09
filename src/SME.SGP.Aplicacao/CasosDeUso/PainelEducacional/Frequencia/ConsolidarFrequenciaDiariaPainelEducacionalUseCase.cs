using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ConsolidacaoFrequenciaDiaria;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ConsolidacaoFrequenciaDiaria.LimparConsolidacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional.Frequencia;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaDiaria;
using SME.SGP.Aplicacao.Queries.UE.ObterTodasUes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Frequencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional.Frequencia
{
    public class ConsolidarFrequenciaDiariaPainelEducacionalUseCase : AbstractUseCase, IConsolidarFrequenciaDiariaPainelEducacionalUseCase
    {
        public ConsolidarFrequenciaDiariaPainelEducacionalUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var listagensDres = await mediator.Send(new ObterTodasDresQuery());
            var listagemUe = await mediator.Send(new ObterTodasUesQuery());

            await mediator.Send(new LimparPainelEducacionalConsolidacaoFrequenciaDiariaCommand());
            await mediator.Send(new LimparPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommand());

            foreach (var dre in listagensDres)
            {
                var listagemFrequencia = await mediator.Send(new ObterFrequenciaDiariaQuery(DateTime.Now.Year, dre.Id));

                var indicadoresTurmas = AgruparConsolicacaoTurmas(listagemFrequencia, listagemUe);

                await mediator.Send(new SalvarPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommand(indicadoresTurmas));

                var indicadoresDre = AgruparConsolicacaoDre(listagemFrequencia, listagemUe);

                await mediator.Send(new SalvarPainelEducacionalConsolidacaoFrequenciaDiariaCommand(indicadoresDre));
            }

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPainelEducacional.ConsolidarFrequenciaSemanalPainelEducacional, Guid.NewGuid(), null));

            return true;
        }

        private static IEnumerable<ConsolidacaoFrequenciaDiariaDreDto> AgruparConsolicacaoDre(IEnumerable<DadosParaConsolidarFrequenciaDiariaAlunoDto> listagemFrequencia,
                                                                                              IEnumerable<Ue> ues)
        {
            var indicadores = listagemFrequencia
                    .GroupBy(x => new { x.UeId, x.DataAula })
                    .Select(g =>
                    {
                        var totalPresentes = g.Sum(x => x.TotalPresentes);
                        var totalAusentes = g.Sum(x => x.TotalAusentes);
                        var totalRemotos = g.Sum(x => x.TotalRemotos);

                        var totalFrequencias = totalPresentes + totalAusentes + totalRemotos;
                        var percentualFrequencia = totalFrequencias == 0
                            ? 0
                            : (decimal)totalPresentes / totalFrequencias * 100;

                        var ue = ues.FirstOrDefault(u => u.Id == g.Key.UeId);

                        return new ConsolidacaoFrequenciaDiariaDreDto
                        {
                            CodigoDre = g.First().CodigoDre,
                            CodigoUe = ue.CodigoUe,
                            Ue = $"{ue.TipoEscola.ObterNomeCurto()} {ue.Nome}",
                            AnoLetivo = g.First().AnoLetivo,
                            DataAula = g.Key.DataAula,
                            TotalEstudantes = totalFrequencias,
                            TotalPresentes = totalPresentes,
                            PercentualFrequencia = percentualFrequencia,
                            NivelFrequencia = ObterNivelFrequencia(percentualFrequencia)
                        };
                    }).DistinctBy(x => new { x.CodigoUe, x.DataAula });

            return indicadores;
        }

        private static IEnumerable<ConsolidacaoFrequenciaDiariaTurmaDto> AgruparConsolicacaoTurmas(IEnumerable<DadosParaConsolidarFrequenciaDiariaAlunoDto> listagemFrequencia,
                                                                                                   IEnumerable<Ue> ues)
        {
            var indicadores = listagemFrequencia
                    .GroupBy(x => new { x.UeId, x.NomeTurma, x.DataAula })
                    .Select(g =>
                    {
                        var totalPresentes = g.Sum(x => x.TotalPresentes);
                        var totalAusentes = g.Sum(x => x.TotalAusentes);
                        var totalRemotos = g.Sum(x => x.TotalRemotos);

                        var totalFrequencias = totalPresentes + totalAusentes + totalRemotos;
                        var percentualFrequencia = totalFrequencias == 0
                            ? 0
                            : (decimal)totalPresentes / totalFrequencias * 100;

                        var ue = ues.FirstOrDefault(u => u.Id == g.Key.UeId);

                        return new ConsolidacaoFrequenciaDiariaTurmaDto
                        {
                            CodigoDre = g.First().CodigoDre,
                            CodigoUe = ue.CodigoUe,
                            TurmaId = g.First().TurmaId,
                            Turma = g.First().NomeTurma,
                            AnoLetivo = g.First().AnoLetivo,
                            DataAula = g.Key.DataAula,
                            TotalEstudantes = totalFrequencias,
                            TotalPresentes = totalPresentes,
                            PercentualFrequencia = percentualFrequencia,
                            NivelFrequencia = ObterNivelFrequencia(percentualFrequencia)
                        };
                    }).DistinctBy(x => new { x.CodigoUe, x.Turma, x.DataAula });

            return indicadores;
        }

        public static NivelFrequenciaEnum ObterNivelFrequencia(decimal percentual) =>
            percentual switch
            {
                < 85 => NivelFrequenciaEnum.Baixa,
                < 90 => NivelFrequenciaEnum.Media,
                _ => NivelFrequenciaEnum.Alta
            };
    }
}
