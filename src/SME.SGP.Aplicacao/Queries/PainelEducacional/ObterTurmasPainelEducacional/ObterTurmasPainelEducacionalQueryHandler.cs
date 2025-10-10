using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterTurmasPainelEducacional
{
    public class ObterTurmasPainelEducacionalQueryHandler : IRequestHandler<ObterTurmasPainelEducacionalQuery, IEnumerable<TurmaPainelEducacionalDto>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;

        public ObterTurmasPainelEducacionalQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
        }

        public async Task<IEnumerable<TurmaPainelEducacionalDto>> Handle(ObterTurmasPainelEducacionalQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurmaConsulta.ObterTurmasPainelEducacionalAsync(request.AnoLetivo);
        }
    }
}
