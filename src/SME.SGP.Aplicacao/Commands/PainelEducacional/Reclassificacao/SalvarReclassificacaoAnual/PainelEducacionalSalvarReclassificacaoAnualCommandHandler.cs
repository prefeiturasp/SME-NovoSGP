using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarReclassificacaoAnual
{
    public class PainelEducacionalSalvarReclassificacaoAnualCommandHandler : IRequestHandler<PainelEducacionalSalvarReclassificacaoAnualCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalReclassificacao repositorioReclassificacao;

        public PainelEducacionalSalvarReclassificacaoAnualCommandHandler(IRepositorioPainelEducacionalReclassificacao repositorioReclassificacao)
        {
            this.repositorioReclassificacao = repositorioReclassificacao;
        }

        public async Task<bool> Handle(PainelEducacionalSalvarReclassificacaoAnualCommand request, CancellationToken cancellationToken)
        {
            await repositorioReclassificacao.BulkInsertAsync(request.ReclassificacaoAnual);
            return true;
        }
    }
}
