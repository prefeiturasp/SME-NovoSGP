UPDATE itinerancia AS i
       SET criado_por = 'THAIS DE MELO GONZALEZ',
           criado_rf  = '7448635'
      FROM ue AS u
     WHERE i.id = 51823
       AND u.id = i.ue_id
       AND u.ue_id = '094722'
       AND u.nome = 'JOAO RAMOS - PERNAMBUCO - ABOLICIONISTA'
       AND i.data_visita::date = DATE '2026-07-02'
       AND i.criado_por = 'Sistema'
       AND i.criado_rf = '0'
       AND NOT i.excluido
       AND EXISTS (
           SELECT 1
             FROM wf_aprovacao_itinerancia AS wai
            WHERE wai.itinerancia_id = i.id
              AND wai.wf_aprovacao_id = 48482765
       );