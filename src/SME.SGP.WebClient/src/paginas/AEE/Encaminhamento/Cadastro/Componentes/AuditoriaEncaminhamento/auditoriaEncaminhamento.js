import PropTypes from 'prop-types';
import React from 'react';
import { Auditoria } from '~/componentes';

const AuditoriaEncaminhamento = props => {
  const { dadosAuditoria } = props;

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

AuditoriaEncaminhamento.propTypes = {
  dadosAuditoria: PropTypes.oneOfType([PropTypes.any]),
};

AuditoriaEncaminhamento.defaultProps = {
  dadosAuditoria: null,
};

export default AuditoriaEncaminhamento;
