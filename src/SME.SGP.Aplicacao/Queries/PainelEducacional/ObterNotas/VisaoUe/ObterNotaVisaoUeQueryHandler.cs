using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoUe
{
    public class ObterNotaVisaoUeQueryHandler : IRequestHandler<ObterNotaVisaoUeQuery, PaginacaoNotaResultadoDto<PainelEducacionalNotasVisaoUeRetornoSelectDto>>
    {
        private readonly IRepositorioNotaConsulta repositorio;

        public ObterNotaVisaoUeQueryHandler(IRepositorioNotaConsulta repositorio) 
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<PaginacaoNotaResultadoDto<PainelEducacionalNotasVisaoUeRetornoSelectDto>> Handle(ObterNotaVisaoUeQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterNotasVisaoUe(request.Paginacao, request.CodigoUe, request.AnoLetivo, request.Bimestre, request.Modalidade);
        }
    }
}
