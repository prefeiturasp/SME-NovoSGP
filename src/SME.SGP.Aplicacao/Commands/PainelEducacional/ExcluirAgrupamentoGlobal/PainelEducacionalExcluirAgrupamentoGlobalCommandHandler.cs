using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirAgrupamentoGlobal
{
    public class PainelEducacionalExcluirAgrupamentoGlobalCommandHandler : IRequestHandler<PainelEducacionalExcluirAgrupamentoGlobalCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal repositorioFrequencia;
        public PainelEducacionalExcluirAgrupamentoGlobalCommandHandler(IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia;
        }

        public async Task<bool> Handle(PainelEducacionalExcluirAgrupamentoGlobalCommand request, CancellationToken cancellationToken)
        {
            var registros = await repositorioFrequencia.ListarAsync();

            foreach (var item in registros)
            {
                await repositorioFrequencia.ExcluirFrequenciaGlobal(item);
            }

            return true;
        }
    }
}
