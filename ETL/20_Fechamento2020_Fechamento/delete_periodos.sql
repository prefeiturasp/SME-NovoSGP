DELETE FROM conselho_classe_nota
                    WHERE conselho_classe_aluno_id IN (SELECT id FROM conselho_classe_aluno
                    WHERE conselho_classe_id IN (
                        SELECT id FROM conselho_classe 
                        WHERE fechamento_turma_id IN (SELECT fechamento_turma.id FROM fechamento_turma 
                        inner join turma t on fechamento_turma.turma_id = t.id
                         where periodo_escolar_id is null and t.ano_letivo = 2020 and t. modalidade_codigo in (3, 5, 6))
                    ));


                    DELETE FROM conselho_classe_aluno
                    WHERE conselho_classe_id IN (
                        SELECT id FROM conselho_classe 
                        WHERE fechamento_turma_id IN (SELECT fechamento_turma.id FROM fechamento_turma inner join turma t on fechamento_turma.turma_id = t.id
                         where periodo_escolar_id is null and t.ano_letivo = 2020 and t. modalidade_codigo in (3, 5, 6))
                    );

                    DELETE FROM conselho_classe 
                    WHERE fechamento_turma_id IN (SELECT fechamento_turma.id FROM fechamento_turma inner join turma t on fechamento_turma.turma_id = t.id
                         where periodo_escolar_id is null and t.ano_letivo = 2020 and t. modalidade_codigo in (3, 5, 6));

                    DELETE FROM fechamento_nota 
                    WHERE fechamento_aluno_id 
                    IN (SELECT id FROM fechamento_aluno 
	                    WHERE fechamento_turma_disciplina_id IN (
	                    	select id from fechamento_turma_disciplina 
	                    	WHERE fechamento_turma_id IN (
								SELECT fechamento_turma.id FROM fechamento_turma inner join turma t on fechamento_turma.turma_id = t.id
		                         	where periodo_escolar_id is null and t.ano_letivo = 2020 and t. modalidade_codigo in (3, 5, 6)))
		               );


                    DELETE FROM fechamento_aluno 
	                    WHERE fechamento_turma_disciplina_id IN (
	                    	select id from fechamento_turma_disciplina 
	                    	WHERE fechamento_turma_id IN (
								SELECT fechamento_turma.id FROM fechamento_turma inner join turma t on fechamento_turma.turma_id = t.id
		                         	where periodo_escolar_id is null and t.ano_letivo = 2020 and t. modalidade_codigo in (3, 5, 6))
                    );

                    DELETE from fechamento_turma_disciplina 
	                    	WHERE fechamento_turma_id IN (
								SELECT fechamento_turma.id FROM fechamento_turma inner join turma t on fechamento_turma.turma_id = t.id
		                         	where periodo_escolar_id is null and t.ano_letivo = 2020 and t. modalidade_codigo in (3, 5, 6));

                    DELETE FROM fechamento_turma WHERE id IN (SELECT fechamento_turma.id FROM fechamento_turma inner join turma t on fechamento_turma.turma_id = t.id
                         where periodo_escolar_id is null and t.ano_letivo = 2020 and t. modalidade_codigo in (3, 5, 6));
                         
