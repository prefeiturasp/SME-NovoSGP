using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoSmeDre;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoSmeDre
{
    public class ObterNotaVisaoUeQueryQueryHandler : IRequestHandler<ObterNotaVisaoSmeDreQuery, IEnumerable<PainelEducacionalNotasVisaoSmeDreRetornoSelectDto>>
    {
        private readonly IRepositorioNota repositorio;

        public ObterNotaVisaoUeQueryQueryHandler(
            IRepositorioNota repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<PainelEducacionalNotasVisaoSmeDreRetornoSelectDto>> Handle(ObterNotaVisaoSmeDreQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterNotasVisaoSmeDre(request.CodigoDre, request.AnoLetivo, request.Bimestre, request.AnoTurma);
        }
    }
}
