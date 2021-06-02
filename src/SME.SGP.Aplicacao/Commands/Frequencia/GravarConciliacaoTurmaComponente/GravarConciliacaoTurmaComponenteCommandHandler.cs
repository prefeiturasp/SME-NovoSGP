using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GravarConciliacaoTurmaComponenteCommandHandler : IRequestHandler<GravarConciliacaoTurmaComponenteCommand, bool>
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public GravarConciliacaoTurmaComponenteCommandHandler(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }

        public async Task<bool> Handle(GravarConciliacaoTurmaComponenteCommand request, CancellationToken cancellationToken)
        {
            await repositorioFrequencia.SalvarConciliacaoTurma(request.TurmaId, request.DisciplinaId, request.DataReferencia, request.Alunos);
            return true;
        }
    }
}
