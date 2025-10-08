using MediatR;
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
            var indicadores = new List<ConsolidacaoAbandonoDto>();

            while (anoUtilizado >= anoMinimoConsulta)
            {
                indicadores.AddRange(await ObterConsolidacaoAlunosTurmas(anoUtilizado));

                anoUtilizado--;
            }

            if (indicadores == null || !indicadores.Any())
                return false;



            return true;
        }

        private async Task<IEnumerable<ConsolidacaoAbandonoDto>> ObterConsolidacaoAlunosTurmas(int anoUtilizado)
        {
            const int tamanhoLote = 1000;
            const int situacaoMatriculaAbandono = 2;
            var alunosTurmas = new List<AlunosSituacaoTurmas>();
            var indicadores = new List<ConsolidacaoAbandonoDto>();

            var listagemTurmas = await mediator.Send(new ObterTurmasPainelEducacionalQuery(anoUtilizado));

            if (listagemTurmas == null || !listagemTurmas.Any())
                return indicadores;

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
                var turmasIds = dreTurmas?.Turmas?.Select(t => t.TurmaId)?.ToList() ?? new List<string>();

                // Divide as turmas em lotes de 1000
                var loteTurmas = turmasIds
                    .Select((codigo, index) => new { codigo, index })
                    .GroupBy(x => x.index / tamanhoLote, x => x.codigo)
                    .Select(g => g.ToList())
                    .ToList();

                // Processa cada lote separadamente
                foreach (var lote in loteTurmas)
                {
                    // Envia o lote para o MediatR
                    var alunos = await mediator.Send(new ObterAlunosSituacaoTurmasQuery(lote, anoUtilizado, situacaoMatriculaAbandono));
                    alunosTurmas.AddRange(alunos);
                }

                indicadores.AddRange(AgruparConsolicacaoTurmas(alunosTurmas, listagemTurmas));
            }

            return indicadores;
        }

        private static IEnumerable<ConsolidacaoAbandonoDto> AgruparConsolicacaoTurmas(IEnumerable<AlunosSituacaoTurmas> alunosAbandonoTurmas, IEnumerable<TurmaPainelEducacionalDto> listagemTurma)
        {
            var indicadores = listagemTurma
                    .Join(alunosAbandonoTurmas,
                          t => t.TurmaId,
                          a => a.CodigoTurma,
                          (t, a) => new { t, a })
                    .GroupBy(x => new { x.t.CodigoDre, x.t.CodigoUe, x.t.Ano, x.t.ModalidadeCodigo, x.t.AnoLetivo })
                    .Select(g => new ConsolidacaoAbandonoDto
                    {
                        CodigoDre = g.Key.CodigoDre,
                        CodigoUe = g.Key.CodigoUe,
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
