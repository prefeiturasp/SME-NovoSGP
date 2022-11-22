ALTER TABLE questao
ADD COLUMN IF NOT EXISTS dimensao int4 default 12 not null,
ADD COLUMN IF NOT EXISTS tamanho int4,
ADD COLUMN IF NOT EXISTS mascara varchar(30),
ADD COLUMN IF NOT EXISTS placeholder varchar(80);