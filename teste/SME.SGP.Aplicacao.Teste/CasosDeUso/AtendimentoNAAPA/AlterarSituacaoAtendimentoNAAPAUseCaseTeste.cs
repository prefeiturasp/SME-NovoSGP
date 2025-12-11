using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AtendimentoNAAPA
{
    public class AlterarSituacaoAtendimentoNAAPAUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AlterarSituacaoAtendimentoNAAPAUseCase _useCase;

        public AlterarSituacaoAtendimentoNAAPAUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new AlterarSituacaoAtendimentoNAAPAUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Quando_Mediator_For_Nulo()
        {
            var excecao = Assert.Throws<ArgumentNullException>(() => new AlterarSituacaoAtendimentoNAAPAUseCase(null));
            Assert.Equal("mediator", excecao.ParamName);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_False_Quando_Atendimento_Nao_Encontrado()
        {
            const long atendimentoId = 123;

            _mediatorMock
                .Setup(m => m.Send(It.Is<ObterAtendimentoNAAPAPorIdQuery>(q => q.Id == atendimentoId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((EncaminhamentoNAAPA)null);

            var resultado = await _useCase.Executar(atendimentoId);

            Assert.False(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterAtendimentoNAAPAPorIdQuery>(q => q.Id == atendimentoId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<AtualizarSituacaoAtendimentoNAAPACommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_True_Quando_Atendimento_Encontrado_E_Atualizacao_Sucedida()
        {
            const long atendimentoId = 456;
            var encaminhamento = new EncaminhamentoNAAPA { Id = atendimentoId };

            _mediatorMock
                .Setup(m => m.Send(It.Is<ObterAtendimentoNAAPAPorIdQuery>(q => q.Id == atendimentoId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(encaminhamento);

            _mediatorMock
                .Setup(m => m.Send(It.Is<AtualizarSituacaoAtendimentoNAAPACommand>(c => c.Id == atendimentoId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(atendimentoId);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterAtendimentoNAAPAPorIdQuery>(q => q.Id == atendimentoId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<AtualizarSituacaoAtendimentoNAAPACommand>(c => c.Id == atendimentoId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_False_Quando_Atendimento_Encontrado_Mas_Atualizacao_Falha()
        {
            const long atendimentoId = 789;
            var encaminhamento = new EncaminhamentoNAAPA { Id = atendimentoId };

            _mediatorMock
                .Setup(m => m.Send(It.Is<ObterAtendimentoNAAPAPorIdQuery>(q => q.Id == atendimentoId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(encaminhamento);

            _mediatorMock
                .Setup(m => m.Send(It.Is<AtualizarSituacaoAtendimentoNAAPACommand>(c => c.Id == atendimentoId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var resultado = await _useCase.Executar(atendimentoId);

            Assert.False(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterAtendimentoNAAPAPorIdQuery>(q => q.Id == atendimentoId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<AtualizarSituacaoAtendimentoNAAPACommand>(c => c.Id == atendimentoId), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
