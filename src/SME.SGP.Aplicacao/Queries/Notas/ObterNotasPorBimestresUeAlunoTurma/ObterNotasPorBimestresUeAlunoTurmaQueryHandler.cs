using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasPorBimestresUeAlunoTurmaQueryHandler : IRequestHandler<ObterNotasPorBimestresUeAlunoTurmaQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        private readonly IRepositorioConselhoClasseNota repositorioConselhoClasseNota;

        public ObterNotasPorBimestresUeAlunoTurmaQueryHandler(IRepositorioConselhoClasseNota repositorioConselhoClasseNota)
        {
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasseNota));
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> Handle(ObterNotasPorBimestresUeAlunoTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasseNota.ObterNotasBimestresAluno(request.AlunoCodigo, request.UeId, request.TurmaId, request.Bimestres);
        }
    }
}
