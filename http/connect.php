<?php 

function prepare($sql)
{

		return $stmt;
}


function preparedQuery($sql,$params) {
  for ($i=0; $i<count($params); $i++) {
    $sql = preg_replace('/\?/',$params[$i],$sql,1);
  }
  return $sql;
}


function sql($sql, $params_types, $params)
{

	//echo preparedQuery($sql, $params);

	try
	{
		 
		$dbhost   = "localhost";  
		$db       = "inventario";   
		$user     = "root";
		$password = "";   

		$mysqli = new mysqli($dbhost,$user,$password, $db); 

		$mysqli->query("SET NAMES utf8");

		$mysqli->query("SET CHARACTER SET utf8");

		$mysqli->set_charset('utf8');

		/* Prepare statement */
		$stmt = $mysqli->prepare($sql);

		//print_r($mysqli);

		if($stmt === false) {
		  trigger_error('Wrong SQL: ' . $sql . ' Error: ' . $mysqli->errno . ' ' . $mysqli->error, E_USER_ERROR);
		}


		$a_params = array();
		 
		$param_type = '';
		$n = strlen($params_types);

		for($i = 0; $i < $n; $i++) {
		  $param_type .= $params_types[$i];
		}

		/* with call_user_func_array, array params must be passed by reference */
		$a_params[] = & $param_type;
		 
		for($i = 0; $i < $n; $i++) {
		  /* with call_user_func_array, array params must be passed by reference */
		  $a_params[] = & $params[$i];
		}

		/* use call_user_func_array, as $stmt->bind_param('s', $param); does not accept params array */
		call_user_func_array(array($stmt, 'bind_param'), $a_params);

		if(!$stmt)
			print_r($mysqli);
		
		/* Execute statement */
		$stmt->execute();


		if($stmt->errno > 0)
			print_r($stmt);

		if(substr( $sql, 0, 6 ) === "INSERT")
		{
			return Array($stmt->insert_id);
		}
		else if(substr( $sql, 0, 6 ) === "UPDATE")
		{
			$stmt->close();

			return Array(0);
		}
		else if(substr( $sql, 0, 6 ) === "DELETE")
		{
			$stmt->close();

			return Array(0);
		}


		$res = $stmt->get_result();

		$a_data = [];


		while($row = $res->fetch_array(MYSQLI_ASSOC)) {
		  array_push($a_data, $row);
		}

//		$stmt->close();

		return $a_data;
	}
	catch (Exception $e)
	{
		
		echo "Failed to get DB handle: " . $e->getMessage() . "\n";

		exit;
	}
}

?>