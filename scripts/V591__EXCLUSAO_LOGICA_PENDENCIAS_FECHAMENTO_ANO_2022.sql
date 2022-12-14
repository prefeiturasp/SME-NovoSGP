update pendencia 
	set excluido = true,
	alterado_em = current_timestamp,
	alterado_por = 'Sistema',
	alterado_rf ='0'
from(
	select  p.id as PendenciaId, 
			p.titulo as descricao, p.descricao as detalhamento, 
			p.descricao_html as descricaohtml, 
			p.situacao, ftd.disciplina_id as DisciplinaId, pe.bimestre, 
			pf.fechamento_turma_disciplina_id as FechamentoId, 
			p.criado_em as CriadoEm, p.criado_por as CriadoPor, 
			p.criado_rf as CriadoRf, p.alterado_em as AlteradoEm, 
			p.alterado_por as AlteradoPor, p.alterado_rf as AlteradoRf,
	        ft.turma_id as turmaId,
	        row_number() over(partition by  p.descricao,p.descricao_html order by p.id desc) as sequencia
	from pendencia_fechamento pf
		 inner join fechamento_turma_disciplina ftd on ftd.id = pf.fechamento_turma_disciplina_id
		 inner join fechamento_turma ft on ftd.fechamento_turma_id = ft.id
		 inner join turma t on t.id = ft.turma_id
		 inner join periodo_escolar pe on pe.id = ft.periodo_escolar_id
		 inner join pendencia p on p.id = pf.pendencia_id
		 where not p.excluido and extract(year from p.criado_em) = 2022
) as p	
where sequencia != 1 and id = p.pendenciaId
