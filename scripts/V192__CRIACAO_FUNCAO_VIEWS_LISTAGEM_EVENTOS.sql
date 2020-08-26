drop function if exists public.f_abrangencia_anos_letivos;
drop function if exists public.f_abrangencia_dres;
drop function if exists public.f_abrangencia_modalidades;
drop function if exists public.f_abrangencia_semestres;
drop function if exists public.f_abrangencia_turmas;
drop function if exists public.f_abrangencia_ues;
drop function if exists public.f_eventos_calendario_dias_com_eventos_no_mes;
drop function if exists public.f_eventos_calendario_eventos_do_dia;
drop function if exists public.f_eventos_calendario_por_rf_criador;
drop function if exists public.f_eventos_calendario_por_data_inicio_fim;
drop function if exists public.f_eventos_listar;
drop function if exists public.f_eventos_listar_sem_paginacao;
drop function if exists public.f_eventos;
drop function if exists public.f_eventos_por_rf_criador;
drop view if exists public.v_estrutura_eventos;
drop view if exists public.v_estrutura_eventos_listar;

CREATE OR REPLACE FUNCTION public.f_abrangencia_anos_letivos(p_login character varying, p_perfil_id uuid, p_historico boolean)
 RETURNS SETOF integer
 LANGUAGE sql
AS $function$
select distinct act.turma_ano_letivo
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  act.turma_historica = p_historico
	 
union

select distinct act.turma_ano_letivo
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  act.turma_historica = p_historico and
	  ((p_perfil_id <> '4ee1e074-37d6-e911-abd6-f81654fe895d') or
	   (p_historico = true and 
	    p_perfil_id = '4ee1e074-37d6-e911-abd6-f81654fe895d' and 
	    act.dre_id in (select dre_id from v_abrangencia_nivel_dre where login = p_login and historico = false) and 
	   	act.ue_id in (select ue_id from v_abrangencia_nivel_ue where login = p_login and historico = false)) or
	   (p_historico = false and p_perfil_id = '4ee1e074-37d6-e911-abd6-f81654fe895d' and a.historico = false))

union

select distinct act.turma_ano_letivo
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  a.historico = p_historico and
	  act.turma_historica = p_historico;	  
$function$;

CREATE OR REPLACE FUNCTION public.f_abrangencia_dres(p_login character varying, p_perfil_id uuid, p_historico boolean, p_modalidade_codigo integer DEFAULT 0, p_turma_semestre integer DEFAULT 0, p_ano_letivo integer DEFAULT 0)
 RETURNS SETOF v_estrutura_abrangencia_dres
 LANGUAGE sql
AS $function$
select distinct act.dre_abreviacao,
				act.dre_codigo,
                act.dre_nome,
                act.modalidade_codigo
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  act.turma_historica = p_historico and		  
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))
	 
union 

select distinct act.dre_abreviacao,
				act.dre_codigo,
                act.dre_nome,
                act.modalidade_codigo
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  act.turma_historica = p_historico and
	  ((p_perfil_id <> '4ee1e074-37d6-e911-abd6-f81654fe895d') or
	   (p_historico = true and 
	    p_perfil_id = '4ee1e074-37d6-e911-abd6-f81654fe895d' and 
	    act.dre_id in (select dre_id from v_abrangencia_nivel_dre where login = p_login and historico = false) and 
	   	act.ue_id in (select ue_id from v_abrangencia_nivel_ue where login = p_login and historico = false)) or
	   (p_historico = false and p_perfil_id = '4ee1e074-37d6-e911-abd6-f81654fe895d' and a.historico = false)) and 
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))

union 

select distinct act.dre_abreviacao,
				act.dre_codigo,
                act.dre_nome,
                act.modalidade_codigo
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  a.historico = p_historico and	  
	  act.turma_historica = p_historico and
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo));
$function$;

CREATE OR REPLACE FUNCTION public.f_abrangencia_modalidades(p_login character varying, p_perfil_id uuid, p_historico boolean, p_ano_letivo integer)
 RETURNS SETOF integer
 LANGUAGE sql
AS $function$
select distinct act.modalidade_codigo
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  act.turma_historica = p_historico and      
	  act.turma_ano_letivo = p_ano_letivo
	 
