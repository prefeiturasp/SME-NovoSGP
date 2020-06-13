using MediatR;
using SME.SGP.Aplicacao.Queries.Relatorios.Comuns.ObterCorrelacaoRelatorio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{


    public class ReceberDadosDownloadRelatorioUseCase : IReceberDadosDownloadRelatorioUseCase
    {
        private readonly IMediator mediator;

        public ReceberDadosDownloadRelatorioUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<DadosDownloadRelatorioDto> Executar(Guid codigoCorrelacao)
        {
            var correlacao = await mediator.Send(new ObterCorrelacaoRelatorioQuery(codigoCorrelacao));

            //TODO PENSAR EM COMO FORNECER O CONTENT TYPE E NOME DO ARQUIVO
            return new DadosDownloadRelatorioDto
            {
                CorrelacaoId = correlacao.Codigo,
                ExportacaoId = correlacao.CorrelacaoJasper.ExportId,
                JSessionId = correlacao.CorrelacaoJasper.JSessionId,
                RequisicaoId = correlacao.CorrelacaoJasper.RequestId,
                NomeArquivo = correlacao.TipoRelatorio.ShortName(),
                ContentType = "application/pdf"
            };
        }
    }
}
