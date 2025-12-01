using Bogus;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PendenciaAula
{
    public class PendenciaAulaDreUeUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PendenciaAulaDreUeUseCase _useCase;
        private readonly Faker<Ue> _ueFaker;

        public PendenciaAulaDreUeUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new PendenciaAulaDreUeUseCase(_mediatorMock.Object);

            // Configura��o do Bogus para gerar um objeto Ue v�lido usando os tipos de dom�nio reais
            _ueFaker = new Faker<Ue>("pt_BR")
                .RuleFor(u => u.Id, f => f.Random.Long(1, 10000))
                .RuleFor(u => u.CodigoUe, f => f.Random.AlphaNumeric(6))
                .RuleFor(u => u.DreId, f => f.Random.Long(1, 100))
                .RuleFor(u => u.TipoEscola, f => f.PickRandom<Dominio.TipoEscola>()); // Usa o enum real
        }

        private void ConfigurarParametroSistema(TipoParametroSistema tipo, bool ativo)
        {
            var parametro = new ParametrosSistema { Ativo = ativo };

            // O UseCase pode receber um nulo se o par�metro n�o existir, ent�o simulamos isso tamb�m.
            ParametrosSistema retorno = ativo ? parametro : null;

            _mediatorMock.Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q => q.TipoParametroSistema == tipo), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(retorno);
        }

        private void ConfigurarTodosParametros(bool ativos)
        {
            ConfigurarParametroSistema(TipoParametroSistema.ExecutaPendenciaAulaDiarioBordo, ativos);
            ConfigurarParametroSistema(TipoParametroSistema.ExecutaPendenciaAulaAvaliacao, ativos);
            ConfigurarParametroSistema(TipoParametroSistema.ExecutaPendenciaAulaFrequencia, ativos);
            ConfigurarParametroSistema(TipoParametroSistema.ExecutaPendenciaPlanoAula, ativos);
        }

        private void VerificarPublicacaoFila(string rota, Times vezes)
        {
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => c.Rota == rota), It.IsAny<CancellationToken>()), vezes);
        }

        [Fact(DisplayName = "Deve retornar false e n�o processar quando a mensagem for inv�lida")]
        public async Task Executar_QuandoMensagemInvalida_DeveRetornarFalseENaoProcessar()
        {
            // Organiza��o
            var mensagemInvalida = new MensagemRabbit("null");

            // A��o
            var resultado = await _useCase.Executar(mensagemInvalida);

            // Verifica��o
            Assert.False(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<bool>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Deve disparar todas as filas quando a UE n�o ignora pend�ncias e os par�metros est�o ativos")]
        public async Task Executar_QuandoUeNaoIgnoraPendenciasEParametrosAtivos_DeveDispararTodasAsFilas()
        {
            // Organiza��o
            var ue = _ueFaker.Generate();
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(ue));

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoUeIgnoraGeracaoPendenciasQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _mediatorMock.Setup(m => m.Send(PodeExecutarPendenciaComponenteSemAulaQuery.Instance, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            ConfigurarTodosParametros(true);

            // A��o
            var resultado = await _useCase.Executar(mensagem);

            // Verifica��o
            Assert.True(resultado);
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasAulaDiarioBordo, Times.Once());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasAulaFrequencia, Times.Once());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasAulaAvaliacao, Times.Once());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasAulaPlanoAula, Times.Once());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaAvaliarPendenciasAulaDiarioClasseFechamento, Times.Once());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasTurmasComponenteSemAulaUe, Times.Once());
        }

        [Fact(DisplayName = "Deve disparar apenas filas obrigat�rias quando a UE ignora pend�ncias")]
        public async Task Executar_QuandoUeIgnoraPendencias_DeveDispararApenasFilasObrigatorias()
        {
            // Organiza��o
            var ue = _ueFaker.Generate();
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(ue));

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoUeIgnoraGeracaoPendenciasQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(PodeExecutarPendenciaComponenteSemAulaQuery.Instance, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            ConfigurarTodosParametros(true);

            // A��o
            var resultado = await _useCase.Executar(mensagem);

            // Verifica��o
            Assert.True(resultado);
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasAulaDiarioBordo, Times.Never());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasAulaFrequencia, Times.Never());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasAulaAvaliacao, Times.Once());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasAulaPlanoAula, Times.Once());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaAvaliarPendenciasAulaDiarioClasseFechamento, Times.Once());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasTurmasComponenteSemAulaUe, Times.Once());
        }

        [Fact(DisplayName = "Deve disparar apenas filas n�o parametrizadas quando par�metros est�o inativos")]
        public async Task Executar_QuandoParametrosInativos_DeveDispararApenasFilasNaoParametrizadas()
        {
            // Organiza��o
            var ue = _ueFaker.Generate();
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(ue));

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoUeIgnoraGeracaoPendenciasQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _mediatorMock.Setup(m => m.Send(PodeExecutarPendenciaComponenteSemAulaQuery.Instance, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            ConfigurarTodosParametros(false); // Par�metros inativos/nulos

            // A��o
            var resultado = await _useCase.Executar(mensagem);

            // Verifica��o
            Assert.True(resultado);
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasAulaDiarioBordo, Times.Never());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasAulaFrequencia, Times.Never());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasAulaAvaliacao, Times.Never());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasAulaPlanoAula, Times.Never());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasTurmasComponenteSemAulaUe, Times.Never());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaAvaliarPendenciasAulaDiarioClasseFechamento, Times.Once());
        }
    }
}
