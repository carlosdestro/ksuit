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

function mssql($sql, $params, $dbh = null)
{
	try
	{
		$hostname = ".\\SQLEXPRESS";
		$port = 1433;
		$dbname = "SIS";
		$username = "sis";
		$pw = 'sis';

		if($dbh == null)
		{
			if(false)//gethostname() != "ip-10-100-1-27" && gethostname() != "sistemas")
				//linux
				$dbh = new PDO ("sqlsrv:Server=$hostname;Database=$dbname","$username","$pw", array(PDO::ATTR_TIMEOUT => "90"));
			else
				//windows
				$dbh = new PDO ("dblib:host=$hostname:$port;dbname=$dbname","$username","$pw");

			$dbh->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
		}

		$stmt = $dbh->prepare($sql);

		$stmt->execute($params);

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

		while ($row = $stmt->fetch())
		{
			//$desc = iconv("ISO-8859-1", "UTF-8", $row["descricao_completa"]);

			$result[] = $row;


			//die(json_encode(array($cod, $desc), JSON_UNESCAPED_UNICODE));	
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