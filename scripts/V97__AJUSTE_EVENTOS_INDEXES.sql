CREATE index if not exists evento_data_inicio_idx ON public.evento (data_inicio);
CREATE index if not exists evento_data_fim_idx ON public.evento (data_fim);
CREATE index if not exists evento_tipo_calendario_idx ON public.evento (tipo_calendario_id);
CREATE index if not exists evento_dre_idx ON public.evento (dre_id);
CREATE index if not exists evento_ue_idx ON public.evento (ue_id);
CREATE index if not exists evento_criado_rf_idx ON public.evento (criado_rf);
CREATE index if not exists evento_excluido_idx ON public.evento (excluido);

CREATE index if not exists evento_tipo_excluido_idx ON public.evento_tipo (excluido);

CREATE index if not exists abrangencia_turma_idx ON public.abrangencia (turma_id);
CREATE index if not exists abrangencia_ue_idx ON public.abrangencia (ue_id);

CREATE index if not exists dre_dre_id_idx ON public.dre (dre_id);
CREATE index if not exists ue_ue_id_idx ON public.ue (ue_id);
CREATE index if not exists turma_turma_id_idx ON public.turma (turma_id);