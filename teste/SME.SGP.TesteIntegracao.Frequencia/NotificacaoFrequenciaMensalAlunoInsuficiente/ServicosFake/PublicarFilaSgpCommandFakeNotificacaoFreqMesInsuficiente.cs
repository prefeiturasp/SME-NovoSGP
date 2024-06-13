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
    public class PublicarFilaSgpCommandFakeNotificacaoFreqMesInsuficiente : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUseCase executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUseCase;
        private readonly IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaDreUseCase executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaDreUseCase;
        private readonly IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCase executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCase;
        private readonly IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase;

        public PublicarFilaSgpCommandFakeNotificacaoFreqMesInsuficiente(IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUseCase executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUseCase,
                                                            IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaDreUseCase executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaDreUseCase,
                                                            IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCase executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCase,
                                                            IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase)
        {
            this.executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUseCase = executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUseCase ?? throw new ArgumentNullException(nameof(executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUseCase));
            this.executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaDreUseCase = executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaDreUseCase ?? throw new ArgumentNullException(nameof(executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaDreUseCase));
            this.executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCase = executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCase ?? throw new ArgumentNullException(nameof(executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCase));
            this.executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase = executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase ?? throw new ArgumentNullException(nameof(executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase));
        }

        public Task<bool> Handle(PublicarFilaSgpCommand request, CancellationToken cancellationToken)
        {
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(request.Filtros));

            return request.Rota switch
            {
                RotasRabbitSgpFrequencia.ExecutarNotificacaoAlunosBaixaFrequenciaBuscaAtiva => executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUseCase.Executar(mensagem),
                RotasRabbitSgpFrequencia.ExecutarNotificacaoAlunosBaixaFrequenciaBuscaAtivaDre => executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaDreUseCase.Executar(mensagem),
                RotasRabbitSgpFrequencia.ExecutarNotificacaoAlunosBaixaFrequenciaBuscaAtivaUe => executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCase.Executar(mensagem),
                RotasRabbitSgpFrequencia.ExecutarNotificacaoAlunosBaixaFrequenciaBuscaAtivaProfissionaisNAAPA => executarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase.Executar(mensagem),
                RotasRabbitSgpNotificacoes.Criacao => Task.FromResult(false),
                _ => throw new NotImplementedException($"Rota: {request.Rota} não implementada para o teste"),
            };
        }
    }
}
