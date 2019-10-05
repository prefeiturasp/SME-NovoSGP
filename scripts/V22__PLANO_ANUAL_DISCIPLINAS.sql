alter table if exists public.plano_anual add column if not exists componente_curricular_eol_id bigint not null;

alter table if exists public.plano_anual add constraint componente_curricular_rol_id_fk foreign key (componente_curricular_eol_id) references componente_curricular(codigo_eol);