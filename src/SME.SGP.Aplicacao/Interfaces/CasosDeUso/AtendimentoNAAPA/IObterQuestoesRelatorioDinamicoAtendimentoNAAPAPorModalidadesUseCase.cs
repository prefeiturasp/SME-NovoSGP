using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Questionario;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterQuestoesRelatorioDinamicoAtendimentoNAAPAPorModalidadesUseCase
    {
        Task<IEnumerable<SecaoQuestoesDTO>> Executar(int[] modalidadesId);
    }
}
