using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    class PossuiAtribuicaoCJPorTurmaRFQueryHandler : IRequestHandler<PossuiAtribuicaoCJPorTurmaRFQuery, bool>
    {
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;

        public PossuiAtribuicaoCJPorTurmaRFQueryHandler(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new System.ArgumentNullException(nameof(repositorioAtribuicaoCJ));
        }

        public async Task<bool> Handle(PossuiAtribuicaoCJPorTurmaRFQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAtribuicaoCJ.PossuiAtribuicaoPorTurmaRFAnoLetivo(request.TurmaCodigo, request.RFProfessor, request.DisciplinaId);
        }
    }
}
