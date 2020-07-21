import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import Alert from '~/componentes/alert';
import modalidade from '~/dtos/modalidade';

const AlertaModalidadeInfantil = props => {
  const { turmaSelecionada } = useSelector(store => store.usuario);

  const { exibir, validarModalidadeFiltroPrincipal } = props;

  const [exibirMsg, setExibirMsg] = useState(exibir);

  useEffect(() => {
    if (
      validarModalidadeFiltroPrincipal &&
      turmaSelecionada &&
      turmaSelecionada.turma &&
      String(turmaSelecionada.modalidade) === String(modalidade.INFANTIL)
    ) {
      setExibirMsg(true);
    } else {
      setExibirMsg(exibir);
    }
  }, [turmaSelecionada, exibir, validarModalidadeFiltroPrincipal]);

  return (
    <div className="col-md-12">
      {exibirMsg ? (
        <Alert
          alerta={{
            tipo: 'warning',
            id: 'alerta-modalidade-infantil',
            mensagem:
              'Esta tela não está disponível para turmas de Educação Infantil',
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

AlertaModalidadeInfantil.propTypes = {
  exibir: PropTypes.bool,
  validarModalidadeFiltroPrincipal: PropTypes.bool,
};

AlertaModalidadeInfantil.defaultProps = {
  exibir: false,
  validarModalidadeFiltroPrincipal: true,
};

export default AlertaModalidadeInfantil;
