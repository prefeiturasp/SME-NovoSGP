do $$
declare
	contador integer := 0;
	nroRegistros integer;
	pendencia_db record;
begin
	 select into nroRegistros count(p.id) from pendencia p 
		    inner join pendencia_aula pa on p.id = pa.pendencia_id 
            inner join pendencia_usuario pu on pu.pendencia_id  = p.id 
            inner join usuario u on u.id = pu.usuario_id 
            left join diario_bordo db on db.aula_id = pa.aula_id 
            where p.tipo = 9 and not p.excluido;
		
	for pendencia_db in 
		select p.id as PendenciaId, pa.aula_id as AulaId, u.rf_codigo as ProfessorRf,
			   coalesce(db.componente_curricular_id, 512) as ComponenteCurricularId, p.criado_em as CriadoEm, p.criado_por as CriadoPor, p.criado_rf as CriadoRf, 
			   p.alterado_em as AlteradoEm, p.alterado_por as AlteradoPor, p.alterado_rf as AlteradoRf
            from pendencia p 
            inner join pendencia_aula pa on p.id = pa.pendencia_id 
            inner join pendencia_usuario pu on pu.pendencia_id  = p.id 
            inner join usuario u on u.id = pu.usuario_id 
            left join diario_bordo db on db.aula_id = pa.aula_id 
            where p.tipo = 9 and not p.excluido
  	loop
  		insert into pendencia_diario_bordo (pendencia_id, aula_id, professor_rf, componente_curricular_id, criado_em, 
          									criado_por, criado_rf, alterado_em, alterado_por, alterado_rf)
  		values (pendencia_db.PendenciaId, pendencia_db.AulaId, pendencia_db.ProfessorRf, 
  				pendencia_db.ComponenteCurricularId, pendencia_db.CriadoEm, pendencia_db.CriadoPor, 
  				pendencia_db.CriadoRf, pendencia_db.AlteradoEm, pendencia_db.AlteradoPor, 
  			    pendencia_db.AlteradoRf);
  	
        contador = contador + 1;
          
	     if (contador % 1000) = 0 or nroRegistros = contador then
	  		commit;
	  	 	raise notice 'Commitou! Esta no registro de numero: %',contador; 
	  	 end if; 	
	  	
  	end loop;
end $$ 