using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarAgrupamentoMensal
{
    public class PainelEducacionalSalvarAgrupamentoMensalCommandHandler : IRequestHandler<PainelEducacionalSalvarAgrupamentoMensalCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal repositorioFrequencia;

        public PainelEducacionalSalvarAgrupamentoMensalCommandHandler(IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia;
        }

        public async Task<bool> Handle(PainelEducacionalSalvarAgrupamentoMensalCommand request, CancellationToken cancellationToken)
        {
            foreach (var registroFrequenciaAgrupamentoMensal in request.RegistroFrequencia)
            {
                await repositorioFrequencia.SalvarAsync(registroFrequenciaAgrupamentoMensal);
            }
            return true;
        }
    }
}
