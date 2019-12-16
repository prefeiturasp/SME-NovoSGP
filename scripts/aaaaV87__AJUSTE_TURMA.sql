create temp table tmp_turmas(id int8);

insert into tmp_turmas 
select a.id	 from (SELECT 
        id, turma_id,
        ROW_NUMBER() OVER (
            PARTITION BY 
                turma_id
            ORDER BY 
                id
        ) as  row_num
     FROM 
        turma  ) a
        where a.row_num > 1;
       

delete from turma where id in (select * from tmp_turmas);
        
drop table tmp_turmas;
       
    

CREATE INDEX if not exists notificacao_frequencia_tipo_idx ON public.notificacao_frequencia (tipo);
CREATE INDEX if not exists aula_data_aula_idx ON public.aula (data_aula);


select f_cria_fk_se_nao_existir('aula', 'aula_turma_fk', 'FOREIGN KEY (turma_id) REFERENCES turma (turma_id)');