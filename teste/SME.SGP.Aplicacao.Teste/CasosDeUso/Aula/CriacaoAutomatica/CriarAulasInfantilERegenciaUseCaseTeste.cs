using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Xunit;
using System.Threading;
using Newtonsoft.Json;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula.CriacaoAutomatica
{
    public class CriarAulasInfantilERegenciaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IRepositorioCache> _repositorioCacheMock;
        private readonly CriarAulasInfantilERegenciaUseCase _useCase;

        public CriarAulasInfantilERegenciaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _repositorioCacheMock = new Mock<IRepositorioCache>();
            _useCase = new CriarAulasInfantilERegenciaUseCase(_mediatorMock.Object, _repositorioCacheMock.Object);
        }

        [Fact]
        public async Task Executar_DeveExecutarComando_QuandoJsonForValido()
        {
            // Arrange
            var chaveCache = "chave-valida";
            var mensagem = new MensagemRabbit(chaveCache);

            var turma = new Turma
            {
                Id = 1001,
                CodigoTurma = "1A",
                Nome = "Turma 1A",
                AnoLetivo = 2025,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                Ue = new Ue { Id = 10, Nome = "EMEF Teste", TipoEscola = Dominio.TipoEscola.CCA, Dre = new SME.SGP.Dominio.Dre { Abreviacao = "DRLeste" } }
            };

            var diasLetivos = new List<DiaLetivoDto>
            {
                new DiaLetivoDto
                {
                    Data = new DateTime(2025, 2, 10),
                    EhLetivo = true,
                    PossuiEvento = true
                }
            };

            var dadosDisciplina = new DadosAulaCriadaAutomaticamenteDto(
                dadosDisciplina: ("MAT", "Matemática"),
                quantidadeAulas: 2,
                rfProfessor: "1234567"
            );

            var comando = new CriarAulasInfantilERegenciaAutomaticamenteCommand(
                diasLetivos,
                turma,
                tipoCalendarioId: 2025,
                diasForaDoPeriodoEscolar: new List<DateTime> { new DateTime(2025, 1, 1) },
                codigosDisciplinasConsideradas: new List<string> { "MAT", "POR" },
                dadosAulaCriadaAutomaticamente: dadosDisciplina
            );

            var jsonComando = JsonConvert.SerializeObject(comando);

            _repositorioCacheMock.Setup(r => r.ObterAsync(chaveCache, false)).ReturnsAsync(jsonComando);
            _repositorioCacheMock.Setup(r => r.RemoverAsync(chaveCache)).Returns(Task.CompletedTask);
            _mediatorMock.Setup(m => m.Send(It.IsAny<CriarAulasInfantilERegenciaAutomaticamenteCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var resultado = await _useCase.Executar(mensagem);
                        
            // Assert
            Assert.True(resultado);
            _repositorioCacheMock.Verify(r => r.ObterAsync(chaveCache, false), Times.Once);
            _repositorioCacheMock.Verify(r => r.RemoverAsync(chaveCache), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<CriarAulasInfantilERegenciaAutomaticamenteCommand>(), It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task Executar_DeveRetornarFalse_QuandoJsonForNuloOuVazio()
        {
            // Arrange
            var mensagemKey = "chave-invalida";
            var mensagem = new MensagemRabbit(mensagemKey);
                        
            _repositorioCacheMock.Setup(r => r.Obter(mensagemKey, It.IsAny<bool>())).Returns((string)null);

            // Act
            var resultado = await _useCase.Executar(mensagem);

            // Assert
            Assert.False(resultado);

            _repositorioCacheMock.Verify(r => r.ObterAsync(mensagemKey, false), Times.Once);
            _repositorioCacheMock.Verify(r => r.RemoverAsync(It.IsAny<string>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<bool>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_DeveRetornarFalse_QuandoComandoDesserializadoForNulo()
        {
            // Arrange
            var mensagemKey = "comando-nulo";
            var mensagem = new MensagemRabbit(mensagemKey);

            _repositorioCacheMock.Setup(r => r.ObterAsync(mensagemKey, false)).ReturnsAsync("null"); // Json string "null"
            _repositorioCacheMock.Setup(r => r.RemoverAsync(mensagemKey)).Returns(Task.CompletedTask);

            // Act
            var resultado = await _useCase.Executar(mensagem);

            // Assert
            Assert.False(resultado);

            _repositorioCacheMock.Verify(r => r.ObterAsync(mensagemKey, false), Times.Once);
            _repositorioCacheMock.Verify(r => r.RemoverAsync(mensagemKey), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<bool>>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
