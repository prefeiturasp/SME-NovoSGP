using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaAulaCommandHandler : IRequestHandler<ExcluirPendenciaAulaCommand, bool>
    {
        private readonly IRepositorioPendenciaAula repositorioPendenciaAula;

        public ExcluirPendenciaAulaCommandHandler(IRepositorioPendenciaAula repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new System.ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<bool> Handle(ExcluirPendenciaAulaCommand request, CancellationToken cancellationToken)
        {
            var pendencia = await repositorioPendenciaAula.ObterPendenciaPorAulaIdETipo(request.TipoPendenciaAula, request.AulaId);
            await repositorioPendenciaAula.Excluir(request.TipoPendenciaAula, request.AulaId);
            return true;
        }
    }
}
