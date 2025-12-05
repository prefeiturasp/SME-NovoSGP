using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExisteAtendimentoNAAPAAtivoParaAlunoQueryHandler : IRequestHandler<ExisteAtendimentoNAAPAAtivoParaAlunoQuery, bool>
    {
        private readonly IRepositorioAtendimentoNAAPA repositorio;

        public ExisteAtendimentoNAAPAAtivoParaAlunoQueryHandler(IRepositorioAtendimentoNAAPA repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public Task<bool> Handle(ExisteAtendimentoNAAPAAtivoParaAlunoQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ExisteEncaminhamentoNAAPAAtivoParaAluno(request.CodigoAluno);
        }
    }
}
