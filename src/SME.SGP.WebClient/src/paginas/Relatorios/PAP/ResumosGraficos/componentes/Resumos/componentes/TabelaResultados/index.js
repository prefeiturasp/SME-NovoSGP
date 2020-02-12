import React, { useEffect, useState, useCallback } from 'react';
import styled from 'styled-components';

// Ant
import { Table } from 'antd';

// Services
import ResumosGraficosPAPServico from '~/servicos/Paginas/Relatorios/PAP/ResumosGraficos';
import { Base, SelectComponent } from '~/componentes';

const Tabela = styled(Table)`
  th.headerTotal {
    background-color: ${Base.Roxo};
    color: ${Base.Branco};
  }
`;

function objetoExistaNaLista(objeto, lista) {
  return lista.some(
    elemento => JSON.stringify(elemento) === JSON.stringify(objeto)
  );
}

const TabelaResultados = ({ dadosParametro }) => {
  let tamanhoObjetivos = 0;
  let tamanhoRespostas = 0;

  const [dados, setDados] = useState([]);

  const colunasFixas = [
    {
      title: 'Eixo',
      dataIndex: 'Eixo',
      colSpan: 3,
      width: 200,
      render: (text, row, index) => {
        return {
          children: text,
          props: {
            rowSpan:
              index % (tamanhoObjetivos * tamanhoRespostas) === 0
                ? tamanhoObjetivos * tamanhoRespostas
                : 0,
            style: { fontWeight: 'bold' },
          },
        };
      },
    },
    {
      title: 'Objetivo',
      dataIndex: 'Objetivo',
      colSpan: 0,
      width: 150,
      render: (text, row, index) => {
        return {
          children: text,
          props: {
            rowSpan: index % tamanhoRespostas === 0 ? tamanhoRespostas : 0,
            style: { fontWeight: 'bold' },
          },
        };
      },
    },
    {
      title: 'Resposta',
      dataIndex: 'Resposta',
      colSpan: 0,
      width: 150,
      render: text => {
        return {
          children: text,
          props: {
            rowSpan: 1,
            style: { fontWeight: 'bold' },
          },
        };
      },
    },
  ];

  const [colunas, setColunas] = useState([]);

  const [filtro] = useState(true);
  const [unidadeSelecionada, setUnidadeSelecionada] = useState('quantidade');

  const montaColunasDados = useCallback(() => {
    ResumosGraficosPAPServico.ListarResultados(filtro).then(retorno => {
      const { data, status } = retorno;

      if (data && status === 200) {
        const montaColunas = [];
        const montaDados = [];

        const eixos = [...data.items];

        eixos.forEach(eixo => {
          eixo.objetivos.forEach(objetivo => {
            objetivo.anos.forEach(ano => {
              const coluna = {
                title: `${ano.anoDescricao}`,
                dataIndex: `${ano.anoDescricao}`,
              };

              if (!objetoExistaNaLista(coluna, montaColunas)) {
                montaColunas.push(coluna);
              }

              ano.respostas.forEach(resposta => {
                const dado = {};
                dado.Eixo = eixo.eixoDescricao;
                dado.Objetivo = objetivo.objetivoDescricao;
                dado.Resposta = resposta.respostaDescricao;
                objetivo.anos.forEach(anoResposta => {
                  dado[anoResposta.anoDescricao] = resposta[unidadeSelecionada];
                });
                if (!objetoExistaNaLista(dado, montaDados)) {
                  montaDados.push(dado);
                }
              });

              if (ano.respostas.length > tamanhoRespostas) {
                tamanhoRespostas = ano.respostas.length;
              }
            });
          });

          if (eixo.objetivos.length > tamanhoObjetivos) {
            tamanhoObjetivos = eixo.objetivos.length;
          }
        });

        setDados([...montaDados]);

        montaColunas.push({
          title: 'Total',
          dataIndex: 'Total',
          width: 100,
          fixed: 'right',
          className: 'headerTotal',
          render: text => {
            return {
              children: text,
              props: {
                style: { backgroundColor: Base.CinzaTabela },
              },
            };
          },
        });

        setColunas([...colunasFixas, ...montaColunas]);
      }
    });
  }, [unidadeSelecionada, dadosParametro]);

  useEffect(() => {
    montaColunasDados();
  }, [montaColunasDados]);

  const listaUnidades = [
    {
      desc: 'Quantidade',
      valor: 'quantidade',
    },
    {
      desc: 'Porcentagem',
      valor: 'porcentagem',
    },
  ];

  const onTrocaUnidade = unidade => {
    setUnidadeSelecionada(unidade);
  };

  return (
    <>
      <SelectComponent
        lista={listaUnidades}
        onChange={onTrocaUnidade}
        valueSelect={unidadeSelecionada}
        valueText="desc"
        valueOption="valor"
        className="w-25"
      />
      <Tabela
        pagination={false}
        columns={colunas}
        dataSource={dados}
        rowKey="Id"
        size="middle"
        className="my-2"
        bordered
      />
    </>
  );
};

export default TabelaResultados;
