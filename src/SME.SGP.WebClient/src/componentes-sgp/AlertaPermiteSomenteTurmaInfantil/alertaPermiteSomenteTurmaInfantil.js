import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import Alert from '~/componentes/alert';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';

const AlertaPermiteSomenteTurmaInfantil = props => {
  const { turmaSelecionada } = useSelector(store => store.usuario);

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const { exibir, validarModalidadeFiltroPrincipal, marginBottom } = props;

  const [exibirMsg, setExibirMsg] = useState(exibir);

  useEffect(() => {
    if (
      validarModalidadeFiltroPrincipal &&
      !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada)
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
              'Esta interface só pode ser utilizada para turmas da educação infantil',
            estiloTitulo: { fontSize: '18px' },
          }}
          className={`mb-${marginBottom}`}
        />
      ) : (
        ''
      )}
    </div>
  );
};

AlertaPermiteSomenteTurmaInfantil.propTypes = {
  exibir: PropTypes.bool,
  validarModalidadeFiltroPrincipal: PropTypes.bool,
  marginBottom: PropTypes.number,
};

AlertaPermiteSomenteTurmaInfantil.defaultProps = {
  exibir: false,
  validarModalidadeFiltroPrincipal: true,
  marginBottom: 2,
};

export default AlertaPermiteSomenteTurmaInfantil;
