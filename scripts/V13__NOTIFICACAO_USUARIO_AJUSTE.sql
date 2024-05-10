delete from public.notificacao where usuario_id is null;

ALTER TABLE public.notificacao alter column usuario_id set not null;

ALTER TABLE public.notificacao ADD CONSTRAINT notificacao_usuario_fk FOREIGN KEY (usuario_id) REFERENCES usuario(id);