union

select distinct act.modalidade_codigo
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  act.turma_historica = p_historico and      
	  ((p_perfil_id <> '4ee1e074-37d6-e911-abd6-f81654fe895d') or
	   (p_historico = true and 
	    p_perfil_id = '4ee1e074-37d6-e911-abd6-f81654fe895d' and 
	    act.dre_id in (select dre_id from v_abrangencia_nivel_dre where login = p_login and historico = false) and 
	   	act.ue_id in (select ue_id from v_abrangencia_nivel_ue where login = p_login and historico = false)) or
	   (p_historico = false and p_perfil_id = '4ee1e074-37d6-e911-abd6-f81654fe895d' and a.historico = false)) and
	  act.turma_ano_letivo = p_ano_letivo

union

select distinct act.modalidade_codigo
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  a.historico = p_historico and
	  act.turma_historica = p_historico and      
	  act.turma_ano_letivo = p_ano_letivo;
$function$;

CREATE OR REPLACE FUNCTION public.f_abrangencia_semestres(p_login character varying, p_perfil_id uuid, p_historico boolean, p_modalidade_codigo integer DEFAULT 0, p_ano_letivo integer DEFAULT 0)
 RETURNS SETOF integer
 LANGUAGE sql
AS $function$
select distinct act.turma_semestre
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  act.turma_historica = p_historico and
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and      
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))
	 
union

select distinct act.turma_semestre
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  act.turma_historica = p_historico and
	  ((p_perfil_id <> '4ee1e074-37d6-e911-abd6-f81654fe895d') or
	   (p_historico = true and 
	    p_perfil_id = '4ee1e074-37d6-e911-abd6-f81654fe895d' and 
	    act.dre_id in (select dre_id from v_abrangencia_nivel_dre where login = p_login and historico = false) and 
	   	act.ue_id in (select ue_id from v_abrangencia_nivel_ue where login = p_login and historico = false)) or
	   (p_historico = false and p_perfil_id = '4ee1e074-37d6-e911-abd6-f81654fe895d' and a.historico = false)) and
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))

union

select distinct act.turma_semestre
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  a.historico = p_historico and
	  act.turma_historica = p_historico and
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo));
$function$;

CREATE OR REPLACE FUNCTION public.f_abrangencia_turmas(p_login character varying, p_perfil_id uuid, p_historico boolean, p_modalidade_codigo integer DEFAULT 0, p_turma_semestre integer DEFAULT 0, p_ue_codigo character varying DEFAULT NULL::character varying, p_ano_letivo integer DEFAULT 0)
 RETURNS SETOF v_estrutura_abrangencia_turmas
 LANGUAGE sql
AS $function$
select distinct act.turma_ano,
				act.turma_ano_letivo,
				act.turma_codigo,
				act.modalidade_codigo,
				act.turma_nome,
				act.turma_semestre,
				act.qt_duracao_aula,
				act.tipo_turno
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  act.turma_historica = p_historico and
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
	  (p_ue_codigo is null or (p_ue_codigo is not null and act.ue_codigo = p_ue_codigo)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))
	 
union

select distinct act.turma_ano,
				act.turma_ano_letivo,
				act.turma_codigo,
				act.modalidade_codigo,
				act.turma_nome,
				act.turma_semestre,
				act.qt_duracao_aula,
				act.tipo_turno
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
		inner join ue
			on act.ue_id = ue.id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  act.turma_historica = p_historico and
	  ((p_perfil_id <> '4ee1e074-37d6-e911-abd6-f81654fe895d') or
	   (p_historico = true and 
	    p_perfil_id = '4ee1e074-37d6-e911-abd6-f81654fe895d' and 
	    act.dre_id in (select dre_id from v_abrangencia_nivel_dre where login = p_login and historico = false) and 
	   	act.ue_id in (select ue_id from v_abrangencia_nivel_ue where login = p_login and historico = false)) or
	   (p_historico = false and p_perfil_id = '4ee1e074-37d6-e911-abd6-f81654fe895d' and a.historico = false)) and
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_ue_codigo is null or (p_ue_codigo is not null and act.ue_codigo = p_ue_codigo)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))

