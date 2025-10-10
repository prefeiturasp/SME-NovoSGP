using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PendenciaRegistroIndividualJobs
{
    public class GerarPendenciaAusenciaRegistroIndividualUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IGerarPendenciaAusenciaRegistroIndividualUseCase _useCase;

        public GerarPendenciaAusenciaRegistroIndividualUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new GerarPendenciaAusenciaRegistroIndividualUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Parametro_Sistema_Nao_Configurado_Deve_Retornar_True()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(string.Empty);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmasPorAnoModalidadeQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Nao_Encontrar_Turmas_Deve_Logar_E_Retornar_False()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync("HABILITADO");
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasPorAnoModalidadeQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(Enumerable.Empty<Turma>());

            var resultado = await _useCase.Executar(new MensagemRabbit());

            resultado.Should().BeFalse();
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(c => c.Mensagem.Contains("Não foram encontradas turmas")), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Tipo_Escola_Ignorado_Deve_Pular_Geracao_E_Retornar_True()
        {
            var turmas = new List<Turma> { CriarTurma(TipoEscola.EMEF) };
            ConfigurarCaminhoFelizInicial(turmas);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTipoUeIgnoraGeracaoPendenciasQuery>(q => q.TipoUe == TipoEscola.EMEF), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<GerarPendenciaAusenciaRegistroIndividualTurmaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Geracao_Para_Turma_Retorna_Nulo_Deve_Logar_E_Continuar()
        {
            var turmas = new List<Turma> { CriarTurma(TipoEscola.EMEF, 1) };
            ConfigurarCaminhoFelizInicial(turmas);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GerarPendenciaAusenciaRegistroIndividualTurmaCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((RetornoBaseDto)null);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(c => c.Mensagem.Contains($"turma {turmas.First().Id}")), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Geracao_Para_Turma_Retorna_Erro_Deve_Logar_E_Continuar()
        {
            var turmas = new List<Turma> { CriarTurma(TipoEscola.EMEF, 1) };
            var retornoComErro = new RetornoBaseDto("Erro específico da turma.");
            ConfigurarCaminhoFelizInicial(turmas);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GerarPendenciaAusenciaRegistroIndividualTurmaCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(retornoComErro);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(c => c.Mensagem.Contains(retornoComErro.Mensagens.First())), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Ocorre_Excecao_Deve_Retornar_False()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoQuery>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new Exception("Erro inesperado"));

            var resultado = await _useCase.Executar(new MensagemRabbit());

            resultado.Should().BeFalse();
        }

        [Fact]
        public async Task Executar_Quando_Fluxo_Valido_Deve_Processar_Turmas_E_Retornar_True()
        {
            var turmas = new List<Turma>
            {
                CriarTurma(TipoEscola.EMEF, 1),
                CriarTurma(TipoEscola.EMEF, 2),
                CriarTurma(TipoEscola.CIEJA, 3)
            };
            ConfigurarCaminhoFelizInicial(turmas);

            _mediatorMock.Setup(m => m.Send(It.Is<GerarPendenciaAusenciaRegistroIndividualTurmaCommand>(c => c.Turma.Ue.TipoEscola == TipoEscola.CIEJA), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new RetornoBaseDto("Erro para CIEJA"));

            var resultado = await _useCase.Executar(new MensagemRabbit());

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<GerarPendenciaAusenciaRegistroIndividualTurmaCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(c => c.Mensagem.Contains("Erro para CIEJA")), It.IsAny<CancellationToken>()), Times.Once);
        }

        private void ConfigurarCaminhoFelizInicial(IEnumerable<Turma> turmas)
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync("HABILITADO");
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasPorAnoModalidadeQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(turmas);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterTipoUeIgnoraGeracaoPendenciasQuery>(q => q.TipoUe != TipoEscola.EMEF), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);
            _mediatorMock.Setup(m => m.Send(It.Is<GerarPendenciaAusenciaRegistroIndividualTurmaCommand>(c => c.Turma.Ue.TipoEscola != TipoEscola.CIEJA), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new RetornoBaseDto());
        }

        private Turma CriarTurma(TipoEscola tipoEscola, long id = 1)
        {
            return new Turma
            {
                Id = id,
                Ue = new Ue
                {
                    TipoEscola = tipoEscola,
                    Dre = new SME.SGP.Dominio.Dre { Abreviacao = "DRE-AB" },
                    Nome = "ESCOLA TESTE"
                }
            };
        }
    }
}
