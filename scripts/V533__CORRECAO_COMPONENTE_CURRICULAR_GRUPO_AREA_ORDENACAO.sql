insert INTO componente_curricular_grupo_area_ordenacao (grupo_matriz_id, area_conhecimento_id,ordem) 
select 2,5,5 where not exists(select 1 from componente_curricular_grupo_area_ordenacao where grupo_matriz_id = 2 and area_conhecimento_id = 5 and ordem = 5);

update componente_curricular_grupo_area_ordenacao set ordem = 6 where area_conhecimento_id = 8;
