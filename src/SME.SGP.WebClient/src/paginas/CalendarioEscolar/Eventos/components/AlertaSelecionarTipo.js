import React from 'react';
import t from 'prop-types';

// Componentes
import { Alert, Grid } from '~/componentes';

function AlertaSelecionarTipo({ filtro }) {
  return !filtro.tipoCalendarioId ? (
    <Grid cols={12} className="mb-3">
      <Alert
        alerta={{
          tipo: 'warning',
          id: 'AlertaPrincipal',
          mensagem:
            'Para cadastrar ou listar eventos você precisa selecionar um tipo de calendário',
        }}
        className="mb-0"
      />
    </Grid>
  ) : null;
}

AlertaSelecionarTipo.propTypes = {
  filtro: t.oneOfType([t.any]),
};

AlertaSelecionarTipo.defaultProps = {
  filtro: {},
};

export default AlertaSelecionarTipo;
