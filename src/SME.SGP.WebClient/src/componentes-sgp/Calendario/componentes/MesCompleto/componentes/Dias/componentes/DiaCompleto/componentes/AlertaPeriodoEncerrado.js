import React from 'react';
import t from 'prop-types';

// Componentes
import { Alert } from '~/componentes';

function AlertaPeriodoEncerrado({ exibir }) {
  return (
    exibir && (
      <Alert
        alerta={{
          tipo: 'warning',
          id: 'alerta-perido-fechamento',
          mensagem:
            'Apenas é possível consultar este registro pois o período não está em aberto.',
          estiloTitulo: { fontSize: '18px' },
        }}
        className="mb-2"
      />
    )
  );
}

AlertaPeriodoEncerrado.propTypes = {
  exibir: t.bool,
};

AlertaPeriodoEncerrado.defaultProps = {
  exibir: false,
};

export default AlertaPeriodoEncerrado;
