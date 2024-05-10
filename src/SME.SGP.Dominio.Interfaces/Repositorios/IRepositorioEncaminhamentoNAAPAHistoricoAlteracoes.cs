﻿using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEncaminhamentoNAAPAHistoricoAlteracoes
    {
        Task<long> SalvarAsync(EncaminhamentoNAAPAHistoricoAlteracoes entidade);
        Task<PaginacaoResultadoDto<EncaminhamentoNAAPAHistoricoDeAlteracaoDto>> ListarPaginadoPorEncaminhamentoNAAPAId(long encaminhamentoNAAPAId, Paginacao paginacao);
    }
}
