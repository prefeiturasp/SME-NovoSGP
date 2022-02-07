do $$
declare
	aprovacoes record;
	aprovacaoNivelId bigint;
	notificacaoId bigint;

begin
	for aprovacoes in 
		select wf_aprovacao_id, conselho_classe_nota_id, nota, conceito_id
		  from wf_aprovacao_nota_conselho wanc 
		order by id
	loop 
		update conselho_classe_nota cn
			set nota = aprovacoes.nota
			  , conceito_id = aprovacoes.conceito_id
		where cn.id = aprovacoes.conselho_classe_nota_id;
		
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
	
		delete from wf_aprovacao_nota_conselho where wf_aprovacao_id = aprovacoes.wf_aprovacao_id;
		delete from wf_aprovacao wa where wa.id = aprovacoes.wf_aprovacao_id;
	
		commit;
	end loop;

end; $$