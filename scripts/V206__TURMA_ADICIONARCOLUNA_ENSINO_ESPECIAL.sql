--adiciona coluna ensino_especial na tabela de turma
ALTER TABLE turma ADD COLUMN IF NOT EXISTS ensino_especial bool default FALSE;