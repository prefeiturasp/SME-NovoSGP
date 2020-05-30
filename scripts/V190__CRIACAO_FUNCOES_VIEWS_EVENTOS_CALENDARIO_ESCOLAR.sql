drop function if exists public.f_abrangencia_dres;
drop function if exists public.f_abrangencia_ues;
drop view if exists public.v_estrutura_abrangencia_dres;
drop view if exists public.v_estrutura_abrangencia_ues;
drop function if exists public.f_eventos_calendario_dias_com_eventos_no_mes;
drop function if exists public.f_eventos_calendario_eventos_do_dia;
drop function if exists public.f_eventos_calendario_por_data_inicio_fim;
drop function if exists public.f_eventos_calendario_por_rf_criador;
drop view if exists public.v_estrutura_eventos_calendario;
drop view if exists public.v_estrutura_eventos_calendario_dias_com_eventos_no_mes;

CREATE OR REPLACE VIEW public.v_estrutura_abrangencia_dres
AS SELECT v_abrangencia_cadeia_turmas.dre_abreviacao AS abreviacao,
    v_abrangencia_cadeia_turmas.dre_codigo AS codigo,
    v_abrangencia_cadeia_turmas.dre_nome AS nome,
    v_abrangencia_cadeia_turmas.modalidade_codigo
   FROM v_abrangencia_cadeia_turmas;

CREATE OR REPLACE VIEW public.v_estrutura_abrangencia_ues
AS SELECT act.ue_codigo AS codigo,
    act.ue_nome AS nome,
    ue.tipo_escola AS tipoescola,
    act.modalidade_codigo
   FROM v_abrangencia_cadeia_turmas act
     JOIN ue ON act.ue_id = act.ue_id;
     
CREATE OR REPLACE FUNCTION public.f_abrangencia_dres(character varying, uuid, boolean, integer DEFAULT 0, integer DEFAULT 0, integer DEFAULT 0)
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
where a.login = $1 and 
	  a.perfil_id = $2 and
	  act.turma_historica = $3 and		  
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 = 0 or ($6 <> 0 and act.turma_ano_letivo = $6))
	 
union

select distinct act.dre_abreviacao,
				act.dre_codigo,
                act.dre_nome,
                act.modalidade_codigo
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
where a.login = $1 and 
	  a.perfil_id = $2 and
	  act.turma_historica = $3 and
	  (($2 <> '4ee1e074-37d6-e911-abd6-f81654fe895d') or
	   ($3 = true and 
	    $2 = '4ee1e074-37d6-e911-abd6-f81654fe895d' and 
	    act.dre_id in (select dre_id from v_abrangencia_nivel_dre where login = $1 and historico = false) and 
	   	act.ue_id in (select ue_id from v_abrangencia_nivel_ue where login = $1 and historico = false)) or
	   ($3 = false and $2 = '4ee1e074-37d6-e911-abd6-f81654fe895d' and a.historico = false)) and 
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 = 0 or ($6 <> 0 and act.turma_ano_letivo = $6))

union

select distinct act.dre_abreviacao,
				act.dre_codigo,
                act.dre_nome,
                act.modalidade_codigo
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
where a.login = $1 and 
	  a.perfil_id = $2 and
	  a.historico = $3 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 = 0 or ($6 <> 0 and act.turma_ano_letivo = $6));
$function$;

CREATE OR REPLACE FUNCTION public.f_abrangencia_ues(character varying, uuid, boolean, integer DEFAULT 0, integer DEFAULT 0, character varying DEFAULT NULL::character varying, integer DEFAULT 0)
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
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
	  ($6 is null or ($6 is not null and act.dre_codigo = $6)) and
      ($7 = 0 or ($7 <> 0 and act.turma_ano_letivo = $7))
	 
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
where a.login = $1 and 
	  a.perfil_id = $2 and
	  act.turma_historica = $3 and
	  (($2 <> '4ee1e074-37d6-e911-abd6-f81654fe895d') or
	   ($3 = true and 
	    $2 = '4ee1e074-37d6-e911-abd6-f81654fe895d' and 
	    act.dre_id in (select dre_id from v_abrangencia_nivel_dre where login = $1 and historico = false) and 
	   	act.ue_id in (select ue_id from v_abrangencia_nivel_ue where login = $1 and historico = false)) or
	   ($3 = false and $2 = '4ee1e074-37d6-e911-abd6-f81654fe895d' and a.historico = false)) and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 is null or ($6 is not null and act.dre_codigo = $6)) and
      ($7 = 0 or ($7 <> 0 and act.turma_ano_letivo = $7))

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
where a.login = $1 and 
	  a.perfil_id = $2 and
	  a.historico = $3 and
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 is null or ($6 is not null and act.dre_codigo = $6)) and
      ($7 = 0 or ($7 <> 0 and act.turma_ano_letivo = $7));
