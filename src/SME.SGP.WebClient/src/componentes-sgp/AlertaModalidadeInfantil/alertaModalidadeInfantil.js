import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import Alert from '~/componentes/alert';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';

const AlertaModalidadeInfantil = props => {
  const { turmaSelecionada } = useSelector(store => store.usuario);

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const {
    exibir,
    validarModalidadeFiltroPrincipal,
    naoPermiteTurmaInfantil,
  } = props;

  const [exibirMsg, setExibirMsg] = useState(exibir);

  useEffect(() => {
    if (validarModalidadeFiltroPrincipal) {
      const turmaInfantil = ehTurmaInfantil(
        modalidadesFiltroPrincipal,
        turmaSelecionada
      );
      if (naoPermiteTurmaInfantil) {
        setExibirMsg(turmaInfantil);
      } else {
        setExibirMsg(!turmaInfantil);
      }
    } else {
      setExibirMsg(exibir);
    }
  }, [
    turmaSelecionada,
    exibir,
    validarModalidadeFiltroPrincipal,
    modalidadesFiltroPrincipal,
    naoPermiteTurmaInfantil,
  ]);

  return (
    <div className="col-md-12">
      {exibirMsg ? (
        <Alert
          alerta={{
            tipo: 'warning',
            id: 'alerta-modalidade-infantil',
            mensagem: `Esta interface ${
              naoPermiteTurmaInfantil
                ? 'não está disponível'
                : 'só pode ser utilizada'
            } para turmas da educação infantil`,
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
  naoPermiteTurmaInfantil: PropTypes.bool,
};

AlertaModalidadeInfantil.defaultProps = {
  exibir: false,
  validarModalidadeFiltroPrincipal: true,
  naoPermiteTurmaInfantil: true,
};

export default AlertaModalidadeInfantil;
