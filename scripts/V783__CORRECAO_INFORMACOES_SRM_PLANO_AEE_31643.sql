-- HOTFIX 155473 - Plano AEE 31643 (aluno 6846353, EMEF MARIA RITA - 019267)
-- A versão vigente (v16) gravou em "Informações da matrícula em SRM" a SRM de 2023 da EMEF THEODOMIRO,
-- copiada da versão anterior. Hoje o EOL não retorna SRM vigente para o aluno.
-- Ajusta somente a resposta SRM da versão vigente para vazio ('[]'), como o SGP grava atualmente.
UPDATE plano_aee_resposta AS r
       SET texto       = '[]',
           alterado_em = now(),
           alterado_por = 'Sistema',
           alterado_rf  = '0'
      FROM plano_aee_questao AS pq
      JOIN plano_aee_versao AS v ON v.id = pq.plano_aee_versao_id
      JOIN plano_aee AS p        ON p.id = v.plano_aee_id
      JOIN questao AS q          ON q.id = pq.questao_id
      JOIN turma AS t            ON t.id = p.turma_id
      JOIN ue AS u               ON u.id = t.ue_id
     WHERE r.id = 8874317
       AND r.plano_questao_id = pq.id
       AND pq.id = 8876653
       AND v.id = 471207
       AND v.numero = 16
       AND p.id = 31643
       AND p.aluno_codigo = '6846353'
       AND u.ue_id = '019267'
       AND q.tipo = 20
       AND r.texto LIKE '%THEODOMIRO MONTEIRO DO AMARAL%'
       AND NOT r.excluido
       AND NOT pq.excluido
       AND NOT p.excluido
       AND NOT EXISTS (
           SELECT 1
             FROM plano_aee_versao AS v2
            WHERE v2.plano_aee_id = p.id
              AND v2.numero > v.numero
       );
