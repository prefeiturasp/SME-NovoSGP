import React, { useState, useEffect, useCallback } from 'react';
import PropTypes from 'prop-types';
import { Base, SelectComponent } from '~/componentes';
import { ColunasFixas, Tabela } from './index.css';

const TabelaInformacoesEscolares = ({ dados, ciclos, anos }) => {
  const [dadosTabela, setDadosTabela] = useState([]);
  const [unidadeSelecionada, setUnidadeSelecionada] = useState('quantidade');
  const [colunas, setColunas] = useState([]);

  const objetoExistaNaLista = (objeto, lista) => {
    return lista.some(
      elemento => JSON.stringify(elemento) === JSON.stringify(objeto)
    );
  };

  const montaColunasDados = useCallback(() => {
    setDadosTabela([]);

    const colunasFixas = ColunasFixas();

    if (dados && Object.entries(dados).length) {
      const montaColunas = [];
      const montaDados = [];

      const eixo = dados[0];

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
              dado.TamanhoObjetivos = eixo.objetivos.length;
              dado.Resposta = resposta.respostaDescricao;
              dado.TamanhoRespostas = ciclo.respostas.length;
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

              if (!objetoExistaNaLista(dado, montaDados)) montaDados.push(dado);
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
              dado.TamanhoObjetivos = eixo.objetivos.length;
              dado.Resposta = resposta.respostaDescricao;
              dado.TamanhoRespostas = ano.respostas.length;
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

              if (!objetoExistaNaLista(dado, montaDados)) montaDados.push(dado);
            });
          });
        }
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
        rowKey="Id"
        size="middle"
        className="my-2"
        bordered
        style={{ borderLeft: `7px solid ${Base.Azul}` }}
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
