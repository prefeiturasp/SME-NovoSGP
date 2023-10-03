using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesUseCase
    {
        Task<IEnumerable<QuestaoDto>> Executar(int[] modalidadesIds);
    }
}
