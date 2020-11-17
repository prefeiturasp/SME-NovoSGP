select distinct ftd.*
	from turma t 
		inner join fechamento_turma ft
			on t.id = ft.turma_id
		inner join fechamento_turma_disciplina ftd
			on ft.id = ftd.fechamento_turma_id 
where t.turma_id  = '1980188' and
	  t.ano_letivo  = 2019 and
	  ftd.disciplina_id = 9; 	

select count(0)
	from turma t,
		 fechamento_turma ft,			
		 fechamento_turma_disciplina ftd,			
		 turmas_fechamento_substituicao_disciplina tfsd			
where t.ano_letivo between 2014 and 2019 and
	  ftd.disciplina_id = 9 and
	  t.id = ft.turma_id and
	  ft.id = ftd.fechamento_turma_id and
	  t.turma_id = tfsd.turma_id and
	  t.ano_letivo  = tfsd.ano_letivo; 
	 
		inner join fechamento_turma_disciplina ftd
			on ft.id = ftd.fechamento_turma_id 
where t.turma_id  = '1980188' and
	  t.ano_letivo  = 2019 and
	  ftd.disciplina_id = 9; 	 

select dre.nome, ue.nome, t.nome
	from turma t 
		inner join ue
			on t.ue_id = ue.id
		inner join dre
			on ue.dre_id = dre.id
where t.ano_letivo  = 2019 and
	  t.nome = '1A' and
	  ue.nome  like '%PARAISOPOLIS%'; 


select count(distinct ftd.id)
	from turma t,
		 fechamento_turma ft,			
		 fechamento_turma_disciplina ftd,			
		 turmas_fechamento_substituicao_disciplina tfsd			
where t.ano_letivo between 2014 and 2019 and
	  ftd.disciplina_id = 9 and
	  t.id = ft.turma_id and
	  ft.id = ftd.fechamento_turma_id and
	  t.turma_id = tfsd.turma_id and
	  t.ano_letivo  = tfsd.ano_letivo; 
	 
select count(distinct fn.id)
	from turmas_fechamento_substituicao_disciplina tfsd,
		 turma t,
		 fechamento_turma ft,
		 fechamento_turma_disciplina ftd,
		 fechamento_aluno fa,
		 fechamento_nota fn
where tfsd.ano_letivo between $ano_inicial and $ano_final and
	  t.ano_letivo between $ano_inicial and $ano_final and
	  ftd.disciplina_id = 9 and
	  fn.disciplina_id = 9 and
	  (tfsd.turma_id = t.turma_id and tfsd.ano_letivo = t.ano_letivo) and
	  t.id = ft.turma_id and
	  ft.id = ftd.fechamento_turma_id and
	  ftd.id = fa.fechamento_turma_disciplina_id and
	  fa.id = fn.fechamento_aluno_id;
	     
	 
select count(distinct ccn.id)
	from turmas_fechamento_substituicao_disciplina tfsd,
		 turma t,
		 fechamento_turma ft,
		 fechamento_turma_disciplina ftd,
		 fechamento_aluno fa,
		 fechamento_nota fn,
		 conselho_classe cc,
		 conselho_classe_aluno cca,
		 conselho_classe_nota ccn 
where tfsd.ano_letivo between $ano_inicial and $ano_final and
	  t.ano_letivo between $ano_inicial and $ano_final and
	  ftd.disciplina_id = 1106 and
	  fn.disciplina_id = 1106 and
	  ccn.componente_curricular_codigo = 9 and
	  (tfsd.turma_id = t.turma_id and tfsd.ano_letivo = t.ano_letivo) and
	  t.id = ft.turma_id and
	  ft.id = ftd.fechamento_turma_id and
	  ftd.id = fa.fechamento_turma_disciplina_id and
	  fa.id = fn.fechamento_aluno_id and
	  cc.fechamento_turma_id = ft.id and
	  cca.conselho_classe_id = cc.id and
	  ccn.conselho_classe_aluno_id = cca.id;

begin transaction;
rollback;

commit;
	 
update fechamento_turma_disciplina ftd
set disciplina_id = 1106
	from turma t,
		 fechamento_turma ft,
		 turmas_fechamento_substituicao_disciplina tfsd			
where t.ano_letivo between 2014 and 2019 and
	  ftd.disciplina_id = 9 and
	  t.id = ft.turma_id and
	  ft.id = ftd.fechamento_turma_id and
	  t.turma_id = tfsd.turma_id and
	  t.ano_letivo  = tfsd.ano_letivo; 
	 
update fechamento_nota fn
set disciplina_id = 1106
	from turmas_fechamento_substituicao_disciplina tfsd,
		 turma t,
		 fechamento_turma ft,
		 fechamento_turma_disciplina ftd,
		 fechamento_aluno fa		 
where tfsd.ano_letivo between $ano_inicial and $ano_final and
	  t.ano_letivo between $ano_inicial and $ano_final and
	  ftd.disciplina_id = 1106 and
	  fn.disciplina_id = 9 and
	  (tfsd.turma_id = t.turma_id and tfsd.ano_letivo = t.ano_letivo) and
	  t.id = ft.turma_id and
	  ft.id = ftd.fechamento_turma_id and
	  ftd.id = fa.fechamento_turma_disciplina_id and
	  fa.id = fn.fechamento_aluno_id;
	 
	 
update conselho_classe_nota ccn
set componente_curricular_codigo = 1106
	from turmas_fechamento_substituicao_disciplina tfsd,
		 turma t,
		 fechamento_turma ft,
		 fechamento_turma_disciplina ftd,
		 fechamento_aluno fa,
		 fechamento_nota fn,
		 conselho_classe cc,
		 conselho_classe_aluno cca		 
where tfsd.ano_letivo between $ano_inicial and $ano_final and
	  t.ano_letivo between $ano_inicial and $ano_final and
	  ftd.disciplina_id = 1106 and
	  fn.disciplina_id = 1106 and
	  ccn.componente_curricular_codigo = 9 and
	  (tfsd.turma_id = t.turma_id and tfsd.ano_letivo = t.ano_letivo) and
	  t.id = ft.turma_id and
	  ft.id = ftd.fechamento_turma_id and
	  ftd.id = fa.fechamento_turma_disciplina_id and
	  fa.id = fn.fechamento_aluno_id and
	  cc.fechamento_turma_id = ft.id and
	  cca.conselho_classe_id = cc.id and
	  ccn.conselho_classe_aluno_id = cca.id;