union

select distinct act.turma_ano,
				act.turma_ano_letivo,
				act.turma_codigo,
				act.modalidade_codigo,
				act.turma_nome,
				act.turma_semestre,
				act.qt_duracao_aula,
				act.tipo_turno
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
		inner join ue
			on act.ue_id = ue.id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  a.historico = p_historico and
	  act.turma_historica = p_historico and
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_ue_codigo is null or (p_ue_codigo is not null and act.ue_codigo = p_ue_codigo)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo));
$function$;

CREATE OR REPLACE FUNCTION public.f_abrangencia_ues(p_login character varying, p_perfil_id uuid, p_historico boolean, p_modalidade_codigo integer DEFAULT 0, p_turma_semestre integer DEFAULT 0, p_dre_codigo character varying DEFAULT NULL::character varying, p_ano_letivo integer DEFAULT 0)
 RETURNS SETOF v_estrutura_abrangencia_ues
 LANGUAGE sql
AS $function$
select distinct act.ue_codigo,
				act.ue_nome,
                ue.tipo_escola,
                act.modalidade_codigo
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
		inner join ue
			on act.ue_id = ue.id		
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  act.turma_historica = p_historico and
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
	  (p_dre_codigo is null or (p_dre_codigo is not null and act.dre_codigo = p_dre_codigo)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))
	 
union

select distinct act.ue_codigo,
				act.ue_nome,
                ue.tipo_escola,
                act.modalidade_codigo
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
		inner join ue
			on act.ue_id = ue.id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  act.turma_historica = p_historico and
	  ((p_perfil_id <> '4ee1e074-37d6-e911-abd6-f81654fe895d') or
	   (p_historico = true and 
	    p_perfil_id = '4ee1e074-37d6-e911-abd6-f81654fe895d' and 
	    act.dre_id in (select dre_id from v_abrangencia_nivel_dre where login = p_login and historico = false) and 
	   	act.ue_id in (select ue_id from v_abrangencia_nivel_ue where login = p_login and historico = false)) or
	   (p_historico = false and p_perfil_id = '4ee1e074-37d6-e911-abd6-f81654fe895d' and a.historico = false)) and
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_dre_codigo is null or (p_dre_codigo is not null and act.dre_codigo = p_dre_codigo)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))

union

