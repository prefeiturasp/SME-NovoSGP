using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasFechamentoAcompanhamentoQueryHandler : ConsultasBase, IRequestHandler<ObterTurmasFechamentoAcompanhamentoQuery, PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;

        public ObterTurmasFechamentoAcompanhamentoQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioTurmaConsulta repositorioTurmaConsulta) : base(contextoAplicacao)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
        }

        public async Task<PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>> Handle(ObterTurmasFechamentoAcompanhamentoQuery request, CancellationToken cancellationToken)
        {
            var turmasPaginada = await repositorioTurmaConsulta.ObterTurmasFechamentoAcompanhamento(Paginacao,
                                                                                            request.DreId,
                                                                                            request.UeId,
                                                                                            request.TurmasCodigo,
                                                                                            request.Modalidade,
                                                                                            request.Semestre,
                                                                                            request.Bimestre,
                                                                                            request.AnoLetivo,
                                                                                            request.SituacaoFechamento,
                                                                                            request.SituacaoConselhoClasse,
                                                                                            request.ListarTodasTurmas);

            return turmasPaginada;
        }
    }
}
