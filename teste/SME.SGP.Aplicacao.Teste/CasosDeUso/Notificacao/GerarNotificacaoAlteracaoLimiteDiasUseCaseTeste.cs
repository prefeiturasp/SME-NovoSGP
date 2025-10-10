using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;
using Xunit;
using static SME.SGP.Aplicacao.GerarNotificacaoAlteracaoLimiteDiasUseCase;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Notificacao
{
    public class GerarNotificacaoAlteracaoLimiteDiasUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IServicoFechamentoTurmaDisciplina> _servicoFechamentoMock;
        private readonly IGerarNotificacaoAlteracaoLimiteDiasUseCase _useCase;

        public GerarNotificacaoAlteracaoLimiteDiasUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _servicoFechamentoMock = new Mock<IServicoFechamentoTurmaDisciplina>();
            _useCase = new GerarNotificacaoAlteracaoLimiteDiasUseCase(_mediatorMock.Object, _servicoFechamentoMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Acionar_Servico_E_Retornar_True()
        {
            var parametros = new GerarNotificacaoAlteracaoLimiteDiasParametros
            {
                Bimestre = 3,
                TurmaFechamento = new Turma(),
                UsuarioLogado = new Usuario(),
                Ue = new Ue(),
                AlunosComNotaAlterada = "Aluno 1, Aluno 2"
            };
            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(parametros)
            };

            _servicoFechamentoMock
                .Setup(s => s.GerarNotificacaoAlteracaoLimiteDias(It.IsAny<Turma>(), It.IsAny<Usuario>(), It.IsAny<Ue>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            _servicoFechamentoMock.Verify(s => s.GerarNotificacaoAlteracaoLimiteDias(
                It.IsAny<Turma>(),
                It.IsAny<Usuario>(),
                It.IsAny<Ue>(),
                parametros.Bimestre,
                parametros.AlunosComNotaAlterada), Times.Once);
        }
    }
}