select distinct act.ue_codigo,
				act.ue_nome,
                ue.tipo_escola,
                act.modalidade_codigo
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
		inner join ue
			on act.ue_id = ue.id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  a.historico = p_historico and
	  act.turma_historica = p_historico and
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_dre_codigo is null or (p_dre_codigo is not null and act.dre_codigo = p_dre_codigo)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo));
$function$;

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
						when ad.codigo is not null then 'DRE'
						when au.codigo is not null then 'UE'
						else 'SME'
					end tipoEvento
		from evento e
			inner join evento_tipo et
				on e.tipo_evento_id = et.id
			inner join tipo_calendario tc
				on e.tipo_calendario_id = tc.id
			left join f_abrangencia_dres(p_login, p_perfil_id, p_historico) ad
				on e.dre_id = ad.codigo 
				-- modalidade 1 (fundamental/m?dio) do tipo de calendario, considera as modalidades 5 (Fundamental) e 6 (m?dio)
				-- modalidade 2 (EJA) do tipo de calend?rio, considera modalidade 3 (EJA)
				and ((tc.modalidade = 1 and ad.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and ad.modalidade_codigo = 3))
				-- para DREs considera local da ocorr?ncia 2 (DRE) e 5 (Todos)
				and et.local_ocorrencia in (2, 5)
			left join f_abrangencia_ues(p_login, p_perfil_id, p_historico) au
				on e.ue_id = au.codigo
				and ((tc.modalidade = 1 and au.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and au.modalidade_codigo = 3))
				-- para UEs considera local da ocorr?ncia 1 (UE) e 4 (SME/UE) e 5 (Todos)
				and et.local_ocorrencia in (1, 4, 5)
	where et.ativo 
		and not et.excluido
		and not e.excluido
		and extract(month from e.data_inicio) = p_mes
		and extract(year from e.data_inicio) = tc.ano_letivo	
		and e.tipo_calendario_id = p_tipo_calendario_id
		-- caso considere 1 (aprovado) e 2 (pendente de aprova??o), sen?o considera s? aprovados
		and ((p_considera_pendente_aprovacao = true and e.status in (1,2)) or (p_considera_pendente_aprovacao = false and e.status = 1)) 
		and (p_dre_id is null or (p_dre_id is not null and e.dre_id = p_dre_id))
		and (p_ue_id is null or (p_ue_id is not null and e.ue_id = p_ue_id))
		-- caso desconsidere o local do evento 2 (DRE)
		and (p_desconsidera_local_dre = false or (p_desconsidera_local_dre = true and et.local_ocorrencia != 2))
		-- caso desconsidere evento SME
		and (p_desconsidera_evento_sme = false or (p_desconsidera_evento_sme = true and et.local_ocorrencia not in (3, 4, 5)))
		
	union
	
	select distinct e.id,
					e.data_fim,
					'(fim)',
					e.nome,
					case
						when ad.codigo is not null then 'DRE'
						when au.codigo is not null then 'UE'
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
		-- caso considere 1 (aprovado) e 2 (pendente de aprova??o), sen?o considera s? aprovados
		and ((p_considera_pendente_aprovacao = true and e.status in (1,2)) or (p_considera_pendente_aprovacao = false and e.status = 1)) 
		and (p_dre_id is null or (p_dre_id is not null and e.dre_id = p_dre_id))
		and (p_ue_id is null or (p_ue_id is not null and e.ue_id = p_ue_id))
		-- caso desconsidere o local do evento 2 (DRE)
		and (p_desconsidera_local_dre = false  or (p_desconsidera_local_dre = true and et.local_ocorrencia != 2))
		-- caso desconsidere evento SME
		and (p_desconsidera_evento_sme = false or (p_desconsidera_evento_sme = true and et.local_ocorrencia not in (3, 4, 5)));	
$function$;

CREATE OR REPLACE FUNCTION public.f_eventos_calendario_por_rf_criador(p_login character varying, p_mes integer, p_tipo_calendario_id bigint, p_dre_id character varying DEFAULT NULL::character varying, p_ue_id character varying DEFAULT NULL::character varying, p_desconsidera_local_dre boolean DEFAULT false, p_desconsidera_evento_sme boolean DEFAULT false)
 RETURNS SETOF v_estrutura_eventos_calendario
 LANGUAGE sql
AS $function$
	select e.id,
		   e.data_inicio,
		   case 
		      when e.data_inicio = e.data_fim then ''
		      else '(inicio)'
		   end descricao_incio_fim,		   
		   e.nome,		   
		   case 
		      when et.local_ocorrencia = 3 then 'DRE'
		      when et.local_ocorrencia = 1 then 'UE'
		      else 'SME'
		   end tipoEvento
		from evento e
			inner join evento_tipo et
				on e.tipo_evento_id = et.id		
			inner join tipo_calendario tc
				on e.tipo_calendario_id = tc.id
	where et.ativo
		and not et.excluido
		and not e.excluido
		-- considera somente pendente de aprova??o
		and e.status = 2
		and e.criado_rf = p_login
		and extract(month from e.data_inicio) = p_mes
		and extract(year from e.data_inicio) = tc.ano_letivo
		and e.tipo_calendario_id = p_tipo_calendario_id
		and (p_dre_id is null or (p_dre_id is not null and e.dre_id = p_dre_id))
		and (p_ue_id is null or (p_ue_id is not null and e.ue_id = p_ue_id))
		-- caso desconsidere o local do evento 2 (DRE)
		and (p_desconsidera_local_dre = false or (p_desconsidera_local_dre = true and et.local_ocorrencia != 2))
		-- caso desconsidere eventos SME
		and (p_desconsidera_evento_sme = false or (p_desconsidera_evento_sme = true and et.local_ocorrencia not in (3, 4, 5)))
		
	union
	
	select e.id,
		   e.data_inicio,
		   '(fim)' descricao_incio_fim,		   
		   e.nome,		   
		   case 
		      when et.local_ocorrencia = 3 then 'DRE'
		      when et.local_ocorrencia = 1 then 'UE'
		      else 'SME'
		   end tipoEvento
		from evento e
			inner join evento_tipo et
				on e.tipo_evento_id = et.id		
			inner join tipo_calendario tc
				on e.tipo_calendario_id = tc.id
	where e.data_inicio <> e.data_fim
		and et.ativo
		and not et.excluido
		and not e.excluido
		-- considera somente pendente de aprova??o
		and e.status = 2
		and e.criado_rf = p_login
		and extract(month from e.data_fim) = p_mes
		and extract(year from e.data_fim) = tc.ano_letivo
		and e.tipo_calendario_id = p_tipo_calendario_id
		and (p_dre_id is null or (p_dre_id is not null and e.dre_id = p_dre_id))
		and (p_ue_id is null or (p_ue_id is not null and e.ue_id = p_ue_id))
		-- caso desconsidere o local do evento 2 (DRE)
		and (p_desconsidera_local_dre = false or (p_desconsidera_local_dre = true and et.local_ocorrencia != 2))
		-- caso desconsidere eventos SME
		and (p_desconsidera_evento_sme = false or (p_desconsidera_evento_sme = true and et.local_ocorrencia not in (3, 4, 5)));
