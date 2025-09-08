using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarAgrupamentoMensal;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarAgrupamentoEscola
{
    public class PainelEducacionalSalvarVisaoGeralCommandCommandHandler : IRequestHandler<PainelEducacionalSalvarVisaoGeralCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalVisaoGeral repositorioPainelEducacionalVisaoGeral;

        public PainelEducacionalSalvarVisaoGeralCommandCommandHandler(IRepositorioPainelEducacionalVisaoGeral repositorioPainelEducacionalVisaoGeral)
        {
            this.repositorioPainelEducacionalVisaoGeral = repositorioPainelEducacionalVisaoGeral;
        }

        public async Task<bool> Handle(PainelEducacionalSalvarVisaoGeralCommand request, CancellationToken cancellationToken)
        {
            foreach (var registroVisaoGeral in request.VisaoGeral)
            {
                await repositorioPainelEducacionalVisaoGeral.SalvarAsync(registroVisaoGeral);
            }
            return true;
        }
    }
}
