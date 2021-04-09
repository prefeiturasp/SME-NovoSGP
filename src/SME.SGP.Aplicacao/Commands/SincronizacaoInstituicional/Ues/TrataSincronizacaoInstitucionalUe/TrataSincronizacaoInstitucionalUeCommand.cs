using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class TrataSincronizacaoInstitucionalUeCommand : IRequest<bool>
    {

        public TrataSincronizacaoInstitucionalUeCommand(UeDetalhesParaSincronizacaoInstituicionalDto ueEOL, Ue ueSGP)
        {
            UeEOL = ueEOL;
            UeSGP = ueSGP;
        }

        public UeDetalhesParaSincronizacaoInstituicionalDto UeEOL { get; set; }
        public Ue UeSGP { get; set; }
    }
}