$function$;


CREATE OR REPLACE FUNCTION public.f_eventos_calendario_dias_com_eventos_no_mes(p_login character varying, p_perfil_id uuid, p_historico boolean, p_mes integer, p_tipo_calendario_id bigint, p_considera_pendente_aprovacao boolean DEFAULT false, p_dre_id character varying DEFAULT NULL::character varying, p_ue_id character varying DEFAULT NULL::character varying, p_desconsidera_local_dre boolean DEFAULT false, p_desconsidera_evento_sme boolean DEFAULT false)
 RETURNS SETOF v_estrutura_eventos_calendario_dias_com_eventos_no_mes
 LANGUAGE sql
AS $function$ 	
select lista.dia,
	   lista.tipoEvento
from (

select distinct extract(day from data_evento) as dia,
				tipoEvento
	from f_eventos_calendario_por_data_inicio_fim(p_login, p_perfil_id, p_historico, p_mes, p_tipo_calendario_id, p_considera_pendente_aprovacao, p_dre_id, p_ue_id, p_desconsidera_local_dre, p_desconsidera_evento_sme)

union 

select distinct extract(day from data_evento) as dia,
				tipoEvento
	from f_eventos_calendario_por_rf_criador(p_login, p_mes, p_tipo_calendario_id, p_dre_id, p_ue_id, p_desconsidera_local_dre, p_desconsidera_evento_sme)) lista
order by 1;
 $function$;

CREATE OR REPLACE FUNCTION public.f_eventos_calendario_eventos_do_dia(p_login character varying, p_perfil_id uuid, p_historico boolean, p_dia integer, p_mes integer, p_tipo_calendario_id bigint, p_considera_pendente_aprovacao boolean DEFAULT false, p_dre_id character varying DEFAULT NULL::character varying, p_ue_id character varying DEFAULT NULL::character varying, p_desconsidera_local_dre boolean DEFAULT false, p_desconsidera_evento_sme boolean DEFAULT false)
 RETURNS SETOF v_estrutura_eventos_calendario
 LANGUAGE sql
AS $function$
	
	select id,
		   data_evento,
		   iniciofimdesc,
		   nome,
		   tipoevento
		from f_eventos_calendario_por_data_inicio_fim(p_login, p_perfil_id, p_historico, p_mes, p_tipo_calendario_id, p_considera_pendente_aprovacao, p_dre_id, p_ue_id, p_desconsidera_local_dre, p_desconsidera_evento_sme)
	where extract(day from data_evento) = p_dia
	
	union
	
	select id,
		   data_evento,
		   iniciofimdesc,
		   nome,
		   tipoevento
		from f_eventos_calendario_por_rf_criador(p_login, p_mes, p_tipo_calendario_id, p_dre_id, p_ue_id, p_desconsidera_local_dre, p_desconsidera_evento_sme)
	where extract(day from data_evento) = p_dia;
$function$;

