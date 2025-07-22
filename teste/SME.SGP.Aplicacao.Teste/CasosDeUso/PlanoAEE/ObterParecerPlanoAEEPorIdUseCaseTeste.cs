using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class ObterParecerPlanoAEEPorIdUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterParecerPlanoAEEPorIdUseCase useCase;

        public ObterParecerPlanoAEEPorIdUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterParecerPlanoAEEPorIdUseCase(mediator.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_DTO_Com_Pareceres_E_Permissoes()
        {
            // Arrange
            var planoAEEId = 1L;
            var planoAEE = new SME.SGP.Dominio.PlanoAEE
            {
                Id = planoAEEId,
                TurmaId = 1,
                ParecerCoordenacao = "Parecer CP",
                ParecerPAAI = "Parecer PAAI",
                ResponsavelPaaiId = 2,
                Situacao = SituacaoPlanoAEE.ParecerPAAI
            };

            var usuarioLogado = new SME.SGP.Dominio.Usuario
            {
                Id = 1,
                CodigoRf = "123456",
                PerfilAtual = Perfis.PERFIL_CP
            };

            var responsavel = new SME.SGP.Dominio.Usuario { Id = 2, CodigoRf = "654321" };
            var usuarioCoreSSO = new MeusDadosDto { Nome = "Responsável PAAI" };
            var turma = new SME.SGP.Dominio.Turma { Id = 1, Ue = new SME.SGP.Dominio.Ue { CodigoUe = "1", Dre = new SME.SGP.Dominio.Dre { CodigoDre = "1" } } };

            mediator.Setup(x => x.Send(It.IsAny<ObterPlanoAEEPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(planoAEE);

            mediator.Setup(x => x.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioLogado);

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(responsavel);

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioCoreSSOQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioCoreSSO);

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(x => x.Send(It.IsAny<ObterCodigoDREPorUeIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("1");

            mediator.Setup(x => x.Send(It.IsAny<ObterFuncionariosDreOuUePorPerfisQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FuncionarioUnidadeDto> { new FuncionarioUnidadeDto { Login = "123456" } });

            mediator.Setup(x => x.Send(It.IsAny<EhGestorDaEscolaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(planoAEEId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(planoAEE.ParecerCoordenacao, resultado.ParecerCoordenacao);
            Assert.Equal(planoAEE.ParecerPAAI, resultado.ParecerPAAI);
            Assert.Equal("Responsável PAAI", resultado.ResponsavelNome);
            Assert.Equal("654321", resultado.ResponsavelRF);
        }

        [Theory]
        [InlineData(SituacaoPlanoAEE.ParecerPAAI, "b1916a47-2341-4f2d-8c15-5e245d9e86a0", false, true, false)]
        public async Task Executar_Deve_Retornar_Permissoes_Corretas_Por_Situacao_E_Perfil(
            SituacaoPlanoAEE situacao,
            string perfilString,
            bool podeEditarCP,
            bool podeEditarPAAI,
            bool podeAtribuirResponsavel)
        {

            // Arrange
            // Converte a string para Guid
            var perfil = new Guid(perfilString);
            var planoAEEId = 1L;
            var planoAEE = new SME.SGP.Dominio.PlanoAEE
            {
                Id = planoAEEId,
                TurmaId = 1,
                Situacao = situacao,
                ResponsavelPaaiId = 2
            };

            var usuarioLogado = new SME.SGP.Dominio.Usuario
            {
                Id = 2, // Mesmo ID do responsável para testar PodeEditarParecerPAAI
                CodigoRf = "123456",
                PerfilAtual = perfil
            };

            var turma = new SME.SGP.Dominio.Turma { Id = 1, Ue = new SME.SGP.Dominio.Ue { CodigoUe = "1", Dre = new SME.SGP.Dominio.Dre { CodigoDre = "1" } } };

            mediator.Setup(x => x.Send(It.IsAny<ObterPlanoAEEPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(planoAEE);

            mediator.Setup(x => x.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioLogado);

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(x => x.Send(It.IsAny<EhGestorDaEscolaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(x => x.Send(It.IsAny<ObterFuncionariosDreOuUePorPerfisQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FuncionarioUnidadeDto> { new FuncionarioUnidadeDto { Login = "123456" } });

            // Act
            var resultado = await useCase.Executar(planoAEEId);

            // Assert
            Assert.Equal(podeEditarCP, resultado.PodeEditarParecerCoordenacao);
            Assert.Equal(podeEditarPAAI, resultado.PodeEditarParecerPAAI);
            Assert.Equal(podeAtribuirResponsavel, resultado.PodeAtribuirResponsavel);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_PodeDevolver_False_Para_Professor()
        {
            // Arrange
            var planoAEEId = 1L;
            var planoAEE = new SME.SGP.Dominio.PlanoAEE
            {
                Id = planoAEEId,
                TurmaId = 1,
                Situacao = SituacaoPlanoAEE.ParecerCP // Situação que permite devolução
            };

            var usuarioLogado = new SME.SGP.Dominio.Usuario
            {
                Id = 1,
                CodigoRf = "123456",
                PerfilAtual = Perfis.PERFIL_PROFESSOR
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterPlanoAEEPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(planoAEE);

            mediator.Setup(x => x.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioLogado);

            // Act
            var resultado = await useCase.Executar(planoAEEId);

            // Assert
            Assert.False(resultado.PodeDevolverPlanoAEE);
        }
    }
}
