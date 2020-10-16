import React from 'react';
import { useSelector } from 'react-redux';
import Alert from '~/componentes/alert';

const AlertaDentroPeriodoFrequenciaPlanoAula = () => {
  const temPeriodoAberto = useSelector(
    state => state.frequenciaPlanoAula.temPeriodoAberto
  );

  const componenteCurricular = useSelector(
    state => state.frequenciaPlanoAula.componenteCurricular
  );

  const dataSelecionada = useSelector(
    state => state.frequenciaPlanoAula.dataSelecionada
  );

  const { turmaSelecionada } = useSelector(store => store.usuario);

  return (
    <div className="col-md-12">
      {turmaSelecionada &&
      turmaSelecionada.turma &&
      componenteCurricular &&
      dataSelecionada &&
      !temPeriodoAberto ? (
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

export default AlertaDentroPeriodoFrequenciaPlanoAula;
