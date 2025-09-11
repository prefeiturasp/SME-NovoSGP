using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.MuralAviso
{
    public class ObterMuralAvisosUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterMuralAvisosUseCase _useCase;

        public ObterMuralAvisosUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterMuralAvisosUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task BuscarPorAulaId_DeveRetornarAvisosQuandoExistirem()
        {
            long aulaId = 123;
            var avisosEsperados = new List<MuralAvisosRetornoDto>
        {
            new MuralAvisosRetornoDto(DateTime.Now.AddDays(-1), "Aviso 1", "email1@teste.com"),
            new MuralAvisosRetornoDto(DateTime.Now, "Aviso 2", "email2@teste.com")
        };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterMuralAvisoPorAulaIdQuery>(q => q.AulaId == aulaId), default))
                .ReturnsAsync(avisosEsperados);

            var resultado = await _useCase.BuscarPorAulaId(aulaId);

            Assert.NotNull(resultado);
            Assert.Equal(avisosEsperados.Count, resultado.Count());
            Assert.Contains(avisosEsperados[0], resultado);
            Assert.Contains(avisosEsperados[1], resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterMuralAvisoPorAulaIdQuery>(q => q.AulaId == aulaId), default), Times.Once);
        }

        [Fact]
        public async Task BuscarPorAulaId_DeveRetornarListaVaziaQuandoNaoExistiremAvisos()
        {
            long aulaId = 456;
            var avisosEsperados = new List<MuralAvisosRetornoDto>();

            _mediatorMock.Setup(m => m.Send(It.Is<ObterMuralAvisoPorAulaIdQuery>(q => q.AulaId == aulaId), default))
                .ReturnsAsync(avisosEsperados);

            var resultado = await _useCase.BuscarPorAulaId(aulaId);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterMuralAvisoPorAulaIdQuery>(q => q.AulaId == aulaId), default), Times.Once);
        }

        [Fact]
        public async Task BuscarPorAulaId_DevePropagarExcecaoQuandoQueryFalhar()
        {
            long aulaId = 789;
            var excecaoEsperada = new Exception("Erro simulado na consulta ao mural de avisos.");

            _mediatorMock.Setup(m => m.Send(It.Is<ObterMuralAvisoPorAulaIdQuery>(q => q.AulaId == aulaId), default))
                .ThrowsAsync(excecaoEsperada);

            var ex = await Assert.ThrowsAsync<Exception>(() => _useCase.BuscarPorAulaId(aulaId));

            Assert.Equal(excecaoEsperada.Message, ex.Message);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterMuralAvisoPorAulaIdQuery>(q => q.AulaId == aulaId), default), Times.Once);
        }

        [Fact]
        public void Construtor_DeveLancarArgumentNullExceptionQuandoMediatorForNulo()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new ObterMuralAvisosUseCase(null));
            Assert.Equal("mediator", ex.ParamName);
        }
    }
}
