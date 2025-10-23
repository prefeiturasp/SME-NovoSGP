
using MediatR;
using Moq;
using SME.SGP.Dominio;
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
    public class SalvarPlanoAulaUseCaseTeste
    {
        private readonly SalvarPlanoAulaUseCase useCase;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IUnitOfWork> unitOfWork;

        public SalvarPlanoAulaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            unitOfWork = new Mock<IUnitOfWork>();
            useCase = new SalvarPlanoAulaUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Adicionar_Novo_Plano_Aula()
        {
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<SalvarPlanoAulaCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new PlanoAulaDto());

            //Act
            var dto = new PlanoAulaDto();
            dto.AulaId = 1;
            dto.Descricao = "Descrição do plano de aula";

            mediator.Setup(a => a.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Aula()
            {
                Id = 1,
                TurmaId = "1"
            });

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Turma()
            {
                Id = 1,
                Nome = "1A"
            });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Usuario()
            {
                Id = 1,
                CodigoRf = "1111111"
            });

            mediator.Setup(a => a.Send(It.IsAny<ObterPlanoAulaPorAulaIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Dominio.PlanoAula()
            {
                Id = 1,
            });

            mediator.Setup(a => a.Send(It.IsAny<ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Dominio.PlanejamentoAnual()
            {
                Id = 1,
            });

            var auditoriaDto = await useCase.Executar(dto);
            //Asert
            mediator.Verify(x => x.Send(It.IsAny<SalvarPlanoAulaCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(true);
        }
    }
}



