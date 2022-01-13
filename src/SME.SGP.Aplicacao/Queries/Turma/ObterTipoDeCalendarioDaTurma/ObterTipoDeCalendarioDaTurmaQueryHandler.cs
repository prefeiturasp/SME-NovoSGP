using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoDeCalendarioDaTurmaQueryHandler : IRequestHandler<ObterTipoDeCalendarioDaTurmaQuery, TipoCalendario>
    {
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;

        public ObterTipoDeCalendarioDaTurmaQueryHandler(IRepositorioTipoCalendarioConsulta repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }
        public async Task<TipoCalendario> Handle(ObterTipoDeCalendarioDaTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(request.Turma.AnoLetivo, request.Turma.ModalidadeTipoCalendario, request.Turma.Semestre);
        }
    }
}
