using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class DiarioBordoControllerTeste
    {
        private readonly Mock<IObterDiarioBordoUseCase> _obterUseCase;
        private readonly Mock<IObterDiarioBordoPorIdUseCase> _obterPorIdUseCase;
        private readonly Mock<IInserirDiarioBordoUseCase> _inserirUseCase;
        private readonly Mock<IAlterarDiarioBordoUseCase> _alterarUseCase;
        private readonly Mock<IInserirAlterarDiarioBordoUseCase> _inserirAlterarUseCase;
        private readonly Mock<IObterDiariosBordoPorDevolutiva> _obterPorDevolutivaUseCase;
        private readonly Mock<IListarObservacaoDiarioBordoUseCase> _listarObservacoesUseCase;
        private readonly Mock<IObterDiariosDeBordoPorPeriodoUseCase> _obterPorIntervaloUseCase;
        private readonly Mock<IAdicionarObservacaoDiarioBordoUseCase> _adicionarObservacaoUseCase;
        private readonly Mock<IAlterarObservacaoDiarioBordoUseCase> _alterarObservacaoUseCase;
        private readonly Mock<IExcluirObservacaoDiarioBordoUseCase> _excluirObservacaoUseCase;
        private readonly Mock<IObterListagemDiariosDeBordoPorPeriodoUseCase> _obterTitulosUseCase;
        private readonly Mock<IObterUsuarioNotificarDiarioBordoObservacaoUseCase> _obterUsuariosNotificarUseCase;
        private readonly Mock<IObterDatasDiarioBordoPorPeriodoUseCase> _obterDatasUseCase;
        private readonly Mock<IExcluirDiarioBordoUseCase> _excluirUseCase;

        private readonly DiarioBordoController _controller;

        public DiarioBordoControllerTeste()
        {
            _obterUseCase = new Mock<IObterDiarioBordoUseCase>();
            _obterPorIdUseCase = new Mock<IObterDiarioBordoPorIdUseCase>();
            _inserirUseCase = new Mock<IInserirDiarioBordoUseCase>();
            _alterarUseCase = new Mock<IAlterarDiarioBordoUseCase>();
            _inserirAlterarUseCase = new Mock<IInserirAlterarDiarioBordoUseCase>();
            _obterPorDevolutivaUseCase = new Mock<IObterDiariosBordoPorDevolutiva>();
            _listarObservacoesUseCase = new Mock<IListarObservacaoDiarioBordoUseCase>();
            _obterPorIntervaloUseCase = new Mock<IObterDiariosDeBordoPorPeriodoUseCase>();
            _adicionarObservacaoUseCase = new Mock<IAdicionarObservacaoDiarioBordoUseCase>();
            _alterarObservacaoUseCase = new Mock<IAlterarObservacaoDiarioBordoUseCase>();
            _excluirObservacaoUseCase = new Mock<IExcluirObservacaoDiarioBordoUseCase>();
            _obterTitulosUseCase = new Mock<IObterListagemDiariosDeBordoPorPeriodoUseCase>();
            _obterUsuariosNotificarUseCase = new Mock<IObterUsuarioNotificarDiarioBordoObservacaoUseCase>();
            _obterDatasUseCase = new Mock<IObterDatasDiarioBordoPorPeriodoUseCase>();
            _excluirUseCase = new Mock<IExcluirDiarioBordoUseCase>();

            _controller = new DiarioBordoController();
        }

        [Fact(DisplayName = "Obter deve retornar Ok com DiarioBordoDto quando encontrado")]
        public async Task Obter_DeveRetornarOk()
        {
            // Arrange
            var aulaId = 1L;
            var componenteCurricularId = 10;
            var retorno = new DiarioBordoDto();

            _obterUseCase
                .Setup(s => s.Executar(aulaId, componenteCurricularId))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.Obter(componenteCurricularId, _obterUseCase.Object, aulaId);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DiarioBordoDto>(ok.Value);
        }

        [Fact(DisplayName = "Obter deve retornar NoContent quando resultado é nulo")]
        public async Task Obter_DeveRetornarNoContent()
        {
            // Arrange
            var aulaId = 1L;
            var componenteCurricularId = 10;

            _obterUseCase
                .Setup(s => s.Executar(aulaId, componenteCurricularId))
                .ReturnsAsync((DiarioBordoDto)null);

            // Act
            var result = await _controller.Obter(componenteCurricularId, _obterUseCase.Object, aulaId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "ObterPorId deve retornar Ok com DiarioBordoDetalhesDto")]
        public async Task ObterPorId_DeveRetornarOk()
        {
            // Arrange
            var id = 1L;
            var retorno = new DiarioBordoDetalhesDto();

            _obterPorIdUseCase
                .Setup(s => s.Executar(id))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterPorId(_obterPorIdUseCase.Object, id);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DiarioBordoDetalhesDto>(ok.Value);
        }

        [Fact(DisplayName = "Salvar deve retornar Ok com AuditoriaDto")]
        public async Task Salvar_DeveRetornarOk()
        {
            // Arrange
            var dto = new InserirDiarioBordoDto();
            var retorno = new AuditoriaDto();

            _inserirUseCase
                .Setup(s => s.Executar(dto))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.Salvar(_inserirUseCase.Object, dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<AuditoriaDto>(ok.Value);
        }

        [Fact(DisplayName = "Alterar deve retornar Ok com AuditoriaDto")]
        public async Task Alterar_DeveRetornarOk()
        {
            // Arrange
            var dto = new AlterarDiarioBordoDto();
            var retorno = new AuditoriaDto();

            _alterarUseCase
                .Setup(s => s.Executar(dto))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.Alterar(_alterarUseCase.Object, dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<AuditoriaDto>(ok.Value);
        }

        [Fact(DisplayName = "SalvarVarios deve retornar Ok com lista de AuditoriaDiarioBordoDto")]
        public async Task SalvarVarios_DeveRetornarOk()
        {
            // Arrange
            var dtos = new List<InserirAlterarDiarioBordoDto>
            {
                new InserirAlterarDiarioBordoDto(),
                new InserirAlterarDiarioBordoDto()
            };
            var retorno = new List<AuditoriaDiarioBordoDto>();

            _inserirAlterarUseCase
                .Setup(s => s.Executar(dtos))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.SalvarVarios(_inserirAlterarUseCase.Object, dtos);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<AuditoriaDiarioBordoDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterPorDevolutiva deve retornar Ok com PaginacaoResultadoDto")]
        public async Task ObterPorDevolutiva_DeveRetornarOk()
        {
            // Arrange
            var devolutivaId = 1L;
            var anoLetivo = 2024;
            var retorno = new PaginacaoResultadoDto<DiarioBordoDevolutivaDto>();

            _obterPorDevolutivaUseCase
                .Setup(s => s.Executar(devolutivaId, anoLetivo))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterPorDevolutiva(_obterPorDevolutivaUseCase.Object, devolutivaId, anoLetivo);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>>(ok.Value);
        }

        [Fact(DisplayName = "ListarObservacoes deve retornar Ok com lista")]
        public async Task ListarObservacoes_DeveRetornarOk()
        {
            // Arrange
            var diarioBordoId = 1L;
            var retorno = new List<ListarObservacaoDiarioBordoDto>
            {
                new ListarObservacaoDiarioBordoDto()
            };

            _listarObservacoesUseCase
                .Setup(s => s.Executar(diarioBordoId))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ListarObservacoes(diarioBordoId, _listarObservacoesUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<ListarObservacaoDiarioBordoDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterPorIntervalo deve retornar Ok com PaginacaoResultadoDto")]
        public async Task ObterPorIntervalo_DeveRetornarOk()
        {
            // Arrange
            var turmaCodigo = "1234567";
            var componenteCurricularId = 1L;
            var dataInicio = DateTime.Now.AddDays(-30);
            var dataFim = DateTime.Now;
            var retorno = new PaginacaoResultadoDto<DiarioBordoDevolutivaDto>();

            _obterPorIntervaloUseCase
                .Setup(s => s.Executar(It.Is<FiltroTurmaComponentePeriodoDto>(f =>
                    f.TurmaCodigo == turmaCodigo &&
                    f.ComponenteCurricularCodigo == componenteCurricularId &&
                    f.PeriodoInicio == dataInicio &&
                    f.PeriodoFim == dataFim)))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterPorIntervalo(
                _obterPorIntervaloUseCase.Object,
                turmaCodigo,
                componenteCurricularId,
                dataInicio,
                dataFim
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>>(ok.Value);
        }

        [Fact(DisplayName = "AdicionarObservacao deve retornar Ok com AuditoriaDto")]
        public async Task AdicionarObservacao_DeveRetornarOk()
        {
            // Arrange
            var diarioBordoId = 1L;
            var dto = new ObservacaoDiarioBordoDto
            {
                Observacao = "Teste",
                UsuariosIdNotificacao = new List<long> { 1, 2 }
            };
            var retorno = new AuditoriaDto();

            _adicionarObservacaoUseCase
                .Setup(s => s.Executar(dto.Observacao, diarioBordoId, dto.UsuariosIdNotificacao))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.AdicionarObservacao(
                diarioBordoId,
                dto,
                _adicionarObservacaoUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<AuditoriaDto>(ok.Value);
        }

        [Fact(DisplayName = "AlterarObservacao deve retornar Ok com AuditoriaDto")]
        public async Task AlterarObservacao_DeveRetornarOk()
        {
            // Arrange
            var observacaoId = 1L;
            var dto = new ObservacaoDiarioBordoDto
            {
                Observacao = "Teste Alterado",
                UsuariosIdNotificacao = new List<long> { 1, 2, 3 }
            };
            var retorno = new AuditoriaDto();

            _alterarObservacaoUseCase
                .Setup(s => s.Executar(dto.Observacao, observacaoId, dto.UsuariosIdNotificacao))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.AlterarrObservacao(
                observacaoId,
                dto,
                _alterarObservacaoUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<AuditoriaDto>(ok.Value);
        }

        [Fact(DisplayName = "ExcluirObservacao deve retornar Ok com bool")]
        public async Task ExcluirObservacao_DeveRetornarOk()
        {
            // Arrange
            var observacaoId = 1L;
            var retorno = true;

            _excluirObservacaoUseCase
                .Setup(s => s.Executar(observacaoId))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ExcluirObservacao(observacaoId, _excluirObservacaoUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<bool>(ok.Value);
            Assert.True((bool)ok.Value);
        }

        [Fact(DisplayName = "ObterTitulosPorIntervalo deve retornar Ok com PaginacaoResultadoDto")]
        public async Task ObterTitulosPorIntervalo_DeveRetornarOk()
        {
            // Arrange
            var turmaId = 1L;
            var componenteCurricularId = 10L;
            var dataInicio = DateTime.Now.AddDays(-30);
            var dataFim = DateTime.Now;
            var retorno = new PaginacaoResultadoDto<DiarioBordoTituloDto>();

            _obterTitulosUseCase
                .Setup(s => s.Executar(It.Is<FiltroListagemDiarioBordoDto>(f =>
                    f.TurmaId == turmaId &&
                    f.ComponenteCurricularId == componenteCurricularId &&
                    f.DataInicio == dataInicio &&
                    f.DataFim == dataFim)))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterTitulosPorIntervalo(
                _obterTitulosUseCase.Object,
                turmaId,
                componenteCurricularId,
                dataInicio,
                dataFim
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<PaginacaoResultadoDto<DiarioBordoTituloDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterTitulosPorIntervalo deve funcionar com datas nulas")]
        public async Task ObterTitulosPorIntervalo_DeveFuncionarComDatasNulas()
        {
            // Arrange
            var turmaId = 1L;
            var componenteCurricularId = 10L;
            DateTime? dataInicio = null;
            DateTime? dataFim = null;
            var retorno = new PaginacaoResultadoDto<DiarioBordoTituloDto>();

            _obterTitulosUseCase
                .Setup(s => s.Executar(It.IsAny<FiltroListagemDiarioBordoDto>()))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterTitulosPorIntervalo(
                _obterTitulosUseCase.Object,
                turmaId,
                componenteCurricularId,
                dataInicio,
                dataFim
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<PaginacaoResultadoDto<DiarioBordoTituloDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterUsuariosParaNotificar deve retornar Ok com lista")]
        public async Task ObterUsuariosParaNotificar_DeveRetornarOk()
        {
            // Arrange
            var dto = new ObterUsuarioNotificarDiarioBordoObservacaoDto();
            var retorno = new List<UsuarioNotificarDiarioBordoObservacaoDto>
            {
                new UsuarioNotificarDiarioBordoObservacaoDto()
            };

            _obterUsuariosNotificarUseCase
                .Setup(s => s.Executar(dto))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterUsuariosParaNotificar(
                dto,
                _obterUsuariosNotificarUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<UsuarioNotificarDiarioBordoObservacaoDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterDatasDiarioPorPeriodo deve retornar Ok quando encontrado")]
        public async Task ObterDatasDiarioPorPeriodo_DeveRetornarOk()
        {
            // Arrange
            var turmaCodigo = "1234567";
            var dataInicio = DateTime.Now.AddDays(-30);
            var dataFim = DateTime.Now;
            var componenteCurricularId = 1L;
            var retorno = new List<DiarioBordoPorPeriodoDto>
            {
                new DiarioBordoPorPeriodoDto()
            };

            _obterDatasUseCase
                .Setup(s => s.Executar(turmaCodigo, dataInicio, dataFim, componenteCurricularId))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterDatasDiarioPorPeriodo(
                turmaCodigo,
                dataInicio,
                dataFim,
                componenteCurricularId,
                _obterDatasUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<DiarioBordoPorPeriodoDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterDatasDiarioPorPeriodo deve retornar NoContent quando resultado é nulo")]
        public async Task ObterDatasDiarioPorPeriodo_DeveRetornarNoContent()
        {
            // Arrange
            var turmaCodigo = "1234567";
            var dataInicio = DateTime.Now.AddDays(-30);
            var dataFim = DateTime.Now;
            var componenteCurricularId = 1L;

            _obterDatasUseCase
                .Setup(s => s.Executar(turmaCodigo, dataInicio, dataFim, componenteCurricularId))
                .ReturnsAsync((IEnumerable<DiarioBordoPorPeriodoDto>)null);

            // Act
            var result = await _controller.ObterDatasDiarioPorPeriodo(
                turmaCodigo,
                dataInicio,
                dataFim,
                componenteCurricularId,
                _obterDatasUseCase.Object
            );

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "Excluir deve retornar Ok com bool")]
        public async Task Excluir_DeveRetornarOk()
        {
            // Arrange
            var id = 1L;
            var retorno = true;

            _excluirUseCase
                .Setup(s => s.Executar(id))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.Excluir(id, _excluirUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<bool>(ok.Value);
            Assert.True((bool)ok.Value);
        }
    }
}