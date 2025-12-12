using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioRespostaNovoEncaminhamentoNAAPA : IRepositorioBase<RespostaEncaminhamentoEscolar>
    {
        Task<bool> RemoverPorArquivoId(long arquivoId);
        Task<IEnumerable<RespostaEncaminhamentoNAAPA>> ObterPorQuestaoEncaminhamentoId(long requestQuestaoEncaminhamentoNaapaId);
        Task<IEnumerable<long>> ObterArquivosPorQuestaoId(long questaoEncaminhamentoAEEId);
        Task<IEnumerable<string>> ObterNomesComponenteSecaoComAnexosEmPdf(long encaminhamentoId);
    }
}