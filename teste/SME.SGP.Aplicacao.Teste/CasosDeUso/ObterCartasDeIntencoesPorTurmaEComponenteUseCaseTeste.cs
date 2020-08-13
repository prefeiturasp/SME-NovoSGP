using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterCartasDeIntencoesPorTurmaEComponenteUseCaseTeste
    {
        private readonly ObterCartasDeIntencoesPorTurmaEComponenteUseCase useCase;
        private readonly Mock<IMediator> mediator;

        public ObterCartasDeIntencoesPorTurmaEComponenteUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase= new ObterCartasDeIntencoesPorTurmaEComponenteUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Obter_Carta_Intencoes()
        {
            // Arrange 
            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario() { CodigoRf = "123" });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma());

            mediator.Setup(a => a.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var periodosEscolares = new List<PeriodoEscolar>()
                {
                    new PeriodoEscolar() { Id = 1, Bimestre = 1 },
                    new PeriodoEscolar() { Id = 2, Bimestre = 2 },
                    new PeriodoEscolar() { Id = 3, Bimestre = 3 },
                    new PeriodoEscolar() { Id = 4, Bimestre = 4 },
                };
            mediator.Setup(a => a.Send(It.IsAny<ObterPeridosEscolaresPorTipoCalendarioIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodosEscolares);

            mediator.Setup(a => a.Send(It.IsAny<ObterCartaDeIntencoesPorTurmaEComponenteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CartaIntencoes>()
                {
                    new CartaIntencoes() { Id = 1, Planejamento = "Planejamento bimestre 1", CriadoPor = "Sistema", CriadoEm = DateTime.Parse("2020-08-11"), CriadoRF = "Sistema", PeriodoEscolarId = 1 },
                    new CartaIntencoes() { Id = 2, Planejamento = "Planejamento bimestre 2", CriadoPor = "Sistema", CriadoEm = DateTime.Parse("2020-08-11"), CriadoRF = "Sistema", PeriodoEscolarId = 2 },
                });

            // Act
            var cartas = await useCase.Executar(new Infra.ObterCartaIntencoesDto("123", 512));

            // Assert
            Assert.NotNull(cartas);
            Assert.True(cartas.Count() == periodosEscolares.Count(), "O número de registros esperado é o mesmo número de períodos escolares");
            Assert.True(cartas.First(c => c.PeriodoEscolarId == 1).Auditoria != null, "Auditoria não carregada");
        }

        [Fact]
        public async Task Deve_Obter_Dto_Vazio()
        {
            // Arrange 
            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario() { CodigoRf = "123" });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma());

            mediator.Setup(a => a.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var periodosEscolares = new List<PeriodoEscolar>()
                {
                    new PeriodoEscolar() { Id = 1, Bimestre = 1 },
                    new PeriodoEscolar() { Id = 2, Bimestre = 2 },
                    new PeriodoEscolar() { Id = 3, Bimestre = 3 },
                    new PeriodoEscolar() { Id = 4, Bimestre = 4 },
                };
            mediator.Setup(a => a.Send(It.IsAny<ObterPeridosEscolaresPorTipoCalendarioIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodosEscolares);

            mediator.Setup(a => a.Send(It.IsAny<ObterCartaDeIntencoesPorTurmaEComponenteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CartaIntencoes>());

            // Act
            var cartas = await useCase.Executar(new Infra.ObterCartaIntencoesDto("123", 512));

            // Assert
            Assert.NotNull(cartas);
            Assert.True(cartas.Count() == periodosEscolares.Count(), "O número de registros esperado é o mesmo número de períodos escolares");
        }
    }
}
