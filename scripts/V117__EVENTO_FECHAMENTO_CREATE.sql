alter table public.evento_tipo add column if not exists somente_leitura boolean not null default false;

update public.evento_tipo 
  set somente_leitura = true 
 where descricao = 'Fechamento de Bimestre';

drop table if exists public.evento_fechamento;
CREATE TABLE public.evento_fechamento
(
    id bigint NOT NULL generated always as identity,
	evento_id bigint not null,
	fechamento_id bigint not null,
    excluido BOOLEAN NOT NULL,
    criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT evento_fechamento_pk PRIMARY KEY (id)
);
ALTER TABLE public.evento_fechamento ADD CONSTRAINT evento_fechamento_evento_fk FOREIGN KEY (evento_id) REFERENCES evento(id);
ALTER TABLE public.evento_fechamento ADD CONSTRAINT evento_fechamento_fechamento_fk FOREIGN KEY (fechamento_id) REFERENCES fechamento(id);

