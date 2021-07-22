CREATE INDEX if not exists registro_frequencia_aluno_rf_id_idx ON public.registro_frequencia_aluno USING btree (registro_frequencia_id);


drop function if exists f_abrangencia_turmas_tipos(character varying,uuid,boolean,integer,integer,character varying,integer,integer[]);
drop function if exists f_abrangencia_turmas_tipos(character varying,uuid,boolean,integer,integer,character varying,integer,integer[],text[]);

drop view if exists v_estrutura_abrangencia_turmas_tipos;
drop function if exists f_abrangencia_turmas;

drop view if exists v_estrutura_abrangencia_turmas;

drop function if exists f_abrangencia_ues;
drop view if exists v_estrutura_abrangencia_ues;


drop view if exists v_abrangencia;
drop view if exists v_abrangencia_cadeia_dres;

drop view if exists v_abrangencia_cadeia_ues;
drop view if exists v_abrangencia_historica;

drop view if exists v_abrangencia_usuario;

drop function if exists f_abrangencia_dres;
drop view if exists v_estrutura_abrangencia_dres;

drop view if exists v_abrangencia_cadeia_turmas;


ALTER TABLE turma ALTER COLUMN ano TYPE varchar(1);

-- public.v_abrangencia_cadeia_turmas source

CREATE OR REPLACE VIEW public.v_abrangencia_cadeia_turmas
AS SELECT ab_dres.id AS dre_id,
    ab_dres.dre_id AS dre_codigo,
    ab_dres.abreviacao AS dre_abreviacao,
    ab_dres.nome AS dre_nome,
    ab_ues.id AS ue_id,
    ab_ues.ue_id AS ue_codigo,
    ab_ues.nome AS ue_nome,
    ab_turma.id AS turma_id,
    ab_turma.ano AS turma_ano,
    ab_turma.ano_letivo AS turma_ano_letivo,
    ab_turma.modalidade_codigo,
    ab_turma.nome AS turma_nome,
    ab_turma.semestre AS turma_semestre,
    ab_turma.qt_duracao_aula,
    ab_turma.tipo_turno,
    ab_turma.turma_id AS turma_codigo,
    ab_turma.historica AS turma_historica,
    ab_turma.dt_fim_eol AS dt_fim_turma,
    ab_turma.ensino_especial,
    ab_turma.tipo_turma,
    ab_turma.nome_filtro
   FROM dre ab_dres
     JOIN ue ab_ues ON ab_ues.dre_id = ab_dres.id
     JOIN turma ab_turma ON ab_turma.ue_id = ab_ues.id;



-- public.v_estrutura_abrangencia_turmas source

CREATE OR REPLACE VIEW public.v_estrutura_abrangencia_turmas
AS SELECT v_abrangencia_cadeia_turmas.turma_ano AS ano,
    v_abrangencia_cadeia_turmas.turma_ano_letivo AS anoletivo,
    v_abrangencia_cadeia_turmas.turma_codigo AS codigo,
    v_abrangencia_cadeia_turmas.modalidade_codigo AS codigomodalidade,
    v_abrangencia_cadeia_turmas.turma_nome AS nome,
    v_abrangencia_cadeia_turmas.turma_semestre AS semestre,
    v_abrangencia_cadeia_turmas.qt_duracao_aula AS qtduracaoaula,
    v_abrangencia_cadeia_turmas.tipo_turno AS tipoturno,
    v_abrangencia_cadeia_turmas.ensino_especial AS ensinoespecial,
    v_abrangencia_cadeia_turmas.turma_id,
    v_abrangencia_cadeia_turmas.nome_filtro
   FROM v_abrangencia_cadeia_turmas;
    
CREATE OR REPLACE VIEW public.v_estrutura_abrangencia_dres
AS SELECT v_abrangencia_cadeia_turmas.dre_abreviacao AS abreviacao,
    v_abrangencia_cadeia_turmas.dre_codigo AS codigo,
    v_abrangencia_cadeia_turmas.dre_nome AS nome,
    v_abrangencia_cadeia_turmas.modalidade_codigo,
    v_abrangencia_cadeia_turmas.dre_id
   FROM v_abrangencia_cadeia_turmas;
  
