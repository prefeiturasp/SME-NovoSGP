begin transaction;
ALTER TABLE public.abrangencia ADD historico bool NOT NULL DEFAULT false;
ALTER TABLE public.abrangencia ADD dt_fim_vinculo date NULL;
end transaction;