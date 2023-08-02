using MediatR;
using Org.BouncyCastle.Utilities;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEventosEscolaAquiPorDreUeTurmaMesUseCase : IObterEventosEscolaAquiPorDreUeTurmaMesUseCase
    {
        private readonly IMediator mediator;

        public ObterEventosEscolaAquiPorDreUeTurmaMesUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public Task<IEnumerable<EventoEADto>> Executar(FiltroEventosEscolaAquiDto filtro)
        {
            return mediator.Send(new ObterEventosEscolaAquiPorDreUeTurmaMesQuery(filtro.CodigoDre, filtro.CodigoUe, filtro.CodigoTurma, filtro.ModalidadeCalendario, filtro.MesAno));
        }
    }
}