$function$;

CREATE OR REPLACE VIEW public.v_estrutura_eventos_calendario
AS SELECT evento.id,
    evento.data_inicio AS data_evento,
    '(início)'::text AS iniciofimdesc,
    evento.nome,
    'aaaa'::text AS tipoevento
   FROM evento;
  
CREATE OR REPLACE VIEW public.v_estrutura_eventos_calendario_dias_com_eventos_no_mes
AS SELECT date_part('day'::text, CURRENT_DATE) AS dia,
    'AAAA'::text AS tipoevento;

CREATE OR REPLACE FUNCTION public.f_eventos_calendario_por_data_inicio_fim(character varying, uuid, boolean, integer, bigint, boolean DEFAULT false, character varying DEFAULT NULL::character varying, character varying DEFAULT NULL::character varying, boolean DEFAULT false, boolean DEFAULT false)
 RETURNS SETOF v_estrutura_eventos_calendario
 LANGUAGE sql
AS $function$
	select distinct e.id,
					e.data_inicio,
					case
						when data_inicio = data_fim then ''
						else '(início)'
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
			left join f_abrangencia_dres($1, $2, $3) ad
				on e.dre_id = ad.codigo 
				-- modalidade 1 (fundamental/médio) do tipo de calendario, considera as modalidades 5 (Fundamental) e 6 (médio)
				-- modalidade 2 (EJA) do tipo de calendário, considera modalidade 3 (EJA)
				and ((tc.modalidade = 1 and ad.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and ad.modalidade_codigo = 3))
				-- para DREs considera local da ocorrência 2 (DRE) e 5 (Todos)
				and et.local_ocorrencia in (2, 5)
			left join f_abrangencia_ues($1, $2, $3) au
				on e.ue_id = au.codigo
				and ((tc.modalidade = 1 and au.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and au.modalidade_codigo = 3))
				-- para UEs considera local da ocorrência 1 (UE) e 4 (SME/UE) e 5 (Todos)
				and et.local_ocorrencia in (1, 4, 5)
	where et.ativo 
		and not et.excluido
		and not e.excluido
		and extract(month from e.data_inicio) = $4
		and extract(year from e.data_inicio) = tc.ano_letivo	
		and e.tipo_calendario_id = $5
		-- caso considere 1 (aprovado) e 2 (pendente de aprovação), senão considera só aprovados
		and (($6 = true and e.status in (1,2)) or ($6 = false and e.status = 1)) 
		and ($7 is null or ($7 is not null and e.dre_id = $7))
		and ($8 is null or ($8 is not null and e.ue_id = $8))
		-- caso desconsidere o local do evento 2 (DRE)
		and ($9 = false or ($9 = true and et.local_ocorrencia != 2))
		-- caso desconsidere evento SME
		and ($10 = false or ($10 = true and et.local_ocorrencia not in (3, 4, 5)))
		
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
			left join f_abrangencia_dres($1, $2, $3) ad
				on e.dre_id = ad.codigo 
				and ((tc.modalidade = 1 and ad.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and ad.modalidade_codigo = 3))
				and et.local_ocorrencia in (2, 5)
			left join f_abrangencia_ues($1, $2, $3) au
				on e.ue_id = au.codigo
				and ((tc.modalidade = 1 and au.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and au.modalidade_codigo = 3))
				and et.local_ocorrencia in (1, 4, 5)
	where e.data_inicio <> e.data_fim
		and et.ativo 
		and not et.excluido
		and not e.excluido
		and extract(month from e.data_fim) = $4
		and extract(year from e.data_inicio) = tc.ano_letivo
		and e.tipo_calendario_id = $5
		-- caso considere 1 (aprovado) e 2 (pendente de aprovação), senão considera só aprovados
		and (($6 = true and e.status in (1,2)) or ($6 = false and e.status = 1)) 
		and ($7 is null or ($7 is not null and e.dre_id = $7))
		and ($8 is null or ($8 is not null and e.ue_id = $8))
		-- caso desconsidere o local do evento 2 (DRE)
		and ($9 = false  or ($9 = true and et.local_ocorrencia != 2))
		-- caso desconsidere evento SME
		and ($10 = false or ($10 = true and et.local_ocorrencia not in (3, 4, 5)));	
