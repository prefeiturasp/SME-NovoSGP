update componente_curricular set permite_registro_frequencia = true
where id in (
508 ,
1030 ,
1033 ,
1051 ,
1052 ,
1053 ,
1054 ,
1060 ,
1061 ,
1214 ,
1215 ,
1216 ,
1217 ,
1218 ,
1219 ,
1220 ,
1221 ,
1222 ,
1223 ,
1222 ,
1223 ,
1322 ,
1046 ,
1018 
);

update componente_curricular set permite_registro_frequencia = false
where id in (
1106 ,
1116 ,
1122 ,
1123 
);

insert into componente_curricular(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao, descricao_sgp)
	values(1321, null, null, null, 'TRABALHO COLABORATIVO DE AUTORIA - TCA', 'Trabalho Colaborativo de Autoria - TCA');
