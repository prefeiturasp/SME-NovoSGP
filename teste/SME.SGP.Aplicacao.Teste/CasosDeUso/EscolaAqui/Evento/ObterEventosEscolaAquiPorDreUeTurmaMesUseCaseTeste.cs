using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Evento
{
    public class ObterEventosEscolaAquiPorDreUeTurmaMesUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterEventosEscolaAquiPorDreUeTurmaMesUseCase useCase;

        public ObterEventosEscolaAquiPorDreUeTurmaMesUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterEventosEscolaAquiPorDreUeTurmaMesUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Eventos_Quando_Filtro_Valido()
        {
            var filtro = new FiltroEventosEscolaAquiDto("dre01", "ue01", "turma01", 1, new DateTime(2025, 07, 01));

            var eventos = new List<EventoEADto>
            {
                new EventoEADto
                {
                    evento_id = "evt1",
                    nome = "Evento Teste",
                    data_inicio = DateTime.Now,
                    data_fim = DateTime.Now.AddDays(1)
                }
            };

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterEventosEscolaAquiPorDreUeTurmaMesQuery>(
                    q => q.Dre_id == filtro.CodigoDre &&
                         q.Ue_id == filtro.CodigoUe &&
                         q.Turma_id == filtro.CodigoTurma &&
                         q.ModalidadeCalendario == filtro.ModalidadeCalendario &&
                         q.MesAno == filtro.MesAno),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(eventos);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("Evento Teste", resultado.First().nome);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterEventosEscolaAquiPorDreUeTurmaMesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Sem_Eventos()
        {
            var filtro = new FiltroEventosEscolaAquiDto("dre01", "ue01", "turma01", 1, new DateTime(2025, 07, 01));
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterEventosEscolaAquiPorDreUeTurmaMesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<EventoEADto>());

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Mediator_Falhar()
        {
            var filtro = new FiltroEventosEscolaAquiDto("dre01", "ue01", "turma01", 1, new DateTime(2025, 07, 01));
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterEventosEscolaAquiPorDreUeTurmaMesQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Erro no Mediator"));

            await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.Executar(filtro));
        }

        [Fact]
        public async Task Deve_Chamar_Mediator_Com_Parametros_Corretos()
        {
            var filtro = new FiltroEventosEscolaAquiDto("DRE01", "UE02", "TURMA03", 2, new DateTime(2025, 8, 1));
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterEventosEscolaAquiPorDreUeTurmaMesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<EventoEADto>());

            await useCase.Executar(filtro);

            mediatorMock.Verify(m => m.Send(It.Is<ObterEventosEscolaAquiPorDreUeTurmaMesQuery>(
                q => q.Dre_id == "DRE01" &&
                     q.Ue_id == "UE02" &&
                     q.Turma_id == "TURMA03" &&
                     q.ModalidadeCalendario == 2 &&
                     q.MesAno == new DateTime(2025, 8, 1)), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
