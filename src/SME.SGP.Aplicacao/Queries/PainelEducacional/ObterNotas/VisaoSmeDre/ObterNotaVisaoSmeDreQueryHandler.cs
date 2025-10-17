using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoSmeDre
{
    public class ObterNotaVisaoSmeDreQueryHandler : IRequestHandler<ObterNotaVisaoSmeDreQuery, IEnumerable<PainelEducacionalNotasVisaoSmeDreDto>>
    {
        private readonly IRepositorioNota repositorio;

        public ObterNotaVisaoSmeDreQueryHandler(
            IRepositorioNota repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<PainelEducacionalNotasVisaoSmeDreDto>> Handle(ObterNotaVisaoSmeDreQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterNotasVisaoSmeDre(request.CodigoDre,request.AnoLetivo, request.Bimestre, request.AnoTurma);
        }
    }
}
