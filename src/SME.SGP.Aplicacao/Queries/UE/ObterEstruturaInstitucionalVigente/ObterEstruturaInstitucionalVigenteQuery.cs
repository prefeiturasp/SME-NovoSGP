using MediatR;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ObterEstruturaInstitucionalVigenteQuery : IRequest<EstruturaInstitucionalRetornoEolDTO>
    {
        private static ObterEstruturaInstitucionalVigenteQuery _instance;
        public static ObterEstruturaInstitucionalVigenteQuery Instance => _instance ??= new();
    }
}
