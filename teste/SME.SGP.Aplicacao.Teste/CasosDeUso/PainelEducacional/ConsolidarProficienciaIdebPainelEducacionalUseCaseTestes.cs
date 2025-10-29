using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoProficienciaIdeb;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdebParaConsolidacao;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.ProficienciaIdeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsolidarProficienciaIdebPainelEducacionalUseCaseTestes
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsolidarProficienciaIdebPainelEducacionalUseCase _consolidarProficienciaIdebPainelEducacionalUseCase;

        public ConsolidarProficienciaIdebPainelEducacionalUseCaseTestes()
        {
            _mediatorMock = new Mock<IMediator>();
            _consolidarProficienciaIdebPainelEducacionalUseCase = new ConsolidarProficienciaIdebPainelEducacionalUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task DadoUmaMensagemRabbitValida_QuandoExecutar_EntaoDeveConsolidarOsDadosESalvarCorretamente()
        {
            // Arrange
            const int anoLetivo = 2023;
            var filtro = new FiltroConsolidacaoProficienciaIdebDto(anoLetivo);
            var jsonFiltro = JsonConvert.SerializeObject(filtro);

            var mensagemRabbit = new MensagemRabbit("", jsonFiltro, Guid.NewGuid(), "usuarioRF");

            var dadosConsolidados = new List<PainelEducacionalConsolidacaoProficienciaIdebUe>
            {
                new PainelEducacionalConsolidacaoProficienciaIdebUe { Id = 1, AnoLetivo = anoLetivo, CodigoUe = "UE1" }
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterProficienciaIdebParaConsolidacaoQuery>(q => q.AnoLetivo == anoLetivo),
                                          It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dadosConsolidados);

            _mediatorMock.Setup(m => m.Send(It.Is<SalvarConsolidacaoProficienciaIdebCommand>(c => c.consolidacaoIdebUe.Any()),
                                          It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var resultado = await _consolidarProficienciaIdebPainelEducacionalUseCase.Executar(mensagemRabbit);

            // Assert
            resultado.Should().BeTrue();

            _mediatorMock.Verify(m => m.Send(It.Is<ObterProficienciaIdebParaConsolidacaoQuery>(q => q.AnoLetivo == anoLetivo),
                                            It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<SalvarConsolidacaoProficienciaIdebCommand>(c => c.consolidacaoIdebUe.Count() == 1 && c.consolidacaoIdebUe.First().CodigoUe == "UE1"),
                                            It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}