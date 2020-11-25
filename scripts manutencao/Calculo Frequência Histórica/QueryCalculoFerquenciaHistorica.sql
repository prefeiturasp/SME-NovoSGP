select dre.dre_id,
	   pe.bimestre,
	   count(distinct rf.id) registros_faltantes
	from registro_frequencia rf
		inner join registro_ausencia_aluno raa
			on rf.id = raa.registro_frequencia_id 
		inner join aula a
			on rf.aula_id = a.id
		inner join tipo_calendario tc
			on a.tipo_calendario_id = tc.id
		inner join periodo_escolar pe
			on tc.id = pe.tipo_calendario_id			   
		inner join turma t
			on a.turma_id = t.turma_id			   
		inner join ue
			on t.ue_id = ue.id
		inner join dre
			on ue.dre_id = dre.id 
where rf.migrado and	  
	  t.ano_letivo = 2019 and	  
	  a.data_aula between pe.periodo_inicio and pe.periodo_fim and
	  not exists (select 1
	  			  	from frequencia_aluno fa	  			  		 			  			   	  			  			
	  			  where fa.codigo_aluno = raa.codigo_aluno and
	  			  	    fa.disciplina_id = a.disciplina_id and
	  			  	    fa.periodo_inicio = pe.periodo_inicio and
	  			  	    fa.periodo_fim = pe.periodo_fim and
	  			  	    fa.bimestre = pe.bimestre and	  			  	    
	  			  	    fa.turma_id = a.turma_id)
group by dre.dre_id,
	     pe.bimestre;
	   	
select fa.criado_em, fa.total_ausencias, ta.total_ausencias, fa.*
	from frequencia_aluno fa 
		inner join turma t
			on fa.turma_id = t.turma_id 
		inner join ue
			on t.ue_id = ue.id
		inner join dre
			on ue.dre_id = dre.id
		inner join periodo_escolar pe
			on fa.periodo_escolar_id = pe.id
		inner join (select raa.codigo_aluno,
	  					   a.turma_id,
						   a.disciplina_id,
						   pe2.bimestre,
	   					   count(distinct raa.id) total_ausencias
						from registro_frequencia rf
							inner join registro_ausencia_aluno raa
								on rf.id = raa.registro_frequencia_id
							inner join aula a
								on rf.aula_id = a.id
							inner join turma t2
								on a.turma_id = t2.turma_id
							inner join tipo_calendario tc
								on a.tipo_calendario_id = tc.id
							inner join periodo_escolar pe2
								on tc.id = pe2.tipo_calendario_id
							inner join ue ue2
								on t2.ue_id = ue2.id
							inner join dre dre2
								on ue2.dre_id = dre2.id
					where t2.ano_letivo = $ano_letivo and
		  				  dre2.dre_id = $dre_id and
		  				  pe2.bimestre = $bimestre and
	  					  a.data_aula between pe2.periodo_inicio and pe2.periodo_fim
					group by raa.codigo_aluno,
		 					 a.turma_id,
	   	 					 a.disciplina_id,
	   	 					 pe2.bimestre) ta
	   	 on fa.codigo_aluno = ta.codigo_aluno and
	   	 	fa.turma_id = ta.turma_id and
	   	 	fa.disciplina_id = ta.disciplina_id and
	   	 	fa.bimestre = ta.bimestre
where t.ano_letivo = $ano_letivo and
	  dre.dre_id = $dre_id and
	  pe.bimestre = $bimestre and
	  fa.tipo = 1 and
	  fa.total_ausencias <> ta.total_ausencias
order by fa.criado_em desc;


select raa.codigo_aluno,
	   a.turma_id,
	   pe.bimestre,
	   count(distinct raa.id)  total_ausencias
	from registro_ausencia_aluno raa
		inner join registro_frequencia rf
			on raa.registro_frequencia_id = rf.id
		inner join aula a
			on rf.aula_id = a.id			
		inner join turma t
			on a.turma_id = t.turma_id 
		inner join tipo_calendario tc
			on a.tipo_calendario_id = tc.id
		inner join periodo_escolar pe
			on tc.id = pe.tipo_calendario_id
