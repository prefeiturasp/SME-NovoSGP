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
    public class ObterTurmasPorAnoEUsuarioIdQueryHandler : IRequestHandler<ObterTurmasPorAnoEUsuarioIdQuery, IEnumerable<TurmaNaoHistoricaDto>>
    {
        private readonly IRepositorioTurma repositorioTurma;
        public ObterTurmasPorAnoEUsuarioIdQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<TurmaNaoHistoricaDto>> Handle(ObterTurmasPorAnoEUsuarioIdQuery request, CancellationToken cancellationToken)
         => await repositorioTurma.ObterTurmasPorUsuarioEAnoLetivo(request.UsuarioId, request.AnoLetivo);
    }
}
