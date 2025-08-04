using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class VerificarExistenciaPlanoAEEPorEstudanteUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly VerificarExistenciaPlanoAEEPorEstudanteUseCase useCase;

        public VerificarExistenciaPlanoAEEPorEstudanteUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new VerificarExistenciaPlanoAEEPorEstudanteUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_True_Quando_Nao_Existir_Plano_AEE_Para_Estudante()
        {
            // Arrange
            var codigoEstudante = "123456";

            mediator.Setup(x => x.Send(It.Is<VerificarExistenciaPlanoAEEPorEstudanteQuery>(q => q.CodigoEstudante == codigoEstudante),
                                     It.IsAny<CancellationToken>()))
                   .ReturnsAsync((PlanoAEEResumoDto)null);

            // Act
            var resultado = await useCase.Executar(codigoEstudante);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Existir_Plano_AEE_Para_Estudante()
        {
            // Arrange
            var codigoEstudante = "123456";
            var planoExistente = new Infra.PlanoAEEResumoDto();

            mediator.Setup(x => x.Send(It.Is<VerificarExistenciaPlanoAEEPorEstudanteQuery>(q => q.CodigoEstudante == codigoEstudante),
                                     It.IsAny<CancellationToken>()))
                   .ReturnsAsync(planoExistente);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(codigoEstudante));
            Assert.Equal("Estudante/Criança já possui plano AEE em aberto", ex.Message);
        }
    }
}