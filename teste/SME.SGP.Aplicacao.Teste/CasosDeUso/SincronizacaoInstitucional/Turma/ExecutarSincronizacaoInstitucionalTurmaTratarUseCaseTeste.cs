using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.SincronizacaoInstitucional.Turma
{
    public class ExecutarSincronizacaoInstitucionalTurmaTratarUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExecutarSincronizacaoInstitucionalTurmaTratarUseCase _useCase;

        public ExecutarSincronizacaoInstitucionalTurmaTratarUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarSincronizacaoInstitucionalTurmaTratarUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Codigo_Turma_Zero_Deve_Retornar_Verdadeiro()
        {
            var filtro = new MensagemSyncTurmaDto { CodigoTurma = 0 };
            var mensagemRabbit = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<TurmaParaSyncInstitucionalDto>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Turma_Eol_Nula_Deve_Salvar_Log_Critico_E_Retornar_Falso()
        {
            var filtro = new MensagemSyncTurmaDto { CodigoTurma = 123 };
            var mensagemRabbit = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TurmaParaSyncInstitucionalDto)null);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.False(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(c => c.Nivel == LogNivel.Critico && c.Mensagem.Contains(filtro.CodigoTurma.ToString())), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Tratamento_Falha_Deve_Salvar_Log_Negocio_E_Lancar_Excecao()
        {
            var filtro = new MensagemSyncTurmaDto { CodigoTurma = 123 };
            var mensagemRabbit = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };
            var turmaEol = new TurmaParaSyncInstitucionalDto();
            var turmaSgp = new SME.SGP.Dominio.Turma();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(turmaEol);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(turmaSgp);
            _mediatorMock.Setup(m => m.Send(It.IsAny<TrataSincronizacaoInstitucionalTurmaCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagemRabbit));

            Assert.Contains(filtro.CodigoTurma.ToString(), exception.Message);
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(c => c.Nivel == LogNivel.Negocio && c.Observacao == exception.Message), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Ocorre_Excecao_Generica_Deve_Salvar_Log_Negocio_E_Lancar_Excecao()
        {
            var filtro = new MensagemSyncTurmaDto { CodigoTurma = 123 };
            var mensagemRabbit = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };
            var exception = new Exception("Erro genérico");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(mensagemRabbit));

            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(c => c.Nivel == LogNivel.Negocio && c.Observacao == exception.Message), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Fluxo_Sucesso_Deve_Retornar_Verdadeiro()
        {
            var filtro = new MensagemSyncTurmaDto { CodigoTurma = 123, UeId = "UE01" };
            var mensagemRabbit = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };
            var turmaEol = new TurmaParaSyncInstitucionalDto();
            var turmaSgp = new SME.SGP.Dominio.Turma();

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery>(q => q.TurmaId == filtro.CodigoTurma), It.IsAny<CancellationToken>())).ReturnsAsync(turmaEol);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == filtro.CodigoTurma.ToString()), It.IsAny<CancellationToken>())).ReturnsAsync(turmaSgp);
            _mediatorMock.Setup(m => m.Send(It.Is<TrataSincronizacaoInstitucionalTurmaCommand>(c => c.TurmaEOL == turmaEol && c.TurmaSGP == turmaSgp), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
