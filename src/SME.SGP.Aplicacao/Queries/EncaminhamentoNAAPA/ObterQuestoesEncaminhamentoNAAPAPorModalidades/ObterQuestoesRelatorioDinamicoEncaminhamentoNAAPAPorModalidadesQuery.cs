using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Questionario;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesQuery : IRequest<IEnumerable<SecaoQuestoesDTO>>
    {
        public ObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesQuery(int[] modalidadesId)
        {
            ModalidadesId = modalidadesId;
        }

        public int[] ModalidadesId { get; }
    }
}
