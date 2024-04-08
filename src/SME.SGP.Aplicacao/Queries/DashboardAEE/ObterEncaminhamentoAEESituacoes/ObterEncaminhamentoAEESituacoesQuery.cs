using MediatR;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoAEESituacoesQuery : IRequest<DashboardAEEEncaminhamentosDto>
    {
        public long UeId { get; set; }
        public int Ano { get; set; }
        public long DreId { get; set; }

        public ObterEncaminhamentoAEESituacoesQuery(int ano, long dreId, long ueId)
        {
            Ano = ano;
            DreId = dreId;
            UeId = ueId;
        }
    }
}
