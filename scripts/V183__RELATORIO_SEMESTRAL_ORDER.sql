alter table secao_relatorio_semestral add column if not exists ordem int4 not null default 0;
update secao_relatorio_semestral set ordem = 10 where id = 1;
update secao_relatorio_semestral set ordem = 20 where id = 2;
update secao_relatorio_semestral set ordem = 30 where id = 3;
update secao_relatorio_semestral set ordem = 40 where id = 4;
update secao_relatorio_semestral set ordem = 50 where id = 5;
