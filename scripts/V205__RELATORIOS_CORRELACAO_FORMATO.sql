alter table relatorio_correlacao add column tipo_formato int null;

update relatorio_correlacao set tipo_formato = 1;

alter table relatorio_correlacao alter column tipo_formato set not null;

