alter table diario_bordo drop column if exists turma_id;
alter table diario_bordo add turma_id int8 null;
alter table diario_bordo ADD CONSTRAINT diario_bordo_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);
create index diario_bordo_turma_idx ON public.diario_bordo USING btree (turma_id);



update diario_bordo db
set turma_id = t.id
from aula a
inner join turma t on t.turma_id = a.turma_id
where a.id = db.aula_id;