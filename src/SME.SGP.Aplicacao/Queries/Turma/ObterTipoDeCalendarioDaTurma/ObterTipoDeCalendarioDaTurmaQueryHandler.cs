using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoDeCalendarioDaTurmaQueryHandler : IRequestHandler<ObterTipoDeCalendarioDaTurmaQuery, TipoCalendario>
    {
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;

        public ObterTipoDeCalendarioDaTurmaQueryHandler(IRepositorioTipoCalendario repositorioTipoCalendario, IRepositorioTurma repositorioTurma)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }
        public async Task<TipoCalendario> Handle(ObterTipoDeCalendarioDaTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(request.Turma.AnoLetivo, request.Turma.ModalidadeTipoCalendario, request.Turma.Semestre);
        }
    }
}
