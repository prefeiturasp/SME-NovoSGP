using MediatR;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ObterEstruturaInstitucionalVigenteQuery : IRequest<EstruturaInstitucionalRetornoEolDTO>
    {
        public ObterEstruturaInstitucionalVigenteQuery(string codigoDre)
        {
            CodigoDre = codigoDre;
        }

        public string CodigoDre { get; set; }

    }
}
