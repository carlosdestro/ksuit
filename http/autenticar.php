<?php


error_reporting(E_ERROR);

session_start();

if(!isset($_SESSION["id"]) || $_SESSION["id"] == "")
{
	die("Sua sessão expirou");

}


?>