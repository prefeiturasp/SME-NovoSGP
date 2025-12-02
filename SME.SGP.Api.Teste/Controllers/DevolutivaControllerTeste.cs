using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class DevolutivaControllerTeste
    {
        private readonly Mock<IObterListaDevolutivasPorTurmaComponenteUseCase> _listarUseCase;
        private readonly Mock<IObterDevolutivaPorIdUseCase> _obterPorIdUseCase;
        private readonly Mock<IInserirDevolutivaUseCase> _inserirUseCase;
        private readonly Mock<IAlterarDevolutivaUseCase> _alterarUseCase;
        private readonly Mock<IExcluirDevolutivaUseCase> _excluirUseCase;
        private readonly Mock<IObterDataDiarioBordoSemDevolutivaPorTurmaComponenteUseCase> _sugestaoDataUseCase;
        private readonly Mock<IObterPeriodoDeDiasDevolutivaUseCase> _periodoDiasUseCase;

        private readonly DevolutivaController _controller;

        public DevolutivaControllerTeste()
        {
            _listarUseCase = new Mock<IObterListaDevolutivasPorTurmaComponenteUseCase>();
            _obterPorIdUseCase = new Mock<IObterDevolutivaPorIdUseCase>();
            _inserirUseCase = new Mock<IInserirDevolutivaUseCase>();
            _alterarUseCase = new Mock<IAlterarDevolutivaUseCase>();
            _excluirUseCase = new Mock<IExcluirDevolutivaUseCase>();
            _sugestaoDataUseCase = new Mock<IObterDataDiarioBordoSemDevolutivaPorTurmaComponenteUseCase>();
            _periodoDiasUseCase = new Mock<IObterPeriodoDeDiasDevolutivaUseCase>();

            _controller = new DevolutivaController();
        }

        [Fact(DisplayName = "Listar deve retornar Ok com PaginacaoResultadoDto")]
        public async Task Listar_DeveRetornarOk()
        {
            // Arrange
            var turmaCodigo = "1234567";
            var componenteCurricularCodigo = 1L;
            var dataReferencia = DateTime.Now;
            var retorno = new PaginacaoResultadoDto<DevolutivaResumoDto>();

            _listarUseCase
                .Setup(s => s.Executar(It.Is<FiltroListagemDevolutivaDto>(f =>
                    f.TurmaCodigo == turmaCodigo &&
                    f.ComponenteCurricularCodigo == componenteCurricularCodigo &&
                    f.DataReferencia == dataReferencia)))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.Listar(
                turmaCodigo,
                componenteCurricularCodigo,
                dataReferencia,
                _listarUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<PaginacaoResultadoDto<DevolutivaResumoDto>>(ok.Value);
        }

        [Fact(DisplayName = "Listar deve funcionar com dataReferencia nula")]
        public async Task Listar_DeveFuncionarComDataReferenciaNula()
        {
            // Arrange
            var turmaCodigo = "1234567";
            var componenteCurricularCodigo = 1L;
            DateTime? dataReferencia = null;
            var retorno = new PaginacaoResultadoDto<DevolutivaResumoDto>();

            _listarUseCase
                .Setup(s => s.Executar(It.IsAny<FiltroListagemDevolutivaDto>()))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.Listar(
                turmaCodigo,
                componenteCurricularCodigo,
                dataReferencia,
                _listarUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<PaginacaoResultadoDto<DevolutivaResumoDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterPorId deve retornar Ok com DevolutivaDto")]
        public async Task ObterPorId_DeveRetornarOk()
        {
            // Arrange
            var devolutivaId = 1L;
            var retorno = new DevolutivaDto();

            _obterPorIdUseCase
                .Setup(s => s.Executar(devolutivaId))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterPorId(devolutivaId, _obterPorIdUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DevolutivaDto>(ok.Value);
        }

        [Fact(DisplayName = "Salvar deve retornar Ok com AuditoriaDto")]
        public async Task Salvar_DeveRetornarOk()
        {
            // Arrange
            var devolutivaDto = new InserirDevolutivaDto();
            var retorno = new AuditoriaDto();

            _inserirUseCase
                .Setup(s => s.Executar(devolutivaDto))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.Salvar(_inserirUseCase.Object, devolutivaDto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<AuditoriaDto>(ok.Value);
        }

        [Fact(DisplayName = "Alterar deve retornar Ok com AuditoriaDto")]
        public async Task Alterar_DeveRetornarOk()
        {
            // Arrange
            var id = 1L;
            var devolutivaDto = new AlterarDevolutivaDto();
            var retorno = new AuditoriaDto();

            _alterarUseCase
                .Setup(s => s.Executar(It.Is<AlterarDevolutivaDto>(d => d.Id == id)))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.Alterar(id, _alterarUseCase.Object, devolutivaDto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<AuditoriaDto>(ok.Value);
            Assert.Equal(id, devolutivaDto.Id);
        }

        [Fact(DisplayName = "Alterar deve atribuir o Id ao DTO")]
        public async Task Alterar_DeveAtribuirIdAoDto()
        {
            // Arrange
            var id = 5L;
            var devolutivaDto = new AlterarDevolutivaDto { Id = 0 };
            var retorno = new AuditoriaDto();

            _alterarUseCase
                .Setup(s => s.Executar(It.IsAny<AlterarDevolutivaDto>()))
                .ReturnsAsync(retorno);

            // Act
            await _controller.Alterar(id, _alterarUseCase.Object, devolutivaDto);

            // Assert
            Assert.Equal(id, devolutivaDto.Id);
            _alterarUseCase.Verify(
                s => s.Executar(It.Is<AlterarDevolutivaDto>(d => d.Id == id)),
                Times.Once
            );
        }

        [Fact(DisplayName = "Excluir deve retornar Ok com bool")]
        public async Task Excluir_DeveRetornarOk()
        {
            // Arrange
            var devolutivaId = 1L;
            var retorno = true;

            _excluirUseCase
                .Setup(s => s.Executar(devolutivaId))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.Excluir(devolutivaId, _excluirUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<bool>(ok.Value);
            Assert.True((bool)ok.Value);
        }

        [Fact(DisplayName = "SugestaoDataInicio deve retornar Ok com DateTime quando houver valor")]
        public async Task SugestaoDataInicio_DeveRetornarOk()
        {
            // Arrange
            var turmaCodigo = "1234567";
            var componenteCurricularId = 1L;
            var data = DateTime.Now;

            _sugestaoDataUseCase
                .Setup(s => s.Executar(It.Is<FiltroTurmaComponenteDto>(f =>
                    f.TurmaCodigo == turmaCodigo &&
                    f.ComponenteCurricularCodigo == componenteCurricularId)))
                .ReturnsAsync(data);

            // Act
            var result = await _controller.SugestaoDataInicio(
                turmaCodigo,
                componenteCurricularId,
                _sugestaoDataUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DateTime>(ok.Value);
        }

        [Fact(DisplayName = "SugestaoDataInicio deve retornar NoContent quando nulo")]
        public async Task SugestaoDataInicio_DeveRetornarNoContent()
        {
            // Arrange
            var turmaCodigo = "1234567";
            var componenteCurricularId = 1L;

            _sugestaoDataUseCase
                .Setup(s => s.Executar(It.IsAny<FiltroTurmaComponenteDto>()))
                .ReturnsAsync((DateTime?)null);

            // Act
            var result = await _controller.SugestaoDataInicio(
                turmaCodigo,
                componenteCurricularId,
                _sugestaoDataUseCase.Object
            );

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "ObterPeriodoDeDiasDevolutiva deve retornar Ok com string")]
        public async Task ObterPeriodoDeDiasDevolutiva_DeveRetornarOk()
        {
            // Arrange
            var anoLetivo = 2024;
            var retorno = "30";

            _periodoDiasUseCase
                .Setup(s => s.Executar(It.Is<FiltroTipoParametroAnoDto>(f =>
                    f.TipoParametro == TipoParametroSistema.PeriodoDeDiasDevolutiva &&
                    f.AnoLetivo == anoLetivo)))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterPeriodoDeDiasDevolutiva(
                anoLetivo,
                _periodoDiasUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<string>(ok.Value);
            Assert.Equal(retorno, ok.Value);
        }

        [Fact(DisplayName = "ObterPeriodoDeDiasDevolutiva deve verificar chamada do UseCase")]
        public async Task ObterPeriodoDeDiasDevolutiva_DeveVerificarChamadaUseCase()
        {
            // Arrange
            var anoLetivo = 2024;
            var retorno = "30";

            _periodoDiasUseCase
                .Setup(s => s.Executar(It.IsAny<FiltroTipoParametroAnoDto>()))
                .ReturnsAsync(retorno);

            // Act
            await _controller.ObterPeriodoDeDiasDevolutiva(anoLetivo, _periodoDiasUseCase.Object);

            // Assert
            _periodoDiasUseCase.Verify(
                s => s.Executar(It.Is<FiltroTipoParametroAnoDto>(f =>
                    f.TipoParametro == TipoParametroSistema.PeriodoDeDiasDevolutiva &&
                    f.AnoLetivo == anoLetivo)),
                Times.Once
            );
        }
    }
}