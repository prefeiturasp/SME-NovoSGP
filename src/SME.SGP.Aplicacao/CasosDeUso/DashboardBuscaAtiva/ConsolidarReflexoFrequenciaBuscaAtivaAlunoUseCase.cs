using MediatR;
using Minio.DataModel;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarReflexoFrequenciaBuscaAtivaAlunoUseCase : AbstractUseCase, IConsolidarReflexoFrequenciaBuscaAtivaAlunoUseCase
    {
        private readonly IRepositorioRegistroAcaoBuscaAtiva repositorioBuscaAtiva;
        private const int ANUAL = 0;
        public ConsolidarReflexoFrequenciaBuscaAtivaAlunoUseCase(IMediator mediator, IRepositorioRegistroAcaoBuscaAtiva repositorioBuscaAtiva) : base(mediator)
        {
            this.repositorioBuscaAtiva = repositorioBuscaAtiva ?? throw new System.ArgumentNullException(nameof(repositorioBuscaAtiva));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroIdAnoLetivoDto>();
            var registrosBuscaAtiva = await repositorioBuscaAtiva.ObterRegistroBuscaAtivaAluno(filtro.Id);
            await IncluirConsolidacaoReflexoFrequenciaMensal(filtro, registrosBuscaAtiva);
            await IncluirConsolidacaoReflexoFrequenciaAnual(filtro, registrosBuscaAtiva);
            return true;
        }

        private async Task IncluirConsolidacaoReflexoFrequenciaMensal(FiltroIdAnoLetivoDto filtro, RegistroAcaoBuscaAtivaAlunoDto registrosBuscaAtiva)
        {
            if (registrosBuscaAtiva.DataBuscaAtiva.Month == filtro.Data.Month)
            {
                RegistroFrequenciaAlunoPorTurmaEMesDto freqMesAntesRegistroAcao = await mediator.Send(new ObterFrequenciaMensalPorTurmaMesAlunoQuery(registrosBuscaAtiva.TurmaCodigo,
                                                                                                      registrosBuscaAtiva.AlunoCodigo,
                                                                                                      registrosBuscaAtiva.DataBuscaAtiva.Date.AddDays(-1),
                                                                                                      filtro.Data.Month));

                RegistroFrequenciaAlunoPorTurmaEMesDto freqAtualMes = await mediator.Send(new ObterFrequenciaMensalPorTurmaMesAlunoQuery(registrosBuscaAtiva.TurmaCodigo,
                                                                                                      registrosBuscaAtiva.AlunoCodigo,
                                                                                                      filtro.Data.Date,
                                                                                                      filtro.Data.Month));

                await mediator.Send(new SalvarConsolidacaoReflexoFrequenciaBuscaAtivaCommand(new ConsolidacaoReflexoFrequenciaBuscaAtivaAluno()
                {
                    AlunoCodigo = registrosBuscaAtiva.AlunoCodigo,
                    AlunoNome = registrosBuscaAtiva.AlunoNome,
                    AnoLetivo = registrosBuscaAtiva.AnoLetivo,
                    DataBuscaAtiva = registrosBuscaAtiva.DataBuscaAtiva,
                    Modalidade = registrosBuscaAtiva.Modalidade,
                    TurmaCodigo = registrosBuscaAtiva.TurmaCodigo,
                    UeCodigo = registrosBuscaAtiva.UeCodigo,
                    Mes = filtro.Data.Month,
                    PercFrequenciaAntesAcao = freqMesAntesRegistroAcao?.Percentual ?? 0,
                    PercFrequenciaAposAcao = freqAtualMes?.Percentual ?? 0
                }));
            }
        }

        private async Task IncluirConsolidacaoReflexoFrequenciaAnual(FiltroIdAnoLetivoDto filtro, RegistroAcaoBuscaAtivaAlunoDto registrosBuscaAtiva)
        {
            RegistroFrequenciaAlunoPorTurmaEMesDto freqGeralAntesRegistroAcao = await mediator.Send(new ObterFrequenciaMensalPorTurmaMesAlunoQuery(registrosBuscaAtiva.TurmaCodigo,
                                                                                                      registrosBuscaAtiva.AlunoCodigo,
                                                                                                      registrosBuscaAtiva.DataBuscaAtiva.Date.AddDays(-1)));

            RegistroFrequenciaAlunoPorTurmaEMesDto freqGeralAtual = await mediator.Send(new ObterFrequenciaMensalPorTurmaMesAlunoQuery(registrosBuscaAtiva.TurmaCodigo,
                                                                                                  registrosBuscaAtiva.AlunoCodigo,
                                                                                                  filtro.Data.Date));


            await mediator.Send(new SalvarConsolidacaoReflexoFrequenciaBuscaAtivaCommand(new ConsolidacaoReflexoFrequenciaBuscaAtivaAluno()
                {
                    AlunoCodigo = registrosBuscaAtiva.AlunoCodigo,
                    AlunoNome = registrosBuscaAtiva.AlunoNome,
                    AnoLetivo = registrosBuscaAtiva.AnoLetivo,
                    DataBuscaAtiva = registrosBuscaAtiva.DataBuscaAtiva,
                    Modalidade = registrosBuscaAtiva.Modalidade,
                    TurmaCodigo = registrosBuscaAtiva.TurmaCodigo,
                    UeCodigo = registrosBuscaAtiva.UeCodigo,
                    Mes = ANUAL,
                    PercFrequenciaAntesAcao = freqGeralAntesRegistroAcao?.Percentual ?? 0,
                    PercFrequenciaAposAcao = freqGeralAtual?.Percentual ?? 0
            }));
        }
    }
}
