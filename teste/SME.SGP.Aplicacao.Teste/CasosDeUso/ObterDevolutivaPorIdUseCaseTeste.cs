using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterDevolutivaPorIdUseCaseTeste
    {
        private readonly ObterDevolutivaPorIdUseCase useCase;
        private readonly ObterListaDevolutivasPorTurmaComponenteUseCase useCase2;
        private readonly Mock<IMediator> mediator;

        public ObterDevolutivaPorIdUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterDevolutivaPorIdUseCase(mediator.Object);
            useCase2 = new ObterListaDevolutivasPorTurmaComponenteUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Obter_Devolutiva()
        {
            // Arrange 
            mediator.Setup(a => a.Send(It.IsAny<ObterDevolutivaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Devolutiva() { Id = 1, Descricao = "teste", CriadoEm = DateTime.Today });

            mediator.Setup(a => a.Send(It.IsAny<ObterIdsDiariosBordoPorDevolutivaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<long> { 1, 2 });

            // Act
            var devolutiva = await useCase.Executar(1);

            // Assert
            Assert.NotNull(devolutiva);
            Assert.NotNull(devolutiva.Auditoria);
            Assert.True(devolutiva.DiariosIds.Count() == 2);
        }

        [Fact]
        public async Task Deve_Obter_Todas_Devolutivas()
        {
            var devolutivasExistentes = new List<DevolutivaResumoDto>()
            {
                new DevolutivaResumoDto()
                {
                    Id = 1,
                    PeriodoInicio = DateTime.Today.AddDays(-20),
                    PeriodoFim = DateTime.Today.AddDays(20),
                    CriadoEm = DateTime.Today,
                    CriadoPor = "Sistema"
                },
                new DevolutivaResumoDto()
                {
                    Id = 2,
                    PeriodoInicio = DateTime.Today.AddDays(-20),
                    PeriodoFim = DateTime.Today.AddDays(20),
                    CriadoEm = DateTime.Today,
                    CriadoPor = "Sistema"
                },
            };
            // Arrange 
            mediator.Setup(a => a.Send(It.IsAny<ObterListaDevolutivasPorTurmaComponenteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PaginacaoResultadoDto<DevolutivaResumoDto>() { Items = devolutivasExistentes, TotalPaginas = 1, TotalRegistros = 2 });

            // Act
            var devolutivas = await useCase2.Executar(new FiltroListagemDevolutivaDto("1", 534, null));

            // Assert
            Assert.NotNull(devolutivas);
            Assert.True(devolutivas.Items.Count() == 2);
            Assert.True(devolutivas.TotalRegistros == 2);
            Assert.True(devolutivas.TotalPaginas == 1);
            Assert.Contains(devolutivas.Items, i => i.Id == 2);
        }
    }
}
