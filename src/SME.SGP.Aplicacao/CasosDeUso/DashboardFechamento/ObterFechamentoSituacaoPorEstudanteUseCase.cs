using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoSituacaoPorEstudanteUseCase : AbstractUseCase,
        IObterFechamentoSituacaoPorEstudanteUseCase
    {
        public ObterFechamentoSituacaoPorEstudanteUseCase(IMediator mediator) : base(mediator)
        {
        }


        public async Task<IEnumerable<FechamentoSituacaoPorEstudanteDto>> Executar(FiltroDashboardFechamentoDto param)
        {
            var fechamentos = new List<FechamentoSituacaoPorEstudanteDto>();

            var alunosTabelaConsolidado = await mediator.Send(new ObterAlunosTurmaPorDreIdUeIdBimestreSemestreQuery(param.UeId, param.AnoLetivo,
                param.DreId,
                param.Modalidade,
                param.Semestre,
                param.Bimestre));

            var alunosComFechamento = await mediator.Send(new ObterAlunosComFechamentoQuery(param.UeId, param.AnoLetivo,
                param.DreId,
                param.Modalidade,
                param.Semestre,
                param.Bimestre));

            var alunosFechamentosStatus = new List<FechamentoAlunoStatusDto>();
            var totalDisciplinas = await mediator.Send(new ObterTotalDisciplinasFechamentoPorTurmaQuery(param.AnoLetivo, param.Bimestre));
            foreach (var turma in alunosTabelaConsolidado.GroupBy(a => a.TurmaId))
            {
                var turmaId = turma.Key;
                var alunos = turma.ToList();
                var totalDisciplinasNaTurma = totalDisciplinas.FirstOrDefault(a => a.TurmaId == turmaId);
                foreach (var aluno in alunos)
                {
                    var alunoFechamento = new FechamentoAlunoStatusDto()
                    {
                        TurmaId = turmaId,
                        Ano = aluno.Ano,
                        Modalidade = aluno.Modalidade,
                        AlunoCodigo = aluno.AlunoCodigo,
                        Situacao = Dominio.SituacaoFechamentoAluno.SemRegistros
                    };

                    var alunoComFechamento = alunosComFechamento.FirstOrDefault(a => a.AlunoCodigo == aluno.AlunoCodigo && a.TurmaId == turmaId);

                    if(alunoComFechamento != null)
                        alunoFechamento.Situacao = Dominio.SituacaoFechamentoAluno.Parcial;

                    if(alunoComFechamento != null && totalDisciplinasNaTurma != null)
                        if(alunoComFechamento.QuantidadeDisciplinaFechadas == totalDisciplinasNaTurma.QuantidadeDisciplinas)
                            alunoFechamento.Situacao = Dominio.SituacaoFechamentoAluno.Completo;

                    alunosFechamentosStatus.Add(alunoFechamento);
                }
            }

            foreach(var alunoFechamentoStatus in alunosFechamentosStatus.GroupBy(t => t.Ano))
            {
                var turmaAno = alunoFechamentoStatus.FirstOrDefault().Ano;
                var turmaModalidade = alunoFechamentoStatus.FirstOrDefault().Modalidade;

                var fechamento = new FechamentoSituacaoPorEstudanteDto();
                fechamento.AdicionarQuantidadeCompleto(alunoFechamentoStatus.Where(a => a.Situacao == Dominio.SituacaoFechamentoAluno.Completo).Count());
                fechamento.AdicionarQuantidadeParcial(alunoFechamentoStatus.Where(a => a.Situacao == Dominio.SituacaoFechamentoAluno.Parcial).Count());
                fechamento.AdicionarQuantidadeSemRegistro(alunoFechamentoStatus.Where(a => a.Situacao == Dominio.SituacaoFechamentoAluno.SemRegistros).Count());
                fechamento.MontarDescricao(turmaModalidade.ShortName(), turmaAno);

                fechamentos.Add(fechamento);
            }

            return fechamentos;
        }
    }
}