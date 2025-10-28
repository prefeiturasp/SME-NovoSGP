using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoUe
{
    public class ObterModalidadesNotasVisaoUeQueryHandler : IRequestHandler<ObterModalidadesNotasVisaoUeQuery, IEnumerable<IdentificacaoInfo>>
    {
        private readonly IRepositorioNotaConsulta repositorio;

        public ObterModalidadesNotasVisaoUeQueryHandler(IRepositorioNotaConsulta repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<IEnumerable<IdentificacaoInfo>> Handle(ObterModalidadesNotasVisaoUeQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterModalidadesNotasVisaoUe(request.AnoLetivo, request.CodigoUe,  request.Bimestre);
        }
    }
}