------------------------------
CREATE OR REPLACE FUNCTION public.f_abrangencia_dres(p_login character varying, p_perfil_id uuid, p_historico boolean, p_modalidade_codigo integer DEFAULT 0, p_turma_semestre integer DEFAULT 0, p_ano_letivo integer DEFAULT 0)
 RETURNS SETOF v_estrutura_abrangencia_dres
 LANGUAGE sql
AS $function$
select distinct act.dre_abreviacao,
				act.dre_codigo,
                act.dre_nome,
                act.modalidade_codigo,
                act.dre_id as id 
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
                act.modalidade_codigo,
                act.dre_id
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
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
                act.modalidade_codigo,
                act.dre_id
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  ((p_historico = true and a.historico = true) or
	   (p_historico = false and a.historico  = false and act.turma_historica = false)) and	  
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo));
$function$
;


-- public.v_abrangencia_usuario source

CREATE OR REPLACE VIEW public.v_abrangencia_usuario
AS SELECT COALESCE(turma.dre_codigo, ue.dre_codigo, dre.dre_codigo) AS dre_codigo,
    COALESCE(turma.dre_abreviacao, ue.dre_abreviacao, dre.dre_abreviacao) AS dre_abreviacao,
    COALESCE(turma.dre_nome, ue.dre_nome, dre.dre_nome) AS dre_nome,
    a.usuario_id,
    a.perfil AS usuario_perfil,
    u.login,
    COALESCE(turma.ue_codigo, ue.ue_codigo, dre.ue_codigo) AS ue_codigo,
    COALESCE(turma.ue_nome, ue.ue_nome, dre.ue_nome) AS ue_nome,
    COALESCE(turma.turma_ano, ue.turma_ano, dre.turma_ano) AS turma_ano,
    COALESCE(turma.turma_ano_letivo, ue.turma_ano_letivo, dre.turma_ano_letivo) AS turma_ano_letivo,
    COALESCE(turma.modalidade_codigo, ue.modalidade_codigo, dre.modalidade_codigo) AS modalidade_codigo,
    COALESCE(turma.turma_nome, ue.turma_nome, dre.turma_nome) AS turma_nome,
    COALESCE(turma.turma_semestre, ue.turma_semestre, dre.turma_semestre) AS turma_semestre,
    COALESCE(turma.qt_duracao_aula, ue.qt_duracao_aula, dre.qt_duracao_aula) AS qt_duracao_aula,
    COALESCE(turma.tipo_turno, ue.tipo_turno, dre.tipo_turno) AS tipo_turno,
    COALESCE(turma.turma_codigo, ue.turma_codigo, dre.turma_codigo) AS turma_id
   FROM abrangencia a
     JOIN usuario u ON a.usuario_id = u.id
     LEFT JOIN v_abrangencia_cadeia_dres_somente dre ON dre.dre_id = a.dre_id
     LEFT JOIN v_abrangencia_cadeia_ues_somente ue ON ue.ue_id = a.ue_id
     LEFT JOIN v_abrangencia_cadeia_turmas turma ON a.dre_id = turma.dre_id OR a.ue_id = turma.ue_id OR turma.turma_id = a.turma_id
  WHERE a.historico = false AND (COALESCE(turma.turma_historica, ue.turma_historica, dre.turma_historica) = false OR COALESCE(turma.turma_historica, ue.turma_historica, dre.turma_historica) IS NULL);

-- public.v_abrangencia_historica source

