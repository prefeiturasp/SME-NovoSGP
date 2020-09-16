import React, { useState } from 'react';
import TransferenciaLista from '~/componentes-sgp/TranferenciaLista/transferenciaLista';
import { ContainerListaObjetivos } from './listaObjetivos.css';

const ListaObjetivos = () => {
  const mock = [
    {
      id: 1,
      codigo: 'EF02M01',
      descricao:
        'Explorar números no contexto diário como indicadores de quantidade, ordem, medida e código; ler e produzir escritas numéricas, identificando algumas regularidades do sistema de numeração decimal ',
    },
    {
      id: 2,
      codigo: 'EF02M02',
      descricao: 'Compor e decompor números naturais de diversas maneiras',
    },
    {
      id: 3,
      codigo: 'EF02M03',
      descricao:
        'Explorar diferentes estratégias para quantificar elementos de uma coleção: contagem um a um, formação de pares, agrupamentos e estimativas ',
    },
    {
      id: 4,
      codigo: 'EF02M01',
      descricao:
        'Explorar números no contexto diário como indicadores de quantidade, ordem, medida e código; ler e produzir escritas numéricas, identificando algumas regularidades do sistema de numeração decimal ',
    },
    {
      id: 5,
      codigo: 'EF02M02',
      descricao: 'Compor e decompor números naturais de diversas maneiras',
    },
    {
      id: 6,
      codigo: 'EF02M03',
      descricao:
        'Explorar diferentes estratégias para quantificar elementos de uma coleção: contagem um a um, formação de pares, agrupamentos e estimativas ',
    },
    {
      id: 7,
      codigo: 'EF02M01',
      descricao:
        'Explorar números no contexto diário como indicadores de quantidade, ordem, medida e código; ler e produzir escritas numéricas, identificando algumas regularidades do sistema de numeração decimal ',
    },
    {
      id: 8,
      codigo: 'EF02M02',
      descricao: 'Compor e decompor números naturais de diversas maneiras',
    },
    {
      id: 9,
      codigo: 'EF02M03',
      descricao:
        'Explorar diferentes estratégias para quantificar elementos de uma coleção: contagem um a um, formação de pares, agrupamentos e estimativas ',
    },
    {
      id: 10,
      codigo: 'EF02M01',
      descricao:
        'Explorar números no contexto diário como indicadores de quantidade, ordem, medida e código; ler e produzir escritas numéricas, identificando algumas regularidades do sistema de numeração decimal ',
    },
    {
      id: 11,
      codigo: 'EF02M02',
      descricao: 'Compor e decompor números naturais de diversas maneiras',
    },
    {
      id: 12,
      codigo: 'EF02M03',
      descricao:
        'Explorar diferentes estratégias para quantificar elementos de uma coleção: contagem um a um, formação de pares, agrupamentos e estimativas ',
    },
    {
      id: 13,
      codigo: 'EF02M01',
      descricao:
        'Explorar números no contexto diário como indicadores de quantidade, ordem, medida e código; ler e produzir escritas numéricas, identificando algumas regularidades do sistema de numeração decimal ',
    },
    {
      id: 14,
      codigo: 'EF02M02',
      descricao: 'Compor e decompor números naturais de diversas maneiras',
    },
    {
      id: 15,
      codigo: 'EF02M03',
      descricao:
        'Explorar diferentes estratégias para quantificar elementos de uma coleção: contagem um a um, formação de pares, agrupamentos e estimativas ',
    },
    {
      id: 16,
      codigo: 'EF02M01',
      descricao:
        'Explorar números no contexto diário como indicadores de quantidade, ordem, medida e código; ler e produzir escritas numéricas, identificando algumas regularidades do sistema de numeração decimal ',
    },
    {
      id: 17,
      codigo: 'EF02M02',
      descricao: 'Compor e decompor números naturais de diversas maneiras',
    },
    {
      id: 18,
      codigo: 'EF02M03',
      descricao:
        'Explorar diferentes estratégias para quantificar elementos de uma coleção: contagem um a um, formação de pares, agrupamentos e estimativas ',
    },
  ];
  const [dadosEsquerda, setDadosEsquerda] = useState(mock);
  const [dadosDireita, setDadosDireita] = useState([]);
  const [idsSelecionadsEsquerda, setIdsSelecionadsEsquerda] = useState([]);
  const [idsSelecionadsDireita, setIdsSelecionadsDireita] = useState([]);

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

export default ListaObjetivos;
