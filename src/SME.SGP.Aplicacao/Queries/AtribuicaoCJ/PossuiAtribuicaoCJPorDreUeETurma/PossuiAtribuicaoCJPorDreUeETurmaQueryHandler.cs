using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PossuiAtribuicaoCJPorDreUeETurmaQueryHandler : IRequestHandler<PossuiAtribuicaoCJPorDreUeETurmaQuery, bool>
    {
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;

        public PossuiAtribuicaoCJPorDreUeETurmaQueryHandler(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new System.ArgumentNullException(nameof(repositorioAtribuicaoCJ));
        }

        public async Task<bool> Handle(PossuiAtribuicaoCJPorDreUeETurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAtribuicaoCJ.PossuiAtribuicaoPorDreUeETurma(request.TurmaId, request.DreCodigo, request.UeCodigo, request.ProfessorRf);
        }
    }
}
