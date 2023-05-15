using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarFrequenciaPorTurmaUseCase : AbstractUseCase, IConsolidarFrequenciaPorTurmaUseCase
    {
        public ConsolidarFrequenciaPorTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            try
            {
                var filtro = mensagem.ObterObjetoMensagem<FiltroConsolidacaoFrequenciaTurma>();
                var turma = !string.IsNullOrWhiteSpace(filtro.TurmaCodigo) ?
                    await mediator.Send(new ObterTurmaPorCodigoQuery(filtro.TurmaCodigo)) :
                    await mediator.Send(new ObterTurmaPorIdQuery(filtro.TurmaId));

                if (turma != null)
                {
                    var dataAtual = DateTimeExtension.HorarioBrasilia();
                    var anoAnterior = turma.AnoLetivo < dataAtual.Year;

                    var alunos = await mediator
                        .Send(new ObterAlunosDentroPeriodoQuery(turma.CodigoTurma, (dataAtual, dataAtual)));

                    if (filtro.PercentualFrequenciaMinimo == 0)
                    {
                        var parametro = await mediator
                            .Send(new ObterParametroSistemaPorTipoEAnoQuery(turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? TipoParametroSistema.PercentualFrequenciaMinimaInfantil : TipoParametroSistema.PercentualFrequenciaCritico, turma.AnoLetivo));

                        filtro.PercentualFrequenciaMinimo = double.Parse(parametro.Valor);
                    }

                    await ConsolidarFrequenciaAlunos(turma.Id, turma.CodigoTurma, filtro.PercentualFrequenciaMinimo, alunos, anoAnterior);
                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Consolidar Frequencia Por Turma UseCase", LogNivel.Critico, LogContexto.Frequencia, ex.Message));
                throw;
            }
        }

        private async Task ConsolidarFrequenciaAlunos(long turmaId, string turmaCodigo, double percentualFrequenciaMinimo, IEnumerable<AlunoPorTurmaResposta> alunos, bool anoAnterior)
        {
            var frequenciaTurma = await mediator.Send(new ObterFrequenciaGeralPorTurmaQuery(turmaCodigo));

            var frequenciasConsideradas = from ft in frequenciaTurma
                                          join a in alunos
                                          on ft.AlunoCodigo equals a.CodigoAluno
                                          where (anoAnterior && !a.Inativo && a.DataMatricula.Date <= ft.PeriodoFim.Date) ||
                                                (!anoAnterior && a.DataMatricula.Date <= ft.PeriodoFim.Date)
                                          select ft;

            var quantidadeReprovados = 0;
            var quantidadeAprovados = 0;

            var frequenciasAgrupadasPorAluno = frequenciasConsideradas.GroupBy(f => f.AlunoCodigo);
            var listaAlunoPercentualGeral = (from fa in frequenciasAgrupadasPorAluno
                                                where fa.Any(f => f.TotalAulas > 0)
                                                select new
                                                {
                                                    codigoAluno = fa.Key,
                                                    totalAulas = Convert.ToDouble(fa.Sum(f => f.TotalAulas)),
                                                    totalAusencias = Convert.ToDouble(fa.Sum(f => f.TotalAusencias) - fa.Sum(f => f.TotalCompensacoes))
                                                })
                                                .Select(fa => new
                                                {
                                                    fa.codigoAluno,
                                                    percentualTotal = Math.Round(100 - ((fa.totalAusencias / fa.totalAulas) * 100))
                                                });

            quantidadeReprovados = listaAlunoPercentualGeral.Count(fg => fg.percentualTotal < percentualFrequenciaMinimo);
            quantidadeAprovados = listaAlunoPercentualGeral.Count(fg => fg.percentualTotal >= percentualFrequenciaMinimo);

            await RegistraConsolidacaoFrequenciaTurma(turmaId, quantidadeAprovados, quantidadeReprovados);
        }

        private async Task RegistraConsolidacaoFrequenciaTurma(long turmaId, int quantidadeAprovados, int quantidadeReprovados)
        {
            await mediator.Send(new RegistraConsolidacaoFrequenciaTurmaCommand(turmaId, quantidadeAprovados, quantidadeReprovados));
        }
    }
}
