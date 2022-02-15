do $$
declare
	aprovacoes record;
	nivelId bigint;
	notificacoes record;
begin
	for aprovacoes in 
		select wf_aprovacao_id, conselho_classe_aluno_id, conselho_classe_parecer_id
  		from wf_aprovacao_parecer_conclusivo wapc 
  	loop
  		for nivelId in 
  			select id 
  			  from wf_aprovacao_nivel n
  			 where n.wf_aprovacao_id = aprovacoes.wf_aprovacao_id
  		loop
  			for notificacoes in 
  				select id, notificacao_id
  				  from wf_aprovacao_nivel_notificacao wann 
  				 where wf_aprovacao_nivel_id = nivelId
  			loop 
	  			delete from wf_aprovacao_nivel_notificacao
	  			where id = notificacoes.id;
	  			
	  			delete from notificacao
	  			where id = notificacoes.notificacao_id;
  			end loop;
  				
  			delete from wf_aprovacao_nivel
  			where id = nivelId;
  		end loop;
  	
  		update conselho_classe_aluno
  			set conselho_classe_parecer_id = aprovacoes.conselho_classe_parecer_id
  		where id = aprovacoes.conselho_classe_aluno_id;
  	
  		delete from wf_aprovacao_parecer_conclusivo
  		where wf_aprovacao_id = aprovacoes.wf_aprovacao_id;
  	
  		delete from wf_aprovacao
  		where id = aprovacoes.wf_aprovacao_id;
  	
  		commit;
  	end loop;
end $$