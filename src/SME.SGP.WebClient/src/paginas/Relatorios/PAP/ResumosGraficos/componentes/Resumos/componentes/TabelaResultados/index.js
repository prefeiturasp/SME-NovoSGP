import React, { useEffect, useState, useCallback } from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';

// Ant
import { Table } from 'antd';
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

<<<<<<< HEAD
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
=======
const TabelaResultados = ({ dados }) => {
  const [dadosTabela, setDadosTabela] = useState([]);
>>>>>>> ac36cae9de5d6d4a4d4613c0f083cd85ded0404c

  const [colunas, setColunas] = useState([]);

  const [unidadeSelecionada, setUnidadeSelecionada] = useState('quantidade');

  const montaColunasDados = useCallback(() => {
    const colunasFixas = [
      {
        title: 'Eixo',
        dataIndex: 'Eixo',
        colSpan: 3,
        width: 200,
        render: (text, row) => {
          return {
            children: text,
            props: {
              rowSpan: row.AgrupaEixo
                ? row.TamanhoObjetivos * row.TamanhoRespostas
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
        render: (text, row) => {
          return {
            children: text,
            props: {
              rowSpan: row.AgrupaObjetivo ? row.TamanhoRespostas : 0,
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

    if (dados && Object.entries(dados).length) {
      const montaColunas = [];
      const montaDados = [];

      const eixos = [...dados.items];

      eixos.forEach(eixo => {
        eixo.objetivos.forEach(objetivo => {
          if (objetivo.anos.length) {
            objetivo.anos.forEach(ano => {
              // Colunas
              const coluna = {
                title: `${ano.anoDescricao}`,
                dataIndex: `${ano.anoDescricao}`,
              };

              if (!objetoExistaNaLista(coluna, montaColunas))
                montaColunas.push(coluna);

              // Dados
              ano.respostas.forEach((resposta, indice) => {
                const dado = {};
                dado.Eixo = eixo.eixoDescricao;
                dado.Objetivo = objetivo.objetivoDescricao;
                dado.TamanhoObjetivos = eixo.objetivos.length;
                dado.Resposta = resposta.respostaDescricao;
                dado.TamanhoRespostas = ano.respostas.length;
                dado.AgrupaEixo = indice === 0;
                dado.AgrupaObjetivo = indice === 0;

                let total = 0;

                objetivo.anos.forEach(anoResposta => {
                  dado[anoResposta.anoDescricao] = resposta[unidadeSelecionada];
                  total += resposta[unidadeSelecionada];
                });

                dado.Total = total;

                if (!objetoExistaNaLista(dado, montaDados))
                  montaDados.push(dado);
              });
            });
          }
        });
<<<<<<< HEAD

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
=======
      });

      setDadosTabela([...montaDados]);

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
  }, [unidadeSelecionada, dados]);
>>>>>>> ac36cae9de5d6d4a4d4613c0f083cd85ded0404c

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
        dataSource={dadosTabela}
        rowKey="Id"
        size="middle"
        className="my-2"
        bordered
      />
    </>
  );
};

TabelaResultados.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.any]),
};

TabelaResultados.defaultProps = {
  dados: [],
};

export default TabelaResultados;
