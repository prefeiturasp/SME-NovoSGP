using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoAbandono;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterTurmasPainelEducacional;
using SME.SGP.Dominio;
using SME.SGP.Infra;
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
            int anoUtilizado = DateTime.Now.Year;
            int anoMinimoConsulta = 2019;
            var indicadoresDre = new List<ConsolidacaoAbandonoDto>();

            while (anoUtilizado >= anoMinimoConsulta)
            {
                indicadoresDre.AddRange(await ObterConsolidacaoAlunosTurmas(anoUtilizado));

                anoUtilizado--;
            }

            if (indicadoresDre == null || !indicadoresDre.Any())
                return false;

            await mediator.Send(new SalvarPainelEducacionalConsolidacaoAbandonoCommand(indicadoresDre));

            return true;
        }

        private async Task<IEnumerable<ConsolidacaoAbandonoDto>> ObterConsolidacaoAlunosTurmas(int anoUtilizado)
        {
            const int situacaoMatriculaAbandono = 2;
            var indicadoresDre = new List<ConsolidacaoAbandonoDto>();

            var listagemTurmas = await mediator.Send(new ObterTurmasPainelEducacionalQuery(anoUtilizado));

            if (listagemTurmas == null || !listagemTurmas.Any())
                return indicadoresDre;

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

                indicadoresDre.AddRange(AgruparConsolicacaoTurmasSerieAno(alunos, listagemTurmas));
                // TODO: agrupar popr turmas
            }

            return indicadoresDre;
        }

        private static IEnumerable<ConsolidacaoAbandonoDto> AgruparConsolicacaoTurmasSerieAno(IEnumerable<AlunosSituacaoTurmas> alunosAbandonoTurmas, IEnumerable<TurmaPainelEducacionalDto> listagemTurma)
        {
            var turmasDistintas = alunosAbandonoTurmas?.Select(x => x.CodigoTurma)?.Distinct()?.ToList();
            var turmas = listagemTurma?.Where(x => turmasDistintas.Contains(x.TurmaId))?.ToList();

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
