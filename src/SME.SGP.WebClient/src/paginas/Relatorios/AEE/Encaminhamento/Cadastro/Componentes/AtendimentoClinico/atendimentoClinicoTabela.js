import { Tooltip } from 'antd';
import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import styled from 'styled-components';
import { DataTable } from '~/componentes';
import Button from '~/componentes/button';
import { Base, Colors } from '~/componentes/colors';
import Label from '~/componentes/label';
import { setEncaminhamentoAEEEmEdicao } from '~/redux/modulos/encaminhamentoAEE/actions';
import { confirmar } from '~/servicos';
import ModalCadastroAtendimentoClinico from './modalCadastroAtendimentoClinico';

const AtendimentoClinicoTabela = props => {
  const { label, questaoAtual, form, desabilitado } = props;

  const dispatch = useDispatch();

  const [exibirModal, setExibirModal] = useState(false);

  const onClickNovoDetalhamento = () => {
    setExibirModal(true);
  };

  const onCloseModal = novosDados => {
    setExibirModal(false);

    if (novosDados) {
      const dadosAtuais = form?.values?.[questaoAtual.id]?.length
        ? form?.values?.[questaoAtual.id]
        : [];
      novosDados.id = dadosAtuais.length + 1;
      dadosAtuais.push(novosDados);
      if (form) {
        form.setFieldValue(questaoAtual.id, dadosAtuais);
        dispatch(setEncaminhamentoAEEEmEdicao(true));
      }
    }
  };

  const formatarCampoTabela = data => {
    let dataFormatada = '';
    if (data) {
      dataFormatada = moment(data).format('HH:mm');
    }
    return <span> {dataFormatada}</span>;
  };

  const colunas = [
    {
      title: 'Dia da Semana',
      dataIndex: 'diaSemana',
    },
    {
      title: 'Atendimento/Atividade',
      dataIndex: 'atendimentoAtividade',
    },
    {
      title: 'Local de realização',
      dataIndex: 'localRealizacao',
    },
    {
      title: 'Horário de início',
      dataIndex: 'horarioInicio',
      render: data => formatarCampoTabela(data),
    },
    {
      title: 'Horário de término',
      dataIndex: 'horarioTermino',
      render: data => formatarCampoTabela(data),
    },
    {
      title: 'Ação',
      dataIndex: 'acaoRemover',
      render: (texto, linha) => {
        return (
          <Tooltip title="Excluir">
            <span>
              <Button
                id="btn-excluir"
                icon="trash-alt"
                iconType="far"
                color={Colors.Azul}
                border
                className="btn-excluir-atendimento-clinico"
                disabled={desabilitado}
                onClick={async () => {
                  if (!desabilitado) {
                    const confirmado = await confirmar(
                      'Excluir',
                      '',
                      'Você tem certeza que deseja excluir este registro?'
                    );

                    if (confirmado) {
                      const dadosAtuais = form?.values?.[questaoAtual.id]
                        ?.length
                        ? form?.values?.[questaoAtual.id]
                        : [];

                      const indice = dadosAtuais.findIndex(
                        item => item.id === linha.id
                      );
                      if (indice !== -1) {
                        dadosAtuais.splice(indice, 1);
                        form.setFieldValue(questaoAtual.id, dadosAtuais);
                      }
                    }
                  }
                }}
                height="30px"
                width="30px"
              />
            </span>
          </Tooltip>
        );
      },
    },
  ];

  const Erro = styled.span`
    color: ${Base.Vermelho};
  `;

  const possuiErro = () => {
    return (
      form &&
      form.errors[String(questaoAtual.id)] &&
      form.touched[String(questaoAtual.id)]
    );
  };

  const obterErros = () => {
    return form &&
      form.touched[String(questaoAtual.id)] &&
      form.errors[String(questaoAtual.id)] ? (
      <Erro>{form.errors[String(questaoAtual.id)]}</Erro>
    ) : (
      ''
    );
  };

  return (
    <>
      <ModalCadastroAtendimentoClinico
        onClose={onCloseModal}
        exibirModal={exibirModal}
      />
      <Label text={label} />
      <div className={possuiErro() ? 'tabela-invalida' : ''}>
        <DataTable
          rowKey="id"
          columns={colunas}
          dataSource={
            form?.values?.[questaoAtual.id]?.length
              ? form?.values?.[questaoAtual.id]
              : []
          }
          pagination={false}
        />
      </div>
      {form ? obterErros() : ''}
      <Button
        id="btn-novo-detalhamento"
        label="Novo detalhamento"
        icon="plus"
        color={Colors.Azul}
        border
        className="mr-3 mt-2"
        onClick={onClickNovoDetalhamento}
        disabled={desabilitado}
      />
    </>
  );
};

AtendimentoClinicoTabela.propTypes = {
  questaoAtual: PropTypes.oneOfType([PropTypes.any]),
  form: PropTypes.oneOfType([PropTypes.any]),
  label: PropTypes.string,
  desabilitado: PropTypes.bool,
};

AtendimentoClinicoTabela.defaultProps = {
  label: '',
  questaoAtual: null,
  form: null,
  desabilitado: false,
};

export default AtendimentoClinicoTabela;
