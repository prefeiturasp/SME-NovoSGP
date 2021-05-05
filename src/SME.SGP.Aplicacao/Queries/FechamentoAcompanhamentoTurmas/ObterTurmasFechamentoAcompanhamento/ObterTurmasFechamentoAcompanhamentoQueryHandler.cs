using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasFechamentoAcompanhamentoQueryHandler : IRequestHandler<ObterTurmasFechamentoAcompanhamentoQuery, PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasFechamentoAcompanhamentoQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>> Handle(ObterTurmasFechamentoAcompanhamentoQuery request, CancellationToken cancellationToken)
        {
            var turmasPaginada = await repositorioTurma.ObterTurmasFechamentoAcompanhamento(request.Paginacao,
                                                                                            request.DreId,
                                                                                            request.UeId,
                                                                                            request.TurmaId,
                                                                                            request.Modalidade,
                                                                                            request.Semestre,
                                                                                            request.Bimestre,
                                                                                            request.AnoLetivo);

            return turmasPaginada;
        }
    }
}