CREATE OR REPLACE VIEW public.v_estrutura_eventos
AS SELECT e.id AS eventoid,
    e.nome,
    e.descricao AS descricaoevento,
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
    et.id AS tipoeventoid,
    et.ativo,
    et.tipo_data,
    et.descricao AS descricaotipoevento,
    et.excluido,
    et.local_ocorrencia
   FROM evento e
     JOIN evento_tipo et ON e.tipo_evento_id = et.id;
    
 CREATE OR REPLACE VIEW public.v_estrutura_eventos_listar
AS SELECT e.id AS eventoid,
    e.nome,
    e.descricao AS descricaoevento,
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
    et.id AS tipoeventoid,
    et.ativo,
    et.tipo_data,
    et.descricao AS descricaotipoevento,
    et.excluido,
    0 AS total_registros
   FROM evento e
     JOIN evento_tipo et ON e.tipo_evento_id = et.id;
    
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
				-- modalidade 1 (fundamental/mdio) do tipo de calendario, considera as modalidades 5 (Fundamental) e 6 (mdio)
				-- modalidade 2 (EJA) do tipo de calendrio, considera modalidade 3 (EJA)
				and ((tc.modalidade = 1 and ad.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and ad.modalidade_codigo = 3))
				-- para DREs considera local da ocorrncia 2 (DRE) e 5 (Todos)
				and et.local_ocorrencia in (2, 5)
			left join f_abrangencia_ues(p_login, p_perfil_id, p_historico) au
				on e.ue_id = au.codigo
				and ((tc.modalidade = 1 and au.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and au.modalidade_codigo = 3))
				-- para UEs considera local da ocorrncia 1 (UE) e 4 (SME/UE) e 5 (Todos)
				and et.local_ocorrencia in (1, 4, 5)
	where not e.excluido 
		and not et.excluido 
		and (extract(year from e.data_inicio) = tc.ano_letivo or extract(year from e.data_fim) = tc.ano_letivo) 
		and e.tipo_calendario_id = p_tipo_calendario_id		
		-- caso considere 1 (aprovado) e 2 (pendente de aprovao), seno considera s aprovados
		and ((p_considera_pendente_aprovacao = true and e.status in (1,2)) or (p_considera_pendente_aprovacao = false and e.status = 1)) 
		and (p_desconsidera_local_dre = false or (p_desconsidera_local_dre = true and et.local_ocorrencia != 2))
		and (p_dre_id is null or (p_dre_id is not null and e.dre_id = p_dre_id))
		and (p_ue_id is null or (p_ue_id is not null and e.ue_id = p_ue_id))
		-- caso desconsidere 6 (liberao excepcional) e 15 (reposio de recesso)
		and (p_desconsidera_liberacao_excep_reposicao_recesso = true or (p_desconsidera_liberacao_excep_reposicao_recesso = false and et.codigo not in (6, 15)))
		and (p_data_inicio is null or (p_data_inicio is not null and date(e.data_inicio) >= date(p_data_inicio)))
		and (p_data_fim is null or (p_data_fim is not null and date(e.data_fim) <= date(p_data_fim)));
$function$;

CREATE OR REPLACE FUNCTION public.f_eventos_por_rf_criador(p_login character varying, p_tipo_calendario_id bigint, p_dre_id character varying DEFAULT NULL::character varying, p_ue_id character varying DEFAULT NULL::character varying, p_data_inicio date DEFAULT NULL::date, p_data_fim date DEFAULT NULL::date)
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
	where not et.excluido
		and not e.excluido
		-- considera somente pendente de aprovao
		and e.status = 2
		and e.criado_rf = p_login		
		and (extract(year from e.data_inicio) = tc.ano_letivo or extract(year from e.data_fim) = tc.ano_letivo)
		and e.tipo_calendario_id = p_tipo_calendario_id
		and (p_dre_id is null or (p_dre_id is not null and e.dre_id = p_dre_id))
		and (p_ue_id is null or (p_ue_id is not null and e.ue_id = p_ue_id))
		and (p_data_inicio is null or (p_data_inicio is not null and date(e.data_inicio) >= date(p_data_inicio)))
		and (p_data_fim is null or (p_data_fim is not null and date(e.data_fim) <= date(p_data_fim)));
$function$;


 