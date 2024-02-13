using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;

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
            => repositorioRegistroColetivo.ListarPaginado(request.AnoLetivo, request.DreId, request.UeId, 
                                                          request.DataReuniaoInicio, request.DataReuniaoFim, request.TiposReuniaoId, Paginacao);
    }
}
