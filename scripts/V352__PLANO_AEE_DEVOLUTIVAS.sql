ALTER TABLE  public.plano_aee 
add column if not exists parecer_coordenacao varchar null,
add column if not exists parecer_paai varchar NULL,
add column if not exists responsavel_id int8 null;

ALTER TABLE public.plano_aee ADD CONSTRAINT plano_aee_usuario_fk FOREIGN KEY (responsavel_id) REFERENCES usuario(id);
CREATE INDEX plano_aee_usuario_idx ON public.plano_aee USING btree (responsavel_id);
