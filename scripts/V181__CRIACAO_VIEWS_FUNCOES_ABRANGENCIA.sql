DROP FUNCTION IF EXISTS public.f_abrangencia_anos_letivos;
DROP FUNCTION IF EXISTS public.f_abrangencia_dres;
DROP FUNCTION IF EXISTS public.f_abrangencia_modalidades;
DROP FUNCTION IF EXISTS public.f_abrangencia_semestres;
DROP FUNCTION IF EXISTS public.f_abrangencia_turmas;
DROP FUNCTION IF EXISTS public.f_abrangencia_ues;
DROP VIEW IF EXISTS public.v_abrangencia_nivel_dre;
DROP VIEW IF EXISTS public.v_abrangencia_nivel_especifico;
DROP VIEW IF EXISTS public.v_abrangencia_nivel_turma;
DROP VIEW IF EXISTS public.v_abrangencia_nivel_ue;
DROP VIEW IF EXISTS public.v_abrangencia_sintetica;
DROP VIEW IF EXISTS public.v_estrutura_abrangencia_dres;
DROP VIEW IF EXISTS public.v_estrutura_abrangencia_turmas;
DROP VIEW IF EXISTS public.v_estrutura_abrangencia_ues;

CREATE VIEW public.v_abrangencia_nivel_dre
AS
	SELECT DISTINCT a.dre_id,
		a.perfil AS perfil_id,
		a.historico,
		u.login
	FROM abrangencia a
		JOIN usuario u
		ON a.usuario_id = u.id
	WHERE a.dre_id IS NOT NULL AND
		a.ue_id IS NULL AND
		a.turma_id IS NULL
	ORDER BY a.dre_id;

CREATE VIEW public.v_abrangencia_nivel_especifico
AS
	SELECT DISTINCT a.dre_id,
		a.ue_id,
		a.turma_id,
		a.perfil AS perfil_id,
		a.historico,
		u.login
	FROM abrangencia a
		JOIN usuario u
		ON a.usuario_id = u.id
	WHERE a.dre_id IS NOT NULL AND
		a.ue_id IS NOT NULL AND
		a.turma_id IS NOT NULL
	ORDER BY a.dre_id, 
             a.ue_id, 
             a.turma_id;

CREATE VIEW public.v_abrangencia_nivel_turma
AS
	SELECT DISTINCT a.turma_id,
		a.perfil AS perfil_id,
		a.historico,
		u.login
	FROM abrangencia a
		JOIN usuario u
		ON a.usuario_id = u.id
	WHERE a.turma_id IS NOT NULL AND
		a.dre_id IS NULL AND
		a.ue_id IS NULL
	ORDER BY a.turma_id;

CREATE VIEW public.v_abrangencia_nivel_ue
AS
	SELECT DISTINCT a.ue_id,
		a.perfil AS perfil_id,
		a.historico,
		u.login
	FROM abrangencia a
		JOIN usuario u
		ON a.usuario_id = u.id
	WHERE a.ue_id IS NOT NULL AND
		a.dre_id IS NULL AND
		a.turma_id IS NULL
	ORDER BY a.ue_id;

CREATE VIEW public.v_estrutura_abrangencia_dres
AS
	SELECT v_abrangencia_cadeia_turmas.dre_abreviacao AS abreviacao,
		v_abrangencia_cadeia_turmas.dre_codigo AS codigo,
		v_abrangencia_cadeia_turmas.dre_nome AS nome
	FROM v_abrangencia_cadeia_turmas;

CREATE VIEW public.v_estrutura_abrangencia_turmas
AS
	SELECT v_abrangencia_cadeia_turmas.turma_ano AS ano,
		v_abrangencia_cadeia_turmas.turma_ano_letivo AS anoletivo,
		v_abrangencia_cadeia_turmas.turma_id AS codigo,
		v_abrangencia_cadeia_turmas.modalidade_codigo AS codigomodalidade,
		v_abrangencia_cadeia_turmas.turma_nome AS nome,
		v_abrangencia_cadeia_turmas.turma_semestre AS semestre,
		v_abrangencia_cadeia_turmas.qt_duracao_aula AS qtduracaoaula,
		v_abrangencia_cadeia_turmas.tipo_turno AS tipoturno
	FROM v_abrangencia_cadeia_turmas;

CREATE VIEW public.v_estrutura_abrangencia_ues
AS
	SELECT act.ue_codigo AS codigo,
		act.ue_nome AS nome,
		ue.tipo_escola AS tipoescola
	FROM v_abrangencia_cadeia_turmas act
		JOIN ue
		ON act.ue_id = act.ue_id;

CREATE VIEW public.v_abrangencia_sintetica
AS
	SELECT a.id,
		a.usuario_id,
		u.login,
		a.dre_id,
		dre.dre_id AS codigo_dre,
		a.ue_id,
		ue.ue_id AS codigo_ue,
		a.turma_id,
		turma.turma_id AS codigo_turma,
		a.perfil,
		a.historico
	FROM abrangencia a
		JOIN usuario u ON u.id = a.usuario_id
		LEFT JOIN dre dre ON dre.id = a.dre_id
		LEFT JOIN ue ue ON ue.id = a.ue_id
		LEFT JOIN turma turma ON turma.id = a.turma_id;

create function f_abrangencia_ues
(
	varchar
(50),
	uuid,
	bool,
	int4 = 0,
	int4 = 0,
	varchar
(15) = null,
	int4 = 0
)
returns setof v_estrutura_abrangencia_ues as 
'
select distinct act.ue_codigo,
				act.ue_nome,
                ue.tipo_escola
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
                ue.tipo_escola
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
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
                ue.tipo_escola
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
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
                ue.tipo_escola
	from v_abrangencia_nivel_especifico a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id and
			   a.ue_id = act.ue_id and
			   a.turma_id = act.turma_id
		inner join ue
			on act.ue_id = ue.id			   
