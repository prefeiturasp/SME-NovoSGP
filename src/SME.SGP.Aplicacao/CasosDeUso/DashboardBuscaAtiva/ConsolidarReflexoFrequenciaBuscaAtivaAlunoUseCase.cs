using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarReflexoFrequenciaBuscaAtivaAlunoUseCase : AbstractUseCase, IConsolidarReflexoFrequenciaBuscaAtivaAlunoUseCase
    {
        private readonly IRepositorioRegistroAcaoBuscaAtiva repositorioBuscaAtiva;
        public ConsolidarReflexoFrequenciaBuscaAtivaAlunoUseCase(IMediator mediator, IRepositorioRegistroAcaoBuscaAtiva repositorioBuscaAtiva) : base(mediator)
        {
            this.repositorioBuscaAtiva = repositorioBuscaAtiva ?? throw new System.ArgumentNullException(nameof(repositorioBuscaAtiva));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroIdAnoLetivoDto>();
            var registrosBuscaAtiva = await repositorioBuscaAtiva.ObterRegistroBuscaAtivaAluno(filtro.Id);
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
            }

            RegistroFrequenciaAlunoPorTurmaEMesDto freqGeralAntesRegistroAcao = await mediator.Send(new ObterFrequenciaMensalPorTurmaMesAlunoQuery(registrosBuscaAtiva.TurmaCodigo,
                                                                                                      registrosBuscaAtiva.AlunoCodigo,
                                                                                                      registrosBuscaAtiva.DataBuscaAtiva.Date.AddDays(-1)));

            RegistroFrequenciaAlunoPorTurmaEMesDto freqGeralAtual = await mediator.Send(new ObterFrequenciaMensalPorTurmaMesAlunoQuery(registrosBuscaAtiva.TurmaCodigo,
                                                                                                  registrosBuscaAtiva.AlunoCodigo,
                                                                                                  filtro.Data.Date));

            return true;
        }
    }
}
