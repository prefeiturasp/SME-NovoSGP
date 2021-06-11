using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class CadastrarDevolutivaCPCommand : IRequest<bool>
    {
        public long PlanoAEEId { get; set; }
        public string ParecerCoordenacao { get; set; }


        public CadastrarDevolutivaCPCommand(long planoAEEId, string parecerCoordenacao)
        {
            PlanoAEEId = planoAEEId;
            ParecerCoordenacao = parecerCoordenacao;
        }
    }
}
