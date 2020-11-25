insert into componente_curricular (id,componente_curricular_pai_id,grupo_matriz_id,area_conhecimento_id,descricao,eh_regencia,eh_compartilhada,
eh_territorio,eh_base_nacional,permite_registro_frequencia,permite_lancamento_nota,descricao_sgp)
select 1107,null,null,null,'DOCENCIA COMPARTILHADA 4H',false,false,false,false,false,false,'DOCENCIA COMPARTILHADA 4H'
where not exists (select 1 from componente_curricular where id = 1107);

insert into componente_curricular (id,componente_curricular_pai_id,grupo_matriz_id,area_conhecimento_id,descricao,eh_regencia,eh_compartilhada,
eh_territorio,eh_base_nacional,permite_registro_frequencia,permite_lancamento_nota,descricao_sgp)
select 1108,0,null,null,'DOCENCIA COMPARTILHADA 8H',false,false,false,false,false,false,'DOCENCIA COMPARTILHADA 8H'
where not exists (select 1 from componente_curricular where id = 1108);

insert into componente_curricular (id,componente_curricular_pai_id,grupo_matriz_id,area_conhecimento_id,descricao,eh_regencia,eh_compartilhada,
eh_territorio,eh_base_nacional,permite_registro_frequencia,permite_lancamento_nota,descricao_sgp)
select 1109,0,null,null,'DOCENCIA COMPARTILHADA 12H',false,false,false,false,false,false,'DOCENCIA COMPARTILHADA 12H'
where not exists (select 1 from componente_curricular where id = 1109);

insert into componente_curricular (id,componente_curricular_pai_id,grupo_matriz_id,area_conhecimento_id,descricao,eh_regencia,eh_compartilhada,
eh_territorio,eh_base_nacional,permite_registro_frequencia,permite_lancamento_nota,descricao_sgp)
select 1110,0,null,null,'DOCENCIA COMPARTILHADA 6H',false,false,false,false,false,false,'DOCENCIA COMPARTILHADA 6H'
where not exists (select 1 from componente_curricular where id = 1110);

insert into componente_curricular (id,componente_curricular_pai_id,grupo_matriz_id,area_conhecimento_id,descricao,eh_regencia,eh_compartilhada,
eh_territorio,eh_base_nacional,permite_registro_frequencia,permite_lancamento_nota,descricao_sgp)
select 1118,0,null,null,'DOCENCIA COMPARTILHADA 11 AULAS',false,false,false,false,false,false,'DOCENCIA COMPARTILHADA 11 AULAS'
where not exists (select 1 from componente_curricular where id = 1118);

insert into componente_curricular (id,componente_curricular_pai_id,grupo_matriz_id,area_conhecimento_id,descricao,eh_regencia,eh_compartilhada,
eh_territorio,eh_base_nacional,permite_registro_frequencia,permite_lancamento_nota,descricao_sgp)
select 1119,0,null,null,'DOCENCIA COMPARTILHADA 7 AULAS',false,false,false,false,false,false,'DOCENCIA COMPARTILHADA 7 AULAS'
where not exists (select 1 from componente_curricular where id = 1119);

insert into componente_curricular (id,componente_curricular_pai_id,grupo_matriz_id,area_conhecimento_id,descricao,eh_regencia,eh_compartilhada,
eh_territorio,eh_base_nacional,permite_registro_frequencia,permite_lancamento_nota,descricao_sgp)
select 1120,0,null,null,'DOCENCIA COMPARTILHADA 5 AULAS',false,false,false,false,false,false,'DOCENCIA COMPARTILHADA 5 AULAS'
where not exists (select 1 from componente_curricular where id = 1120);


update componente_curricular set permite_lancamento_nota = false, permite_registro_frequencia = false where id in (1150,1151);

update componente_curricular set permite_lancamento_nota = false where id = 1111;