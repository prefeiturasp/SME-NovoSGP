using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarAtualQueryHandler : IRequestHandler<ObterPeriodoEscolarAtualQuery, PeriodoEscolar>
    {
        private readonly IRepositorioPeriodoEscolarConsulta _repositorioPeriodoEscolar;
        private readonly IRepositorioTurmaConsulta _repositorioTurmaConsulta;

        public ObterPeriodoEscolarAtualQueryHandler(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar, IRepositorioTurmaConsulta repositorioTurmaConsulta)
        {
            _repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
            _repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new System.ArgumentNullException(nameof(repositorioTurmaConsulta));
        }
        public async Task<PeriodoEscolar> Handle(ObterPeriodoEscolarAtualQuery request, CancellationToken cancellationToken)
        {
            var turma = await _repositorioTurmaConsulta.ObterPorId(request.TurmaId);

            if (turma == null)
                throw new NegocioException("Turma não encontrada");

            return await _repositorioPeriodoEscolar.ObterPeriodoEscolarAtualAsync(turma.ModalidadeTipoCalendario, request.DataReferencia);
        }
    }
}