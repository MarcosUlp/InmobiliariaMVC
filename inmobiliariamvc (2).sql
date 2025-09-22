-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 22-09-2025 a las 20:55:29
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
  `Estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`IdContrato`, `IdInmueble`, `IdInquilino`, `FechaInicio`, `FechaFin`, `PrecioMensual`, `Estado`) VALUES
(1, 2, 2, '2025-02-05', '2025-05-05', 0.00, 1),
(2, 1, 1, '2025-10-02', '2025-10-06', 350000.00, 1),
(3, 1, 1, '0001-01-01', '0001-01-01', 0.00, 0),
(4, 2, 3, '2025-02-06', '2026-06-02', 0.00, 0),
(5, 3, 4, '2025-09-01', '2025-10-01', 1500000.00, 1),
(6, 1, 4, '2025-09-17', '2025-10-01', 3456789.00, 1),
(7, 1, 2, '2025-11-11', '2025-11-12', 123123.00, 1);

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
(1, 2, 'mitre 1235', 'Vivienda', 'casa', 2, NULL, 250000.00, 0, '2025-09-18 19:53:15'),
(2, 1, 'los caldenes', 'vivienda', 'casa', 5, NULL, 1500000.00, 1, '2025-09-18 19:54:20'),
(3, 2, 'centro vm', 'domestico ', 'vivienda', 3, NULL, 150000.00, 1, '2025-09-21 19:39:45');

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
  `Email` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`IdInquilino`, `Nombre`, `Apellido`, `Dni`, `Telefono`, `Email`) VALUES
(1, 'marcos', 'zapata', '41851232', '26657523562', 'zapata@gmail.com'),
(2, 'emanuel', 'rodriguez', '1234521', '265724964', 'emanuelrodrigof@gmail.com'),
(3, 'marquinho', 'Da Silva', '42562212', '266543567', 'marquinho@gmail.com'),
(4, 'samuel', 'chimarrao', '42596921', '2665034499', 'tomochimarrao@gmail.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `IdPago` int(11) NOT NULL,
  `IdContrato` int(11) NOT NULL,
  `FechaPago` date NOT NULL,
  `Monto` decimal(18,2) NOT NULL,
  `Observaciones` varchar(200) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

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
  `Clave` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`IdPropietario`, `Nombre`, `Apellido`, `Dni`, `Telefono`, `Email`, `Clave`) VALUES
(1, 'marcos', 'crinó', '42562212', '2665034499', 'crino@gmail.com', '1234'),
(2, 'Zara', 'Ruiz', '42265435', '26657523562', 'zaracho@gmail.com', '123'),
(3, 'marcos', 'giraudi', '42562212', '266592394', 'giraudi@gmail.com', '1234'),
(4, 'samuel', 'cantaruti', '2894789 ', '2657345631', 'cantarutisContruccion@gmail.com', '123');

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
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `IdContrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `IdInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `IdInquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `IdPago` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `IdPropietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

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
