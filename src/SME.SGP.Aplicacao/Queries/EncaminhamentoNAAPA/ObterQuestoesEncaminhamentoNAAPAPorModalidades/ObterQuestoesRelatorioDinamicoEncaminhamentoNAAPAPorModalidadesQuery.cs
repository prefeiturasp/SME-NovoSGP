using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesQuery : IRequest<IEnumerable<QuestaoDto>>
    {
        public ObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesQuery(int? modalidadeId)
        {
            ModalidadeId = modalidadeId;
        }

        public int? ModalidadeId { get; }
    }
}
