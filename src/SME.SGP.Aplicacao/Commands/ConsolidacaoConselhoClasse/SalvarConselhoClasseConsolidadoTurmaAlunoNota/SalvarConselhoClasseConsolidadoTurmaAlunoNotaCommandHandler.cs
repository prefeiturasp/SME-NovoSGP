using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarConselhoClasseConsolidadoTurmaAlunoNotaCommandHandler : IRequestHandler<SalvarConselhoClasseConsolidadoTurmaAlunoNotaCommand, long>
    {
        private readonly IRepositorioConselhoClasseConsolidadoNota repositorioConsolidacaoConselhoClasseNota;

        public SalvarConselhoClasseConsolidadoTurmaAlunoNotaCommandHandler(IRepositorioConselhoClasseConsolidadoNota repositorioConsolidacaoConselhoClasseNota)
        {
            this.repositorioConsolidacaoConselhoClasseNota = repositorioConsolidacaoConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoConselhoClasseNota));
        }

        public Task<long> Handle(SalvarConselhoClasseConsolidadoTurmaAlunoNotaCommand request, CancellationToken cancellationToken)
        {
            return repositorioConsolidacaoConselhoClasseNota.SalvarAsync(request.ConselhoClasseConsolidadoTurmaAlunoNota);
        }
    }
}
