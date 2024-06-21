
Create database elGatoMensajero;

use elGatoMensajero;

CREATE TABLE sedes (
	id int IDENTITY NOT NULL PRIMARY KEY,
	provincia VARCHAR(255) NOT NULL,
	localidad VARCHAR(255) NOT NULL,
	cp INT NOT NULL,
	calle VARCHAR(255) not null,
	num int
);

CREATE TABLE transportes (
	matricula VARCHAR(8) NOT NULL PRIMARY KEY,
	marca VARCHAR(32) NOT NULL,
	modelo VARCHAR(64) NOT NULL,
	consumo FLOAT NOT NULL,
	km FLOAT NOT NULL,
	fecha_matriculacion DATE NOT NULL,
	sede int,
	FOREIGN KEY (sede)
	REFERENCES sedes(id)
		ON UPDATE set null
);

CREATE TABLE tipo_empleados (
	tipo SMALLINT IDENTITY NOT NULL PRIMARY KEY,
	salario FLOAT NOT NULL,
	descripcion VARCHAR(64) NOT NULL
);

create table roles(
	tipo varchar(32) not null primary key
);


create table personas(
	id INT IDENTITY NOT NULL PRIMARY KEY,
	email VARCHAR(255) NOT NULL UNIQUE,
	password VARCHAR(255) NOT NULL,
	nombre VARCHAR(64) NOT NULL,
	apellidos VARCHAR(255) NULL,
	telefono INT NOT NULL,
	fecha_nacimiento DATE NOT NULL,
	direccion VARCHAR(255) NOT NULL,
	codPostal int not null,
	rol varchar(32) not null foreign key references roles(tipo) on update cascade
);

CREATE TABLE empleados (
	id int NOT NULL PRIMARY KEY,
	dni VARCHAR(16) NOT NULL,
	nss VARCHAR(100) UNIQUE NOT NULL,
	fecha_inicio DATE NOT NULL,
	fecha_fin DATE NULL,
	tipo SMALLINT,
	sede int,
	FOREIGN KEY (id)
	REFERENCES personas(id)
		ON UPDATE cascade,
	FOREIGN KEY (tipo)
	REFERENCES tipo_empleados(tipo)
		ON UPDATE cascade,
	FOREIGN KEY (sede)
	REFERENCES sedes(id)
		ON UPDATE cascade
);

CREATE TABLE usuarios (
	id int NOT NULL PRIMARY KEY,
	username VARCHAR(32) NOT NULL UNIQUE,
	FOREIGN KEY (id)
	REFERENCES personas(id)
		ON UPDATE cascade,
);

CREATE TABLE estado_paquetes (
	estado VARCHAR(32) NOT NULL PRIMARY KEY
);

CREATE TABLE paquetes (
	id INT IDENTITY NOT NULL PRIMARY KEY,
	peso FLOAT NOT NULL,
	dimensiones VARCHAR(32) NOT NULL,
	estado VARCHAR(32) NOT NULL,
	emisor INT,
	receptor INT,
	nombreOrigen varchar(255),
	nombreDestino varchar(255),
	direccionOrigen varchar(100),
	codPostalOrigen int, 
	tlorigen int,
	tlDestino int,
	direccionDestino varchar(100),
	codPostalDestino int,
	FOREIGN KEY (estado)
	REFERENCES estado_paquetes(estado)
		ON UPDATE CASCADE,
	FOREIGN KEY (emisor)
	REFERENCES personas(id),
	FOREIGN KEY (receptor)
	REFERENCES personas(id)
);

create table reparte (
	paquete INT NOT NULL,
	empleado int NOT NULL,
	primary key(paquete, empleado),
	FOREIGN key (paquete) references paquetes(id),
	FOREIGN key (empleado) references empleados(id)
);

create table tramita(
	paquete INT NOT NULL,
	sede int NOT NULL,
	primary key(paquete, sede),
	FOREIGN key (paquete) references paquetes(id),
	FOREIGN key (sede) references sedes(id)
);

create table conduce(
	matricula VARCHAR(8) NOT NULL,
	empleado int NOT NULL,
	fecha date not null,
	FOREIGN key (matricula) references transportes(matricula),
	FOREIGN key (empleado) references empleados(id),
	Primary key(matricula, empleado, fecha)	
);

-- Valores sede --

