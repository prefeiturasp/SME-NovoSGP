import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import TransferenciaLista from '~/componentes-sgp/TranferenciaLista/transferenciaLista';
import ServicoPlanoAnual from '~/servicos/Paginas/ServicoPlanoAnual';
import { ContainerListaObjetivos } from './listaObjetivos.css';

const ListaObjetivos = props => {
  const { dadosBimestre, tabAtualComponenteCurricular } = props;
  const { bimestre } = dadosBimestre;

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const dadosBimestrePlanoAnual = useSelector(
    store => store.planoAnual.dadosBimestresPlanoAnual[bimestre]
  );

  const [dadosEsquerda, setDadosEsquerda] = useState([]);
  const [dadosDireita, setDadosDireita] = useState([]);
  const [idsSelecionadsEsquerda, setIdsSelecionadsEsquerda] = useState([]);
  const [idsSelecionadsDireita, setIdsSelecionadsDireita] = useState([]);

  const obterDadosComponenteAtual = useCallback(() => {
    return dadosBimestrePlanoAnual?.componentes.find(
      item =>
        String(item.componenteCurricularId) ===
        String(tabAtualComponenteCurricular.codigoComponenteCurricular)
    );
  }, [tabAtualComponenteCurricular, dadosBimestrePlanoAnual]);

  const montarDadosListasObjetivos = useCallback(
    listaObjetivos => {
      const dadosComponenteAtual = obterDadosComponenteAtual();

      if (dadosComponenteAtual.objetivosAprendizagemId.length) {
        const listaEsquerda = [];
        const listaDireita = [];
        listaObjetivos.forEach(objetivo => {
          if (
            dadosComponenteAtual.objetivosAprendizagemId.find(
              objetivoId => objetivoId === objetivo.id
            )
          ) {
            listaDireita.push(objetivo);
          } else {
            listaEsquerda.push(objetivo);
          }
        });

        if (listaEsquerda.length) {
          setDadosEsquerda(listaEsquerda);
        }
        if (listaDireita.length) {
          setDadosDireita(listaDireita);
        }
      }
    },
    [obterDadosComponenteAtual]
  );

  const obterObjetivosPorAnoEComponenteCurricular = useCallback(() => {
    if (
      tabAtualComponenteCurricular &&
      tabAtualComponenteCurricular.codigoComponenteCurricular &&
      obterDadosComponenteAtual()
    ) {
      // TODO LOADER!
      ServicoPlanoAnual.obterObjetivosPorAnoEComponenteCurricular(
        turmaSelecionada.ano,
        turmaSelecionada.ensinoEspecial,
        [tabAtualComponenteCurricular.codigoComponenteCurricular]
      )
        .then(resposta => {
          montarDadosListasObjetivos(resposta.data);
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
  }, [
    tabAtualComponenteCurricular,
    turmaSelecionada,
    montarDadosListasObjetivos,
    obterDadosComponenteAtual,
  ]);

  useEffect(() => {
    if (tabAtualComponenteCurricular && dadosBimestrePlanoAnual) {
      obterObjetivosPorAnoEComponenteCurricular();
    }
  }, [
    tabAtualComponenteCurricular,
    obterObjetivosPorAnoEComponenteCurricular,
    dadosBimestrePlanoAnual,
  ]);

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
  dadosBimestre: PropTypes.oneOfType([PropTypes.object]),
  tabAtualComponenteCurricular: PropTypes.oneOfType([PropTypes.object]),
};

ListaObjetivos.defaultProps = {
  dadosBimestre: {},
  tabAtualComponenteCurricular: {},
};

export default ListaObjetivos;
