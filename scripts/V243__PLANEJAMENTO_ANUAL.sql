--Área do conhecimento
CREATE TABLE if not exists public.componente_curricular_area_conhecimento (
	id int8 NOT NULL,
	nome varchar(200) NOT null,
	CONSTRAINT componente_curricular_area_conhecimento_pk PRIMARY KEY (id)
);


INSERT INTO public.componente_curricular_area_conhecimento
(id, nome)
VALUES(1, 'Linguagens');
INSERT INTO public.componente_curricular_area_conhecimento
(id, nome)
VALUES(2, 'Matemática');
INSERT INTO public.componente_curricular_area_conhecimento
(id, nome)
VALUES(3, 'Ciências da Natureza');
INSERT INTO public.componente_curricular_area_conhecimento
(id, nome)
VALUES(4, 'Ciências Humanas');
INSERT INTO public.componente_curricular_area_conhecimento
(id, nome)
VALUES(5, 'Língua Estrangeira Moderna');
INSERT INTO public.componente_curricular_area_conhecimento
(id, nome)
VALUES(6, 'Língua Brasileira de Sinais');



--Grupo Matriz

CREATE TABLE if not exists public.componente_curricular_grupo_matriz (
	id int8 NOT null,
	nome varchar(200) NOT null,
	CONSTRAINT componente_curricular_grupo_matriz_pk PRIMARY KEY (id)
);

INSERT INTO public.componente_curricular_grupo_matriz
(id, nome)
VALUES(1, 'Base Nacional Comum');
INSERT INTO public.componente_curricular_grupo_matriz
(id, nome)
VALUES(2, 'Diversificada');
INSERT INTO public.componente_curricular_grupo_matriz
(id, nome)
VALUES(3, 'Enriquecimento Curricular');
INSERT INTO public.componente_curricular_grupo_matriz
(id, nome)
VALUES(4, 'Integral');



--Componente Curricular

CREATE TABLE if not exists public.componente_curricular(
	id int8 NOT NULL,
	componente_curricular_pai_id int8 NULL,
	grupo_matriz_id int8 NULL,
	area_conhecimento_id int8 null,
	descricao varchar(100) NULL,
	eh_regencia bool NOT NULL DEFAULT false,
	eh_compartilhada bool NOT NULL DEFAULT false,
	eh_territorio bool NOT NULL DEFAULT false,
	eh_base_nacional bool NOT NULL DEFAULT false,
	permite_registro_frequencia bool NOT NULL DEFAULT false,
	permite_lancamento_nota bool NOT NULL DEFAULT true,
	CONSTRAINT componente_curricular_teste_pk PRIMARY KEY (id)
);


select
	f_cria_fk_se_nao_existir(
		'componente_curricular',
		'componente_curricular_area_conhecimento_fk',
		'FOREIGN KEY (area_conhecimento_id) REFERENCES componente_curricular_area_conhecimento (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'componente_curricular',
		'componente_curricular_grupo_matriz_fk',
		'FOREIGN KEY (grupo_matriz_id) REFERENCES componente_curricular_grupo_matriz (id)'
	);


--Jurema/Curriculo

CREATE TABLE if not exists public.componente_curriculo_cidade(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo int8 NOT NULL,
	componente_curricular_id int8 null,
	CONSTRAINT componente_curriculo_cidade_pk PRIMARY KEY (id)
);


select
	f_cria_fk_se_nao_existir(
		'componente_curriculo_cidade',
		'componente_curriculo_cidade_componente_curricular_fk',
		'FOREIGN KEY (componente_curricular_id) REFERENCES componente_curricular (id)'
	);


--Plano Anual
CREATE TABLE if not exists public.planejamento_anual (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 NOT NULL,
	componente_curricular_id int8 NOT NULL,
	migrado bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT planejamento_anual_pk PRIMARY KEY (id)
);


CREATE TABLE if not exists public.planejamento_anual_periodo_escolar (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	periodo_escolar_id int8 NOT NULL,
	planejamento_anual_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT planejamento_anual_periodo_escolar_pk PRIMARY KEY (id)
);

select
	f_cria_fk_se_nao_existir(
		'planejamento_anual',
		'planejamento_anual_turma_fk',
		'FOREIGN KEY (turma_id) REFERENCES turma (id)'
	);
	
	
