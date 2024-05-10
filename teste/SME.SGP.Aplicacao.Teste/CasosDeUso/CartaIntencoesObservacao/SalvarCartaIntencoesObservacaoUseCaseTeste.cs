
using MediatR;
using Moq;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class SalvarCartaIntencoesObservacaoUseCaseTeste
    {
        private readonly SalvarCartaIntencoesObservacaoUseCase useCase;
        private readonly Mock<IMediator> mediator;

        public SalvarCartaIntencoesObservacaoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new SalvarCartaIntencoesObservacaoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Adicionar_Observacao_Carta_de_Intencoes()
        {
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<SalvarCartaIntencoesObservacaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AuditoriaDto()
                {
                    Id = 1
                });

            //Act
            var dto = new SalvarCartaIntencoesObservacaoDto();
            dto.Observacao = "observacao";
            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaIdPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var auditoriaDto = await useCase.Executar("1",1, dto);
            //Asert
            mediator.Verify(x => x.Send(It.IsAny<SalvarCartaIntencoesObservacaoCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(auditoriaDto.Id == 1);
        }
    }
}



