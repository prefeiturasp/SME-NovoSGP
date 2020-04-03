select * from SYS_Visao;

select * from SYS_GrupoPermissao where sis_id = 1000;

select * from SYS_Grupo where sis_id = 1000;

select * from SYS_VisaoModulo where sis_id = 1000;

INSERT INTO SYS_VisaoModulo
select 1, 1000, mod_id from SYS_VisaoModulo where sis_id = 1000
union all
select 2, 1000, mod_id from SYS_VisaoModulo where sis_id = 1000
union all
select 3, 1000, mod_id from SYS_VisaoModulo where sis_id = 1000;

--INSERT INTO [dbo].[SYS_Grupo]([gru_id],[gru_nome],[gru_situacao],[vis_id],[sis_id],[gru_integridade])VALUES('5DE1E074-37D6-E911-ABD6-F81654FE895D','COPED Basico - SME',1,1,1000,0);


--INSERT INTO SYS_GrupoPermissao
--select '5DE1E074-37D6-E911-ABD6-F81654FE895D', 1000, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir from SYS_GrupoPermissao where sis_id = 1000 AND gru_id = '51E1E074-37D6-E911-ABD6-F81654FE895D'

-------------------------------------------------------------------------------------------------------------------------

--UE

update [dbo].[SYS_Grupo] set vis_id = 3 where gru_id = '3BE1E074-37D6-E911-ABD6-F81654FE895D';--'ATE'
update [dbo].[SYS_Grupo] set vis_id = 3 where gru_id = '3CE1E074-37D6-E911-ABD6-F81654FE895D';--'Professor Readaptado';
update [dbo].[SYS_Grupo] set vis_id = 3 where gru_id = '3DE1E074-37D6-E911-ABD6-F81654FE895D';--'PAEE'
update [dbo].[SYS_Grupo] set vis_id = 3 where gru_id = '3EE1E074-37D6-E911-ABD6-F81654FE895D';--'PAP'
update [dbo].[SYS_Grupo] set vis_id = 3 where gru_id = '3FE1E074-37D6-E911-ABD6-F81654FE895D';--'POA'
update [dbo].[SYS_Grupo] set vis_id = 3 where gru_id = '40E1E074-37D6-E911-ABD6-F81654FE895D';--'Professor'
update [dbo].[SYS_Grupo] set vis_id = 3 where gru_id = '41E1E074-37D6-E911-ABD6-F81654FE895D';--'Professor CJ'
update [dbo].[SYS_Grupo] set vis_id = 3 where gru_id = '42E1E074-37D6-E911-ABD6-F81654FE895D';--'ADM UE'
update [dbo].[SYS_Grupo] set vis_id = 3 where gru_id = '43E1E074-37D6-E911-ABD6-F81654FE895D';--'Secretário'
update [dbo].[SYS_Grupo] set vis_id = 3 where gru_id = '44E1E074-37D6-E911-ABD6-F81654FE895D';--'CP'
update [dbo].[SYS_Grupo] set vis_id = 3 where gru_id = '45E1E074-37D6-E911-ABD6-F81654FE895D';--'AD'
update [dbo].[SYS_Grupo] set vis_id = 3 where gru_id = '46E1E074-37D6-E911-ABD6-F81654FE895D';--'Diretor'
--update [dbo].[SYS_Grupo] set vis_id = 3 where gru_id = '5CE1E074-37D6-E911-ABD6-F81654FE895D';--'POEI'

--DRE

update [dbo].[SYS_Grupo] set vis_id = 2 where gru_id = '47E1E074-37D6-E911-ABD6-F81654FE895D'; --'DRE Básico'
update [dbo].[SYS_Grupo] set vis_id = 2 where gru_id = '48E1E074-37D6-E911-ABD6-F81654FE895D'; --'ADM DRE'
update [dbo].[SYS_Grupo] set vis_id = 2 where gru_id = '49E1E074-37D6-E911-ABD6-F81654FE895D'; --'DIPED'
update [dbo].[SYS_Grupo] set vis_id = 2 where gru_id = '4AE1E074-37D6-E911-ABD6-F81654FE895D'; --'PAAI'
update [dbo].[SYS_Grupo] set vis_id = 2 where gru_id = '4BE1E074-37D6-E911-ABD6-F81654FE895D'; --'CEFAI'
update [dbo].[SYS_Grupo] set vis_id = 2 where gru_id = '4CE1E074-37D6-E911-ABD6-F81654FE895D'; --'NAAPA'
update [dbo].[SYS_Grupo] set vis_id = 2 where gru_id = '4DE1E074-37D6-E911-ABD6-F81654FE895D'; --'Diretor DIPED'
update [dbo].[SYS_Grupo] set vis_id = 2 where gru_id = '4EE1E074-37D6-E911-ABD6-F81654FE895D'; --'Supervisor'
update [dbo].[SYS_Grupo] set vis_id = 2 where gru_id = '4FE1E074-37D6-E911-ABD6-F81654FE895D'; --'Supervisor Técnico'
update [dbo].[SYS_Grupo] set vis_id = 2 where gru_id = '50E1E074-37D6-E911-ABD6-F81654FE895D'; --'Diretor Regional'

--SME

update [dbo].[SYS_Grupo] set vis_id = 1 where gru_id = '51E1E074-37D6-E911-ABD6-F81654FE895D'; --'COPED Básico'
update [dbo].[SYS_Grupo] set vis_id = 1 where gru_id = '52E1E074-37D6-E911-ABD6-F81654FE895D'; --'NTC - NAAPA',
update [dbo].[SYS_Grupo] set vis_id = 1 where gru_id = '53E1E074-37D6-E911-ABD6-F81654FE895D'; --'NTC'
update [dbo].[SYS_Grupo] set vis_id = 1 where gru_id = '54E1E074-37D6-E911-ABD6-F81654FE895D'; --'NTA'
update [dbo].[SYS_Grupo] set vis_id = 1 where gru_id = '55E1E074-37D6-E911-ABD6-F81654FE895D'; --'DIEE'
update [dbo].[SYS_Grupo] set vis_id = 1 where gru_id = '56E1E074-37D6-E911-ABD6-F81654FE895D'; --'DIEJA'
update [dbo].[SYS_Grupo] set vis_id = 1 where gru_id = '57E1E074-37D6-E911-ABD6-F81654FE895D'; --'DIEI'
update [dbo].[SYS_Grupo] set vis_id = 1 where gru_id = '58E1E074-37D6-E911-ABD6-F81654FE895D'; --'DIEFM'
update [dbo].[SYS_Grupo] set vis_id = 1 where gru_id = '59E1E074-37D6-E911-ABD6-F81654FE895D'; --'COPED'
update [dbo].[SYS_Grupo] set vis_id = 1 where gru_id = '5AE1E074-37D6-E911-ABD6-F81654FE895D'; --'ADM SME'
update [dbo].[SYS_Grupo] set vis_id = 1 where gru_id = '5BE1E074-37D6-E911-ABD6-F81654FE895D'; --'ADM COTIC'
