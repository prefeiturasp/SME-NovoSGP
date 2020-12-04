
insert into componente_curricular
select 21173663771217,null,4,null,'CULTURAS, ARTE E MEMÓRIA - ARTES PLASTICAS',false,false,true,false,true,false,'CULTURAS, ARTE E MEMÓRIA - ARTES PLASTICAS' 
where not exists (select 1 from componente_curricular WHERE id = 21173663771217)
