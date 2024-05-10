alter table encaminhamento_naapa drop if exists motivo_encerramento;

alter table encaminhamento_naapa add motivo_encerramento varchar(500);