alter table itinerancia drop column if exists evento_id;
alter table itinerancia 
  add evento_id int8 null;

ALTER TABLE public.itinerancia ADD CONSTRAINT itinerancia_evento_fk FOREIGN KEY (evento_id) REFERENCES evento(id);
CREATE INDEX itinerancia_evento_idx ON public.itinerancia USING btree (evento_id);
