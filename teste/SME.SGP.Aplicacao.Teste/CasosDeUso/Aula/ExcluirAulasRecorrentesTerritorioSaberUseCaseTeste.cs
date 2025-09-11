using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula
{
    public class ExcluirAulasRecorrentesTerritorioSaberUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ExcluirAulasRecorrentesTerritorioSaberUseCase _useCase;

        public ExcluirAulasRecorrentesTerritorioSaberUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _useCase = new ExcluirAulasRecorrentesTerritorioSaberUseCase(_mediatorMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Executar_DeveExcluirAulasFuturas_QuandoFiltroForValido()
        {
            // Arrange
            var dataHoje = DateTimeExtension.HorarioBrasilia().Date;
            var filtro = new ExcluirAulasRecorrentesComponenteTerritorioSaberDisponibilizadoFiltro
            {
                CodigoTurma = "T123",
                CodigosComponentesCurricularesDisponibilizados = new[] { "111", "222" },
                DataReferenciaAtribuicao = dataHoje.AddDays(-2) // dentro dos 5 dias
            };

            var mensagem = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject(filtro));


            var aulas = new List<AulaConsultaDto>
            {
                new AulaConsultaDto { Id = 10 },
                new AulaConsultaDto { Id = 20 }
            };

            _mediatorMock
            .Setup(m => m.Send(
                It.IsAny<ObterAulasFuturasPorDataBaseTurmaComponentesCurricularesQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(aulas);

            _mediatorMock
                .Setup(m => m.Send(
                    It.IsAny<ExcluirAulaFuturaTerritorioDisponibilizadoCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RetornoBaseDto("Excluído"));

            // Act
            var resultado = await _useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);
            _unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            _unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Once);
            _unitOfWorkMock.Verify(u => u.Rollback(), Times.Never);
            _mediatorMock.Verify(m => m.Send(
                It.IsAny<ExcluirAulaFuturaTerritorioDisponibilizadoCommand>(), 
                It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Executar_DeveExecutarRollback_QuandoHouverErroNaExclusao()
        {
            // Arrange
            var dataHoje = DateTimeExtension.HorarioBrasilia().Date;
            var filtro = new ExcluirAulasRecorrentesComponenteTerritorioSaberDisponibilizadoFiltro
            {
                CodigoTurma = "T123",
                CodigosComponentesCurricularesDisponibilizados = new[] { "111" },
                DataReferenciaAtribuicao = dataHoje.AddDays(-1)
            };

            var mensagem = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject(filtro));

            var aulas = new List<AulaConsultaDto>
            {
                new AulaConsultaDto { Id = 30 }
            };

            _mediatorMock
            .Setup(m => m.Send(
                It.IsAny<ObterAulasFuturasPorDataBaseTurmaComponentesCurricularesQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(aulas);

            _mediatorMock
                .Setup(m => m.Send(
                    It.IsAny<ExcluirAulaFuturaTerritorioDisponibilizadoCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro simulado"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(mensagem));

            _unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            _unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Never);
            _unitOfWorkMock.Verify(u => u.Rollback(), Times.Once);
        }
    }
}
