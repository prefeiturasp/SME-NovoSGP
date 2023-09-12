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
        private readonly IConfiguration configuration;

        public ReceberRelatorioProntoEscolaAquiUseCase(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var relatorioCorrelacao = await mediator.Send(new ObterCorrelacaoRelatorioQuery(mensagemRabbit.CodigoCorrelacao));
            if (relatorioCorrelacao == null)
            {
                throw new NegocioException($"Não foi possível obter a correlação do relatório pronto {mensagemRabbit.CodigoCorrelacao}");
            }

            var mensagem = mensagemRabbit.ObterObjetoMensagem<MensagemRelatorioProntoDto>();
            bool relatorioEscolaAqui = true;
            var urlRedirecionamentoBase = configuration.GetSection("UrlServidorRelatorios").Value;
            switch (relatorioCorrelacao.TipoRelatorio)
            {
                case TipoRelatorio.BoletimDetalhadoApp:
                    await MensagemAutomaticaEscolaAqui(mensagem, urlRedirecionamentoBase, TipoRelatorio.BoletimDetalhadoApp, TipoRelatorio.BoletimDetalhadoApp.ShortName());
                    break;
                case TipoRelatorio.RaaEscolaAqui:
                    await MensagemAutomaticaEscolaAqui(mensagem, urlRedirecionamentoBase, TipoRelatorio.RaaEscolaAqui, TipoRelatorio.RaaEscolaAqui.ShortName());
                    break;
                default:
                    relatorioEscolaAqui = false;
                    break;
            }

            if (relatorioEscolaAqui)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);

        }
        private async Task MensagemAutomaticaEscolaAqui(MensagemRelatorioProntoDto relatorioPronto, string urlRedirecionamentoBase, TipoRelatorio tipoRelatorio, string nomeRelatorio)
        {
            var parametros = JsonConvert.DeserializeObject<MensagemRelatorioAutomaticoEscolaAquiDto>(relatorioPronto.MensagemDados);

            await mediator.Send(new InserirComunicadoMensagemAutomaticaCommand(relatorioPronto.MensagemUsuario, relatorioPronto.MensagemTitulo, parametros.AnoLetivo, parametros.TurmaCodigo, parametros.Modalidade,
                parametros.Semestre, parametros.AlunoCodigo, parametros.CodigoArquivo, urlRedirecionamentoBase, tipoRelatorio, nomeRelatorio));
        }
    }
}
