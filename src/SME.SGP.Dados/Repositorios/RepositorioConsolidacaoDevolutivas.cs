using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsolidacaoDevolutivas : IRepositorioConsolidacaoDevolutivas
    {
        private readonly ISgpContext database;

        public RepositorioConsolidacaoDevolutivas(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<QuantidadeDevolutivasEstimadasEConfirmadasPorTurmaAnoDto>> ObterDevolutivasEstimadasEConfirmadasAsync(int anoLetivo, Modalidade modalidade, long? dreId, long? ueId)
        {
            var possuiFiltroUe = ueId.HasValue;
            var query = new StringBuilder(DefinirSelectQueryDevolutivasEstimadasEConfirmadas(possuiFiltroUe));

            query.AppendLine(@"
                from 
	                consolidacao_devolutivas cd
                inner join
	                turma t 
	                on cd.turma_id = t.id
                inner join 
	                ue u
	                on t.ue_id = u.id
                where 
                    t.ano_letivo = @anoLetivo
                    and t.modalidade_codigo = @modalidade ");

            if (dreId.HasValue)
                query.AppendLine("and u.dre_id = @dreId ");

            if (ueId.HasValue)
                query.AppendLine("and u.id = @ueId ");

            if(!possuiFiltroUe)
            {
                query.AppendLine(@"
                    group by
                        t.ano");
            }

            var parametros = new
            {
                anoLetivo,
                modalidade,
                dreId,
                ueId
            };

            return await database.QueryAsync<QuantidadeDevolutivasEstimadasEConfirmadasPorTurmaAnoDto>(query.ToString(), parametros);
        }

        private string DefinirSelectQueryDevolutivasEstimadasEConfirmadas(bool possuiFiltroDeUe)
            => possuiFiltroDeUe
                ? @"select
                        t.turma_id,
                        t.nome as TurmaAno,
                        cd.quantidade_estimada_devolutivas as DevolutivasEstimadas,
	                    cd.quantidade_registrada_devolutivas as DevolutivasRegistradas"
                : @"select
                        t.ano as TurmaAno,
                        sum(cd.quantidade_estimada_devolutivas) as DevolutivasEstimadas,
	                    sum(cd.quantidade_registrada_devolutivas) as DevolutivasRegistradas";

        public async Task<long> Inserir(ConsolidacaoDevolutivas consolidacao)
        {
            return (long)(await database.Conexao.InsertAsync(consolidacao));
        }

        public Task LimparConsolidacaoDevolutivasPorAno(int anoLetivo)
        {
            throw new NotImplementedException();
        }
    }
}