using MediatR;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ReceberRelatorioProntoEscolaAquiUseCaseTeste
    {
        private readonly ReceberRelatorioProntoEscolaAquiUseCase useCase;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly Mock<IConfiguration> configuration;
        private readonly Mock<ISgpContext> sgpContext;
        private readonly MensagemRabbit mensagemRabbiInfantilt;
        private readonly MensagemRabbit mensagemRabbitBoletim;

        public ReceberRelatorioProntoEscolaAquiUseCaseTeste()
        {
            this.mediator = new Mock<IMediator>();
            this.sgpContext = new Mock<ISgpContext>();
            this.unitOfWork = new Mock<IUnitOfWork>();
            this.configuration = new Mock<IConfiguration>();
            this.useCase = new ReceberRelatorioProntoEscolaAquiUseCase(mediator.Object, configuration.Object);
            mensagemRabbiInfantilt = CriarMensagemRabbitRaa();
            mensagemRabbitBoletim = CriarMensagemRabbitBoletim();
        }

        private MensagemRabbit CriarMensagemRabbitRaa()
        {
            var mensagemRelatorioAutomaticoEscolaAquiDto = new MensagemRelatorioAutomaticoEscolaAquiDto
            {
                DreCodigo = "111",
                UeCodigo = "222",
                Semestre = 1,
                TurmaCodigo = "333",
                AnoLetivo = 2022,
                Modalidade = 1,
                ModalidadeCodigo = 1,
                AlunoCodigo = "123",
                CodigoArquivo = Guid.NewGuid()
            };
            var mensagemRelatorioAutomaticoEscolaAquiDtoJson = JsonConvert.SerializeObject(mensagemRelatorioAutomaticoEscolaAquiDto);
            var mensagemRelatorioPronto = new MensagemRelatorioProntoDto
            {
                MensagemUsuario = null,
                MensagemTitulo = null,
                MensagemDados = mensagemRelatorioAutomaticoEscolaAquiDtoJson
            };
            var mensagemRelatorioProntoJson = JsonConvert.SerializeObject(mensagemRelatorioPronto);
            return new MensagemRabbit
            {
                Action = "sgp/relatorio/app/pronto/notificar",
                Mensagem = mensagemRelatorioProntoJson,
                UsuarioLogadoNomeCompleto = "Professor",
                UsuarioLogadoRF = "999999",
            };

        }
        private MensagemRabbit CriarMensagemRabbitBoletim()
        {
            var mensagemRelatorioAutomaticoEscolaAquiDto = new MensagemRelatorioAutomaticoEscolaAquiDto
            {
                DreCodigo = "111",
                UeCodigo = "222",
                Semestre = 0,
                TurmaCodigo = "333",
                AnoLetivo = 2022,
                Modalidade = 5,
                ModalidadeCodigo = 5,
                AlunoCodigo = "123",
                CodigoArquivo = Guid.NewGuid()
            };
            var mensagemRelatorioAutomaticoEscolaAquiDtoJson = JsonConvert.SerializeObject(mensagemRelatorioAutomaticoEscolaAquiDto);
            var mensagemRelatorioPronto = new MensagemRelatorioProntoDto
            {
                MensagemUsuario = null,
                MensagemTitulo = null,
                MensagemDados = mensagemRelatorioAutomaticoEscolaAquiDtoJson
            };
            var mensagemRelatorioProntoJson = JsonConvert.SerializeObject(mensagemRelatorioPronto);
            return new MensagemRabbit
            {
                Action = "sgp/relatorio/app/pronto/notificar",
                Mensagem = mensagemRelatorioProntoJson,
                UsuarioLogadoNomeCompleto = "Professor",
                UsuarioLogadoRF = "999999",
            };
        }
        [Fact]
        public async Task Solicitacao_Boletim_Com_Sucesso()
        {
            this.configuration.Setup(x => x.GetSection("UrlServidorRelatorios").Value);
            var correlacao = new RelatorioCorrelacao
            {
                Id = 2,
                Codigo = Guid.NewGuid(),
                TipoRelatorio = TipoRelatorio.BoletimDetalhadoApp,
                UsuarioSolicitanteId = 1,
                Formato = TipoFormatoRelatorio.Pdf,
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterCorrelacaoRelatorioQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(correlacao);

            mediator.Setup(a => a.Send(It.IsAny<ReceberRelatorioProntoEscolaAquiUseCase>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            mensagemRabbitBoletim.CodigoCorrelacao = correlacao.Codigo;
            var retorno = await useCase.Executar(mensagemRabbitBoletim);
            Assert.True(retorno);
        }

        [Fact]
        public async Task Solicitacao_RAA_Com_Sucesso()
        {

            this.configuration.Setup(x => x.GetSection("UrlServidorRelatorios").Value);
            var correlacao = new RelatorioCorrelacao
            {
                Id = 1,
                Codigo = Guid.NewGuid(),
                TipoRelatorio = TipoRelatorio.RaaEscolaAqui,
                UsuarioSolicitanteId = 1,
                Formato = TipoFormatoRelatorio.Html,
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterCorrelacaoRelatorioQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(correlacao);

            mediator.Setup(a => a.Send(It.IsAny<ReceberRelatorioProntoEscolaAquiUseCase>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            mensagemRabbiInfantilt.CodigoCorrelacao = correlacao.Codigo;
            var retorno = await useCase.Executar(mensagemRabbiInfantilt);
            Assert.True(retorno);

        }

        [Fact]
        public async Task Solicitacao_Notificacao_Sem_Sucesso()
        {
            this.configuration.Setup(x => x.GetSection("UrlServidorRelatorios").Value);
            var correlacao = new RelatorioCorrelacao
            {
                Id = 3,
                Codigo = Guid.NewGuid(),
                TipoRelatorio = TipoRelatorio.Calendario,
                UsuarioSolicitanteId = 1,
                Formato = TipoFormatoRelatorio.Pdf,
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterCorrelacaoRelatorioQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(correlacao);

            mediator.Setup(a => a.Send(It.IsAny<ReceberRelatorioProntoEscolaAquiUseCase>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            mensagemRabbiInfantilt.CodigoCorrelacao = correlacao.Codigo;
            var retorno = await useCase.Executar(mensagemRabbiInfantilt);
            Assert.False(retorno);
        }

        [Fact]
        public async Task Notificacao_Com_Codigo_Correlacao_Invalido()
        {
            var codigo = Guid.NewGuid();
            try
            {
                mediator.Setup(a => a.Send(It.IsAny<ReceberRelatorioProntoEscolaAquiUseCase>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
                mensagemRabbiInfantilt.CodigoCorrelacao = codigo;
                var retorno = await useCase.Executar(mensagemRabbiInfantilt);
            }
            catch (NegocioException ex)
            {
                Assert.True($"Não foi possível obter a correlação do relatório pronto {codigo}" == ex.Message);
            }

        }
    }
}
