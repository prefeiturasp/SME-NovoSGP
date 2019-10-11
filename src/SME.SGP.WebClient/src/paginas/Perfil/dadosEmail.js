import React from 'react';
import CampoTexto from '~/componentes/campoTexto';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import styled from 'styled-components';

const DadosEmail = () => {

    const Campos = styled.div`
        display: flex;

        .campo{
            min-width: 390px !important;
        }

        .btn-editar{
            margin-left: 10px !important;
        }
    `;

    return (
        <div>
            <Campos>
                <CampoTexto
                    label="E-mail"
                    className="col-11 campo"
                    placeholder="Insira um e-mail"
                    onChange={() => { }}
                />
                <Button
                    className="col-1 btn-editar"
                    label="Editar"
                    color={Colors.Roxo}
                    border
                    bold
                    onClick={() => { }}
                />
            </Campos>
            <Campos>
                <CampoTexto
                    label="Senha"
                    className="col-11 campo"
                    placeholder="Insira uma senha"
                    onChange={() => { }}
                />
                <Button
                    className="col-1 btn-editar"
                    label="Editar"
                    color={Colors.Roxo}
                    border
                    bold
                    onClick={() => { }}
                />
            </Campos>
        </div>
    );
}

export default DadosEmail;