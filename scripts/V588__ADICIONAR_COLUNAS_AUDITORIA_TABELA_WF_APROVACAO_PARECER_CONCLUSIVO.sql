ALTER TABLE wf_aprovacao_parecer_conclusivo
ALTER column wf_aprovacao_id DROP NOT NULL,
ADD COLUMN IF NOT EXISTS criado_em timestamp(6) default now() not null,
ADD COLUMN IF NOT EXISTS criado_por varchar(200) default '' not null,
ADD COLUMN IF NOT EXISTS alterado_em timestamp(6) null,
ADD COLUMN IF NOT EXISTS alterado_por varchar(200) null,
ADD COLUMN IF NOT EXISTS criado_rf varchar(200) default '' not null,
ADD COLUMN IF NOT EXISTS alterado_rf varchar(200) null;	
