using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{

    public interface IObterParecerConclusivoUseCase
    {
        Task<ParecerConclusivoDto> Executar(ConselhoClasseParecerConclusivoConsultaDto parecerConclusivoConsultaDto);
    }
}