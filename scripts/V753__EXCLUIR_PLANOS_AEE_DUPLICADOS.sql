WITH planos_aee_ranqueados AS (
    SELECT
        id, 
        ROW_NUMBER() OVER (
            PARTITION BY aluno_codigo, turma_id 
            ORDER BY
                COALESCE(alterado_em, criado_em) DESC, 
                criado_em DESC,
                id DESC
        ) as rn
    FROM
        plano_aee
    WHERE
        excluido is not true
)

UPDATE plano_aee
SET
    excluido = true
WHERE
    id IN (
        SELECT id FROM planos_aee_ranqueados WHERE rn > 1 
    );