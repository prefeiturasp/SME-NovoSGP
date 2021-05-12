-- itinerancia_objetivo_base
alter table public.itinerancia_objetivo_base DROP COLUMN IF EXISTS permite_varias_ues;

-- itinerancia_ue
drop table if exists public.itinerancia_ue;

-- itinerancia - ue
alter table public.itinerancia add column if not exists ue_id int8;
ALTER TABLE public.itinerancia ADD CONSTRAINT itinerancia_ue_fk FOREIGN KEY (ue_id) REFERENCES ue(id);
CREATE index if not exists itinerancia_ue_id_idx ON public.itinerancia USING btree (ue_id);

-- itinerancia - dre
alter table public.itinerancia add column if not exists dre_id int8;
ALTER TABLE public.itinerancia ADD CONSTRAINT itinerancia_dre_fk FOREIGN KEY (dre_id) REFERENCES dre(id);
CREATE index if not exists itinerancia_dre_id_idx ON public.itinerancia USING btree (dre_id);
