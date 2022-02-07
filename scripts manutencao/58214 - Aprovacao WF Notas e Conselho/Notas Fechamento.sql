do $$
declare
	aprovacoes record;
	aprovacaoNivelId bigint;
	notificacaoId bigint;

begin
	for aprovacoes in 
		select wf_aprovacao_id, fechamento_nota_id, nota, conceito_id
		  from wf_aprovacao_nota_fechamento
		order by id
	loop 
		update fechamento_nota fn
			set nota = aprovacoes.nota
			  , conceito_id = aprovacoes.conceito_id
		where fn.id = aprovacoes.fechamento_nota_id;
		
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
	
		delete from wf_aprovacao_nota_fechamento where wf_aprovacao_id = aprovacoes.wf_aprovacao_id;
		delete from wf_aprovacao wa where wa.id = aprovacoes.wf_aprovacao_id;
	
		commit;
	end loop;

end; $$