insert into sedes values ('A CORUÑA', 'Abegondo' , 15318, 'C/ San Marcos', null);
insert into sedes values ('A CORUÑA', 'Aranga' , 15317, 'Plaza Maestro Mosquera', 1);
insert into sedes values ('A CORUÑA', 'Ares' , 15624, 'Plaza Jose Antonio', null);
insert into sedes values ('A CORUÑA', 'Camariñas' , 15123, 'Praza Insuela', 57);
insert into sedes values ('A CORUÑA', 'Fene' , 15500, 'Avda. Concello', null);
insert into sedes values ('A CORUÑA', 'Curtis' , 15310, 'Plaza España',1);
insert into sedes values ('A CORUÑA', 'Frades' , 15686, 'C/ Cimadevila', null);
insert into sedes values ('A CORUÑA', 'Muros' , 15250, 'Curro Da Plaza', 1);
insert into sedes values ('A CORUÑA', 'Negreira' , 15830, 'C/ Carmen', 3);
insert into sedes values ('A CORUÑA', 'Santa Comba' , 15111, 'Plaza Concello',1);
insert into sedes values ('A CORUÑA', 'Sobrado' , 15813, 'Plaza Portal',1);
insert into sedes values ('ÁLAVA', 'BARRUNDIA' , 01206, 'Plaza del Ayuntamiento',1);
insert into sedes values ('ÁLAVA', 'BERANTEVILLA' , 01218, 'Mayor',11);
insert into sedes values ('ÁLAVA', 'IRUÑA DE OCA' , 01230, 'Parque Lehendakaria J.A.Agirre',1);
insert into sedes values ('ÁLAVA', 'LABASTIDA' , 01330, 'Plaza de la Paz', null);
insert into sedes values ('ÁLAVA', 'LAGRÁN' , 01118, 'Herrería', 1);
insert into sedes values ('ÁLAVA', 'LAGUARDIA' , 01300, 'Pza. Mayor', 1);
insert into sedes values ('ÁLAVA', 'LANCIEGO' , 01308, 'Calvo Sotelo',4);
insert into sedes values ('ÁLAVA', 'LEGUTIANO' , 01170, 'Carmen', 10);
insert into sedes values ('ÁLAVA', 'NAVARIDAS' , 01307, 'Fabulista Samaniego', 4);
insert into sedes values ('ÁLAVA', 'PEÑACERRADA-URIZAHARRA' , 01212, 'Plaza Fray Jacinto Martínez', 2);
insert into sedes values ('ÁLAVA', 'RIBERA BAJA' , 01213, 'Plaza San Martín',1);
insert into sedes values ('ÁLAVA', 'URKABUSTAIZ' , 01440, '	Plaza Municipal',2);
insert into sedes values ('ÁLAVA', 'ZIGOITIA' , 01138, '	Udal Plaza',2);

select * from sedes;

-- Valores roles --

insert into roles values ('usuario');
insert into roles values ('administrador');
insert into roles values ('repartidor');
insert into roles values ('encargadoSede');
insert into roles values ('encargadoZona');

select * from roles;

-- Valores personas --

insert into personas values ('anastasia@gmail.com', 'hola123', 'Anastasia', 'Chinaeva', 689111168, '16/03/1995', 'sidi ifni n20 esc2 3ºB', 03014,'administrador');
insert into personas values ('pedro@gmail.com', 'adios321', 'Pedro', 'i don nou', 986123478, '16/03/1995', 'sidi ifni n20 esc2 3ºB', 03014,'administrador');
insert into personas values ('slava@gmail.com', 'hola123', 'Slava', 'sinapellido :)', 123456789, '10/03/1985', 'sidi ifni n20 esc2 3ºB', 03014,'administrador');
insert into personas values ('ana@ana.com', 'parol123', 'Anna', 'Gaona', 369852147, '13/08/1990', 'direcciones', 03202, 'usuario');
insert into personas values ('pepe@ana.com', 'contrsenya123', 'Juan', 'Pepe', 258963476,'06/07/1998', 'direccion de pepe', 03610, 'repartidor');

select * from personas;

-- delete from roles where tipo='sudo';
-- update paquetes set nombreOrigen = 'Juan', nombreDestino ='Sergio'  where id=7;


-- Valores en usuarios --

insert into usuarios values (4, 'anaTop');

select * from usuarios;

-- Valores tipo empleados --

insert into tipo_empleados values (1, 12000, 'junior 1');
insert into tipo_empleados values (2, 12000, 'junior 2');
insert into tipo_empleados values (3, 13000, 'junior 3');
insert into tipo_empleados values (4, 14000, 'junior 4');
insert into tipo_empleados values (5, 15000, 'senior');

select * from tipo_empleados;

-- Valores Empleados --

