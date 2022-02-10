alter table grade add column if not exists inicio_vigencia timestamp;
alter table grade add column if not exists fim_vigencia timestamp;

update grade set inicio_vigencia = '2014-01-01'::date where id in (1, 2, 3, 4, 5, 6, 8, 9);
update grade set inicio_vigencia = '2021-01-01'::date,
                 fim_vigencia = '2021-12-31'::date where id in (7, 10, 11, 12, 13, 14, 15);