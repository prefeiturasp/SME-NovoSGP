using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterGuidAutenticacaoFrequenciaSGA
    {
        Task<Guid> Executar(SolicitacaoGuidAutenticacaoFrequenciaSGADto filtroSolicitacaoGuidAutenticacao);
    }
}
