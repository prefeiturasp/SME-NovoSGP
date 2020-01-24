alter table compensacao_ausencia add ano_letivo int4 not null default 0;
CREATE INDEX compensacao_ausencia_ano_letivo_idx ON public.compensacao_ausencia USING btree (ano_letivo);
