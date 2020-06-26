using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoCalendarioIdPorTurmaQueryHandler: IRequestHandler<ObterTipoCalendarioIdPorTurmaQuery, long>
    {
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        public ObterTipoCalendarioIdPorTurmaQueryHandler(IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public async Task<long> Handle(ObterTipoCalendarioIdPorTurmaQuery request, CancellationToken cancellationToken)
            => await repositorioTipoCalendario.ObterIdPorAnoLetivoEModalidadeAsync(request.Turma.AnoLetivo
                    , request.Turma.ModalidadeTipoCalendario
                    , request.Turma.Semestre);
    }
}
