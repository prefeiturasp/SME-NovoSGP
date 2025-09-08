using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirVisaoGeral;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirAgrupamentoMensal
{
    public class PainelEducacionalExcluirVisaoGeralCommandHandler : IRequestHandler<PainelEducacionalExcluirVisaoGeralCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalVisaoGeral repositorioPainelEducacionalVisaoGeral;
        public PainelEducacionalExcluirVisaoGeralCommandHandler(IRepositorioPainelEducacionalVisaoGeral repositorioPainelEducacionalVisaoGeral)
        {
            this.repositorioPainelEducacionalVisaoGeral = repositorioPainelEducacionalVisaoGeral;
        }
        public async Task<bool> Handle(PainelEducacionalExcluirVisaoGeralCommand request, CancellationToken cancellationToken)
        {
            await repositorioPainelEducacionalVisaoGeral.ExcluirVisaoGeral();

            return true;
        }
    }
}
