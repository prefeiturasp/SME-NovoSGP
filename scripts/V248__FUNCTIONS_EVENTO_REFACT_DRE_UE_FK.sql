drop function f_eventos;
drop function f_eventos_por_rf_criador;
drop function f_eventos_listar_sem_paginacao;
drop function f_eventos_listar;
DROP VIEW public.v_estrutura_eventos_listar;
DROP VIEW public.v_estrutura_eventos;

CREATE OR REPLACE VIEW public.v_estrutura_eventos
AS SELECT e.id AS eventoid,
    e.nome,
    e.descricao AS descricaoevento,
    e.data_inicio,
    e.data_fim,
    e.dre_codigo,
    e.letivo,
    e.feriado_id,
    e.tipo_calendario_id,
    e.tipo_evento_id,
    e.ue_codigo,
    e.criado_em,
    e.criado_por,
    e.alterado_em,
    e.alterado_por,
    e.criado_rf,
    e.alterado_rf,
    e.status,
    et.id AS tipoeventoid,
    et.ativo,
    et.tipo_data,
    et.descricao AS descricaotipoevento,
    et.excluido,
    et.local_ocorrencia
   FROM evento e
     JOIN evento_tipo et ON e.tipo_evento_id = et.id;

    
CREATE OR REPLACE FUNCTION public.f_eventos(p_login character varying, p_perfil_id uuid, p_historico boolean, p_tipo_calendario_id bigint, p_considera_pendente_aprovacao boolean DEFAULT false, p_desconsidera_local_dre boolean DEFAULT false, p_dre_codigo varchar DEFAULT null::varchar, p_ue_codigo varchar DEFAULT null::varchar, p_desconsidera_liberacao_excep_reposicao_recesso boolean DEFAULT false, p_data_inicio date DEFAULT NULL::date, p_data_fim date DEFAULT NULL::date)
 RETURNS SETOF v_estrutura_eventos
 LANGUAGE sql
AS $function$
	select e.id,
		   e.nome,
		   e.descricao,
		   e.data_inicio,
		   e.data_fim,
		   e.dre_codigo,
		   e.letivo,
		   e.feriado_id,
		   e.tipo_calendario_id,
		   e.tipo_evento_id,
		   e.ue_codigo,
		   e.criado_em,
		   e.criado_por,
	       e.alterado_em,
	       e.alterado_por,
	       e.criado_rf,
		   e.alterado_rf,
		   e.status,	
		   et.id,
		   et.ativo,
		   et.tipo_data,
		   et.descricao,
		   et.excluido,
		   et.local_ocorrencia		   
		from evento e
			inner join evento_tipo et
				on e.tipo_evento_id = et.id
			inner join tipo_calendario tc
				on e.tipo_calendario_id = tc.id
			left join f_abrangencia_dres(p_login, p_perfil_id, p_historico) ad
				on e.dre_id = ad.dre_id 
				-- modalidade 1 (fundamental/medio) do tipo de calendario, considera as modalidades 5 (Fundamental) e 6 (medio)
				-- modalidade 2 (EJA) do tipo de calendario, considera modalidade 3 (EJA)
				and ((tc.modalidade = 1 and ad.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and ad.modalidade_codigo = 3))
				-- para DREs considera local da ocorrencia 2 (DRE) e 5 (Todos)
				and et.local_ocorrencia in (2, 5)
			left join f_abrangencia_ues(p_login, p_perfil_id, p_historico) au
				on e.ue_id = au.ue_id
				and ((tc.modalidade = 1 and au.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and au.modalidade_codigo = 3))
				-- para UEs considera local da ocorrencia 1 (UE) e 4 (SME/UE) e 5 (Todos)
				and et.local_ocorrencia in (1, 4, 5)
	where not e.excluido 
		and not et.excluido 
		and (extract(year from e.data_inicio) = tc.ano_letivo or extract(year from e.data_fim) = tc.ano_letivo) 
		and e.tipo_calendario_id = p_tipo_calendario_id		
		-- caso considere 1 (aprovado) e 2 (pendente de aprovacao), senao considera so aprovados
		and ((p_considera_pendente_aprovacao = true and e.status in (1,2)) or (p_considera_pendente_aprovacao = false and e.status = 1)) 
		and (p_desconsidera_local_dre = false or (p_desconsidera_local_dre = true and et.local_ocorrencia != 2))
		and ((p_dre_codigo is null and ((e.dre_codigo is null and e.ue_codigo is null) or e.dre_codigo in (select dre_codigo from f_abrangencia_dres(p_login, p_perfil_id, p_historico)))) or (p_dre_codigo is not null and ((e.dre_codigo is null and e.ue_codigo is null) or e.dre_codigo = p_dre_codigo)))
		and ((p_ue_codigo is null and (e.ue_codigo is null or e.ue_codigo in (select ue_codigo from f_abrangencia_ues(p_login, p_perfil_id, p_historico)))) or (p_ue_codigo is not null and (e.ue_codigo is null or e.ue_codigo = p_ue_codigo)))
		-- caso desconsidere 6 (liberacao excepcional) e 15 (reposicao de recesso)
		and (p_desconsidera_liberacao_excep_reposicao_recesso = true or (p_desconsidera_liberacao_excep_reposicao_recesso = false and et.codigo not in (6, 15)))
		and (p_data_inicio is null or (p_data_inicio is not null and date(e.data_inicio) >= date(p_data_inicio)))
		and (p_data_fim is null or (p_data_fim is not null and date(e.data_fim) <= date(p_data_fim)));
