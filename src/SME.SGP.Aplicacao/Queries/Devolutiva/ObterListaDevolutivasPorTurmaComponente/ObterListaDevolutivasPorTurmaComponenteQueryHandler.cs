using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterListaDevolutivasPorTurmaComponenteQueryHandler : ConsultasBase, IRequestHandler<ObterListaDevolutivasPorTurmaComponenteQuery, PaginacaoResultadoDto<DevolutivaResumoDto>>
    {
        private readonly IRepositorioDevolutiva repositorioDevolutiva;

        public ObterListaDevolutivasPorTurmaComponenteQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioDevolutiva repositorioDevolutiva) : base(contextoAplicacao)
        {
            this.repositorioDevolutiva = repositorioDevolutiva ?? throw new ArgumentNullException(nameof(repositorioDevolutiva));
        }

        public async Task<PaginacaoResultadoDto<DevolutivaResumoDto>> Handle(ObterListaDevolutivasPorTurmaComponenteQuery request, CancellationToken cancellationToken)
            => await repositorioDevolutiva.ListarDevolutivasPorTurmaComponentePaginado(request.TurmaCodigo, request.ComponenteCurricularCodigo, request.DataReferencia, Paginacao);
    }
}
