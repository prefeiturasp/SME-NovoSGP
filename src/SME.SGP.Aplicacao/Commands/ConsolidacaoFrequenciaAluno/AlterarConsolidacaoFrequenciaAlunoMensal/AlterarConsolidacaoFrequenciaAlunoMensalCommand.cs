using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarConsolidacaoFrequenciaAlunoMensalCommand : IRequest<bool>
    {
        public AlterarConsolidacaoFrequenciaAlunoMensalCommand(long idConsolidacao,
            decimal percentual, int quantidadeAulas, int quantidadeAusencias, int quantidadeCompensacoes)
        {
            ConsolidacaoId = idConsolidacao;
            Percentual = percentual;
            QuantidadeAulas = quantidadeAulas;
            QuantidadeAusencias = quantidadeAusencias;
            QuantidadeCompensacoes = quantidadeCompensacoes;
        }

        public long ConsolidacaoId { get; set; }
        public decimal Percentual { get; set; }
        public int QuantidadeAulas { get; set; }
        public int QuantidadeAusencias { get; set; }
        public int QuantidadeCompensacoes { get; set; }
    }
}