where a.login = $1 and 
	  a.perfil_id = $2 and
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 is null or ($6 is not null and act.dre_codigo = $6)) and
      ($7 = 0 or ($7 <> 0 and act.turma_ano_letivo = $7));
'
language sql;

create function f_abrangencia_anos_letivos
(
	varchar
(50),
	uuid,
	bool
)
returns setof int as 
'
select distinct act.turma_ano_letivo
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3
	 
union

select distinct act.turma_ano_letivo
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3

union

select distinct act.turma_ano_letivo
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
where a.login = $1 and 
	  a.perfil_id = $2 and
	  act.turma_historica = $3

union

select distinct act.turma_ano_letivo
	from v_abrangencia_nivel_especifico a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id and
			   a.ue_id = act.ue_id and
			   a.turma_id = act.turma_id			   
where a.login = $1 and 
	  a.perfil_id = $2 and
	  act.turma_historica = $3;
'
language sql;

create function f_abrangencia_dres
(
	varchar
(50),
	uuid,
	bool,
	int4 = 0,
	int4 = 0,
	int4 = 0
)
returns setof v_estrutura_abrangencia_dres as 
'
select distinct act.dre_abreviacao,
				act.dre_codigo,
                act.dre_nome
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
                act.dre_nome
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 = 0 or ($6 <> 0 and act.turma_ano_letivo = $6))

union

select distinct act.dre_abreviacao,
				act.dre_codigo,
                act.dre_nome
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 = 0 or ($6 <> 0 and act.turma_ano_letivo = $6))

union

select distinct act.dre_abreviacao,
				act.dre_codigo,
                act.dre_nome
	from v_abrangencia_nivel_especifico a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id and
			   a.ue_id = act.ue_id and
			   a.turma_id = act.turma_id			   
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 = 0 or ($6 <> 0 and act.turma_ano_letivo = $6));
'
language sql;

create function f_abrangencia_modalidades
(
	varchar
(50),
	uuid,
	bool,
	int
)
returns setof int as 
'
select distinct act.modalidade_codigo
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and      
	  act.turma_ano_letivo = $4
	 
union

select distinct act.modalidade_codigo
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
where a.login = $1 and 
	  a.perfil_id = $2 and
	  act.turma_historica = $3 and      
	  act.turma_ano_letivo = $4

union

select distinct act.modalidade_codigo
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
where a.login = $1 and 
	  a.perfil_id = $2 and
	  act.turma_historica = $3 and      
	  act.turma_ano_letivo = $4

union

select distinct act.modalidade_codigo
	from v_abrangencia_nivel_especifico a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id and
			   a.ue_id = act.ue_id and
			   a.turma_id = act.turma_id			   
where a.login = $1 and 
	  a.perfil_id = $2 and
	  act.turma_historica = $3 and      
	  act.turma_ano_letivo = $4;
'
language sql;

create function f_abrangencia_semestres
(
	varchar
(50),
	uuid,
	bool,
	int4 = 0,
	int4 = 0	
)
returns setof int4 as 
'
select distinct act.turma_semestre
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and      
      ($5 = 0 or ($5 <> 0 and act.turma_ano_letivo = $5))
	 
union

select distinct act.turma_semestre
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_ano_letivo = $5))

union

select distinct act.turma_semestre
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_ano_letivo = $5))

union

select distinct act.turma_semestre
	from v_abrangencia_nivel_especifico a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id and
			   a.ue_id = act.ue_id and
			   a.turma_id = act.turma_id			   
where a.login = $1 and 
	  a.perfil_id = $2 and
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_ano_letivo = $5));
'
language sql;

create function f_abrangencia_turmas
(
	varchar
(50),
	uuid,
	bool,
	int4 = 0,
	int4 = 0,
	varchar
(15) = null,
	int4 = 0
)
returns setof v_estrutura_abrangencia_turmas as 
'
select distinct act.turma_ano,
				act.turma_ano_letivo,
				act.turma_id,
				act.modalidade_codigo,
				act.turma_nome,
				act.turma_semestre,
				act.qt_duracao_aula,
				act.tipo_turno
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id				
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
	  ($6 is null or ($6 is not null and act.ue_codigo = $6)) and
      ($7 = 0 or ($7 <> 0 and act.turma_ano_letivo = $7))
	 
union

select distinct act.turma_ano,
				act.turma_ano_letivo,
				act.turma_id,
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
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 is null or ($6 is not null and act.ue_codigo = $6)) and
      ($7 = 0 or ($7 <> 0 and act.turma_ano_letivo = $7))

union

select distinct act.turma_ano,
				act.turma_ano_letivo,
				act.turma_id,
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
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 is null or ($6 is not null and act.ue_codigo = $6)) and
      ($7 = 0 or ($7 <> 0 and act.turma_ano_letivo = $7))

union

select distinct act.turma_ano,
				act.turma_ano_letivo,
				act.turma_id,
				act.modalidade_codigo,
				act.turma_nome,
				act.turma_semestre,
				act.qt_duracao_aula,
				act.tipo_turno
	from v_abrangencia_nivel_especifico a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id and
			   a.ue_id = act.ue_id and
			   a.turma_id = act.turma_id
		inner join ue
			on act.ue_id = ue.id			   
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 is null or ($6 is not null and act.ue_codigo = $6)) and
      ($7 = 0 or ($7 <> 0 and act.turma_ano_letivo = $7));
'
language sql;