ALTER TABLE PUBLIC.usuario add column login varchar(20);

ALTER TABLE public.usuario alter column rf_codigo drop not null;

CREATE INDEX usuario_login_idx ON public.usuario (login);