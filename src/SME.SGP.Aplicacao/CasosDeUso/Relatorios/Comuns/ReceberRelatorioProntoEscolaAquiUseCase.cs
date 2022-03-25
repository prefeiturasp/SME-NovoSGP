using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReceberRelatorioProntoEscolaAquiUseCase : IReceberRelatorioProntoEscolaAquiUseCase
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        public ReceberRelatorioProntoEscolaAquiUseCase(IMediator mediator, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var relatorioCorrelacao = await mediator.Send(new ObterCorrelacaoRelatorioQuery(mensagemRabbit.CodigoCorrelacao));
            if (relatorioCorrelacao == null)
            {
                throw new NegocioException($"Não foi possível obter a correlação do relatório pronto {mensagemRabbit.CodigoCorrelacao}");
            }

            unitOfWork.IniciarTransacao();

            var mensagem = mensagemRabbit.ObterObjetoMensagem<MensagemRelatorioProntoDto>();
            var urlRedirecionamentoBase = configuration.GetSection("UrlServidorRelatorios").Value;
            switch (relatorioCorrelacao.TipoRelatorio)
            {
                case TipoRelatorio.BoletimDetalhadoApp:
                    await MensagemAutomaticaBoletim(mensagem, urlRedirecionamentoBase, TipoRelatorio.BoletimDetalhadoApp, "Boletim");
                    break;
                case TipoRelatorio.RaaEscolaAqui:
                    await MensagemAutomaticaBoletim(mensagem, urlRedirecionamentoBase, TipoRelatorio.RaaEscolaAqui, "RAA");
                    break;
                default:
                    break;
            }

            unitOfWork.PersistirTransacao();

            return await Task.FromResult(true);

        }
        private async Task MensagemAutomaticaBoletim(MensagemRelatorioProntoDto relatorioPronto, string urlRedirecionamentoBase, TipoRelatorio tipoRelatorio, string nomeRelatorio)
        {
            var parametros = JsonConvert.DeserializeObject<MensagemRelatorioAutomaticoEscolaAquiDto>(relatorioPronto.MensagemDados.ToString());

            await mediator.Send(new InserirComunicadoMensagemAutomaticaCommand(relatorioPronto.MensagemUsuario, relatorioPronto.MensagemTitulo, parametros.AnoLetivo, parametros.TurmaCodigo, parametros.Modalidade,
                parametros.Semestre, parametros.AlunoCodigo, parametros.CodigoArquivo, urlRedirecionamentoBase, tipoRelatorio, nomeRelatorio));
        }
    }
}