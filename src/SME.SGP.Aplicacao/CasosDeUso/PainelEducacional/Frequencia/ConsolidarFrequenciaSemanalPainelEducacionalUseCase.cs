using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ConsolidacaoFrequenciaSemanal;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional.Frequencia;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaSemanal;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Frequencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional.Frequencia
{
    public class ConsolidarFrequenciaSemanalPainelEducacionalUseCase : AbstractUseCase, IConsolidarFrequenciaSemanalPainelEducacionalUseCase
    {
        public ConsolidarFrequenciaSemanalPainelEducacionalUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var aulas = ObterUltimasDatasSemanais();
            var listagemFrequencia = await mediator.Send(new ObterFrequenciaSemanalQuery(aulas));

            var indicadores = AgruparConsolicacao(listagemFrequencia);

            await mediator.Send(new SalvarPainelEducacionalConsolidacaoFrequenciaSemanalCommand(indicadores));

            return true;
        }

        private static IEnumerable<ConsolidacaoFrequenciaSemanalDto> AgruparConsolicacao(IEnumerable<DadosParaConsolidarFrequenciaSemanalAlunoDto> listagemFrequencia)
        {
            if (listagemFrequencia == null)
                return Enumerable.Empty<ConsolidacaoFrequenciaSemanalDto>();

            var indicadores = listagemFrequencia
                .GroupBy(x => new { x.CodigoUe, x.DataAula })
                .Select(g =>
                {
                    var primeiro = g.First();

                    var totalEstudantes = g.Sum(x => x.TotalEstudantes);
                    var totalPresentes = g.Sum(x => x.TotalPresentes);

                    var percentualFrequencia = totalEstudantes > 0
                        ? (decimal)totalPresentes / totalEstudantes * 100
                        : 0;

                    return new ConsolidacaoFrequenciaSemanalDto
                    {
                        CodigoDre = primeiro.CodigoDre,
                        CodigoUe = g.Key.CodigoUe,
                        AnoLetivo = primeiro.AnoLetivo,
                        DataAula = g.Key.DataAula,
                        TotalEstudantes = totalEstudantes,
                        TotalPresentes  = totalPresentes,
                        PercentualFrequencia = Math.Round(percentualFrequencia, 2)
                    };
                })
                .OrderBy(x => x.CodigoUe)
                .ThenBy(x => x.DataAula);

            return indicadores;
        }


        private static List<DateTime> ObterUltimasDatasSemanais()
        {
            DateTime hoje = DateTime.Today;

            // Descobre a última sexta-feira (antes da semana atual)
            int diasAtras = (7 + (hoje.DayOfWeek - DayOfWeek.Friday)) % 7;
            DateTime ultimaSexta = hoje.AddDays(-diasAtras);

            var semanas = new List<DateTime>();

            for (int i = 0; i < 4; i++)
            {
                semanas.Add(ultimaSexta);

                // Vai para a semana anterior
                ultimaSexta = ultimaSexta.AddDays(-7);
            }

            return semanas;
        }
    }
}
