using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosColetivosNAAPAQueryHandler : ConsultasBase, IRequestHandler<ObterRegistrosColetivosNAAPAQuery, PaginacaoResultadoDto<RegistroColetivoListagemDto>>
    {
        public IMediator mediator { get; }
        public IRepositorioRegistroColetivo repositorioRegistroColetivo { get; }


        public ObterRegistrosColetivosNAAPAQueryHandler(IContextoAplicacao contextoAplicacao, IMediator mediator,
                                                        IRepositorioRegistroColetivo repositorioRegistroColetivo) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioRegistroColetivo = repositorioRegistroColetivo ?? throw new ArgumentNullException(nameof(repositorioRegistroColetivo));
        }

        public Task<PaginacaoResultadoDto<RegistroColetivoListagemDto>> Handle(ObterRegistrosColetivosNAAPAQuery request, CancellationToken cancellationToken)
            => repositorioRegistroColetivo.ListarPaginado(request.DreId, request.UeId,
                                                          request.DataReuniaoInicio, request.DataReuniaoFim, request.TiposReuniaoId, Paginacao);
    }
}
