import React from 'react';
import { useSelector } from 'react-redux';
import Alert from '~/componentes/alert';
import modalidade from '~/dtos/modalidade';

const AlertaDentroPeriodo = () => {
  const dentroPeriodo = useSelector(
    store => store.conselhoClasse.dentroPeriodo
  );
  const { turmaSelecionada } = useSelector(store => store.usuario);

  return (
    <div className="col-md-12">
      {!dentroPeriodo &&
      turmaSelecionada &&
      turmaSelecionada.turma &&
      String(turmaSelecionada.modalidade) !== String(modalidade.INFANTIL) ? (
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
      ) : (
        ''
      )}
    </div>
  );
};

export default AlertaDentroPeriodo;
