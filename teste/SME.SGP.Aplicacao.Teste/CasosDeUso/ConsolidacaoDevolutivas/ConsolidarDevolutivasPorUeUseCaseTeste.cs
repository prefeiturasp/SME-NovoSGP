using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoDevolutivas
{
    public class ConsolidarDevolutivasPorUeUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsolidarDevolutivasPorUeUseCase useCase;

        public ConsolidarDevolutivasPorUeUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsolidarDevolutivasPorUeUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Retorna_False_Quando_Parametro_Nulo()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((ParametrosSistema)null);

            var resultado = await useCase.Executar(new MensagemRabbit());

            resultado.Should().BeFalse();
        }

        [Fact]
        public async Task Executar_Retorna_False_Quando_Parametro_Desativado()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = false });

            var resultado = await useCase.Executar(new MensagemRabbit());

            resultado.Should().BeFalse();
        }

        [Fact]
        public async Task Executar_Retorna_True_E_Publica_Mensagens_Quando_Parametro_Ativo()
        {
            var uesIds = new List<long> { 1, 2, 3 };
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = true });

            mediatorMock.Setup(m => m.Send(It.Is<ObterUesIdsPorModalidadeCalendarioQuery>(q =>
                    q.ModalidadeTipoCalendario == ModalidadeTipoCalendario.Infantil &&
                    q.AnoLetivo == DateTime.Now.Year), It.IsAny<CancellationToken>()))
                .ReturnsAsync(uesIds);

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(new MensagemRabbit());

            resultado.Should().BeTrue();

            foreach (var ueId in uesIds)
            {
                mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd => cmd.Rota == RotasRabbitSgp.ConsolidarDevolutivasPorTurmaInfantil && (long)cmd.Filtros == ueId), It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        [Fact]
        public async Task Publicar_Mensagem_Consolidar_Devolutivas_Por_Turmas_Infantil_Continua_Execucao_Quando_Excecao_Acontece()
        {
            var uesIds = new List<long> { 1 };
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = true });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesIdsPorModalidadeCalendarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(uesIds);

            mediatorMock.SetupSequence(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro simulado"))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(new MensagemRabbit());

            resultado.Should().BeTrue();

            mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(cmd =>
                cmd.Mensagem.Contains("Consolidar Devolutivas")), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
