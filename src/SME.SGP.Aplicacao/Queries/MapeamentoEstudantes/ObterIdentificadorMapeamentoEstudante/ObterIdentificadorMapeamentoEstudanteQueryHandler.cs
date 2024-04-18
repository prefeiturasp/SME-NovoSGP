using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdentificadorMapeamentoEstudanteQueryHandler : IRequestHandler<ObterIdentificadorMapeamentoEstudanteQuery, long>
    {
        private readonly IRepositorioMapeamentoEstudante repositorio;

        public ObterIdentificadorMapeamentoEstudanteQueryHandler(IRepositorioMapeamentoEstudante repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public Task<long> Handle(ObterIdentificadorMapeamentoEstudanteQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterIdentificador(request.CodigoAluno, request.TurmaId, request.Bimestre);
        }
    }
}
