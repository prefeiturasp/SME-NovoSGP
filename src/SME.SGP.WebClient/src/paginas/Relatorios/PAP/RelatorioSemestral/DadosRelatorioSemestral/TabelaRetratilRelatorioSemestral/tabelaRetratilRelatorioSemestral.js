import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import TabelaRetratil from '~/componentes/TabelaRetratil';

const TabelaRetratilRelatorioSemestral = ({
  onChangeAlunoSelecionado,
  children,
  permiteOnChangeAluno,
}) => {
  const alunosRelatorioSemestral = useSelector(
    store => store.relatorioSemestralPAP.alunosRelatorioSemestral
  );

  const codigoAlunoSelecionado = useSelector(
    store => store.relatorioSemestralPAP.codigoAlunoSelecionado
  );

  return (
    <>
      {alunosRelatorioSemestral && alunosRelatorioSemestral.length ? (
        <TabelaRetratil
          onChangeAlunoSelecionado={onChangeAlunoSelecionado}
          permiteOnChangeAluno={permiteOnChangeAluno}
          alunos={alunosRelatorioSemestral}
          codigoAlunoSelecionado={codigoAlunoSelecionado}
          exibirProcessoConcluido
        >
          {children}
        </TabelaRetratil>
      ) : (
        ''
      )}
    </>
  );
};

TabelaRetratilRelatorioSemestral.propTypes = {
  onChangeAlunoSelecionado: PropTypes.func,
  children: PropTypes.oneOfType([PropTypes.element, PropTypes.func]),
  permiteOnChangeAluno: PropTypes.func,
};

TabelaRetratilRelatorioSemestral.defaultProps = {
  onChangeAlunoSelecionado: () => {},
  children: () => {},
  permiteOnChangeAluno: null,
};

export default TabelaRetratilRelatorioSemestral;
