using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosAcaoQueryHandler : ConsultasBase, IRequestHandler<ObterRegistrosAcaoQuery, PaginacaoResultadoDto<RegistroAcaoBuscaAtivaListagemDto>>
    {
        public IMediator mediator { get; }
        public IRepositorioRegistroAcaoBuscaAtiva repositorioRegistroAcao { get; }


        public ObterRegistrosAcaoQueryHandler(IContextoAplicacao contextoAplicacao, IMediator mediator, IRepositorioRegistroAcaoBuscaAtiva repositorioRegistroAcao) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioRegistroAcao = repositorioRegistroAcao ?? throw new ArgumentNullException(nameof(repositorioRegistroAcao));
        }

        public async Task<PaginacaoResultadoDto<RegistroAcaoBuscaAtivaListagemDto>> Handle(ObterRegistrosAcaoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioRegistroAcao.ListarPaginado(request.Filtros.AnoLetivo,
                                                                            request.Filtros.DreId,
                                                                            request.Filtros.UeId,
                                                                            request.Filtros.TurmaId,
                                                                            request.Filtros.Modalidade,
                                                                            request.Filtros.Semestre,
                                                                            request.Filtros.NomeAluno,
                                                                            request.Filtros.DataRegistroInicio,
                                                                            request.Filtros.DataRegistroFim,
                                                                            request.Filtros.OrdemProcedimentoRealizado,
                                                                              Paginacao);
        }
    }
}
