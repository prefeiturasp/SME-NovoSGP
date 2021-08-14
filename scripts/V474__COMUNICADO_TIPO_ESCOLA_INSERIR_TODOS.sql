do $$
declare comunicadoObtido RECORD;
begin
	for comunicadoObtido in select distinct id, excluido from comunicado c where id not in (select comunicado_id from comunicado_tipo_escola cte) loop
		--EMEF
        insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 1, comunicadoObtido.excluido);
        --EMEI
		insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 2, comunicadoObtido.excluido);
        --EMEFM
		insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 3, comunicadoObtido.excluido);
        --EMEBS
		insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 4, comunicadoObtido.excluido);
        --CEIDIRET
		insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 10, comunicadoObtido.excluido);
        --CEIINDIR
		insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 11, comunicadoObtido.excluido);
        --CRPCONV
		insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 12, comunicadoObtido.excluido);
        --CIEJA
		insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 13, comunicadoObtido.excluido);
        --CCICIPS
		insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 14, comunicadoObtido.excluido);
        ---ESCPART
		insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 15, comunicadoObtido.excluido);
        --CEUEMEF
		insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 16, comunicadoObtido.excluido);
        --CEUEMEI
		insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 17, comunicadoObtido.excluido);
        --CEUCEI
		insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 18, comunicadoObtido.excluido);
        --CEU
		insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 19, comunicadoObtido.excluido);
        --MOVA
		insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 22, comunicadoObtido.excluido);
		--CMCT
		insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 23, comunicadoObtido.excluido);
		--ETEC
		insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 25, comunicadoObtido.excluido);
        --ESPCONV
		insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 26, comunicadoObtido.excluido);
		--CEUATCOMPL
        insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 27, comunicadoObtido.excluido);
        --CCA
		insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 29, comunicadoObtido.excluido);
		--CEMEI
        insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 28, comunicadoObtido.excluido);
        --CECI
		insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 30, comunicadoObtido.excluido);
        --CEU CEMEI
		insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido) values(comunicadoObtido.id, 31, comunicadoObtido.excluido);
	end loop;
end;
$$ LANGUAGE plpgsql;