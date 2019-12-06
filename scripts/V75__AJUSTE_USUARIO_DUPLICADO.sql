begin transaction;

create temp table tmp_usuarios(id int8);

insert into tmp_usuarios 
select a.id	 from (SELECT 
        id, rf_codigo, login,
        ROW_NUMBER() OVER (
            PARTITION BY 
                rf_codigo
            ORDER BY 
                id
        ) as  row_num
     FROM 
        usuario  ) a
        where a.row_num > 1;

        
       
        delete from wf_aprovacao_nivel_usuario where usuario_id in (select * from tmp_usuarios);
        delete from wf_aprovacao_nivel_notificacao where notificacao_id in (select id from notificacao where usuario_id in (select * from tmp_usuarios));
       
        delete from notificacao where usuario_id in (select * from tmp_usuarios);
        delete from abrangencia_dres where usuario_id in (select * from tmp_usuarios);
        delete from abrangencia where usuario_id in (select * from tmp_usuarios);
        delete from usuario where id in (select * from tmp_usuarios);
        
        drop table tmp_usuarios;
end transaction
