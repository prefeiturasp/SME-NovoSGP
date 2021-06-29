using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConsolidacaoAcompanhamentoAprendizagemAluno
    {
        Task<long> Inserir(ConsolidacaoAcompanhamentoAprendizagemAluno consolidacao);

        Task Limpar();
    }
}
