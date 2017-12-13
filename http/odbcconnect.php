<?php 

ini_set('max_execution_time', 300000);


function mssql_real_escape_string($data) {
        if ( !isset($data) or empty($data) ) return '';
        if ( is_numeric($data) ) return $data;

        $non_displayables = array(
                '/%0[0-8bcef]/',            // url encoded 00-08, 11, 12, 14, 15
                '/%1[0-9a-f]/',             // url encoded 16-31
                '/[\x00-\x08]/',            // 00-08
                '/\x0b/',                   // 11
                '/\x0c/',                   // 12
                '/[\x0e-\x1f]/'             // 14-31
        );
        foreach ( $non_displayables as $regex )
                $data = preg_replace( $regex, '', $data );
        $data = str_replace("'", "''", $data );
        return $data;
}

function sql($sql, $params, $dbh = null)
{
	try
	{
		$hostname = ".\\SQLEXPRESS";
		$port = 1433;
		$dbname = "Sis";
		$username = "sis";
		$pw = 'sis';

		if($dbh == null)
		{
			$dbh = odbc_connect("Driver={ODBC Driver 13 for SQL Server};Server=$hostname;Database=$dbname;",$username,$pw);

//			$dbh->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
		}

		$stmt = odbc_prepare ($dbh, $sql);
		$res = odbc_execute ($stmt, $params);
		if(substr( $sql, 0, 6 ) === "INSERT")
		{
			return mssql("SELECT @@IDENTITY;", array(), $dbh);
		}
		else if(substr( $sql, 0, 6 ) === "UPDATE")
		{
			return mssql("SELECT @@IDENTITY;", array(), $dbh);
		}

		$result = [];


		//return $stmt->fetchAll();
		while($r= odbc_fetch_array($stmt)){
			$result[]= $r;
		}


		return $result;

	}
	catch (Exception $e)
	{
		echo gethostname();
		
		echo "Failed to get DB handle: " . $e->getMessage() . "\n";

		exit;
	}
}

?>