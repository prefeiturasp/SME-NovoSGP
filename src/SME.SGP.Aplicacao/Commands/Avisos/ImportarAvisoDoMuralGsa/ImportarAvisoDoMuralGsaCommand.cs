using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ImportarAvisoDoMuralGsaCommand : IRequest
    {
        public ImportarAvisoDoMuralGsaCommand(Infra.AvisoMuralGsaDto avisoDto)
        {
            AvisoDto = avisoDto;
        }

        public AvisoMuralGsaDto AvisoDto { get; }
    }
}
