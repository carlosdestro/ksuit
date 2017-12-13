<?php

include_once ("odbcconnect.php");

function exec1($operation, $table, $params = null)
{	

	if($operation == "LOGIN")
	{
		login ();

	}

	//include_once ("autenticar.php");

	if($operation == "SELECT")
	{
		$sql = "SELECT * FROM $table";

		$r = sql($sql, $params);

		die(json_encode($r));
	}
	else if($operation == "SCHEMA")
	{
		die(json_encode(form($table)));
	}
}

function form($formName)
{
	$form = sql("SELECT * FROM formulario WHERE nome = ? ORDER BY ISNULL(formulario,'')", array($formName));	

	$fields = sql("SELECT * FROM campo WHERE formulario = ? ORDER BY ID", array($form[0]["nome"]));

	$form[0]["fields"] = $fields;

	$subForms = sql("SELECT * FROM formulario WHERE formulario = ? ORDER BY ID", array($form[0]["nome"]));

	for ($i=0;$i<count($subForms);$i++)
	{
		 $form[0]["subs"] = form($subForms[$i]["nome"]);
	}

	//$form[0]["subs"] = $subForms;

	return $form;	
}

	function login()
	{

		session_start();



		if($_REQUEST["sair"] == 1)
		{
			unset($_SESSION["id"]);

			return;
		}

		
		$r = sql("SELECT a.id,cpf,a.empresa,b.nome nomeEmpresa,banco,b.usuario bancoUsuario,b.senha bancoSenha
			FROM usuario a
			JOIN empresa b ON a.empresa= b.id WHERE cpf = ? AND (a.senha=?) ",
			Array($_REQUEST["cpf"], $_REQUEST["senha"]));

		if (count ($r) == 0) {
			die ("senha errada");
			return;
		}

			$_SESSION["id"] = $r[0]["id"];
			$_SESSION["cpf"] = $r[0]["cpf"];
			$_SESSION["empresa"] = $r[0]["a.empresa"];
			$_SESSION["nome"] = $r[0]["nome"];
			$_SESSION["nomeEmpresa"] = $r[0]["nomeEmpresa"];
			$_SESSION["banco"] = $r[0]["banco"];
			$_SESSION["usuario"] = $r[0]["usuario"];
			$_SESSION["bancoUsuario"] = $r[0]["bancoUsuario"];
			$_SESSION["senha"] = $r[0]["senha"];
			$_SESSION["bancoSenha"] = $r[0]["bancoSenha"];

			die (json_encode($r));


	}


?>