insert into empleados values (1, 'X5057717V', '123456789ABC' ,'10/11/2017', null, 1,1);
insert into empleados values (2, '25869183H', '133456789ABC' ,'10/11/2017', null, 1,1);
insert into empleados values (3, 'X5053689L', '133456689ABC' ,'10/11/2017', null, 1,1);
insert into empleados values (5, 'X5053899L', '133456689CCC' ,'11/11/2017', null, 1,1);

select * from empleados;
select * from sedes;

select * from personas;

--alter table empleados drop column fecha_nacimiento;

-- Valores en transportes --

select * from transportes;

insert into transportes values('1111AAA', 'seat', 'furgoneta', 7.9, 630, '16/02/2016', 1);
insert into transportes values('2222AAA', 'nissan', 'furgoneta', 1.4, 500.8, '17/02/2016', 1);
insert into transportes values('3333AAA', 'bmv', 'furgoneta', 1.4, 500.8, '18/02/2016', 1);
insert into transportes values('4444AAA', 'hyundai', 'furgoneta', 1.4, 500.8, '19/02/2016', 1);
insert into transportes values('5555AAA', 'seat', 'furgoneta', 1.4, 580.8, '19/02/2016', 1);
insert into transportes values('6666AAA', 'seat', 'furgoneta', 1.4, 500.8, '20/02/2016', 1);
insert into transportes values('7777AAA', 'seat', 'furgoneta', 1.4, 5080.8, '22/02/2016', 1);
insert into transportes values('8888AAA', 'seat', 'furgoneta', 1.4, 700.7, '01/03/2016', 1);
insert into transportes values('9999AAA', 'seat', 'furgoneta', 1.4, 1007.4, '01/03/2016', 1);
insert into transportes values('1111BBB', 'nissan', 'furgoneta', 1.4, 50.8, '15/04/2016', 1);
insert into transportes values('2222BBB', 'nissan', 'furgoneta', 1.4, 150.3, '15/04/2016', 1);
insert into transportes values('3333BBB', 'nissan', 'furgoneta', 1.4, 6004.7, '15/04/2016', 1);
insert into transportes values('4444BBB', 'nissan', 'furgoneta', 1.4, 500.8, '15/04/2016', 1);
insert into transportes values('5555BBB', 'bmv', 'furgoneta', 1.4, 50540.1, '19/05/2016', 1);
insert into transportes values('6666BBB', 'bmv', 'furgoneta', 1.4, 500.8, '19/05/2016', 1);

-- Valores estado paquetes --

insert into estado_paquetes values('enviado');
insert into estado_paquetes values('esperando registro');
insert into estado_paquetes values('extraviado');
insert into estado_paquetes values('registrado');
insert into estado_paquetes values('en espera');
insert into estado_paquetes values('devuelto');

select * from paquetes;

insert into paquetes values(10, '10/23/58', 'enviado', 4, 1, 'Maria', 'Juan', 'Calle 24 de los pepinos', 03015, 685254458, 'Avenida de las sandias', 04012); 
insert into paquetes values(22, '11 03 08', 'extraviado', 2, 4, 'Pedro', 'Carlos', 'Calle 22 de los pepinos', 03016, 685255558, 'Avenida de las sandias 3', 04012); 
insert into paquetes values(03, '21-10-58', 'en espera', 3, 5, 'Anna', 'Salva', 'Calle 13 de los pepinos', 09012, 685892658, 'Avenida de las sandias 52', 04012); 

-- Valores conduce --

insert into conduce values ('1111AAA', 1, '10/05/2017');
insert into conduce values ('8888AAA', 1, '25/05/2017');
insert into conduce values ('6666BBB', 2, '10/05/2017');
insert into conduce values ('2222AAA', 3, '10/05/2017');

-- Valores tramita --

insert into tramita values(2, 10);
insert into tramita values(3, 5);

--Valores reparte --

insert into reparte values(2, 3);
insert into reparte values(3, 5);

-- MODIFICACIONES 12/01/2018 --
-- AÑADIDAS LONGITUD/LATITUD PARA EL ALGORITMO --
ALTER TABLE paquetes 
ADD longitud geography, latitud geography;

ALTER TABLE sedes
ADD longitud geography, latitud geography;

-- Modificaciones 16/01/2018 --

-- Se añade columna desguace a transportes --

use elGatoMensajero;

alter table transportes add desguace bit default(0);

--select * from tramita;

--drop table tramita;

--create table tramita(
--	paquete INT NOT NULL,
--	sede int NOT NULL,
--	primary key(paquete, sede),
--	FOREIGN key (paquete) references paquetes(id),
--	FOREIGN key (sede) references sedes(id), 
--	constraint eliminar on delete (sede) cascade
--);

