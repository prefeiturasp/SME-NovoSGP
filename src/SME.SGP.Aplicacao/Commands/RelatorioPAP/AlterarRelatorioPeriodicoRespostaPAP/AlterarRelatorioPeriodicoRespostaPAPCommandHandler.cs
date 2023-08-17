using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarRelatorioPeriodicoRespostaPAPCommandHandler : AbstractPersistenciaRelatorioPeriodicoRespostaPAPCommand, IRequestHandler<AlterarRelatorioPeriodicoRespostaPAPCommand, long>
    {
        public AlterarRelatorioPeriodicoRespostaPAPCommandHandler(IMediator mediator, IRepositorioRelatorioPeriodicoPAPResposta repositorio) : base(mediator, repositorio)
        {
        }

        public Task<long> Handle(AlterarRelatorioPeriodicoRespostaPAPCommand request, CancellationToken cancellationToken)
        {
            return ExecutePersistencia(request.Resposta, request.RespostaDto.TipoQuestao, request.RespostaDto.Resposta);
        }
    }
}
