import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import TransferenciaLista from '~/componentes-sgp/TranferenciaLista/transferenciaLista';
import ServicoPlanoAnual from '~/servicos/Paginas/ServicoPlanoAnual';
import { ContainerListaObjetivos } from './listaObjetivos.css';

const ListaObjetivos = props => {
  const { tabAtualComponenteCurricular } = props;

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const [dadosEsquerda, setDadosEsquerda] = useState([]);
  const [dadosDireita, setDadosDireita] = useState([]);
  const [idsSelecionadsEsquerda, setIdsSelecionadsEsquerda] = useState([]);
  const [idsSelecionadsDireita, setIdsSelecionadsDireita] = useState([]);

  const obterObjetivosPorAnoEComponenteCurricular = useCallback(() => {
    if (
      tabAtualComponenteCurricular &&
      tabAtualComponenteCurricular.codigoComponenteCurricular
    ) {
      // TODO LOADER!
      ServicoPlanoAnual.obterObjetivosPorAnoEComponenteCurricular(
        turmaSelecionada.ano,
        turmaSelecionada.ensinoEspecial,
        [tabAtualComponenteCurricular.codigoComponenteCurricular]
      )
        .then(resposta => {
          setDadosEsquerda(resposta.data);
          // TODO LOADER!
        })
        .catch(e => {
          // mostrarErros(e);
        })
        .finally(() => {
          // TODO LOADER!
        });
    } else {
      // TODO LOADER!
    }
  }, [tabAtualComponenteCurricular, turmaSelecionada]);

  useEffect(() => {
    if (tabAtualComponenteCurricular) {
      obterObjetivosPorAnoEComponenteCurricular();
    }
  }, [tabAtualComponenteCurricular, obterObjetivosPorAnoEComponenteCurricular]);

  const parametrosListaEsquerda = {
    title: 'Objetivos de aprendizagem',
    columns: [
      {
        title: 'Código',
        dataIndex: 'codigo',
        className: 'desc-codigo',
        width: '90px',
      },
      {
        title: 'Descrição',
        dataIndex: 'descricao',
        className: 'desc-descricao',
      },
    ],
    dataSource: dadosEsquerda,
    onSelectRow: setIdsSelecionadsEsquerda,
    selectedRowKeys: idsSelecionadsEsquerda,
  };

  const parametrosListaDireita = {
    title:
      'Você precisa selecionar ao menos um objetivo para poder inserir a descrição do plano',
    columns: [
      {
        title: 'Código',
        dataIndex: 'codigo',
        className: 'desc-codigo fundo-codigo-lista-direita',
        width: '90px',
      },
      {
        title: 'Descrição',
        dataIndex: 'descricao',
        className: 'desc-descricao',
      },
    ],
    dataSource: dadosDireita,
    onSelectRow: setIdsSelecionadsDireita,
    selectedRowKeys: idsSelecionadsDireita,
  };

  const obterListaComIdsSelecionados = (list, ids) => {
    return list.filter(item => ids.find(id => String(id) === String(item.id)));
  };

  const obterListaSemIdsSelecionados = (list, ids) => {
    return list.filter(item => !ids.find(id => String(id) === String(item.id)));
  };

  const onClickAdicionar = () => {
    if (idsSelecionadsEsquerda && idsSelecionadsEsquerda.length) {
      const novaListaDireita = obterListaComIdsSelecionados(
        dadosEsquerda,
        idsSelecionadsEsquerda
      );

      const novaListaEsquerda = obterListaSemIdsSelecionados(
        dadosEsquerda,
        idsSelecionadsEsquerda
      );

      setDadosEsquerda([...novaListaEsquerda]);
      setDadosDireita([...novaListaDireita, ...dadosDireita]);
      setIdsSelecionadsEsquerda([]);
    }
  };

  const onClickRemover = async () => {
    if (idsSelecionadsDireita && idsSelecionadsDireita.length) {
      const novaListaEsquerda = obterListaComIdsSelecionados(
        dadosDireita,
        idsSelecionadsDireita
      );

      const novaListaDireita = obterListaSemIdsSelecionados(
        dadosDireita,
        idsSelecionadsDireita
      );

      setDadosEsquerda([...novaListaEsquerda, ...dadosEsquerda]);
      setDadosDireita([...novaListaDireita]);
      setIdsSelecionadsDireita([]);
    }
  };

  return (
    <ContainerListaObjetivos>
      <TransferenciaLista
        listaEsquerda={parametrosListaEsquerda}
        listaDireita={parametrosListaDireita}
        onClickAdicionar={onClickAdicionar}
        onClickRemover={onClickRemover}
      />
    </ContainerListaObjetivos>
  );
};

ListaObjetivos.propTypes = {
  tabAtualComponenteCurricular: PropTypes.oneOfType([PropTypes.object]),
};

ListaObjetivos.defaultProps = {
  tabAtualComponenteCurricular: {},
};

export default ListaObjetivos;