where raa.codigo_aluno = '6481705' and
	  a.disciplina_id = '6' and
	  t.ano_letivo = 2019 and
	  pe.bimestre = 1
group by raa.codigo_aluno,
		 pe.bimestre,
	     a.turma_id;
	     
	    
begin transaction;
rollback;
commit;
	    
update frequencia_aluno fa
set total_ausencias = ta.total_ausencias
	from turma t, 
		 periodo_escolar pe,
		 ue,
		 dre,		
		(select raa.codigo_aluno,
	  			a.turma_id,
				a.disciplina_id,
				pe2.bimestre,
	   			count(distinct raa.id) total_ausencias
			from registro_frequencia rf
				inner join registro_ausencia_aluno raa
					on rf.id = raa.registro_frequencia_id
				inner join aula a
					on rf.aula_id = a.id
				inner join turma t2
					on a.turma_id = t2.turma_id
				inner join tipo_calendario tc
					on a.tipo_calendario_id = tc.id
				inner join periodo_escolar pe2
					on tc.id = pe2.tipo_calendario_id
				inner join ue ue2
					on t2.ue_id = ue2.id
				inner join dre dre2
					on ue2.dre_id = dre2.id
			where t2.ano_letivo = $ano_letivo and
  				  dre2.dre_id = $dre_id and
  				  pe2.bimestre = $bimestre and
				  a.data_aula between pe2.periodo_inicio and pe2.periodo_fim
			group by raa.codigo_aluno,
				 	 a.turma_id,
 					 a.disciplina_id,
 					 pe2.bimestre) ta	   	
where fa.turma_id = t.turma_id and
	  fa.periodo_escolar_id = pe.id and
	  t.ue_id = ue.id and
	  ue.dre_id = dre.id and
	  t.ano_letivo = $ano_letivo and
	  dre.dre_id = $dre_id and
	  pe.bimestre = $bimestre and
	  fa.tipo = 1 and
	  fa.codigo_aluno = ta.codigo_aluno and
 	  fa.turma_id = ta.turma_id and
 	  fa.disciplina_id = ta.disciplina_id and
 	  fa.bimestre = ta.bimestre and
	  fa.total_ausencias <> ta.total_ausencias;

select caa.codigo_aluno,
	   t.turma_id,
	   ca.disciplina_id,
	   ca.bimestre,
	   sum(caa.qtd_faltas_compensadas) total_ausencias_compensadas
	from turma t 
		inner join compensacao_ausencia ca
			on t.id = ca.turma_id
		inner join compensacao_ausencia_aluno caa
			on ca.id = caa.compensacao_ausencia_id 		
where t.ano_letivo = 2019
group by caa.codigo_aluno,
	   	 t.turma_id,
	   	 ca.disciplina_id,
	   	 ca.bimestre;
	   	 
	   	
select *
	from frequencia_aluno fa 
		inner join turma t
			on fa.turma_id = t.turma_id
		inner join (select caa.codigo_aluno,
	   					   t2.turma_id,
	   					   ca.disciplina_id,
	   					   ca.bimestre,
	   					   sum(caa.qtd_faltas_compensadas) total_ausencias_compensadas
						from turma t2
							inner join compensacao_ausencia ca
								on t2.id = ca.turma_id
							inner join compensacao_ausencia_aluno caa
								on ca.id = caa.compensacao_ausencia_id 		
					where t2.ano_letivo = 2019
					group by caa.codigo_aluno,
	   	 					 t2.turma_id,
	   	 					 ca.disciplina_id,
	   	 					 ca.bimestre) tca
	   	 	on fa.codigo_aluno = tca.codigo_aluno and
	   	 	   fa.turma_id = tca.turma_id and 
	   	 	   fa.disciplina_id = tca.disciplina_id and
	   	 	   fa.bimestre = tca.bimestre
where t.ano_letivo = 2019 and
	  fa.total_compensacoes <> tca.total_ausencias_compensadas;