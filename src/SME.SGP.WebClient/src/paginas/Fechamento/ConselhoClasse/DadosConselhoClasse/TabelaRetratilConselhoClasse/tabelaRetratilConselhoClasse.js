import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import TabelaRetratil from '~/componentes/TabelaRetratil';

const TabelaRetratilConselhoClasse = ({
  onChangeAlunoSelecionado,
  children,
  permiteOnChangeAluno,
}) => {
  const alunosConselhoClasse = useSelector(
    store => store.conselhoClasse.alunosConselhoClasse
  );

  return (
    <>
      {alunosConselhoClasse && alunosConselhoClasse.length ? (
        <TabelaRetratil
          onChangeAlunoSelecionado={onChangeAlunoSelecionado}
          permiteOnChangeAluno={permiteOnChangeAluno}
          alunos={alunosConselhoClasse}
        >
          {children}
        </TabelaRetratil>
      ) : (
        ''
      )}
    </>
  );
};

TabelaRetratilConselhoClasse.propTypes = {
  onChangeAlunoSelecionado: PropTypes.func,
  children: PropTypes.oneOfType([PropTypes.element, PropTypes.func]),
  permiteOnChangeAluno: PropTypes.func,
};

TabelaRetratilConselhoClasse.defaultProps = {
  onChangeAlunoSelecionado: () => {},
  children: () => {},
  permiteOnChangeAluno: null,
};

export default TabelaRetratilConselhoClasse;
