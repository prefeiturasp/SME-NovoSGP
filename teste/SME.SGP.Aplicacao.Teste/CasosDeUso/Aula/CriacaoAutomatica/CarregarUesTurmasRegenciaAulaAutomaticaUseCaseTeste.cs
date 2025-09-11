using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra;
using Xunit;
using System.Threading;
using Newtonsoft.Json;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula.CriacaoAutomatica
{
   
    public class CarregarUesTurmasRegenciaAulaAutomaticaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IRepositorioCache> repositorioCacheMock;
        private readonly CarregarUesTurmasRegenciaAulaAutomaticaUseCase useCase;

        public CarregarUesTurmasRegenciaAulaAutomaticaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            repositorioCacheMock = new Mock<IRepositorioCache>();

            useCase = new CarregarUesTurmasRegenciaAulaAutomaticaUseCase(mediatorMock.Object, repositorioCacheMock.Object);
        }

        [Fact]
        public async Task Executar_DeveRetornarFalse_QuandoParametroExecutacaoManutencaoDesligado()
        {
            // Arrange
            var mensagem = new MensagemRabbit
            {
                Mensagem = new DadosCriacaoAulasAutomaticasCarregamentoDto { CodigoTurma = null }
            };

            mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterExecutarManutencaoAulasInfantilQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mediatorMock
                .Setup(x => x.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task Executar_DeveRetornarTrue_QuandoParametroExecutacaoManutencaoLigado()
        {
            // Arrange

            var dto = new DadosCriacaoAulasAutomaticasCarregamentoDto
            {
                Pagina = 1,
                CodigoTurma = "123456"
            };

            var mensagemRabbit = new MensagemRabbit(JsonConvert.SerializeObject(dto));

            
            mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterExecutarManutencaoAulasInfantilQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma
                {
                    CodigoTurma = "TURMA123",
                    ModalidadeCodigo = Modalidade.Fundamental,
                    Ue = new Ue { CodigoUe = "UE123" },
                    Semestre = 1
                });

            mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1L);

            mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoEscolar> { new PeriodoEscolar() });

            mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterDiasForaDoPeriodoEscolarQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DateTime> { DateTime.Today });

            mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DiaLetivoDto> { new DiaLetivoDto() });

            mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterCodigosComponentesCurricularesRegenciaAulasAutomaticasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { "123" });

            mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterComponentesCurricularesEOLPorTurmasCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ComponenteCurricularEol>
                {
                new ComponenteCurricularEol { Codigo = 123, Regencia = true, Descricao = "Matemática" }
                });

            mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterDadosTurmaEolPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DadosTurmaEolDto { DataInicioTurma = DateTime.Today });

            mediatorMock
                .Setup(x => x.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            repositorioCacheMock
                .Setup(x => x.SalvarAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(resultado);
        }
    }
    
}
