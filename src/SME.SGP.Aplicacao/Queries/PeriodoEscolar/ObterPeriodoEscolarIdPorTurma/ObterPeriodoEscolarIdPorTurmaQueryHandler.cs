using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarIdPorTurmaQueryHandler : IRequestHandler<ObterPeriodoEscolarIdPorTurmaQuery, long>
    {
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTurma repositorioTurma;

        public ObterPeriodoEscolarIdPorTurmaQueryHandler(IRepositorioPeriodoEscolar repositorioPeriodoEscolar, IRepositorioTurma repositorioTurma)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<long> Handle(ObterPeriodoEscolarIdPorTurmaQuery request, CancellationToken cancellationToken)
        {
            var turma = await repositorioTurma.ObterPorCodigo(request.TurmaCodigo);
            if (turma == null)
                throw new NegocioException("Turma não encontrada");

            return await repositorioPeriodoEscolar.ObterPeriodoEscolarIdPorTurma(request.TurmaCodigo, turma.ModalidadeTipoCalendario, request.DataReferencia);
        }
    }
}
