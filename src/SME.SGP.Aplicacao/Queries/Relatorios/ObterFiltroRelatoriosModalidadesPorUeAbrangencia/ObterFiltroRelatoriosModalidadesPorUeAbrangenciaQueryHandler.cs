using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQueryHandler :  IRequestHandler<ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQuery, IEnumerable<OpcaoDropdownDto>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IMediator mediator;

        public string CodigoUe { get; }
        public ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQueryHandler(IRepositorioAbrangencia repositorioAbrangencia, IMediator mediator)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<OpcaoDropdownDto>> Handle(ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQuery request, CancellationToken cancellationToken)
        {
            if (request.CodigoUe == "-99")
            {
                return EnumExtensao.ListarDto<Modalidade>().Select(c => new OpcaoDropdownDto(c.Id.ToString(), c.Descricao));
            }

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (usuarioLogado == null)
                throw new NegocioException("Não foi possível localizar o usuario logado.");

            var modalidades = await repositorioAbrangencia.ObterModalidadesPorUeAbrangencia(request.CodigoUe, usuarioLogado.Login, usuarioLogado.PerfilAtual);

            return modalidades?.Select(c => new OpcaoDropdownDto(((int)c).ToString(), c.Name()));
        }
    }
  
}
