using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaEmAprovacaoQueryHandler : IRequestHandler<ObterNotaEmAprovacaoQuery, double>
    {
        private readonly IRepositorioNotasConceitosConsulta repositorioNotasConceitos;

        public ObterNotaEmAprovacaoQueryHandler(IRepositorioNotasConceitosConsulta repositorioNotasConceitos)
        {
            this.repositorioNotasConceitos = repositorioNotasConceitos ?? throw new ArgumentNullException(nameof(repositorioNotasConceitos));
        }

        public async Task<double> Handle(ObterNotaEmAprovacaoQuery request, CancellationToken cancellationToken)
            => await repositorioNotasConceitos.ObterNotaEmAprovacao(request.CodigoAluno, request.DisciplinaId, request.TurmaFechamentoId);
    }
}
