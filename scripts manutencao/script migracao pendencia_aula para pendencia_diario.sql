do $$
declare
	contador integer := 0;
	pendencia_db record;
begin	
	for pendencia_db in 
		select max(p.id) as PendenciaId,pa.aula_id as AulaId, pa.id as PendenciaAulaId, u.rf_codigo as ProfessorRf,
			   coalesce(db.componente_curricular_id, 512) as ComponenteCurricularId, max(p.criado_em) as CriadoEm, p.criado_por as CriadoPor, p.criado_rf as CriadoRf, 
			   max(p.alterado_em) as AlteradoEm, p.alterado_por as AlteradoPor, p.alterado_rf as AlteradoRf
            from pendencia p 
            inner join pendencia_aula pa on p.id = pa.pendencia_id 
            inner join pendencia_usuario pu on pu.pendencia_id  = p.id 
            inner join usuario u on u.id = pu.usuario_id 
            left join diario_bordo db on db.aula_id = pa.aula_id 
            where p.tipo = 9 and not p.excluido 
            group by pa.aula_id, pa.id, u.rf_codigo, db.componente_curricular_id,
			    p.criado_por, p.criado_rf, p.alterado_por, p.alterado_rf
  	loop
  		insert into pendencia_diario_bordo (pendencia_id, aula_id, professor_rf, componente_curricular_id, criado_em, 
          									criado_por, criado_rf, alterado_em, alterado_por, alterado_rf)
  		values (pendencia_db.PendenciaId, pendencia_db.AulaId, pendencia_db.ProfessorRf, 
  				pendencia_db.ComponenteCurricularId, pendencia_db.CriadoEm, pendencia_db.CriadoPor, 
  				pendencia_db.CriadoRf, pendencia_db.AlteradoEm, pendencia_db.AlteradoPor, 
  			    pendencia_db.AlteradoRf);
  	
        contador = contador + 1;
       
	     if (contador % 1000) = 0 then
	  		commit;
	  	 	raise notice 'Commitou! Esta no registro de numero: %',contador; 
	  	 end if; 	
	  	
	  	DELETE FROM pendencia_aula pa WHERE pa.id = pendencia_db.PendenciaAulaId;
	  
  	end loop;
     commit;  
end $$ 
