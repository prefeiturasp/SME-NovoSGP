create table monitoramento.fechamento_turma_duplicado(
	turma_id int8 not null,
	periodo_escolar_id int8 null,
	quantidade int not null,
	primeiro_registro timestamp not null,
	ultimo_registro timestamp not null
);

create table monitoramento.fechamento_turma_disciplina_duplicado (
	fechamento_turma_id int8 not null,
	disciplina_id int8 not null,
	quantidade int not null,
	primeiro_registro timestamp not null,
	ultimo_registro timestamp not null
);

create table monitoramento.fechamento_aluno_duplicado (
	fechamento_turma_disciplina_id int8 not null,
	aluno_codigo int8 not null,
	quantidade int not null,
	primeiro_registro timestamp not null,
	ultimo_registro timestamp not null
);


create table monitoramento.conselho_classe_duplicado (
	fechamento_turma_id int8 not null,
	quantidade int not null,
	primeiro_registro timestamp not null,
	ultimo_registro timestamp not null
);


create table monitoramento.conselho_classe_aluno_duplicado (
	conselho_classe_id int8 not null,
	quantidade int not null,
	primeiro_registro timestamp not null,
	ultimo_registro timestamp not null
);


/****************************/
/* Scripts de Monitoramento */
/****************************/

-- fechamento turma duplicado
delete from monitoramento.fechamento_turma_duplicado;
insert into monitoramento.fechamento_turma_duplicado (
select ft.turma_id
	, ft.periodo_escolar_id
	, count(ft.id) as quantidade
	, min(ft.criado_em) as primeiro_registro
	, max(ft.criado_em) as ultimo_registro
  from turma t
 inner join fechamento_turma ft on ft.turma_id = t.id
 where not ft.excluido 
   and t.ano_letivo = extract(year from NOW())
group by ft.turma_id, ft.periodo_escolar_id
having count(ft.id) > 1
);

-- Fechamento Turma Disciplina Duplicado
delete from monitoramento.fechamento_turma_disciplina_duplicado;
insert into monitoramento.fechamento_turma_disciplina_duplicado (
select ftd.fechamento_turma_id
	, ftd.disciplina_id
	, count(ftd.id) as quantidade
	, min(ftd.criado_em) as primeiro_registro
	, max(ftd.criado_em) as ultimo_registro
  from turma t
 inner join fechamento_turma ft on ft.turma_id = t.id and not ft.excluido
 inner join fechamento_turma_disciplina ftd on ftd.fechamento_turma_id = ft.id and not ftd.excluido 
 where t.ano_letivo = extract(year from NOW())
group by ftd.fechamento_turma_id
	, ftd.disciplina_id
having count(ftd.id) > 1
);


-- Fechamento Aluno
delete from monitoramento.fechamento_aluno_duplicado;
insert into monitoramento.fechamento_aluno_duplicado (
select fa.fechamento_turma_disciplina_id
	, fa.aluno_codigo 
	, count(fa.id) as quantidade
	, min(fa.criado_em) as primeiro_registro
	, max(fa.criado_em) as ultimo_registro
  from fechamento_aluno fa  
 where fa.criado_em >= '2022-01-01'
group by fa.fechamento_turma_disciplina_id
	, fa.aluno_codigo
having count(fa.id) > 1
);

-- Conselho Classe
delete from monitoramento.conselho_classe_duplicado;
insert into monitoramento.conselho_classe_duplicado (
select cc.fechamento_turma_id
	, count(cc.id) as quantidade
	, min(cc.criado_em) as primeiro_registro
	, max(cc.criado_em) as ultimo_registro
  from turma t
 inner join fechamento_turma ft on ft.turma_id = t.id and not ft.excluido
 inner join conselho_classe cc on cc.fechamento_turma_id = ft.id and not cc.excluido 
 where t.ano_letivo = extract(year from NOW())
group by cc.fechamento_turma_id
having count(cc.id) > 1
);

-- Conselho Classe Aluno
delete from monitoramento.conselho_classe_aluno_duplicado;
insert into monitoramento.conselho_classe_aluno_duplicado (
select cca.conselho_classe_id
	, count(cca.id) as quantidade
	, min(cca.criado_em) as primeiro_registro
	, max(cca.criado_em) as ultimo_registro
  from turma t
 inner join fechamento_turma ft on ft.turma_id = t.id and not ft.excluido
 inner join conselho_classe cc on cc.fechamento_turma_id = ft.id and not cc.excluido 
 inner join conselho_classe_aluno cca on cca.conselho_classe_id = cc.id and not cca.excluido 
 where t.ano_letivo = extract(year from NOW())
group by cca.conselho_classe_id 
having count(cca.id) > 1
);


select count(*) from monitoramento.fechamento_turma_duplicado
select count(*) from monitoramento.fechamento_turma_disciplina_duplicado
select count(*) from monitoramento.fechamento_aluno_duplicado
select count(*) from monitoramento.conselho_classe_duplicado
select count(*) from monitoramento.conselho_classe_aluno_duplicado
