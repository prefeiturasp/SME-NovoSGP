using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterGuidAutenticacaoFrequencia
    {
        Task<Guid> Executar(SolicitacaoGuidAutenticacaoFrequenciaDto filtroSolicitacaoGuidAutenticacao);
    }
}
