begin transaction;
rollback;
-- commit;

select *
	from turma t
		inner join fechamento_turma ft
			on t.id = ft.turma_id 
		inner join conselho_classe cc
			on ft.id = cc.fechamento_turma_id
		inner join conselho_classe_aluno cca
			on cc.id = cca.conselho_classe_id
		inner join conselho_classe_nota ccn
			on cca.id = ccn.conselho_classe_aluno_id 
where t.turma_id = 'XXX';

do $$
declare 
	alunos_eja record;
	disciplinas_consideradas record;
	conselho_classe_id_referencia bigint;
	fechamento_turma_id_referencia bigint;
	conselho_classe_aluno_id_referencia bigint;
	conselho_classe_parecer_id_referencia bigint;
	ano_turma_referencia bpchar(1);
	conselho_classe_nota_id_referencia bigint;	

begin
	for alunos_eja in
		select distinct ae.*,
						cc.eh_regencia,
						nccp.tipo_nota
			from alunos_eja_2020 ae
				inner join componente_curricular cc
					on ae.cd_componente_curricular = cc.id
				left join (select cca.aluno_codigo,
								  t.turma_id,
								  ccn.componente_curricular_codigo
						      from fechamento_turma ft
							     inner join conselho_classe cc
							        on ft.id = cc.fechamento_turma_id
							     inner join conselho_classe_aluno cca
							     	on cc.id = cca.conselho_classe_id
							     inner join conselho_classe_nota ccn
							     	on cca.id = ccn.conselho_classe_aluno_id
							     inner join turma t
							     	on ft.turma_id = t.id
							where t.ano_letivo = 2020) a
					on ae.cd_aluno = a.aluno_codigo and
				       ae.cd_turma_escola = a.turma_id::int and
				       ae.cd_componente_curricular = a.componente_curricular_codigo
				inner join turma t2
					on ae.cd_turma_escola = t2.turma_id::int
				inner join tipo_ciclo_ano tca on
				    tca.ano = t2.ano
				    and tca.modalidade = t2.modalidade_codigo
				inner join tipo_ciclo tc on
				    tca.tipo_ciclo_id = tc.id
				inner join notas_conceitos_ciclos_parametos nccp on
				    nccp.ciclo = tc.id
		where a.aluno_codigo is null			  
		order by 1, 2, 3
	loop
		select cc2.id, ft2.id, t2.ano into conselho_classe_id_referencia, fechamento_turma_id_referencia, ano_turma_referencia
			from conselho_classe cc2 
				inner join fechamento_turma ft2
					on cc2.fechamento_turma_id = ft2.id
				inner join turma t2
					on ft2.turma_id = t2.id
		where t2.turma_id::int = alunos_eja.cd_turma_escola;			
		
		if conselho_classe_id_referencia is null then
			insert into conselho_classe (fechamento_turma_id,
										 migrado,
										 excluido,
										 criado_em,
										 criado_rf,
										 alterado_em,
										 alterado_por,
										 criado_rf,
										 alterado_rf,
										 situacao)	
			values (fechamento_turma_id_referencia, false, false, current_date, 'Sistema', null, null, 'Sistema', null, 2)
			returning id into conselho_classe_id_referencia;
		end if;
	
		select cca.id, cp.id into conselho_classe_aluno_id_referencia, conselho_classe_parecer_id_referencia
			from conselho_classe_aluno cca
				left join conselho_classe_parecer cp
					on cca.conselho_classe_parecer_id = cp.id
		where cca.conselho_classe_id = conselho_classe_id_referencia and
			  cca.aluno_codigo = alunos_eja.cd_aluno;
			 
		if conselho_classe_aluno_id_referencia is null then
			insert into conselho_classe_aluno (conselho_classe_id,
											   aluno_codigo,
											   recomendacoes_aluno,
											   recomendacoes_familia,
											   anotacoes_pedagogicas,
											   migrado,
											   excluido,
											   criado_em,
											   criado_por,
											   alterado_em,
											   alterado_por,
											   criado_rf,
											   alterado_rf,
											   conselho_classe_parecer_id)	
			values (conselho_classe_id_referencia, 
					alunos_eja.cd_aluno, 
					'<ul><li>Busque ir além dos conhecimentos trabalhados em sala de aula. Seja curioso.</li><li>Cuide de seu material escolar. Ele é de sua responsabilidade.</li><li>Cuide de suas relações pessoais. Busque ajuda e orientação de professores,funcionários e gestores sempre que necessário..</li><li>Desenvolva uma rotina de estudo e organização para o cumprimento das tarefas e prazos escolares.</li><li>Esclareça suas dúvidas com os professores sempre que necessário.</li><li>Frequente as aulas diariamente. Em caso de ausência,justifique-a.</li><li>Frequente bibliotecas e sites confiáveis para pesquisa.</li><li>Leia,releia,converse com seus colegas e outros adultos sobre temas estudados,buscando ampliar seu entendimento sobre eles.</li><li>Participe das aulas com atenção,pergunte quando tiver dúvidas e faça registro das ideias centrais da aula.</li><li>Peça permissão para falar e saiba ouvir seus colegas.</li><li>Pesquise. Reflita. Questione. Discuta. Escreva.</li><li>Procure desenvolver seus próprios métodos de estudo. Use agenda e anote seus compromissos.</li><li>Valorize,respeite e coopere com o trabalho de todos no ambiente escolar.</li></ul>',
					'<ul><li>Acompanhe a frequência de seus filhos às aulas e às atividades escolares.</li><li>Acompanhe seu filho na realização das lições de casa.</li><li>Ajude a construir uma escola democrática. Participe do Conselho de Escola.</li><li>Compareçaa às reuniões da escola. Dê sua opinião. Ela é muito importante.</li><li>Confira o boletim escolar de seu filho,caso tenha alguma dúvida procure o professor/coordenador.</li><li>Incentive seu filho a cumprir prazos,tarefas e regras da Unidade Escolar.</li><li>Leia bilhetes e avisos que a escola mandar e responda quando necessário.</li><li>Oriente seu filho a cuidar de seu material escolar.</li><li>Peça orientação aos professores e coordenadores caso perceba alguma dificuldade no desempenho do seu filho.</li><li>Procure visitar a escola de seus filhos sempre que precisar.</li><li>Verifique diariamente os cadernos e livros de seus filhos.</li></ul>',
					null,
					false,
					false,
					current_date,
					'Sistema',
					null,
					null,
					'Sistema',
					null,
					case when ano_turma_referencia::int < 3 then 3 else 1 end)
			returning id into conselho_classe_aluno_id_referencia;
		end if;
	
		for disciplinas_consideradas in
			select ccr.componente_curricular_id cd_componente_curricular
				from componente_curricular_regencia ccr 
			where ccr.turno is null and
				  alunos_eja.eh_regencia
			
			union
		
			select alunos_eja.cd_componente_curricular
			where not alunos_eja.eh_regencia
		loop			
			select ccn.id into conselho_classe_nota_id_referencia
				from conselho_classe_nota ccn 
			where ccn.conselho_classe_aluno_id = conselho_classe_aluno_id_referencia and
				  ccn.componente_curricular_codigo = disciplinas_consideradas.cd_componente_curricular;
				 
			if conselho_classe_nota_id_referencia is null then
				insert into conselho_classe_nota (conselho_classe_aluno_id,
												  componente_curricular_codigo,
												  nota,
												  conceito_id,
												  justificativa,
												  migrado,
												  excluido,
												  criado_em,
												  criado_por,
												  alterado_em,
												  alterado_por,
												  criado_rf,
												  alterado_rf)
				values (conselho_classe_aluno_id_referencia,
					    disciplinas_consideradas.cd_componente_curricular,
					    case when alunos_eja.tipo_nota = 1 then 5 else null end,
					    case when alunos_eja.tipo_nota = 2 then 2 else null end,
					    case when alunos_eja.tipo_nota = 1 then '<p>S</p>' else '<p>Continuidade dos estudos</p>' end,					    
					    false,
					    false,
					    current_date,
					    'Sistema',
					    null,
					    null,
					    'Sistema',
					    null);
			end if;		
		end loop;
	end loop;
end $$;