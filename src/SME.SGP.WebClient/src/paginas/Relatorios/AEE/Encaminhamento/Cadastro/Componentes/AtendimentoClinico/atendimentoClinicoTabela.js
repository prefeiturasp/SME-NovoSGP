import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { TabelaColunasFixas } from './atendimentoClinicoTabela.css';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import Label from '~/componentes/label';
import { setExibirModalCadastroAtendimentoClinicoAEE } from '~/redux/modulos/encaminhamentoAEE/actions';
import ModalCadastroAtendimentoClinico from './modalCadastroAtendimentoClinico';

const AtendimentoClinicoTabela = props => {
  const { label } = props;

  const dispatch = useDispatch();

  const dados = [
    {
      diaSemana: 'Quinta',
      atendimentoAtividade: 'Academia estudantil de letras (AEL)',
      localRealizacao: 'Escola',
      horarioInicio: '09:00',
      horarioTermino: '09:30',
    },
    {
      diaSemana: 'Sexta',
      atendimentoAtividade: 'Academia estudantil de letras (AEL)',
      localRealizacao: 'Escola',
      horarioInicio: '09:00',
      horarioTermino: '09:30',
    },
  ];

  const onClickNovoDetalhamento = () => {
    dispatch(setExibirModalCadastroAtendimentoClinicoAEE(true));
  };

  return (
    <>
      <ModalCadastroAtendimentoClinico />
      <Label text={label} />
      <TabelaColunasFixas>
        <div className="wrapper">
          <div className="header-fixo">
            <table className="table">
              <thead className="tabela-dois-thead">
                <tr>
                  <th className="col-linha-um">Dia da Semana</th>
                  <th className="col-linha-um">Atendimento/Atividade</th>
                  <th className="col-linha-um">Local de realização</th>
                  <th className="col-linha-um">Horário de início</th>
                  <th className="col-linha-um">Horário de término</th>
                </tr>
              </thead>
              <tbody className="tabela-dois-tbody">
                {dados.map((data, index) => {
                  return (
                    <tr id={index}>
                      <td className="col-valor-linha-um">{data.diaSemana}</td>
                      <td className="col-valor-linha-um">
                        {data.atendimentoAtividade}
                      </td>
                      <td className="col-valor-linha-um">
                        {data.localRealizacao}
                      </td>
                      <td className="col-valor-linha-um">
                        {data.horarioInicio}
                      </td>
                      <td className="col-valor-linha-um">
                        {data.horarioTermino}
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          </div>
        </div>
      </TabelaColunasFixas>
      <Button
        id="btn-novo-detalhamento"
        label="Novo detalhamento"
        icon="plus"
        color={Colors.Azul}
        border
        className="mr-3"
        onClick={onClickNovoDetalhamento}
      />
    </>
  );
};

AtendimentoClinicoTabela.propTypes = {
  name: PropTypes.string,
  id: PropTypes.string,
  label: PropTypes.string,
};

AtendimentoClinicoTabela.defaultProps = {
  indexLinha: PropTypes.number,
  dados: PropTypes.oneOfType([PropTypes.array]),
};

export default AtendimentoClinicoTabela;
