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

        [Fact]
        public async Task Executar_DeveRetornarPeriodosRegenciaComMultiplasSemanas()
        {
            var turmaCodigo = "456EM";
            long componenteCodigo = 0;
            bool ehRegencia = true;
            int bimestre = 1;
            bool exibirDataFutura = true;

            var usuarioMock = new Usuario { CodigoRf = "12345" };
            usuarioMock.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil { CodigoPerfil = Guid.NewGuid(), NomePerfil = "Teste", Ordem = 1, Tipo = TipoPerfil.UE } });
            usuarioMock.DefinirPerfilAtual(usuarioMock.Perfis.First().CodigoPerfil);

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioMock);

            var turmaMock = new Turma { CodigoTurma = turmaCodigo };
            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == turmaCodigo), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaMock);

            var periodoBimestreMock = new PeriodoEscolarBimestreDto
            {
                Id = 1,
                Bimestre = bimestre,
                PeriodoInicio = new DateTime(2025, 3, 3),
                PeriodoFim = new DateTime(2025, 3, 21), 
                AulaCj = false
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodoEscolaresPorTurmaBimestresAulaCjQuery>(q =>
                q.Turma == turmaMock && q.Bimestre == bimestre && q.AulaCj == usuarioMock.EhSomenteProfessorCj()
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(periodoBimestreMock);

            var result = await _useCase.Executar(turmaCodigo, componenteCodigo, ehRegencia, bimestre, exibirDataFutura);

            Assert.NotNull(result);
            Assert.Equal(3, result.Count());

            Assert.Equal(new DateTime(2025, 3, 3), result.ElementAt(0).DataInicio);
            Assert.Equal(new DateTime(2025, 3, 8), result.ElementAt(0).DataFim);
            Assert.Equal("03/03/25 - 08/03/25", result.ElementAt(0).PeriodoEscolar);

            Assert.Equal(new DateTime(2025, 3, 9), result.ElementAt(1).DataInicio);
            Assert.Equal(new DateTime(2025, 3, 15), result.ElementAt(1).DataFim);
            Assert.Equal("09/03/25 - 15/03/25", result.ElementAt(1).PeriodoEscolar);

            Assert.Equal(new DateTime(2025, 3, 16), result.ElementAt(2).DataInicio);
            Assert.Equal(new DateTime(2025, 3, 21), result.ElementAt(2).DataFim);
            Assert.Equal("16/03/25 - 21/03/25", result.ElementAt(2).PeriodoEscolar);

            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == turmaCodigo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterPeriodoEscolaresPorTurmaBimestresAulaCjQuery>(q =>
                q.Turma == turmaMock && q.Bimestre == bimestre && q.AulaCj == usuarioMock.EhSomenteProfessorCj()
            ), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveRetornarPeriodosRegenciaSemExibirDataFutura()
        {
            var turmaCodigo = "789EM";
            long componenteCodigo = 0;
            bool ehRegencia = true;
            int bimestre = 2;
            bool exibirDataFutura = false;

            var usuarioMock = new Usuario { CodigoRf = "67890" };
            usuarioMock.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil { CodigoPerfil = Guid.NewGuid(), NomePerfil = "Teste", Ordem = 1, Tipo = TipoPerfil.UE } });
            usuarioMock.DefinirPerfilAtual(usuarioMock.Perfis.First().CodigoPerfil);

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioMock);

            var turmaMock = new Turma { CodigoTurma = turmaCodigo };
            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == turmaCodigo), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaMock);

            var dataFutura = DateTime.Now.AddDays(30);
            var periodoBimestreMock = new PeriodoEscolarBimestreDto
            {
                Id = 1,
                Bimestre = bimestre,
                PeriodoInicio = DateTime.Now.AddDays(-14), 
                PeriodoFim = dataFutura, 
                AulaCj = false
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodoEscolaresPorTurmaBimestresAulaCjQuery>(q =>
                q.Turma == turmaMock && q.Bimestre == bimestre && q.AulaCj == usuarioMock.EhSomenteProfessorCj()
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(periodoBimestreMock);

            var result = await _useCase.Executar(turmaCodigo, componenteCodigo, ehRegencia, bimestre, exibirDataFutura);

            Assert.NotNull(result);
            Assert.True(result.Any());


            foreach (var periodo in result)
            {
                Assert.True(periodo.DataFim <= DateTime.Now.Date.AddDays(1)); 
            }

            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == turmaCodigo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterPeriodoEscolaresPorTurmaBimestresAulaCjQuery>(q =>
                q.Turma == turmaMock && q.Bimestre == bimestre && q.AulaCj == usuarioMock.EhSomenteProfessorCj()
            ), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveRetornarPeriodosComponenteSemExibirDataFutura()
        {
            var turmaCodigo = "123FM";
            long componenteCodigo = 202;
            bool ehRegencia = false;
            int bimestre = 3;
            bool exibirDataFutura = false;

            var usuarioMock = new Usuario { CodigoRf = "11111" };
            usuarioMock.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil { CodigoPerfil = Guid.NewGuid(), NomePerfil = "Teste", Ordem = 1, Tipo = TipoPerfil.UE } });
            usuarioMock.DefinirPerfilAtual(usuarioMock.Perfis.First().CodigoPerfil);

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioMock);

            var dataAtual = DateTime.Now;
            var periodosAulasMock = new List<PeriodoEscolarVerificaRegenciaDto>
            {
                new PeriodoEscolarVerificaRegenciaDto { DataAula = dataAtual.AddDays(-5), AulaCj = false }, 
                new PeriodoEscolarVerificaRegenciaDto { DataAula = dataAtual.AddDays(-4), AulaCj = false }, 
                new PeriodoEscolarVerificaRegenciaDto { DataAula = dataAtual.AddDays(-3), AulaCj = false }, 
                new PeriodoEscolarVerificaRegenciaDto { DataAula = dataAtual.AddDays(1), AulaCj = false },  
                new PeriodoEscolarVerificaRegenciaDto { DataAula = dataAtual.AddDays(2), AulaCj = false },  
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery>(q =>
                q.TurmaCodigo == turmaCodigo && q.ComponentesCodigos.Contains(componenteCodigo) && q.Bimestre == bimestre && q.AulaCj == usuarioMock.EhSomenteProfessorCj()
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(periodosAulasMock);

            var result = await _useCase.Executar(turmaCodigo, componenteCodigo, ehRegencia, bimestre, exibirDataFutura);

            Assert.NotNull(result);
            Assert.Single(result); 

            Assert.Equal(dataAtual.AddDays(-5).Date, result.First().DataInicio.Date);
            Assert.Equal(dataAtual.AddDays(-3).Date, result.First().DataFim.Date);

            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery>(q =>
                q.TurmaCodigo == turmaCodigo && q.ComponentesCodigos.Contains(componenteCodigo) && q.Bimestre == bimestre && q.AulaCj == usuarioMock.EhSomenteProfessorCj()
            ), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveRetornarPeriodosComponenteComQuantidadeNaoDivisivel()
        {
            var turmaCodigo = "999EM";
            long componenteCodigo = 404;
            bool ehRegencia = false;
            int bimestre = 1;
            bool exibirDataFutura = true;

            var usuarioMock = new Usuario { CodigoRf = "22222" };
            usuarioMock.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil { CodigoPerfil = Guid.NewGuid(), NomePerfil = "Teste", Ordem = 1, Tipo = TipoPerfil.UE } });
            usuarioMock.DefinirPerfilAtual(usuarioMock.Perfis.First().CodigoPerfil);

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioMock);

            var periodosAulasMock = new List<PeriodoEscolarVerificaRegenciaDto>
            {
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 6, 2), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 6, 3), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 6, 4), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 6, 5), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 6, 6), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 6, 9), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 6, 10), AulaCj = false },
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery>(q =>
                q.TurmaCodigo == turmaCodigo && q.ComponentesCodigos.Contains(componenteCodigo) && q.Bimestre == bimestre && q.AulaCj == usuarioMock.EhSomenteProfessorCj()
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(periodosAulasMock);

            var result = await _useCase.Executar(turmaCodigo, componenteCodigo, ehRegencia, bimestre, exibirDataFutura);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count()); 

            Assert.Equal(new DateTime(2025, 6, 2), result.ElementAt(0).DataInicio);
            Assert.Equal(new DateTime(2025, 6, 6), result.ElementAt(0).DataFim);
            Assert.Equal("02/06/25 - 06/06/25", result.ElementAt(0).PeriodoEscolar);

            Assert.Equal(new DateTime(2025, 6, 9), result.ElementAt(1).DataInicio);
            Assert.Equal(new DateTime(2025, 6, 10), result.ElementAt(1).DataFim);
            Assert.Equal("09/06/25 - 10/06/25", result.ElementAt(1).PeriodoEscolar);

            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery>(q =>
                q.TurmaCodigo == turmaCodigo && q.ComponentesCodigos.Contains(componenteCodigo) && q.Bimestre == bimestre && q.AulaCj == usuarioMock.EhSomenteProfessorCj()
            ), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveRetornarPeriodosComponenteComQuantidadeDivisivel()
        {
            var turmaCodigo = "777EM";
            long componenteCodigo = 505;
            bool ehRegencia = false;
            int bimestre = 2;
            bool exibirDataFutura = true;

            var usuarioMock = new Usuario { CodigoRf = "33333" };
            usuarioMock.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil { CodigoPerfil = Guid.NewGuid(), NomePerfil = "Teste", Ordem = 1, Tipo = TipoPerfil.UE } });
            usuarioMock.DefinirPerfilAtual(usuarioMock.Perfis.First().CodigoPerfil);

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioMock);

            var periodosAulasMock = new List<PeriodoEscolarVerificaRegenciaDto>
            {
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 7, 1), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 7, 2), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 7, 3), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 7, 4), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 7, 5), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 7, 8), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 7, 9), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 7, 10), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 7, 11), AulaCj = false },
                new PeriodoEscolarVerificaRegenciaDto { DataAula = new DateTime(2025, 7, 12), AulaCj = false },
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery>(q =>
                q.TurmaCodigo == turmaCodigo && q.ComponentesCodigos.Contains(componenteCodigo) && q.Bimestre == bimestre && q.AulaCj == usuarioMock.EhSomenteProfessorCj()
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(periodosAulasMock);

            var result = await _useCase.Executar(turmaCodigo, componenteCodigo, ehRegencia, bimestre, exibirDataFutura);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            Assert.Equal(new DateTime(2025, 7, 1), result.ElementAt(0).DataInicio);
            Assert.Equal(new DateTime(2025, 7, 5), result.ElementAt(0).DataFim);
            Assert.Equal("01/07/25 - 05/07/25", result.ElementAt(0).PeriodoEscolar);

            Assert.Equal(new DateTime(2025, 7, 8), result.ElementAt(1).DataInicio);
            Assert.Equal(new DateTime(2025, 7, 12), result.ElementAt(1).DataFim);
            Assert.Equal("08/07/25 - 12/07/25", result.ElementAt(1).PeriodoEscolar);

            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery>(q =>
                q.TurmaCodigo == turmaCodigo && q.ComponentesCodigos.Contains(componenteCodigo) && q.Bimestre == bimestre && q.AulaCj == usuarioMock.EhSomenteProfessorCj()
            ), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }
    }
}