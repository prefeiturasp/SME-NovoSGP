using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.Fechamento.SalvarFechamentoTurma
{
    public class SalvarFechamentoTurmaCommandHandler : IRequestHandler<SalvarFechamentoTurmaCommand, long>
    {
        private readonly IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma;

        public SalvarFechamentoTurmaCommandHandler(IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma)
        {
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoTurma));
        }
        public async Task<long> Handle(SalvarFechamentoTurmaCommand request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoTurma.SalvarAsync(request.FechamentoTurma);
        }
    }
}
