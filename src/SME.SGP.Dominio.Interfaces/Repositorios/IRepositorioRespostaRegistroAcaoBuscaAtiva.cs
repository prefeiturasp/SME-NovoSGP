using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRespostaRegistroAcaoBuscaAtiva : IRepositorioBase<RespostaRegistroAcaoBuscaAtiva>
    {
        Task<bool> RemoverPorArquivoId(long arquivoId);
        Task<IEnumerable<RespostaRegistroAcaoBuscaAtiva>> ObterPorQuestaoRegistroAcaoId(long questaoRegistroAcaoId);
        Task<IEnumerable<long>> ObterArquivosPorQuestaoId(long questaoRegistroAcaoId);
    }
}
