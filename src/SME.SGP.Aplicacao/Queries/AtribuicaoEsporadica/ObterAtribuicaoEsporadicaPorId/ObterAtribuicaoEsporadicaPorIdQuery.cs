using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAtribuicaoEsporadicaPorIdQuery : IRequest<AtribuicaoEsporadica>
    {
        public ObterAtribuicaoEsporadicaPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
