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

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes.ServicosFakes
{
    public class PublicarFilaSgpCommandFakeAtualizacaoMapEstudante : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly IAtualizarMapeamentoDosEstudantesUseCase atualizarMapeamentoDosEstudantesUseCase;
        private readonly IAtualizarMapeamentoDoEstudanteDoBimestreUseCase atualizarMapeamentoDoEstudanteDoBimestreUseCase;

        public PublicarFilaSgpCommandFakeAtualizacaoMapEstudante(IAtualizarMapeamentoDosEstudantesUseCase atualizarMapeamentoDosEstudantesUseCase,
                                                            IAtualizarMapeamentoDoEstudanteDoBimestreUseCase atualizarMapeamentoDoEstudanteDoBimestreUseCase)
        {
            this.atualizarMapeamentoDosEstudantesUseCase = atualizarMapeamentoDosEstudantesUseCase ?? throw new ArgumentNullException(nameof(atualizarMapeamentoDosEstudantesUseCase));
            this.atualizarMapeamentoDoEstudanteDoBimestreUseCase = atualizarMapeamentoDoEstudanteDoBimestreUseCase ?? throw new ArgumentNullException(nameof(atualizarMapeamentoDoEstudanteDoBimestreUseCase));
        }

        public Task<bool> Handle(PublicarFilaSgpCommand request, CancellationToken cancellationToken)
        {
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(request.Filtros));

            return request.Rota switch
            {
                RotasRabbitSgp.ExecutarAtualizacaoMapeamentoEstudantes => atualizarMapeamentoDosEstudantesUseCase.Executar(mensagem),
                RotasRabbitSgp.ExecutarAtualizacaoMapeamentoEstudantesBimestre => atualizarMapeamentoDoEstudanteDoBimestreUseCase.Executar(mensagem),
                _ => throw new NotImplementedException($"Rota: {request.Rota} não implementada para o teste"),
            };
        }
    }
}
