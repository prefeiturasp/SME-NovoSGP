using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoCalendarioIdPorTurmaQueryHandler: IRequestHandler<ObterTipoCalendarioIdPorTurmaQuery, long>
    {
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;
        public ObterTipoCalendarioIdPorTurmaQueryHandler(IRepositorioTipoCalendarioConsulta repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public async Task<long> Handle(ObterTipoCalendarioIdPorTurmaQuery request, CancellationToken cancellationToken)
            => await repositorioTipoCalendario.ObterIdPorAnoLetivoEModalidadeAsync(request.Turma.AnoLetivo
                    , request.Turma.ModalidadeTipoCalendario
                    , request.Turma.Semestre);
    }
}
