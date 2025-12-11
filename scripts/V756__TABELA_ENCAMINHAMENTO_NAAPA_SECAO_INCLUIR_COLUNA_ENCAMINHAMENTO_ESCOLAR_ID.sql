ALTER TABLE public.encaminhamento_naapa_secao
ADD COLUMN encaminhamento_escolar_id int8 NULL;

CREATE INDEX encaminhamento_naapa_secao_enc_esc_idx
    ON public.encaminhamento_naapa_secao (encaminhamento_escolar_id);

ALTER TABLE public.encaminhamento_naapa_secao
ADD CONSTRAINT encaminhamento_naapa_secao_enc_esc_fk
    FOREIGN KEY (encaminhamento_escolar_id)
    REFERENCES public.encaminhamento_escolar (id);    