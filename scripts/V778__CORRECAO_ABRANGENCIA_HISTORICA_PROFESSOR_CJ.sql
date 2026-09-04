update abrangencia a
   set historico      = true
  from turma t
 where t.id = a.turma_id
   and a.historico = false
   and t.historica = true
   and t.ano_letivo = 2026
   and a.perfil in ('41e1e074-37d6-e911-abd6-f81654fe895d',   -- Professor CJ
                    '61e1e074-37d6-e911-abd6-f81654fe895d');  -- Professor CJ Ed. Infantil
