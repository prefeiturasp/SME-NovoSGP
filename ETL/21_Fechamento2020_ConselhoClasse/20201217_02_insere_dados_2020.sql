DO $$
DECLARE
   conselhoClasseId bigint;
   conselhoClasseAlunoId bigint;
   fechamentoDisciplinaTurmaId bigint;
   fechamentoAlunoId bigint;
   fechamentoTurma RECORD;
   fechamentoAluno RECORD;
   fechamentoNota RECORD;
   aluno RECORD;
BEGIN
    FOR fechamentoTurma IN select ft.id as id, 
    case
		when modalidade_codigo = 5 then 
			case
				when ano in ('1','2','4','5') then 3 
				else 1
			end
		when modalidade_codigo = 3 then
			case
				when serie_ensino in ('EJA ALFABETIZACAO I','EJA BASICA I') then 3 
				else 1
			end
		else 1
	end as parecer from fechamento_turma ft inner join turma t on t.id = ft.turma_id where t.ano_letivo = 2020 and ft.periodo_escolar_id is null
	loop
		insert into conselho_classe (fechamento_turma_id , criado_em, criado_por, criado_rf, situacao) values(fechamentoTurma.id, now(), 'SISTEMA', '0', 2) returning id into conselhoClasseId;
		for fechamentoAluno in select distinct fa.aluno_codigo as codigo from fechamento_turma_disciplina ftd inner join fechamento_aluno fa on ftd.id = fa.fechamento_turma_disciplina_id where ftd.fechamento_turma_id = fechamentoTurma.id
		loop
			insert into conselho_classe_aluno (conselho_classe_id, aluno_codigo, conselho_classe_parecer_id, criado_em, criado_por, criado_rf, recomendacoes_aluno, recomendacoes_familia) 
				values(conselhoClasseId, fechamentoAluno.codigo,fechamentoTurma.parecer, now(), 'SISTEMA', '0',
				'<ul><li>Busque ir al�m dos conhecimentos trabalhados em sala de aula. Seja curioso.</li><li>Cuide de seu material escolar. Ele � de sua responsabilidade.</li><li>Cuide de suas rela��es pessoais. Busque ajuda e orienta��o de professores,funcion�rios e gestores sempre que necess�rio..</li><li>Desenvolva uma rotina de estudo e organiza��o para o cumprimento das tarefas e prazos escolares.</li><li>Esclare�a suas d�vidas com os professores sempre que necess�rio.</li><li>Frequente �s aulas diariamente. Em caso de aus�ncia,justifique-a.</li><li>Frequente bibliotecas e sites confi�veis para pesquisa.</li><li>Leia,releia,converse com seus colegas e outros adultos sobre temas estudados,buscando ampliar seu entendimento sobre eles.</li><li>Participe das aulas com aten��o,pergunte quando tiver d�vidas e fa�a registro das ideias centrais da aula.</li><li>Pe�a permiss�o para falar e saiba ouvir seus colegas.</li><li>Pesquise. Reflita. Questione. Discuta. Escreva.</li><li>Procure desenvolver seus pr�prios m�todos de estudo. Use agenda e anote seus compromissos.</li><li>Valorize,respeite e coopere com o trabalho de todos no ambiente escolar.</li></ul>',
				'<ul><li>Acompanhe a frequ�ncia de seus filhos �s aulas e �s atividades escolares.</li><li>Acompanhe seu filho na realiza��o das li��es de casa.</li><li>Ajude a construir uma escola democr�tica. Participe do Conselho de Escola.</li><li>Compare�a �s reuni�es da escola. D� sua opini�o. Ela � muito importante.</li><li>Confira o boletim escolar de seu filho,caso tenha alguma d�vida procure o professor/coordenador.</li><li>Incentive seu filho a cumprir prazos,tarefas e regras da Unidade Escolar.</li><li>Leia bilhetes e avisos que a escola mandar e responda quando necess�rio.</li><li>Oriente seu filho a cuidar de seu material escolar.</li><li>Pe�a orienta��o aos professores e coordenadores caso perceba alguma dificuldade no desempenho do seu filho.</li><li>Procure visitar a escola de seus filhos sempre que precisar.</li><li>Verifique diariamente os cadernos e livros de seus filhos.</li></ul>'
				) returning id into conselhoClasseAlunoId;	
		end loop;
	commit;
	end loop;
end;
$$ LANGUAGE plpgsql;



SELECT * FROM fechamento_turma