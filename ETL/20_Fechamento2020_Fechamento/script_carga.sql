DO $$
DECLARE
   fechamentoTurmaId bigint;
   fechamentoDisciplinaTurmaId bigint;
   fechamentoAlunoId bigint;
   turma RECORD;
   componenteCurricular RECORD;
   componenteCurricularRegencia RECORD;
   aluno RECORD;
BEGIN
    FOR turma IN select turmaid, turmacodigo, tiponota from tmp_fechamento_turmas where turmaid::bigint not in (select distinct ft.turma_id from fechamento_turma ft inner join turma t on 
       		ft.turma_id = t.id where t.ano_letivo = 2020 and periodo_escolar_id is null and EXTRACT(year from criado_em) = 2020) limit 1000 
		loop
        insert into fechamento_turma (turma_id, periodo_escolar_id, criado_em, criado_por, criado_rf) 
       		values(turma.turmaid::bigint, null, now(), 'SISTEMA', '0' ) returning id into fechamentoTurmaId;
       	
       	--Componentes curriculares normais
       	for componenteCurricular in select * from tmp_fechamento_componentes_curriculares tfcc where turmacodigo = turma.turmacodigo and ehregencia = '0' loop
       		insert into
                fechamento_turma_disciplina (disciplina_id, criado_em, criado_por, criado_rf, fechamento_turma_id, situacao) 
               	values(componenteCurricular.componentecurricularid::bigint, now(), 'SISTEMA', '0', fechamentoTurmaId, 3) returning id into fechamentoDisciplinaTurmaId;
            for aluno in select codigo as alunocodigo, nome as alunonome from tmp_fechamento_alunos where turmacodigo = turma.turmacodigo loop
            	insert into fechamento_aluno (aluno_codigo, criado_em, criado_por, criado_rf, fechamento_turma_disciplina_id) 
            		values(aluno.alunocodigo, now(), 'SISTEMA', '0', fechamentoDisciplinaTurmaId) returning id into fechamentoAlunoId;
            	if componenteCurricular.permitelancamentonota = 'True' then
            		if turma.tiponota = '1' then
	            		insert into fechamento_nota (criado_em, criado_por, criado_rf, disciplina_id, nota, fechamento_aluno_id)
    	        			values(now(), 'SISTEMA', '0', componenteCurricular.componentecurricularid::bigint,5, fechamentoAlunoId);
    	        	else
    	        		insert into fechamento_nota (criado_em, criado_por, criado_rf, disciplina_id, conceito_id, fechamento_aluno_id)
    	        			values(now(), 'SISTEMA', '0', componenteCurricular.componentecurricularid::bigint,2, fechamentoAlunoId);
    	        	end if;
    	       	else
    	       		insert into fechamento_nota (criado_em, criado_por, criado_rf, disciplina_id,  fechamento_aluno_id, sintese_id)
    	        			values(now(), 'SISTEMA', '0', componenteCurricular.componentecurricularid::bigint,fechamentoAlunoId, 1);
            	end if;
            END LOOP;
        END LOOP;
       
       --Componentes curriculares regencia
       for componenteCurricular in select distinct componentecurricularregenciaid as componentecurricularid, turmacodigo from tmp_fechamento_componentes_curriculares tfcc where turmacodigo = turma.turmacodigo and ehregencia = '1' loop
       		insert into
                fechamento_turma_disciplina (disciplina_id, criado_em, criado_por, criado_rf, fechamento_turma_id, situacao) 
               	values(componenteCurricular.componentecurricularid::bigint, now(), 'SISTEMA', '0', fechamentoTurmaId, 3) returning id into fechamentoDisciplinaTurmaId;
            for aluno in select codigo as alunocodigo, nome as alunonome from tmp_fechamento_alunos where turmacodigo = turma.turmacodigo loop
            	insert into fechamento_aluno (aluno_codigo, criado_em, criado_por, criado_rf, fechamento_turma_disciplina_id) 
            		values(aluno.alunocodigo, now(), 'SISTEMA', '0', fechamentoDisciplinaTurmaId) returning id into fechamentoAlunoId;
            	
            	for componenteCurricularRegencia in select distinct componentecurricularid, permitelancamentonota, turmacodigo from tmp_fechamento_componentes_curriculares tfcc where turmacodigo = turma.turmacodigo and componentecurricularregenciaid = componenteCurricular.componentecurricularid loop
	            	if componenteCurricularRegencia.permitelancamentonota = 'True' then
	            		if turma.tiponota = '1' then
		            		insert into fechamento_nota (criado_em, criado_por, criado_rf, disciplina_id, nota, fechamento_aluno_id)
	    	        			values(now(), 'SISTEMA', '0', componenteCurricularRegencia.componentecurricularid::bigint,5, fechamentoAlunoId);
	    	        	else
	    	        		insert into fechamento_nota (criado_em, criado_por, criado_rf, disciplina_id, conceito_id, fechamento_aluno_id)
	    	        			values(now(), 'SISTEMA', '0', componenteCurricularRegencia.componentecurricularid::bigint,2, fechamentoAlunoId);
	    	        	end if;
	    	       	else
	    	       		insert into fechamento_nota (criado_em, criado_por, criado_rf, disciplina_id,  fechamento_aluno_id, sintese_id)
	    	        			values(now(), 'SISTEMA', '0', componenteCurricularRegencia.componentecurricularid::bigint,fechamentoAlunoId, 1);
	            	end if;
            	END LOOP;
            
            END LOOP;
        END LOOP;
    commit;
    END LOOP;
end
$$ LANGUAGE plpgsql;
