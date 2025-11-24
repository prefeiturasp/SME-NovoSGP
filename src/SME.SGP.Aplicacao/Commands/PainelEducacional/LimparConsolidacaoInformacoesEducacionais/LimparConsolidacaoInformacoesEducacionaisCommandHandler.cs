using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.LimparConsolidacaoInformacoesEducacionais
{
    public class LimparConsolidacaoInformacoesEducacionaisCommandHandler : IRequestHandler<LimparConsolidacaoInformacoesEducacionaisCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoInformacoesEducacionais repositorio;

        public LimparConsolidacaoInformacoesEducacionaisCommandHandler(IRepositorioPainelEducacionalConsolidacaoInformacoesEducacionais repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<bool> Handle(LimparConsolidacaoInformacoesEducacionaisCommand request, CancellationToken cancellationToken)
        {
            await repositorio.LimparConsolidacao();
            return true;
        }
    }
}
