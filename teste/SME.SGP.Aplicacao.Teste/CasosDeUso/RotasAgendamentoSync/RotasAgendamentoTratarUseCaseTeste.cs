using MediatR;
using Microsoft.Extensions.Options;
using Moq;
using Polly;
using Polly.Registry;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.RotasAgendamentoSync
{
    public class RotasAgendamentoTratarUseCaseTeste
    {
        private readonly Mock<IOptions<ConfiguracaoRabbitOptions>> mockConfiguracaoRabbit;
        private readonly Mock<IReadOnlyPolicyRegistry<string>> mockRegistry;
        private readonly Mock<IMediator> mockMediator;
        private readonly Mock<IAsyncPolicy> mockPolicy;
        private readonly RotasAgendamentoTratarUseCase useCase;

        public RotasAgendamentoTratarUseCaseTeste()
        {
            mockConfiguracaoRabbit = new Mock<IOptions<ConfiguracaoRabbitOptions>>();
            mockRegistry = new Mock<IReadOnlyPolicyRegistry<string>>();
            mockMediator = new Mock<IMediator>();
            mockPolicy = new Mock<IAsyncPolicy>();

            var config = new ConfiguracaoRabbitOptions
            {
                HostName = "testhost",
                Password = "testpassword",
                Port = 1234,
                UserName = "testuser",
                VirtualHost = "/"
            };
            mockConfiguracaoRabbit.Setup(c => c.Value).Returns(config);

            mockRegistry.Setup(r => r.Get<IAsyncPolicy>(PoliticaPolly.PublicaFila))
                .Returns(mockPolicy.Object);

            useCase = new RotasAgendamentoTratarUseCase(mockConfiguracaoRabbit.Object, mockRegistry.Object, mockMediator.Object);
        }

        [Fact]
        public void Construtor_Quando_ConfiguracaoRabbit_Nula_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("configuracaoRabbit",
                () => new RotasAgendamentoTratarUseCase(null, mockRegistry.Object, mockMediator.Object));
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("mediator",
                () => new RotasAgendamentoTratarUseCase(mockConfiguracaoRabbit.Object, mockRegistry.Object, null));
        }

        [Fact]
        public void Construtor_Quando_Registry_Nulo_Deve_Lancar_NullReferenceException()
        {
            Assert.Throws<NullReferenceException>(
                () => new RotasAgendamentoTratarUseCase(mockConfiguracaoRabbit.Object, null, mockMediator.Object));
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Invocar_Policy_E_Retornar_True()
        {
            var mensagemRabbit = new MensagemRabbit("fila.teste");

            mockPolicy.Setup(p => p.ExecuteAsync(It.IsAny<Func<Task>>()))
                .Returns(Task.CompletedTask);

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            mockRegistry.Verify(r => r.Get<IAsyncPolicy>(PoliticaPolly.PublicaFila), Times.Once);
            mockPolicy.Verify(p => p.ExecuteAsync(It.IsAny<Func<Task>>()), Times.Once);
        }
    }
}
