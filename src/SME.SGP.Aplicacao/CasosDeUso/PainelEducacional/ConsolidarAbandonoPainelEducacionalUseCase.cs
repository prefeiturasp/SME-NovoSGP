using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoAbandono;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAbandonoUltimoAnoConsolidado;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterTurmasPainelEducacional;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoAbandono;
using SME.SGP.Infra.Dtos.SituacaoAluno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarAbandonoPainelEducacionalUseCase : AbstractUseCase, IConsolidarAbandonoPainelEducacionalUseCase
    {
        public ConsolidarAbandonoPainelEducacionalUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var anoInicioConsolidacao = await DeterminarAnoInicioConsolidacao();
            var indicadoresDre = new List<ConsolidacaoAbandonoDto>();
            var indicadoresUe = new List<ConsolidacaoAbandonoUeDto>();

            for (int anoUtilizado = anoInicioConsolidacao; anoUtilizado <= DateTime.Now.Year; anoUtilizado++)
            {
                var (indicadoresDreTemp, indicadoresUeTemp) = await ObterConsolidacaoAlunosTurmas(anoUtilizado);
                indicadoresUe.AddRange(indicadoresUeTemp);
                indicadoresDre.AddRange(indicadoresDreTemp);
            }

            if (indicadoresDre == null || !indicadoresDre.Any())
                return false;

            await mediator.Send(new SalvarPainelEducacionalConsolidacaoAbandonoCommand(indicadoresDre, indicadoresUe));

            return true;
        }

        private async Task<int> DeterminarAnoInicioConsolidacao()
        {
            var ultimoAnoConsolidado = await mediator.Send(new ObterAbandonoUltimoAnoConsolidadoQuery());
            if (ultimoAnoConsolidado == 0)
                return PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE;
            if (ultimoAnoConsolidado == DateTime.Now.Year)
                return DateTime.Now.Year;
            return ultimoAnoConsolidado + 1;
        }

        private async Task<(IEnumerable<ConsolidacaoAbandonoDto> indicadoresDre,
                            IEnumerable<ConsolidacaoAbandonoUeDto> indicadoresUe)> 
            ObterConsolidacaoAlunosTurmas(int anoUtilizado)
        {
            const int situacaoMatriculaAbandono = (int)SituacaoMatriculaAluno.Desistente;
            var indicadoresDre = new List<ConsolidacaoAbandonoDto>();
            var indicadoresUe = new List<ConsolidacaoAbandonoUeDto>();

            var listagemTurmas = await mediator.Send(new ObterTurmasPainelEducacionalQuery(anoUtilizado));

            if (listagemTurmas == null || !listagemTurmas.Any())
                return (indicadoresDre, indicadoresUe);

            var turmasAgrupadasDre = listagemTurmas
                    .GroupBy(t => t.CodigoDre)
                    .Select(g => new DreTurmaPainelEducacionalDto
                    {
                        CodigoDre = g.Key,
                        Turmas = g.ToList()
                    })
                    .ToList();

            foreach (var dreTurmas in turmasAgrupadasDre)
            {
                var alunos = await mediator.Send(new ObterAlunosSituacaoTurmasQuery(anoUtilizado, situacaoMatriculaAbandono, dreTurmas.CodigoDre));

                if (alunos == null || !alunos.Any())
                    continue;

                var turmasDistintas = ObterTurmasDistintas(alunos, dreTurmas.Turmas);

                indicadoresDre.AddRange(AgruparConsolicacaoTurmasSerieAno(alunos, turmasDistintas));
                indicadoresUe.AddRange(AgruparConsolicacaoTurmasUe(alunos, turmasDistintas));
            }

            return (indicadoresDre, indicadoresUe);
        }

        private static IEnumerable<TurmaPainelEducacionalDto> ObterTurmasDistintas(IEnumerable<AlunosSituacaoTurmas> alunosAbandonoTurmas, IEnumerable<TurmaPainelEducacionalDto> listagemTurma)
        {
            var turmasDistintas = alunosAbandonoTurmas?.Select(x => x.CodigoTurma)?.Distinct()?.ToList();
            var turmas = listagemTurma?.Where(x => turmasDistintas.Contains(x.TurmaId))?.ToList();
            return turmas;
        }

        private static IEnumerable<ConsolidacaoAbandonoDto> AgruparConsolicacaoTurmasSerieAno(IEnumerable<AlunosSituacaoTurmas> alunosAbandonoTurmas, IEnumerable<TurmaPainelEducacionalDto> turmas)
        {
            var indicadores = turmas
                    .Join(alunosAbandonoTurmas,
                          t => t.TurmaId,
                          a => a.CodigoTurma,
                          (t, a) => new { t, a })
                    .GroupBy(x => new { x.t.CodigoDre, x.t.Ano, x.t.ModalidadeCodigo, x.t.AnoLetivo })
                    .Select(g => new ConsolidacaoAbandonoDto
                    {
                        CodigoDre = g.Key.CodigoDre,
                        Ano = g.Key.Ano,
                        Modalidade = ObterNomeModalidade(g.Key.ModalidadeCodigo, g.Key.Ano),
                        AnoLetivo = g.Key.AnoLetivo,
                        QuantidadeDesistencias = g.Sum(x => x.a.QuantidadeAlunos)
                    })
                    .ToList();

            return indicadores;
        }

        private static IEnumerable<ConsolidacaoAbandonoUeDto> AgruparConsolicacaoTurmasUe(IEnumerable<AlunosSituacaoTurmas> alunosAbandonoTurmas, IEnumerable<TurmaPainelEducacionalDto> turmas)
        {
            var indicadores = turmas
                    .Join(alunosAbandonoTurmas,
                          t => t.TurmaId,
                          a => a.CodigoTurma,
                          (t, a) => new { t, a })
                    .GroupBy(x => new { x.t.CodigoDre, x.t.CodigoUe, x.t.TurmaId, x.t.Nome, x.t.ModalidadeCodigo, x.t.AnoLetivo })
                    .Select(g => new ConsolidacaoAbandonoUeDto
                    {
                        CodigoDre = g.Key.CodigoDre,
                        CodigoUe = g.Key.CodigoUe,
                        CodigoTurma = g.Key.TurmaId,
                        NomeTurma = g.Key.Nome,
                        Modalidade = ObterNomeModalidade(g.Key.ModalidadeCodigo, null),
                        AnoLetivo = g.Key.AnoLetivo,
                        QuantidadeDesistencias = g.Sum(x => x.a.QuantidadeAlunos)
                    })
                    .ToList();
            return indicadores;
        }

        private static string ObterNomeModalidade(int modalidadeCodigo, string anoTurma)
        {
            if (modalidadeCodigo == (int)Modalidade.EducacaoInfantil)
            {
                if (!string.IsNullOrWhiteSpace(anoTurma) && int.TryParse(anoTurma, out var ano))
                {
                    if (ano >= 1 && ano <= 4)
                        return "Creche";

                    if (ano >= 5 && ano <= 7)
                        return "Pré-Escola";
                }
            }

            return ((Modalidade)modalidadeCodigo).ObterDisplayName();
        }
    }
}
