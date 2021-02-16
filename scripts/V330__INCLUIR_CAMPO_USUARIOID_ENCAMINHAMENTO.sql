alter table encaminhamento_aee drop if exists responsavel_id;

alter table encaminhamento_aee add column responsavel_id int8 null; 

select
	f_cria_fk_se_nao_existir(
		'encaminhamento_aee',
		'encaminhamento_aee_usuario_fk',
		'FOREIGN KEY (responsavel_id) REFERENCES usuario (id)'
	);

CREATE INDEX encaminhamento_aee_usuario_idx ON public.encaminhamento_aee USING btree (responsavel_id);