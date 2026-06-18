using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{

    public interface IObterParecerConclusivoUseCase
    {
        Task<ParecerConclusivoDto> Executar(ConselhoClasseParecerConclusivoConsultaDto parecerConclusivoConsultaDto);
    }
}