using Dapper;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Entidade;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.Repositorios
{

    public class RepositorioSGPConsulta : IRepositorioSGPConsulta
    {
        private readonly ISgpContext database;

        public RepositorioSGPConsulta(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public Task<IEnumerable<ConselhoClasseAlunoDuplicado>> ObterConselhosClasseAlunoDuplicados(long ueId)
            => database.Conexao.QueryAsync<ConselhoClasseAlunoDuplicado>(
                @"select cca.conselho_classe_id as ConselhoClasseId
        	, cca.aluno_codigo as AlunoCodigo
        	, count(cca.id) as Quantidade
        	, min(cca.criado_em) as PrimeiroRegistro
        	, max(cca.criado_em) as UltimoRegistro
        	, max(cca.id) as UltimoId
          from conselho_classe_aluno cca
         inner join conselho_classe cc on cc.id = cca.conselho_classe_id  
         inner join fechamento_turma ft on ft.id = cc.fechamento_turma_id  
         inner join turma t on t.id = ft.turma_id 
     	where t.ano_letivo = extract(year from NOW())
	      and t.ue_id = @ueId
        group by cca.conselho_classe_id 
        	, cca.aluno_codigo
        having count(cca.id) > 1", new { ueId });

        public Task<IEnumerable<ConselhoClasseDuplicado>> ObterConselhosClasseDuplicados()
            => database.Conexao.QueryAsync<ConselhoClasseDuplicado>(
                @"select cc.fechamento_turma_id as FechamentoTurmaId
        	        , count(cc.id) as Quantidade
        	        , min(cc.criado_em) as PrimeiroRegistro
        	        , max(cc.criado_em) as UltimoRegistro
        	        , max(cc.id) as UltimoId
                  from turma t
                 inner join fechamento_turma ft on ft.turma_id = t.id and not ft.excluido
                 inner join conselho_classe cc on cc.fechamento_turma_id = ft.id and not cc.excluido 
                 where t.ano_letivo = extract(year from NOW())
                group by cc.fechamento_turma_id
                having count(cc.id) > 1");

        public Task<int> ObterQuantidadeAcessosDia(DateTime data)
            => database.Conexao.QueryFirstOrDefaultAsync<int>("select count(u.id) from usuario u where date(u.ultimo_login) = @data", new { data });

        public Task<IEnumerable<long>> ObterUesIds()
            => database.Conexao.QueryAsync<long>("select id from ue order by id");
    }
}
