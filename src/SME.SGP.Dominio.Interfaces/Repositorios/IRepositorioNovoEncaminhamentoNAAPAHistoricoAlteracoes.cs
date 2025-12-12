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
    public interface IRepositorioNovoEncaminhamentoNAAPAHistoricoAlteracoes
    {
        Task<long> SalvarAsync(EncaminhamentoEscolarHistoricoAlteracoes entidade);
        Task<PaginacaoResultadoDto<NovoEncaminhamentoNAAPAHistoricoDeAlteracaoDto>> ListarPaginadoPorEncaminhamentoNAAPAId(long encaminhamentoNAAPAId, Paginacao paginacao);
    }
}