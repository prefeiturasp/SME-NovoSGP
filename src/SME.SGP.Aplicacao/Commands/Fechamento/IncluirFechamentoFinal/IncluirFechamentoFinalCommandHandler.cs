using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IncluirFechamentoFinalCommandHandler : IRequestHandler<IncluirFechamentoFinalCommand, long>
    {
        private readonly IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma;

        public IncluirFechamentoFinalCommandHandler(IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma)
        {
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoTurma));
        }
        public async Task<long> Handle(IncluirFechamentoFinalCommand request, CancellationToken cancellationToken)
        {
            var entidade = new FechamentoTurma
            {
                TurmaId = request.IncluirFechamentoDto.TurmaId,
                PeriodoEscolarId = request.IncluirFechamentoDto.PeriodoId
            };

            return await repositorioFechamentoTurma.SalvarAsync(entidade);
        }
    }
}
