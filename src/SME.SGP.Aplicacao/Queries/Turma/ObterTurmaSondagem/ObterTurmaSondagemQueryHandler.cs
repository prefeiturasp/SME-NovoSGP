using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaSondagemQueryHandler : IRequestHandler<ObterTurmaSondagemQuery, IEnumerable<TurmaRetornoDto>>
    {

        private readonly IRepositorioTurmaConsulta repositorioTurma;
        public ObterTurmaSondagemQueryHandler(IRepositorioTurmaConsulta repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public Task<IEnumerable<TurmaRetornoDto>> Handle(ObterTurmaSondagemQuery request, CancellationToken cancellationToken)
        {
            return repositorioTurma.ObterTurmasSondagem(request.CodigoUe, request.AnoLetivo);
        }
    }
}
