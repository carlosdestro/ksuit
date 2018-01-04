<?php

include_once ("odbcconnect.php");

function exec1($operation, $objectName, $guid, $params = null)
{	
	if($operation == "LOGIN")
	{
		login ();

	}

	if($operation == "SCHEMA")
	{
		die(json_encode(form($objectName, $guid)));
	}

	$operations = sql("SELECT * FROM operation WHERE id = ?", array($operation));	

	$dbh = connect();




	$form = form ($objectName, $guid);

	for ($i=0;$i<count($operations);$i++)
	{
		sql($form[0]["queryForm"], array(), $dbh);
		sql($form[0]["queryInsert"], $params, $dbh);
		$rr = sql( $operations[$i]["tsql"], array(), $dbh);
		$form[0]["values"] = $rr[0];
/*
		$rr = sql(
			implode("; ", array("SET NOCOUNT ON",
				$form[0]["queryFormulario"], 
				$form[0]["queryInsert"], 
				$operacoes[$i]["tsql"]
			))
			 , $params, $dbh);
*/
		die(json_encode($form));
	}


	//include_once ("autenticar.php");

	if($operation == "SELECT")
	{
		$sql = "SELECT * FROM $objectName";

		$r = sql($sql, $params);

		die(json_encode($r));
	}
}

function form($formName, $guid)
{

	$form = sql("SELECT * FROM form WHERE name = ? ORDER BY ISNULL(form,'')", array($formName));	

	$fields = sql("SELECT a.*, b.name dataTypeName, b.tsql FROM field a JOIN dataType b on a.dataType = b.name WHERE form = ? ORDER BY position", array($form[0]["name"]));

	$queryForm = "CREATE TABLE #$formName (";

	for ($i=0;$i<count($fields);$i++)
		$tsqlFields[] = $fields[$i]["name"]." ".$fields[$i]["tsql"]." ".($fields[$i]["required"]==1?"NOT ":'')."NULL";

	$queryForm.=implode(", ", $tsqlFields).")";

	$form[0]["queryForm"] = $queryForm;

	$queryInsert ="SET IDENTITY_INSERT #$formName ON; INSERT INTO #$formName (";

	$tsqlFields = array();

	for ($i=0;$i<count($fields);$i++)
	{
		$tsqlFields[] = $fields[$i]["name"];
		$tsqlParams[] = "?";
	}

	$queryInsert.=implode(", ", $tsqlFields).") VALUES (". implode(", ", $tsqlParams).")";

	$form[0]["queryInsert"] = $queryInsert;

	$form[0]["fields"] = $fields;

	$operations = sql("SELECT * FROM operation WHERE (objectName = ? AND objectType='FORM') ORDER BY POSITION", array($form[0]["name"]));

	$form[0]["operations"] = $operations;


	$subForms = sql("SELECT * FROM form WHERE form = ? ORDER BY POSITION", array($form[0]["name"]));

	for ($i=0;$i<count($subForms);$i++)
	{
		 $form[0]["subs"] = form($subForms[$i]["name"]);
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

		
		$r = sql("SELECT a.id,cpf,a.instancia,b.name nomeEmpresa,banco,b.usuario bancoUsuario,b.senha bancoSenha
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