using MediatR;

namespace SME.SGP.Aplicacao
{
    public class CadastrarParecerCPCommand : IRequest<bool>
    {
        public long PlanoAEEId { get; set; }
        public string ParecerCoordenacao { get; set; }


        public CadastrarParecerCPCommand(long planoAEEId, string parecerCoordenacao)
        {
            PlanoAEEId = planoAEEId;
            ParecerCoordenacao = parecerCoordenacao;
        }
    }
}
