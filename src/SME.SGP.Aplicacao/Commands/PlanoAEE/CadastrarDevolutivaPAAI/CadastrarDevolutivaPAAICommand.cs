using MediatR;

namespace SME.SGP.Aplicacao
{
    public class CadastrarDevolutivaPAAICommand : IRequest<bool>
    {
        public long PlanoAEEId { get; set; }
        public string ParecerPAAI { get; set; }


        public CadastrarDevolutivaPAAICommand(long planoAEEId, string parecerPAAI)
        {
            PlanoAEEId = planoAEEId;
            ParecerPAAI = parecerPAAI;
        }
    }
}
