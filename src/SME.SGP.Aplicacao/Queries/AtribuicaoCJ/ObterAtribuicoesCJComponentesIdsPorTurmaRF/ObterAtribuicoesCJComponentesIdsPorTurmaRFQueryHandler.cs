using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtribuicoesCJComponentesIdsPorTurmaRFQueryHandler : IRequestHandler<ObterAtribuicoesCJComponentesIdsPorTurmaRFQuery, long[]>
    {
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;

        public ObterAtribuicoesCJComponentesIdsPorTurmaRFQueryHandler(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
        }
        public async Task<long[]> Handle(ObterAtribuicoesCJComponentesIdsPorTurmaRFQuery request, CancellationToken cancellationToken)
        {
            var ids = await repositorioAtribuicaoCJ.ObterComponentesIdsPorTurmaRFAsync(request.TurmaCodigo, request.UsuarioRF);
            if (ids == null || !ids.Any())
                return default;
            else return ids.ToArray();
        }
    }
}
