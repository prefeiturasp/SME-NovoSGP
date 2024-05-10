import React from 'react';
import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';

import { TabelaRetratil } from '~/componentes';

const TabelaRetratilRegistroIndividual = ({
  onChangeAlunoSelecionado,
  children,
  permiteOnChangeAluno,
}) => {
  const alunos = useSelector(
    store => store.registroIndividual.alunosRegistroIndividual
  );

  return (
    <>
      {!!alunos?.length && (
        <TabelaRetratil
          onChangeAlunoSelecionado={onChangeAlunoSelecionado}
          permiteOnChangeAluno={permiteOnChangeAluno}
          alunos={alunos}
          tituloCabecalho="Detalhes da criança"
          pularDesabilitados
        >
          {children}
        </TabelaRetratil>
      )}
    </>
  );
};

TabelaRetratilRegistroIndividual.propTypes = {
  onChangeAlunoSelecionado: PropTypes.func,
  children: PropTypes.oneOfType([PropTypes.element, PropTypes.func]),
  permiteOnChangeAluno: PropTypes.func,
};

TabelaRetratilRegistroIndividual.defaultProps = {
  onChangeAlunoSelecionado: () => {},
  children: () => {},
  permiteOnChangeAluno: null,
};

export default TabelaRetratilRegistroIndividual;
