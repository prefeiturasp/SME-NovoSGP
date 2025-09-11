using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PeriodosEscolares
{
    public class ObterPeriodosPorComponenteUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterPeriodosPorComponenteUseCase _useCase;

        public ObterPeriodosPorComponenteUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterPeriodosPorComponenteUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveRetornarPeriodosCorretosParaComponente()
        {
            var turmaCodigo = "456EM";
            long componenteCodigo = 101;
            bool ehRegencia = false;
            int bimestre = 2;
            bool exibirDataFutura = true;

            var usuarioMock = new Usuario { CodigoRf = "54321" };
            usuarioMock.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil { CodigoPerfil = Guid.NewGuid(), NomePerfil = "Teste", Ordem = 1, Tipo = TipoPerfil.UE } });
            usuarioMock.DefinirPerfilAtual(usuarioMock.Perfis.First().CodigoPerfil);

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioMock);

            var periodosAulasMock = new List<PeriodoEscolarVerificaRegenciaDto>
            {
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 5, 5), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 5, 6), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 5, 7), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 5, 8), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 5, 9), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 5, 12), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 5, 13), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 5, 14), AulaCj = false },
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery>(q =>
                q.TurmaCodigo == turmaCodigo && q.ComponentesCodigos.Contains(componenteCodigo) && q.Bimestre == bimestre && q.AulaCj == usuarioMock.EhSomenteProfessorCj()
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(periodosAulasMock);

            var result = await _useCase.Executar(turmaCodigo, componenteCodigo, ehRegencia, bimestre, exibirDataFutura);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            Assert.Equal(new DateTime(2025, 5, 5), result.ElementAt(0).DataInicio);
            Assert.Equal(new DateTime(2025, 5, 9), result.ElementAt(0).DataFim);
            Assert.Equal("05/05/25 - 09/05/25", result.ElementAt(0).PeriodoEscolar);

            Assert.Equal(new DateTime(2025, 5, 12), result.ElementAt(1).DataInicio);
            Assert.Equal(new DateTime(2025, 5, 14), result.ElementAt(1).DataFim);
            Assert.Equal("12/05/25 - 14/05/25", result.ElementAt(1).PeriodoEscolar);

            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery>(q =>
                q.TurmaCodigo == turmaCodigo && q.ComponentesCodigos.Contains(componenteCodigo) && q.Bimestre == bimestre && q.AulaCj == usuarioMock.EhSomenteProfessorCj()
            ), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveRetornarListaVaziaQuandoPeriodoBimestreRegenciaEhNulo()
        {
            var turmaCodigo = "TurmaVaziaRegencia";
            long componenteCodigo = 0;
            bool ehRegencia = true;
            int bimestre = 1;
            bool exibirDataFutura = true;

            var usuarioMock = new Usuario { CodigoRf = "user1" };
            usuarioMock.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil { CodigoPerfil = Guid.NewGuid(), NomePerfil = "Teste", Ordem = 1, Tipo = TipoPerfil.UE } });
            usuarioMock.DefinirPerfilAtual(usuarioMock.Perfis.First().CodigoPerfil);

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioMock);

            var turmaMock = new Turma { CodigoTurma = turmaCodigo };
            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == turmaCodigo), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodoEscolaresPorTurmaBimestresAulaCjQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PeriodoEscolarBimestreDto)null);

            var result = await _useCase.Executar(turmaCodigo, componenteCodigo, ehRegencia, bimestre, exibirDataFutura);

            Assert.NotNull(result);
            Assert.Empty(result);

            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == turmaCodigo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterPeriodoEscolaresPorTurmaBimestresAulaCjQuery>(q =>
                q.Turma == turmaMock && q.Bimestre == bimestre && q.AulaCj == usuarioMock.EhSomenteProfessorCj()
            ), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveRetornarListaVaziaQuandoPeriodosAulasComponenteEhVazio()
        {
            var turmaCodigo = "TurmaVaziaComponente";
            long componenteCodigo = 303;
            bool ehRegencia = false;
            int bimestre = 4;
            bool exibirDataFutura = true;

            var usuarioMock = new Usuario { CodigoRf = "user2" };
            usuarioMock.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil { CodigoPerfil = Guid.NewGuid(), NomePerfil = "Teste", Ordem = 1, Tipo = TipoPerfil.UE } });
            usuarioMock.DefinirPerfilAtual(usuarioMock.Perfis.First().CodigoPerfil);

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoEscolarVerificaRegenciaDto>());

            var result = await _useCase.Executar(turmaCodigo, componenteCodigo, ehRegencia, bimestre, exibirDataFutura);

            Assert.NotNull(result);
            Assert.Empty(result);

            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery>(q =>
                q.TurmaCodigo == turmaCodigo && q.ComponentesCodigos.Contains(componenteCodigo) && q.Bimestre == bimestre && q.AulaCj == usuarioMock.EhSomenteProfessorCj()
            ), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarExcecaoQuandoUsuarioLogadoFalhar()
        {
            var turmaCodigo = "erroUsuario";
            long componenteCodigo = 1;
            bool ehRegencia = false;
            int bimestre = 1;
            bool exibirDataFutura = false;

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao obter usuário logado"));

            var ex = await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(turmaCodigo, componenteCodigo, ehRegencia, bimestre, exibirDataFutura));
            Assert.Contains("Erro ao obter usuário logado", ex.Message);

            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }
    }
}