using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Frequencia.BuscaAtiva
{
    public class ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase _useCase;

        public ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Nao_Ha_Consolidacoes_Nao_Deve_Fazer_Nada()
        {
            var filtro = new FiltroNotificacaoAlunosFreqMensalInsuficienteBuscaAtiva { ConsolidacoesFrequenciaMensalInsuficientes = new List<ConsolidacaoFreqAlunoMensalInsuficienteDto>() };
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<object>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Ha_Consolidacoes_Deve_Montar_E_Enviar_Notificacoes()
        {
            var consolidacoes = new List<ConsolidacaoFreqAlunoMensalInsuficienteDto>
            {
                new ConsolidacaoFreqAlunoMensalInsuficienteDto { Ue = "UE TESTE", Turma = "1A", AlunoCodigo = "ALUNO1", Mes = 9, Modalidade = Modalidade.Fundamental, TurmaCodigo = "T1" },
                new ConsolidacaoFreqAlunoMensalInsuficienteDto { Ue = "UE TESTE", Turma = "1A", AlunoCodigo = "ALUNO2", Mes = 9, Modalidade = Modalidade.Fundamental, TurmaCodigo = "T1" }
            };
            var responsaveis = new List<AtribuicaoResponsavelDto> { new AtribuicaoResponsavelDto { CodigoRF = "RF1" } };
            var filtro = new FiltroNotificacaoAlunosFreqMensalInsuficienteBuscaAtiva
            {
                ConsolidacoesFrequenciaMensalInsuficientes = consolidacoes,
                ResponsaveisNotificacao = responsaveis
            };
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunoPorTurmaAlunoCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new AlunoPorTurmaResposta());
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterQdadeRegistrosAcaoAlunoMesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAlunoPorTurmaAlunoCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(consolidacoes.Count));
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterQdadeRegistrosAcaoAlunoMesQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(consolidacoes.Count));
            _mediatorMock.Verify(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(responsaveis.Count));
        }
    }
}
