using MediatR;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class EncerrarEncaminhamentoAEEAutomaticoSyncUseCaseTeste
    {
        private readonly Mock<IMediator> _mediator;
        private readonly EncerrarEncaminhamentoAEEAutomaticoSyncUseCase _encerrarEncaminhamentoAEEAutomaticoSyncUseCase;

        public EncerrarEncaminhamentoAEEAutomaticoSyncUseCaseTeste()
        {
            _mediator = new Mock<IMediator>();
            _encerrarEncaminhamentoAEEAutomaticoSyncUseCase = new EncerrarEncaminhamentoAEEAutomaticoSyncUseCase(_mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Encaminhamentos_AEE_Vigentes()
        {
            //-> Assert
            _mediator.Setup(c => c.Send(It.IsAny<ObterEncaminhamentoAEEVigenteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<EncaminhamentoAEEVigenteDto> {
                    new EncaminhamentoAEEVigenteDto()
                    {
                        EncaminhamentoId = 1983,
                        AlunoCodigo = "4824410",
                        TurmaId = 869773,
                        TurmaCodigo = "2369048",
                        AnoLetivo = 2022,
                        UeId = 276,
                        UeCodigo = "094668"
                    }
                });

            //-> Act
            var retorno = await _encerrarEncaminhamentoAEEAutomaticoSyncUseCase.Executar(CriarMensagemRabbitEncerrarEncaminhamentoAEEAutomaticoSync());

            //-> Assert    
            _mediator.Verify(c => c.Send(It.IsAny<ObterEncaminhamentoAEEVigenteQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(retorno);
        }

        private MensagemRabbit CriarMensagemRabbitEncerrarEncaminhamentoAEEAutomaticoSync()
        {
            var json = JsonConvert.SerializeObject(new object { });

            return new MensagemRabbit(json)
            {
                Action = "sgp/encaminhamento/aee/encerrar/automatico/encerrar",
                UsuarioLogadoNomeCompleto = "MILENA PEDROSO RUELLA MARTINS",
                UsuarioLogadoRF = "7707533"
            };
        }
    }
}
