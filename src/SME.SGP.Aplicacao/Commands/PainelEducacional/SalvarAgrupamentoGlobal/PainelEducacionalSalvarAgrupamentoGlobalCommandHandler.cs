using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarAgrupamentoGlobal
{
    public class PainelEducacionalSalvarAgrupamentoGlobalCommandHandler : IRequestHandler<PainelEducacionalSalvarAgrupamentoGlobalCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal repositorioFrequencia;

        public PainelEducacionalSalvarAgrupamentoGlobalCommandHandler(IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia;
        }

        public async Task<bool> Handle(PainelEducacionalSalvarAgrupamentoGlobalCommand request, CancellationToken cancellationToken)
        {
            foreach (var registroFrequenciaAgrupamentoMensal in request.RegistroFrequencia)
            {
                await repositorioFrequencia.SalvarAsync(registroFrequenciaAgrupamentoMensal);
            }
            return true;
        }
    }
}
