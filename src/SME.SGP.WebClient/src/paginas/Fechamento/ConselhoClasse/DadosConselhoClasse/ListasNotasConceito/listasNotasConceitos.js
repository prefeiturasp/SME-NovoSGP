import PropTypes from 'prop-types';
import React, { useCallback, useEffect } from 'react';
import { useSelector } from 'react-redux';
// import ListaBimestre from './ListaBimestre/listaBimestre';
import ListaNotasConselho from '../ListaNotasConselho/listaNotasConselho';
import ListaFinal from './ListaFinal/listaFinal';

const ListasNotasConceitos = props => {
  const { bimestreSelecionado } = props;

  const dadosPrincipaisConselhoClasse = useSelector(
    store => store.conselhoClasse.dadosPrincipaisConselhoClasse
  );

  const obterDadosLista = useCallback(async () => {
    console.log('START: obterDadosLista');

    // TODO Fazer a consulta para trazer as notas e conceitos aba bimestre e final!
    // Quando for aba final passar bimestre = 0!
    // Quando a consulta retornar vai ter o periodo fim para consultar a lista de conceitos
  }, []);

  useEffect(() => {
    const {
      turmaCodigo,
      fechamentoTurmaId,
      conselhoClasseId,
      alunoCodigo,
    } = dadosPrincipaisConselhoClasse;

    const bimestre = bimestreSelecionado.valor;

    const ehFinal = bimestreSelecionado.valor === 'final';
    if (bimestre && turmaCodigo && fechamentoTurmaId && alunoCodigo) {
      obterDadosLista(ehFinal);
    }

    console.log(`bimestre: ${bimestre}`);
    console.log(`turmaCodigo: ${turmaCodigo}`);
    console.log(`fechamentoTurmaId: ${fechamentoTurmaId}`);
    console.log(`conselhoClasseId: ${conselhoClasseId}`);
    console.log(`alunoCodigo: ${alunoCodigo}`);
    console.log(`ehFinal: ${ehFinal}`);
  }, [bimestreSelecionado, dadosPrincipaisConselhoClasse, obterDadosLista]);

  const validaExibirLista = () => {
    const ehFinal = bimestreSelecionado.valor === 'final';
    if (ehFinal) {
      return <ListaFinal />;
    }
    return <ListaNotasConselho />;
    // TODO Adicioanr os fontes no ListaBimestre!
    // return <ListaBimestre />;
  };

  return <>{validaExibirLista()}</>;
};

ListasNotasConceitos.propTypes = {
  bimestreSelecionado: PropTypes.oneOfType([PropTypes.object]),
};

ListasNotasConceitos.defaultProps = {
  bimestreSelecionado: {},
};

export default ListasNotasConceitos;
