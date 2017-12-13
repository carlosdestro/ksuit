<?php 
include_once ("model.php");
error_reporting(E_ERROR);

$op = $_GET["op"];

$table = $_GET["table"];

exec1($op, $table, array());	

?>