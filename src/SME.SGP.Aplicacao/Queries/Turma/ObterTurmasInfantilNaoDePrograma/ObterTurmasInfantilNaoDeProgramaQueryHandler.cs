using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasInfantilNaoDeProgramaQueryHandler : IRequestHandler<ObterTurmasInfantilNaoDeProgramaQuery, IEnumerable<Dominio.Turma>>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasInfantilNaoDeProgramaQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
        }
        public async Task<IEnumerable<Dominio.Turma>> Handle(ObterTurmasInfantilNaoDeProgramaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterTurmasInfantilNaoDeProgramaPorAnoLetivoAsync(request.AnoLetivo, request.CodigoTurma);
        }
    }
}
