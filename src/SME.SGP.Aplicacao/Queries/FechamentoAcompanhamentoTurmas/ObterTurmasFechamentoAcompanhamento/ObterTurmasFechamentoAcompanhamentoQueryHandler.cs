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
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasFechamentoAcompanhamentoQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioTurma repositorioTurma) : base(contextoAplicacao)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>> Handle(ObterTurmasFechamentoAcompanhamentoQuery request, CancellationToken cancellationToken)
        {
            var turmasPaginada = await repositorioTurma.ObterTurmasFechamentoAcompanhamento(Paginacao,
                                                                                            request.DreId,
                                                                                            request.UeId,
                                                                                            request.TurmasId,
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
