using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui
{
    public interface IObterEventosEscolaAquiPorDreUeTurmaMesUseCase
    {
        Task<IEnumerable<EventoEADto>> Executar(FiltroEventosEscolaAquiDto filtro);
    }

    
}