CREATE OR REPLACE VIEW public.v_abrangencia_historica
AS SELECT COALESCE(turma.dre_codigo, ue.dre_codigo, dre.dre_codigo) AS dre_codigo,
    COALESCE(turma.dre_abreviacao, ue.dre_abreviacao, dre.dre_abreviacao) AS dre_abreviacao,
    COALESCE(turma.dre_nome, ue.dre_nome, dre.dre_nome) AS dre_nome,
    a.usuario_id,
    a.perfil AS usuario_perfil,
    u.login,
    COALESCE(turma.ue_codigo, ue.ue_codigo, dre.ue_codigo) AS ue_codigo,
    COALESCE(turma.ue_nome, ue.ue_nome, dre.ue_nome) AS ue_nome,
    COALESCE(turma.turma_ano, ue.turma_ano, dre.turma_ano) AS turma_ano,
    COALESCE(turma.turma_ano_letivo, ue.turma_ano_letivo, dre.turma_ano_letivo) AS turma_ano_letivo,
    COALESCE(turma.modalidade_codigo, ue.modalidade_codigo, dre.modalidade_codigo) AS modalidade_codigo,
    COALESCE(turma.turma_nome, ue.turma_nome, dre.turma_nome) AS turma_nome,
    COALESCE(turma.turma_semestre, ue.turma_semestre, dre.turma_semestre) AS turma_semestre,
    COALESCE(turma.qt_duracao_aula, ue.qt_duracao_aula, dre.qt_duracao_aula) AS qt_duracao_aula,
    COALESCE(turma.tipo_turno, ue.tipo_turno, dre.tipo_turno) AS tipo_turno,
    COALESCE(turma.turma_codigo, ue.turma_codigo, dre.turma_codigo) AS turma_id,
    COALESCE(turma.dt_fim_turma, ue.dt_fim_turma, dre.dt_fim_turma) AS dt_fim_turma,
    a.dt_fim_vinculo
   FROM abrangencia a
     JOIN usuario u ON a.usuario_id = u.id
     LEFT JOIN v_abrangencia_cadeia_dres_somente dre ON dre.dre_id = a.dre_id
     LEFT JOIN v_abrangencia_cadeia_ues_somente ue ON ue.ue_id = a.ue_id
     LEFT JOIN v_abrangencia_cadeia_turmas turma ON a.dre_id = turma.dre_id OR a.ue_id = turma.ue_id OR a.turma_id = turma.turma_id
  WHERE a.historico = true OR dre.turma_historica = true;

-- public.v_abrangencia_cadeia_ues source

CREATE OR REPLACE VIEW public.v_abrangencia_cadeia_ues
AS SELECT v_abrangencia_cadeia_turmas.dre_id,
    v_abrangencia_cadeia_turmas.dre_codigo,
    v_abrangencia_cadeia_turmas.dre_abreviacao,
    v_abrangencia_cadeia_turmas.dre_nome,
    v_abrangencia_cadeia_turmas.ue_id,
    v_abrangencia_cadeia_turmas.ue_codigo,
    v_abrangencia_cadeia_turmas.ue_nome,
    v_abrangencia_cadeia_turmas.turma_id,
    v_abrangencia_cadeia_turmas.turma_ano,
    v_abrangencia_cadeia_turmas.turma_ano_letivo,
    v_abrangencia_cadeia_turmas.modalidade_codigo,
    v_abrangencia_cadeia_turmas.turma_nome,
    v_abrangencia_cadeia_turmas.turma_semestre,
    v_abrangencia_cadeia_turmas.qt_duracao_aula,
    v_abrangencia_cadeia_turmas.tipo_turno,
    v_abrangencia_cadeia_turmas.turma_codigo,
    v_abrangencia_cadeia_turmas.turma_historica,
    v_abrangencia_cadeia_turmas.dt_fim_turma
   FROM v_abrangencia_cadeia_turmas
UNION ALL
 SELECT ab_dres.id AS dre_id,
    ab_dres.dre_id AS dre_codigo,
    ab_dres.abreviacao AS dre_abreviacao,
    ab_dres.nome AS dre_nome,
    ab_ues.id AS ue_id,
    ab_ues.ue_id AS ue_codigo,
    ab_ues.nome AS ue_nome,
    NULL::bigint AS turma_id,
    NULL::bpchar AS turma_ano,
    NULL::integer AS turma_ano_letivo,
    NULL::integer AS modalidade_codigo,
    NULL::character varying AS turma_nome,
    NULL::integer AS turma_semestre,
    NULL::smallint AS qt_duracao_aula,
    NULL::smallint AS tipo_turno,
    NULL::character varying AS turma_codigo,
    NULL::boolean AS turma_historica,
    NULL::date AS dt_fim_turma
   FROM dre ab_dres
     JOIN ue ab_ues ON ab_ues.dre_id = ab_dres.id;
    
-- public.v_abrangencia_cadeia_dres source

