ALTER TABLE public.itinerancia_aluno_questao ALTER COLUMN resposta DROP NOT NULL;
ALTER TABLE public.itinerancia_aluno_questao ADD arquivo_id int8 NULL;
ALTER TABLE public.itinerancia_aluno_questao ADD CONSTRAINT itinerancia_aluno_questao_fk FOREIGN KEY (arquivo_id) REFERENCES public.arquivo(id);
