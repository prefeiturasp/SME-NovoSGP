using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Frequencia.BuscaAtiva
{
    public class ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCase _useCase;

        public ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Nao_Eh_Ultimo_Dia_Do_Mes_Deve_Sair()
        {
            var filtro = new FiltroIdAnoLetivoDto(1, new System.DateTime(2025, 9, 29));
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<object>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Nao_Ha_Alunos_Com_Frequencia_Insuficiente_Deve_Sair()
        {
            var dataFiltro = new System.DateTime(2025, 9, 30);
            var filtro = new FiltroIdAnoLetivoDto(1, dataFiltro);
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterConsolidacoesFrequenciaAlunoMensalInsuficientesQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<ConsolidacaoFreqAlunoMensalInsuficienteDto>());

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterConsolidacoesFrequenciaAlunoMensalInsuficientesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Fluxo_Completo_Deve_Buscar_Responsaveis_E_Publicar()
        {
            var dataFiltro = new System.DateTime(2025, 9, 30);
            var ueId = 1L;
            var filtro = new FiltroIdAnoLetivoDto(ueId, dataFiltro);
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };
            var consolidacoes = new List<ConsolidacaoFreqAlunoMensalInsuficienteDto> { new ConsolidacaoFreqAlunoMensalInsuficienteDto() };
            var ueCodigo = "UE-CODIGO";

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterConsolidacoesFrequenciaAlunoMensalInsuficientesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(consolidacoes);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterCodigoUEDREPorIdQuery>(q => q.UeId == ueId), It.IsAny<CancellationToken>())).ReturnsAsync(new DreUeCodigoDto { UeCodigo = ueCodigo });

            var responsavelRf1 = new AtribuicaoResponsavelDto { CodigoRF = "RF1" };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAtribuicaoResponsaveisPorUeTipoQuery>(q => q.CodigoUE == ueCodigo && q.Tipo == TipoResponsavelAtribuicao.PsicologoEscolar), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<AtribuicaoResponsavelDto> { responsavelRf1 });

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAtribuicaoResponsaveisPorUeTipoQuery>(q => q.CodigoUE == ueCodigo && q.Tipo == TipoResponsavelAtribuicao.Psicopedagogo), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<AtribuicaoResponsavelDto> { new AtribuicaoResponsavelDto { CodigoRF = "RF2" } });

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAtribuicaoResponsaveisPorUeTipoQuery>(q => q.CodigoUE == ueCodigo && q.Tipo == TipoResponsavelAtribuicao.AssistenteSocial), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<AtribuicaoResponsavelDto> { responsavelRf1 });

            FiltroNotificacaoAlunosFreqMensalInsuficienteBuscaAtiva filtroPublicacao = null;

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .Callback((IRequest<bool> cmd, CancellationToken token) =>
                         {
                             var command = cmd as PublicarFilaSgpCommand;
                             filtroPublicacao = (FiltroNotificacaoAlunosFreqMensalInsuficienteBuscaAtiva)command.Filtros;
                         })
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAtribuicaoResponsaveisPorUeTipoQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(filtroPublicacao);
            Assert.Equal(2, filtroPublicacao.ResponsaveisNotificacao.Count());
        }
    }
}
