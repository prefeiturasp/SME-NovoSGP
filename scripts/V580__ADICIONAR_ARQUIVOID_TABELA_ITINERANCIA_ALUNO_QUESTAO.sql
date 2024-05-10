ALTER TABLE public.itinerancia_questao ALTER COLUMN resposta DROP NOT NULL;
ALTER TABLE public.itinerancia_questao ADD arquivo_id int8 NULL;
ALTER TABLE public.itinerancia_questao ADD CONSTRAINT itinerancia_questao_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES public.arquivo(id);
