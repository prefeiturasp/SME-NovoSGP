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
    public class ObterDiariosBordoPorDevolutivaQueryHandler : ConsultasBase, IRequestHandler<ObterDiariosBordoPorDevolutivaQuery, PaginacaoResultadoDto<DiarioBordoDevolutivaDto>>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public ObterDiariosBordoPorDevolutivaQueryHandler(IContextoAplicacao contextoAplicacao, 
                                                          IRepositorioDiarioBordo repositorioDiarioBordo) : base(contextoAplicacao)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public async Task<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>> Handle(ObterDiariosBordoPorDevolutivaQuery request, CancellationToken cancellationToken)
            => await repositorioDiarioBordo.ObterDiariosBordoPorDevolutivaPaginado(request.DevolutivaId, Paginacao);
    }
}
