import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import TabelaRetratil from '~/componentes/TabelaRetratil';

const TabelaRetratilAcompanhamentoAprendizagem = ({
  onChangeAlunoSelecionado,
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
          alunos={alunosAcompanhamentoAprendizagem}
          codigoAlunoSelecionado={codigoAlunoSelecionado}
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
  children: PropTypes.oneOfType([PropTypes.any]),
};

TabelaRetratilAcompanhamentoAprendizagem.defaultProps = {
  onChangeAlunoSelecionado: () => {},
  children: () => {},
};

export default TabelaRetratilAcompanhamentoAprendizagem;
