using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAtendimentoNAAPAHistoricoAlteracoes : IRepositorioAtendimentoNAAPAHistoricoAlteracoes
    {
        protected readonly ISgpContext database;

        public RepositorioAtendimentoNAAPAHistoricoAlteracoes(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<long> SalvarAsync(EncaminhamentoNAAPAHistoricoAlteracoes entidade)
        {
            return (long)(await database.Conexao.InsertAsync(entidade));
        }

        public async Task<PaginacaoResultadoDto<AtendimentoNAAPAHistoricoDeAlteracaoDto>> ListarPaginadoPorEncaminhamentoNAAPAId(long encaminhamentoNAAPAId, Paginacao paginacao)
        {
            var retorno = new PaginacaoResultadoDto<AtendimentoNAAPAHistoricoDeAlteracaoDto>();

            var sql = @$"select ha.Id, u.login as UsuarioLogin, u.nome as UsuarioNome, tipo_historico as TipoHistoricoAlteracoes, 
                                sen.nome as Secao, campos_inseridos as CamposInseridos, campos_alterados as CamposAlterados, 
                                data_atendimento as DataAtendimento, data_historico as DataHistorico, ha.Id as IdHistorico
                        from encaminhamento_naapa_historico_alteracoes ha
                                 inner join usuario u on u.id = ha.usuario_id
                                 left join secao_encaminhamento_naapa sen on sen.id = ha.secao_encaminhamento_naapa_id
                        where ha.encaminhamento_naapa_id = @encaminhamentoNAAPAId
                        order by data_historico desc
                        OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY;
                        select count(ha.Id)
                        from encaminhamento_naapa_historico_alteracoes ha
                                 inner join usuario u on u.id = ha.usuario_id
                                 left join secao_encaminhamento_naapa sen on sen.id = ha.secao_encaminhamento_naapa_id
                        where ha.encaminhamento_naapa_id = @encaminhamentoNAAPAId;";

            using (var historicoAlteracoes = await database.Conexao.QueryMultipleAsync(sql, new { encaminhamentoNAAPAId }))
            {
                retorno.Items = historicoAlteracoes.Read<AtendimentoNAAPAHistoricoDeAlteracaoDto>();
                retorno.TotalRegistros = historicoAlteracoes.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }
    }
}
