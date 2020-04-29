ALTER TABLE public.grupo_comunicado ADD COLUMN if not exists etapa_ensino_id varchar(50) null;
update public.grupo_comunicado set etapa_ensino_id = '1,10' where id = 2;
update public.grupo_comunicado set etapa_ensino_id = '1,10' where id = 3;
update public.grupo_comunicado set etapa_ensino_id = '5', tipo_ciclo_id = '1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,31,32' where id = 4;
update public.grupo_comunicado set etapa_ensino_id = '6,8,9', tipo_ciclo_id = '1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,31,32' where id = 5;
update public.grupo_comunicado set etapa_ensino_id = '3,11', tipo_ciclo_id = '1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,31,32' where id = 6;

