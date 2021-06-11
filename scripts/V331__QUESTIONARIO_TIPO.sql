alter table questionario drop column if exists tipo;
alter table questionario add tipo Int4 not null default 1;
