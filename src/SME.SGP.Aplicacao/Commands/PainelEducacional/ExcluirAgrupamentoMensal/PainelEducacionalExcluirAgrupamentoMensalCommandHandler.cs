using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirAgrupamentoMensal
{
    public class PainelEducacionalExcluirAgrupamentoMensalCommandHandler : IRequestHandler<PainelEducacionalExcluirAgrupamentoMensalCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal repositorioFrequencia;
        public PainelEducacionalExcluirAgrupamentoMensalCommandHandler(IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia;
        }
        public async Task<bool> Handle(PainelEducacionalExcluirAgrupamentoMensalCommand request, CancellationToken cancellationToken)
        {
            await repositorioFrequencia.ExcluirFrequenciaMensal(request.AnoLetivo);

            return true;
        }
    }
}
