using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPP.Entities.Objects.Generales;

namespace WPP.Helpers
{
    public class WPPConstants
    {
        // Administrador: Permiso a todo (Consultar, Crear, Editar, Eliminar) 
        // Consultor: Permiso a todo (Consultar) 
        // Editor: Permiso a todo (Consultar, Crear, Editar) 

        public  const String ROL_ADMINISTRADOR = "Administrador";

        public  const String ROL_SUPER_USUARIO = "Super Usuario";

        public const String ROLES_ADMINISTRACION = "Super Usuario, Administrador";

        public const String ROLES_ADMIN_CONTRATOS = "Administrador de Contratos";

        public const String ROLES_CONS_CONTRATOS = "Consultor de Contratos";

        public const String ROLES_EDIT_CONTRATOS = "Editor de Contratos";

        public const String ROLES_ADMIN_USUARIOS = "Administrador de Usuarios";

        public const String ROLES_EDIT_USUARIOS = "Editor de Usuarios";

        public const String ROLES_CONS_USUARIOS = "Consultor de Usuarios";

        public const String ROLES_ADMIN_CLIENTES = "Administrador de Clientes";

        public const String ROLES_EDIT_CLIENTES = "Editor de Clientes";

        public const String ROLES_CONS_CLIENTES = "Consultor de Clientes";

        public const String ROLES_ADMIN_COMPANIA = "Administrador de Compañías";

        public const String ROLES_CONS_COMPANIA = "Consultor de Compañías";

        public const String ROLES_EDIT_COMPANIA = "Editor de Compañías";

        public const String ROLES_ADMIN_PUNTOS_FACTURACION = "Administrador de Puntos de Facturación";

        public const String ROLES_CONS_PUNTOS_FACTURACION = "Consultor de Puntos de Facturación";

        public const String ROLES_EDIT_PUNTOS_FACTURACION = "Editor de Puntos de Facturación";

        public const String ROLES_CONS_CATEGORIA_PRODUCTOS = "Consultor de Categoría de Productos";

        public const String ROLES_ADMIN_CATEGORIA_PRODUCTOS = "Administrador de Categoría de Productos";

        public const String ROLES_EDIT_CATEGORIA_PRODUCTOS = "Editor de Categoría de Productos";

        public const String ROLES_CONS_PRODUCTOS = "Consultor de Productos";

        public const String ROLES_ADMIN_PRODUCTOS = "Administrador de Productos";

        public const String ROLES_EDIT_PRODUCTOS = "Editor de Productos";

        public const String ROLES_CONS_SERVICIOS = "Consultor de Servicios";

        public const String ROLES_ADMIN_SERVICIOS = "Administrador de Servicios";

        public const String ROLES_EDIT_SERVICIOS = "Editor de Servicios";

        public const String ROLES_CONS_BOLETAS = "Consultor de Boletas de Báscula";

        public const String ROLES_ADMIN_BOLETAS = "Administrador de Boletas de Báscula";

        public const String ROLES_EDIT_BOLETAS = "Editor de Boletas de Báscula";

        public const String ROLES_CONS_BOLETAS_MANUALES = "Consultor de Boletas Manuales (Báscula)";

        public const String ROLES_ADMIN_BOLETAS_MANUALES = "Administrador de Boletas Manuales (Báscula)";

        public const String ROLES_EDIT_BOLETAS_MANUALES = "Editor de Boletas Manuales (Báscula)";

        public const String ROLES_CONS_CONTENEDORES = "Consultor de Contenedores";

        public const String ROLES_ADMIN_CONTENEDORES = "Administrador de Contenedores";

        public const String ROLES_EDIT_CONTENEDORES = "Editor de Contenedores";

        public const String ROLES_CONS_EQUIPOS = "Consultor de Equipos";

        public const String ROLES_ADMIN_EQUIPOS = "Administrador de Equipos";

        public const String ROLES_EDIT_EQUIPOS = "Editor de Equipos";

        public const String ROLES_ADMIN_CONSECUTIVO_BOLETAS = "Administrador de Consecutivo de Boletas (Báscula)";

        public const String ROLES_CONS_CUADRILLA = "Consultor de Cuadrilla";

        public const String ROLES_ADMIN_CUADRILLA = "Administrador de Cuadrilla";

        public const String ROLES_EDIT_CUADRILLA = "Editor de Cuadrilla";

        public const String ROLES_CONS_RELLENO_SANITARIO = "Consultor de Rellenos Sanitarios";

        public const String ROLES_ADMIN_RELLENO_SANITARIO = "Administrador de Rellenos Sanitarios";

        public const String ROLES_EDIT_RELLENO_SANITARIO = "Editor de Rellenos Sanitarios";

