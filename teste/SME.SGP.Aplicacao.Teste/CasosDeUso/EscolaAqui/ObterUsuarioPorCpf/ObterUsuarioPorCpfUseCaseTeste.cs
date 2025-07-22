using MediatR;
using Moq;
using SME.SGP.Infra.Dtos.EscolaAqui;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.ObterUsuarioPorCpf
{
    public class ObterUsuarioPorCpfUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterUsuarioPorCpfUseCase useCase;

        public ObterUsuarioPorCpfUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterUsuarioPorCpfUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Usuario_Quando_Valido()
        {
            var codigoDre = "dre01";
            var codigoUe = "ue01";
            var cpf = "12345678900";
            var usuario = new UsuarioEscolaAquiDto(cpf, "Maria");

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterUsuarioPorCpfQuery>(q =>
                    q.CodigoDre == codigoDre &&
                    q.CodigoUe == codigoUe &&
                    q.Cpf == cpf), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            var resultado = await useCase.Executar(codigoDre, codigoUe, cpf);

            Assert.NotNull(resultado);
            Assert.Equal("Maria", resultado.Nome);
            Assert.Equal(cpf, resultado.Cpf);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterUsuarioPorCpfQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Null_Se_Usuario_Nao_Encontrado()
        {
            var codigoDre = "dre02";
            var codigoUe = "ue02";
            var cpf = "00000000000";

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUsuarioPorCpfQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((UsuarioEscolaAquiDto)null);

            var resultado = await useCase.Executar(codigoDre, codigoUe, cpf);

            Assert.Null(resultado);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_Mediator_Falhar()
        {
            var codigoDre = "dre03";
            var codigoUe = "ue03";
            var cpf = "99999999999";

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUsuarioPorCpfQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Erro no Mediator"));

            await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.Executar(codigoDre, codigoUe, cpf));
        }

        [Fact]
        public async Task Deve_Chamar_Mediator_Com_Parametros_Corretos()
        {         
            var codigoDre = "DRE123";
            var codigoUe = "UE456";
            var cpf = "12345678901";

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUsuarioPorCpfQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UsuarioEscolaAquiDto(cpf, "Teste"));

            await useCase.Executar(codigoDre, codigoUe, cpf);

            mediatorMock.Verify(m => m.Send(It.Is<ObterUsuarioPorCpfQuery>(q =>
                q.CodigoDre == codigoDre &&
                q.CodigoUe == codigoUe &&
                q.Cpf == cpf), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
