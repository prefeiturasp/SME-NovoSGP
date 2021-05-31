create table if not exists public.migracao_frequencia ( 
codigo_turma varchar(25) not null,
codigo_aluno varchar(25) not null,
codigo_situacao varchar(25) not null,
data_situacao timestamp not null);