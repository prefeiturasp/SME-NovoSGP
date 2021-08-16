using System.Collections.Generic;
using MediatR;
using SME.SGP.Infra.Dtos.DashboardFechamento;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoSituacaoQuery : IRequest<IEnumerable<FechamentoSituacaoQuantidadeDto>>
    {
        public long UeId { get; set; }
        public int Ano { get; set; }
        public long DreId { get; set; }
        public int Modalidade { get; set; }
        public int Semestre { get; set; }
        public int Bimestre { get; set; }

        public ObterFechamentoSituacaoQuery(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre)
        {
            UeId = ueId;
            Ano = ano;
            DreId = dreId;
            Modalidade = modalidade;
            Semestre = semestre;
            Bimestre = bimestre;
        }
    }
}