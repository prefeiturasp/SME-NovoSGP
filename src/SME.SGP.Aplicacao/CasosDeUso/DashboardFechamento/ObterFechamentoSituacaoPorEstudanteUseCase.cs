using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoSituacaoPorEstudanteUseCase : AbstractUseCase,
        IObterFechamentoSituacaoPorEstudanteUseCase
    {
        public ObterFechamentoSituacaoPorEstudanteUseCase(IMediator mediator) : base(mediator)
        {
        }


        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroDashboardFechamentoDto param)
        {
            List<GraficoBaseDto> fechamentos = new List<GraficoBaseDto>();

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
                var alunoComFechamentoTurma = alunosComFechamento.Where(a => a.TurmaId == turmaId);
                foreach (var aluno in alunos)
                {
                    var alunoFechamento = new FechamentoAlunoStatusDto()
                    {
                        TurmaId = turmaId,
                        Ano = param.UeId == 0 && aluno.TurmaTipo == 2 ? "-88" : aluno.Ano,
                        Modalidade = aluno.TurmaModalidade,
                        TurmaNome = aluno.TurmaNome,
                        AlunoCodigo = aluno.AlunoCodigo,
                        Situacao = Dominio.SituacaoFechamentoAluno.SemRegistros
                    };

                    var alunoComFechamento = alunoComFechamentoTurma.FirstOrDefault(a => a.AlunoCodigo == aluno.AlunoCodigo);

                    if (alunoComFechamentoTurma.Where(a => a.AlunoCodigo == aluno.AlunoCodigo).Any())
                        alunoFechamento.Situacao = Dominio.SituacaoFechamentoAluno.Parcial;

                    if (alunoComFechamentoTurma.Where(a => a.AlunoCodigo == aluno.AlunoCodigo).Any() && totalDisciplinasNaTurma != null)
                        if (alunoComFechamento.QuantidadeDisciplinaFechadas == totalDisciplinasNaTurma.QuantidadeDisciplinas)
                            alunoFechamento.Situacao = Dominio.SituacaoFechamentoAluno.Completo;

                    alunosFechamentosStatus.Add(alunoFechamento);
                }
            }

            if (param.UeId > 0)
            {
                foreach (var alunoFechamentoStatus in alunosFechamentosStatus.GroupBy(t => t.TurmaNome))
                {
                    var turmaNome = alunoFechamentoStatus.FirstOrDefault().TurmaNome;
                    var turmaModalidade = alunoFechamentoStatus.FirstOrDefault().Modalidade;

                    var fechamento = new FechamentoSituacaoPorEstudanteDto();
                    fechamento.AdicionarQuantidadeCompleto(alunoFechamentoStatus.Where(a => a.Situacao == Dominio.SituacaoFechamentoAluno.Completo).Count());
                    fechamento.AdicionarQuantidadeParcial(alunoFechamentoStatus.Where(a => a.Situacao == Dominio.SituacaoFechamentoAluno.Parcial).Count());
                    fechamento.AdicionarQuantidadeSemRegistro(alunoFechamentoStatus.Where(a => a.Situacao == Dominio.SituacaoFechamentoAluno.SemRegistros).Count());

                    if(fechamento.QuantidadeSemRegistro > 0)
                        fechamentos.Add(new GraficoBaseDto(turmaNome, fechamento.QuantidadeSemRegistro, fechamento.LegendaSemRegistro));
                    if (fechamento.QuantidadeParcial > 0)
                        fechamentos.Add(new GraficoBaseDto(turmaNome, fechamento.QuantidadeParcial, fechamento.LegendaParcial));
                    if (fechamento.QuantidadeCompleto > 0)
                        fechamentos.Add(new GraficoBaseDto(turmaNome, fechamento.QuantidadeCompleto, fechamento.LegendaCompleto));
                }
            }
            else
            {
                foreach (var alunoFechamentoStatus in alunosFechamentosStatus.GroupBy(t => t.Ano))
                {
                    var turmaAno = alunoFechamentoStatus.FirstOrDefault().Ano;
                    var turmaModalidade = alunoFechamentoStatus.FirstOrDefault().Modalidade;

                    var fechamento = new FechamentoSituacaoPorEstudanteDto();
                    fechamento.AdicionarQuantidadeCompleto(alunoFechamentoStatus.Where(a => a.Situacao == Dominio.SituacaoFechamentoAluno.Completo).Count());
                    fechamento.AdicionarQuantidadeParcial(alunoFechamentoStatus.Where(a => a.Situacao == Dominio.SituacaoFechamentoAluno.Parcial).Count());
                    fechamento.AdicionarQuantidadeSemRegistro(alunoFechamentoStatus.Where(a => a.Situacao == Dominio.SituacaoFechamentoAluno.SemRegistros).Count());

                    if (fechamento.QuantidadeSemRegistro > 0)
                        fechamentos.Add(new GraficoBaseDto(turmaAno == "-88" ? "Ed. Física" : turmaAno.ToString(), fechamento.QuantidadeSemRegistro, fechamento.LegendaSemRegistro));
                    if (fechamento.QuantidadeParcial > 0)
                        fechamentos.Add(new GraficoBaseDto(turmaAno == "-88" ? "Ed. Física" : turmaAno.ToString(), fechamento.QuantidadeParcial, fechamento.LegendaParcial));
                    if (fechamento.QuantidadeCompleto > 0)
                        fechamentos.Add(new GraficoBaseDto(turmaAno == "-88" ? "Ed. Física" : turmaAno.ToString(), fechamento.QuantidadeCompleto, fechamento.LegendaCompleto));
                }
            }
            return fechamentos.OrderBy(a => a.Grupo).ToList();
        }
    }
}