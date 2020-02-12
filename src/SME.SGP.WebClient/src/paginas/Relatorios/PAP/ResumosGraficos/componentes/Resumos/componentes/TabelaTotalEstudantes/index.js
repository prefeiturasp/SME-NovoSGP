import React, { useEffect, useState, useCallback } from 'react';
import shortid from 'shortid';
import styled from 'styled-components';
import PropTypes from 'prop-types';

// Ant
import { Table } from 'antd';

// Componentes
import { Base, Loader } from '~/componentes';

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

const TabelaTotalEstudantes = ({ dados }) => {
  const [colunas, setColunas] = useState([]);

  const [dadosTabela, setDadosTabela] = useState([]);
  const [carregandoDados, setCarregandoDados] = useState(false);

  const montaColunasDados = useCallback(() => {
    const colunasFixas = [
      {
        title: 'Ano',
        dataIndex: 'TipoDado',
        fixed: 'left',
        width: 150,
        render: text => {
          return {
            children: text,
            props: {
              style: { fontWeight: 'bold' },
            },
          };
        },
      },
    ];

    setCarregandoDados(true);

    if (dados && Object.entries(dados).length) {
      const montaColunas = [];

      if (dados.ciclos.length) {
        dados.ciclos.forEach(ciclo => {
          const coluna = {
            title: ciclo.cicloDescricao,
            dataIndex: ciclo.cicloDescricao,
          };

          if (!objetoExistaNaLista(coluna, montaColunas)) {
            montaColunas.push(coluna);
          }
        });

        const montaDados = [];

        // Linha Quantidade

        const dadoQuantidade = {};
        dadoQuantidade.Id = shortid.generate();
        dadoQuantidade.TipoDado = 'Quantidade';

        dados.ciclos.forEach(ano => {
          dadoQuantidade[`${ano.cicloDescricao}`] = ano.quantidade;
        });
        dadoQuantidade.Total = dados.quantidadeTotal;

        montaDados.push(dadoQuantidade);

        // Linha Porcentagem

        const dadoPorcentagem = {};
        dadoPorcentagem.Id = shortid.generate();
        dadoPorcentagem.TipoDado = 'Porcentagem';

        dados.ciclos.forEach(ano => {
          dadoPorcentagem[`${ano.cicloDescricao}`] = `${Math.round(
            ano.porcentagem,
            2
          )}%`;
        });
        dadoPorcentagem.Total = `${dados.porcentagemTotal}%`;

        montaDados.push(dadoPorcentagem);

        setDadosTabela(montaDados);
      } else {
        dados.anos.forEach(ano => {
          const coluna = {
            title: ano.anoDescricao,
            dataIndex: ano.anoDescricao,
          };

          if (!objetoExistaNaLista(coluna, montaColunas)) {
            montaColunas.push(coluna);
          }
        });

        const montaDados = [];

        // Linha Quantidade

        const dadoQuantidade = {};
        dadoQuantidade.Id = shortid.generate();
        dadoQuantidade.TipoDado = 'Quantidade';

        dados.anos.forEach(ano => {
          dadoQuantidade[`${ano.anoDescricao}`] = ano.quantidade;
        });
        dadoQuantidade.Total = dados.quantidadeTotal;

        montaDados.push(dadoQuantidade);

        // Linha Porcentagem

        const dadoPorcentagem = {};
        dadoPorcentagem.Id = shortid.generate();
        dadoPorcentagem.TipoDado = 'Porcentagem';

        dados.anos.forEach(ano => {
          dadoPorcentagem[`${ano.anoDescricao}`] = `${Math.round(
            ano.porcentagem,
            2
          )}%`;
        });
        dadoPorcentagem.Total = `${dados.porcentagemTotal}%`;

        montaDados.push(dadoPorcentagem);

        setDadosTabela(montaDados);
      }

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
              style: {
                backgroundColor: Base.CinzaTabela,
                fontWeight: 'bold',
              },
            },
          };
        },
      });

      setColunas([...colunasFixas, ...montaColunas]);
      setCarregandoDados(false);
    }
  }, [dados]);

  useEffect(() => {
    montaColunasDados();
  }, [montaColunasDados]);

  return (
    <Loader loading={carregandoDados}>
      <Tabela
        pagination={false}
        columns={colunas}
        dataSource={dadosTabela}
        rowKey="Id"
        size="middle"
        className="my-2"
        style={{ borderLeft: `7px solid ${Base.Azul}` }}
        bordered
      />
    </Loader>
  );
};

TabelaTotalEstudantes.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.any]),
};

TabelaTotalEstudantes.defaultProps = {
  dados: [],
};

export default TabelaTotalEstudantes;
