delete from pendencia_usuario 
where id in (
select pu.id from pendencia_usuario pu 
inner join pendencia p on p.id = pu.pendencia_id 
inner join pendencia_perfil pp on pp.pendencia_id = p.id 
inner join pendencia_perfil_usuario ppu on ppu.pendencia_perfil_id = pp.id
where ppu.usuario_id = pu.usuario_id 
);