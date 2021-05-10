-- itinerancia_objetivo_base
alter table public.itinerancia_objetivo_base DROP COLUMN IF EXISTS permite_varias_ues;

-- itinerancia_ue
drop table if exists public.itinerancia_ue;

-- itinerancia
alter table public.itinerancia add column if not exists ue_id int8 NOT null;
ALTER TABLE public.itinerancia ADD CONSTRAINT itinerancia_ue_fk FOREIGN KEY (ue_id) REFERENCES ue(id);
