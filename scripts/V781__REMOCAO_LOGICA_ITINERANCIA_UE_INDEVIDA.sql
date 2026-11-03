-- bugfix: 157561 Registro de outra unidade em relatório AEE

UPDATE itinerancia AS i
       SET excluido = true
      FROM ue AS u
     WHERE u.id = i.ue_id
       AND i.id = 25476
       AND u.ue_id = '200571'
       AND u.nome = 'BOA ESPERANCA'
       AND i.criado_rf = '7250550'
       AND NOT i.excluido;
 