using SME.SGP.Dados;
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

        public Task<IEnumerable<long>> ObterUesIds()
            => database.Conexao.QueryAsync<long>("select id from ue order by id");

        public Task<IEnumerable<long>> ObterTurmasIds(int[] modalidades)
            => database.Conexao.QueryAsync<long>("select id from turma where modalidade_codigo = @modalidades order by id", new { modalidades });

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

        public Task<IEnumerable<ConselhoClasseNotaDuplicado>> ObterConselhosClasseNotaDuplicados()
            => database.Conexao.QueryAsync<ConselhoClasseNotaDuplicado>(
                @"select ccn.conselho_classe_aluno_id as ConselhoClasseAlunoId
        	        , ccn.componente_curricular_codigo as ComponenteCorricularId
        	        , count(ccn.id) as Quantidade
        	        , min(ccn.criado_em) as PrimeiroRegistro
        	        , max(ccn.criado_em) as UltimoRegistro
        	        , max(ccn.id) as UltimoId
                  from conselho_classe_nota ccn 
                 inner join conselho_classe_aluno cca on cca.id = ccn.conselho_classe_aluno_id 
                 inner join conselho_classe cc on cc.id = cca.conselho_classe_id 
                 inner join fechamento_turma ft on ft.id = cc.fechamento_turma_id 
                 inner join turma t on t.id = ft.turma_id
                where t.ano_letivo = extract(year from NOW())
                group by ccn.conselho_classe_aluno_id
        	        , ccn.componente_curricular_codigo 
                having count(ccn.id) > 1");

        public Task<int> ObterQuantidadeAcessosDia(DateTime data)
            => database.Conexao.QueryFirstOrDefaultAsync<int>("select count(u.id) from usuario u where date(u.ultimo_login) = @data", new { data });

        public Task<IEnumerable<FechamentoTurmaDuplicado>> ObterFechamentosTurmaDuplicados()
            => database.Conexao.QueryAsync<FechamentoTurmaDuplicado>(
                @"select ft.turma_id as TurmaId
        	        , ft.periodo_escolar_id as PeriodoEscolarId
        	        , count(ft.id) as Quantidade
        	        , min(ft.criado_em) as PrimeiroRegistro
        	        , max(ft.criado_em) as UltimoRegistro
        	        , max(ft.id) as UltimoId
                  from turma t
                 inner join fechamento_turma ft on ft.turma_id = t.id
                 where not ft.excluido 
                   and t.ano_letivo = extract(year from NOW())
                group by ft.turma_id, ft.periodo_escolar_id
                having count(ft.id) > 1");

        public Task<IEnumerable<FechamentoTurmaDisciplinaDuplicado>> ObterFechamentosTurmaDisciplinaDuplicados()
            => database.Conexao.QueryAsync<FechamentoTurmaDisciplinaDuplicado>(
                @"select ftd.fechamento_turma_id as FechamentoTurmaId
        	        , ftd.disciplina_id as DisciplinaId
        	        , count(ftd.id) as Quantidade
        	        , min(ftd.criado_em) as PrimeiroRegistro
        	        , max(ftd.criado_em) as UltimoRegistro
        	        , max(ftd.id) as UltimoId
                  from turma t
                 inner join fechamento_turma ft on ft.turma_id = t.id and not ft.excluido
                 inner join fechamento_turma_disciplina ftd on ftd.fechamento_turma_id = ft.id and not ftd.excluido 
                 where t.ano_letivo = extract(year from NOW())
                group by ftd.fechamento_turma_id
        	        , ftd.disciplina_id
                having count(ftd.id) > 1");

        public Task<IEnumerable<FechamentoAlunoDuplicado>> ObterFechamentosAlunoDuplicados(long ueId)
            => database.Conexao.QueryAsync<FechamentoAlunoDuplicado>(
                @"select fa.fechamento_turma_disciplina_id
        	        , fa.aluno_codigo 
        	        , count(fa.id) as quantidade
        	        , min(fa.criado_em) as primeiro_registro
        	        , max(fa.criado_em) as ultimo_registro
        	        , max(fa.id) as ultimo_id
                  from fechamento_aluno fa
                 inner join fechamento_turma_disciplina ftd on ftd.id = fa.fechamento_turma_disciplina_id 
                 inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id 
                 inner join turma t on t.id = ft.turma_id 
     	        where t.ano_letivo = extract(year from NOW())
	              and t.ue_id = @ueId
                group by fa.fechamento_turma_disciplina_id
        	        , fa.aluno_codigo
                having count(fa.id) > 1", new { ueId });

        public Task<IEnumerable<FechamentoNotaDuplicado>> ObterFechamentosNotaDuplicados(long turmaId)
            => database.Conexao.QueryAsync<FechamentoNotaDuplicado>(
                @"select fn.fechamento_aluno_id as FechamentoAlunoId
		            , fn.disciplina_id as DisciplinaId
		            , count(fn.id) as Quantidade
		            , min(fn.criado_em) as PrimeiroRegistro
		            , max(fn.criado_em) as UltimoRegistro
		            , max(fn.id) as UltimoId 
		            from fechamento_nota fn
		            inner join fechamento_aluno fa on fa.id = fn.fechamento_aluno_id 
		            inner join fechamento_turma_disciplina ftd on ftd.id = fa.fechamento_turma_disciplina_id 
		            inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id 
	            WHERE ft.id = p_turmaid
	            group by fn.fechamento_aluno_id
		            , fn.disciplina_id 
	            having count(fn.id) > 1", new { turmaId });

        public Task<IEnumerable<ConsolidacaoConselhoClasseNotaNulos>> ObterConsolidacaoCCNotasNulos()
            => database.Conexao.QueryAsync<ConsolidacaoConselhoClasseNotaNulos>(
                @"select t.turma_id as TurmaId, cccatn.bimestre, cccat.aluno_codigo as AlunoCodigo, cccatn.componente_curricular_id as ComponenteCurricularId
			        from consolidado_conselho_classe_aluno_turma_nota cccatn 
			       inner join consolidado_conselho_classe_aluno_turma cccat on cccat.id = cccatn.consolidado_conselho_classe_aluno_turma_id  
			       inner join turma t on t.id = cccat.turma_id  
			       where cccatn.nota is null and cccatn.conceito_id is null");

        public Task<IEnumerable<ConsolidacaoConselhoClasseAlunoTurmaDuplicado>> ObterConsolidacaoCCAlunoTurmaDuplicados(long ueId)
            => database.Conexao.QueryAsync<ConsolidacaoConselhoClasseAlunoTurmaDuplicado>(
                @"select cccat.aluno_codigo as AlunoCodigo, 
                        cccat.turma_id as TurmaId,
					    count(cccat.id) as Quantidade,
					    min(cccat.criado_em) as PrimeiroRegistro,
					    max(cccat.criado_em) as UltimoRegistro,
					    max(cccat.id) as UltimoId 
				 from consolidado_conselho_classe_aluno_turma cccat
				 join turma t on t.id = cccat.turma_id 
			    where t.ue_id = @ueId
			    and t.ano_letivo = extract(year from NOW())
			    and not cccat.excluido
			    group by cccat.aluno_codigo, cccat.turma_id 
			    having count(cccat.id) > 1");
    }
}
