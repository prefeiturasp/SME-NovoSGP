using MediatR;
using SME.Pedagogico.Interface.DTO.Turma;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoDistorcaoIdade;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDistorcaoIdadeUltimoAnoConsolidado;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterTurmasPainelEducacional;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoDistorcaoIdade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarDistorcaoIdadePainelEducacionalUseCase : AbstractUseCase, IConsolidarDistorcaoIdadePainelEducacionalUseCase
    {
        public ConsolidarDistorcaoIdadePainelEducacionalUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var anoInicioConsolidacao = await DeterminarAnoInicioConsolidacao();
            var indicadores = new List<ConsolidacaoDistorcaoIdadeDto>();

            for (int anoUtilizado = anoInicioConsolidacao; anoUtilizado <= DateTime.Now.Year; anoUtilizado++)
            {
                var indicadoresTemp = await ObterConsolidacaoAlunosTurmas(anoUtilizado);
                indicadores.AddRange(indicadoresTemp);
            }

            if (indicadores == null || !indicadores.Any())
                return false;

            await mediator.Send(new SalvarPainelEducacionalConsolidacaoDistorcaoIdadeCommand(indicadores));

            return true;
        }

        private async Task<int> DeterminarAnoInicioConsolidacao()
        {
            var ultimoAnoConsolidado = await mediator.Send(new ObterDistorcaoIdadeUltimoAnoConsolidadoQuery());
            if (ultimoAnoConsolidado == 0)
                return PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE;
            if (ultimoAnoConsolidado == DateTime.Now.Year)
                return DateTime.Now.Year;
            return ultimoAnoConsolidado + 1;
        }

        private async Task<IEnumerable<ConsolidacaoDistorcaoIdadeDto>> ObterConsolidacaoAlunosTurmas(int anoUtilizado)
        {
            const int situacaoMatriculaAtiva = (int)SituacaoMatriculaAluno.Ativo;
            var indicadores = new List<ConsolidacaoDistorcaoIdadeDto>();

            var listagemTurmas = await mediator.Send(new ObterTurmasPainelEducacionalQuery(anoUtilizado));

            if (listagemTurmas == null || !listagemTurmas.Any())
                return indicadores;

            var turmasAgrupadasDre = listagemTurmas
                .Where(t => t.ModalidadeCodigo == (int)Modalidade.Fundamental
                || t.ModalidadeCodigo == (int)Modalidade.Medio
                   && int.TryParse(t.Ano, out var anoEnsinoMedio) && anoEnsinoMedio >= 1 && anoEnsinoMedio <= 3)
                .Where(t => !string.IsNullOrWhiteSpace(t.Ano) && t.Ano.All(char.IsDigit))
                .GroupBy(t => t.CodigoDre)
                .Select(g => new DreTurmaPainelEducacionalDistorcaoIdadeDto
                {
                    CodigoDre = g.Key,
                    Turmas = g.ToList(),
                })
                .ToList();

            foreach (var dreTurmas in turmasAgrupadasDre)
            {

                var alunos = await mediator.Send(new ObterMatriculaTurmaEscolaAlunoQuery(
                    anoUtilizado,
                    dreTurmas.CodigoDre,
                    situacaoMatriculaAtiva,
                    dreTurmas.Turmas.Select(t => t.TurmaId).ToArray()));

                await Task.Delay(1000);

                if (alunos == null || !alunos.Any())
                    continue;

                var turmasDistintas = ObterTurmasDistintas(alunos, dreTurmas.Turmas);
                indicadores.AddRange(AgruparConsolicacaoTurmas(alunos, turmasDistintas));
            }

            return indicadores;
        }

        private static IEnumerable<TurmaPainelEducacionalDto> ObterTurmasDistintas(IEnumerable<AlunoMatriculaTurmaEscolaDto> alunosAtivosTurmas, IEnumerable<TurmaPainelEducacionalDto> listagemTurma)
        {
            var turmasDistintas = alunosAtivosTurmas?.Select(x => x.CodigoTurma)?.Distinct()?.ToList();
            var turmas = listagemTurma?.Where(x => turmasDistintas.Contains(x.TurmaId))?.ToList();
            return turmas;
        }

        private static IEnumerable<ConsolidacaoDistorcaoIdadeDto> AgruparConsolicacaoTurmas(IEnumerable<AlunoMatriculaTurmaEscolaDto> alunosAtivosTurmas, IEnumerable<TurmaPainelEducacionalDto> turmas)
        {
            var indicadores = turmas
                .Join(alunosAtivosTurmas,
                      t => t.TurmaId,
                      a => a.CodigoTurma,
                      (t, a) => new { t, a })
                .Where(x => PossuiDistorcaoIdade(x.a, x.t))
                .GroupBy(x => new { x.t.CodigoDre, x.t.CodigoUe, x.t.Ano, x.t.ModalidadeCodigo, x.t.AnoLetivo })
                .Select(g => new ConsolidacaoDistorcaoIdadeDto
                {
                    CodigoDre = g.Key.CodigoDre,
                    CodigoUe = g.Key.CodigoUe,
                    Modalidade = ObterNomeModalidade(g.Key.ModalidadeCodigo, null),
                    Ano = g.Key.Ano,
                    QuantidadeAlunos = g.Count(),
                    AnoLetivo = g.Key.AnoLetivo,
                })
                .ToList();

            return indicadores;
        }

        private static bool PossuiDistorcaoIdade(AlunoMatriculaTurmaEscolaDto aluno, TurmaPainelEducacionalDto turma)
        {
            if (aluno.DataNascimentoAluno == DateTime.MinValue)
                return false;

            int idadeAluno = turma.AnoLetivo - aluno.DataNascimentoAluno.Year;
            int idadeEsperada = IdadeEsperadaParaAnoLetivo(turma.ModalidadeCodigo, int.Parse(turma.Ano));

            return idadeAluno - idadeEsperada >= 2;
        }

        private static int IdadeEsperadaParaAnoLetivo(int modalidade, int ano)
        {
            if (modalidade == (int)Modalidade.Fundamental)
            {
                switch (ano)
                {
                    case 1: return 6;
                    case 2: return 7;
                    case 3: return 8;
                    case 4: return 9;
                    case 5: return 10;
                    case 6: return 11;
                    case 7: return 12;
                    case 8: return 13;
                    case 9: return 14;
                }
            }

            if (modalidade == (int)Modalidade.Medio)
            {
                switch (ano)
                {
                    case 1: return 15;
                    case 2: return 16;
                    case 3: return 17;
                }
            }
            return 0;
        }

        private static string ObterNomeModalidade(int modalidadeCodigo, string anoTurma)
        {
            return ((Modalidade)modalidadeCodigo).ObterDisplayName();
        }
    }
}