        public const String ROLES_CONS_RUTA_RECOLECCION = "Consultor de Rutas Recolección";

        public const String ROLES_ADMIN_RUTA_RECOLECCION = "Administrador de Rutas Recolección";

        public const String ROLES_EDIT_RUTA_RECOLECCION = "Editor de Rutas Recolección";

        public const String ROLES_CONS_OTR = "Consultor de OTR";

        public const String ROLES_ADMIN_OTR = "Administrador de OTR";

        public const String ROLES_EDIT_OTR = "Editor de OTR";

        public const String ROLES_ADMIN_CIERRE_CAJA = "Administrador de Cierre de Caja";

        public const String ROLES_ADMIN_CIERRE_CAJA_REIMPRESION = "Administrador de Reimpresión Cierre de Caja";

        public const String ROLES_ADMIN_CONSECUTIVO_FACTURACION = "Administrador de Consecutivo de Facturación";

        public const String ROLES_ADMIN_CONSULTA_FACTURACION = "Administrador de Consulta de Facturación";

        public const String ROLES_ADMIN_FACTURACION = "Administrador de Facturación";

        public const String ROLES_REV_FACTURACION = "Reversión de Facturación";

        public const String ROLES_ADMIN_PREFACTURACION = "Administrador de Prefacturación";

        public const String ROLES_ADMIN_TIPO_CAMBIO = "Administrador de Tipo de Cambio";
        
        public const String ROLES_REP_USUARIOS = "Control de Operaciones: Usuarios";

        public const String ROLES_REP_CLIENTES = "Control de Operaciones: Clientes";

        public const String ROLES_REP_COMPANIAS = "Control de Operaciones: Compañías";

        public const String ROLES_REP_CONTRATO = "Control de Operaciones: Contrato";

        public const String ROLES_REP_PRODUCTOS = "Control de Operaciones: Productos";

        public const String ROLES_REP_CATEGORIA_PRODUCTOS = "Control de Operaciones: Categoría de Productos";
        
        public const String ROLES_REP_CIERRE_CAJA = "Control de Operaciones: Cierre de Caja";

        public const String ROLES_REP_CONTENEDOR = "Control de Operaciones: Contenedores";

        public const String ROLES_REP_EQUIPO = "Control de Operaciones: Equipos";

        public const String ROLES_REP_TONELADAS_BOLETA = "Control de Operaciones: Toneladas por Boleta";

        public const String ROLES_REP_FACTURACION = "Control de Operaciones: Facturación";

        public const String ROLES_REP_CONTROL_OPERACIONES_DIARIO = "Control de Operaciones: Control de Operaciones Diario";

        public const String ROLES_REP_OTR = "Control de Operaciones: OTR";

        public const String ROLES_REP_CUADRILLAS = "Control de Operaciones: Cuadrillas";

        public const String ROLES_REP_RUTAS_RECOLECCION = "Control de Operaciones: Rutas de Recolección";

        public const String ROLES_REP_CIERRE_CREDITO_CONTADO = "Control de Operaciones: Cierre Caja Crédito/Contado";

        public const String ROLES_REP_BITACORA_CLIENTE= "Control de Operaciones: Bitácora de Clientes";

        public const String ROLES_REP_BITACORA_CONTRATO = "Control de Operaciones: Bitácora de Contratos";

        public const String ROLES_REPORTES_GERENCIA = "Reportes Gerencia";

        public const String ROLES_ADM_COMBUSTIBLE = "Administrador de Combustible";

        public const String ROLES_ADM_COSTO_HORA = "Administrador de Costo por Hora";

        public const String ROLES_ADM_ANUNCIOS = "Administrador Anuncios";

        public const String ROLES_ADMIN_COSTOS_RUTA = "Administrador Costos Rutas de Recolección";

        public const String ROLES_CONS_COSTOS_RUTA = "Consultor Costos Rutas de Recolección";

        public const String ROLES_CONS_DIAS_FESTIVOS= "Consultor de Días Festivos";

        public const String ROLES_ADMIN_DIAS_FESTIVOS = "Administrador de Días Festivos";

        public const String ROLES_EDIT_DIAS_FESTIVOS = "Editor de Días Festivos";

        public const String ROLES_ADMIN_CARGAR_NOMINA = "Administrador Cargar Nómina";

        public const String ROLES_ADMIN_APROBAR_NOMINA = "Administrador Aprobar Nómina";

        public const String ROLES_ADM_JORNADA = "Administrador de Jornada Laboral";

        public const String ROLES_CONS_JORNADA = "Consultor de Jornada Laboral";

        public const String ROLES_ADM_EMPLEADO = "Administrador de Empleados";

