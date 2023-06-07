create table if not exists encaminhamento_naapa_historico_alteracoes(
id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
encaminhamento_naapa_id int8 NOT NULL, 
secao_encaminhamento_naapa_id int8 NULL,
usuario_id int8 NOT NULL, 
campos_inseridos TEXT NULL, 
campos_alterados TEXT NULL,
data_atendimento varchar(20) NULL,
data_historico timestamp NOT NULL, 
tipo_historico int4 NOT NULL,
CONSTRAINT encaminhamento_naapa_historico_alteracoes_pk PRIMARY KEY (id),
CONSTRAINT encaminhamento_naapa_historico_alteracoes_naapa_id_fk FOREIGN KEY (encaminhamento_naapa_id) REFERENCES encaminhamento_naapa(id),
CONSTRAINT encaminhamento_naapa_historico_alteracoes_naapa_secao_id_fk FOREIGN KEY (secao_encaminhamento_naapa_id) REFERENCES secao_encaminhamento_naapa(id),
CONSTRAINT encaminhamento_naapa_historico_alteracoes_usuario_id_fk FOREIGN KEY (usuario_id) REFERENCES usuario(id)
);

CREATE index if not exists encaminhamento_naapa_historico_alteracoes_id_ix ON encaminhamento_naapa_historico_alteracoes USING btree (encaminhamento_naapa_id);
CREATE index if not exists encaminhamento_naapa_historico_alteracoes_secao_id_ix ON encaminhamento_naapa_historico_alteracoes USING btree (secao_encaminhamento_naapa_id);
CREATE index if not exists encaminhamento_naapa_historico_alteracoes_id_ix ON encaminhamento_naapa_historico_alteracoes USING btree (usuario_id);