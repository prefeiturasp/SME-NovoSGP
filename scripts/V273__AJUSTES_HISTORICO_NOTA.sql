alter table historico_nota drop column nota_anterior;
alter table historico_nota drop column nota_nova;
alter table historico_nota drop column if exists conceito_anterior_id;
alter table historico_nota drop column if exists conceito_novo_id;

alter table historico_nota add column nota_anterior numeric(5,2) null;
alter table historico_nota add column nota_nova numeric(5,2) null;

alter table historico_nota add column conceito_anterior_id int8 null;
alter table historico_nota add column conceito_novo_id int8 null;

select
	f_cria_fk_se_nao_existir(
		'historico_nota',
		'historico_nota_conceito_anterior_fk',
		'FOREIGN KEY (conceito_anterior_id) REFERENCES conceito_valores (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'historico_nota',
		'historico_nota_conceito_novo_fk',
		'FOREIGN KEY (conceito_novo_id) REFERENCES conceito_valores (id)'
	);

CREATE INDEX historico_nota_conceito_anterior_idx ON public.historico_nota USING btree (conceito_anterior_id);
CREATE INDEX historico_nota_conceito_novo_idx ON public.historico_nota USING btree (conceito_novo_id);
