ALTER TABLE conselho_classe_aluno ADD COLUMN IF NOT EXISTS parecer_alterado_manual boolean default false;

ALTER TABLE wf_aprovacao_parecer_conclusivo ADD COLUMN IF NOT EXISTS parecer_alterado_manual boolean default false;