CREATE OR REPLACE VIEW public.v_abrangencia_cadeia_dres
AS SELECT v_abrangencia_cadeia_turmas.dre_id,
    v_abrangencia_cadeia_turmas.dre_codigo,
    v_abrangencia_cadeia_turmas.dre_abreviacao,
    v_abrangencia_cadeia_turmas.dre_nome,
    v_abrangencia_cadeia_turmas.ue_id,
    v_abrangencia_cadeia_turmas.ue_codigo,
    v_abrangencia_cadeia_turmas.ue_nome,
    v_abrangencia_cadeia_turmas.turma_id,
    v_abrangencia_cadeia_turmas.turma_ano,
    v_abrangencia_cadeia_turmas.turma_ano_letivo,
    v_abrangencia_cadeia_turmas.modalidade_codigo,
    v_abrangencia_cadeia_turmas.turma_nome,
    v_abrangencia_cadeia_turmas.turma_semestre,
    v_abrangencia_cadeia_turmas.qt_duracao_aula,
    v_abrangencia_cadeia_turmas.tipo_turno,
    v_abrangencia_cadeia_turmas.turma_codigo,
    v_abrangencia_cadeia_turmas.turma_historica,
    v_abrangencia_cadeia_turmas.dt_fim_turma
   FROM v_abrangencia_cadeia_turmas
UNION ALL
 SELECT ab_dres.id AS dre_id,
    ab_dres.dre_id AS dre_codigo,
    ab_dres.abreviacao AS dre_abreviacao,
    ab_dres.nome AS dre_nome,
    NULL::bigint AS ue_id,
    NULL::character varying AS ue_codigo,
    NULL::character varying AS ue_nome,
    NULL::bigint AS turma_id,
    NULL::bpchar AS turma_ano,
    NULL::integer AS turma_ano_letivo,
    NULL::integer AS modalidade_codigo,
    NULL::character varying AS turma_nome,
    NULL::integer AS turma_semestre,
    NULL::smallint AS qt_duracao_aula,
    NULL::smallint AS tipo_turno,
    NULL::character varying AS turma_codigo,
    NULL::boolean AS turma_historica,
    NULL::date AS dt_fim_turma
   FROM dre ab_dres;
  
-- public.v_abrangencia source

CREATE OR REPLACE VIEW public.v_abrangencia
AS SELECT COALESCE(turma.dre_codigo, ue.dre_codigo, dre.dre_codigo) AS dre_codigo,
    COALESCE(turma.dre_abreviacao, ue.dre_abreviacao, dre.dre_abreviacao) AS dre_abreviacao,
    COALESCE(turma.dre_nome, ue.dre_nome, dre.dre_nome) AS dre_nome,
    a.usuario_id,
    a.perfil AS usuario_perfil,
    COALESCE(turma.ue_codigo, ue.ue_codigo, dre.ue_codigo) AS ue_codigo,
    COALESCE(turma.ue_nome, ue.ue_nome, dre.ue_nome) AS ue_nome,
    COALESCE(turma.turma_ano, ue.turma_ano, dre.turma_ano) AS turma_ano,
    COALESCE(turma.turma_ano_letivo, ue.turma_ano_letivo, dre.turma_ano_letivo) AS turma_ano_letivo,
    COALESCE(turma.modalidade_codigo, ue.modalidade_codigo, dre.modalidade_codigo) AS modalidade_codigo,
    COALESCE(turma.turma_nome, ue.turma_nome, dre.turma_nome) AS turma_nome,
    COALESCE(turma.turma_semestre, ue.turma_semestre, dre.turma_semestre) AS turma_semestre,
    COALESCE(turma.qt_duracao_aula, ue.qt_duracao_aula, dre.qt_duracao_aula) AS qt_duracao_aula,
    COALESCE(turma.tipo_turno, ue.tipo_turno, dre.tipo_turno) AS tipo_turno,
    COALESCE(turma.turma_codigo, ue.turma_codigo, dre.turma_codigo) AS turma_id
   FROM abrangencia a
     LEFT JOIN v_abrangencia_cadeia_dres dre ON dre.dre_id = a.dre_id
     LEFT JOIN v_abrangencia_cadeia_ues ue ON ue.ue_id = a.ue_id
     LEFT JOIN v_abrangencia_cadeia_turmas turma ON turma.turma_id = a.turma_id
  WHERE a.historico = false AND (COALESCE(turma.turma_historica, ue.turma_historica, dre.turma_historica) = false OR COALESCE(turma.turma_historica, ue.turma_historica, dre.turma_historica) IS NULL);
 
-- public.v_estrutura_abrangencia_ues source

CREATE OR REPLACE VIEW public.v_estrutura_abrangencia_ues
AS SELECT act.ue_codigo AS codigo,
    act.ue_nome AS nome,
    ue.tipo_escola AS tipoescola,
    act.modalidade_codigo,
    act.ue_id
   FROM v_abrangencia_cadeia_turmas act
     JOIN ue ON act.ue_id = act.ue_id;
    

