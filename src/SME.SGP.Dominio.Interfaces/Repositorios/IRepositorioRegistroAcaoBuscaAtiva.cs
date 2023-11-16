using System;
using System.Collections.Generic;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRegistroAcaoBuscaAtiva : IRepositorioBase<RegistroAcaoBuscaAtiva>
    {
        Task<RegistroAcaoBuscaAtiva> ObterRegistroAcaoPorId(long id);
        Task<RegistroAcaoBuscaAtiva> ObterRegistroAcaoCabecalhoPorId(long id);
        Task<RegistroAcaoBuscaAtiva> ObterRegistroAcaoPorIdESecao(long id, long secaoId);
        Task<RegistroAcaoBuscaAtiva> ObterRegistroAcaoComTurmaPorId(long id);
    }
}
