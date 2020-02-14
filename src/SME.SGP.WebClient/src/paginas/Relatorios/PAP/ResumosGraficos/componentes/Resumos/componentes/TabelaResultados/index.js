import React, { useEffect, useState, useCallback } from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';

// Ant
import { Table, Tooltip } from 'antd';
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

const TabelaResultados = ({ dados, ciclos, anos }) => {
  const [dadosTabela, setDadosTabela] = useState([]);

  const [colunas, setColunas] = useState([]);

  const [unidadeSelecionada, setUnidadeSelecionada] = useState('quantidade');

  const montaColunasDados = useCallback(() => {
    setDadosTabela([]);

    if (Object.entries(dados).length && !Object.entries(dados.items).length)
      return;

    let eixoCores = [];

    const renderizarCor = eixo => {
      const cores = [
        Base.AzulCalendario,
        Base.CinzaTabela,
        Base.Laranja,
        Base.RosaCalendario,
        Base.Roxo,
        Base.Verde,
        Base.Vermelho,
      ];
      return cores[eixoCores.indexOf(eixo)];
    };

    const colunasFixas = [
      {
        title: 'Eixo',
        dataIndex: 'Eixo',
        colSpan: 1,
        fixed: 'left',
        width: 200,
        render: (text, row) => {
          let valor = text;
          if (valor.length > 50) valor = `${text.substr(0, 50)}...`;
          return {
            children: <Tooltip title={text}>{valor}</Tooltip>,
            props: {
              rowSpan: row.AgrupaEixo ? row.ObjetivoSize : 0,
              style: {
                borderLeft: `${
                  row.AgrupaEixo ? `7px solid ${renderizarCor(text)}` : ``
                }`,
                fontWeight: 'bold',
              },
            },
          };
        },
      },
      {
        title: 'Objetivo',
        dataIndex: 'Objetivo',
        colSpan: 1,
        fixed: 'left',
        width: 150,
        render: (text, row) => {
          let valor = text;
          if (valor.length > 50) valor = `${text.substr(0, 50)}...`;
          return {
            children: <Tooltip title={text}>{valor}</Tooltip>,
            props: {
              rowSpan: row.AgrupaObjetivo ? row.RespostaSize : 0,
              style: { fontWeight: 'bold' },
            },
          };
        },
      },
      {
        title: 'Resposta',
        dataIndex: 'Resposta',
        colSpan: 1,
        fixed: 'left',
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

      const eixosSize = [];
      eixos.forEach(eixo => {
        if (!objetoExistaNaLista(eixo.eixoDescricao, eixosSize))
          eixosSize[eixo.eixoDescricao] = 0;
        eixo.objetivos.forEach(objetivo => {
          if (anos && objetivo.anos.length) {
            eixosSize[eixo.eixoDescricao] += parseInt(
              objetivo.anos[0].respostas.length,
              10
            );
          } else if (ciclos && objetivo.ciclos.length) {
            eixosSize[eixo.eixoDescricao] += parseInt(
              objetivo.ciclos[0].respostas.length,
              10
            );
          }
        });
      });

      eixoCores = Object.keys(eixosSize);

      eixos.forEach(eixo => {
        eixo.objetivos.forEach((objetivo, o) => {
          if (ciclos && objetivo.ciclos.length) {
            // Ciclos

            objetivo.ciclos.forEach(ciclo => {
              // Colunas
              const coluna = {
                title: `${ciclo.cicloDescricao}`,
                dataIndex: `${ciclo.cicloDescricao}`,
              };

              if (!objetoExistaNaLista(coluna, montaColunas))
                montaColunas.push(coluna);

              // Dados
              ciclo.respostas.forEach((resposta, r) => {
                const dado = {};
                dado.Eixo = eixo.eixoDescricao;
                dado.Objetivo = objetivo.objetivoDescricao;
                dado.ObjetivoSize = eixosSize[eixo.eixoDescricao];
                dado.Resposta = resposta.respostaDescricao;
                dado.RespostaSize = ciclo.respostas.length;
                dado.AgrupaEixo = o === 0 && r === 0;
                dado.AgrupaObjetivo = r === 0;

                let total = 0;

                objetivo.ciclos.forEach(cicloResposta => {
                  dado[cicloResposta.cicloDescricao] =
                    unidadeSelecionada === 'quantidade'
                      ? resposta[unidadeSelecionada]
                      : `${Math.round(resposta[unidadeSelecionada], 2)}%`;
                  total += resposta[unidadeSelecionada];
                });

                dado.Total =
                  unidadeSelecionada === 'quantidade'
                    ? total
                    : `${Math.round(total, 2)}%`;

                if (!objetoExistaNaLista(dado, montaDados))
                  montaDados.push(dado);
              });
            });
          } else if (anos && objetivo.anos.length) {
            // Anos
            objetivo.anos.forEach(ano => {
              // Colunas
              const coluna = {
                title: `${ano.anoDescricao}`,
                dataIndex: `${ano.anoDescricao}`,
              };

              if (!objetoExistaNaLista(coluna, montaColunas))
                montaColunas.push(coluna);

              // Dados
              ano.respostas.forEach((resposta, r) => {
                const dado = {};
                dado.Eixo = eixo.eixoDescricao;
                dado.Objetivo = objetivo.objetivoDescricao;
                dado.ObjetivoSize = eixosSize[eixo.eixoDescricao];
                dado.Resposta = resposta.respostaDescricao;
                dado.RespostaSize = ano.respostas.length;
                dado.AgrupaEixo = o === 0 && r === 0;
                dado.AgrupaObjetivo = r === 0;

                let total = 0;

                objetivo.anos.forEach(anoResposta => {
                  dado[anoResposta.anoDescricao] =
                    unidadeSelecionada === 'quantidade'
                      ? resposta[unidadeSelecionada]
                      : `${Math.round(resposta[unidadeSelecionada], 2)}%`;
                  total += resposta[unidadeSelecionada];
                });

                dado.Total =
                  unidadeSelecionada === 'quantidade'
                    ? total
                    : `${Math.round(total, 2)}%`;

                if (!objetoExistaNaLista(dado, montaDados))
                  montaDados.push(dado);
              });
            });
          }
        });
      });

      console.log([...montaDados]);

      setDadosTabela([...montaDados]);

      montaColunas.push({
        title: 'Total',
        dataIndex: 'Total',
        fixed: 'right',
        width: 100,
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

      console.log([...colunasFixas, ...montaColunas]);

      setColunas([...colunasFixas, ...montaColunas]);
    }
  }, [dados, ciclos, anos, unidadeSelecionada]);

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
        allowClear={false}
      />
      <Tabela
        pagination={false}
        columns={colunas}
        dataSource={dadosTabela}
        rowKey="Resposta"
        size="middle"
        className="my-2"
        bordered
        locale={{ emptyText: 'Sem dados' }}
      />
    </>
  );
};

TabelaResultados.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.any]),
  ciclos: PropTypes.oneOfType([PropTypes.bool]),
  anos: PropTypes.oneOfType([PropTypes.bool]),
};

TabelaResultados.defaultProps = {
  dados: [],
  ciclos: false,
  anos: false,
};

export default TabelaResultados;
