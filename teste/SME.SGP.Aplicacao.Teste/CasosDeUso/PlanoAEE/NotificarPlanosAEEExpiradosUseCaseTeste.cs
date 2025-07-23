using MediatR;
using Microsoft.Extensions.Configuration;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class NotificarPlanosAEEExpiradosUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IConfiguration> configuration;
        private readonly NotificarPlanosAEEExpiradosUseCase useCase;

        public NotificarPlanosAEEExpiradosUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            configuration = new Mock<IConfiguration>();
            useCase = new NotificarPlanosAEEExpiradosUseCase(mediator.Object, configuration.Object);
        }

        [Fact]
        public async Task Nao_Deve_Executar_Quando_Parametro_Notificacao_Inativo()
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ParametrosSistema)null);

            // Act
            var resultado = await useCase.Executar(new MensagemRabbit());

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Parametro_DataFim_Nao_Encontrado()
        {
            // Arrange
            mediator.Setup(x => x.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q => q.TipoParametroSistema == TipoParametroSistema.GerarNotificacaoPlanoAEE), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = true });

            mediator.Setup(x => x.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q => q.TipoParametroSistema == TipoParametroSistema.DiasGeracaoNotificacoesPlanoAEEExpirado), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ParametrosSistema)null);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(new MensagemRabbit()));
        }

        [Fact]
        public async Task Deve_Executar_Fluxo_Completo_Quando_Todos_Parametros_Validos()
        {
            // Arrange
            var dataFim = DateTime.Today.AddDays(-5);
            var planoExpirado = new SME.SGP.Dominio.PlanoAEE
            {
                Id = 1,
                TurmaId = 1,
                AlunoNome = "Aluno Teste",
                AlunoCodigo = "123"
            };
            var turma = new Turma
            {
                Id = 1,
                Nome = "Turma Teste",
                ModalidadeCodigo = Modalidade.Fundamental,
                Ue = new Ue
                {
                    Nome = "UE Teste",
                    TipoEscola = TipoEscola.EMEF,
                    Dre = new Dre { CodigoDre = "1", Abreviacao = "DRE1" }
                }
            };
            var usuariosCEFAI = new List<long> { 101, 102 };

            mediator.Setup(x => x.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q => q.TipoParametroSistema == TipoParametroSistema.GerarNotificacaoPlanoAEE), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = true });

            mediator.Setup(x => x.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q => q.TipoParametroSistema == TipoParametroSistema.DiasGeracaoNotificacoesPlanoAEEExpirado), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Valor = "5", Ativo = true });

            mediator.Setup(x => x.Send(It.IsAny<ObterPlanosAEEPorDataFimQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SME.SGP.Dominio.PlanoAEE> { planoExpirado });

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(x => x.Send(It.IsAny<ObtemUsuarioCEFAIDaDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuariosCEFAI);

            configuration.Setup(x => x["UrlFrontEnd"]).Returns("http://teste");

            // Act
            var resultado = await useCase.Executar(new MensagemRabbit());

            // Assert
            Assert.True(resultado);
            mediator.Verify(x => x.Send(It.IsAny<GerarNotificacaoPlanoAEECommand>(), It.IsAny<CancellationToken>()), Times.Exactly(usuariosCEFAI.Count));
        }

        [Fact]
        public async Task Deve_Gerar_Notificacao_Com_Descricao_Correta_Para_Educacao_Infantil()
        {
            // Arrange
            var planoExpirado = new SME.SGP.Dominio.PlanoAEE
            {
                Id = 1,
                TurmaId = 1,
                AlunoNome = "Aluno Teste",
                AlunoCodigo = "123"
            };
            var turma = new Turma
            {
                Id = 1,
                Nome = "Turma Teste",
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                Ue = new Ue
                {
                    Nome = "UE Teste",
                    TipoEscola = TipoEscola.EMEI,
                    Dre = new Dre { CodigoDre = "1", Abreviacao = "DRE1" }
                }
            };

            // Configuração dos parâmetros necessários
            mediator.Setup(x => x.Send(
                It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                    q.TipoParametroSistema == TipoParametroSistema.GerarNotificacaoPlanoAEE),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = true });

            mediator.Setup(x => x.Send(
                It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                    q.TipoParametroSistema == TipoParametroSistema.DiasGeracaoNotificacoesPlanoAEEExpirado),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Valor = "5", Ativo = true });

            mediator.Setup(x => x.Send(It.IsAny<ObterPlanosAEEPorDataFimQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SME.SGP.Dominio.PlanoAEE> { planoExpirado });

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(x => x.Send(It.IsAny<ObtemUsuarioCEFAIDaDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<long> { 101 });

            configuration.Setup(x => x["UrlFrontEnd"]).Returns("http://teste");

            string descricaoGerada = null;
            mediator.Setup(x => x.Send(It.IsAny<GerarNotificacaoPlanoAEECommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<bool>, CancellationToken>((cmd, _) =>
                    descricaoGerada = ((GerarNotificacaoPlanoAEECommand)cmd).Descricao)
                .ReturnsAsync(true);

            // Act
            await useCase.Executar(new MensagemRabbit());

            // Assert
            Assert.Contains("da criança", descricaoGerada);
            Assert.Contains("Turma Teste", descricaoGerada);
            Assert.Contains("UE Teste", descricaoGerada);
            Assert.Contains("DRE1", descricaoGerada);
            Assert.Contains("http://teste", descricaoGerada);
        }
    }
}
