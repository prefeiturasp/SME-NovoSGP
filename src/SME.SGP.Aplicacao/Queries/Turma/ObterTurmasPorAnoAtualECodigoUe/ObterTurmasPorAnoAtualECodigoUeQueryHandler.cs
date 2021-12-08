using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorAnoAtualECodigoUeQueryHandler : IRequestHandler<ObterTurmasPorAnoAtualECodigoUeQuery, IEnumerable<TurmaNaoHistoricaDto>>
    {
        private readonly IRepositorioTurma repositorioTurma;
        public ObterTurmasPorAnoAtualECodigoUeQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<TurmaNaoHistoricaDto>> Handle(ObterTurmasPorAnoAtualECodigoUeQuery request, CancellationToken cancellationToken)
         => await repositorioTurma.ObterTurmasPorAnoAtualECodigoUe(request.UeCodigo, request.AnoLetivo);
    }
}
