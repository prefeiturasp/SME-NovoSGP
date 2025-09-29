using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoDiariosBordo
{
    public class ConsolidarDiariosBordoCarregarUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsolidarDiariosBordoCarregarUseCase _useCase;
        private readonly MensagemRabbit _mensagemRabbit;

        public ConsolidarDiariosBordoCarregarUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ConsolidarDiariosBordoCarregarUseCase(_mediatorMock.Object);
            _mensagemRabbit = new MensagemRabbit("Test", null, Guid.NewGuid(), "test_user");
        }

        [Fact]
        public async Task Executar_Quando_Consolidacao_Inativa_Deve_Retornar_True_Sem_Executar_Processo()
        {
            var parametroSistema = new ParametrosSistema { Ativo = false };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroSistema);

            var resultado = await _useCase.Executar(_mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<RemoverConsolidacoesDiariosBordoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTodasUesIdsQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Parametro_De_Execucao_Nao_Existe_Deve_Retornar_True_Sem_Executar_Processo()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ParametrosSistema)null);

            var resultado = await _useCase.Executar(_mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Nao_Houver_Ues_Para_Processar_Deve_Limpar_E_Atualizar_Data_Sem_Publicar()
        {
            var parametroSistema = new ParametrosSistema { Ativo = true, Valor = "data_antiga" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroSistema);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodasUesIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<long>());

            var resultado = await _useCase.Executar(_mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<RemoverConsolidacoesDiariosBordoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<AtualizarParametroSistemaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotEqual("data_antiga", parametroSistema.Valor);
        }

        [Fact]
        public async Task Executar_Quando_Consolidacao_Ativa_E_Houver_Ues_Deve_Executar_Processo_Completo()
        {
            var parametroSistema = new ParametrosSistema { Ativo = true, Valor = "data_antiga" };
            var ues = new List<long> { 123456, 987654 };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroSistema);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodasUesIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ues);

            var resultado = await _useCase.Executar(_mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<RemoverConsolidacoesDiariosBordoCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTodasUesIdsQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(
                c => c.Rota == RotasRabbitSgp.ConsolidarDiariosBordoPorUeTratar),
                It.IsAny<CancellationToken>()), Times.Exactly(ues.Count));

            _mediatorMock.Verify(m => m.Send(It.Is<AtualizarParametroSistemaCommand>(
                c => c.Parametro == parametroSistema),
                It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotEqual("data_antiga", parametroSistema.Valor);
            Assert.True(DateTime.TryParse(parametroSistema.Valor, out _));
        }
    }
}
