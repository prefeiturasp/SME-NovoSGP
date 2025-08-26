using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirAgrupamentoEscola;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirAgrupamentoMensal
{
    public class PainelEducacionalExcluirAgrupamentoGlobalEscolaCommandHandler : IRequestHandler<PainelEducacionalExcluirAgrupamentoGlobalEscolaCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola repositorioFrequencia;

        public PainelEducacionalExcluirAgrupamentoGlobalEscolaCommandHandler(IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia;
        }

        public async Task<bool> Handle(PainelEducacionalExcluirAgrupamentoGlobalEscolaCommand request, CancellationToken cancellationToken)
        {
            await repositorioFrequencia.ExcluirFrequenciaGlobal();

            return true;
        }
    }
}
