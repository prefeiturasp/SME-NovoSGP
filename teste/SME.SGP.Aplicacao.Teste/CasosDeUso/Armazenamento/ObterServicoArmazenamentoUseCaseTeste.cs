using MediatR;
using Moq;
using System;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Armazenamento
{
    public class ObterServicoArmazenamentoUseCaseTeste
    {
        [Fact]
        public void Constructor_Null_Servico_Armazenamento_Throws_Argument_Null_Exception()
        {
            var mediator = new Mock<IMediator>().Object;

            var ex = Assert.Throws<ArgumentNullException>(() =>
                new ObterServicoArmazenamentoUseCase(null, mediator));

            Assert.Equal("servicoArmazenamento", ex.ParamName);
        }

        [Theory]
        [InlineData("arquivo1.txt", true, "conteudo-temp")]
        [InlineData("arquivo2.pdf", false, "conteudo-normal")]
        public void Executar_Deve_Chamar_Servico_Armazenamento_E_Retornar_Valor(string nomeArquivo, bool ehPastaTemporaria, string retornoEsperado)
        {
            var mockServicoArmazenamento = new Mock<SME.SGP.Infra.Interface.IServicoArmazenamento>(); 
            var mockMediator = new Mock<IMediator>();

            mockServicoArmazenamento
                .Setup(s => s.Obter(nomeArquivo, ehPastaTemporaria))
                .Returns(retornoEsperado);

            var useCase = new ObterServicoArmazenamentoUseCase(mockServicoArmazenamento.Object, mockMediator.Object);

            var resultado = useCase.Executar(nomeArquivo, ehPastaTemporaria);

            Assert.Equal(retornoEsperado, resultado);
            mockServicoArmazenamento.Verify(s => s.Obter(nomeArquivo, ehPastaTemporaria), Times.Once);
        }
    }
}
