using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Pendencias
{
    public class RemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly RemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase useCase;

        public RemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new RemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase(mediator.Object);
        }

        [Fact(DisplayName = "RemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase - Deve executar a rotina mesmo com a mesnagem rabbit nula e efetuar a chamada para a fila de exclusão de pendência por UE")]
        public async Task Deve_Executar_Remover_Pendencias_No_Final_Do_Ano_Letivo_Por_Ano_Com_Mensagem_Rabbit_Nula()
        {
            mediator.Setup(x => x.Send(It.IsAny<ObterIdsDresQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<long>() { 1 });

            var resultado = await useCase.Executar(null);

            Assert.True(resultado);

            var filtros = new FiltroRemoverPendenciaFinalAnoLetivoDto(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 1);

            mediator.Verify(x => x.Send(It.Is<PublicarFilaSgpCommand>(y => y.Rota == RotasRabbitSgpPendencias.RotaExecutarExclusaoPendenciasNoFinalDoAnoLetivoPorUe &&
                ((FiltroRemoverPendenciaFinalAnoLetivoDto)y.Filtros).Equals(filtros)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Executar deve retornar false se o ano for menor que 2014")]
        public async Task Executar_Deve_Retornar_False_Se_Ano_For_Menor_Que_2014()
        {
            var mensagem = new MensagemRabbit { Mensagem = "2010" };

            var resultado = await useCase.Executar(mensagem);

            Assert.False(resultado);
            mediator.Verify(x => x.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Executar deve retornar false se o ano for maior ou igual ao atual")]
        public async Task Executar_Deve_Retornar_False_Se_Ano_For_Maior_Ou_Igual_Ano_Atual()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var mensagem = new MensagemRabbit { Mensagem = anoAtual.ToString() };

            var resultado = await useCase.Executar(mensagem);

            Assert.False(resultado);
            mediator.Verify(x => x.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Executar deve usar o ano vindo da mensagem se for válido")]
        public async Task Executar_Deve_Usar_Ano_Da_Mensagem_Se_For_Valido()
        {
            var anoValido = DateTimeExtension.HorarioBrasilia().Year - 2;
            var mensagem = new MensagemRabbit { Mensagem = anoValido.ToString() };

            mediator.Setup(x => x.Send(It.IsAny<ObterIdsDresQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<long> { 10 });

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);

            mediator.Verify(x => x.Send(It.Is<PublicarFilaSgpCommand>(
                cmd => ((FiltroRemoverPendenciaFinalAnoLetivoDto)cmd.Filtros).AnoLetivo == anoValido
                    && ((FiltroRemoverPendenciaFinalAnoLetivoDto)cmd.Filtros).DreId == 10),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
