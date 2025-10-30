using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Responsavel
{
    public class AtribuirUeResponsavelUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IRepositorioSupervisorEscolaDre> repositorioMock;
        private readonly AtribuirUeResponsavelUseCase atribuirUeResponsavelUseCase;

        public AtribuirUeResponsavelUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            repositorioMock = new Mock<IRepositorioSupervisorEscolaDre>();
            atribuirUeResponsavelUseCase = new AtribuirUeResponsavelUseCase(mediatorMock.Object, repositorioMock.Object);
        }

        [Fact]
        public async Task Deve_Sincronizar_Atribuicoes_Com_Sucesso_Quando_Dados_Validos()
        {
            // Arrange
            var atribuicaoDto = new AtribuicaoResponsavelUEDto
            {
                DreId = "108300",
                ResponsavelId = "789123",
                UesIds = new List<string> { "UE-B", "UE-C", "UE-D" }, // Manter B, Adicionar C, Reativar D
                TipoResponsavelAtribuicao = TipoResponsavelAtribuicao.SupervisorEscolar
            };

            var atribuicoesAtuais = new List<SupervisorEscolasDreDto>
            {
                new SupervisorEscolasDreDto { AtribuicaoSupervisorId = 1, EscolaId = "UE-A", AtribuicaoExcluida = false }, // Para remover
                new SupervisorEscolasDreDto { AtribuicaoSupervisorId = 2, EscolaId = "UE-B", AtribuicaoExcluida = false }, // Para manter
                new SupervisorEscolasDreDto { AtribuicaoSupervisorId = 4, EscolaId = "UE-D", AtribuicaoExcluida = true }   // Para reativar
            };

            ConfigurarMocksDeValidacaoSucesso(atribuicaoDto);
            repositorioMock.Setup(r => r.ObtemPorDreESupervisor(atribuicaoDto.DreId, atribuicaoDto.ResponsavelId, true))
                           .ReturnsAsync(atribuicoesAtuais);

            var registroParaReativar = new SupervisorEscolaDre { Id = 4, Excluido = true };
            repositorioMock.Setup(r => r.ObterPorIdAsync(4)).ReturnsAsync(registroParaReativar);

            // Act
            var resultado = await atribuirUeResponsavelUseCase.Executar(atribuicaoDto);

            // Assert
            Assert.True(resultado.AtribuidoComSucesso);
            repositorioMock.Verify(r => r.Salvar(It.Is<SupervisorEscolaDre>(s => s.EscolaId == "UE-C")), Times.Once);
            repositorioMock.Verify(r => r.Remover(1), Times.Once);
            repositorioMock.Verify(r => r.SalvarAsync(It.Is<SupervisorEscolaDre>(s => s.Id == 4 && !s.Excluido)), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_NegocioException_Quando_Ue_Ja_Atribuida_A_Outro_Responsavel()
        {
            // Arrange
            var atribuicaoDto = new AtribuicaoResponsavelUEDto
            {
                DreId = "108300",
                ResponsavelId = "789123",
                UesIds = new List<string> { "UE-A" },
                TipoResponsavelAtribuicao = TipoResponsavelAtribuicao.SupervisorEscolar
            };

            mediatorMock.Setup(m => m.Send(It.Is<ObterDREIdPorCodigoQuery>(q => q.CodigoDre == atribuicaoDto.DreId), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);
            var dreSimulada = new SME.SGP.Dominio.Dre { CodigoDre = atribuicaoDto.DreId };
            var ueCompletaSimulada = new Ue
            {
                Nome = "ESCOLA TESTE",
                TipoEscola = Dominio.TipoEscola.EMEF,
                Dre = dreSimulada // Inclui a DRE para a primeira validação
            };

            mediatorMock.Setup(m => m.Send(It.Is<ObterUeComDrePorCodigoQuery>(q => q.UeCodigo == "UE-A"), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(ueCompletaSimulada);

            repositorioMock.Setup(r => r.VerificarSeJaExisteAtribuicaoAtivaComOutroResponsavelParaAqueleTipoUe(
                (int)atribuicaoDto.TipoResponsavelAtribuicao, "UE-A", atribuicaoDto.DreId, atribuicaoDto.ResponsavelId))
                .ReturnsAsync(1); // Retorna 1, indicando que já existe

            // Act
            // O método é chamado diretamente e seu resultado é capturado.
            var resultado = await atribuirUeResponsavelUseCase.Executar(atribuicaoDto);

            // Assert
            // O teste agora verifica o estado do objeto de resultado, não uma exceção.
            Assert.False(resultado.AtribuidoComSucesso);
            Assert.Contains("já está atribuída para outro responsável", resultado.Mensagem);

            // Garante que nenhuma operação de escrita foi realizada
            repositorioMock.Verify(r => r.Salvar(It.IsAny<SupervisorEscolaDre>()), Times.Never);
            repositorioMock.Verify(r => r.Remover(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Lancar_NegocioException_E_Remover_Atribuicoes_Quando_Responsavel_Invalido()
        {
            // Arrange
            var atribuicaoDto = new AtribuicaoResponsavelUEDto
            {
                DreId = "108300",
                ResponsavelId = "INVALIDO",
                UesIds = new List<string> { "UE-A" },
                TipoResponsavelAtribuicao = TipoResponsavelAtribuicao.SupervisorEscolar
            };

            var atribuicoesAtuais = new List<SupervisorEscolasDreDto>
            {
                new SupervisorEscolasDreDto { AtribuicaoSupervisorId = 10, EscolaId = "UE-X", AtribuicaoExcluida = false },
                new SupervisorEscolasDreDto { AtribuicaoSupervisorId = 11, EscolaId = "UE-Y", AtribuicaoExcluida = false }
            };

            ConfigurarMocksDeValidacaoEntidadesSucesso(atribuicaoDto);
            repositorioMock.Setup(r => r.VerificarSeJaExisteAtribuicaoAtivaComOutroResponsavelParaAqueleTipoUe(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(0);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterSupervisoresPorDreEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SupervisoresRetornoDto>()); // Lista vazia para simular responsável inválido

            repositorioMock.Setup(r => r.ObtemPorDreESupervisor(atribuicaoDto.DreId, atribuicaoDto.ResponsavelId, false))
                           .ReturnsAsync(atribuicoesAtuais);

            // Act
            var resultado = await atribuirUeResponsavelUseCase.Executar(atribuicaoDto);

            // Assert
            Assert.False(resultado.AtribuidoComSucesso);
            Assert.Contains("não é valido para essa atribuição", resultado.Mensagem);
            repositorioMock.Verify(r => r.Remover(10), Times.Once());
            repositorioMock.Verify(r => r.Remover(11), Times.Once());
            repositorioMock.Verify(r => r.Remover(It.IsAny<long>()), Times.Exactly(2));
        }

        #region MÉTODOS PRIVADOS DE SETUP

        private void ConfigurarMocksDeValidacaoEntidadesSucesso(AtribuicaoResponsavelUEDto atribuicaoDto)
        {
            mediatorMock.Setup(m => m.Send(It.Is<ObterDREIdPorCodigoQuery>(q => q.CodigoDre == atribuicaoDto.DreId), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

            foreach (var ueId in atribuicaoDto.UesIds)
            {
                var dreSimulada = new SME.SGP.Dominio.Dre { CodigoDre = atribuicaoDto.DreId };
                var ueSimulada = new Ue { Dre = dreSimulada };

                mediatorMock.Setup(m => m.Send(It.Is<ObterUeComDrePorCodigoQuery>(q => q.UeCodigo == ueId), It.IsAny<CancellationToken>()))
                            .ReturnsAsync(ueSimulada);
            }
        }

        private void ConfigurarMocksDeValidacaoSucesso(AtribuicaoResponsavelUEDto atribuicaoDto)
        {
            ConfigurarMocksDeValidacaoEntidadesSucesso(atribuicaoDto);

            repositorioMock.Setup(r => r.VerificarSeJaExisteAtribuicaoAtivaComOutroResponsavelParaAqueleTipoUe(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(0);

            var responsaveisValidos = new List<SupervisoresRetornoDto>
            {
                new SupervisoresRetornoDto { CodigoRf = atribuicaoDto.ResponsavelId, NomeServidor = "Servidor Teste" }
            };
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterSupervisoresPorDreEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(responsaveisValidos);
        }

        #endregion
    }
}
