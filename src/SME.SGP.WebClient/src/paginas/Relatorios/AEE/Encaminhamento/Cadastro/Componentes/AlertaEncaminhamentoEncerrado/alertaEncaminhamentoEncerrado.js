import React from 'react';
import { useSelector } from 'react-redux';
import Alert from '~/componentes/alert';
import situacaoAEE from '~/dtos/situacaoAEE';

const AlertaEncaminhamentoEncerrado = () => {
  const dadosEncaminhamento = useSelector(
    store => store.encaminhamentoAEE.dadosEncaminhamento
  );

  return (
    <div className="col-md-12">
      {dadosEncaminhamento?.situacao === situacaoAEE.Encerrado ? (
        <Alert
          alerta={{
            tipo: 'warning',
            id: 'alerta',
            mensagem: 'Encaminhamento encerrado pelo CP',
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

export default AlertaEncaminhamentoEncerrado;
