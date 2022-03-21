drop table tmp_turma_alunos_quantidade;
create table tmp_turma_alunos_quantidade (
	turma_id varchar not null,
	quantidade int not null default 0
);
CREATE INDEX tmp_turma_alunos_quantidade_turma_idx ON public.tmp_turma_alunos_quantidade USING btree (turma_id);

insert into tmp_turma_alunos_quantidade (turma_id, quantidade) 
select turmacodigo, count(*) 
  from tmp_aluno_turma
 group by turmacodigo;


create table tmp_turmas_2019 (
	dre_id varchar,
	abreviacao varchar,
	ue_id varchar,
	ue_nome varchar,
	turma_id varchar,
	turma_nome varchar,
	turma_ano varchar
);
CREATE INDEX tmp_turmas_2019_turma_idx ON public.tmp_turmas_2019 USING btree (turma_id);
CREATE INDEX tmp_turmas_2019_ue_idx ON public.tmp_turmas_2019 USING btree (ue_id);
CREATE INDEX tmp_turmas_2019_dre_idx ON public.tmp_turmas_2019 USING btree (dre_id);

insert into tmp_turmas_2019 (dre_id, abreviacao, ue_id, ue_nome, turma_id, turma_nome, turma_ano) 
select dre.dre_id,dre.abreviacao, ue.ue_id, ue.nome
    , t.turma_id, t.nome, t.ano
  from turma t
 inner join ue on ue.id = t.ue_id
 inner join dre on dre.id = ue.dre_id
 where t.ano_letivo = 2019
   and t.modalidade_codigo in (1,3,5,6)
   and t.ano in ('1', '2', '3', '4', '5', '6', '7', '8', '9');



select t.dre_id, t.abreviacao, t.ue_id, t.ue_nome
    , t.turma_id, t.turma_nome, t.turma_ano
    , a.data_aula
    , al.quantidade as alunos
    , al.quantidade - count(distinct raa.codigo_aluno) as presencas
    , count(distinct raa.codigo_aluno) as ausencias
  from tmp_turmas_2019 t
 inner join tmp_turma_alunos_quantidade al on al.turma_id = t.turma_id
 inner join aula a on a.turma_id = t.turma_id
 inner join registro_frequencia rf on rf.aula_id = a.id
  left join registro_ausencia_aluno raa on raa.registro_frequencia_id = rf.id and not raa.excluido
 where not a.excluido
   and not rf.excluido
   

   and t.dre_id = '109300'
group by t.dre_id, t.abreviacao, t.ue_id, t.ue_nome
    , t.turma_id, t.turma_nome, t.turma_ano
    , a.data_aula, al.quantidade
order by t.dre_id, t.abreviacao, t.ue_id, t.ue_nome
    , t.turma_id, t.turma_nome, t.turma_ano
    , a.data_aula, al.quantidade
    
