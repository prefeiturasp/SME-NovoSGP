update wf_aprovacao set excluido = true
from  ( 
		select wapc.wf_aprovacao_id,cca.conselho_classe_id, 
			aluno_codigo,cca.conselho_classe_parecer_id as ParecerConclusivoConselho,
			wapc.conselho_classe_parecer_id as ParecerConclusivoWF,wa.excluido, ano_letivo
			from conselho_classe_aluno cca 
			inner join wf_aprovacao_parecer_conclusivo wapc on wapc.conselho_classe_aluno_id = cca.id
			inner join wf_aprovacao wa ON wa.id = wapc.wf_aprovacao_id 
			inner join turma t on t.turma_id = wa.turma_id
		where aluno_codigo in('4152560','4230667','3609151','3609155','4230870','7531721') and ano_letivo = 2020
	 ) p 
where id = p.wf_aprovacao_id 