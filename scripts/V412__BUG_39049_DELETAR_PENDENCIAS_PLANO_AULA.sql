delete from pendencia_fechamento 
where pendencia_id in (select id from pendencia where tipo = 3);

delete from pendencia_usuario where pendencia_id in (select id from pendencia where tipo = 3);

delete from pendencia where tipo = 3;