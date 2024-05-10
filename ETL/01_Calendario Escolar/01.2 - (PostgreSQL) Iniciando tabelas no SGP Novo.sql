TRUNCATE TABLE evento CASCADE;

ALTER TABLE evento 
    ALTER COLUMN id
        RESTART WITH 1;

TRUNCATE TABLE periodo_escolar CASCADE;

ALTER TABLE periodo_escolar
    ALTER COLUMN id
        RESTART WITH 1;

TRUNCATE TABLE tipo_calendario CASCADE;

ALTER TABLE tipo_calendario
    ALTER COLUMN id
        RESTART WITH 1;