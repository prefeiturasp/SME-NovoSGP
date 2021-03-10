using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSituacaoEncaminhamentoAEEPorEstudanteQueryHandler : IRequestHandler<ObterSituacaoEncaminhamentoAEEPorEstudanteQuery, SituacaoEncaminhamentoPorEstudanteDto>
    {
        private readonly IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE;

        public ObterSituacaoEncaminhamentoAEEPorEstudanteQueryHandler(IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE)
        {
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public async Task<SituacaoEncaminhamentoPorEstudanteDto> Handle(ObterSituacaoEncaminhamentoAEEPorEstudanteQuery request, CancellationToken cancellationToken)
        {
            var encaminhamento = await repositorioEncaminhamentoAEE.ObterEncaminhamentoPorEstudante(request.CodigoEstudante);
            
            return MapearParaDto(encaminhamento);
        }

        private SituacaoEncaminhamentoPorEstudanteDto MapearParaDto(EncaminhamentoAEEAlunoTurmaDto encaminhamento)
            => encaminhamento == null ? null :
            new SituacaoEncaminhamentoPorEstudanteDto()
            {
                Id = encaminhamento.Id,
                Situacao = encaminhamento.Situacao != 0 ? encaminhamento.Situacao.Name() : ""
            };
    }
}
