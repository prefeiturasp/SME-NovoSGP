using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulaPorAulaIdsQueryHandler : IRequestHandler<ObterPendenciasAulaPorAulaIdsQuery, bool>
    {
        private readonly IRepositorioPendenciaAulaConsulta repositorioPendenciaAula;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IServicoEol servicoEol;
        public ObterPendenciasAulaPorAulaIdsQueryHandler(IRepositorioPendenciaAulaConsulta repositorioPendenciaAula, IServicoUsuario servicoUsuario, IServicoEol servicoEol)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }
        public async Task<bool> Handle(ObterPendenciasAulaPorAulaIdsQuery request, CancellationToken cancellationToken)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            var componentesCurriculares = await servicoEol.ObterComponentesCurricularesPorLoginEIdPerfil(usuarioLogado.Login, usuarioLogado.PerfilAtual);
            var componentesCurricularesId = componentesCurriculares.Select(x => x.Codigo).ToArray();

            var possuiPendencia = await repositorioPendenciaAula.PossuiPendenciasPorAulasId(request.AulasId, request.EhModalidadeInfantil, componentesCurricularesId);
            if (!possuiPendencia)
                possuiPendencia = await repositorioPendenciaAula.PossuiAtividadeAvaliativaSemNotaPorAulasId(request.AulasId);
            return possuiPendencia;
        }
    }
}
