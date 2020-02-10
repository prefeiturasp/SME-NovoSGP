import React, { useEffect, useState } from 'react';
import shortid from 'shortid';
import styled from 'styled-components';

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

const TabelaTotalEstudantes = () => {
  const [colunas, setColunas] = useState([
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
  ]);

  const [filtro] = useState(true);
  const [dadosTabela, setDadosTabela] = useState([]);
  const [carregandoDados, setCarregandoDados] = useState(false);

  const buscarDadosApi = () => {
    setCarregandoDados(true);

    ResumosGraficosPAPServico.ListarTotalEstudantes(filtro)
      .then(resposta => {
        const { data, status } = resposta;

        if (data && status === 200) {
          const montaColunas = [];

          if (filtro) {
            data.ciclos.forEach(ciclo => {
              const coluna = {};
              coluna.title = ciclo.cicloDescricao;
              coluna.dataIndex = ciclo.cicloDescricao;
              if (montaColunas.indexOf(coluna) === -1) {
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

          setColunas([...colunas, ...montaColunas]);
        }
      })
      .finally(() => {
        setCarregandoDados(false);
      });
  };

  useEffect(() => {
    buscarDadosApi();
  }, []);

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

export default TabelaTotalEstudantes;