$function$;

CREATE OR REPLACE FUNCTION public.f_eventos_calendario_por_rf_criador(character varying, integer, bigint, character varying DEFAULT NULL::character varying, character varying DEFAULT NULL::character varying, boolean DEFAULT false, boolean DEFAULT false)
 RETURNS SETOF v_estrutura_eventos_calendario
 LANGUAGE sql
AS $function$
	select e.id,
		   e.data_inicio,
		   case 
		      when e.data_inicio = e.data_fim then ''
		      else '(início)'
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
		-- considera somente pendente de aprovação
		and e.status = 2
		and e.criado_rf = $1
		and extract(month from e.data_inicio) = $2
		and extract(year from e.data_inicio) = tc.ano_letivo
		and e.tipo_calendario_id = $3
		and ($4 is null or ($4 is not null and e.dre_id = $4))
		and ($5 is null or ($5 is not null and e.ue_id = $5))
		-- caso desconsidere o local do evento 2 (DRE)
		and ($6 = false or ($6 = true and et.local_ocorrencia != 2))
		-- caso desconsidere eventos SME
		and ($7 = false or ($7 = true and et.local_ocorrencia not in (3, 4, 5)))
		
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
		-- considera somente pendente de aprovação
		and e.status = 2
		and e.criado_rf = $1
		and extract(month from e.data_fim) = $2
		and extract(year from e.data_fim) = tc.ano_letivo
		and e.tipo_calendario_id = $3
		and ($4 is null or ($4 is not null and e.dre_id = $4))
		and ($5 is null or ($5 is not null and e.ue_id = $5))
		-- caso desconsidere o local do evento 2 (DRE)
		and ($6 = false or ($6 = true and et.local_ocorrencia != 2))
		-- caso desconsidere eventos SME
		and ($7 = false or ($7 = true and et.local_ocorrencia not in (3, 4, 5)));
$function$;

CREATE OR REPLACE FUNCTION public.f_eventos_calendario_dias_com_eventos_no_mes(character varying, uuid, boolean, integer, bigint, boolean DEFAULT false, character varying DEFAULT NULL::character varying, character varying DEFAULT NULL::character varying, boolean DEFAULT false, boolean DEFAULT false)
 RETURNS SETOF v_estrutura_eventos_calendario_dias_com_eventos_no_mes
 LANGUAGE sql
AS $function$ 	
select lista.dia,
	   lista.tipoEvento
from (

select distinct extract(day from data_evento) as dia,
				tipoEvento
	from f_eventos_calendario_por_data_inicio_fim($1, $2, $3, $4, $5, $6, $7, $8, $9, $10)

union 

select distinct extract(day from data_evento) as dia,
				tipoEvento
	from f_eventos_calendario_por_rf_criador($1, $4, $5, $7, $8, $9, $10)) lista
order by 1;
 $function$;

CREATE OR REPLACE FUNCTION public.f_eventos_calendario_eventos_do_dia(character varying, uuid, boolean, integer, integer, bigint, boolean DEFAULT false, character varying DEFAULT NULL::character varying, character varying DEFAULT NULL::character varying, boolean DEFAULT false, boolean DEFAULT false)
 RETURNS SETOF v_estrutura_eventos_calendario
 LANGUAGE sql
AS $function$
	
	select id,
		   data_evento,
		   iniciofimdesc,
		   nome,
		   tipoevento
		from f_eventos_calendario_por_data_inicio_fim($1, $2, $3, $5, $6, $7, $8, $9, $10, $11)
	where extract(day from data_evento) = $4
	
	union
	
	select id,
		   data_evento,
		   iniciofimdesc,
		   nome,
		   tipoevento
		from f_eventos_calendario_por_rf_criador($1, $5, $6, $8, $9, $10, $11)
	where extract(day from data_evento) = $4;

$function$;