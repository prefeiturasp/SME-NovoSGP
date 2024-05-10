
-- ### INCLUI TURMAS ###
insert into turma (turma_id, data_atualizacao)
select distinct turma_id, now()
from atribuicao_cj as acj
where acj.turma_id not in (select turma_id from turma)

-- ### INSERE USUÁRIOS INEXISTENTES NA TABELA DE USUÁRIOS ###
insert into usuario (rf_codigo, criado_em, criado_por, criado_rf)
select distinct acj.professor_rf, now(), 'Migração/ETL', '0'
from atribuicao_cj as acj
where migrado = true
and rtrim(acj.professor_rf) not in (select rtrim(rf_codigo) from usuario where rf_codigo is not null)

-- ### INCLUI NA ABRANGENCIA ###
insert into abrangencia (usuario_id, turma_id, perfil, historico)
select distinct usu.id,
tur.id,
cast('41E1E074-37D6-E911-ABD6-F81654FE895D' as uuid),
true
from atribuicao_cj as acj
inner join usuario as usu on usu.rf_codigo = acj.professor_rf --and usu.criado_por = 'Migração/ETL'
inner join turma as tur on tur.turma_id = acj.turma_id
where acj.professor_rf not in (select cast(usuario_id as varchar) from abrangencia where cast(turma_id as varchar) = ACJ.turma_id)

