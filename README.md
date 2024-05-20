Internet Banking App

Descripción del Proyecto:
Este proyecto consiste en el desarrollo de una aplicación de banca en línea utilizando ASP.NET Core MVC (versión 6 o 7).
La aplicación está diseñada para gestionar las operaciones bancarias de dos tipos de usuarios: administradores y clientes.
El objetivo principal es proporcionar una plataforma segura y eficiente para la gestión de cuentas, transacciones y otros servicios bancarios esenciales.

Objetivo General:
El objetivo general del proyecto es crear una aplicación de banca en línea robusta y funcional que permita a los usuarios administrar sus finanzas de manera efectiva y segura.
Los administradores tendrán herramientas para gestionar usuarios y monitorear las operaciones bancarias, mientras que los clientes podrán realizar pagos, transferencias, y otras operaciones financieras.

Funcionalidades Generales:

Login
* Pantalla inicial con formulario de usuario y contraseña.
* Redirección automática al home correspondiente si el usuario ya está logueado.
* Mensajes de error para credenciales incorrectas y usuarios inactivos.

Seguridad
* Acceso restringido a funcionalidades según el tipo de usuario (cliente o administrador).
* Redirección a la pantalla de login con mensajes de acceso denegado para usuarios no autenticados.
* Validaciones mediante filtros de autorización de Identity y creación de usuarios por defecto (admin y cliente).


Funcionalidades del Administrador:

Dashboard (Home)
* Menú con opciones: Home y Administración de usuario.
* Indicadores de transacciones totales y diarias, pagos realizados, clientes activos e inactivos, y productos asignados.

Administración de Usuarios
* Listado de usuarios registrados, diferenciando entre administradores y clientes, activos e inactivos.
* Formulario para crear nuevos usuarios con validaciones adecuadas.
* Capacidad para inactivar, activar y editar usuarios, con restricciones según el tipo de usuario.
* Gestión de productos asociados a usuarios, con generación de identificadores únicos para productos como cuentas de ahorro, tarjetas de crédito y préstamos.
  
Funcionalidades del Cliente: 

Home (Listado de Productos)
* Menú con opciones para gestionar pagos, beneficiarios, avances de efectivo y transferencias.
* Listado de productos del cliente, mostrando detalles como saldos de cuentas de ahorro, deudas de tarjetas de crédito y préstamos.
  
Beneficiarios
* Gestión de beneficiarios con opciones para agregar y eliminar cuentas frecuentes.
  
Pagos
* Opciones de pago expreso, pagos a tarjetas de crédito, préstamos y beneficiarios, con validaciones y confirmaciones necesarias.
* Registro detallado de todas las transacciones realizadas.
  
Avances de Efectivo
* Funcionalidad para realizar avances de efectivo desde tarjetas de crédito a cuentas de ahorro, con cálculos de intereses.
  
Transferencia entre Cuentas
* Permite transferencias entre cuentas del usuario, asegurando la disponibilidad de fondos.
  
Requerimientos Técnicos
* Uso de ViewModels con validaciones.
* Persistencia de datos mediante Entity Framework con Code First.
* Interfaz de usuario intuitiva utilizando Bootstrap.
* Aplicación de la arquitectura Onion al 100%.
* Implementación de repositorios y servicios genéricos.
* Manejo de usuarios con Identity.
* Mapeo de ViewModels, Entities y DTOs con AutoMapper.
* Precisión en el manejo de cantidades debido a la naturaleza financiera del proyecto.

  
Este proyecto representa un esfuerzo significativo para crear una plataforma de banca en línea confiable y fácil de usar,
aplicando las mejores prácticas de desarrollo y asegurando la seguridad y eficiencia en todas las operaciones bancarias.



OJO: Al momento de alguien bajar los cambios debe de cambiar solamente del appsettings.json (en el proyecto de WebApp) el IdentityConnection
y el DefaultConnetion con la ruta de su base de datos, luego vas a Package Manager Console y en la opción que dice Default Projects lo vas a poner primero
en la capa de identity y vas a escribir: Update-Database -Context IdentityContext, Lo mismo vas hacer con la capa de persistence, vas a cambiar de la capa
de identity a persistencia y en el panel de escritura vas a escribir: Update-Database -Context ApplicationContext. Sino haces estos cambios no te va a funcionar
la app y te va a dar un error de mapeo con la base de datos.
