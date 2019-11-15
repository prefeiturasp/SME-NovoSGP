alter table if exists public.abrangencia_ues add column if not exists tipo_escola int2 null;
alter table if exists public.abrangencia_turmas add column if not exists qt_duracao_aula int2 null;
alter table if exists public.abrangencia_turmas add column if not exists tipo_turno int2 null;

drop view v_abrangencia;
CREATE VIEW  v_abrangencia 
as
select ab_dres.dre_id as dre_codigo
	, ab_dres.abreviacao as dre_abreviacao
	, ab_dres.nome as dre_nome
	, ab_ues.ue_id as ue_codigo
	, ab_ues.nome as ue_nome
	, ab_turmas.ano as turma_ano
	, ab_turmas.ano_letivo as turma_ano_letivo
	, ab_turmas.modalidade_codigo as modalidade_codigo
	, ab_turmas.nome as turma_nome
	, ab_turmas.semestre as turma_semestre
	, ab_turmas.qt_duracao_aula as qt_duracao_aula
	, ab_turmas.tipo_turno as tipo_turno
	, ab_dres.usuario_id as usuario_id
	, ab_dres.perfil as usuario_perfil
 from abrangencia_dres as ab_dres
inner join abrangencia_ues ab_ues on ab_ues.abrangencia_dres_id = ab_dres.id
inner join abrangencia_turmas ab_turmas on ab_turmas.abrangencia_ues_id = ab_ues.id	;
