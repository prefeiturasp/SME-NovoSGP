/* eslint-disable no-param-reassign */
import React, { useState, useEffect, useCallback } from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import { Base, SelectComponent } from '~/componentes';
import { ColunasFixas, Tabela } from './index.css';
import { ordenarPor } from '~/utils/funcoes/gerais';

function objetoExistaNaLista(objeto, lista) {
  return lista.some(
    elemento => JSON.stringify(elemento) === JSON.stringify(objeto)
  );
}

const TabelaInformacoesEscolares = ({ dados, ciclos, anos }) => {
  const [dadosTabela, setDadosTabela] = useState([]);
  const [unidadeSelecionada, setUnidadeSelecionada] = useState('quantidade');
  const [colunas, setColunas] = useState([]);

  const UNIDADES = {
    Q: 'quantidade',
    P: 'porcentagem',
  };

  const montaColunasDados = useCallback(() => {
    setDadosTabela([]);

    const colunasFixas = ColunasFixas();

    if (dados && Object.entries(dados).length) {
      const montaColunas = [];
      const montaDados = [];

      const eixo = dados[0];

      eixo.objetivos.forEach(objetivo => {
        const item = [];

        if (ciclos && objetivo.ciclos.length) {
          // Ciclos
          let ciclosSize = 0;
          objetivo.ciclos.forEach(ciclo => {
            ciclosSize =
              ciclo.respostas.length > ciclosSize
                ? ciclo.respostas.length
                : ciclosSize;
          });

          objetivo.ciclos.forEach((ciclo, c) => {
            ciclo.respostas.forEach((resposta, r) => {
              if (
                !item.find(dado => dado.Resposta === resposta.respostaDescricao)
              ) {
                item.push({
                  Id: shortid.generate(),
                  Eixo: eixo.eixoDescricao,
                  Objetivo: objetivo.objetivoDescricao,
                  ObjetivoGrupo: c === 0 && r === 0,
                  Resposta: resposta.respostaDescricao,
                  Total: 0,
                });
              }
            });
          });

          item.map(i => {
            i.ObjetivoSize = item.length;
            return i;
          });

          objetivo.ciclos.forEach(ciclo => {
            ciclo.respostas.forEach(resposta => {
              item
                .filter(i => i.Resposta === resposta.respostaDescricao)
                .map(i => {
                  i[ciclo.cicloDescricao] =
                    unidadeSelecionada === UNIDADES.Q
                      ? resposta[unidadeSelecionada]
                      : `${resposta[unidadeSelecionada].toFixed(2)}%`;
                  i.Total += resposta[unidadeSelecionada];
                  return item;
                });
            });
          });

          if (unidadeSelecionada === UNIDADES.P) {
            item.forEach(i => {
              i.Total = `${i.Total.toFixed(2)}%`;
            });
          }

          // Ciclos
          objetivo.ciclos.forEach(ciclo => {
            // Colunas
            const coluna = {
              title: `${ciclo.cicloDescricao}`,
              dataIndex: `${ciclo.cicloDescricao}`,
              render: text =>
                text || `0${unidadeSelecionada === UNIDADES.P ? `%` : ``}`,
            };

            if (!objetoExistaNaLista(coluna, montaColunas))
              montaColunas.push(coluna);
          });
        } else if (anos && objetivo.anos.length) {
          let anosSize = 0;
          objetivo.anos.forEach(ano => {
            anosSize =
              ano.respostas.length > anosSize ? ano.respostas.length : anosSize;
          });

          objetivo.anos.forEach((ano, a) => {
            ano.respostas.forEach((resposta, r) => {
              if (
                !item.find(dado => dado.Resposta === resposta.respostaDescricao)
              ) {
                item.push({
                  Id: shortid.generate(),
                  Eixo: eixo.eixoDescricao,
                  Objetivo: objetivo.objetivoDescricao,
                  ObjetivoGrupo: a === 0 && r === 0,
                  Resposta: resposta.respostaDescricao,
                  Total: 0,
                });
              }
            });
          });

          item.map(i => {
            i.ObjetivoSize = item.length;
            return i;
          });

          objetivo.anos.forEach(ano => {
            ano.respostas.forEach(resposta => {
              item
                .filter(dado => dado.Resposta === resposta.respostaDescricao)
                .map(dado => {
                  dado[ano.anoDescricao] =
                    unidadeSelecionada === UNIDADES.Q
                      ? resposta[unidadeSelecionada]
                      : `${resposta[unidadeSelecionada].toFixed(2)}%`;
                  dado.Total += resposta[unidadeSelecionada];
                  return dado;
                });
            });
          });

          if (unidadeSelecionada === UNIDADES.P) {
            item.forEach(i => {
              i.Total = `${i.Total.toFixed(2)}%`;
            });
          }

          // Anos
          objetivo.anos.forEach(ano => {
            // Colunas
            const coluna = {
              title: `${ano.anoDescricao}`,
              dataIndex: `${ano.anoDescricao}`,
              render: text =>
                text || `0${unidadeSelecionada === UNIDADES.P ? `%` : ``}`,
            };

            if (!objetoExistaNaLista(coluna, montaColunas))
              montaColunas.push(coluna);
          });
        }

        montaDados.push(...item);
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

      setColunas([...colunasFixas, ...ordenarPor(montaColunas, 'title')]);
    }
  }, [dados, ciclos, anos, unidadeSelecionada, UNIDADES.P, UNIDADES.Q]);

  useEffect(() => {
    montaColunasDados();
  }, [montaColunasDados]);

  const onTrocaUnidade = unidade => {
    setUnidadeSelecionada(unidade);
  };

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
        rowKey="Id"
        size="middle"
        className="my-2"
        bordered
        style={
          dadosTabela.length ? { borderLeft: `7px solid ${Base.Azul}` } : {}
        }
        locale={{ emptyText: 'Sem dados' }}
      />
    </>
  );
};

TabelaInformacoesEscolares.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.any]),
  ciclos: PropTypes.oneOfType([PropTypes.bool]),
  anos: PropTypes.oneOfType([PropTypes.bool]),
};

TabelaInformacoesEscolares.defaultProps = {
  dados: [],
  ciclos: false,
  anos: false,
};

export default TabelaInformacoesEscolares;
