
alter table public.auditoria add column
if not exists perfil uuid null;

CREATE INDEX auditoria_rf_idx ON public.auditoria USING btree
(rf);
CREATE INDEX auditoria_perfil_idx ON public.auditoria USING btree
(perfil); 