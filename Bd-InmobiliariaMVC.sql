-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 30-09-2025 a las 06:44:33
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliariamvc`
--
CREATE DATABASE IF NOT EXISTS `inmobiliariamvc` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `inmobiliariamvc`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `IdContrato` int(11) NOT NULL,
  `IdInmueble` int(11) NOT NULL,
  `IdInquilino` int(11) NOT NULL,
  `FechaInicio` date NOT NULL,
  `FechaFin` date NOT NULL,
  `PrecioMensual` decimal(18,2) NOT NULL,
  `Estado` tinyint(1) NOT NULL,
  `CreadoPor` int(11) NOT NULL,
  `FechaCreacion` datetime NOT NULL DEFAULT current_timestamp(),
  `AnuladoPor` int(11) DEFAULT NULL,
  `FechaAnulacion` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`IdContrato`, `IdInmueble`, `IdInquilino`, `FechaInicio`, `FechaFin`, `PrecioMensual`, `Estado`, `CreadoPor`, `FechaCreacion`, `AnuladoPor`, `FechaAnulacion`) VALUES
(24, 7, 8, '2025-02-05', '2026-02-05', 1700000.00, 1, 2, '2025-09-26 17:18:59', NULL, NULL),
(25, 10, 9, '2025-05-03', '2027-05-03', 280000.00, 1, 2, '2025-09-26 17:19:31', NULL, NULL),
(26, 8, 12, '2025-10-04', '2025-10-06', 350000.00, 1, 2, '2025-09-30 01:35:54', NULL, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `IdInmueble` int(11) NOT NULL,
  `IdPropietario` int(11) NOT NULL,
  `Direccion` varchar(255) NOT NULL,
  `Uso` varchar(100) DEFAULT NULL,
  `Tipo` varchar(100) DEFAULT NULL,
  `Ambientes` int(11) DEFAULT NULL,
  `Superficie` decimal(10,2) DEFAULT NULL,
  `Precio` decimal(18,2) NOT NULL,
  `Disponible` tinyint(1) NOT NULL DEFAULT 1,
  `FechaCreacion` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`IdInmueble`, `IdPropietario`, `Direccion`, `Uso`, `Tipo`, `Ambientes`, `Superficie`, `Precio`, `Disponible`, `FechaCreacion`) VALUES
(7, 7, 'los caldenes', 'domestico', 'habitacional', 5, NULL, 1500000.00, 1, '2025-09-26 17:16:42'),
(8, 8, 'las grutas', 'eventual', 'Multiespacio', 1, NULL, 600000.00, 1, '2025-09-26 17:17:16'),
(9, 7, 'rio colorado 368', 'habitacional', 'domestico', 2, NULL, 150000.00, 1, '2025-09-26 17:17:46'),
(10, 8, 'edison 1356', 'Vivienda', 'habitacional', 3, NULL, 150000.00, 1, '2025-09-26 17:18:29');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `IdInquilino` int(11) NOT NULL,
  `Nombre` longtext NOT NULL,
  `Apellido` longtext NOT NULL,
  `Dni` longtext NOT NULL,
  `Telefono` longtext NOT NULL,
  `Email` longtext NOT NULL,
  `Estado` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`IdInquilino`, `Nombre`, `Apellido`, `Dni`, `Telefono`, `Email`, `Estado`) VALUES
(8, 'marcos', 'giraudi', '42562212', '2665034499', 'giraudi@gmail.com', 1),
(9, 'samuel', 'alvarez', '41851232', '2657345631', 'alvarez@gmail.com', 1),
(10, 'leny', 'tavarez', '42683565', '266543123', 'leny@gmail.com', 1),
(11, 'Laura', 'Fernandez', '32458671', '114558234', 'laura.fernandez@gmail.com', 1),
(12, 'Martín', 'Pereyra', '28745790', '161249834', 'martin.pereyra@hotmail.com', 1),
(13, 'Julieta', 'Gonzales', '402345923', '2657889942', 'julietaGonzalez@gmail.com', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `IdPago` int(11) NOT NULL,
  `IdContrato` int(11) NOT NULL,
  `FechaPago` date NOT NULL,
  `Monto` decimal(18,2) NOT NULL,
  `Observaciones` varchar(200) DEFAULT NULL,
  `Estado` tinyint(4) NOT NULL DEFAULT 1,
  `CreadoPor` int(11) NOT NULL,
  `FechaCreacion` datetime NOT NULL DEFAULT current_timestamp(),
  `AnuladoPor` int(11) DEFAULT NULL,
  `FechaAnulacion` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`IdPago`, `IdContrato`, `FechaPago`, `Monto`, `Observaciones`, `Estado`, `CreadoPor`, `FechaCreacion`, `AnuladoPor`, `FechaAnulacion`) VALUES
(7, 24, '2025-09-26', 1500000.00, 'falta pagar $200.000', 1, 2, '2025-09-26 17:20:07', NULL, NULL),
(8, 25, '2025-09-26', 280000.00, 'pago en tiempo y forma', 1, 2, '2025-09-26 17:20:46', NULL, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `IdPropietario` int(11) NOT NULL,
  `Nombre` longtext NOT NULL,
  `Apellido` longtext NOT NULL,
  `Dni` longtext NOT NULL,
  `Telefono` longtext NOT NULL,
  `Email` longtext NOT NULL,
  `Estado` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`IdPropietario`, `Nombre`, `Apellido`, `Dni`, `Telefono`, `Email`, `Estado`) VALUES
(7, 'javier', 'cantaruti', '19485374', '2657494565', 'cantarutis@gmail.com', 1),
(8, 'marcos', 'Di palma', '31984343', '2657654321', 'marquitos@gmail.com', 1),
(9, 'Camila', 'Rodriguez', '42356331', '2665438552', 'camilarodriguez@live.com', 1),
(10, 'juana', 'Diarco', '4568921', '2668294752', 'diarco@gmail.com', 1),
(11, 'Ramiro', 'Altamira', '42567545', '2665403495', 'ramialtamira@gmail.com', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `IdUsuario` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `Email` varchar(150) NOT NULL,
  `ClaveHash` varchar(200) NOT NULL,
  `Rol` varchar(20) NOT NULL,
  `Avatar` varchar(250) DEFAULT NULL,
  `Estado` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`IdUsuario`, `Nombre`, `Apellido`, `Email`, `ClaveHash`, `Rol`, `Avatar`, `Estado`) VALUES
(2, 'marcos', 'administer', 'admin@gmail.com', '$2a$11$mwC9EKTYXlgotF.sEmjkR.Rc6PoHk4R7AzuFNaQzw5QyQkorNqly2', 'Administrador', '/img/9630f649-b8dc-4903-8899-c5a363063d7d.png', 1),
(3, 'Berna', 'braw', 'empleado@gmail.com', '$2a$11$cyJ5leyNktZ7ucXWceYSp.C45fbuskdVgiC8Mji7K9KZokyRfnWxq', 'Empleado', NULL, 1);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`IdContrato`),
  ADD KEY `IdInmueble` (`IdInmueble`),
  ADD KEY `IdInquilino` (`IdInquilino`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`IdInmueble`),
  ADD KEY `IdPropietario` (`IdPropietario`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`IdInquilino`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`IdPago`),
  ADD KEY `IdContrato` (`IdContrato`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`IdPropietario`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`IdUsuario`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `IdContrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=27;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `IdInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `IdInquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `IdPago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `IdPropietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `IdUsuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `contratos_ibfk_1` FOREIGN KEY (`IdInmueble`) REFERENCES `inmuebles` (`IdInmueble`),
  ADD CONSTRAINT `contratos_ibfk_2` FOREIGN KEY (`IdInquilino`) REFERENCES `inquilinos` (`IdInquilino`);

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `inmuebles_ibfk_1` FOREIGN KEY (`IdPropietario`) REFERENCES `propietarios` (`IdPropietario`);

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `pagos_ibfk_1` FOREIGN KEY (`IdContrato`) REFERENCES `contratos` (`IdContrato`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
