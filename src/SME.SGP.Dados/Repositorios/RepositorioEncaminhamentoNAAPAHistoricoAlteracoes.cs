using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEncaminhamentoNAAPAHistoricoAlteracoes : IRepositorioEncaminhamentoNAAPAHistoricoAlteracoes
    {
        protected readonly ISgpContext database;

        public RepositorioEncaminhamentoNAAPAHistoricoAlteracoes(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<long> SalvarAsync(EncaminhamentoNAAPAHistoricoAlteracoes entidade)
        {
            return (long)(await database.Conexao.InsertAsync(entidade));
        }

        public async Task<PaginacaoResultadoDto<EncaminhamentoNAAPAHistoricoDeAlteracaoDto>> ListarPaginadoPorEncaminhamentoNAAPAId(long encaminhamentoNAAPAId, Paginacao paginacao)
        {
            var retorno = new PaginacaoResultadoDto<EncaminhamentoNAAPAHistoricoDeAlteracaoDto>();
            var sql = @$"select ha.Id, u.login as UsuarioLogin, u.nome as UsuarioNome, tipo_historico as TipoHistoricoAlteracoes, 
                                sen.nome as Secao, campos_inseridos as CamposInseridos, campos_alterados as CamposAlterados, 
                                data_atendimento as DataAtendimento, data_historico as DataHistorico, ha.Id as IdHistorico
                        from encaminhamento_naapa_historico_alteracoes ha
		                         inner join usuario u on u.id = ha.usuario_id
		                         left join secao_encaminhamento_naapa sen on sen.id = ha.secao_encaminhamento_naapa_id
                        where ha.encaminhamento_naapa_id = @encaminhamentoNAAPAId
                        order by data_historico desc";

            if (paginacao == null || (paginacao.QuantidadeRegistros == 0 && paginacao.QuantidadeRegistrosIgnorados == 0))
                paginacao = new Paginacao(1, 10);

            var historicos = await database.Conexao.QueryAsync<EncaminhamentoNAAPAHistoricoDeAlteracaoDto>(sql, new { encaminhamentoNAAPAId });

            retorno.Items = historicos;
            retorno.TotalRegistros = historicos.Count();
            retorno.TotalPaginas = historicos.Count() > 0 ? (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros) : 0;
            
            return retorno;
        }
    }
}
