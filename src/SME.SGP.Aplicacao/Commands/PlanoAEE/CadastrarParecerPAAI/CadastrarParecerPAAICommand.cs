using MediatR;

namespace SME.SGP.Aplicacao
{
    public class CadastrarParecerPAAICommand : IRequest<bool>
    {
        public long PlanoAEEId { get; set; }
        public string ParecerPAAI { get; set; }


        public CadastrarParecerPAAICommand(long planoAEEId, string parecerPAAI)
        {
            PlanoAEEId = planoAEEId;
            ParecerPAAI = parecerPAAI;
        }
    }
}
