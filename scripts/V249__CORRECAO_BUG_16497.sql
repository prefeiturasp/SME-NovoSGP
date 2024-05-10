CREATE OR REPLACE FUNCTION public.f_eventos(p_login character varying, p_perfil_id uuid, p_historico boolean, p_tipo_calendario_id bigint, p_considera_pendente_aprovacao boolean DEFAULT false, p_desconsidera_local_dre boolean DEFAULT false, p_dre_id character varying DEFAULT NULL::character varying, p_ue_id character varying DEFAULT NULL::character varying, p_desconsidera_liberacao_excep_reposicao_recesso boolean DEFAULT false, p_data_inicio date DEFAULT NULL::date, p_data_fim date DEFAULT NULL::date)
 RETURNS SETOF v_estrutura_eventos
 LANGUAGE sql
AS $function$
	select e.id,
		   e.nome,
		   e.descricao,
		   e.data_inicio,
		   e.data_fim,
		   e.dre_id,
		   e.letivo,
		   e.feriado_id,
		   e.tipo_calendario_id,
		   e.tipo_evento_id,
		   e.ue_id,
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
				on e.dre_id = ad.codigo 
				-- modalidade 1 (fundamental/medio) do tipo de calendario, considera as modalidades 5 (Fundamental) e 6 (medio)
				-- modalidade 2 (EJA) do tipo de calendario, considera modalidade 3 (EJA)
				and ((tc.modalidade = 1 and ad.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and ad.modalidade_codigo = 3))
				-- para DREs considera local da ocorrencia 2 (DRE) e 5 (Todos)
				and et.local_ocorrencia in (2,3,5)
			left join f_abrangencia_ues(p_login, p_perfil_id, p_historico) au
				on e.ue_id = au.codigo
				and ((tc.modalidade = 1 and au.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and au.modalidade_codigo = 3))
				-- para UEs considera local da ocorrencia 1 (UE) e 4 (SME/UE) e 5 (Todos)
				and et.local_ocorrencia in (1,3,4, 5)
	where not e.excluido 
		and not et.excluido 
		and (extract(year from e.data_inicio) = tc.ano_letivo or extract(year from e.data_fim) = tc.ano_letivo) 
		and e.tipo_calendario_id = p_tipo_calendario_id		
		-- caso considere 1 (aprovado) e 2 (pendente de aprovacao), senao considera so aprovados
		and ((p_considera_pendente_aprovacao = true and e.status in (1,2)) or (p_considera_pendente_aprovacao = false and e.status = 1)) 
		and (p_desconsidera_local_dre = false or (p_desconsidera_local_dre = true and et.local_ocorrencia != 2))
		and ((p_dre_id is null and ((e.dre_id is null and e.ue_id is null) or e.dre_id in (select codigo from f_abrangencia_dres(p_login, p_perfil_id, p_historico)))) or (p_dre_id is not null and ((e.dre_id is null and e.ue_id is null) or e.dre_id = p_dre_id)))
		and ((p_ue_id is null and (e.ue_id is null or e.ue_id in (select codigo from f_abrangencia_ues(p_login, p_perfil_id, p_historico)))) or (p_ue_id is not null and (e.ue_id is null or e.ue_id = p_ue_id)))
		-- caso desconsidere 6 (liberacao excepcional) e 15 (reposicao de recesso)
		and (p_desconsidera_liberacao_excep_reposicao_recesso = true or (p_desconsidera_liberacao_excep_reposicao_recesso = false and et.codigo not in (6, 15)))
		and (p_data_inicio is null or (p_data_inicio is not null and date(e.data_inicio) >= date(p_data_inicio)))
		and (p_data_fim is null or (p_data_fim is not null and date(e.data_fim) <= date(p_data_fim)));
$function$
;
