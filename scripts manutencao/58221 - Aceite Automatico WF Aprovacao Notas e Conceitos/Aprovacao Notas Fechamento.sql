do $$
declare
	aprovacoes record;
	nivelId bigint;
	notificacoes record;
begin
	for aprovacoes in 
		select wf_aprovacao_id, fechamento_nota_id, nota, conceito_id 
  		from wf_aprovacao_nota_fechamento wanf 
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
  	
  		update fechamento_nota
  			set nota = aprovacoes.nota
  			  , conceito_id = aprovacoes.conceito_id
  		where id = aprovacoes.fechamento_nota_id;
  	
  		delete from wf_aprovacao_nota_fechamento
  		where wf_aprovacao_id = aprovacoes.wf_aprovacao_id;
  	
  		delete from wf_aprovacao
  		where id = aprovacoes.wf_aprovacao_id;
  	
  		commit;
  	end loop;
end $$