        public const String ROLES_CONS_EMPLEADO = "Consultor de Empleados";
              
        public const String ROL_ = "Super Usuario";

        //public static readonly String[] ListaRoles = { ROL_ADMINISTRADOR, ROL_SUPER_USUARIO };


        public static readonly String[] ListaRoles = { ROL_ADMINISTRADOR, ROL_SUPER_USUARIO, ROLES_ADMIN_CONTRATOS, ROLES_CONS_CONTRATOS, ROLES_EDIT_CONTRATOS, ROLES_ADMIN_CLIENTES, ROLES_EDIT_CLIENTES, 
                                                      ROLES_CONS_CLIENTES, ROLES_ADMIN_COMPANIA, ROLES_CONS_COMPANIA, ROLES_EDIT_COMPANIA, ROLES_ADMIN_PUNTOS_FACTURACION,
                                                      ROLES_CONS_PUNTOS_FACTURACION, ROLES_EDIT_PUNTOS_FACTURACION, ROLES_CONS_CATEGORIA_PRODUCTOS, ROLES_ADMIN_CATEGORIA_PRODUCTOS, 
                                                      ROLES_EDIT_CATEGORIA_PRODUCTOS, ROLES_CONS_PRODUCTOS, ROLES_ADMIN_PRODUCTOS, ROLES_EDIT_PRODUCTOS, ROLES_CONS_SERVICIOS,
                                                      ROLES_ADMIN_SERVICIOS, ROLES_EDIT_SERVICIOS, ROLES_CONS_BOLETAS, ROLES_ADMIN_BOLETAS, ROLES_EDIT_BOLETAS, ROLES_CONS_BOLETAS_MANUALES,
                                                      ROLES_ADMIN_BOLETAS_MANUALES, ROLES_EDIT_BOLETAS_MANUALES, ROLES_CONS_CONTENEDORES, ROLES_ADMIN_CONTENEDORES, ROLES_EDIT_CONTENEDORES,
                                                      ROLES_CONS_EQUIPOS, ROLES_ADMIN_EQUIPOS, ROLES_EDIT_EQUIPOS, ROLES_ADMIN_CONSECUTIVO_BOLETAS, ROLES_CONS_CUADRILLA, 
                                                      ROLES_ADMIN_CUADRILLA, ROLES_EDIT_CUADRILLA, ROLES_CONS_RELLENO_SANITARIO, ROLES_ADMIN_RELLENO_SANITARIO, ROLES_EDIT_RELLENO_SANITARIO,
                                                      ROLES_CONS_RUTA_RECOLECCION, ROLES_ADMIN_RUTA_RECOLECCION, ROLES_EDIT_RUTA_RECOLECCION, ROLES_CONS_OTR, ROLES_ADMIN_OTR, ROLES_EDIT_OTR,
                                                      ROLES_ADMIN_CIERRE_CAJA, ROLES_ADMIN_CIERRE_CAJA_REIMPRESION, ROLES_ADMIN_CONSECUTIVO_FACTURACION, ROLES_ADMIN_CONSULTA_FACTURACION,
                                                      ROLES_ADMIN_FACTURACION, ROLES_ADMIN_PREFACTURACION, ROLES_ADMIN_TIPO_CAMBIO, ROLES_REP_USUARIOS, ROLES_REP_CLIENTES, ROLES_REP_COMPANIAS, 
                                                      ROLES_REP_CONTRATO, ROLES_REP_PRODUCTOS, ROLES_REP_CATEGORIA_PRODUCTOS, ROLES_REP_CIERRE_CAJA, ROLES_REP_CONTENEDOR, ROLES_REP_EQUIPO, 
                                                      ROLES_REP_TONELADAS_BOLETA, ROLES_REP_FACTURACION, ROLES_REP_CONTROL_OPERACIONES_DIARIO, ROLES_REP_OTR, ROLES_REP_CUADRILLAS, 
                                                      ROLES_REP_RUTAS_RECOLECCION,ROLES_REPORTES_GERENCIA, ROLES_ADM_COMBUSTIBLE,ROLES_ADM_COSTO_HORA,ROLES_ADM_ANUNCIOS,
                                                      ROLES_REP_CIERRE_CREDITO_CONTADO, ROLES_ADMIN_COSTOS_RUTA, ROLES_CONS_COSTOS_RUTA, ROLES_CONS_DIAS_FESTIVOS, ROLES_ADMIN_DIAS_FESTIVOS,
                                                      ROLES_EDIT_DIAS_FESTIVOS,ROLES_REV_FACTURACION, ROLES_REP_BITACORA_CLIENTE, ROLES_REP_BITACORA_CONTRATO,ROLES_ADM_JORNADA,ROLES_CONS_JORNADA,
                                                      ROLES_ADM_EMPLEADO, ROLES_CONS_EMPLEADO  };

