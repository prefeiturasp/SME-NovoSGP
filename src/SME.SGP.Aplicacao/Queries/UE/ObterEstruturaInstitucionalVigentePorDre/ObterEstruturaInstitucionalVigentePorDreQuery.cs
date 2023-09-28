using MediatR;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ObterEstruturaInstitucionalVigentePorDreQuery : IRequest<EstruturaInstitucionalRetornoEolDTO>
    {
        public ObterEstruturaInstitucionalVigentePorDreQuery(string codigoDre)
        {
            CodigoDre = codigoDre;
        }

        public string CodigoDre { get; set; }

    }
}
