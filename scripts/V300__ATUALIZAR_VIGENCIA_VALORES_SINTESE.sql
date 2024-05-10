UPDATE sintese_valores SET inicio_vigencia = inicio_vigencia + 
    MAKE_INTERVAL(YEARS := 2014 - EXTRACT(YEAR FROM inicio_vigencia)::INTEGER);