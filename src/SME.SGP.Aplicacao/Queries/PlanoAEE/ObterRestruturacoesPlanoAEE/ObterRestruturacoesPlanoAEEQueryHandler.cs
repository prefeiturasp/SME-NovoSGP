using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRestruturacoesPlanoAEEQueryHandler : ConsultasBase, IRequestHandler<ObterRestruturacoesPlanoAEEQuery, IEnumerable<PlanoAEEReestruturacaoDto>>
    {
        public IMediator mediator { get; }
        public IRepositorioPlanoAEEReestruturacao repositorioPlanoAEEReestruturacao { get; }

        public ObterRestruturacoesPlanoAEEQueryHandler(IContextoAplicacao contextoAplicacao, IMediator mediator, IRepositorioPlanoAEEReestruturacao repositorioPlanoAEEReestruturacao) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPlanoAEEReestruturacao = repositorioPlanoAEEReestruturacao ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEReestruturacao));
        }
        public async Task<IEnumerable<PlanoAEEReestruturacaoDto>> Handle(ObterRestruturacoesPlanoAEEQuery request, CancellationToken cancellationToken)
        {
            var planosAEERestruturados = await repositorioPlanoAEEReestruturacao.ObterRestruturacoesPorPlanoAEEId(request.PlanoId);
            return MapearPlanosRestruturados(planosAEERestruturados);
        }

        private static IEnumerable<PlanoAEEReestruturacaoDto> MapearPlanosRestruturados(IEnumerable<PlanoAEEReestruturacaoDto> planosAEERestruturados)
        {
            var planosAEERestruturacoesDTO = new List<PlanoAEEReestruturacaoDto>();
            foreach (var planoRestruturado in planosAEERestruturados)
            {
                var plano = new PlanoAEEReestruturacaoDto
                {
                    Id = planoRestruturado.Id,
                    Data = planoRestruturado.Data,
                    DataVersao = planoRestruturado.DataVersao,
                    Semestre = planoRestruturado.Semestre,
                    VersaoId = planoRestruturado.VersaoId,
                    Versao = $"v{planoRestruturado.Versao} - {planoRestruturado.DataVersao.ToString("dd/MM/yyyy")}",
                    Descricao = planoRestruturado.Descricao,
                    DescricaoSimples = UtilRegex.RemoverTagsHtml(planoRestruturado.Descricao)
                };

                planosAEERestruturacoesDTO.Add(plano);
            }
            return planosAEERestruturacoesDTO;
        }

    }
}
