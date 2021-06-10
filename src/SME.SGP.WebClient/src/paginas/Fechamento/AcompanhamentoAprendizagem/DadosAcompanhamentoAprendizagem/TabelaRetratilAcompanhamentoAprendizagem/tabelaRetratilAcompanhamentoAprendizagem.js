import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import TabelaRetratil from '~/componentes/TabelaRetratil';

const TabelaRetratilAcompanhamentoAprendizagem = ({
  onChangeAlunoSelecionado,
  permiteOnChangeAluno,
  children,
}) => {
  const alunosAcompanhamentoAprendizagem = useSelector(
    store => store.acompanhamentoAprendizagem.alunosAcompanhamentoAprendizagem
  );

  const codigoAlunoSelecionado = useSelector(
    store => store.acompanhamentoAprendizagem.codigoAlunoSelecionado
  );

  return (
    <>
      {alunosAcompanhamentoAprendizagem?.length ? (
        <TabelaRetratil
          onChangeAlunoSelecionado={onChangeAlunoSelecionado}
          permiteOnChangeAluno={permiteOnChangeAluno}
          alunos={alunosAcompanhamentoAprendizagem}
          codigoAlunoSelecionado={codigoAlunoSelecionado}
          pularDesabilitados
          larguraAluno="60%"
        >
          {children}
        </TabelaRetratil>
      ) : (
        ''
      )}
    </>
  );
};

TabelaRetratilAcompanhamentoAprendizagem.propTypes = {
  onChangeAlunoSelecionado: PropTypes.func,
  permiteOnChangeAluno: PropTypes.func,
  children: PropTypes.oneOfType([PropTypes.any]),
};

TabelaRetratilAcompanhamentoAprendizagem.defaultProps = {
  onChangeAlunoSelecionado: () => {},
  permiteOnChangeAluno: () => {},
  children: () => {},
};

export default TabelaRetratilAcompanhamentoAprendizagem;
