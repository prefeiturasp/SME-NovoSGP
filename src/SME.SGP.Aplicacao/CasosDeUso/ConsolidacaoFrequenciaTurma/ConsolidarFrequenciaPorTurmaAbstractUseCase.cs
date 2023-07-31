using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public abstract class ConsolidarFrequenciaPorTurmaAbstractUseCase : AbstractUseCase
    {
        protected FiltroConsolidacaoFrequenciaTurma Filtro { get; set; }
        protected bool AnoAnterior { get; set; }

        public ConsolidarFrequenciaPorTurmaAbstractUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            try
            {
                Filtro = mensagem.ObterObjetoMensagem<FiltroConsolidacaoFrequenciaTurma>();
                var turma = !string.IsNullOrWhiteSpace(Filtro.TurmaCodigo) ?
                    await mediator.Send(new ObterTurmaPorCodigoQuery(Filtro.TurmaCodigo)) :
                    await mediator.Send(new ObterTurmaPorIdQuery(Filtro.TurmaId));

                if (turma != null)
                {
                    var dataAtual = DateTimeExtension.HorarioBrasilia();
                    AnoAnterior = turma.AnoLetivo < dataAtual.Year;

                    if (Filtro.PercentualFrequenciaMinimo == 0)
                    {
                        var parametro = await mediator
                            .Send(new ObterParametroSistemaPorTipoEAnoQuery(turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? TipoParametroSistema.PercentualFrequenciaMinimaInfantil : TipoParametroSistema.PercentualFrequenciaCritico, turma.AnoLetivo));

                        Filtro.PercentualFrequenciaMinimo = double.Parse(parametro.Valor);
                    }

                    await ConsolidarFrequenciaAlunos(turma.Id, turma.CodigoTurma, Filtro.PercentualFrequenciaMinimo);
                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Consolidar Frequencia Por Turma UseCase", LogNivel.Critico, LogContexto.Frequencia, ex.Message));
                throw;
            }
        }
        protected abstract (DateTime?, DateTime?) Periodos { get; }
        protected abstract TipoConsolidadoFrequencia TipoConsolidado { get; }
        protected abstract Task<IEnumerable<FrequenciaAlunoDto>> ObterFrequenciaConsideradas(string codigoTurma);

        private async Task ConsolidarFrequenciaAlunos(long turmaId, string turmaCodigo, double percentualFrequenciaMinimo)
        {
            var frequenciasConsideradas = await ObterFrequenciaConsideradas(turmaCodigo);

            int quantidadeReprovados, quantidadeAprovados, totalAulas, totalFrequencias = 0;

            var frequenciasAgrupadasPorAluno = frequenciasConsideradas.GroupBy(f => f.AlunoCodigo);
            var listaAlunoPercentualGeral = (from fa in frequenciasAgrupadasPorAluno
                                                where fa.Any(f => f.TotalAulas > 0)
                                                select new
                                                {
                                                    codigoAluno = fa.Key,
                                                    totalAulas = fa.FirstOrDefault().TotalAulas,
                                                    totalAusencias = Convert.ToDouble(fa.Sum(f => f.TotalAusencias) - fa.Sum(f => f.TotalCompensacoes)),
                                                    totalFrequencias = int.Parse(fa.FirstOrDefault().TotalFrequencias.ToString())
                                                })
                                                .Select(fa => new
                                                {
                                                    fa.codigoAluno,
                                                    percentualTotal = Math.Round(100 - ((fa.totalAusencias / fa.totalAulas) * 100)),
                                                    fa.totalAulas,
                                                    fa.totalFrequencias
                                                });

            quantidadeReprovados = listaAlunoPercentualGeral.Count(fg => fg.percentualTotal < percentualFrequenciaMinimo);
            quantidadeAprovados = listaAlunoPercentualGeral.Count(fg => fg.percentualTotal >= percentualFrequenciaMinimo);
            totalAulas = listaAlunoPercentualGeral.Any() ? int.Parse(listaAlunoPercentualGeral.FirstOrDefault().totalAulas.ToString()) : 0;
            totalFrequencias = listaAlunoPercentualGeral.Any() ? listaAlunoPercentualGeral.FirstOrDefault().totalFrequencias : 0;

            await RegistraConsolidacaoFrequenciaTurma(turmaId, quantidadeAprovados, quantidadeReprovados,totalAulas, totalFrequencias);
        }

        private async Task RegistraConsolidacaoFrequenciaTurma(long turmaId, int quantidadeAprovados, int quantidadeReprovados, int totalAulas, int totalFrequencias)
        {
            await mediator.Send(new RegistraConsolidacaoFrequenciaTurmaCommand(turmaId, quantidadeAprovados, quantidadeReprovados, TipoConsolidado, Periodos.Item1, Periodos.Item2,totalAulas, totalFrequencias));
        }
    }
}
