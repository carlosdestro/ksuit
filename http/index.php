<?php 
error_reporting(E_ERROR);

include_once ("model.php");

$op = $_GET["op"];

$table = $_GET["table"];

exec1($op, $table, $guid, $_POST);	

?>