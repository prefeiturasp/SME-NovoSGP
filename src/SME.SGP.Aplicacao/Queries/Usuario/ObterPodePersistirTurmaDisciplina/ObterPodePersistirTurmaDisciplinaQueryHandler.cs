using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterPodePersistirTurmaDisciplinaQueryHandler : IRequestHandler<ObterPodePersistirTurmaDisciplinaQuery,bool>
    {
        private readonly IServicoUsuario servicoUsuario;
        public ObterPodePersistirTurmaDisciplinaQueryHandler(IServicoUsuario servicoUsuario)
        {
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task<bool> Handle(ObterPodePersistirTurmaDisciplinaQuery request, CancellationToken cancellationToken)
        {
            return await servicoUsuario.PodePersistirTurmaDisciplina(request.CodigoRf, request.TurmaId, request.DisciplinaId, request.Data, request.Usuario);
        }
    }
}