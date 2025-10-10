using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Ocorrencia
{
    public class ObterOcorrenciasPorAlunoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IObterOcorrenciasPorAlunoUseCase _useCase;

        public ObterOcorrenciasPorAlunoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterOcorrenciasPorAlunoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Turma_Nao_Encontrada_Deve_Lancar_Excecao()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Turma)null);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(CriarFiltroDto()));

            excecao.Message.Should().Be("A Turma informada não foi encontrada");
        }

        [Fact]
        public async Task Executar_Quando_Aluno_Nao_Encontrado_Deve_Lancar_Excecao()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new Turma());
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunoPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((AlunoReduzidoDto)null);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(CriarFiltroDto()));

            excecao.Message.Should().Be("O Aluno informado não foi encontrado");
        }

        [Fact]
        public async Task Executar_Quando_Tipo_Calendario_Nao_Encontrado_Deve_Lancar_Excecao()
        {
            ConfigurarMocksIniciaisValidos();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(0L);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(CriarFiltroDto()));

            excecao.Message.Should().Be("O tipo de calendário da turma não foi encontrado.");
        }

        [Fact]
        public async Task Executar_Quando_Periodos_Escolares_Nao_Encontrados_Deve_Lancar_Excecao()
        {
            ConfigurarMocksIniciaisValidos();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(1L);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((IEnumerable<PeriodoEscolar>)null);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(CriarFiltroDto()));

            excecao.Message.Should().Be("Não foi possível encontrar o período escolar da turma.");
        }

        [Fact]
        public async Task Executar_Quando_Primeiro_Semestre_Deve_Calcular_Periodo_Correto()
        {
            var periodos = CriarPeriodosEscolares();
            ConfigurarMocksDeSucesso(periodos);
            var filtro = CriarFiltroDto(semestre: 1);

            await _useCase.Executar(filtro);

            _mediatorMock.Verify(m => m.Send(
                It.Is<ObterOcorrenciasPorAlunoQuery>(q =>
                    q.PeriodoInicio == periodos[0].PeriodoInicio &&
                    q.PeriodoFim == periodos[1].PeriodoFim),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Segundo_Semestre_Deve_Calcular_Periodo_Correto()
        {
            var periodos = CriarPeriodosEscolares();
            ConfigurarMocksDeSucesso(periodos);
            var filtro = CriarFiltroDto(semestre: 2);

            await _useCase.Executar(filtro);

            _mediatorMock.Verify(m => m.Send(
                It.Is<ObterOcorrenciasPorAlunoQuery>(q =>
                    q.PeriodoInicio == periodos[2].PeriodoInicio &&
                    q.PeriodoFim == periodos[3].PeriodoFim),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        private FiltroTurmaAlunoSemestreDto CriarFiltroDto(long turmaId = 1, long alunoCodigo = 123, int semestre = 1)
        {
            return new FiltroTurmaAlunoSemestreDto(turmaId, alunoCodigo, semestre);
        }

        private void ConfigurarMocksIniciaisValidos()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new Turma());
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunoPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new AlunoReduzidoDto());
        }

        private void ConfigurarMocksDeSucesso(List<PeriodoEscolar> periodos)
        {
            ConfigurarMocksIniciaisValidos();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(1L);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(periodos);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterOcorrenciasPorAlunoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new PaginacaoResultadoDto<OcorrenciasPorAlunoDto>());
        }

        private List<PeriodoEscolar> CriarPeriodosEscolares()
        {
            return new List<PeriodoEscolar>
            {
                new PeriodoEscolar { Bimestre = 1, PeriodoInicio = new DateTime(2025, 2, 3), PeriodoFim = new DateTime(2025, 4, 11) },
                new PeriodoEscolar { Bimestre = 2, PeriodoInicio = new DateTime(2025, 4, 14), PeriodoFim = new DateTime(2025, 6, 27) },
                new PeriodoEscolar { Bimestre = 3, PeriodoInicio = new DateTime(2025, 7, 14), PeriodoFim = new DateTime(2025, 9, 26) },
                new PeriodoEscolar { Bimestre = 4, PeriodoInicio = new DateTime(2025, 9, 29), PeriodoFim = new DateTime(2025, 12, 19) }
            };
        }
    }
}
