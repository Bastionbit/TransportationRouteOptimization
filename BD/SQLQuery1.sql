

use Ejercicio3;

create table tiempo2(
adios int NOT NULL PRIMARY KEY,
hola dateTime not null DEFAULT curdate());

insert into tiempo2 values (1, '10/03/2044');

select * from tiempo2;