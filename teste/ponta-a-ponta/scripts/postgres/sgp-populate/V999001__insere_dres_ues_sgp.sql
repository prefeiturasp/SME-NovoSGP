do $$
declare dreId bigint;

begin
	insert into dre (dre_id, abreviacao, nome,data_atualizacao) values('108200', 'DRE - CL', 'DIRETORIA REGIONAL DE EDUCACAO CAMPO LIMPO','2020-01-07') RETURNING id INTO dreId;	
	insert into ue (ue_id, dre_id, nome, tipo_escola, data_atualizacao) values('019493',dreId,'CAPAO REDONDO',28,'2020-01-07');
	insert into ue (ue_id, dre_id, nome, tipo_escola, data_atualizacao) values('019375',dreId,'PARAISOPOLIS',17,'2020-01-07');

	insert into dre (dre_id, abreviacao, nome,data_atualizacao) values('108800', 'DRE - JT', 'DIRETORIA REGIONAL DE EDUCACAO JACANA/TREMEMBE','2020-01-07') RETURNING id INTO dreId;	
	insert into ue (ue_id, dre_id, nome, tipo_escola, data_atualizacao) values('094765',dreId,'MAXIMO DE MOURA SANTOS, PROF.',1,'2020-01-07');
	insert into ue (ue_id, dre_id, nome, tipo_escola, data_atualizacao) values('094668',dreId,'DERVILLE ALLEGRETTI, PROF.',3,'2020-01-07');
	insert into ue (ue_id, dre_id, nome, tipo_escola, data_atualizacao) values('017442',dreId,'ANTONIO SAMPAIO, VER.',3,'2020-01-07');
	
	insert into dre (dre_id, abreviacao, nome,data_atualizacao) values('109300', 'DRE - MP', 'DIRETORIA REGIONAL DE EDUCACAO SAO MIGUEL','2020-01-07') RETURNING id INTO dreId;	
	insert into ue (ue_id, dre_id, nome, tipo_escola, data_atualizacao) values('019506',dreId,'MILTON PEREIRA COSTA',1,'2020-01-07');

	insert into dre (dre_id, abreviacao, nome,data_atualizacao) values('108100', 'DRE - BT', 'DIRETORIA REGIONAL DE EDUCACAO BUTANTA','2020-01-07') RETURNING id INTO dreId;	
	insert into ue (ue_id, dre_id, nome, tipo_escola, data_atualizacao) values('019262',dreId,'BUTANTA',16,'2020-01-07');
	
end  $$;