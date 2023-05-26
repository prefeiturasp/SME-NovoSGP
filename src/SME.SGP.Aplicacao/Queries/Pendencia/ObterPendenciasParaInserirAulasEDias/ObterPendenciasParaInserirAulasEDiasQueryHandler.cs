using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasParaInserirAulasEDiasQueryHandler : ConsultasBase, IRequestHandler<ObterPendenciasParaInserirAulasEDiasQuery, IEnumerable<AulasDiasPendenciaDto>>
    {
        private readonly IRepositorioPendencia repositorioPendencia;
        
        public ObterPendenciasParaInserirAulasEDiasQueryHandler(IContextoAplicacao contextoAplicacao,IRepositorioPendencia repositorioPendencia) : base(contextoAplicacao)
        {
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
        }

        public async Task<IEnumerable<AulasDiasPendenciaDto>> Handle(ObterPendenciasParaInserirAulasEDiasQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPendencia.ObterPendenciasParaCargaDiasAulas(request.AnoLetivo,request.UeId);
        }
    }
}