import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import Alert from '~/componentes/alert';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';

const AlertaNaoPermiteTurmaInfantil = props => {
  const { turmaSelecionada } = useSelector(store => store.usuario);

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const { exibir, validarModalidadeFiltroPrincipal } = props;

  const [exibirMsg, setExibirMsg] = useState(exibir);

  useEffect(() => {
    if (
      validarModalidadeFiltroPrincipal &&
      ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada)
    ) {
      setExibirMsg(true);
    } else {
      setExibirMsg(exibir);
    }
  }, [
    turmaSelecionada,
    exibir,
    validarModalidadeFiltroPrincipal,
    modalidadesFiltroPrincipal,
  ]);

  return (
    <div className="col-md-12">
      {exibirMsg ? (
        <Alert
          alerta={{
            tipo: 'warning',
            id: 'alerta-modalidade-infantil',
            mensagem:
              'Esta interface não está disponível para turmas de Educação Infantil',
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

AlertaNaoPermiteTurmaInfantil.propTypes = {
  exibir: PropTypes.bool,
  validarModalidadeFiltroPrincipal: PropTypes.bool,
};

AlertaNaoPermiteTurmaInfantil.defaultProps = {
  exibir: false,
  validarModalidadeFiltroPrincipal: true,
};

export default AlertaNaoPermiteTurmaInfantil;
