using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.Relatorios.GerarRelatorio
{
    public class GerarRelatorioCommandHandler : IRequestHandler<GerarRelatorioComand, bool>
    {
        private readonly IServicoFila servicoFila;

        public GerarRelatorioCommandHandler(IServicoFila servicoFila)
        {
            this.servicoFila = servicoFila ?? throw new ArgumentNullException(nameof(servicoFila));
        }

        public async Task<bool> Handle(GerarRelatorioComand request, CancellationToken cancellationToken)
        {
            await servicoFila.AdicionaFila(new AdicionaFilaDto("relatorios", request.Filtros, request.TipoRelatorio.Name()));
            return true;
        }
    }
}
