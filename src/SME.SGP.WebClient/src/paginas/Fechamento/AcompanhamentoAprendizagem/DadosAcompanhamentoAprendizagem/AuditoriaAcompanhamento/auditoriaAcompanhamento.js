import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { Auditoria } from '~/componentes';

const AuditoriaAcompanhamentoAprendizagem = () => {
  const auditoriasAcompanhamentoAprendizagem = useSelector(
    store =>
      store.acompanhamentoAprendizagem.dadosAcompanhamentoAprendizagem
        ?.auditoria
  );

  const [dadosAuditoria, setDadosAuditoria] = useState({});

  useEffect(() => {
    if (
      auditoriasAcompanhamentoAprendizagem?.id &&
      dadosAuditoria !== auditoriasAcompanhamentoAprendizagem
    ) {
      setDadosAuditoria();
      setDadosAuditoria(auditoriasAcompanhamentoAprendizagem);
    }
  }, [auditoriasAcompanhamentoAprendizagem, dadosAuditoria]);

  return dadosAuditoria ? (
    <Auditoria
      alteradoEm={dadosAuditoria?.alteradoEm}
      alteradoPor={dadosAuditoria?.alteradoPor}
      alteradoRf={dadosAuditoria?.alteradoRF}
      criadoEm={dadosAuditoria?.criadoEm}
      criadoPor={dadosAuditoria?.criadoPor}
      criadoRf={dadosAuditoria?.criadoRF}
    />
  ) : (
    ''
  );
};

export default AuditoriaAcompanhamentoAprendizagem;
