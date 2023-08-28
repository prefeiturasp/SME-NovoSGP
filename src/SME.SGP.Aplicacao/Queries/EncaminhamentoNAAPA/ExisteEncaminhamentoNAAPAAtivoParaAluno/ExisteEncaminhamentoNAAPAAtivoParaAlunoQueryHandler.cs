using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExisteEncaminhamentoNAAPAAtivoParaAlunoQueryHandler : IRequestHandler<ExisteEncaminhamentoNAAPAAtivoParaAlunoQuery, bool>
    {
        private readonly IRepositorioEncaminhamentoNAAPA repositorio;

        public ExisteEncaminhamentoNAAPAAtivoParaAlunoQueryHandler(IRepositorioEncaminhamentoNAAPA repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public Task<bool> Handle(ExisteEncaminhamentoNAAPAAtivoParaAlunoQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ExisteEncaminhamentoNAAPAAtivoParaAluno(request.CodigoAluno);
        }
    }
}
