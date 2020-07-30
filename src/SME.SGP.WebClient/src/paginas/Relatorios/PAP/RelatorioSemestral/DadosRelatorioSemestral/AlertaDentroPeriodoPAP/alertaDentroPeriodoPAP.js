import React from 'react';
import { useSelector } from 'react-redux';
import Alert from '~/componentes/alert';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';

const AlertaDentroPeriodoPAP = () => {
  const dentroPeriodo = useSelector(
    store => store.relatorioSemestralPAP.dentroPeriodo
  );
  const { turmaSelecionada } = useSelector(store => store.usuario);
  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  return (
    <div className="col-md-12">
      {!dentroPeriodo &&
      turmaSelecionada &&
      turmaSelecionada.turma &&
      !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) ? (
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