$function$
;

CREATE OR REPLACE FUNCTION public.f_eventos_por_rf_criador(p_login character varying, p_tipo_calendario_id bigint, p_dre_codigo varchar DEFAULT null::varchar, p_ue_codigo varchar DEFAULT null::varchar, p_data_inicio date DEFAULT NULL::date, p_data_fim date DEFAULT NULL::date)
 RETURNS SETOF v_estrutura_eventos
 LANGUAGE sql
AS $function$
	select e.id,
		   e.nome,
		   e.descricao,
		   e.data_inicio,
		   e.data_fim,
		   e.dre_codigo,
		   e.letivo,
		   e.feriado_id,
		   e.tipo_calendario_id,
		   e.tipo_evento_id,
		   e.ue_codigo,
		   e.criado_em,
		   e.criado_por,
	       e.alterado_em,
	       e.alterado_por,
	       e.criado_rf,
		   e.alterado_rf,
		   e.status,	
		   et.id,
		   et.ativo,
		   et.tipo_data,
		   et.descricao,
		   et.excluido,
		   et.local_ocorrencia
		from evento e
			inner join evento_tipo et
				on e.tipo_evento_id = et.id		
			inner join tipo_calendario tc
				on e.tipo_calendario_id = tc.id
	where not et.excluido
		and not e.excluido
		-- considera somente pendente de aprova��o
		and e.status = 2
		and e.criado_rf = p_login		
		and (extract(year from e.data_inicio) = tc.ano_letivo or extract(year from e.data_fim) = tc.ano_letivo)
		and e.tipo_calendario_id = p_tipo_calendario_id
		and (p_dre_codigo is null or (p_dre_codigo is not null and e.dre_codigo = p_dre_codigo))
		and (p_ue_codigo is null or (p_ue_codigo is not null and e.ue_codigo = p_ue_codigo))
		and (p_data_inicio is null or (p_data_inicio is not null and date(e.data_inicio) >= date(p_data_inicio)))
		and (p_data_fim is null or (p_data_fim is not null and date(e.data_fim) <= date(p_data_fim)));
$function$
;

CREATE OR REPLACE VIEW public.v_estrutura_eventos_listar
AS SELECT e.id AS eventoid,
    e.nome,
    e.descricao AS descricaoevento,
    e.data_inicio,
    e.data_fim,
    e.dre_codigo,
    e.letivo,
    e.feriado_id,
    e.tipo_calendario_id,
    e.tipo_evento_id,
    e.ue_codigo,
    e.criado_em,
    e.criado_por,
    e.alterado_em,
    e.alterado_por,
    e.criado_rf,
    e.alterado_rf,
    e.status,
    et.id AS tipoeventoid,
    et.ativo,
    et.tipo_data,
    et.descricao AS descricaotipoevento,
    et.excluido,
    0 AS total_registros
   FROM evento e
     JOIN evento_tipo et ON e.tipo_evento_id = et.id;


CREATE OR REPLACE FUNCTION public.f_eventos_listar_sem_paginacao(p_login character varying, p_perfil_id uuid, p_historico boolean, p_tipo_calendario_id bigint, p_considera_pendente_aprovacao boolean DEFAULT false, p_desconsidera_local_dre boolean DEFAULT false, p_dre_codigo varchar DEFAULT null::varchar, p_ue_codigo varchar DEFAULT null::varchar, p_desconsidera_liberacao_excep_reposicao_recesso boolean DEFAULT false, p_data_inicio date DEFAULT NULL::date, p_data_fim date DEFAULT NULL::date, p_tipo_evento_id bigint DEFAULT NULL::bigint, p_nome_evento character varying DEFAULT NULL::character varying)
 RETURNS SETOF v_estrutura_eventos_listar
 LANGUAGE sql
