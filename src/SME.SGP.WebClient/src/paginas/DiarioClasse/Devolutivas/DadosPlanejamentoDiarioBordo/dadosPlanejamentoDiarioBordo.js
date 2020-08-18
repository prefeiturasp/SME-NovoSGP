import React, { useState, useEffect, useCallback } from 'react';
import { useSelector } from 'react-redux';
import CardPlanejamento from '~/paginas/DiarioClasse/Devolutivas/DadosPlanejamentoDiarioBordo/CardPlanejamento/cardPlanejamento';
import { BarraNavegacao } from '~/componentes';

const DadosPlanejamentoDiarioBordo = props => {
  const { onChangePage } = props;

  const dadosPlanejamentos = useSelector(
    store => store.devolutivas.dadosPlanejamentos
  );

  const [paginaAtiva, setPaginaAtiva] = useState();
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

  const onChangeItem = pagina => {
    setPaginaAtiva(pagina);
    onChangePage(pagina.id);
  };

  return (
    <>
      {dadosPlanejamentos && paginas && paginas.length ? (
        <>
          <BarraNavegacao
            itens={paginas}
            itemAtivo={
              paginaAtiva
                ? paginas.find(pagina => pagina.id === paginaAtiva.id)
                : paginas[0]
            }
            onChangeItem={onChangeItem}
          />

          <CardPlanejamento />
        </>
      ) : (
        ''
      )}
    </>
  );
};

export default DadosPlanejamentoDiarioBordo;
