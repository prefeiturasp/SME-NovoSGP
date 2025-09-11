using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class EncerrarPlanosAEEEstudantesInativosUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly EncerrarPlanosAEEEstudantesInativosUseCase useCase;

        public EncerrarPlanosAEEEstudantesInativosUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new EncerrarPlanosAEEEstudantesInativosUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_True_E_Disparar_Comandos_Para_Todos_Os_Planos_Ativos()
        {
            // Arrange
            var planosAtivos = new List<SME.SGP.Dominio.PlanoAEE>
            {
                new SME.SGP.Dominio.PlanoAEE { Id = 1 },
                new SME.SGP.Dominio.PlanoAEE { Id = 2 }
            };

            var usuarioSistema = new Usuario { Id = 1, Login = "Sistema" };

            mediator.Setup(m => m.Send(ObterPlanosAEEAtivosQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(planosAtivos);

            mediator.Setup(m => m.Send(It.Is<ObterUsuarioPorRfQuery>(q => q.CodigoRf == "Sistema"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioSistema);

            mediator.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(new MensagemRabbit());

            // Assert
            Assert.True(resultado);
            mediator.Verify(m => m.Send(ObterPlanosAEEAtivosQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.Is<ObterUsuarioPorRfQuery>(q => q.CodigoRf == "Sistema"), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(
                c => c.Rota == RotasRabbitSgpAEE.EncerrarPlanoAEEEstudantesInativosTratar),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Nao_Existirem_Planos_Ativos()
        {
            // Arrange
            mediator.Setup(m => m.Send(ObterPlanosAEEAtivosQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SME.SGP.Dominio.PlanoAEE>());

            // Act
            var resultado = await useCase.Executar(new MensagemRabbit());

            // Assert
            Assert.False(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Tratar_Excecao_Ao_Obter_Planos_Ativos()
        {
            // Arrange
            mediator.Setup(m => m.Send(ObterPlanosAEEAtivosQuery.Instance, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao obter planos ativos"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => useCase.Executar(new MensagemRabbit()));
        }

        [Fact]
        public async Task Deve_Tratar_Excecao_Ao_Obter_Usuario_Sistema()
        {
            // Arrange
            var planosAtivos = new List<SME.SGP.Dominio.PlanoAEE> { new SME.SGP.Dominio.PlanoAEE { Id = 1 } };

            mediator.Setup(m => m.Send(ObterPlanosAEEAtivosQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(planosAtivos);

            mediator.Setup(m => m.Send(It.Is<ObterUsuarioPorRfQuery>(q => q.CodigoRf == "Sistema"), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao obter usuário sistema"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => useCase.Executar(new MensagemRabbit()));
        }

        [Fact]
        public async Task Deve_Falhar_Se_Qualquer_Plano_Falhar()
        {
            // Arrange
            var planosAtivos = new List<SME.SGP.Dominio.PlanoAEE> { new SME.SGP.Dominio.PlanoAEE { Id = 1 }, new SME.SGP.Dominio.PlanoAEE { Id = 2 } };
            var usuarioSistema = new Usuario { Id = 1, Login = "Sistema" };

            mediator.Setup(m => m.Send(ObterPlanosAEEAtivosQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(planosAtivos);

            mediator.Setup(m => m.Send(It.Is<ObterUsuarioPorRfQuery>(q => q.CodigoRf == "Sistema"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioSistema);

            mediator.SetupSequence(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ThrowsAsync(new Exception("Erro no segundo plano"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => useCase.Executar(new MensagemRabbit()));
            mediator.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }
    }
}