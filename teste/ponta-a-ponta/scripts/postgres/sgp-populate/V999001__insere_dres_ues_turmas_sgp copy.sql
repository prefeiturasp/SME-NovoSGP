do $$
declare dreId bigint;
declare ueId bigint;

begin
	insert into dre (dre_id, abreviacao, nome,data_atualizacao) values('108200', 'DRE - CL', 'DIRETORIA REGIONAL DE EDUCACAO CAMPO LIMPO','2020-01-07') RETURNING id INTO dreId;	
	insert into ue (ue_id, dre_id, nome, tipo_escola, data_atualizacao) values('019493',dreId,'CAPAO REDONDO',28,'2020-01-07') RETURNING id INTO ueId;

	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2266234',ueId,'5A',5,2021,1,0,6,1,'2020-09-03',false,null,	false,0,'2021-02-04','INFANTIL I',1);

	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2266236',ueId,'5B',5,2021,1,0,6,1,'2020-09-03',false,null,	false,0,'2021-02-04','INFANTIL I',1);

	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2266245',ueId,'6A',6,2021,1,0,6,1,'2020-09-03',false,null,	false,0,'2021-02-04','INFANTIL II',1);

	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2266247',ueId,'6B',6,2021,1,0,6,1,'2020-09-03',false,null,	false,0,'2021-02-04','INFANTIL II',1);

	/*-----------------------------------------------*/
	insert into ue (ue_id, dre_id, nome, tipo_escola, data_atualizacao) values('019375',dreId,'PARAISOPOLIS',17,'2020-01-07') RETURNING id INTO ueId;

	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2270146',ueId,'5A',5,2021,1,0,6,1,'2020-12-17',false,null,	false,0,'2021-02-04','INFANTIL I',1);

	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2270156',ueId,'5B',5,2021,1,0,6,1,'2020-09-04',false,null,	false,0,'2021-02-04','INFANTIL I',1);

	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2270200',ueId,'6A',6,2021,1,0,6,1,'2020-09-04',false,null,	false,0,'2021-02-04','INFANTIL II',1);

	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2270227',ueId,'6B',6,2021,1,0,6,1,'2020-09-04',false,null,	false,0,'2021-02-04','INFANTIL II',1);
	
	/*-----------------------------------------------*/
	insert into dre (dre_id, abreviacao, nome,data_atualizacao) values('108800', 'DRE - JT', 'DIRETORIA REGIONAL DE EDUCACAO JACANA/TREMEMBE','2020-01-07') RETURNING id INTO dreId;	
	insert into ue (ue_id, dre_id, nome, tipo_escola, data_atualizacao) values('094765',dreId,'MAXIMO DE MOURA SANTOS, PROF.',1,'2020-01-07') RETURNING id INTO ueId;

	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2261300',ueId,'3A',3,2021,3,1,4,5,'2021-01-07',false,null,	false,2,'2021-02-04','EJA COMPLEMENTAR II',1);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2314656',ueId,'4A',4,2021,3,1,4,5,'2020-12-10',true,null,	false,1,'2021-02-04','EJA FINAL I',1);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2261329',ueId,'4A',4,2021,3,1,4,5,'2020-09-02',false,null,	false,2,'2021-02-04','EJA FINAL II',1);
	
	/*-----------------------------------------------*/
	insert into ue (ue_id, dre_id, nome, tipo_escola, data_atualizacao) values('094668',dreId,'DERVILLE ALLEGRETTI, PROF.',3,'2020-01-07') RETURNING id INTO ueId;

	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2325755',ueId,'1A',1,2021,6,0,7,6,'2021-01-18',false,null,false,0,'2021-02-04','1ª Série',1);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2268431',ueId,'1X',1,2021,6,0,2,1,'2020-09-04',false,null,false,0,'2021-02-04',null,2);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2317816',ueId,'2A',2,2021,6,0,7,6,'2021-01-14',false,null,false,0,'2021-02-04','2ª Série',1);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2317830',ueId,'2G',2,2021,6,0,7,6,'2020-12-14',false,null,false,0,'2021-02-04','2ª Série',1);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2268506',ueId,'2X',2,2021,6,0,2,1,'2020-09-04',false,null,false,0,'2021-02-04',null,2);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2257214',ueId,'3A',3,2021,6,0,5,1,'2020-09-01',false,null,false,0,'2021-02-04','3ª Série',1);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2268187',ueId,'3A',3,2021,6,0,3,1,'2020-09-04',false,null,false,0,'2021-02-04',null,2);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2257238',ueId,'3C',3,2021,6,0,5,1,'2020-09-01',false,null,false,0,'2021-02-04','3ª Série',1);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2268241',ueId,'3C',3,2021,6,0,3,1,'2020-09-04',false,null,false,0,'2021-02-04',null,2);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2257361',ueId,'GA','G',2021,6,0,6,1,'2020-11-04',false,null,false,0,'2021-02-04','2º Ano',1);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2257344',ueId,'GA','G',2021,6,0,6,1,'2020-09-01',false,null,false,0,'2021-02-04','1º Ano',1);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2257460',ueId,'GA','G',2021,6,0,6,1,'2020-09-01',false,null,false,0,'2021-02-04','4º Ano',1);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2257398',ueId,'GA','G',2021,6,0,6,1,'2020-09-01',false,null,false,0,'2021-02-04','3º Ano',1);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2257353',ueId,'GB','G',2021,6,0,6,1,'2020-09-01',false,null,false,0,'2021-02-04','1º Ano',1);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2257380',ueId,'GB','G',2021,6,0,6,1,'2020-09-01',false,null,false,0,'2021-02-04','2º Ano',1);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2257421',ueId,'GB','G',2021,6,0,6,1,'2020-09-01',false,null,false,0,'2021-02-04','3º Ano',1);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2341736',ueId,'LA',2,2021,6,0,4,1,'2021-04-14',false,null,false,0,'2021-02-15',null,7);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2341737',ueId,'LB',2,2021,6,0,4,1,'2021-04-14',false,null,false,0,'2021-02-15',null,7);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2341738',ueId,'LC',2,2021,6,0,4,1,'2021-04-14',false,null,false,0,'2021-02-15',null,7);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2341644',ueId,'MA',2,2021,6,0,4,1,'2021-04-14',false,null,false,0,'2021-02-15',null,7);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2341645',ueId,'MB',2,2021,6,0,4,1,'2021-04-14',false,null,false,0,'2021-02-15',null,7);
	

	/*-----------------------------------------------*/
	insert into ue (ue_id, dre_id, nome, tipo_escola, data_atualizacao) values('017442',dreId,'ANTONIO SAMPAIO, VER.',3,'2020-01-07') RETURNING id INTO ueId;

	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2319331',ueId,'1A',1,2021,6,0,7,6,'2021-01-08',false,null,false,0,'2021-02-04','1ª Série',1);

	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2339895',ueId,'2 CN',2,2021,6,0,4,1,'2021-03-03',false,null,false,0,'2021-02-04',null,7);

	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2339907',ueId,'2 L',2,2021,6,0,4,1,'2021-03-03',false,null,false,0,'2021-02-04',null,7);

	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2319094',ueId,'2A',2,2021,6,0,7,6,'2020-12-16',false,null,false,0,'2021-02-04','2ª Série',1);

	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2257647',ueId,'3A',3,2021,6,0,3,1,'2020-09-01',false,null,false,0,'2021-02-04',null,2);

	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2257614',ueId,'3B',3,2021,6,0,5,1,'2020-09-15',false,null,false,0,'2021-02-04','3ª Série',1);
	
	
	/*-----------------------------------------------*/
	insert into dre (dre_id, abreviacao, nome,data_atualizacao) values('109300', 'DRE - MP', 'DIRETORIA REGIONAL DE EDUCACAO SAO MIGUEL','2020-01-07') RETURNING id INTO dreId;	
	insert into ue (ue_id, dre_id, nome, tipo_escola, data_atualizacao) values('019506',dreId,'MILTON PEREIRA COSTA',1,'2020-01-07') RETURNING id INTO ueId;

	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2248061',ueId,'1A',1,2021,5,0,5,3,'2021-01-07',false,null,	false,2,'2021-02-04','1º Ano',1);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2248087',ueId,'2A',2,2021,5,0,5,3,'2021-01-07',false,null,	false,2,'2021-02-04','2º Ano',1);

	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2248113',ueId,'3A',3,2021,5,0,5,3,'2021-01-07',false,null,	false,2,'2021-02-04','3º Ano',1);

	/*-----------------------------------------------*/
	insert into dre (dre_id, abreviacao, nome,data_atualizacao) values('108100', 'DRE - BT', 'DIRETORIA REGIONAL DE EDUCACAO BUTANTA','2020-01-07') RETURNING id INTO dreId;	
	insert into ue (ue_id, dre_id, nome, tipo_escola, data_atualizacao) values('019262',dreId,'BUTANTA',16,'2020-01-07') RETURNING id INTO ueId;

	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2269980',ueId,'1B',1,2021,5,0,7,6,'2021-01-07',false,null,	false,2,'2021-02-04','1º Ano',1);
	
	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2269982',ueId,'2B',2,2021,5,0,7,6,'2021-01-07',false,null,	false,2,'2021-02-04','2º Ano',1);

	insert into turma (turma_id, ue_id, nome, ano, ano_letivo, modalidade_codigo, semestre, qt_duracao_aula, tipo_turno, data_atualizacao, historica, dt_fim_eol, ensino_especial, etapa_eja, data_inicio, serie_ensino, tipo_turma)
	values ('2269984',ueId,'3B',3,2021,5,0,5,1,'2021-01-07',false,null,	false,2,'2021-02-04','3º Ano',1);

	
end  $$;