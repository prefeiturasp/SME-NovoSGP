using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EncaminhamentoNAAPA;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.NovoEncaminhamentoNAAPA;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class NovoEncaminhamentoNAAPAControllerTeste
    {
        private readonly NovoEncaminhamentoNAAPAController _controller;
        private readonly Mock<IObterSecoesAtendimentoIndividualNAAPAUseCase> _secoesUseCase;
        private readonly Mock<IObterQuestionarioAtendimentoNAAPAUseCase> _questionarioUseCase;
        private readonly Mock<IExisteAtendimentoNAAPAAtivoParaAlunoUseCase> _existeAtivoUseCase;
        private readonly Mock<IObterAtendimentoNAAPAPorIdUseCase> _obterPorIdUseCase;
        private readonly Mock<IUploadDeArquivoUseCase> _uploadUseCase;
        private readonly Mock<IObterNovosEncaminhamentosNAAPAPorTipoUseCase> _paginadoUseCase;
        private readonly Mock<IExcluirArquivoNAAPAUseCase> _excluirArquivoUseCase;
        private readonly Mock<IRegistrarNovoEncaminhamentoNAAPAUseCase> _registrarUseCase;
        private readonly Mock<IExcluirAtendimentoNAAPAUseCase> _excluirEncaminhamentoUseCase;

        public NovoEncaminhamentoNAAPAControllerTeste()
        {
            _secoesUseCase = new Mock<IObterSecoesAtendimentoIndividualNAAPAUseCase>();
            _questionarioUseCase = new Mock<IObterQuestionarioAtendimentoNAAPAUseCase>();
            _existeAtivoUseCase = new Mock<IExisteAtendimentoNAAPAAtivoParaAlunoUseCase>();
            _obterPorIdUseCase = new Mock<IObterAtendimentoNAAPAPorIdUseCase>();
            _uploadUseCase = new Mock<IUploadDeArquivoUseCase>();
            _paginadoUseCase = new Mock<IObterNovosEncaminhamentosNAAPAPorTipoUseCase>();
            _excluirArquivoUseCase = new Mock<IExcluirArquivoNAAPAUseCase>();
            _registrarUseCase = new Mock<IRegistrarNovoEncaminhamentoNAAPAUseCase>();
            _excluirEncaminhamentoUseCase = new Mock<IExcluirAtendimentoNAAPAUseCase>();

            _controller = new NovoEncaminhamentoNAAPAController();
        }

        [Fact(DisplayName = "ObterSecoesDeEncaminhamento deve retornar Ok com lista")]
        public async Task ObterSecoes_DeveRetornarOk()
        {
            var retorno = new List<SecaoQuestionarioDto> { new SecaoQuestionarioDto() };

            _secoesUseCase.Setup(s => s.Executar(It.IsAny<long?>()))
                          .ReturnsAsync(retorno);

            var result = await _controller.ObterSecoesDeEncaminhamento(1, _secoesUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<SecaoQuestionarioDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterQuestionario deve retornar Ok com lista")]
        public async Task ObterQuestionario_DeveRetornarOk()
        {
            var retorno = new List<QuestaoDto> { new QuestaoDto() };

            _questionarioUseCase.Setup(s => s.Executar(It.IsAny<long>(), It.IsAny<long?>(),
                                                       It.IsAny<string>(), It.IsAny<string>()))
                                .ReturnsAsync(retorno);

            var result = await _controller.ObterQuestionario(1, null, "123", "T1", _questionarioUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<QuestaoDto>>(ok.Value);
        }

        [Fact(DisplayName = "ExisteEncaminhamentoAtivoParaAluno deve retornar Ok com booleano")]
        public async Task ExisteEncaminhamento_DeveRetornarOk()
        {
            _existeAtivoUseCase.Setup(s => s.Executar("123"))
                               .ReturnsAsync(true);

            var result = await _controller.ExisteEncaminhamentoAtivoParaAluno("123", _existeAtivoUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)ok.Value);
        }

        [Fact(DisplayName = "ObterEncaminhamento deve retornar Ok com DTO")]
        public async Task ObterEncaminhamento_DeveRetornarOk()
        {
            var dto = new AtendimentoNAAPARespostaDto();

            _obterPorIdUseCase.Setup(s => s.Executar(10)).ReturnsAsync(dto);

            var result = await _controller.ObterEncaminhamento(10, _obterPorIdUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<AtendimentoNAAPARespostaDto>(ok.Value);
        }

        [Fact(DisplayName = "Upload deve retornar Ok quando arquivo possui conteúdo")]
        public async Task Upload_DeveRetornarOk()
        {
            var content = "arquivo fake";
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
            var file = new FormFile(stream, 0, stream.Length, "file", "arquivo.pdf");

            _uploadUseCase.Setup(s => s.Executar(file, Dominio.TipoArquivo.AtendimentoNAAPA))
                          .ReturnsAsync(Guid.NewGuid());

            var result = await _controller.Upload(file, _uploadUseCase.Object);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact(DisplayName = "Upload deve retornar BadRequest quando arquivo estiver vazio")]
        public async Task Upload_DeveRetornarBadRequest()
        {
            var stream = new MemoryStream();
            var file = new FormFile(stream, 0, 0, "file", "arquivo.pdf");

            var result = await _controller.Upload(file, _uploadUseCase.Object);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact(DisplayName = "ObterEncaminhamentosPaginados deve retornar Ok com paginação")]
        public async Task ObterEncaminhamentosPaginados_DeveRetornarOk()
        {
            var paginado = new PaginacaoResultadoDto<NovoEncaminhamentoNAAPAResumoDto>
            {
                Items = new List<NovoEncaminhamentoNAAPAResumoDto>
            {
                new NovoEncaminhamentoNAAPAResumoDto()
            }
            };

            _paginadoUseCase.Setup(s => s.Executar(It.IsAny<FiltroNovoEncaminhamentoNAAPADto>()))
                            .ReturnsAsync(paginado);

            var result = await _controller.ObterEncaminhamentosPaginados(
                            new FiltroNovoEncaminhamentoNAAPADto(),
                            _paginadoUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<PaginacaoResultadoDto<NovoEncaminhamentoNAAPAResumoDto>>(ok.Value);
        }

        [Fact(DisplayName = "ExcluirArquivo deve retornar Ok")]
        public async Task ExcluirArquivo_DeveRetornarOk()
        {
            _excluirArquivoUseCase.Setup(s => s.Executar(It.IsAny<Guid>()))
                                  .ReturnsAsync(true);

            var result = await _controller.ExcluirArquivo(Guid.NewGuid(), _excluirArquivoUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)ok.Value);
        }

        [Fact(DisplayName = "RegistrarNovoEncaminhamento deve retornar Ok com DTO único")]
        public async Task RegistrarNovoEncaminhamento_DeveRetornarOk()
        {
            var dtoEntrada = new NovoEncaminhamentoNAAPADto();

            var dtoRetorno = new ResultadoNovoEncaminhamentoNAAPADto();

            _registrarUseCase
                .Setup(s => s.Executar(It.IsAny<NovoEncaminhamentoNAAPADto>()))
                .ReturnsAsync(dtoRetorno);

            var result = await _controller.RegistrarNovoEncaminhamento(dtoEntrada, _registrarUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<ResultadoNovoEncaminhamentoNAAPADto>(ok.Value);
        }

        [Fact(DisplayName = "ExcluirEncaminhamento deve retornar Ok")]
        public async Task ExcluirEncaminhamento_DeveRetornarOk()
        {
            // Arrange
            long encaminhamentoId = 123;

            _excluirEncaminhamentoUseCase
                .Setup(s => s.Executar(encaminhamentoId))
                .ReturnsAsync(true); 

            // Act
            var result = await _controller.ExcluirEncaminhamento(
                encaminhamentoId,
                _excluirEncaminhamentoUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<bool>(ok.Value);        
            Assert.True((bool)ok.Value);         
        }
    }
}
