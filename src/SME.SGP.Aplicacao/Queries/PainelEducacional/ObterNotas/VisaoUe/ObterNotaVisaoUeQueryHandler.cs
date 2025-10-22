using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoUe
{
    public class ObterNotaVisaoUeQueryHandler : ConsultasBase, IRequestHandler<ObterNotaVisaoUeQuery, PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeRetornoSelectDto>>
    {
        private readonly IRepositorioNota repositorio;

        public ObterNotaVisaoUeQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioNota repositorio) : base(contextoAplicacao)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeRetornoSelectDto>> Handle(ObterNotaVisaoUeQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterNotasVisaoUe(Paginacao, request.CodigoUe, request.AnoLetivo, request.Bimestre, request.Modalidade);
        }
    }
}
