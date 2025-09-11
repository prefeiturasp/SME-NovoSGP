using MediatR;
using Moq;
using SME.SGP.Infra.Dtos.EscolaAqui;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.SolicitarReiniciarSenha
{
    public class SolicitarReiniciarSenhaEscolaAquiUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly SolicitarReiniciarSenhaEscolaAquiUseCase useCase;

        public SolicitarReiniciarSenhaEscolaAquiUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new SolicitarReiniciarSenhaEscolaAquiUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Resposta_Quando_Cpf_Valido()
        {
            var cpf = "12345678900";
            var mensagem = "Solicitação enviada com sucesso.";

            mediatorMock
                .Setup(m => m.Send(It.Is<SolicitarReiniciarSenhaEscolaAquiCommand>(c => c.Cpf == cpf), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RespostaSolicitarReiniciarSenhaEscolaAquiDto(mensagem));

            var resultado = await useCase.Executar(cpf);

            Assert.NotNull(resultado);
            Assert.Equal(mensagem, resultado.Mensagem);
            mediatorMock.Verify(m => m.Send(It.IsAny<SolicitarReiniciarSenhaEscolaAquiCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Null_Se_Mediator_Nao_Trouxer_Resposta()
        {
            var cpf = "99999999999";

            mediatorMock
                .Setup(m => m.Send(It.IsAny<SolicitarReiniciarSenhaEscolaAquiCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((RespostaSolicitarReiniciarSenhaEscolaAquiDto)null);

            var resultado = await useCase.Executar(cpf);

            Assert.Null(resultado);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_Mediator_Falhar()
        {
            var cpf = "88888888888";

            mediatorMock
                .Setup(m => m.Send(It.IsAny<SolicitarReiniciarSenhaEscolaAquiCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Erro no mediator"));

            await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.Executar(cpf));
        }

        [Fact]
        public async Task Deve_Chamar_Mediator_Com_Cpf_Correto()
        {
            var cpfEsperado = "22233344455";
            mediatorMock
                .Setup(m => m.Send(It.IsAny<SolicitarReiniciarSenhaEscolaAquiCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RespostaSolicitarReiniciarSenhaEscolaAquiDto("OK"));

            await useCase.Executar(cpfEsperado);

            mediatorMock.Verify(m => m.Send(It.Is<SolicitarReiniciarSenhaEscolaAquiCommand>(c => c.Cpf == cpfEsperado), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
