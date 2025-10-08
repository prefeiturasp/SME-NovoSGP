using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterTurmasPainelEducacional;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.SituacaoAluno;
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
            const int tamanhoLote = 1000;
            const int situacaoMatriculaAbandono = 2;
            var alunosTurmas = new List<AlunosSituacaoTurmas>();

            var listagemTurmas = await mediator.Send(new ObterTurmasPainelEducacionalQuery(2025));

            if (listagemTurmas == null || !listagemTurmas.Any())
                return false;

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
                    // Envia o batch para o MediatR
                    var alunos = await mediator.Send(new ObterAlunosSituacaoTurmasQuery(lote, 2025, situacaoMatriculaAbandono));
                    alunosTurmas.AddRange(alunos);

                    // Aqui você pode acumular, salvar, ou processar os alunos
                    // Exemplo:
                    // await ProcessarAlunos(alunos);
                }
            }

            return true;
        }
    }
}
