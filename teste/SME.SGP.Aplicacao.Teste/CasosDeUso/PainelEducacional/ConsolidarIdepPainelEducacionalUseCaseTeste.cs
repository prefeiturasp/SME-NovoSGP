using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdep;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsolidarIdepPainelEducacionalUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ConsolidarIdepPainelEducacionalUseCase useCase;

        public ConsolidarIdepPainelEducacionalUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ConsolidarIdepPainelEducacionalUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Executar_Consolidacao_Idep_Com_Sucesso()
        {
            var mensagemRabbit = new MensagemRabbit();
            var registrosIdep = new List<PainelEducacionalConsolidacaoIdep>();

            mediator.Setup(x => x.Send(It.IsAny<ObterIdepQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(registrosIdep);

            mediator.Setup(x => x.Send(It.IsAny<InserirConsolidacaoIdepCommand>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediator.Verify(x => x.Send(It.IsAny<ObterIdepQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<InserirConsolidacaoIdepCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_False_Se_InserirConsolidacao_Falhar()
        {
            var mensagemRabbit = new MensagemRabbit();
            var registrosIdep = new List<PainelEducacionalConsolidacaoIdep>();

            mediator.Setup(x => x.Send(It.IsAny<ObterIdepQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(registrosIdep);

            mediator.Setup(x => x.Send(It.IsAny<InserirConsolidacaoIdepCommand>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(false);

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.False(resultado);
            mediator.Verify(x => x.Send(It.IsAny<ObterIdepQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<InserirConsolidacaoIdepCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Chamar_Comandos_Na_Ordem_Correta()
        {
            var mensagemRabbit = new MensagemRabbit();
            var registrosIdep = new List<PainelEducacionalConsolidacaoIdep>();
            var chamadas = new List<string>();

            mediator.Setup(x => x.Send(It.IsAny<ObterIdepQuery>(), It.IsAny<CancellationToken>()))
                   .Callback(() => chamadas.Add("ObterIdepCommand"))
                   .ReturnsAsync(registrosIdep);

            mediator.Setup(x => x.Send(It.IsAny<InserirConsolidacaoIdepCommand>(), It.IsAny<CancellationToken>()))
                   .Callback(() => chamadas.Add("InserirConsolidacaoIdepCommand"))
                   .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            Assert.Equal("ObterIdepCommand", chamadas[0]);
            Assert.Equal("InserirConsolidacaoIdepCommand", chamadas[1]);
        }
    }
}