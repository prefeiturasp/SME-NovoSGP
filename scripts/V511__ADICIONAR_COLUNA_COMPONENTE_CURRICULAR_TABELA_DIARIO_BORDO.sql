alter table diario_bordo 
add column componente_curricular_id int8;

update diario_bordo set componente_curricular_id = 512;

alter table diario_bordo add constraint componente_curricular_fk foreign key (componente_curricular_id) references componente_curricular(id);

alter  table componente_curricular 
add column descricao_infantil varchar(100);

update componente_curricular set descricao_infantil ='REGÊNCIA INFANTIL EMEI 4H' where id = 512;
update componente_curricular set descricao_infantil ='REGÊNCIA INFANTIL EMEI 2H' where id = 513;
update componente_curricular set descricao_infantil ='REGÊNCIA INFANTIL CEI MANHÃ' where id = 517;
update componente_curricular set descricao_infantil ='REGÊNCIA INFANTIL CEI TARDE' where id = 518;
update componente_curricular set descricao_infantil ='REGÊNCIA INFANTIL EMEI MANHÃ' where id = 534;
update componente_curricular set descricao_infantil ='REGÊNCIA INFANTIL EMEI TARDE' where id = 535;


