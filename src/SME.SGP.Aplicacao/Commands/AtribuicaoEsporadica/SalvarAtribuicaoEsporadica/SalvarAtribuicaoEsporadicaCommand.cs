using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

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
