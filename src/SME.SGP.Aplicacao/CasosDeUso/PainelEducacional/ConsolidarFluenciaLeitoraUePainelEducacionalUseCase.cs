using MediatR;
using SME.Pedagogico.Interface.DTO.Turma;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoFluenciaLeitoraUe;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFluenciaLeitoraUe;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterTurmasPainelEducacional;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoFluenciaLeitoraUe;
using SME.SGP.Infra.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarFluenciaLeitoraUePainelEducacionalUseCase : AbstractUseCase, IConsolidarFluenciaLeitoraUePainelEducacionalUseCase
    {
        public ConsolidarFluenciaLeitoraUePainelEducacionalUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var mensagem = param.ObterObjetoMensagem<MensagemConsolidacaoFluenciaLeitoraUeDto>();

            if (mensagem.AnoLetivo > 0)
            {
                var indicadores = new List<ConsolidacaoFluenciaLeitoraUeDto>();

                var indicadoresTemp = await ObterConsolidacaoAlunosTurmas(mensagem.AnoLetivo);
                indicadores.AddRange(indicadoresTemp);

                if (indicadores == null || !indicadores.Any())
                    return false;

                await mediator.Send(new SalvarPainelEducacionalConsolidacaoFluenciaLeitoraUeCommand(mensagem.AnoLetivo, indicadores));
            }

            return true;
        }


        private async Task<IEnumerable<ConsolidacaoFluenciaLeitoraUeDto>> ObterConsolidacaoAlunosTurmas(int anoUtilizado)
        {
            var indicadores = new List<ConsolidacaoFluenciaLeitoraUeDto>();

            var dadosFluencia = await mediator.Send(
                new ObterDadosParaConsolidarFluenciaLeitoraUePainelEducacionalQuery(anoUtilizado)
            );

            if (dadosFluencia == null || !dadosFluencia.Any())
                return indicadores;

            var listagemTurmas = await mediator.Send(new ObterTurmasPainelEducacionalQuery(anoUtilizado));

            if (listagemTurmas == null || !listagemTurmas.Any())
                return indicadores;

            var turmasAgrupadasDre = listagemTurmas
                .Where(t => t.ModalidadeCodigo == (int)Modalidade.Fundamental && int.TryParse(t.Ano, out var anoTurma) && anoTurma == 2)
                .Where(t => !string.IsNullOrWhiteSpace(t.Ano) && t.Ano.All(char.IsDigit))
                .GroupBy(t => t.CodigoDre)
                .Select(g => new DreTurmasFluenciaLeitoraUePainelEducacionalDto
                {
                    CodigoDre = g.Key,
                    Turmas = g.ToList(),
                })
                .ToList();

            foreach (var dreTurmas in turmasAgrupadasDre)
            {
                var codigosTurmas = dreTurmas.Turmas.Select(t => t.TurmaId).ToArray();

                var alunos = await mediator.Send(new ObterMatriculaTurmaEscolaAlunoQuery(
                    anoUtilizado,
                    dreTurmas.CodigoDre,
                    (int)SituacaoMatriculaAluno.Ativo,
                    codigosTurmas
                ));

                if (alunos == null || !alunos.Any())
                    continue;

                foreach (var turma in dreTurmas.Turmas)
                {
                    var codigoTurma = turma.TurmaId;

                    var totalPrevistos = alunos.Count(a => a.CodigoTurma == codigoTurma);

                    var totalAvaliados = dadosFluencia.Count(f => f.CodigoTurma == codigoTurma);

                    var turmasDistintas = ObterTurmasDistintas(alunos, dreTurmas.Turmas);
                    indicadores.AddRange(AgruparConsolicacaoTurmas(alunos, turmasDistintas, dadosFluencia));
                }
            }

            return indicadores;
        }

        private static IEnumerable<TurmaPainelEducacionalDto> ObterTurmasDistintas(
            IEnumerable<AlunoMatriculaTurmaEscolaDto> alunosAtivosTurmas,
            IEnumerable<TurmaPainelEducacionalDto> listagemTurma)
        {
            var turmasDistintas = alunosAtivosTurmas?
                .Select(x => x.CodigoTurma)?
                .Distinct()?
                .ToList();

            return listagemTurma?
                .Where(x => turmasDistintas.Contains(x.TurmaId))
                .ToList();
        }

        private static IEnumerable<ConsolidacaoFluenciaLeitoraUeDto> AgruparConsolicacaoTurmas(
            IEnumerable<AlunoMatriculaTurmaEscolaDto> alunosAtivosTurmas,
            IEnumerable<TurmaPainelEducacionalDto> turmas,
            IEnumerable<ConsolidacaoFluenciaLeitoraUeDto> dadosFluencia
        )
        {
            var indicadores = new List<ConsolidacaoFluenciaLeitoraUeDto>();

            foreach (var g in turmas
                .Join(alunosAtivosTurmas, t => t.TurmaId, a => a.CodigoTurma, (t, a) => new { t, a })
                .GroupBy(x => new { x.t.CodigoDre, x.t.CodigoUe, x.t.AnoLetivo, x.t.TurmaId, x.t.Nome }))
            {
                var codigoTurma = g.Key.TurmaId;
                var alunosPrevistos = g.Count();

                var tiposAvaliacao = dadosFluencia
                    .Where(f => f.CodigoTurma == codigoTurma)
                    .Select(f => f.TipoAvaliacao)
                    .Union(new[] { 
                        (int)FluenciaLeitoraTipoAvaliacaoEnum.AvaliacaoEntrada, 
                        (int)FluenciaLeitoraTipoAvaliacaoEnum.AvaliacaoSaida 
                    })
                    .Distinct()
                    .ToList();

                foreach (var tipoAvaliacao in tiposAvaliacao)
                {
                    var registrosAvaliacao = dadosFluencia
                        .Where(f => f.CodigoTurma == codigoTurma && f.TipoAvaliacao == tipoAvaliacao)
                        .ToList();

                    var alunosAvaliados = registrosAvaliacao.Count();

                    var contagemPorNivel = registrosAvaliacao
                        .GroupBy(f => (int)f.Fluencia)
                        .ToDictionary(k => k.Key, v => v.Count());

                    int n1 = contagemPorNivel.GetValueOrDefault(1, 0);
                    int n2 = contagemPorNivel.GetValueOrDefault(2, 0);
                    int n3 = contagemPorNivel.GetValueOrDefault(3, 0);
                    int n4 = contagemPorNivel.GetValueOrDefault(4, 0);
                    int n5 = contagemPorNivel.GetValueOrDefault(5, 0);
                    int n6 = contagemPorNivel.GetValueOrDefault(6, 0);

                    var preLeitorTotal = n1 + n2 + n3 + n4;

                    for (int nivel = 1; nivel <= 6; nivel++)
                    {
                        var quantidadeNivel = contagemPorNivel.GetValueOrDefault(nivel, 0);
                        var percentualFluencia = alunosAvaliados > 0
                            ? Math.Round((decimal)quantidadeNivel * 100m / alunosAvaliados, 2, MidpointRounding.AwayFromZero)
                            : 0m;

                        indicadores.Add(new ConsolidacaoFluenciaLeitoraUeDto
                        {
                            AnoLetivo = g.Key.AnoLetivo,
                            CodigoDre = g.Key.CodigoDre,
                            CodigoUe = g.Key.CodigoUe,
                            CodigoTurma = g.Key.TurmaId,
                            Turma = g.Key.Nome,
                            AlunosPrevistos = alunosPrevistos,
                            AlunosAvaliados = alunosAvaliados,
                            TipoAvaliacao = tipoAvaliacao,
                            PreLeitorTotal = preLeitorTotal,
                            Fluencia = (NivelFluenciaLeitoraEnum)nivel,
                            QuantidadeAlunoFluencia = quantidadeNivel,
                            PercentualFluencia = percentualFluencia
                        });
                    }
                }

            }
            return indicadores;
        }

    }
}
