using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.Reclassificacao.ExcluirReclassificacaoAnual
{
    public class PainelEducacionalExcluirReclassificacaoAnualCommandHandler : IRequestHandler<PainelEducacionalExcluirReclassificacaoAnualCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalReclassificacao repositorioReclassificacao;
        public PainelEducacionalExcluirReclassificacaoAnualCommandHandler(IRepositorioPainelEducacionalReclassificacao repositorioReclassificacao)
        {
            this.repositorioReclassificacao = repositorioReclassificacao;
        }
        public async Task<bool> Handle(PainelEducacionalExcluirReclassificacaoAnualCommand request, CancellationToken cancellationToken)
        {
            await repositorioReclassificacao.ExcluirReclassificacaoAnual(request.AnoLetivo);

            return true;
        }
    }
}
