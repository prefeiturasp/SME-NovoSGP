--NOVOS GRUPOS MATRIZ
INSERT INTO public.componente_curricular_grupo_matriz
(id, nome)
VALUES(5, 'Percurso Comum');
INSERT INTO public.componente_curricular_grupo_matriz
(id, nome)
VALUES(6, 'Itinerário Formativo');

UPDATE public.componente_curricular set grupo_matriz_id = 5
where id in (1329,1330,1331,1332,1333,1336,1335,1334,1346,1318,1350,1359,1347);

UPDATE public.componente_curricular set grupo_matriz_id = 6
where id in (1417,1418,1418,1419,1425,1420,1421,1422,1423,1424,1426,1427,1428,1337,1431,1429,1430);

--NOVAS AREAS DE CONHECIMENTO
--Anterior 'Linguagens'
update public.componente_curricular_area_conhecimento 
set nome = 'Linguagens e suas tecnologias' 
where id = 1;

--Anterior 'Matemática'
update public.componente_curricular_area_conhecimento 
set nome = 'Matemática e suas tecnologias' 
where id = 2;

--Anterior 'Ciências da Natureza'
update public.componente_curricular_area_conhecimento 
set nome = 'Ciências da Natureza e suas tecnologias' 
where id = 3;

--Anterior 'Ciências Humanas'
update public.componente_curricular_area_conhecimento 
set nome = 'Ciências Humanas e Sociais Aplicadas' 
where id = 4;

insert into public.componente_curricular_area_conhecimento (id, nome)
values 
(7, 'Formação para estudos e apronfundamento'), 
(8, 'Itinerário Integrador'),
(9, 'Ciências da Natureza, Matemática e suas tecnologias');

UPDATE public.componente_curricular set area_conhecimento_id = 7
where id in (1329,1330,1331,1332,1333,1336,1335,1334);

UPDATE public.componente_curricular set area_conhecimento_id = 8
where id in (1346,1318,1350,1359,1347);

UPDATE public.componente_curricular set area_conhecimento_id = 1
where id in (1417,1418,1318,1419);

UPDATE public.componente_curricular set area_conhecimento_id = 4
where id in (1425,1420,1421,1422,1423,1424,1426,1427);

UPDATE public.componente_curricular set area_conhecimento_id = 9
where id in (1428,1337,1431,1429,1430);
