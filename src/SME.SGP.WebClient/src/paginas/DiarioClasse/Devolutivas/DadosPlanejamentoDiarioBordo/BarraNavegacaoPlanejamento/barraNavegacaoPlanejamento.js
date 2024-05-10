import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { BarraNavegacao } from '~/componentes';
import {
  setPaginaAtiva,
  setPlanejamentoExpandido,
  setPlanejamentoSelecionado,
} from '~/redux/modulos/devolutivas/actions';

const BarraNavegacaoPlanejamento = React.memo(props => {
  const { onChangePage } = props;

  const dadosPlanejamentos = useSelector(
    store => store.devolutivas.dadosPlanejamentos
  );

  const paginaAtiva = useSelector(store => store.devolutivas.paginaAtiva);

  const dispatch = useDispatch();
  const [paginas, setPaginas] = useState([]);

  const montarPaginas = useCallback(() => {
    const itens = [];
    for (let index = 0; index < dadosPlanejamentos.totalPaginas; index += 1) {
      itens.push({ id: index + 1 });
    }
    setPaginas(itens);
  }, [dadosPlanejamentos]);

  useEffect(() => {
    if (dadosPlanejamentos && dadosPlanejamentos.totalPaginas) {
      montarPaginas();
    }
  }, [dadosPlanejamentos, montarPaginas]);

  const onChangeItem = useCallback(
    pagina => {
      setPaginaAtiva(pagina);
      dispatch(setPaginaAtiva(pagina));
      onChangePage(pagina.id);
      dispatch(setPlanejamentoExpandido(false));
      dispatch(setPlanejamentoSelecionado([]));
    },
    [onChangePage, dispatch]
  );

  return (
    <BarraNavegacao
      itens={paginas}
      itemAtivo={
        paginaAtiva
          ? paginas.find(pagina => pagina.id === paginaAtiva.id)
          : paginas[0]
      }
      onChangeItem={onChangeItem}
    />
  );
});

BarraNavegacaoPlanejamento.propTypes = {
  onChangePage: PropTypes.func,
};

BarraNavegacaoPlanejamento.defaultProps = {
  onChangePage: () => {},
};

export default BarraNavegacaoPlanejamento;
