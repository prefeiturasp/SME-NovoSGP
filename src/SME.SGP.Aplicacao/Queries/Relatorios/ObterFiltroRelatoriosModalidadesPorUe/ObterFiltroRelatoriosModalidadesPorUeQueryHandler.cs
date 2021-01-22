using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosModalidadesPorUeQueryHandler : IRequestHandler<ObterFiltroRelatoriosModalidadesPorUeQuery, IEnumerable<OpcaoDropdownDto>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IServicoUsuario servicoUsuario;

        public ObterFiltroRelatoriosModalidadesPorUeQueryHandler(IRepositorioAbrangencia repositorioAbrangencia, IServicoUsuario servicoUsuario)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task<IEnumerable<OpcaoDropdownDto>> Handle(ObterFiltroRelatoriosModalidadesPorUeQuery request, CancellationToken cancellationToken)
        {
            if (request.CodigoUe == "-99")
            {
                return EnumExtensao.ListarDto<Modalidade>().Select(c => new OpcaoDropdownDto(c.Id.ToString(), c.Descricao));
            }

            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var listaAbrangencia = await repositorioAbrangencia.ObterModalidades(login, perfil, request.AnoLetivo, request.ConsideraHistorico);

            var modalidades = await repositorioAbrangencia.ObterModalidadesPorUe(request.CodigoUe);

            return modalidades?.Where(m => listaAbrangencia.Contains((int)m))?. Select(c => new OpcaoDropdownDto(((int)c).ToString(), c.Name()));
        }
    }
}
