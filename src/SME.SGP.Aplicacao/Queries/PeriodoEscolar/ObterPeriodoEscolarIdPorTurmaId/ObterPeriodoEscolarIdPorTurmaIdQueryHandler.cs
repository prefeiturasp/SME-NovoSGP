using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarIdPorTurmaIdQueryHandler : IRequestHandler<ObterPeriodoEscolarIdPorTurmaIdQuery, long>
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;

        public ObterPeriodoEscolarIdPorTurmaIdQueryHandler(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar, IRepositorioTurma repositorioTurmaConsulta)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioTurmaConsulta = this.repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
        }

        public async Task<long> Handle(ObterPeriodoEscolarIdPorTurmaIdQuery request, CancellationToken cancellationToken)
        {
            var turma = await repositorioTurmaConsulta.ObterPorId(request.TurmaId);
            if (turma == null)
                throw new NegocioException("Turma não encontrada");

            return await repositorioPeriodoEscolar.ObterPeriodoEscolarIdPorTurmaId(request.TurmaId, turma.ModalidadeTipoCalendario, request.DataReferencia);
        }
    }
}
