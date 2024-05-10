import React from 'react';

// Redux
import { useSelector } from 'react-redux';

// Componentes
import { Alert } from '~/componentes';

function AlertaSelecionarTurma() {
  const { turmaSelecionada } = useSelector(state => state.usuario);
  return turmaSelecionada && turmaSelecionada.turma ? (
    ''
  ) : (
    <Alert
      alerta={{
        tipo: 'warning',
        id: 'plano-ciclo-selecione-turma',
        mensagem: 'Você precisa escolher uma turma.',
        estiloTitulo: { fontSize: '18px' },
      }}
      className="mb-4"
    />
  );
}

export default AlertaSelecionarTurma;
