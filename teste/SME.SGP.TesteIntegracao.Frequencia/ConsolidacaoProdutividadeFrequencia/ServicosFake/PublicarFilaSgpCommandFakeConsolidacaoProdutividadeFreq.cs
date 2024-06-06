using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Frequencia.NotificacaoFrequenciaMensalAlunoInsuficiente.ServicosFake
{
    public class PublicarFilaSgpCommandFakeConsolidacaoProdutividadeFreq : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly IConsolidarInformacoesProdutividadeFrequenciaUseCase executarConsolidacaoInformacoesProdutividadeFrequencia;
        private readonly IConsolidarInformacoesProdutividadeFrequenciaDreUseCase executarConsolidacaoInformacoesProdutividadeFrequenciaDre;
        private readonly IConsolidarInformacoesProdutividadeFrequenciaUeUseCase executarConsolidacaoInformacoesProdutividadeFrequenciaUe;
        private readonly IConsolidarInformacoesProdutividadeFrequenciaBimestreUseCase executarConsolidacaoInformacoesProdutividadeFrequenciaBimestre;

        public PublicarFilaSgpCommandFakeConsolidacaoProdutividadeFreq(IConsolidarInformacoesProdutividadeFrequenciaUseCase executarConsolidacaoInformacoesProdutividadeFrequencia,
                                                            IConsolidarInformacoesProdutividadeFrequenciaDreUseCase executarConsolidacaoInformacoesProdutividadeFrequenciaDre,
                                                            IConsolidarInformacoesProdutividadeFrequenciaUeUseCase executarConsolidacaoInformacoesProdutividadeFrequenciaUe,
                                                            IConsolidarInformacoesProdutividadeFrequenciaBimestreUseCase executarConsolidacaoInformacoesProdutividadeFrequenciaBimestre)
        {
            this.executarConsolidacaoInformacoesProdutividadeFrequencia = executarConsolidacaoInformacoesProdutividadeFrequencia ?? throw new ArgumentNullException(nameof(executarConsolidacaoInformacoesProdutividadeFrequencia));
            this.executarConsolidacaoInformacoesProdutividadeFrequenciaDre = executarConsolidacaoInformacoesProdutividadeFrequenciaDre ?? throw new ArgumentNullException(nameof(executarConsolidacaoInformacoesProdutividadeFrequenciaDre));
            this.executarConsolidacaoInformacoesProdutividadeFrequenciaUe = executarConsolidacaoInformacoesProdutividadeFrequenciaUe ?? throw new ArgumentNullException(nameof(executarConsolidacaoInformacoesProdutividadeFrequenciaUe));
            this.executarConsolidacaoInformacoesProdutividadeFrequenciaBimestre = executarConsolidacaoInformacoesProdutividadeFrequenciaBimestre ?? throw new ArgumentNullException(nameof(executarConsolidacaoInformacoesProdutividadeFrequenciaBimestre));
        }

        public Task<bool> Handle(PublicarFilaSgpCommand request, CancellationToken cancellationToken)
        {
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(request.Filtros));

            return request.Rota switch
            {
                RotasRabbitSgpFrequencia.ConsolidarInformacoesProdutividadeFrequencia => executarConsolidacaoInformacoesProdutividadeFrequencia.Executar(mensagem),
                RotasRabbitSgpFrequencia.ConsolidarInformacoesProdutividadeFrequenciaDre => executarConsolidacaoInformacoesProdutividadeFrequenciaDre.Executar(mensagem),
                RotasRabbitSgpFrequencia.ConsolidarInformacoesProdutividadeFrequenciaUe => executarConsolidacaoInformacoesProdutividadeFrequenciaUe.Executar(mensagem),
                RotasRabbitSgpFrequencia.ConsolidarInformacoesProdutividadeFrequenciaBimestre => executarConsolidacaoInformacoesProdutividadeFrequenciaBimestre.Executar(mensagem),
                RotasRabbitSgpNotificacoes.Criacao => Task.FromResult(false),
                _ => throw new NotImplementedException($"Rota: {request.Rota} não implementada para o teste"),
            };
        }
    }
}
