import React, { useEffect, useState, useCallback } from 'react';
import shortid from 'shortid';
import styled from 'styled-components';
import PropTypes from 'prop-types';

// Ant
import { Table } from 'antd';

// Componentes
import { Base, Loader } from '~/componentes';
import ResumosGraficosPAPServico from '~/servicos/Paginas/Relatorios/PAP/ResumosGraficos';

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

const TabelaTotalEstudantes = ({ filtros }) => {
  const [colunas, setColunas] = useState([]);

  const [dadosTabela, setDadosTabela] = useState([]);
  const [carregandoDados, setCarregandoDados] = useState(false);

  const buscarDadosApi = useCallback(() => {
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

    ResumosGraficosPAPServico.ListarTotalEstudantes(filtros)
      .then(resposta => {
        const { data, status } = resposta;

        if (data && status === 200) {
          const montaColunas = [];

          if (data.ciclos.length) {
            data.ciclos.forEach(ciclo => {
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

            data.ciclos.forEach(ano => {
              dadoQuantidade[`${ano.cicloDescricao}`] = ano.quantidade;
            });
            dadoQuantidade.Total = data.quantidadeTotal;

            montaDados.push(dadoQuantidade);

            // Linha Porcentagem

            const dadoPorcentagem = {};
            dadoPorcentagem.Id = shortid.generate();
            dadoPorcentagem.TipoDado = 'Porcentagem';

            data.ciclos.forEach(ano => {
              dadoPorcentagem[`${ano.cicloDescricao}`] = `${ano.porcentagem}%`;
            });
            dadoPorcentagem.Total = `${data.porcentagemTotal}%`;

            montaDados.push(dadoPorcentagem);

            setDadosTabela(montaDados);
          } else {
            data.anos.forEach(ano => {
              const coluna = {};
              coluna.title = ano.anoDescricao;
              coluna.dataIndex = ano.anoDescricao;
              if (montaColunas.indexOf(coluna) === -1) {
                montaColunas.push(coluna);
              }
            });

            const montaDados = [];

            // Linha Quantidade

            const dadoQuantidade = {};
            dadoQuantidade.Id = shortid.generate();
            dadoQuantidade.TipoDado = 'Quantidade';

            data.anos.forEach(ano => {
              dadoQuantidade[`${ano.anoDescricao}`] = ano.quantidade;
            });
            dadoQuantidade.Total = data.quantidadeTotal;

            montaDados.push(dadoQuantidade);

            // Linha Porcentagem

            const dadoPorcentagem = {};
            dadoPorcentagem.Id = shortid.generate();
            dadoPorcentagem.TipoDado = 'Porcentagem';

            data.anos.forEach(ano => {
              dadoPorcentagem[`${ano.anoDescricao}`] = `${ano.porcentagem}%`;
            });
            dadoPorcentagem.Total = `${data.porcentagemTotal}%`;

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
        }
      })
      .finally(() => {
        setCarregandoDados(false);
      });
  }, [filtros]);

  useEffect(() => {
    buscarDadosApi();
  }, [buscarDadosApi]);

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
  filtros: PropTypes.oneOfType([PropTypes.any]),
};

TabelaTotalEstudantes.defaultProps = {
  filtros: [],
};

export default TabelaTotalEstudantes;
