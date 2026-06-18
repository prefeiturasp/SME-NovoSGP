using SME.SGP.Infra.Dtos.Questionario;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesUseCase
    {
        Task<IEnumerable<SecaoQuestoesDTO>> Executar(int[] modalidadesId);
    }
}
