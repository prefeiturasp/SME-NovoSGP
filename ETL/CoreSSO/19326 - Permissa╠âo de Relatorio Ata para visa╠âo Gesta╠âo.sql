-- insere o módulo 55 ata para a visão 2 (gestão)
if not exists(select * from SYS_VisaoModulo where vis_id = 2 and sis_id = 1000 and mod_id = 55)
begin
	insert into SYS_VisaoModulo(vis_id, sis_id, mod_id) values(2, 1000, 55)
end