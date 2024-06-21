Aplicación web para el cálculo de la ruta óptima de los transportistas de una empresa mediante algoritmos de aprendizaje automático de inteligencia artificial desarrollado por Anastasia Chinaeva, Pedro José Pérez Román y Viacheslav Kostenko empleando las tecnologías C#.NET, Entity, Javascript, Bootstrap, jQuery y Microsoft SQL Server.

La aplicación permite el uso de 6 tipos de roles diferentes:

- Administrador. Acceso completo.
- Encargado de Zona. Encargado de gestionar las sedes de su provincia.
- Encargado de Sede. Encargado de gestionar los empleados de su sede.
- Repartidor. Encargado de los envios a sedes o domicilio.
- Usuario. Puede acceder a sus paquetes y comprobar su localización.
- Visitante. Cualquiera que acceda sin loguear. Puede buscar paquetes por número.

Para el cálculo de la ruta óptima a seguir por los transportistas de la compañía,
entendiendo como tal a la que minimiza la distancia recorrida por los mismos
teniendo en cuenta las restricciones de capacidad existentes, se recurre a una
popular técnica de inteligencia aritificial de aprendizaje automático no 
supervisado de tipo competitivo denominada algoritmo genético, por inspirarse 
en la mejora continua de la selección natural.
Mediante esta técnica, en el programa se definirá una población compuesta por
rutas aleatorias para, comparando el resultado obtenido, seleccionar las más
óptimas de la población. Partiendo de las mejores, se realizarán pequeñas
variaciones aleatorias y se combinarán entre sí para formar rutas alternativas,
formándose entonces una nueva población partiendo de la anterior. En esta nueva
población se volverá a seleccionar a las soluciones más óptimas, volviendo a 
repetir el proceso para que, a lo largo de miles de selecciones entre las 
variaciones aleatorias más óptimas, se llegue a una solución optimizada.
