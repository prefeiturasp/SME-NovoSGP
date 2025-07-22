using MediatR;
using Moq;
using SME.SGP.Dto;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Comunicados.ObterComunicadosPorId
{
    public class ObterComunicadoEscolaAquiUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterComunicadoEscolaAquiUseCase useCase;

        public ObterComunicadoEscolaAquiUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterComunicadoEscolaAquiUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Comunicado_Com_Id_Correto()
        {
            var comunicadoId = 42;

            var comunicadoEsperado = new ComunicadoCompletoDto
            {
                Id = comunicadoId,
                Titulo = "Comunicado de Teste",
                CriadoEm = DateTime.Today,
                CriadoPor = "Sistema",
                CriadoRF = "1234567",
                Descricao = "Descrição de teste"
            };

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterComunicadoPorIdQuery>(q => q.Id == comunicadoId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(comunicadoEsperado);

            var resultado = await useCase.Executar(comunicadoId);

            Assert.NotNull(resultado);
            Assert.Equal(comunicadoId, resultado.Id);
            Assert.Equal("Comunicado de Teste", resultado.Titulo);
        }

        [Fact]
        public async Task Deve_Retornar_Null_Quando_Comunicado_Nao_Encontrado()
        {
            var comunicadoId = 99L;

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterComunicadoPorIdQuery>(q => q.Id == comunicadoId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ComunicadoCompletoDto)null);

            var resultado = await useCase.Executar(comunicadoId);

            Assert.Null(resultado);
        }
    }
}
