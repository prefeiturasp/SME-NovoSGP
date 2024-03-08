using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Questionario;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterRelatorioPAPConselhoClasseUseCase
    {
        Task<IEnumerable<SecaoQuestoesDTO>> Executar(string codigoTurma, string codigoAluno, int bimestre);
    }
}
