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

            // Configuração do Bogus para gerar um objeto Ue válido usando os tipos de domínio reais
            _ueFaker = new Faker<Ue>("pt_BR")
                .RuleFor(u => u.Id, f => f.Random.Long(1, 10000))
                .RuleFor(u => u.CodigoUe, f => f.Random.AlphaNumeric(6))
                .RuleFor(u => u.DreId, f => f.Random.Long(1, 100))
                .RuleFor(u => u.TipoEscola, f => f.PickRandom<TipoEscola>()); // Usa o enum real
        }

        private void ConfigurarParametroSistema(TipoParametroSistema tipo, bool ativo)
        {
            var parametro = new ParametrosSistema { Ativo = ativo };

            // O UseCase pode receber um nulo se o parâmetro não existir, então simulamos isso também.
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

        [Fact(DisplayName = "Deve retornar false e não processar quando a mensagem for inválida")]
        public async Task Executar_QuandoMensagemInvalida_DeveRetornarFalseENaoProcessar()
        {
            // Organização
            var mensagemInvalida = new MensagemRabbit("null");

            // Ação
            var resultado = await _useCase.Executar(mensagemInvalida);

            // Verificação
            Assert.False(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<bool>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Deve disparar todas as filas quando a UE não ignora pendências e os parâmetros estão ativos")]
        public async Task Executar_QuandoUeNaoIgnoraPendenciasEParametrosAtivos_DeveDispararTodasAsFilas()
        {
            // Organização
            var ue = _ueFaker.Generate();
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(ue));

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoUeIgnoraGeracaoPendenciasQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _mediatorMock.Setup(m => m.Send(PodeExecutarPendenciaComponenteSemAulaQuery.Instance, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            ConfigurarTodosParametros(true);

            // Ação
            var resultado = await _useCase.Executar(mensagem);

            // Verificação
            Assert.True(resultado);
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasAulaDiarioBordo, Times.Once());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasAulaFrequencia, Times.Once());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasAulaAvaliacao, Times.Once());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasAulaPlanoAula, Times.Once());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaAvaliarPendenciasAulaDiarioClasseFechamento, Times.Once());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasTurmasComponenteSemAulaUe, Times.Once());
        }

        [Fact(DisplayName = "Deve disparar apenas filas obrigatórias quando a UE ignora pendências")]
        public async Task Executar_QuandoUeIgnoraPendencias_DeveDispararApenasFilasObrigatorias()
        {
            // Organização
            var ue = _ueFaker.Generate();
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(ue));

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoUeIgnoraGeracaoPendenciasQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(PodeExecutarPendenciaComponenteSemAulaQuery.Instance, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            ConfigurarTodosParametros(true);

            // Ação
            var resultado = await _useCase.Executar(mensagem);

            // Verificação
            Assert.True(resultado);
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasAulaDiarioBordo, Times.Never());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasAulaFrequencia, Times.Never());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasAulaAvaliacao, Times.Once());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasAulaPlanoAula, Times.Once());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaAvaliarPendenciasAulaDiarioClasseFechamento, Times.Once());
            VerificarPublicacaoFila(RotasRabbitSgpAula.RotaExecutaPendenciasTurmasComponenteSemAulaUe, Times.Once());
        }

        [Fact(DisplayName = "Deve disparar apenas filas não parametrizadas quando parâmetros estão inativos")]
        public async Task Executar_QuandoParametrosInativos_DeveDispararApenasFilasNaoParametrizadas()
        {
            // Organização
            var ue = _ueFaker.Generate();
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(ue));

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoUeIgnoraGeracaoPendenciasQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _mediatorMock.Setup(m => m.Send(PodeExecutarPendenciaComponenteSemAulaQuery.Instance, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            ConfigurarTodosParametros(false); // Parâmetros inativos/nulos

            // Ação
            var resultado = await _useCase.Executar(mensagem);

            // Verificação
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
