using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasNomeFiltroPorTurmasCodigosQueryHandler : IRequestHandler<ObterTurmasNomeFiltroPorTurmasCodigosQuery, IEnumerable<RetornoConsultaTurmaNomeFiltroDto>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;
        public ObterTurmasNomeFiltroPorTurmasCodigosQueryHandler(IRepositorioTurmaConsulta repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }
        public async Task<IEnumerable<RetornoConsultaTurmaNomeFiltroDto>> Handle(ObterTurmasNomeFiltroPorTurmasCodigosQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterTurmasNomeFiltro(request.TurmasCodigos);
        }
    }
}
