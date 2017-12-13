<?php

//phpinfo();

header('Access-Control-Allow-Origin: *');

//error_reporting(E_ALL);

session_start();



if($_REQUEST["sair"] == 1)
{
	unset($_SESSION["id"]);

	return;
}

if(isset($_SESSION["id"]) && $_SESSION["id"] != "")
{
	header('location: ../inventario/cadastrar.php');

	return;
}

include_once ('../controller/connect.php');
$r = sql("SELECT id,nome,loja FROM usuario WHERE usuario = ? AND (senha = sha2(?, 256) or senha=?) ", "sss", Array($_REQUEST["usuario"], $_REQUEST["senha"], $_REQUEST["senha"]));


function Erro()
{
	header('location: index.php#erro');
}

if(!isset($_REQUEST["usuario"]))
	Erro();

if(!isset($_REQUEST["senha"]))
	Erro();


if (count ($r) == 0) {
	Erro ();
	return;
}




	$_SESSION["nome"] = $r[0]["nome"];
	$_SESSION["id"] = $r[0]["id"];
	$_SESSION["loja"] = $r[0]["loja"];

	header('location: ../inventario/cadastrar.php');

	return;


?>