        public static readonly String[] ListaRoles1 = { ROLES_ADMIN_CONTRATOS, ROLES_CONS_CONTRATOS, ROLES_EDIT_CONTRATOS, ROLES_ADMIN_CLIENTES, ROLES_EDIT_CLIENTES, 
                                                      ROLES_CONS_CLIENTES, ROLES_ADMIN_COMPANIA, ROLES_CONS_COMPANIA, ROLES_EDIT_COMPANIA, ROLES_ADMIN_PUNTOS_FACTURACION,
                                                      ROLES_CONS_PUNTOS_FACTURACION, ROLES_EDIT_PUNTOS_FACTURACION, ROLES_CONS_CATEGORIA_PRODUCTOS, ROLES_ADMIN_CATEGORIA_PRODUCTOS, 
                                                      ROLES_EDIT_CATEGORIA_PRODUCTOS, ROLES_CONS_PRODUCTOS, ROLES_ADMIN_PRODUCTOS, ROLES_EDIT_PRODUCTOS, ROLES_CONS_SERVICIOS,
                                                      ROLES_ADMIN_SERVICIOS, ROLES_EDIT_SERVICIOS, ROLES_CONS_BOLETAS, ROLES_ADMIN_BOLETAS, ROLES_EDIT_BOLETAS, ROLES_CONS_BOLETAS_MANUALES,
                                                      ROLES_ADMIN_BOLETAS_MANUALES, ROLES_EDIT_BOLETAS_MANUALES, ROLES_CONS_CONTENEDORES, ROLES_ADMIN_CONTENEDORES, ROLES_EDIT_CONTENEDORES,
                                                      ROLES_CONS_EQUIPOS, ROLES_ADMIN_EQUIPOS, ROLES_EDIT_EQUIPOS, ROLES_ADMIN_CONSECUTIVO_BOLETAS, ROLES_CONS_CUADRILLA, 
                                                      ROLES_ADMIN_CUADRILLA, ROLES_EDIT_CUADRILLA, ROLES_CONS_RELLENO_SANITARIO, ROLES_ADMIN_RELLENO_SANITARIO, ROLES_EDIT_RELLENO_SANITARIO,
                                                      ROLES_CONS_RUTA_RECOLECCION, ROLES_ADMIN_RUTA_RECOLECCION, ROLES_EDIT_RUTA_RECOLECCION, ROLES_CONS_OTR, ROLES_ADMIN_OTR, ROLES_EDIT_OTR,
                                                      ROLES_ADMIN_CIERRE_CAJA, ROLES_ADMIN_CIERRE_CAJA_REIMPRESION, ROLES_ADMIN_CONSECUTIVO_FACTURACION, ROLES_ADMIN_CONSULTA_FACTURACION,
                                                      ROLES_ADMIN_FACTURACION, ROLES_ADMIN_PREFACTURACION, ROLES_ADMIN_TIPO_CAMBIO, ROLES_REP_USUARIOS, ROLES_REP_CLIENTES, 
                                                      ROLES_REP_COMPANIAS, ROLES_REP_CONTRATO, ROLES_REP_PRODUCTOS, ROLES_REP_CATEGORIA_PRODUCTOS, ROLES_REP_CIERRE_CAJA, ROLES_REP_CONTENEDOR, 
                                                      ROLES_REP_EQUIPO, ROLES_REP_TONELADAS_BOLETA, ROLES_REP_FACTURACION, ROLES_REP_CONTROL_OPERACIONES_DIARIO, ROLES_REP_OTR, 
                                                      ROLES_REP_CUADRILLAS, ROLES_REP_RUTAS_RECOLECCION,ROLES_REPORTES_GERENCIA, ROLES_ADM_COMBUSTIBLE, ROLES_ADM_COSTO_HORA,ROLES_ADM_ANUNCIOS,
                                                      ROLES_REP_CIERRE_CREDITO_CONTADO, ROLES_ADMIN_COSTOS_RUTA, ROLES_CONS_COSTOS_RUTA, ROLES_CONS_DIAS_FESTIVOS, ROLES_ADMIN_DIAS_FESTIVOS,
                                                      ROLES_EDIT_DIAS_FESTIVOS,ROLES_REV_FACTURACION, ROLES_REP_BITACORA_CLIENTE, ROLES_REP_BITACORA_CONTRATO,ROLES_ADM_JORNADA,ROLES_CONS_JORNADA,
                                                      ROLES_ADM_EMPLEADO, ROLES_CONS_EMPLEADO  };

    }
}