CREATE OR REPLACE FUNCTION public.f_abrangencia_ues(p_login character varying, p_perfil_id uuid, p_historico boolean, p_modalidade_codigo integer DEFAULT 0, p_turma_semestre integer DEFAULT 0, p_dre_codigo character varying DEFAULT NULL::character varying, p_ano_letivo integer DEFAULT 0, p_ignorar_tipos_ue integer[] DEFAULT NULL::integer[])
 RETURNS SETOF v_estrutura_abrangencia_ues
 LANGUAGE sql
AS $function$
select distinct act.ue_codigo,
				act.ue_nome,
                ue.tipo_escola,
                act.modalidade_codigo,
                act.ue_id 
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
      and (p_ignorar_tipos_ue is null or not(ue.tipo_escola = ANY(p_ignorar_tipos_ue)))

union

select distinct act.ue_codigo,
				act.ue_nome,
                ue.tipo_escola,
                act.modalidade_codigo,
                act.ue_id
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
		inner join ue
			on act.ue_id = ue.id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  act.turma_historica = p_historico and	  
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_dre_codigo is null or (p_dre_codigo is not null and act.dre_codigo = p_dre_codigo)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))
      and (p_ignorar_tipos_ue is null or not(ue.tipo_escola = ANY(p_ignorar_tipos_ue)))

union

select distinct act.ue_codigo,
				act.ue_nome,
                ue.tipo_escola,
                act.modalidade_codigo,
                act.ue_id
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
		inner join ue
			on act.ue_id = ue.id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  ((p_historico = true and a.historico = true) or
	   (p_historico = false and a.historico  = false and act.turma_historica = false)) and	  
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_dre_codigo is null or (p_dre_codigo is not null and act.dre_codigo = p_dre_codigo)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))
      and (p_ignorar_tipos_ue is null or not(ue.tipo_escola = ANY(p_ignorar_tipos_ue)))

$function$
;


-- public.v_estrutura_abrangencia_turmas source

CREATE OR REPLACE VIEW public.v_estrutura_abrangencia_turmas
AS SELECT v_abrangencia_cadeia_turmas.turma_ano AS ano,
    v_abrangencia_cadeia_turmas.turma_ano_letivo AS anoletivo,
    v_abrangencia_cadeia_turmas.turma_codigo AS codigo,
    v_abrangencia_cadeia_turmas.modalidade_codigo AS codigomodalidade,
    v_abrangencia_cadeia_turmas.turma_nome AS nome,
    v_abrangencia_cadeia_turmas.turma_semestre AS semestre,
    v_abrangencia_cadeia_turmas.qt_duracao_aula AS qtduracaoaula,
    v_abrangencia_cadeia_turmas.tipo_turno AS tipoturno,
    v_abrangencia_cadeia_turmas.ensino_especial AS ensinoespecial,
    v_abrangencia_cadeia_turmas.turma_id,
    v_abrangencia_cadeia_turmas.nome_filtro
   FROM v_abrangencia_cadeia_turmas;
  
-----------------f_abrangencia_turmas
CREATE OR REPLACE FUNCTION public.f_abrangencia_turmas(p_login character varying, p_perfil_id uuid, p_historico boolean, p_modalidade_codigo integer DEFAULT 0, p_turma_semestre integer DEFAULT 0, p_ue_codigo character varying DEFAULT NULL::character varying, p_ano_letivo integer DEFAULT 0, p_anos_desconsiderar_turma_infantil text[] DEFAULT NULL::text[])
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
				act.tipo_turno,
				act.ensino_especial,
				act.turma_id,
				act.nome_filtro
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
      and(
    	p_anos_desconsiderar_turma_infantil is null
    	or act.modalidade_codigo <> 1
    	or(
    		array_length(p_anos_desconsiderar_turma_infantil, 1) > 0 
    		and act.modalidade_codigo = 1
    		and not act.turma_ano = ANY(p_anos_desconsiderar_turma_infantil)
    	)
      )	 
union

