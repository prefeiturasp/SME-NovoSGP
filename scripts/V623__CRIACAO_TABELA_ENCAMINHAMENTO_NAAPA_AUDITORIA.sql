create table if not exists encaminhamento_naapa_auditoria(
id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
encaminhamento_naapa_id int8 NOT NULL, 
encaminhamento_naapa_secao_id int8 NULL,
usuario_id int8 NOT NULL, 
campos_inseridos TEXT NULL, 
campos_alterados TEXT NULL,
data_auditoria timestamp NOT NULL, 
tipo_auditoria int4 NOT NULL,
CONSTRAINT encaminhamento_naapa_auditoria_pk PRIMARY KEY (id),
CONSTRAINT encaminhamento_naapa_auditoria_naapa_id_fk FOREIGN KEY (encaminhamento_naapa_id) REFERENCES encaminhamento_naapa(id),
CONSTRAINT encaminhamento_naapa_auditoria_naapa_secao_id_fk FOREIGN KEY (encaminhamento_naapa_secao_id) REFERENCES encaminhamento_naapa_secao(id),
CONSTRAINT encaminhamento_naapa_auditoria_usuario_id_fk FOREIGN KEY (usuario_id) REFERENCES usuario(id)
);

CREATE index if not exists encaminhamento_naapa_auditoria_naapa_id_ix ON encaminhamento_naapa_auditoria USING btree (encaminhamento_naapa_id);
CREATE index if not exists encaminhamento_naapa_auditoria_naapa_secao_id_ix ON encaminhamento_naapa_auditoria USING btree (encaminhamento_naapa_secao_id);
CREATE index if not exists encaminhamento_naapa_auditoria_usuario_id_ix ON encaminhamento_naapa_auditoria USING btree (usuario_id);