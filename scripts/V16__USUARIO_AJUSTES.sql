ALTER TABLE PUBLIC.usuario add column login varchar(50);
ALTER TABLE PUBLIC.usuario add column ultimo_login timestamp without time zone;

ALTER TABLE public.usuario alter column rf_codigo drop not null;

CREATE INDEX usuario_login_idx ON public.usuario (login);