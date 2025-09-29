using MediatR;
using Moq;
using SME.SGP.Infra.Interface;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Armazenamento
{
    public class ExcluirTemporarioServicoArmazenamentoUseCaseTeste
    {
        [Fact]
        public void Constructor_Null_Servico_Armazenamento_Throws_Argument_Null_Exception()
        {
            var mediator = new Mock<IMediator>().Object;

            var ex = Assert.Throws<ArgumentNullException>(() =>
                new ExcluirTemporarioServicoArmazenamentoUseCase(null, mediator));

            Assert.Equal("servicoArmazenamento", ex.ParamName);
        }

        [Theory]
        [InlineData("arquivo1.txt", "bucket-temp", true)]
        [InlineData("arquivo2.pdf", "bucket-outro", false)]
        public async Task Executar_Deve_Chamar_Excluir_E_Retornar_Resultado(string nomeArquivo, string bucket, bool retornoEsperado)
        {
            var mockServicoArmazenamento = new Mock<IServicoArmazenamento>();
            var mockMediator = new Mock<IMediator>();

            mockServicoArmazenamento
                .Setup(s => s.Excluir(nomeArquivo, bucket))
                .ReturnsAsync(retornoEsperado);

            var useCase = new ExcluirTemporarioServicoArmazenamentoUseCase(mockServicoArmazenamento.Object, mockMediator.Object);

            var resultado = await useCase.Executar(nomeArquivo, bucket);

            Assert.Equal(retornoEsperado, resultado);
            mockServicoArmazenamento.Verify(s => s.Excluir(nomeArquivo, bucket), Times.Once);
        }
    }
}
