using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardFrequenciaPorAnoTurmaUseCase : AbstractUseCase, IObterDadosDashboardFrequenciaPorAnoTurmaUseCase
    {
        public ObterDadosDashboardFrequenciaPorAnoTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<GraficoFrequenciaAlunoDto> Executar(int anoLetivo, long dreId, long ueId, int modalidade, int semestre, int anoTurma, DateTime dataInicio, DateTime datafim, int mes, int tipoPeriodoDashboard)
        {
            var dadosFrequenciaAlunos = await mediator.Send(new ObterDadosDashboardFrequenciaPorAnoTurmaQuery(anoLetivo,
                                                                                                              dreId,
                                                                                                              ueId,
                                                                                                              modalidade,
                                                                                                              semestre,
                                                                                                              anoTurma,
                                                                                                              dataInicio,
                                                                                                              datafim,
                                                                                                              mes,
                                                                                                              tipoPeriodoDashboard));


            var dadosGraficoDiario = new GraficoFrequenciaAlunoDto
            {
                QuantidadeFrequenciaRegistrada = 2000,
                PorcentagemAulas = 25,
                DadosFrequenciaDashboard = new List<DadosRetornoFrequenciaAlunoDashboardDto>()
                {
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "EF-1", Quantidade = 120},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "EF-1", Quantidade = 170},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "EF-1", Quantidade = 130},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "EF-1", Quantidade = 420},

                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "EF-2", Quantidade = 200},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "EF-2", Quantidade = 150},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "EF-2", Quantidade = 160},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "EF-2", Quantidade = 480},

                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "EF-3", Quantidade = 300},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "EF-3", Quantidade = 267},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "EF-3", Quantidade = 180},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "EF-3", Quantidade = 567},

                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "EF-4", Quantidade = 400},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "EF-4", Quantidade = 345},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "EF-4", Quantidade = 200},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "EF-4", Quantidade = 867},

                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "EF-5", Quantidade = 500},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "EF-5", Quantidade = 456},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "EF-5", Quantidade = 300},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "EF-5", Quantidade = 988},

                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "EF-6", Quantidade = 600},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "EF-6", Quantidade = 500},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "EF-6", Quantidade = 400},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "EF-6", Quantidade = 1000},

                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "EF-7", Quantidade = 200},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "EF-7", Quantidade = 534},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "EF-7", Quantidade = 130},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "EF-7", Quantidade = 987},

                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "EF-8", Quantidade = 120},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "EF-8", Quantidade = 267},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "EF-8", Quantidade = 148},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "EF-8", Quantidade = 765},

                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "EF-9", Quantidade = 150},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "EF-9", Quantidade = 345},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "EF-9", Quantidade = 187},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "EF-9", Quantidade = 876},
                }
            };

            return dadosGraficoDiario;
        }
    }
}
