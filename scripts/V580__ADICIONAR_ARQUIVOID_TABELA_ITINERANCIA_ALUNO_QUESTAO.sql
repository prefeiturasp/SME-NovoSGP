ALTER TABLE public.itinerancia_aluno_questao ALTER COLUMN resposta DROP NOT NULL;
ALTER TABLE public.itinerancia_aluno_questao ADD arquivoid int8 NULL;
ALTER TABLE public.itinerancia_aluno_questao ADD CONSTRAINT itinerancia_aluno_questao_fk FOREIGN KEY (arquivoid) REFERENCES public.arquivo(id);