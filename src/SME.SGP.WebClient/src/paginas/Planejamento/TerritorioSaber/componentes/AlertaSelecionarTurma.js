import React from 'react';

// Redux
import { useSelector } from 'react-redux';

// Componentes
import { Alert } from '~/componentes';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';

function AlertaSelecionarTurma() {
  const { turmaSelecionada } = useSelector(state => state.usuario);
  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  return !turmaSelecionada.turma &&
    !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) ? (
    <Alert
      alerta={{
        tipo: 'warning',
        id: 'plano-ciclo-selecione-turma',
        mensagem: 'Você precisa escolher uma turma.',
        estiloTitulo: { fontSize: '18px' },
      }}
      className="mb-4"
    />
  ) : (
    ''
  );
}

export default AlertaSelecionarTurma;
