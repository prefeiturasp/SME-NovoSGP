using MediatR;
using Moq;
using SME.SGP.Infra.Interface;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Armazenamento
{
    public class MoverServicoArmazenamentoUseCaseTeste
    {
        [Fact]
        public void Constructor_Null_Servico_Armazenamento_Throws_Argument_Null_Exception()
        {
            var mediator = new Mock<IMediator>().Object;

            var ex = Assert.Throws<ArgumentNullException>(() =>
                new MoverServicoArmazenamentoUseCase(null, mediator));

            Assert.Equal("servicoArmazenamento", ex.ParamName);
        }

        [Theory]
        [InlineData("arquivo1.txt", "novo-caminho/arquivo1.txt")]
        [InlineData("arquivo2.pdf", "novo-caminho/arquivo2.pdf")]
        public async Task Executar_Deve_Chamar_Mover_E_Retornar_Resultado(string nomeArquivo, string retornoEsperado)
        {
            var mockServicoArmazenamento = new Mock<IServicoArmazenamento>();
            var mockMediator = new Mock<IMediator>();

            mockServicoArmazenamento
                .Setup(s => s.Mover(nomeArquivo))
                .ReturnsAsync(retornoEsperado);

            var useCase = new MoverServicoArmazenamentoUseCase(mockServicoArmazenamento.Object, mockMediator.Object);

            var resultado = await useCase.Executar(nomeArquivo);

            Assert.Equal(retornoEsperado, resultado);
            mockServicoArmazenamento.Verify(s => s.Mover(nomeArquivo), Times.Once);
        }
    }
}
