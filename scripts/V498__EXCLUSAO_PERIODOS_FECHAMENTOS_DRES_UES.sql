delete
from
	evento_fechamento
where
	fechamento_id in(
	select
		pfb.id
	from
		periodo_fechamento_bimestre pfb
	inner join periodo_fechamento pf on
		pf.id = pfb.periodo_fechamento_id
	where
		pf.dre_id is not null);

delete
from
	periodo_fechamento_bimestre pfb
		using periodo_fechamento pf
where
	pf.id = pfb.periodo_fechamento_id
	and
	pf.dre_id is not null;

delete
from
	periodo_fechamento pf
where
	dre_id is not null;