using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarAtribuicaoEsporadicaCommand : IRequest<long>
    {
        public SalvarAtribuicaoEsporadicaCommand(AtribuicaoEsporadica atribuicaoEsporadica)
        {
            AtribuicaoEsporadica = atribuicaoEsporadica;
        }

        public AtribuicaoEsporadica AtribuicaoEsporadica { get; set; }
    }
}
