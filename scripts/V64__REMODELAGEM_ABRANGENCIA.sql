/*
drop view public.abrangencia_cadeia_dres;
drop view public.abrangencia_cadeia_ues;
drop view public.abrangencia_cadeia_turmas;

drop table turma;
drop table ue;
drop table dre;
drop table abrangencia;
*/
begin transaction;
-- DREs
create table public.dre as 
SELECT id, dre_id, abreviacao, nome
FROM public.abrangencia_dres where 1=0;

ALTER TABLE public.dre ADD CONSTRAINT dre_pk PRIMARY KEY (id);

CREATE INDEX dre_dre_id_idx ON public.dre USING btree (dre_id);

ALTER TABLE public.dre 
    ALTER id SET NOT NULL,
    ALTER id ADD GENERATED ALWAYS AS IDENTITY (START WITH 1);
   
-- UEs
create table public.ue as
SELECT id, ue_id, abrangencia_dres_id as dre_id, nome, tipo_escola
FROM public.abrangencia_ues  where 1=0;

ALTER TABLE public.ue ADD CONSTRAINT ue_pk PRIMARY KEY (id);

ALTER TABLE public.ue ADD CONSTRAINT ue_dre_id_fk FOREIGN KEY (dre_id) REFERENCES dre(id) ON DELETE CASCADE;

CREATE INDEX ue_dre_idx ON public.ue USING btree (dre_id);
CREATE INDEX ue_ue_idx ON public.ue USING btree (ue_id);

ALTER TABLE public.ue 
    ALTER id SET NOT NULL,
    ALTER id ADD GENERATED ALWAYS AS IDENTITY (START WITH 1);

-- Turmas
create table turma as
SELECT id, turma_id, abrangencia_ues_id as ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno
FROM public.abrangencia_turmas where 1=0;

ALTER TABLE public.turma ADD CONSTRAINT turma_pk PRIMARY KEY (id);

ALTER TABLE public.turma ADD CONSTRAINT turma_ue_id_fk FOREIGN KEY (ue_id) REFERENCES ue(id) ON DELETE CASCADE;

CREATE INDEX turma_ue_idx ON public.turma USING btree (ue_id);
CREATE INDEX turma_modalidade_idx ON public.turma USING btree (modalidade_codigo);
CREATE INDEX turma_turma_id_idx ON public.turma USING btree (turma_id);

ALTER TABLE public.turma 
    ALTER id SET NOT NULL,
    ALTER id ADD GENERATED ALWAYS AS IDENTITY (START WITH 1);


-- abrangencia

CREATE TABLE public.abrangencia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	usuario_id int8 NOT NULL,
	dre_id int8 NULL,
	ue_id int8 NULL,
	turma_id int8 NULL,
	perfil uuid NOT NULL,
	CONSTRAINT abrangencia_pk PRIMARY KEY (id)
);

CREATE INDEX abrangencia_dre_id_idx ON public.abrangencia USING btree (dre_id);
CREATE INDEX abrangencia_usuario_idx ON public.abrangencia USING btree (usuario_id);

ALTER TABLE public.abrangencia ADD CONSTRAINT abrangencia_dre_usario_fk FOREIGN KEY (usuario_id) REFERENCES usuario(id);

-- views de apoio


create or replace view public.v_abrangencia_cadeia_turmas
as SELECT 
	ab_dres.id dre_id,
	ab_dres.dre_id AS dre_codigo,
    ab_dres.abreviacao AS dre_abreviacao,
    ab_dres.nome AS dre_nome,
    ab_ues.id ue_id,
    ab_ues.ue_id AS ue_codigo,
    ab_ues.nome AS ue_nome,
    ab_turma.id turma_id,
    ab_turma.ano AS turma_ano,
    ab_turma.ano_letivo AS turma_ano_letivo,
    ab_turma.modalidade_codigo,
    ab_turma.nome AS turma_nome,
    ab_turma.semestre AS turma_semestre,
    ab_turma.qt_duracao_aula,
    ab_turma.tipo_turno,
    ab_turma.turma_id turma_codigo
   FROM dre ab_dres
     JOIN ue ab_ues ON ab_ues.dre_id = ab_dres.id
     JOIN turma ab_turma ON ab_turma.ue_id = ab_ues.id;

create or replace view public.v_abrangencia_cadeia_ues
as SELECT * from public.v_abrangencia_cadeia_turmas
     union all 
SELECT 
	ab_dres.id dre_id,
	ab_dres.dre_id AS dre_codigo,
    ab_dres.abreviacao AS dre_abreviacao,
    ab_dres.nome AS dre_nome,
    ab_ues.id ue_id,
    ab_ues.ue_id AS ue_codigo,
    ab_ues.nome AS ue_nome,
    null as turma_id,
    null as turma_ano,
    null as turma_ano_letivo,
    null as modalidade_codigo,
    null as turma_nome,
    null as turma_semestre,
    null as qt_duracao_aula,
    null as tipo_turno,
    null as turma_codigo
   FROM dre ab_dres
     JOIN ue ab_ues ON ab_ues.dre_id = ab_dres.id    ;
    
create or replace view public.v_abrangencia_cadeia_dres
as SELECT * from public.v_abrangencia_cadeia_turmas
     union all 
SELECT 
	ab_dres.id dre_id,
	ab_dres.dre_id AS dre_codigo,
    ab_dres.abreviacao AS dre_abreviacao,
    ab_dres.nome AS dre_nome,
    null as ue_id,
    null as ue_codigo,
    null as ue_nome,
    null as turma_id,
    null as turma_ano,
    null as turma_ano_letivo,
    null as modalidade_codigo,
    null as turma_nome,
    null as turma_semestre,
    null as qt_duracao_aula,
    null as tipo_turno,
    null as turma_codigo
   FROM dre ab_dres;  
end transaction;



