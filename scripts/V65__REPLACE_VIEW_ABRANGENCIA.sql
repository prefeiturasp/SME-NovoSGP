begin transaction;
drop view public.v_abrangencia;
create or replace view  public.v_abrangencia as
select
	coalesce(turma.dre_codigo, ue.dre_codigo, dre.dre_codigo) AS dre_codigo,
    coalesce(turma.dre_abreviacao, ue.dre_abreviacao, dre.dre_abreviacao) AS dre_abreviacao,
    coalesce(turma.dre_nome, ue.dre_nome, dre.dre_nome) AS dre_nome,
    a.usuario_id,
    a.perfil AS usuario_perfil,
    coalesce(turma.ue_codigo, ue.ue_codigo, dre.ue_codigo) AS ue_codigo,
    coalesce(turma.ue_nome, ue.ue_nome, dre.ue_nome) AS ue_nome,
    coalesce(turma.turma_ano, ue.turma_ano, dre.turma_ano) AS turma_ano,
    coalesce(turma.turma_ano_letivo, ue.turma_ano_letivo, dre.turma_ano_letivo) AS turma_ano_letivo,
    coalesce(turma.modalidade_codigo, ue.modalidade_codigo, dre.modalidade_codigo) AS modalidade_codigo,
    coalesce(turma.turma_nome, ue.turma_nome, dre.turma_nome) AS turma_nome,
    coalesce(turma.turma_semestre, ue.turma_semestre, dre.turma_semestre) AS turma_semestre,
    coalesce(turma.qt_duracao_aula, ue.qt_duracao_aula, dre.qt_duracao_aula) AS qt_duracao_aula,
    coalesce(turma.tipo_turno, ue.tipo_turno, dre.tipo_turno) AS tipo_turno,
    coalesce(turma.turma_codigo, ue.turma_codigo, dre.turma_codigo) AS turma_id
from
	abrangencia a
left join public.v_abrangencia_cadeia_dres dre on
	dre.dre_id = a.dre_id
left join public.v_abrangencia_cadeia_ues ue on
	ue.ue_id = a.ue_id
left join public.v_abrangencia_cadeia_turmas turma on
	turma.turma_id = a.turma_id;
create or replace view public.v_abrangencia_magra
as
select
	a.id,
	a.usuario_id,
	u.login,
	a.dre_id,
	dre.dre_id as codigo_dre,
	a.ue_id,
	ue.ue_id codigo_ue,
	a.turma_id,
	turma.turma_id codigo_turma,
	a.perfil
from
	abrangencia a
inner join usuario u on
	u.id = a.usuario_id
inner join dre dre on
	dre.id = a.dre_id
inner join ue ue on
	ue.id = a.ue_id
inner join turma turma on
	turma.id = a.turma_id;
CREATE OR REPLACE VIEW public.v_abrangencia_sintetica
AS SELECT a.id,
    a.usuario_id,
    u.login,
    a.dre_id,
    dre.dre_id AS codigo_dre,
    a.ue_id,
    ue.ue_id AS codigo_ue,
    a.turma_id,
    turma.turma_id AS codigo_turma,
    a.perfil
   FROM abrangencia a
     JOIN usuario u ON u.id = a.usuario_id
     left JOIN dre dre ON dre.id = a.dre_id
     left JOIN ue ue ON ue.id = a.ue_id
     left JOIN turma turma ON turma.id = a.turma_id;
end transaction;