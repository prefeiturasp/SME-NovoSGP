CREATE INDEX if not EXISTS periodo_escolar_tipo_calendario_id_idx ON public.periodo_escolar USING btree (tipo_calendario_id);
CREATE INDEX if not EXISTS periodo_escolar_tipo_bimestre_idx ON public.periodo_escolar USING btree (bimestre);
CREATE INDEX if not EXISTS periodo_escolar_periodo_inicio_idx ON public.periodo_escolar USING btree (periodo_inicio);
CREATE INDEX if not EXISTS periodo_escolar_periodo_fim_idx ON public.periodo_escolar USING btree (periodo_fim);