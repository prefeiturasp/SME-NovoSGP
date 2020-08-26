using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{


    public class ReceberDadosDownloadRelatorioUseCase : IReceberDadosDownloadRelatorioUseCase
    {
        private readonly IMediator mediator;
        private readonly ISevicoJasper sevicoJasper;
        private readonly IServicoServidorRelatorios servicoServidorRelatorios;

        public ReceberDadosDownloadRelatorioUseCase(IMediator mediator, ISevicoJasper sevicoJasper, IServicoServidorRelatorios servicoServidorRelatorios)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.sevicoJasper = sevicoJasper ?? throw new ArgumentNullException(nameof(sevicoJasper));
            this.servicoServidorRelatorios = servicoServidorRelatorios ?? throw new ArgumentNullException(nameof(servicoServidorRelatorios));
        }
        public async Task<(byte[], string, string)> Executar(Guid codigoCorrelacao)
        {
            var correlacao = await mediator.Send(new ObterCorrelacaoRelatorioQuery(codigoCorrelacao));

            if (correlacao == null || correlacao.PrazoDownloadExpirado)
            {
                //TODO DISPARAR FILA PARA REMOVER O PDF DO SERVIDOR DE RELATORIOS
                throw new NegocioException("Relatório não encontrado ou prazo para download expirado.");
            }

            if (correlacao.EhRelatorioJasper)
                return (await sevicoJasper.DownloadRelatorio(correlacao.CorrelacaoJasper.ExportId, correlacao.CorrelacaoJasper.RequestId, correlacao.CorrelacaoJasper.JSessionId), "application/pdf", correlacao.TipoRelatorio.ShortName());

            return (await servicoServidorRelatorios.DownloadRelatorio(correlacao.Codigo), "application/pdf", correlacao.TipoRelatorio.ShortName());
        }
    }
}
