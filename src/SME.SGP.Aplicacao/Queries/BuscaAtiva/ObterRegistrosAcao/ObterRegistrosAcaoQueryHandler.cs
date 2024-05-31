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
            return await repositorioRegistroAcao.ListarPaginado(new FiltroTurmaRegistrosAcaoDto() {
                                                                  AnoLetivo = request.Filtros.AnoLetivo,
                                                                  DreId =  request.Filtros.DreId,
                                                                  UeId =  request.Filtros.UeId,
                                                                  TurmaId =  request.Filtros.TurmaId,
                                                                  Modalidade =  request.Filtros.Modalidade,
                                                                  Semestre =  request.Filtros.Semestre },
                                                                new FiltroRespostaRegistrosAcaoDto() {
                                                                  CodigoNomeAluno = request.Filtros.CodigoNomeAluno,
                                                                  DataRegistroInicio = request.Filtros.DataRegistroInicio,
                                                                  DataRegistroFim = request.Filtros.DataRegistroFim,
                                                                  OrdemRespostaQuestaoProcedimentoRealizado = request.Filtros.OrdemProcedimentoRealizado},
                                                                  Paginacao);
        }
    }
}
