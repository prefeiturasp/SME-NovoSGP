using System;
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
        private readonly IRepositorioCache _repositorioCache;

        public ObterPeriodoEscolarAtualQueryHandler(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar, IRepositorioTurmaConsulta repositorioTurmaConsulta, IRepositorioCache repositorioCache)
        {
            _repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            _repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
            _repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<PeriodoEscolar> Handle(ObterPeriodoEscolarAtualQuery request, CancellationToken cancellationToken)
        {
            var turma = await _repositorioTurmaConsulta.ObterPorId(request.TurmaId);
            if (turma.EhNulo())
                throw new NegocioException("Turma não encontrada");

            var nomeChave = $"periodo-escolar-atual-{turma.ModalidadeTipoCalendario}-{request.DataReferencia.Date}";
            return await _repositorioCache.ObterAsync(nomeChave, () => _repositorioPeriodoEscolar.ObterPeriodoEscolarAtualAsync(turma.ModalidadeTipoCalendario, request.DataReferencia));
        }
    }
}
