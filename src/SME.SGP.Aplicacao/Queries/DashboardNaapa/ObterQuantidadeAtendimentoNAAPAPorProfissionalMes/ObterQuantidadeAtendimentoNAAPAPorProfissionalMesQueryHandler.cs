﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAtendimentoNAAPAPorProfissionalMesQueryHandler : IRequestHandler<ObterQuantidadeAtendimentoNAAPAPorProfissionalMesQuery, GraficoEncaminhamentoNAAPADto>
    {
        private IRepositorioConsolidadoAtendimentoNAAPA repositorioConsolidado;
        private IMediator mediator;

        public ObterQuantidadeAtendimentoNAAPAPorProfissionalMesQueryHandler(IRepositorioConsolidadoAtendimentoNAAPA repositorioConsolidado, IMediator mediator)
        {
            this.repositorioConsolidado = repositorioConsolidado ?? throw new ArgumentNullException(nameof(repositorioConsolidado));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<GraficoEncaminhamentoNAAPADto> Handle(ObterQuantidadeAtendimentoNAAPAPorProfissionalMesQuery request, CancellationToken cancellationToken)
        {
            var graficos = await this.repositorioConsolidado.ObterQuantidadeAtendimentoNAAPAPorProfissionalMes(request.AnoLetivo, request.DreId, request.UeId, request.Mes);
            var grafico = new GraficoEncaminhamentoNAAPADto()
            {
                DataUltimaConsolidacao = await mediator.Send(new ObterDataUltimaConsolicacaoDashboardNaapaQuery(TipoParametroSistema.GerarConsolidadoAtendimentoNAAPA))
            };

            foreach (var item in graficos)
            {
                grafico.Graficos.Add(new GraficoBaseDto() { Quantidade = item.Quantidade, Descricao = item.Descricao });
            }

            return grafico;
        }
    }
}
