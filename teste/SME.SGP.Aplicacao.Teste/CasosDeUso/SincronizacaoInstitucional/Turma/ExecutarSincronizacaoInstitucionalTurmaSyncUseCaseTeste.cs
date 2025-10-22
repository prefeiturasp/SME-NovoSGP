using MediatR;
using Moq;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.SincronizacaoInstitucional.Turma
{
    public class ExecutarSincronizacaoInstitucionalTurmaSyncUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExecutarSincronizacaoInstitucionalTurmaSyncUseCase _useCase;
        private readonly int _anoAtual;
        private readonly int _anoAnterior;

        public ExecutarSincronizacaoInstitucionalTurmaSyncUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarSincronizacaoInstitucionalTurmaSyncUseCase(_mediatorMock.Object);
            _anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            _anoAnterior = _anoAtual - 1;
        }

        [Fact]
        public async Task Executar_Quando_Ue_Id_Nulo_Deve_Salvar_Log_E_Retornar_Verdadeiro()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoLetivoTurmasVigentesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<int>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterCodigosTurmasEOLPorUeIdParaSyncEstruturaInstitucionalQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<long>());

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(c => c.Mensagem.Contains("O codígo da Ue não foi informado")), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Executar_Quando_Nao_Existem_Codigos_De_Turma_Deve_Retornar_Verdadeiro(bool turmasNulas)
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "123456" };
            var turmas = turmasNulas ? null : new List<long>();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoLetivoTurmasVigentesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<int> { _anoAtual, _anoAnterior });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterCodigosTurmasEOLPorUeIdParaSyncEstruturaInstitucionalQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmas);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterUsuarioPorRfQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Anos_Letivos_Nao_Contem_Atual_E_Anterior_Deve_Adicionar()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "123456", CodigoCorrelacao = Guid.NewGuid() };
            var codigosTurma = new List<long> { 987 };
            var usuario = new Usuario();
            var anoDiferente = _anoAtual - 5;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoLetivoTurmasVigentesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<int> { anoDiferente });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterCodigosTurmasEOLPorUeIdParaSyncEstruturaInstitucionalQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(codigosTurma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioPorRfQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterCodigosTurmasEOLPorUeIdParaSyncEstruturaInstitucionalQuery>(
                c => c.AnosLetivos.Contains(anoDiferente) && c.AnosLetivos.Contains(_anoAtual) && c.AnosLetivos.Contains(_anoAnterior)),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Publicacao_Falha_Deve_Salvar_Log_De_Negocio()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "123456", CodigoCorrelacao = Guid.NewGuid() };
            var codigosTurma = new List<long> { 987 };
            var usuario = new Usuario();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoLetivoTurmasVigentesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<int>());
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterCodigosTurmasEOLPorUeIdParaSyncEstruturaInstitucionalQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(codigosTurma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioPorRfQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            await _useCase.Executar(mensagemRabbit);

            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(
                c => c.Mensagem.Contains("Não foi possível inserir a turma de código") && c.Nivel == LogNivel.Negocio),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Ocorre_Excecao_Na_Publicacao_Deve_Salvar_Log_Critico()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "123456", CodigoCorrelacao = Guid.NewGuid() };
            var codigosTurma = new List<long> { 987 };
            var usuario = new Usuario();
            var exception = new InvalidOperationException("Erro na fila");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoLetivoTurmasVigentesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<int>());
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterCodigosTurmasEOLPorUeIdParaSyncEstruturaInstitucionalQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(codigosTurma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioPorRfQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            await _useCase.Executar(mensagemRabbit);

            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(
                c => c.Mensagem.Contains("Ocorreu um erro ao sincronizar a turma") && c.Nivel == LogNivel.Critico && c.Observacao == exception.Message),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Publicacao_Sucesso_Deve_Retornar_Verdadeiro()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "123456", CodigoCorrelacao = Guid.NewGuid() };
            var codigosTurma = new List<long> { 987, 654 };
            var usuario = new Usuario();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoLetivoTurmasVigentesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<int> { _anoAtual });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterCodigosTurmasEOLPorUeIdParaSyncEstruturaInstitucionalQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(codigosTurma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioPorRfQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(c => c.Nivel != LogNivel.Critico), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
