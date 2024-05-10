alter table comunicado drop column comunicado_grupo_id;
alter table comunicado alter column data_expiracao drop not null;
alter table comunidado_grupo drop constraint comunidado_grupo_comunicado_fk;
alter table comunidado_grupo drop constraint comunidado_grupo_grupo_comunicado_fk;
select f_cria_fk_se_nao_existir('comunidado_grupo', 'comunidado_grupo_comunicado_fk', 'FOREIGN KEY (comunicado_id) REFERENCES comunicado(id)');
select f_cria_fk_se_nao_existir('comunidado_grupo', 'comunidado_grupo_grupo_comunicado_fk', 'FOREIGN KEY (grupo_comunicado_id) REFERENCES  grupo_comunicado(id)');
