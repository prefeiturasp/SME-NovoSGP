using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Ocorrencia
{
    public class ExcluirOcorrenciaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExcluirOcorrenciaUseCase _useCase;

        public ExcluirOcorrenciaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExcluirOcorrenciaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Ids_Nulo_Deve_Retornar_Erro()
        {
            var resultado = await _useCase.Executar(null);

            resultado.ExistemErros.Should().BeTrue();
            resultado.Mensagens.Should().Contain("Devem ser informadas ao menos uma ocorrência para exclusão.");
            _mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirOcorrenciaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Ids_Vazio_Deve_Retornar_Erro()
        {
            var resultado = await _useCase.Executar(new List<long>());

            resultado.ExistemErros.Should().BeTrue();
            resultado.Mensagens.Should().Contain("Devem ser informadas ao menos uma ocorrência para exclusão.");
            _mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirOcorrenciaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Comando_Retorna_Erro_Deve_Agregar_Mensagem()
        {
            var idsParaExcluir = new List<long> { 1, 2 };
            var retornoComErro = new RetornoBaseDto("Ocorrência 1 não encontrada.");

            _mediatorMock.Setup(m => m.Send(It.Is<ExcluirOcorrenciaCommand>(c => c.Id == 1), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(retornoComErro);
            _mediatorMock.Setup(m => m.Send(It.Is<ExcluirOcorrenciaCommand>(c => c.Id == 2), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new RetornoBaseDto());

            var resultado = await _useCase.Executar(idsParaExcluir);

            resultado.ExistemErros.Should().BeTrue();
            resultado.Mensagens.Should().HaveCount(1);
            resultado.Mensagens.Should().Contain("Ocorrência 1 não encontrada.");
            _mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirOcorrenciaCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Executar_Quando_Comando_Retorna_Sucesso_Deve_Retornar_Sem_Erros()
        {
            var idsParaExcluir = new List<long> { 10, 20 };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirOcorrenciaCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new RetornoBaseDto());

            var resultado = await _useCase.Executar(idsParaExcluir);

            resultado.ExistemErros.Should().BeFalse();
            resultado.Mensagens.Should().BeEmpty();
            _mediatorMock.Verify(m => m.Send(It.Is<ExcluirOcorrenciaCommand>(c => c.Id == 10), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ExcluirOcorrenciaCommand>(c => c.Id == 20), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