select distinct act.turma_ano,
				act.turma_ano_letivo,
				act.turma_codigo,
				act.modalidade_codigo,
				act.turma_nome,
				act.turma_semestre,
				act.qt_duracao_aula,
				act.tipo_turno,
				act.ensino_especial ,
				act.turma_id,
				act.nome_filtro
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
		inner join ue
			on act.ue_id = ue.id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  act.turma_historica = p_historico and	 
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_ue_codigo is null or (p_ue_codigo is not null and act.ue_codigo = p_ue_codigo)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))
      and(
    	p_anos_desconsiderar_turma_infantil is null
    	or act.modalidade_codigo <> 1
    	or(
    		array_length(p_anos_desconsiderar_turma_infantil, 1) > 0 
    		and act.modalidade_codigo = 1
    		and not act.turma_ano = ANY(p_anos_desconsiderar_turma_infantil)
    	)
      )	 
union

select distinct act.turma_ano,
				act.turma_ano_letivo,
				act.turma_codigo,
				act.modalidade_codigo,
				act.turma_nome,
				act.turma_semestre,
				act.qt_duracao_aula,
				act.tipo_turno,
				act.ensino_especial,
				act.turma_id,
				act.nome_filtro
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
		inner join ue
			on act.ue_id = ue.id
where a.login = p_login and 			
	  a.perfil_id = p_perfil_id and	  
	  a.historico = p_historico and
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_ue_codigo is null or (p_ue_codigo is not null and act.ue_codigo = p_ue_codigo)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))
      and(
    	p_anos_desconsiderar_turma_infantil is null
    	or act.modalidade_codigo <> 1
    	or(
    		array_length(p_anos_desconsiderar_turma_infantil, 1) > 0 
    		and act.modalidade_codigo = 1
    		and not act.turma_ano = ANY(p_anos_desconsiderar_turma_infantil)
    	)
      );
$function$
;

-----------------v_estrutura_abrangencia_turmas_tipos
CREATE OR REPLACE VIEW public.v_estrutura_abrangencia_turmas_tipos
AS SELECT act.turma_ano AS ano,
    act.turma_ano_letivo AS anoletivo,
    act.turma_codigo AS codigo,
    act.modalidade_codigo AS codigomodalidade,
    act.turma_nome AS nome,
    act.turma_semestre AS semestre,
    act.qt_duracao_aula AS qtduracaoaula,
    act.tipo_turno AS tipoturno,
    act.ensino_especial AS ensinoespecial,
    act.turma_id,
    act.tipo_turma AS tipoturma,
	act.nome_filtro
   FROM v_abrangencia_cadeia_turmas act;
  
  
-----------f_abrangencia_turmas_tipos
CREATE OR REPLACE FUNCTION public.f_abrangencia_turmas_tipos(p_login character varying, 
	p_perfil_id uuid, 
	p_historico boolean, 
	p_modalidade_codigo integer DEFAULT 0, 
	p_turma_semestre integer DEFAULT 0, 
	p_ue_codigo character varying DEFAULT NULL::character varying, 
	p_ano_letivo integer DEFAULT 0, 
	p_tipos_turma integer[] DEFAULT NULL::integer[], 
	p_anos_desconsiderar_turma_infantil text[] default null::text[])
 RETURNS SETOF v_estrutura_abrangencia_turmas_tipos
 LANGUAGE sql
AS $function$

select distinct act.turma_ano,
    act.turma_ano_letivo,
    act.turma_codigo,
    act.modalidade_codigo,
    act.turma_nome,
    act.turma_semestre,
    act.qt_duracao_aula,
    act.tipo_turno,
    act.ensino_especial,
    act.turma_id,
    act.tipo_turma,
    act.nome_filtro
from v_abrangencia_nivel_dre a
    inner join v_abrangencia_cadeia_turmas act on a.dre_id = act.dre_id
where a.login = p_login
    and a.perfil_id = p_perfil_id
    and act.turma_historica = p_historico
    and (
        p_modalidade_codigo = 0
        or (
            p_modalidade_codigo <> 0
            and act.modalidade_codigo = p_modalidade_codigo
        )
    )
    and (
        p_turma_semestre = 0
        or (
            p_turma_semestre <> 0
            and act.turma_semestre = p_turma_semestre
        )
    )
    and (
        p_ue_codigo is null
        or (
            p_ue_codigo is not null
            and act.ue_codigo = p_ue_codigo
        )
    )
    and (
        p_ano_letivo = 0
        or (
            p_ano_letivo <> 0
            and act.turma_ano_letivo = p_ano_letivo
        )
    )
    and (
        p_tipos_turma is null
        or (
            array_length(p_tipos_turma, 1) > 0
            and act.tipo_turma = ANY(p_tipos_turma)
        )
    )
    and(
    	p_anos_desconsiderar_turma_infantil is null
    	or act.modalidade_codigo <> 1
    	or(
    		array_length(p_anos_desconsiderar_turma_infantil, 1) > 0 
    		and act.modalidade_codigo = 1
    		and not act.turma_ano = ANY(p_anos_desconsiderar_turma_infantil)
    	)
    )
