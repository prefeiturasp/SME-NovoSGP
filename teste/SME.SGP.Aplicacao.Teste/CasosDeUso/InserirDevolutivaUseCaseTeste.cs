using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class InserirDevolutivaUseCaseTeste
    {
        private readonly InserirDevolutivaUseCase inserirDevolutivaUseCase;
        private readonly Mock<IMediator> mediator;

        public InserirDevolutivaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            inserirDevolutivaUseCase = new InserirDevolutivaUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Inserir_Devolutiva()
        {
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<InserirDevolutivaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Infra.AuditoriaDto()
                {
                    Id = 1
                });

            mediator.Setup(a => a.Send(It.IsAny<AtualizarDiarioBordoComDevolutivaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(a => a.Send(It.IsAny<TurmaEmPeriodoAbertoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(a => a.Send(It.IsAny<ObterBimestreAtualQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { CodigoTurma = "123" });

            mediator.Setup(a => a.Send(It.IsAny<ObterDatasEfetivasDiariosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Tuple<long, DateTime>> {
                    new Tuple<long, DateTime>(1, DateTime.Today.AddDays(-15)),
                    new Tuple<long, DateTime>(2, DateTime.Today.AddDays(-5)),
                    new Tuple<long, DateTime>(3, DateTime.Today.AddDays(-10)),
                    new Tuple<long, DateTime>(4, DateTime.Today.AddDays(5)),
                    new Tuple<long, DateTime>(5, DateTime.Today.AddDays(10)),
                    new Tuple<long, DateTime>(6, DateTime.Today.AddDays(15))
                });

            //Act
            var auditoriaDto = await inserirDevolutivaUseCase.Executar(new Infra.InserirDevolutivaDto()
            {
                CodigoComponenteCurricular = 1,
                Descricao = "teste",
                PeriodoInicio = DateTime.Today.AddDays(-15),
                PeriodoFim = DateTime.Today.AddDays(15),
            });

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<InserirDevolutivaCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(auditoriaDto.Id == 1);
        }
    }
}
