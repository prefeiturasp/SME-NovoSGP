import React from 'react';
import { useSelector } from 'react-redux';
import { Auditoria } from '~/componentes';

const AuditoriaEncaminhamento = () => {
  const dadosEncaminhamento = useSelector(
    store => store.encaminhamentoAEE.dadosEncaminhamento
  );

  return dadosEncaminhamento?.auditoria?.id > 0 ? (
    <Auditoria
      className="mt-2"
      alteradoEm={dadosEncaminhamento?.auditoria.alteradoEm}
      alteradoPor={dadosEncaminhamento?.auditoria.alteradoPor}
      alteradoRf={dadosEncaminhamento?.auditoria.alteradoRf}
      criadoEm={dadosEncaminhamento?.auditoria.criadoEm}
      criadoPor={dadosEncaminhamento?.auditoria.criadoPor}
      criadoRf={dadosEncaminhamento?.auditoria.criadoRf}
    />
  ) : (
    ''
  );
};

export default AuditoriaEncaminhamento;
