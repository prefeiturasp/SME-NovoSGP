--Roubo
insert into public.ocorrencia_tipo 
	(alterado_em, alterado_por, alterado_rf, criado_em , criado_por , criado_rf , descricao , excluido )
select
    null, null, null, CURRENT_TIMESTAMP, 'Carga inicial', 'Carga inicial', 'Roubo', false
where
    not exists(select 1 from public.ocorrencia_tipo where descricao = 'Roubo');

--Furto
insert into public.ocorrencia_tipo 
	(alterado_em, alterado_por, alterado_rf, criado_em , criado_por , criado_rf , descricao , excluido )
select
    null, null, null, CURRENT_TIMESTAMP, 'Carga inicial', 'Carga inicial', 'Furto', false
where
    not exists(select 1 from public.ocorrencia_tipo where descricao = 'Furto');
			   
--Violência contra os professores
insert into public.ocorrencia_tipo 
	(alterado_em, alterado_por, alterado_rf, criado_em , criado_por , criado_rf , descricao , excluido )
select
    null, null, null, CURRENT_TIMESTAMP, 'Carga inicial', 'Carga inicial', 'Violência contra os professores', false
where
    not exists(select 1 from public.ocorrencia_tipo where descricao = 'Violência contra os professores');
			   
--Violência contra os funcionários
insert into public.ocorrencia_tipo 
	(alterado_em, alterado_por, alterado_rf, criado_em , criado_por , criado_rf , descricao , excluido )
select
    null, null, null, CURRENT_TIMESTAMP, 'Carga inicial', 'Carga inicial', 'Violência contra os funcionários', false
where
    not exists(select 1 from public.ocorrencia_tipo where descricao = 'Violência contra os funcionários');
			   
--Violência contra a criança/estudante
insert into public.ocorrencia_tipo 
	(alterado_em, alterado_por, alterado_rf, criado_em , criado_por , criado_rf , descricao , excluido )
select
    null, null, null, CURRENT_TIMESTAMP, 'Carga inicial', 'Carga inicial', 'Violência contra a criança/estudante', false
where
    not exists(select 1 from public.ocorrencia_tipo where descricao = 'Violência contra a criança/estudante');