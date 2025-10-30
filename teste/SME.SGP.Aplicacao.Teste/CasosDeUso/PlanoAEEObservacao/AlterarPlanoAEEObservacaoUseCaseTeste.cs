using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEEObservacao
{
    public class AlterarPlanoAEEObservacaoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AlterarPlanoAEEObservacaoUseCase _useCase;

        public AlterarPlanoAEEObservacaoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new AlterarPlanoAEEObservacaoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Dto_Valido_Deve_Enviar_Comando_E_Retornar_Auditoria_()
        {
            var dto = new PersistenciaPlanoAEEObservacaoDto
            {
                Id = 1,
                PlanoAEEId = 10,
                Observacao = "Teste Obs",
                Usuarios = new List<long> { 123 }
            };

            var auditoriaRetorno = new AuditoriaDto { Id = 1, CriadoEm = DateTime.Now, AlteradoEm = DateTime.Now };

            _mediatorMock.Setup(m => m.Send(
                It.Is<AlterarPlanoAEEObservacaoCommand>(c =>
                    c.Id == dto.Id &&
                    c.PlanoAEEId == dto.PlanoAEEId &&
                    c.Observacao == dto.Observacao &&
                    c.Usuarios == dto.Usuarios
                ),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(auditoriaRetorno);

            var resultado = await _useCase.Executar(dto);

            Assert.NotNull(resultado);
            Assert.Equal(auditoriaRetorno.Id, resultado.Id);
            _mediatorMock.Verify(m => m.Send(
                It.Is<AlterarPlanoAEEObservacaoCommand>(c =>
                    c.Id == dto.Id &&
                    c.PlanoAEEId == dto.PlanoAEEId &&
                    c.Observacao == dto.Observacao &&
                    c.Usuarios == dto.Usuarios
                ),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Dto_Nulo_Deve_Lancar_Null_Reference_Exception_()
        {
            PersistenciaPlanoAEEObservacaoDto dto = null;

            await Assert.ThrowsAsync<NullReferenceException>(() => _useCase.Executar(dto));
        }
    }
}
