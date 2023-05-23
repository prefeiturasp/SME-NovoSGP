using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarConselhoClasseConsolidadoTurmaAlunoCommandHandler : IRequestHandler<SalvarConselhoClasseConsolidadoTurmaAlunoCommand, long>
    {
        private readonly IRepositorioConselhoClasseConsolidado repositorioConsolidacaoConselhoClasse;

        public SalvarConselhoClasseConsolidadoTurmaAlunoCommandHandler(IRepositorioConselhoClasseConsolidado repositorioConsolidacaoConselhoClasse)
        {
            this.repositorioConsolidacaoConselhoClasse = repositorioConsolidacaoConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoConselhoClasse));
        }

        public Task<long> Handle(SalvarConselhoClasseConsolidadoTurmaAlunoCommand request, CancellationToken cancellationToken)
        {
            return repositorioConsolidacaoConselhoClasse.SalvarAsync(request.ConselhoClasseConsolidadoTurmaAluno);
        }
    }
}
