CREATE OR REPLACE FUNCTION public.f_eventos_calendario_por_data_inicio_fim(p_login character varying, p_perfil_id uuid, p_historico boolean, p_mes integer, p_tipo_calendario_id bigint, p_considera_pendente_aprovacao boolean DEFAULT false, p_dre_id character varying DEFAULT NULL::character varying, p_ue_id character varying DEFAULT NULL::character varying, p_desconsidera_local_dre boolean DEFAULT false, p_desconsidera_evento_sme boolean DEFAULT false)
 RETURNS SETOF v_estrutura_eventos_calendario
 LANGUAGE sql
AS $function$
	select distinct e.id,
					e.data_inicio,
					case
						when data_inicio = data_fim then ''
						else '(inicio)'
					end descricao_inicio_fim,
					e.nome,
					case
						when e.dre_id is not null and e.ue_id is null then 'DRE'
				      	when e.dre_id is not null and e.ue_id is not null then 'UE'
						else 'SME'
					end tipoEvento
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
				and et.local_ocorrencia in (2, 5)
			left join f_abrangencia_ues(p_login, p_perfil_id, p_historico) au
				on e.ue_id = au.codigo
				and ((tc.modalidade = 1 and au.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and au.modalidade_codigo = 3))
				-- para UEs considera local da ocorrencia 1 (UE) e 4 (SME/UE) e 5 (Todos)
				and et.local_ocorrencia in (1, 4, 5)
			left join perfil_evento_tipo pet on pet.codigo_perfil = p_perfil_id and not pet.excluido
	where et.ativo 
		and not et.excluido
		and not e.excluido
		and extract(month from e.data_inicio) = p_mes
		and extract(year from e.data_inicio) = tc.ano_letivo	
		and e.tipo_calendario_id = p_tipo_calendario_id
		-- caso considere 1 (aprovado) e 2 (pendente de aprovacao), senao considera so aprovados
		and ((p_considera_pendente_aprovacao = true and e.status in (1,2)) or (p_considera_pendente_aprovacao = false and e.status = 1)) 
		and ((p_dre_id is null and ((e.dre_id is null and e.ue_id is null) or e.dre_id in (select codigo from f_abrangencia_dres(p_login, p_perfil_id, p_historico)))) or (p_dre_id is not null and ((e.dre_id is null and e.ue_id is null) or e.dre_id = p_dre_id)))
		and ((p_ue_id is null and (e.ue_id is null or e.ue_id in (select codigo from f_abrangencia_ues(p_login, p_perfil_id, p_historico)))) or (p_ue_id is not null and (e.ue_id is null or e.ue_id = p_ue_id)))
		-- caso desconsidere o local do evento 2 (DRE)
		and (p_desconsidera_local_dre = false or (p_desconsidera_local_dre = true and et.local_ocorrencia != 2))
		-- caso desconsidere evento SME
		and (p_desconsidera_evento_sme = false or (p_desconsidera_evento_sme = true and not (e.dre_id is null and e.ue_id is null)))
		and (pet.id is null or pet.evento_tipo_id = et.id)
		
	union
	
	select distinct e.id,
					e.data_fim,
					'(fim)',
					e.nome,
					case
						when e.dre_id is not null and e.ue_id is null then 'DRE'
				      	when e.dre_id is not null and e.ue_id is not null then 'UE'
						else 'SME'
					end tipoEvento
		from evento e
			inner join evento_tipo et
				on e.tipo_evento_id = et.id
			inner join tipo_calendario tc
				on e.tipo_calendario_id = tc.id
			left join f_abrangencia_dres(p_login, p_perfil_id, p_historico) ad
				on e.dre_id = ad.codigo 
				and ((tc.modalidade = 1 and ad.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and ad.modalidade_codigo = 3))
				and et.local_ocorrencia in (2, 5)
			left join f_abrangencia_ues(p_login, p_perfil_id, p_historico) au
				on e.ue_id = au.codigo
				and ((tc.modalidade = 1 and au.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and au.modalidade_codigo = 3))
				and et.local_ocorrencia in (1, 4, 5)
	where e.data_inicio <> e.data_fim
		and et.ativo 
		and not et.excluido
		and not e.excluido
		and extract(month from e.data_fim) = p_mes
		and extract(year from e.data_inicio) = tc.ano_letivo
		and e.tipo_calendario_id = p_tipo_calendario_id
		-- caso considere 1 (aprovado) e 2 (pendente de aprovacao), senao considera so aprovados
		and ((p_considera_pendente_aprovacao = true and e.status in (1,2)) or (p_considera_pendente_aprovacao = false and e.status = 1)) 
		and ((p_dre_id is null and ((e.dre_id is null and e.ue_id is null) or e.dre_id in (select codigo from f_abrangencia_dres(p_login, p_perfil_id, p_historico)))) or (p_dre_id is not null and ((e.dre_id is null and e.ue_id is null) or e.dre_id = p_dre_id)))
		and ((p_ue_id is null and (e.ue_id is null or e.ue_id in (select codigo from f_abrangencia_ues(p_login, p_perfil_id, p_historico)))) or (p_ue_id is not null and (e.ue_id is null or e.ue_id = p_ue_id)))
		-- caso desconsidere o local do evento 2 (DRE)
		and (p_desconsidera_local_dre = false  or (p_desconsidera_local_dre = true and et.local_ocorrencia != 2))
		-- caso desconsidere evento SME
		and (p_desconsidera_evento_sme = false or (p_desconsidera_evento_sme = true and not (e.dre_id is null and e.ue_id is null)));	
$function$
;
