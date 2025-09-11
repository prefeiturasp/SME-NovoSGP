using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Comunicados.ObterComunicadosPaginados
{
    public class ObterComunicadosPaginadosEscolaAquiUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterComunicadosPaginadosEscolaAquiUseCase useCase;

        public ObterComunicadosPaginadosEscolaAquiUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterComunicadosPaginadosEscolaAquiUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Executar_Mediator_Send_Com_Filtro_Completo_E_Verificar_Campos()
        {
            var filtro = new FiltroComunicadoDto
            {
                AnoLetivo = 2025,
                DreCodigo = "DRE01",
                UeCodigo = "UE01",
                Modalidades = new[] { 1, 2 },
                Semestre = 1,
                DataEnvioInicio = new DateTime(2025, 01, 01),
                DataEnvioFim = new DateTime(2025, 12, 31),
                DataExpiracaoInicio = new DateTime(2025, 02, 01),
                DataExpiracaoFim = new DateTime(2025, 12, 01),
                Titulo = "Reunião",
                TurmasCodigo = new[] { "TURMA01", "TURMA02" },
                AnosEscolares = new[] { "1º", "2º" },
                TiposEscolas = new[] { 1, 2 }
            };

            var resultadoEsperado = new PaginacaoResultadoDto<ComunicadoListaPaginadaDto>
            {
                Items = new List<ComunicadoListaPaginadaDto>
                {
                    new ComunicadoListaPaginadaDto
                    {
                        Id = 1,
                        Titulo = "Reunião importante",
                        DataEnvio = DateTime.Today,
                        DataExpiracao = DateTime.Today.AddDays(10),
                        ModalidadeCodigo = new[] { 1 },
                        TipoEscolaCodigo = new[] { 2 }
                    }
                },
                TotalPaginas = 1,
                TotalRegistros = 1
            };

            ObterComunicadosPaginadosQuery queryCapturada = null;

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterComunicadosPaginadosQuery>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<PaginacaoResultadoDto<ComunicadoListaPaginadaDto>>, CancellationToken>((req, _) =>
                {
                    queryCapturada = req as ObterComunicadosPaginadosQuery;
                })
                .ReturnsAsync(resultadoEsperado);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Single(resultado.Items);
            Assert.Equal("Reunião importante", ((List<ComunicadoListaPaginadaDto>)resultado.Items)[0].Titulo);
            Assert.Equal(1, resultado.TotalPaginas);
            Assert.Equal(1, resultado.TotalRegistros);

            Assert.NotNull(queryCapturada);
            Assert.Equal(filtro.AnoLetivo, queryCapturada.AnoLetivo);
            Assert.Equal(filtro.DreCodigo, queryCapturada.DreCodigo);
            Assert.Equal(filtro.UeCodigo, queryCapturada.UeCodigo);
            Assert.Equal(filtro.Semestre, queryCapturada.Semestre);
            Assert.Equal(filtro.DataEnvioInicio, queryCapturada.DataEnvioInicio);
            Assert.Equal(filtro.DataEnvioFim, queryCapturada.DataEnvioFim);
            Assert.Equal(filtro.DataExpiracaoInicio, queryCapturada.DataExpiracaoInicio);
            Assert.Equal(filtro.DataExpiracaoFim, queryCapturada.DataExpiracaoFim);
            Assert.Equal(filtro.Titulo, queryCapturada.Titulo);
            Assert.Equal(filtro.Modalidades, queryCapturada.Modalidades);
            Assert.Equal(filtro.TurmasCodigo, queryCapturada.TurmasCodigo);
            Assert.Equal(filtro.AnosEscolares, queryCapturada.AnosEscolares);
            Assert.Equal(filtro.TiposEscolas, queryCapturada.TiposEscolas);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterComunicadosPaginadosQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
