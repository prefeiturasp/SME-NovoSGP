using MediatR;
using Moq;
using SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Dashboard.ObterComunicadosParaFiltro
{
    public class ObterComunicadosParaFiltroDaDashboardUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterComunicadosParaFiltroDaDashboardUseCase useCase;

        public ObterComunicadosParaFiltroDaDashboardUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterComunicadosParaFiltroDaDashboardUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Mediator_Com_Parametro_Correto()
        {
            var filtroDto = new ObterComunicadosParaFiltroDaDashboardDto
            {
                AnoLetivo = 2025,
                CodigoDre = "DRE01",
                CodigoUe = "UE01",
                Modalidades = new[] { 1, 2 },
                Semestre = 1,
                AnoEscolar = "5º Ano",
                CodigoTurma = "T01",
                DataEnvioInicial = new DateTime(2025, 1, 1),
                DataEnvioFinal = new DateTime(2025, 12, 31),
                Descricao = "Teste"
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterComunicadosParaFiltroDaDashboardQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ComunicadoParaFiltroDaDashboardDto>());

            await useCase.Executar(filtroDto);

            mediatorMock.Verify(m => m.Send(
                It.Is<ObterComunicadosParaFiltroDaDashboardQuery>(q =>
                    q.AnoLetivo == filtroDto.AnoLetivo &&
                    q.CodigoDre == filtroDto.CodigoDre &&
                    q.CodigoUe == filtroDto.CodigoUe &&
                    q.Modalidades == filtroDto.Modalidades &&
                    q.Semestre == filtroDto.Semestre &&
                    q.AnoEscolar == filtroDto.AnoEscolar &&
                    q.CodigoTurma == filtroDto.CodigoTurma &&
                    q.DataEnvioInicial == filtroDto.DataEnvioInicial &&
                    q.DataEnvioFinal == filtroDto.DataEnvioFinal &&
                    q.Descricao == filtroDto.Descricao
                ),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Lista_Vazia_Quando_Mediator_Retorna_Vazia()
        {
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterComunicadosParaFiltroDaDashboardQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ComunicadoParaFiltroDaDashboardDto>());

            var resultado = await useCase.Executar(new ObterComunicadosParaFiltroDaDashboardDto());

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Lista_Com_Dados_Quando_Mediator_Retorna_Dados()
        {
            var comunicadoDto = new ComunicadoParaFiltroDaDashboardDto
            {
                Id = 1,
                Titulo = "Comunicado Teste",
                DataEnvio = DateTime.Now,
                CodigoDre = "DRE001",
                CodigoUe = "UE001",
                Modalidade = "Modalidade1",
                AgruparModalidade = true,
                TurmasCodigo = new List<ComunicadoTurmaDto>
                {
                    new ComunicadoTurmaDto { CodigoTurma = "Turma01" }
                }
            };

            var listaRetorno = new List<ComunicadoParaFiltroDaDashboardDto> { comunicadoDto };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterComunicadosParaFiltroDaDashboardQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaRetorno);

            var resultado = await useCase.Executar(new ObterComunicadosParaFiltroDaDashboardDto());

            Assert.NotNull(resultado);
            var lista = Assert.IsAssignableFrom<IEnumerable<ComunicadoParaFiltroDaDashboardDto>>(resultado);
            Assert.Single(lista);

            var item = Assert.Single(lista);
            Assert.Equal(comunicadoDto.Id, item.Id);
            Assert.Equal(comunicadoDto.Titulo, item.Titulo);
            Assert.Equal(comunicadoDto.CodigoDre, item.CodigoDre);
            Assert.Equal(comunicadoDto.CodigoUe, item.CodigoUe);
            Assert.Equal(comunicadoDto.Modalidade, item.Modalidade);
            Assert.Equal(comunicadoDto.AgruparModalidade, item.AgruparModalidade);
            Assert.NotNull(item.TurmasCodigo);
            Assert.Single(item.TurmasCodigo);
            Assert.Equal("Turma01", item.TurmasCodigo[0].CodigoTurma);
        }
    }

    public class ObterComunicadosParaFiltroDaDashboardQueryTeste
    {
        [Fact]
        public void Construtor_Deve_Copiar_Propriedades_Do_Dto_Correctamente()
        {
            var dto = new ObterComunicadosParaFiltroDaDashboardDto
            {
                AnoLetivo = 2025,
                CodigoDre = "DRE01",
                CodigoUe = "UE01",
                Modalidades = new[] { 1, 2, 3 },
                Semestre = 2,
                AnoEscolar = "6º Ano",
                CodigoTurma = "T02",
                DataEnvioInicial = new DateTime(2025, 02, 01),
                DataEnvioFinal = new DateTime(2025, 06, 30),
                Descricao = "Descrição teste"
            };

            var query = new ObterComunicadosParaFiltroDaDashboardQuery(dto);

            Assert.Equal(dto.AnoLetivo, query.AnoLetivo);
            Assert.Equal(dto.CodigoDre, query.CodigoDre);
            Assert.Equal(dto.CodigoUe, query.CodigoUe);
            Assert.Equal(dto.Modalidades, query.Modalidades);
            Assert.Equal(dto.Semestre, query.Semestre);
            Assert.Equal(dto.AnoEscolar, query.AnoEscolar);
            Assert.Equal(dto.CodigoTurma, query.CodigoTurma);
            Assert.Equal(dto.DataEnvioInicial, query.DataEnvioInicial);
            Assert.Equal(dto.DataEnvioFinal, query.DataEnvioFinal);
            Assert.Equal(dto.Descricao, query.Descricao);
        }
    }
}
