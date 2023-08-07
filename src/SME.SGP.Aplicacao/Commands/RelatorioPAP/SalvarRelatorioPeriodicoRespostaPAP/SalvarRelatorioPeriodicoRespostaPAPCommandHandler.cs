using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarRelatorioPeriodicoRespostaPAPCommandHandler : AbstractPersistenciaRelatorioPeriodicoRespostaPAPCommand, IRequestHandler<SalvarRelatorioPeriodicoRespostaPAPCommand, long>
    {
        public SalvarRelatorioPeriodicoRespostaPAPCommandHandler(IMediator mediator, IRepositorioRelatorioPeriodicoPAPResposta repositorio) : base(mediator, repositorio)
        {
        }

        public async Task<long> Handle(SalvarRelatorioPeriodicoRespostaPAPCommand request, CancellationToken cancellationToken)
        {
            var resposta = new RelatorioPeriodicoPAPResposta() { RelatorioPeriodicoQuestaoId = request.RelatorioPeriodicoQuestaoId };

            return await ExecutePersistencia(resposta, request.TipoQuestao, request.Resposta);
        }
    }
}
