using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAcompanhamentoAlunoSemestre : IRepositorioBase<AcompanhamentoAlunoSemestre>
    {
        Task<int> ObterAnoPorId(long acompanhamentoAlunoSemestreId);
        Task<IEnumerable<AjusteRotaImagensAcompanhamentoAlunoDto>> ObterImagensParaAjusteRota();
        Task AtualizarLinkImagem(long id, string linkAnterior, string linkAtual);
    }
}
