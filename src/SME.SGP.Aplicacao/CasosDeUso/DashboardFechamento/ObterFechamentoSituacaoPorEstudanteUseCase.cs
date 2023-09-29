using MediatR;
using Minio.DataModel;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
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

            var alunosFechamentosStatus = new List<FechamentoAlunoStatusDto>();
            var totalDisciplinas = await mediator.Send(new ObterTotalDisciplinasFechamentoPorTurmaQuery(param.AnoLetivo, param.Bimestre));
            foreach (var turma in alunosTabelaConsolidado.GroupBy(a => a.TurmaId))
            {
                var alunosFechamentosStatusTurma = new List<FechamentoAlunoStatusDto>();
                var turmaId = turma.Key;
                var alunos = turma.ToList();
                var totalDisciplinasNaTurma = totalDisciplinas.FirstOrDefault(a => a.TurmaId == turmaId);
                foreach (var aluno in alunos)
                {
                    var alunoFechamento = new FechamentoAlunoStatusDto()
                    {
                        TurmaId = turmaId,
                        Ano = param.UeId == 0 && aluno.TurmaTipo == 2 ? "-88" : aluno.Ano,
                        Modalidade = aluno.TurmaModalidade,
                        TurmaNome = aluno.TurmaNome,
                        AlunoCodigo = aluno.AlunoCodigo,
                        QuantidadeDisciplinasFechadas = aluno.QuantidadeDisciplinaFechadas,
                        Situacao = Dominio.SituacaoFechamentoAluno.SemRegistros
                    };
                    alunosFechamentosStatusTurma.Add(alunoFechamento);
                }

                alunosFechamentosStatus.AddRange(DefinirSituacaoAlunosTurma(alunosFechamentosStatusTurma, totalDisciplinasNaTurma));
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
                    fechamento.AdicionarQuantidadeSemRegistro(await TotalAlunosSemRegistroPorTurma(new long[] { alunoFechamentoStatus.FirstOrDefault().TurmaId }, param.Bimestre, "0", 
                                                                (fechamento.QuantidadeParcial + fechamento.QuantidadeCompleto), param.UeId, param.DreId));

                    if (fechamento.QuantidadeSemRegistro > 0)
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
                    fechamento.AdicionarQuantidadeSemRegistro(await TotalAlunosSemRegistroPorTurma(alunoFechamentoStatus.GroupBy(c => c.TurmaId).Select(a=> a.Key).ToArray(), param.Bimestre, turmaAno,
                                                                                            (fechamento.QuantidadeParcial + fechamento.QuantidadeCompleto), param.UeId, param.DreId));;

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

        private List<FechamentoAlunoStatusDto> DefinirSituacaoAlunosTurma(List<FechamentoAlunoStatusDto> alunoFechamento,
            TurmaFechamentoDisciplinaDto totalDisciplinasNaTurma)
        {
            foreach (FechamentoAlunoStatusDto aluno in alunoFechamento)
            {
                aluno.Situacao = Dominio.SituacaoFechamentoAluno.Parcial;
                if (totalDisciplinasNaTurma.NaoEhNulo())
                {
                    aluno.Situacao = aluno.QuantidadeDisciplinasFechadas == totalDisciplinasNaTurma.QuantidadeDisciplinas ?
                        Dominio.SituacaoFechamentoAluno.Completo :
                        aluno.Situacao;
                }
            }
            return alunoFechamento;
        }

        private async Task<int> TotalAlunosSemRegistroPorTurma(long[] turmaIds, int bimestre, string anoEscolar, int totalParcialOuCompleto, long ueId, long dreId)
        {
            int totalAlunosSemRegistro = 0;
            var turmas = await mediator.Send(new ObterTurmasPorIdsQuery(turmaIds));
            var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turmas.FirstOrDefault(), bimestre));

            if (periodoEscolar.NaoEhNulo())
            {
                if (ueId == 0)
                {
                    if (dreId > 0)
                    {
                        var dadosDre = await mediator.Send(new ObterDREPorIdQuery(dreId));
                        if (dadosDre.NaoEhNulo())
                            dreId = Convert.ToInt64(dadosDre.CodigoDre);
                    }
                      
                    var alunos = await mediator
                                    .Send(new ObterTotalAlunosPorAnoModalidadeDreAnoLetivoInicioEFimQuery(anoEscolar, 
                                    (int)turmas.FirstOrDefault().ModalidadeCodigo, turmas.FirstOrDefault().AnoLetivo, dreId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim));

                    if (alunos > 0)
                        totalAlunosSemRegistro += Convert.ToInt32(alunos);
                    
                }
                else
                {
                    foreach(var turma in turmas)
                    {
                        var alunosDaTurma = await mediator.Send(new ObterTodosAlunosNaTurmaQuery(int.Parse(turma.CodigoTurma)));
                        if (alunosDaTurma.Any() && alunosDaTurma.NaoEhNulo())
                            totalAlunosSemRegistro += RetornaAlunosAtivos(periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim, alunosDaTurma).Count();
                    }
                }

                return totalAlunosSemRegistro > totalParcialOuCompleto
                       ? totalAlunosSemRegistro - totalParcialOuCompleto
                       : 0;
            }

            return 0;

        }

        private IEnumerable<AlunoPorTurmaResposta> RetornaAlunosAtivos(DateTime dataInicio, DateTime dataFim, IEnumerable<AlunoPorTurmaResposta> alunos)
            => from a in alunos
               where a.DataMatricula.Date <= dataFim.Date
               && (!a.Inativo || a.Inativo && a.DataSituacao >= dataInicio.Date)
               group a by new { a.CodigoAluno, a.NumeroAlunoChamada } into grupoAlunos
               orderby grupoAlunos.First().NomeValido(), grupoAlunos.First().NumeroAlunoChamada
               select grupoAlunos.OrderByDescending(a => a.DataSituacao).First();
    }
}