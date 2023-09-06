using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsaveisPorDreUeNAAPAQueryHandler : IRequestHandler<ObterResponsaveisPorDreUeNAAPAQuery, IEnumerable<FuncionarioUnidadeDto>>
    {
        private readonly IMediator mediator;
        public ObterResponsaveisPorDreUeNAAPAQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<FuncionarioUnidadeDto>> Handle(ObterResponsaveisPorDreUeNAAPAQuery request, CancellationToken cancellationToken)
        {
            var perfisDre = new Guid[] { Perfis.PERFIL_COORDENADOR_NAAPA };
            var tiposAtribuicaoUe = new TipoResponsavelAtribuicao[] { TipoResponsavelAtribuicao.Psicopedagogo,
                                        TipoResponsavelAtribuicao.PsicologoEscolar,
                                        TipoResponsavelAtribuicao.AssistenteSocial };

            var responsaveisDre = (await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(request.CodigoUe, perfisDre))).ToList();
            if (!responsaveisDre.Any())
                responsaveisDre = (await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(request.CodigoDre, perfisDre))).ToList();

            var responsaveisUe = (await mediator.Send(new ObterResponsaveisAtribuidosUePorDreUeTiposQuery(request.CodigoDre, request.CodigoUe, tiposAtribuicaoUe)))
                                .Select(atribuicaoResponsavel => new FuncionarioUnidadeDto()
                                {
                                    Login = atribuicaoResponsavel.SupervisorId,
                                    NomeServidor = atribuicaoResponsavel.SupervisorNome,
                                    Perfil = ((TipoResponsavelAtribuicao)atribuicaoResponsavel.TipoAtribuicao).ToPerfil()
                                }).ToList();


            if (responsaveisDre != null && responsaveisDre.Any())
                responsaveisUe.AddRange(responsaveisDre);
            return responsaveisUe.DistinctBy(resp => resp.Login).ToList();
        }
    }
}
