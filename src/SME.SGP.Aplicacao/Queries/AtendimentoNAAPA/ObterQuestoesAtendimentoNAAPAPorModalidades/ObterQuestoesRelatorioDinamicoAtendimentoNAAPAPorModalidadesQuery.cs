using MediatR;
using SME.SGP.Infra.Dtos.Questionario;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesRelatorioDinamicoAtendimentoNAAPAPorModalidadesQuery : IRequest<IEnumerable<SecaoQuestoesDTO>>
    {
        public ObterQuestoesRelatorioDinamicoAtendimentoNAAPAPorModalidadesQuery(int[] modalidadesId)
        {
            ModalidadesId = modalidadesId;
        }

        public int[] ModalidadesId { get; }
    }
}
