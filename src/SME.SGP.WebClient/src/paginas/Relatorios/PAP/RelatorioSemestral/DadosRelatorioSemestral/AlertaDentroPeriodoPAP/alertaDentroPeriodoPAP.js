import React from 'react';
import { useSelector } from 'react-redux';
import Alert from '~/componentes/alert';

const AlertaDentroPeriodoPAP = () => {
  const dentroPeriodo = useSelector(
    store => store.relatorioSemestral.dentroPeriodo
  );

  return (
    <div className="col-md-12">
      {!dentroPeriodo ? (
        <Alert
          alerta={{
            tipo: 'warning',
            id: 'alerta-perido-fechamento',
            mensagem:
              'Não é possível preencher o relatório fora do período estipulado pela SME',
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

export default AlertaDentroPeriodoPAP;
