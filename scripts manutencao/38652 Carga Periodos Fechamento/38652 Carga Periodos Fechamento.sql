do $$
declare 
	periodoFechamentoId bigint;
	ueId bigint;
	dreId bigint;
	eventoId bigint;

begin
	for eventoId in
		select evento_id 
		  from evento_fechamento ef
		 inner join periodo_fechamento_bimestre pfb on pfb.id = ef.fechamento_id 
		 where pfb.periodo_fechamento_id not in (116,117,118) 
	       and pfb.periodo_escolar_id in (45,46,47,48,49,50,51,52)
	loop
		delete from evento_fechamento where evento_id = eventoId;
		delete from evento where id = eventoId;
	end loop;

	delete from periodo_fechamento_bimestre pfb 
	 where periodo_fechamento_id not in (116,117,118) 
	   and periodo_escolar_id in (45,46,47,48,49,50,51,52);
	
	for ueId, dreId in
		select id, dre_id
		  from ue 
		 where tipo_escola in (1,3,4,16)
	loop
		-- Fund.Medio
		insert into periodo_fechamento (ue_id, dre_id, criado_em, criado_por, criado_rf)
			values (ueId, dreId, NOW(), 'SISTEMA', '0')
		RETURNING id INTO periodoFechamentoId;

		insert into periodo_fechamento_bimestre (periodo_fechamento_id, periodo_escolar_id, inicio_fechamento, final_fechamento)
			values (periodoFechamentoId, 45, '2021-04-12', '2021-05-14');
		insert into periodo_fechamento_bimestre (periodo_fechamento_id, periodo_escolar_id, inicio_fechamento, final_fechamento)
			values (periodoFechamentoId, 46, '2021-06-14', '2021-07-08');
		insert into periodo_fechamento_bimestre (periodo_fechamento_id, periodo_escolar_id, inicio_fechamento, final_fechamento)
			values (periodoFechamentoId, 47, '2021-09-06', '2021-10-01');
		insert into periodo_fechamento_bimestre (periodo_fechamento_id, periodo_escolar_id, inicio_fechamento, final_fechamento)
			values (periodoFechamentoId, 48, '2021-11-29', '2021-12-22');

		-- EJA 1o. Bim
		insert into periodo_fechamento (ue_id, dre_id, criado_em, criado_por, criado_rf)
			values (ueId, dreId, NOW(), 'SISTEMA', '0')
		RETURNING id INTO periodoFechamentoId;

		insert into periodo_fechamento_bimestre (periodo_fechamento_id, periodo_escolar_id, inicio_fechamento, final_fechamento)
			values (periodoFechamentoId, 49, '2021-04-12', '2021-05-14');
		insert into periodo_fechamento_bimestre (periodo_fechamento_id, periodo_escolar_id, inicio_fechamento, final_fechamento)
			values (periodoFechamentoId, 50, '2021-06-14', '2021-07-08');
				
		-- EJA 2o. Bim
		insert into periodo_fechamento (ue_id, dre_id, criado_em, criado_por, criado_rf)
			values (ueId, dreId, NOW(), 'SISTEMA', '0')
		RETURNING id INTO periodoFechamentoId;

		insert into periodo_fechamento_bimestre (periodo_fechamento_id, periodo_escolar_id, inicio_fechamento, final_fechamento)
			values (periodoFechamentoId, 51, '2021-09-06', '2021-10-01');
		insert into periodo_fechamento_bimestre (periodo_fechamento_id, periodo_escolar_id, inicio_fechamento, final_fechamento)
			values (periodoFechamentoId, 52, '2021-11-29', '2021-12-22');
	end loop;

end $$;
