import PropTypes from 'prop-types';
import React, { useCallback, useEffect } from 'react';
import ListaFinal from './ListaFinal/listaFinal';
// import ListaBimestre from './ListaBimestre/listaBimestre';
import ListaNotasConselho from '../ListaNotasConselho/listaNotasConselho';

const ListasNotasConceitos = props => {
  const { bimestreSelecionado, codigoEOL, codigoTurma } = props;

  const obterDadosLista = useCallback(async (turma, codigoAluno) => {
    console.log(`Turma: ${turma}`);
    console.log(`Aluno: ${codigoAluno}`);

    // TODO Fazer a consulta para trazer as notas e conceitos aba bimestre e final!
    // Quando for aba final passar bimestre = 0!
    // Quando a consulta retornar vai ter o periodo fim para consultar a lista de conceitos
  }, []);

  useEffect(() => {
    if (codigoTurma && codigoEOL) {
      const ehFinal = bimestreSelecionado.valor === 'final';
      obterDadosLista(codigoTurma, codigoEOL, ehFinal);
    }
  }, [codigoEOL, codigoTurma, obterDadosLista, bimestreSelecionado]);

  const validaExibirLista = () => {
    const ehFinal = bimestreSelecionado.valor === 'final';
    if (ehFinal) {
      return <ListaFinal />;
    }
    return <ListaNotasConselho bimestreSelecionado={bimestreSelecionado} />;
    // TODO Adicioanr os fontes no ListaBimestre!
    // return <ListaBimestre />;
  };

  return <>{validaExibirLista()}</>;
};

ListasNotasConceitos.propTypes = {
  bimestreSelecionado: PropTypes.string,
  codigoTurma: PropTypes.string,
  codigoEOL: PropTypes.string,
};

ListasNotasConceitos.defaultProps = {
  bimestreSelecionado: '',
  codigoTurma: '',
  codigoEOL: '',
};

export default ListasNotasConceitos;
