using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarConselhoClasseAlunoRecomendacaoCommand : IRequest
    {
        public IEnumerable<long> RecomendacoesAlunoId { get; set; }
        public IEnumerable<long> RecomendacoesFamiliaId { get; set; }
        public long ConselhoClasseAlunoId { get; set; }

        public SalvarConselhoClasseAlunoRecomendacaoCommand(IEnumerable<long> recomendacoesAlunoId, IEnumerable<long> recomendacoesFamiliaId, long conselhoClasseAlunoId)
        {
            RecomendacoesAlunoId = recomendacoesAlunoId;
            RecomendacoesFamiliaId = recomendacoesFamiliaId;
            ConselhoClasseAlunoId = conselhoClasseAlunoId;
        }
    }
}
