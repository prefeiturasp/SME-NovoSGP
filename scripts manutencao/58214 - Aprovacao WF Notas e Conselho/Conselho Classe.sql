do $$
declare
	aprovacoes record;
	aprovacaoNivelId bigint;
	notificacaoId bigint;

begin
	for aprovacoes in 
		select wf_aprovacao_id, conselho_classe_aluno_id, conselho_classe_parecer_id
		  from wf_aprovacao_parecer_conclusivo wapc 
		order by id
	loop 
		update conselho_classe_aluno ca
			set conselho_classe_parecer_id = aprovacoes.conselho_classe_parecer_id
		where ca.id = aprovacoes.conselho_classe_aluno_id;
		
		for aprovacaoNivelId in 
			select id 
			from wf_aprovacao_nivel
			where wf_aprovacao_id = aprovacoes.wf_aprovacao_id
		loop
			for notificacaoId in 
				select notificacao_id 
				from wf_aprovacao_nivel_notificacao
				where wf_aprovacao_nivel_id = aprovacaoNivelId
			loop
				delete from wf_aprovacao_nivel_notificacao where notificacao_id = notificacaoId;
				delete from notificacao where id = notificacaoId;
			end loop;
		
			delete from wf_aprovacao_nivel where id = aprovacaoNivelId;
		end loop;
	
		delete from wf_aprovacao_parecer_conclusivo where wf_aprovacao_id = aprovacoes.wf_aprovacao_id;
		delete from wf_aprovacao wa where wa.id = aprovacoes.wf_aprovacao_id;
	
		commit;
	end loop;

end; $$