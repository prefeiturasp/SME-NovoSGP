using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Estudante
{
    public class SalvarFotoEstudanteUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly SalvarFotoEstudanteUseCase useCase;

        public SalvarFotoEstudanteUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new SalvarFotoEstudanteUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_DtoValido_Deve_RetornarGuid_Teste()
        {
            var alunoCodigo = "12345";
            var fileMock = new Mock<IFormFile>();
            var guidEsperado = Guid.NewGuid();
            var dto = new EstudanteFotoDto { AlunoCodigo = alunoCodigo, File = fileMock.Object };

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarFotoEstudanteCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(guidEsperado);

            var resultado = await useCase.Executar(dto);

            Assert.Equal(guidEsperado, resultado);
            mediatorMock.Verify(m => m.Send(It.Is<SalvarFotoEstudanteCommand>(c => c.AlunoCodigo == alunoCodigo && c.File == fileMock.Object), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