select
	f_cria_fk_se_nao_existir(
		'planejamento_anual',
		'planejamento_anual_componente_curricular_fk',
		'FOREIGN KEY (componente_curricular_id) REFERENCES componente_curricular (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'planejamento_anual_periodo_escolar',
		'planejamento_anual_periodo_escolar_planejamento_anual_fk',
		'FOREIGN KEY (planejamento_anual_id) REFERENCES planejamento_anual (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'planejamento_anual_periodo_escolar',
		'planejamento_anual_periodo_escolar_periodo_escolar_fk',
		'FOREIGN KEY (periodo_escolar_id) REFERENCES periodo_escolar (id)'
	);

	
CREATE TABLE if not exists public.planejamento_anual_componente (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	planejamento_anual_periodo_escolar_id int8 NOT NULL,
	componente_curricular_id int8 NOT NULL,
	descricao varchar NOT null,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT planejamento_anual_componente_pk PRIMARY KEY (id)
);


--ALTER TABLE public.planejamento_anual_componente ADD CONSTRAINT planejamento_anual_componente_componente_curricular_fk FOREIGN KEY (componente_curricular_id) REFERENCES public.componente_curricular(id);


select
	f_cria_fk_se_nao_existir(
		'planejamento_anual_componente',
		'planejamento_anual_componente_planejamento_anual_periodo_escolar_fk',
		'FOREIGN KEY (planejamento_anual_periodo_escolar_id) REFERENCES planejamento_anual_periodo_escolar (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'planejamento_anual_componente',
		'planejamento_anual_componente_componente_curricular_fk',
		'FOREIGN KEY (componente_curricular_id) REFERENCES componente_curricular (id)'
	);
	


CREATE TABLE if not exists public.planejamento_anual_objetivos_aprendizagem(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	planejamento_anual_componente_id int8 NOT NULL,
	objetivo_aprendizagem_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT planejamento_anual_objetivos_aprendizagem_pk PRIMARY KEY (id)
);

select
	f_cria_fk_se_nao_existir(
		'planejamento_anual_objetivos_aprendizagem',
		'planejamento_anual_objetivos_aprendizagem_planejamento_anual_fk',
		'FOREIGN KEY (planejamento_anual_componente_id) REFERENCES planejamento_anual_componente (id)'
	);
	

select
	f_cria_fk_se_nao_existir(
		'planejamento_anual_objetivos_aprendizagem',
		'planejamento_anual_objetivos_aprendizagem_objetivos_aprendizagem_fk',
		'FOREIGN KEY (objetivo_aprendizagem_id) REFERENCES objetivo_aprendizagem (id)'
	);
	

INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1, NULL, 1, NULL, 'PORTUGUES', false, false, false, true, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(2, NULL, 1, 2, 'MATEMATICA', false, false, false, true, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(6, NULL, 1, 1, 'ED. FISICA', false, false, false, true, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(7, NULL, 1, 4, 'HISTORIA', false, false, false, true, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(8, NULL, 1, 4, 'GEOGRAFIA', false, false, false, true, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(9, NULL, 2, 5, 'INGLES', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(51, NULL, 1, 3, 'FISICA', false, false, false, true, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(52, NULL, 1, 3, 'QUIMICA', false, false, false, true, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(53, NULL, 1, 3, 'BIOLOGIA', false, false, false, true, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(68, NULL, 2, NULL, 'DIDAT.PRAT.ENS.', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(79, NULL, 2, NULL, 'LITER. INFANTIL', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(89, NULL, 1, 3, 'CIENCIAS', false, false, false, true, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(98, NULL, 2, 4, 'FILOSOFIA', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(103, NULL, 2, 4, 'SOCIOLOGIA', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(106, NULL, 2, NULL, 'HISTORIA DA EDUCACAO', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(138, NULL, 1, 1, 'LINGUA PORTUGUESA', false, false, false, true, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(139, NULL, 1, 1, 'ARTE', false, false, false, true, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(157, NULL, 2, NULL, 'ESTATIS. APLIC. EDUC', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(158, NULL, 2, NULL, 'INFORMAT.APLIC. EDUC', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(159, NULL, 2, NULL, 'PSICOL.GERAL E EDUC', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(160, NULL, 2, NULL, 'SOCIOL.GERAL E EDUC', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(161, NULL, 2, NULL, 'BIOLOGIA EDUCACIONAL', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(162, NULL, 2, NULL, 'FILOSOF.GERAL E EDUC', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(163, NULL, 2, NULL, 'LING. PORT. LIT.BRAS', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(164, NULL, 2, NULL, 'ESTR.FUNC.E.F. E INF', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(165, NULL, 2, NULL, 'PSIC.DES.E DA APREND', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(167, NULL, 2, NULL, 'MET.ENS.LING.PORT', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(168, NULL, 2, NULL, 'MET.ENS.HIST. E GEOG', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(169, NULL, 2, NULL, 'MET.ENS.CIENCIAS', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(170, NULL, 2, NULL, 'MET.ENSINO DA ARTE', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(171, NULL, 2, NULL, 'MET.ENS.ED.FIS.INFAN', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(172, NULL, 2, NULL, 'MET.ENS.MATEMATICA', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(173, NULL, 2, NULL, 'MET.ENS.EDUC.INFANTI', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(218, NULL, 2, 6, 'LIBRAS', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(508, NULL, 1, NULL, 'REGENCIA CLAS.F I', true, false, false, false, false, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(512, 512, NULL, NULL, 'ED.INF. EMEI 4 HS', true, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(513, 512, NULL, NULL, 'ED.INF. EMEI 2 HS', true, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(515, 512, NULL, NULL, 'REGENCIA CEI PARCIAL', true, false, false, false, true, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(517, 512, NULL, NULL, 'REG CEI INTEG/MANHA', true, false, false, false, true, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(518, 512, NULL, NULL, 'REG CEI INTEG/TARDE', true, false, false, false, true, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(534, 512, NULL, NULL, 'REG -EMEI -INT/MANHA', true, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(535, 512, NULL, NULL, 'REG -EMEI -INT/TARDE', true, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(537, NULL, 2, 5, 'LINGUA ESPANHOLA', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1030, NULL, NULL, NULL, 'SRM', false, false, false, false, false, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1033, NULL, NULL, NULL, 'REC PAR MATEMATICA', false, false, false, false, false, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1051, NULL, NULL, NULL, 'REC PAR CIENCIAS', false, false, false, false, false, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1052, NULL, NULL, NULL, 'REC PAR GEOGRAFIA', false, false, false, false, false, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1053, NULL, NULL, NULL, 'REC PAR HISTORIA', false, false, false, false, false, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1054, NULL, NULL, NULL, 'REC PAR PORTUGUES', false, false, false, false, false, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1060, NULL, 3, NULL, 'INFORMATICA - OIE', false, false, false, false, false, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1061, NULL, 3, NULL, 'LEITURA - OSL', false, false, false, false, false, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1062, NULL, 2, NULL, 'METODOLOGIA - EJA', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1063, NULL, 2, NULL, 'ED. ESPECIAL', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1066, NULL, 1, NULL, 'ARTE FI - INTEGRAL MANHA', false, false, false, true, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1067, NULL, 1, NULL, 'ARTE FI - INTEGRAL TARDE', false, false, false, true, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1068, NULL, 3, NULL, 'LEITURA (OSL) FI - INTEGRAL MANHA', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1069, NULL, 3, NULL, 'LEITURA (OSL) FI - INTEGRAL TARDE', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1070, NULL, 3, NULL, 'INFORMATICA (OIE) FI - INTEGRAL MANHA', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1071, NULL, 3, NULL, 'INFORMATICA (OIE) FI - INTEGRAL TARDE', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1105, NULL, 1, NULL, 'REG CLASSE CICLO ALFAB / INTERD 5HRS', true, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1106, NULL, 2, NULL, 'LINGUA INGLESA COMPARTILHADA', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1111, NULL, 3, NULL, 'PROJETOS', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1112, NULL, 1, NULL, 'REG CLASSE CICLO ALFAB / INTERD 4HRS', true, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1113, NULL, 1, NULL, 'REG CLASSE EJA ETAPA ALFAB', true, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1114, NULL, 1, NULL, 'REG CLASSE EJA ETAPA BASICA', true, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1115, NULL, 1, NULL, 'REG CLASSE ESPECIAL DIURNO', true, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1116, NULL, 2, NULL, 'LIBRAS COMPARTILHADA', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1117, NULL, 1, NULL, 'REG CLASSE ESPECIAL NOTURNO', true, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1121, 11211124, 1, NULL, 'REG CLASSE ALFAB_INTEGRAL TARDE', true, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1122, NULL, 2, NULL, 'LINGUA INGLESA COMPARTILHADA MANHA', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1123, NULL, 2, NULL, 'LINGUA INGLESA COMPARTILHADA TARDE', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1124, 11211124, 1, NULL, 'REG CLASSE ALFAB_INTEGRAL MANHA', true, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1125, NULL, 1, NULL, 'REG CLASSE EJA EE', true, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1150, NULL, 3, NULL, 'DOCENCIA COMPARTILHADA 4 AULAS', false, true, false, false, false, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1151, NULL, 3, NULL, 'DOCENCIA COMPARTILHADA 3 AULAS', false, true, false, false, false, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1209, NULL, 3, NULL, 'PROJETO INTEGRAL - MANHA', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1210, NULL, 3, NULL, 'PROJETO INTEGRAL - TARDE', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1211, NULL, 1, NULL, 'REG CLASSE CICLO II / INTER _ INTEGRAL MANHA', true, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1212, NULL, 1, NULL, 'REG CLASSE CICLO II / INTER -INTEGRAL - TARDE', true, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1213, NULL, 1, NULL, 'REG CLASSE SP INTEGRAL 1A5 ANOS', true, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1214, NULL, 4, NULL, 'TERRIT SABER / EXP PEDAG 1', false, false, true, false, false, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1215, NULL, 4, NULL, 'TERRIT SABER / EXP PEDAG 2', false, false, true, false, false, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1216, NULL, 4, NULL, 'TERRIT SABER / EXP PEDAG 3', false, false, true, false, false, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1217, NULL, 4, NULL, 'TERRIT SABER / EXP PEDAG 4', false, false, true, false, false, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1218, NULL, 4, NULL, 'TERRIT SABER / EXP PEDAG 5', false, false, true, false, false, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1219, NULL, 4, NULL, 'TERRIT SABER / EXP PEDAG 6', false, false, true, false, false, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1220, NULL, 4, NULL, 'TERRIT SABER / EXP PEDAG 7', false, false, true, false, false, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1221, NULL, 4, NULL, 'TERRIT SABER / EXP PEDAG 8', false, false, true, false, false, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1222, NULL, 4, NULL, 'TERRIT SABER / EXP PEDAG 9', false, false, true, false, false, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1223, NULL, 4, NULL, 'TERRIT SABER / EXP PEDAG 10', false, false, true, false, false, false);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1288, NULL, 1, NULL, 'ED. FISICA  INTEGRAL MANHA', false, false, false, true, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1289, NULL, 1, NULL, 'ED. FISICA  INTEGRAL TARDE', false, false, false, true, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1290, NULL, 1, NULL, 'REG CLASSE SP INTEGRAL ESP 1A5 ANOS / EMEBS', true, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1293, NULL, 2, NULL, 'ORIENTAÇÃO DE ESTUDOS E PROJETOS', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1301, NULL, 1, NULL, 'REG CLASSE SURDOCEGUEIRA', true, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1311, NULL, 2, NULL, 'CULTURA DOS PAISES DE LINGUA ESPANHOLA', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1312, NULL, 2, NULL, 'TECNOLOGIAS PARA APRENDIZAGEM', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1313, NULL, 2, NULL, 'PRODUCAO TEXTUAL', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1314, NULL, 2, NULL, 'INVESTIGACAO CIENTIFICA E PROCESSOS MATEMATICOS', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1317, NULL, 2, NULL, 'PRATICAS ESPORTIVAS', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1318, NULL, 2, NULL, 'EXPRESSOES CULTURAIS ARTISTICAS', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1319, NULL, 2, NULL, 'SALA DE LEITURA', false, false, false, false, true, true);
INSERT INTO public.componente_curricular
(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional, permite_registro_frequencia, permite_lancamento_nota)
VALUES(1322, NULL, 1, NULL, 'PAP - RECUPERACAO DE APRENDIZAGENS', false, false, false, false, false, false);