union
select distinct act.turma_ano,
    act.turma_ano_letivo,
    act.turma_codigo,
    act.modalidade_codigo,
    act.turma_nome,
    act.turma_semestre,
    act.qt_duracao_aula,
    act.tipo_turno,
    act.ensino_especial,
    act.turma_id,
    act.tipo_turma,
    act.nome_filtro
from v_abrangencia_nivel_ue a
    inner join v_abrangencia_cadeia_turmas act on a.ue_id = act.ue_id
    inner join ue on act.ue_id = ue.id
where a.login = p_login
    and a.perfil_id = p_perfil_id
    and act.turma_historica = p_historico
    and (
        p_modalidade_codigo = 0
        or (
            p_modalidade_codigo <> 0
            and act.modalidade_codigo = p_modalidade_codigo
        )
    )
    and (
        p_turma_semestre = 0
        or (
            p_turma_semestre <> 0
            and act.turma_semestre = p_turma_semestre
        )
    )
    and (
        p_ue_codigo is null
        or (
            p_ue_codigo is not null
            and act.ue_codigo = p_ue_codigo
        )
    )
    and (
        p_ano_letivo = 0
        or (
            p_ano_letivo <> 0
            and act.turma_ano_letivo = p_ano_letivo
        )
    )
    and (
        p_tipos_turma is null
        or (
            array_length(p_tipos_turma, 1) > 0
            and act.tipo_turma = ANY(p_tipos_turma)
        )
    )
    and(
    	p_anos_desconsiderar_turma_infantil is null
    	or act.modalidade_codigo <> 1
    	or(
    		array_length(p_anos_desconsiderar_turma_infantil, 1) > 0 
    		and act.modalidade_codigo = 1
    		and not act.turma_ano = ANY(p_anos_desconsiderar_turma_infantil)
    	)
    )
union
select distinct act.turma_ano,
    act.turma_ano_letivo,
    act.turma_codigo,
    act.modalidade_codigo,
    act.turma_nome,
    act.turma_semestre,
    act.qt_duracao_aula,
    act.tipo_turno,
    act.ensino_especial,
    act.turma_id,
    act.tipo_turma,
    act.nome_filtro
from v_abrangencia_nivel_turma a
    inner join v_abrangencia_cadeia_turmas act on a.turma_id = act.turma_id
    inner join ue on act.ue_id = ue.id
where a.login = p_login
    and a.perfil_id = p_perfil_id
    and a.historico = p_historico
    and (
        p_modalidade_codigo = 0
        or (
            p_modalidade_codigo <> 0
            and act.modalidade_codigo = p_modalidade_codigo
        )
    )
    and (
        p_turma_semestre = 0
        or (
            p_turma_semestre <> 0
            and act.turma_semestre = p_turma_semestre
        )
    )
    and (
        p_ue_codigo is null
        or (
            p_ue_codigo is not null
            and act.ue_codigo = p_ue_codigo
        )
    )
    and (
        p_ano_letivo = 0
        or (
            p_ano_letivo <> 0
            and act.turma_ano_letivo = p_ano_letivo
        )
    )
    and (
        p_tipos_turma is null
        or (
            array_length(p_tipos_turma, 1) > 0
            and act.tipo_turma = ANY(p_tipos_turma)
        )
    )
    and(
    	p_anos_desconsiderar_turma_infantil is null
    	or act.modalidade_codigo <> 1
    	or(
    		array_length(p_anos_desconsiderar_turma_infantil, 1) > 0 
    		and act.modalidade_codigo = 1
    		and not act.turma_ano = ANY(p_anos_desconsiderar_turma_infantil)
    	)
    )
$function$
;



CREATE INDEX if not exists turma_ano_idx ON public.turma USING btree (ano);