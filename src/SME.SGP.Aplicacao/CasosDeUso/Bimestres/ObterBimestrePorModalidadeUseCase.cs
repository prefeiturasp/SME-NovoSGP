using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{

    public class ObterBimestrePorModalidadeUseCase : IObterBimestrePorModalidadeUseCase
    {

        private readonly IMediator mediator;

        public ObterBimestrePorModalidadeUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<List<FiltroBimestreDto>> Executar(bool opcaoTodos, bool opcaoFinal, Modalidade modalidade)
        {
            var retorno =  await mediator.Send(new ObterBimestrePorModalidadeQuery(opcaoTodos, opcaoFinal, modalidade));
            return retorno;
        }

    }

}

