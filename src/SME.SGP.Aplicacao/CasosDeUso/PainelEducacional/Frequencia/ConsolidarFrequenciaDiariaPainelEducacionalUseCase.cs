using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ConsolidacaoFrequenciaDiaria;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional.Frequencia;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaDiaria;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterTurmasPainelEducacional;
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

            var listagemFrequencia = await mediator.Send(new ObterFrequenciaDiariaQuery(2024));

            var indicadoresTurmas = AgruparConsolicacaoTurmas(listagemFrequencia);

            await mediator.Send(new SalvarPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommand(indicadoresTurmas));

            //var frequencia = new List<RegistroFrequenciaPorDisciplinaAlunoDto>();
            //var listagemDre = await mediator.Send(new ObterTodasDresQuery());
            //foreach (var dre in listagemDre)
            //{
            //    var listagemUe = await mediator.Send(new ObterUesCodigosPorDreQuery(dre.Id));
            //    var listagemTurmas = await mediator.Send(new ObterTurmasPainelEducacionalQuery(DateTime.Now.Year));
            //    foreach (var ue in listagemUe)
            //    {
            //        var turmasDaUe = listagemTurmas.Where(t => t.CodigoUe == ue).ToList();

            //        var turmasFiltro = turmasDaUe.Select(x => x.TurmaId).ToList();

            //        turmasFiltro.Add("2853538");
            //        //frequencia.AddRange(await mediator.Send(new ObterFrequenciaDiariaQuery(turmasFiltro)));
            //    }

            //}

            return true;
        }

        private static IEnumerable<ConsolidacaoFrequenciaDiariaTurmaDto> AgruparConsolicacaoTurmas(IEnumerable<DadosParaConsolidarFrequenciaDiariaAlunoDto> listagemFrequencia)
        {
            var indicadores = listagemFrequencia
                    .GroupBy(x => new { x.TurmaId, x.DataAula })
                    .Select(g =>
                    {
                        var totalPresentes = g.Sum(x => x.TotalPresentes);
                        var totalAusentes = g.Sum(x => x.TotalAusentes);
                        var totalRemotos = g.Sum(x => x.TotalRemotos);

                        var totalFrequencias = totalPresentes + totalAusentes + totalRemotos;
                        var percentualFrequencia = totalFrequencias == 0
                            ? 0
                            : (decimal)totalPresentes / totalFrequencias * 100;

                        return new ConsolidacaoFrequenciaDiariaTurmaDto
                        {
                            TurmaId = g.Key.TurmaId,
                            Turma = g.First().NomeTurma,
                            AnoLetivo = g.First().AnoLetivo,
                            DataAula = g.Key.DataAula,
                            TotalEstudantes = totalFrequencias,
                            TotalPresentes = totalPresentes,
                            PercentualFrequencia = percentualFrequencia,
                            NivelFrequencia = ObterNivelFrequencia(percentualFrequencia)
                        };
                    }).OrderBy(x => x.Turma);

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
