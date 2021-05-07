import PropTypes from 'prop-types';
import React from 'react';
import { DataTable, Label } from '~/componentes';
import SelectComponent from '~/componentes/select';
import { confirmar } from '~/servicos/alertas';

import { CardTabelaAlunos } from '../styles';

const ListaAlunosAusenciasCompensadas = props => {
  const {
    listaAusenciaCompensada,
    idsAlunosAusenciaCompensadas,
    onSelectRow,
    atualizarValoresListaCompensacao,
    desabilitarCampos,
  } = props;

  const atualizarValores = async (qt, indexAluno) => {
    if (indexAluno >= 0) {
      const lista = listaAusenciaCompensada;
      lista[indexAluno].quantidadeFaltasCompensadas = qt;
      atualizarValoresListaCompensacao(lista);
    }
  };

  const montaCompensacao = (qtCompensada, dadosAluno) => {
    const listaMaximoCompensar = [];
    const qtMaxima = dadosAluno.maximoCompensacoesPermitidas;
    for (let index = 0; index < qtMaxima; index++) {
      listaMaximoCompensar.push({
        valor: String(index + 1),
        descricao: String(index + 1),
      });
    }
    return (
      <SelectComponent
        onChange={qt => {
          if (!desabilitarCampos) {
            const aluno = listaAusenciaCompensada.find(
              item => item.id == dadosAluno.id
            );
            let indexAluno = null;
            if (aluno) {
              indexAluno = listaAusenciaCompensada.indexOf(aluno);
            }
            atualizarValores(qt, indexAluno, aluno);
          }
        }}
        valueOption="valor"
        valueText="descricao"
        lista={listaMaximoCompensar}
        valueSelect={qtCompensada || undefined}
        placeholder="Faltas"
      />
    );
  };

  const colunasListaAlunosAusenciaCompensada = [
    {
      title: 'Nome',
      dataIndex: 'nome',
      ellipsis: true,
    },
    {
      title: 'Compensações',
      dataIndex: 'quantidadeFaltasCompensadas',
      render: (qtdFaltas, dadosAluno) => {
        return montaCompensacao(qtdFaltas ? String(qtdFaltas) : '', dadosAluno);
      },
    },
  ];

  const onSelectRowAlunos = ids => {
    onSelectRow(ids);
  };

  return (
    <>
      <Label text="Estudantes com Ausências Compensadas" />
      <CardTabelaAlunos>
        <DataTable
          scroll={{ y: 420 }}
          id="lista-alunos-ausencia-compensada"
          selectedRowKeys={idsAlunosAusenciaCompensadas}
          onSelectRow={onSelectRowAlunos}
          columns={colunasListaAlunosAusenciaCompensada}
          dataSource={listaAusenciaCompensada}
          selectMultipleRows
          pagination={false}
          pageSize={9999}
        />
      </CardTabelaAlunos>
    </>
  );
};

ListaAlunosAusenciasCompensadas.propTypes = {
  listaAusenciaCompensada: PropTypes.oneOfType([
    PropTypes.array,
    PropTypes.object,
  ]),
  idsAlunosAusenciaCompensadas: PropTypes.oneOfType([
    PropTypes.array,
    PropTypes.string,
  ]),
  onSelectRow: PropTypes.func,
  atualizarValoresListaCompensacao: PropTypes.func,
  desabilitarCampos: PropTypes.bool,
};

ListaAlunosAusenciasCompensadas.defaultProps = {
  listaAusenciaCompensada: [],
  idsAlunosAusenciaCompensadas: [],
  onSelectRow: () => {},
  atualizarValoresListaCompensacao: () => {},
  desabilitarCampos: false,
};

export default ListaAlunosAusenciasCompensadas;
