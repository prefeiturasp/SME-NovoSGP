update notas_conceito set conceito = null
where id in (select id from notas_conceito nc
where nc.conceito = 0) ;


update notas_conceito set nota = null
where id in (select id from notas_conceito nc
where nc.nota = 0
and conceito <> 0) ;