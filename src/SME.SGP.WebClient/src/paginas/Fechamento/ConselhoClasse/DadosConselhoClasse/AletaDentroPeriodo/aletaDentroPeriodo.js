import React from 'react';
import { useSelector } from 'react-redux';

import Alert from '~/componentes/alert';

const AletaDentroPeriodo = () => {
  const dentroPeriodo = useSelector(
    store => store.conselhoClasse.dentroPeriodo
  );

  return (
    <div className="col-md-12">
      {!dentroPeriodo ? (
        <Alert
          alerta={{
            tipo: 'warning',
            id: 'alerta-perido-fechamento',
            mensagem:
              'Apenas é possível consultar este registro pois o período de fechamento deste bimestre está encerrado.',
            estiloTitulo: { fontSize: '18px' },
          }}
          className="mb-2"
        />
      ) : (
        ''
      )}
    </div>
  );
};

export default AletaDentroPeriodo;
