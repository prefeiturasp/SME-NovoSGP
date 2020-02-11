alter table public.tipo_avaliacao add if not exists codigo int4 null;
CREATE INDEX if not exists evento_tipo_excluido_idx ON public.tipo_avaliacao USING btree (codigo);

update tipo_avaliacao set codigo = 1 where nome = 'Avaliação bimestral';
update tipo_avaliacao set codigo = 2 where nome = 'Avaliação mensal';
update tipo_avaliacao set codigo = 3 where nome = 'Chamada oral';
update tipo_avaliacao set codigo = 4 where nome = 'Debate';
update tipo_avaliacao set codigo = 5 where nome = 'Dramatização';
update tipo_avaliacao set codigo = 6 where nome = 'Estudo de meio';
update tipo_avaliacao set codigo = 7 where nome = 'Pesquisa';
update tipo_avaliacao set codigo = 8 where nome = 'Produção de texto';
update tipo_avaliacao set codigo = 9 where nome = 'Projeto escolar';
update tipo_avaliacao set codigo = 10 where nome = 'Seminário';
update tipo_avaliacao set codigo = 11 where nome = 'TCA';
update tipo_avaliacao set codigo = 12 where nome = 'Teste';
update tipo_avaliacao set codigo = 13 where nome = 'Teste de múltipla escolha';
update tipo_avaliacao set codigo = 14 where nome = 'Texto';
update tipo_avaliacao set codigo = 15 where nome = 'Trabalho individual';
update tipo_avaliacao set codigo = 16 where nome = 'Trabalho em grupo';
