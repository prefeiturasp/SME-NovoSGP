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
    public class GerarPendenciaValidadePlanoAEEUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IConfiguration> configuration;
        private readonly GerarPendenciaValidadePlanoAEEUseCase useCase;

        public GerarPendenciaValidadePlanoAEEUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            configuration = new Mock<IConfiguration>();
            useCase = new GerarPendenciaValidadePlanoAEEUseCase(mediator.Object, configuration.Object);
        }

        [Fact]
        public async Task Nao_Deve_Executar_Quando_Parametro_Geracao_Pendencias_Inativo()
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SME.SGP.Dominio.ParametrosSistema)null);

            // Act
            var resultado = await useCase.Executar(new MensagemRabbit());

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task Deve_Executar_Quando_Parametro_Geracao_Pendencias_Ativo()
        {
            // Arrange
            var parametroAtivo = new SME.SGP.Dominio.ParametrosSistema { Ativo = true, Tipo = TipoParametroSistema.GerarPendenciasPlanoAEE, Ano = DateTime.Today.Year };

            var planoEncerrado = new SME.SGP.Dominio.PlanoAEE { Id = 1, TurmaId = 1, AlunoNome = "Aluno Teste", AlunoCodigo = "123", ResponsavelId = 1 };
            var turma = new Turma
            {
                Id = 1,
                Nome = "Turma Teste",
                ModalidadeCodigo = Modalidade.Fundamental,
                Ue = new Ue
                {
                    Nome = "UE Teste",
                    Dre = new SME.SGP.Dominio.Dre { Abreviacao = "DRE" },
                    TipoEscola = Dominio.TipoEscola.EMEF
                }
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroAtivo);

            mediator.Setup(x => x.Send(It.IsAny<ObterPlanosAEEPorDataFimQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SME.SGP.Dominio.PlanoAEE> { planoEncerrado });

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            configuration.Setup(x => x["UrlFrontEnd"]).Returns("http://teste");

            // Act
            var resultado = await useCase.Executar(new MensagemRabbit());

            // Assert
            Assert.True(resultado);
            mediator.Verify(x => x.Send(It.IsAny<PersistirPlanoAEECommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<GerarPendenciaPlanoAEECommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