AS $function$
	select eventoid,
		   nome,
		   descricaoevento,
		   data_inicio,
		   data_fim,
		   dre_codigo,
		   letivo,
		   feriado_id,
		   tipo_calendario_id,
		   tipo_evento_id,
		   ue_codigo,
		   criado_em,
		   criado_por,
	       alterado_em,
	       alterado_por,
	       criado_rf,
		   alterado_rf,
		   status,	
		   tipoeventoid,
		   ativo,
		   tipo_data,
		   descricaotipoevento,
		   excluido,
		   int4(0) total_registros
		from (
			select eventoid,
				   nome,
				   descricaoevento,
				   data_inicio,
				   data_fim,
				   dre_codigo,
				   letivo,
				   feriado_id,
				   tipo_calendario_id,
				   tipo_evento_id,
				   ue_codigo,
				   criado_em,
				   criado_por,
			       alterado_em,
			       alterado_por,
			       criado_rf,
				   alterado_rf,
				   status,	
				   tipoeventoid,
				   ativo,
				   tipo_data,
				   descricaotipoevento,
				   excluido				   				   
				from f_eventos(p_login, p_perfil_id, p_historico, p_tipo_calendario_id, p_considera_pendente_aprovacao, p_desconsidera_local_dre, p_dre_codigo, p_ue_codigo, p_desconsidera_liberacao_excep_reposicao_recesso, p_data_inicio, p_data_fim)
			
			union
			
			select eventoid,
				   nome,
				   descricaoevento,
				   data_inicio,
				   data_fim,
				   dre_codigo,
				   letivo,
				   feriado_id,
				   tipo_calendario_id,
				   tipo_evento_id,
				   ue_codigo,
				   criado_em,
				   criado_por,
			       alterado_em,
			       alterado_por,
			       criado_rf,
				   alterado_rf,
				   status,	
				   tipoeventoid,
				   ativo,
				   tipo_data,
				   descricaotipoevento,
				   excluido
				from f_eventos_por_rf_criador(p_login, p_tipo_calendario_id, p_dre_codigo, p_ue_codigo, p_data_inicio, p_data_fim)) lista
	where (p_tipo_evento_id is null or (p_tipo_evento_id is not null and tipo_evento_id = p_tipo_evento_id)) and
  		  (p_nome_evento is null or (p_nome_evento is not null and upper(nome) like upper('%' || p_nome_evento || '%')));
$function$
;


CREATE OR REPLACE FUNCTION public.f_eventos_listar(p_login character varying, p_perfil_id uuid, p_historico boolean, p_tipo_calendario_id bigint, p_considera_pendente_aprovacao boolean DEFAULT false, p_desconsidera_local_dre boolean DEFAULT false, p_dre_codigo varchar DEFAULT null::varchar, p_ue_codigo varchar DEFAULT null::varchar, p_desconsidera_liberacao_excep_reposicao_recesso boolean DEFAULT false, p_data_inicio date DEFAULT NULL::date, p_data_fim date DEFAULT NULL::date, p_qtde_registros_ignorados integer DEFAULT 0, p_qtde_registros integer DEFAULT 0, p_tipo_evento_id bigint DEFAULT NULL::bigint, p_nome_evento character varying DEFAULT NULL::character varying)
 RETURNS SETOF v_estrutura_eventos_listar
 LANGUAGE plpgsql
AS $function$
	declare 
		total_registros_obtido int4;
	begin
		total_registros_obtido := int4((select count(0) from f_eventos_listar_sem_paginacao(p_login, p_perfil_id, p_historico, p_tipo_calendario_id, p_considera_pendente_aprovacao, p_desconsidera_local_dre, p_dre_codigo, p_ue_codigo, p_desconsidera_liberacao_excep_reposicao_recesso, p_data_inicio, p_data_fim, p_tipo_evento_id, p_nome_evento)));
		
		if (p_qtde_registros_ignorados > 0 and p_qtde_registros > 0) then
			return query select eventoid,
								nome,
								descricaoevento,
								data_inicio,
								data_fim,
								dre_codigo,
								letivo,
								feriado_id,
								tipo_calendario_id,
								tipo_evento_id,
								ue_codigo,
								criado_em,
								criado_por,
							    alterado_em,
							    alterado_por,
							    criado_rf,
								alterado_rf,
								status,	
								tipoeventoid,
								ativo,
								tipo_data,
								descricaotipoevento,
								excluido,
								total_registros_obtido
							from f_eventos_listar_sem_paginacao(p_login, p_perfil_id, p_historico, p_tipo_calendario_id, p_considera_pendente_aprovacao, p_desconsidera_local_dre, p_dre_codigo, p_ue_codigo, p_desconsidera_liberacao_excep_reposicao_recesso, p_data_inicio, p_data_fim, p_tipo_evento_id, p_nome_evento)
							offset p_qtde_registros_ignorados rows fetch next p_qtde_registros rows only;
		else
			return query select eventoid,
								nome,
								descricaoevento,
								data_inicio,
								data_fim,
								dre_codigo,
								letivo,
								feriado_id,
								tipo_calendario_id,
								tipo_evento_id,
								ue_codigo,
								criado_em,
								criado_por,
							    alterado_em,
							    alterado_por,
							    criado_rf,
								alterado_rf,
								status,	
								tipoeventoid,
								ativo,
								tipo_data,
								descricaotipoevento,
								excluido,
								total_registros_obtido
							from f_eventos_listar_sem_paginacao(p_login, p_perfil_id, p_historico, p_tipo_calendario_id, p_considera_pendente_aprovacao, p_desconsidera_local_dre, p_dre_codigo, p_ue_codigo, p_desconsidera_liberacao_excep_reposicao_recesso, p_data_inicio, p_data_fim, p_tipo_evento_id, p_nome_evento);
		end if;
	END;
$function$
;

