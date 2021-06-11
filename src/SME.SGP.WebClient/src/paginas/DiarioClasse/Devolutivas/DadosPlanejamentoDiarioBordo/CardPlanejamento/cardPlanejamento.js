import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { SelectComponent } from '~/componentes';
import { MontarEditor } from './componentes';
import {
  setAlterouCaixaSelecao,
  setNumeroRegistros,
  setPaginaAtiva,
  setPlanejamentoExpandido,
  setPlanejamentoSelecionado,
} from '~/redux/modulos/devolutivas/actions';

const CardPlanejamento = React.memo(() => {
  const dadosPlanejamentos = useSelector(
    store => store.devolutivas.dadosPlanejamentos
  );
  const alterouCaixaSelecao = useSelector(
    store => store.devolutivas.alterouCaixaSelecao
  );

  const listaRegistros = [
    {
      valor: 1,
      descricao: '1 registro por página',
    },
    {
      valor: 2,
      descricao: '2 registros por página',
    },
    {
      valor: 3,
      descricao: '3 registros por página',
    },
    {
      valor: 4,
      descricao: '4 registros por página',
    },
  ];

  const [totalRegistrosSelecionado, setTotalRegistrosSelecionado] = useState(
    '4'
  );
  const [listaRegistrosPorPaginas, setListaRegistrosPorPaginas] = useState(
    listaRegistros
  );

  const dispatch = useDispatch();

  const onChangeTotalPaginas = valor => {
    if (valor) {
      dispatch(setAlterouCaixaSelecao(true));
      setTotalRegistrosSelecionado(valor);
      dispatch(setNumeroRegistros(valor));
      dispatch(setPaginaAtiva(null));
      dispatch(setPlanejamentoSelecionado([]));
      dispatch(setPlanejamentoExpandido(false));
    }
  };

  useEffect(() => {
    if (dadosPlanejamentos?.totalRegistros < 4 && !alterouCaixaSelecao) {
      const { totalRegistros } = dadosPlanejamentos;
      const novaLista = listaRegistros.filter(
        ({ valor }) => valor <= totalRegistros
      );
      setTotalRegistrosSelecionado(String(totalRegistros));
      setListaRegistrosPorPaginas(novaLista);
    }
    if (dadosPlanejamentos?.totalRegistros >= 4 && !alterouCaixaSelecao) {
      setTotalRegistrosSelecionado('4');
      setListaRegistrosPorPaginas(listaRegistros);
    }
  }, [dadosPlanejamentos, alterouCaixaSelecao]);

  return (
    <div style={{ border: '1px solid #DADADA' }}>
      {dadosPlanejamentos?.items?.length ? (
        <>
          <div className="col-md-3 col-sm-12 col-xl-3 col-lg-3 mt-3">
            <SelectComponent
              id="registrosPorPagina"
              lista={listaRegistrosPorPaginas}
              valueOption="valor"
              valueText="descricao"
              onChange={onChangeTotalPaginas}
              valueSelect={totalRegistrosSelecionado}
            />
          </div>
          <div className="row mt-3 p-3">
            <MontarEditor />
          </div>
        </>
      ) : (
        <div className="text-center p-2"> Sem dados</div>
      )}
    </div>
  );
});

export default CardPlanejamento;
