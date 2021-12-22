using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaPorIdAnoLetivoModalidadeTipoQueryHandler : IRequestHandler<ObterTurmaPorIdAnoLetivoModalidadeTipoQuery, Turma>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;

        public ObterTurmaPorIdAnoLetivoModalidadeTipoQueryHandler(IRepositorioTurmaConsulta repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
        }
        public async Task<Turma> Handle(ObterTurmaPorIdAnoLetivoModalidadeTipoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterTurmaPorAnoLetivoModalidadeTipoAsync(request.UeId, request.AnoLetivo, request.TurmaTipo);
        }
    }
}
