using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioQuestaoNovoEncaminhamentoNAAPA : IRepositorioBase<QuestaoEncaminhamentoEscolar>
    {
        Task<IEnumerable<long>> ObterQuestoesPorSecaoId(long encaminhamentoNAAPASecaoId);
        Task<IEnumerable<RespostaQuestaoNovoEncaminhamentoNAAPADto>> ObterRespostasEncaminhamento(long encaminhamentoId);
        Task<IEnumerable<PrioridadeNovoEncaminhamentoNAAPADto>> ObterPrioridadeEncaminhamento();
        Task<IEnumerable<RespostaQuestaoNovoEncaminhamentoNAAPADto>> ObterRespostasItinerarioEncaminhamento(long encaminhamentoSecaoId);
        Task<QuestaoEncaminhamentoNAAPA> ObterQuestaoEnderecoResidencialPorEncaminhamentoId(long encaminhamentoNAAPAId);
        Task<QuestaoEncaminhamentoNAAPA> ObterQuestaoTurmasProgramaPorEncaminhamentoId(long encaminhamentoNAAPAId);
    }
}