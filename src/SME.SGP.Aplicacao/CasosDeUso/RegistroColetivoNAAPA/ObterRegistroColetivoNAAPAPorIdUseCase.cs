using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using System.Linq;
using System.Threading;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroColetivoNAAPAPorIdUseCase : IObterRegistroColetivoNAAPAPorIdUseCase
    {
        private readonly IMediator mediator;

        public ObterRegistroColetivoNAAPAPorIdUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<RegistroColetivoCompletoDto> Executar(long id)
        {
            var registroColetivoNAAPA = await mediator.Send(new ObterRegistroColetivoNAAPACompletoPorIdQuery(id));

            if(registroColetivoNAAPA.EhNulo())
                throw new NegocioException(MensagemNegocioRegistroColetivoNAAPA.REGISTRO_COLETIVO_NAO_ENCONTRADO);

            return registroColetivoNAAPA;
        }
    }
}
