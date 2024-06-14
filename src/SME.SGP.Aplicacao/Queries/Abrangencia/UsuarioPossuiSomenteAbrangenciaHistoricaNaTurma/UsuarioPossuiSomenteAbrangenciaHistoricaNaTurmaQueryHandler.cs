using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class UsuarioPossuiSomenteAbrangenciaHistoricaNaTurmaQueryHandler : IRequestHandler<UsuarioPossuiSomenteAbrangenciaHistoricaNaTurmaQuery, bool>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public UsuarioPossuiSomenteAbrangenciaHistoricaNaTurmaQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<bool> Handle(UsuarioPossuiSomenteAbrangenciaHistoricaNaTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAbrangencia
               .UsuarioPossuiSomenteAbrangenciaHistorica(request.TurmaId, request.UsuarioId);
        }        
    }
}
