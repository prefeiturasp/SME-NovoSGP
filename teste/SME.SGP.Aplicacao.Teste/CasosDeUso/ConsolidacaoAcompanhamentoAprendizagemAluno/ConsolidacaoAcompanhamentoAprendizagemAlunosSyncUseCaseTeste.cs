using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoAcompanhamentoAprendizagemAluno
{
    public class ConsolidacaoAcompanhamentoAprendizagemAlunosSyncUseCaseTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsolidacaoAcompanhamentoAprendizagemAlunosSyncUseCase _useCase;

        public ConsolidacaoAcompanhamentoAprendizagemAlunosSyncUseCaseTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ConsolidacaoAcompanhamentoAprendizagemAlunosSyncUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveConsolidarComSucessoQuandoAtivo()
        {
            var anoAtual = DateTime.Now.Year;
            var parametroAtivo = new ParametrosSistema
            {
                Tipo = TipoParametroSistema.ExecucaoConsolidacaoAcompanhamentoAprendizagemAlunoTurma,
                Ano = anoAtual,
                Ativo = true,
                Valor = "true"
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                q.TipoParametroSistema == TipoParametroSistema.ExecucaoConsolidacaoAcompanhamentoAprendizagemAlunoTurma &&
                q.Ano == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroAtivo);

            _mediatorMock.Setup(m => m.Send(It.IsAny<LimparConsolidacaoAcompanhamentoAprendizagemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var ues = new List<string> { "UE001", "UE002" };
            _mediatorMock.Setup(m => m.Send(ObterCodigosUEsQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ues);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<AtualizarParametroSistemaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1); 

            var result = await _useCase.Executar(new MensagemRabbit(JsonConvert.SerializeObject(new { })));

            Assert.True(result);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                q.TipoParametroSistema == TipoParametroSistema.ExecucaoConsolidacaoAcompanhamentoAprendizagemAlunoTurma &&
                q.Ano == anoAtual), It.IsAny<CancellationToken>()), Times.Exactly(2)); 
            _mediatorMock.Verify(m => m.Send(It.Is<LimparConsolidacaoAcompanhamentoAprendizagemCommand>(c => c.AnoLetivo == anoAtual), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(ObterCodigosUEsQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => c.Rota == RotasRabbitSgp.ConsolidarAcompanhamentoAprendizagemAlunoPorUE && ((FiltroUEDto)c.Filtros).UeCodigo == "UE001"), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => c.Rota == RotasRabbitSgp.ConsolidarAcompanhamentoAprendizagemAlunoPorUE && ((FiltroUEDto)c.Filtros).UeCodigo == "UE002"), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<AtualizarParametroSistemaCommand>(c => c.Parametro.Ano == anoAtual), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveRetornarFalsoQuandoConsolidacaoNaoEstaAtiva()
        {
            var anoAtual = DateTime.Now.Year;
            var parametroInativo = new ParametrosSistema
            {
                Tipo = TipoParametroSistema.ExecucaoConsolidacaoAcompanhamentoAprendizagemAlunoTurma,
                Ano = anoAtual,
                Ativo = false,
                Valor = "false"
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                q.TipoParametroSistema == TipoParametroSistema.ExecucaoConsolidacaoAcompanhamentoAprendizagemAlunoTurma &&
                q.Ano == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroInativo);

            var result = await _useCase.Executar(new MensagemRabbit(JsonConvert.SerializeObject(new { })));

            Assert.False(result);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                q.TipoParametroSistema == TipoParametroSistema.ExecucaoConsolidacaoAcompanhamentoAprendizagemAlunoTurma &&
                q.Ano == anoAtual), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveRetornarFalsoQuandoParametroDeExecucaoNaoExiste()
        {
            var anoAtual = DateTime.Now.Year;

            _mediatorMock.Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                q.TipoParametroSistema == TipoParametroSistema.ExecucaoConsolidacaoAcompanhamentoAprendizagemAlunoTurma &&
                q.Ano == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ParametrosSistema)null);

            var result = await _useCase.Executar(new MensagemRabbit(JsonConvert.SerializeObject(new { })));

            Assert.False(result);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                q.TipoParametroSistema == TipoParametroSistema.ExecucaoConsolidacaoAcompanhamentoAprendizagemAlunoTurma &&
                q.Ano == anoAtual), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }
    }
}