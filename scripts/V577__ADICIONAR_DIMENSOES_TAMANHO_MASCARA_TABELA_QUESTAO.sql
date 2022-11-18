ALTER TABLE questao
ADD COLUMN IF NOT EXISTS dimensoes int4 default 12,
ADD COLUMN IF NOT EXISTS tamanho int4,
ADD COLUMN IF NOT EXISTS mascara varchar(30);