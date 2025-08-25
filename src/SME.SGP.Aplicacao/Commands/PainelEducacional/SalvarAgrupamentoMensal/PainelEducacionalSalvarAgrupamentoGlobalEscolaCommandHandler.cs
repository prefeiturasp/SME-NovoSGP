using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarAgrupamentoMensal
{
    public class PainelEducacionalSalvarAgrupamentoGlobalEscolaCommandHandler : IRequestHandler<PainelEducacionalSalvarAgrupamentoGlobalEscolaCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola repositorioFrequencia;

        public PainelEducacionalSalvarAgrupamentoGlobalEscolaCommandHandler(IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia;
        }

        public async Task<bool> Handle(PainelEducacionalSalvarAgrupamentoGlobalEscolaCommand request, CancellationToken cancellationToken)
        {
            foreach (var registroFrequenciaAgrupamentoMensal in request.RegistroFrequencia)
            {
                await repositorioFrequencia.SalvarAsync(registroFrequenciaAgrupamentoMensal);
            }
            return true;
        }
    }
}
