alter table evento rename column dre_id to dre_codigo;
alter table evento rename column ue_id to ue_codigo;

alter table evento add dre_id bigint null;
alter table evento add ue_id bigint null;

ALTER TABLE evento ADD CONSTRAINT evento_dre_fk FOREIGN KEY (dre_id) REFERENCES dre(id);
ALTER TABLE evento ADD CONSTRAINT evento_ue_fk FOREIGN KEY (ue_id) REFERENCES ue(id);

CREATE INDEX evento_dre_fkx ON public.evento USING btree (dre_id);
CREATE INDEX evento_ue_fkx ON public.evento USING btree (ue_id);

update evento e
 	set dre_id = dre.id
 from dre  
 where dre.dre_id = e.dre_codigo
   and e.dre_id is null 
   and e.dre_codigo is not null;

update evento e
 	set ue_id = ue.id
 from ue
 where ue.ue_id = e.ue_codigo
   and e.ue_id is null 
   and e.ue_codigo is not null;
