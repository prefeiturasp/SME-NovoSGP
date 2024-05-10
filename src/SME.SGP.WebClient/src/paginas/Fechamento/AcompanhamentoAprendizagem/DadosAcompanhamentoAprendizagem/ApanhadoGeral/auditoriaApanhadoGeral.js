import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { Auditoria } from '~/componentes';

const AuditoriaApanhadoGeral = () => {
  const dadosApanhadoGeral = useSelector(
    store => store.acompanhamentoAprendizagem.dadosApanhadoGeral
  );

  const [dadosAuditoria, setDadosAuditoria] = useState({});

  useEffect(() => {
    if (
      dadosApanhadoGeral?.auditoria?.id &&
      dadosAuditoria !== dadosApanhadoGeral?.auditoria
    ) {
      setDadosAuditoria();
      setDadosAuditoria(dadosApanhadoGeral?.auditoria);
    }
  }, [dadosApanhadoGeral, dadosAuditoria]);

  return dadosAuditoria ? (
    <div className="row">
      <Auditoria
        alteradoEm={dadosAuditoria?.alteradoEm}
        alteradoPor={dadosAuditoria?.alteradoPor}
        alteradoRf={dadosAuditoria?.alteradoRF}
        criadoEm={dadosAuditoria?.criadoEm}
        criadoPor={dadosAuditoria?.criadoPor}
        criadoRf={dadosAuditoria?.criadoRF}
      />
    </div>
  ) : (
    ''
  );
};

export default AuditoriaApanhadoGeral;
